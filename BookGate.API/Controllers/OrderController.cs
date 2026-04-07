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
            try
            {
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(Id))
                {
                    return RedirectToAction("Login", "Auth");
                }
                int userId = int.Parse(Id);

                decimal shippingFee = 32000;
                var newOrderId = Guid.NewGuid().ToString();

                order.OrderId = newOrderId;
                order.Id = userId;
                order.OrderDate = DateTime.Now;
                order.ShippingFee = shippingFee;
                order.StatusId = "PENDING"; // CHÚ Ý: Cột này rất hay gây lỗi khóa ngoại

                if (!string.IsNullOrEmpty(bookId))
                {
                    // --- LUỒNG MUA NGAY ---
                    var book = await _memberBookService.GetById(bookId);

                    if (book.Quantity < quantity)
                    {
                        TempData["Error"] = $"Sách '{book.Title}' chỉ còn {book.Quantity} quyển.";
                        return RedirectToAction("Index");
                    }

                    decimal subTotal = book.SellingPrice * quantity;
                    order.TotalAmount = subTotal + shippingFee;

                    await _service.Add(order);

                    OrderDetailDTO orderDetails = new OrderDetailDTO
                    {
                        OrderDetailId = Guid.NewGuid().ToString(),
                        OrderId = newOrderId,
                        BookId = book.BookId,
                        Quantity = quantity,
                        UnitPrice = book.SellingPrice
                    };
                    await _orderDetailService.Add(orderDetails);

                    // Trừ kho
                    book.Quantity -= quantity;
                    await _memberBookService.Update(book);
                }
                else
                {
                    // --- LUỒNG GIỎ HÀNG ---
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

                        // Trừ kho
                        var bookInCart = await _memberBookService.GetById(item.BookId);
                        bookInCart.Quantity -= item.Quantity;
                        await _memberBookService.Update(bookInCart);
                    }

                    foreach (var item in cartItems)
                    {
                        await _cartIteamService.Delete(item.CartItemId);
                    }
                }

                return RedirectToAction("Index", "Member");
            }
            catch (Exception ex)
            {
                string errorMessage = "Bắt được lỗi: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Lỗi chi tiết (Database): " + ex.InnerException.Message;
                }
                return Content(errorMessage);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var order = await _orderDetailService.GetOrderDetailById(id);
            return View(order);
        }
    }
}
