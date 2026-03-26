using BookGate.Application.DTOs;
using BookGate.Application.Services;
using BookGate.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Security.Claims;

namespace BookGate.API.Controllers
{
    public class CartItemController : Controller
    {
        private readonly CartItemService _service;
        private readonly BookService _bookService;
        private readonly OrderService _orderService;
        private readonly OrderDetailService _orderDetailService;
        public CartItemController(CartItemService service, BookService bookService)
        {
            _service = service;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(Id);
            var cartitems = await _service.GetCartItemById(userId);
            return View(cartitems);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.Delete(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> OrderIndex()
        {
            return RedirectToAction("Index", "Order");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(string id, int quantity)
        {
            try
            {
                if (quantity < 1)
                {
                    return Json(new { success = false, message = "Số lượng không hợp lệ" });
                }

                await _service.UpdateQuantityCartItem(id, quantity);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Trả về JSON chứa thông báo lỗi thay vì quăng lỗi màn hình vàng
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
