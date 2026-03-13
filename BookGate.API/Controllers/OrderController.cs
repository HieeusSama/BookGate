using BookGate.Application.DTOs;
using BookGate.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookGate.API.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartItemService _cartIteamService;
        private readonly OrderService _service;
        private readonly OrderDetailService _orderDetailService;


        public OrderController(OrderService service, OrderDetailService orderDetailService, CartItemService cartIteamService)
        {
            _service = service;
            _orderDetailService = orderDetailService;
            _cartIteamService = cartIteamService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 1. Lấy giỏ hàng của user từ DB
            var cartItems = await _cartIteamService.GetCartItemById(userId);
            if (cartItems == null || !cartItems.Any())
            {
                return RedirectToAction("Index", "CartItem");
            }

            decimal subTotal = cartItems.Sum(x => x.SellingPrice * x.Quantity);
            decimal shippingFee = 32000;
            decimal total = subTotal + shippingFee;
            ViewBag.TotalAmount = total;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderDTO order)
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(Id);
            var cartItems = await _cartIteamService.GetCartItemById(userId);
            decimal subTotal = cartItems.Sum(x => x.SellingPrice * x.Quantity);
            decimal shippingFee = 32000;
            decimal total = subTotal + shippingFee;
            var newOrderId = Guid.NewGuid().ToString();

            order.OrderId = newOrderId;
            order.Id = userId;
            order.TotalAmount = total;
            order.OrderDate = DateTime.Now;
            order.ShippingFee = shippingFee;
            order.StatusId = "PENDING";
            
            await _service.Add(order);
            foreach (var item in cartItems)
            {
                OrderDetailDTO orderDetails = new OrderDetailDTO
                {
                    OrderDetailId = Guid.NewGuid().ToString(),
                    OrderId = newOrderId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = item.SellingPrice
                };
                await _orderDetailService.Add(orderDetails);
            }
            
            foreach (var item in cartItems)
            {
                await _cartIteamService.Delete(item.CartItemId);
            }

            return RedirectToAction("Index", "Member");
        }
    }
}
