using BookGate.Application.DTOs;
using BookGate.Application.Services;
using BookGate.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookGate.API.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartItemService _cartIteamService;
        private readonly OrderService _service;
        private readonly OrderDetailService _orderDetailService;
        private readonly MemberBookService _memberBookService;

        public OrderController(OrderService service, OrderDetailService orderDetailService, CartItemService cartIteamService, MemberBookService memberBookService)
        {
            _service = service;
            _orderDetailService = orderDetailService;
            _cartIteamService = cartIteamService;
            _memberBookService = memberBookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string bookId, int quantity = 1)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (bookId != null)
            {
                var book = await _memberBookService.GetById(bookId);
                decimal subTotal = book.SellingPrice * quantity;
                decimal shippingFee = 32000;
                decimal total = subTotal + shippingFee;
                ViewBag.TotalAmount = total;
                ViewBag.BookId = bookId;
                ViewBag.Quantity = quantity;
                return View();
            }
            else
            {
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
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderDTO order, string bookId, int quantity = 1)
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(Id);

            decimal shippingFee = 32000;
            var newOrderId = Guid.NewGuid().ToString();

            // 1. Gán các thông tin chung cho đơn hàng (Giữ nguyên của bạn)
            order.OrderId = newOrderId;
            order.Id = userId;
            order.OrderDate = DateTime.Now;
            order.ShippingFee = shippingFee;
            order.StatusId = "PENDING";

            // 2. TÁCH LUỒNG XỬ LÝ
            if (!string.IsNullOrEmpty(bookId))
            {
                // --- LUỒNG MUA NGAY ---
                var book = await _memberBookService.GetById(bookId);

                decimal subTotal = book.SellingPrice * quantity;
                order.TotalAmount = subTotal + shippingFee;

                await _service.Add(order);

                // Chỉ tạo 1 OrderDetail cho cuốn sách vừa mua
                OrderDetailDTO orderDetails = new OrderDetailDTO
                {
                    OrderDetailId = Guid.NewGuid().ToString(),
                    OrderId = newOrderId,
                    BookId = book.BookId,
                    Quantity = quantity,
                    UnitPrice = book.SellingPrice
                };
                await _orderDetailService.Add(orderDetails);
            }
            else
            {
                // --- LUỒNG GIỎ HÀNG (Giữ nguyên 100% code cũ của bạn) ---
                var cartItems = await _cartIteamService.GetCartItemById(userId);

                decimal subTotal = cartItems.Sum(x => x.SellingPrice * x.Quantity);
                order.TotalAmount = subTotal + shippingFee;

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
            }

            return RedirectToAction("Index", "Member");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var order = await _orderDetailService.GetOrderDetailById(id);
            return View(order);
        }
    }
}
