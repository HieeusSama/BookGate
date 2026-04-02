using Azure;
using BookGate.Application.DTOs;
using BookGate.Application.Services;
using BookGate.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookGate.API.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberBookService _service;
        private readonly CartItemService _cartItemService;
        public MemberController(MemberBookService service, CartItemService cartitemservice)
        {
            _service = service;
            _cartItemService = cartitemservice;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            //var books = await _service.GetAll();
            //return View(books);
            var allBooks = await _service.GetAll();

            // 1. Xử lý tìm kiếm (Tìm gần đúng, không phân biệt hoa thường)
            if (!string.IsNullOrEmpty(searchString))
            {
                allBooks = allBooks.Where(b => b.Title.ToLower().Contains(searchString.ToLower())).ToList();
            }

            // 2. Xử lý phân trang
            int pageSize = 12; // Số lượng sách hiển thị trên 1 trang (bạn có thể tự điều chỉnh)
            int totalItems = allBooks.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Đảm bảo số trang luôn hợp lệ
            page = page < 1 ? 1 : page;
            page = page > totalPages && totalPages > 0 ? totalPages : page;

            // Lấy dữ liệu của trang hiện tại
            var pagedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // 3. Truyền dữ liệu phân trang và từ khóa tìm kiếm sang View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;

            return View(pagedBooks);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCartItem(string bookId)
        {
            var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(Id))
            {
                return RedirectToAction("Login", "Auth");
            }
            int userId = int.Parse(Id);
            CartItemDTO cartItem = new CartItemDTO
            {
                BookId = bookId,
                Id = userId,
                Quantity = 1,
            };
            await _cartItemService.Add(cartItem);
            return RedirectToAction("Index");
        }
    }
}
