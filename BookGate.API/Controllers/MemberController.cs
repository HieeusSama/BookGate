using Azure;
using BookGate.Application.DTOs;
using BookGate.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookGate.API.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberBookService _service;
        private readonly CartItemService _cartItemService;
        private readonly GeminiService _geminiService;
        public MemberController(MemberBookService service, CartItemService cartitemservice, GeminiService geminiService)
        {
            _service = service;
            _cartItemService = cartitemservice;
            _geminiService = geminiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchString, int page = 1)
        {
            var allBooks = await _service.GetAll();

            if (!string.IsNullOrEmpty(searchString))
            {
                string search = searchString.ToLower().Trim();

                allBooks = allBooks.Where(b =>
                    (b.Title != null && b.Title.ToLower().Contains(search)) ||
                    (b.Author != null && b.Author.ToLower().Contains(search))
                ).ToList();
            }

            int pageSize = 12;
            int totalItems = allBooks.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            page = page < 1 ? 1 : page;
            page = page > totalPages && totalPages > 0 ? totalPages : page;
            var pagedBooks = allBooks.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.SearchString = searchString;

            return View(pagedBooks);
        }

        [HttpGet]
        public async Task<IActionResult> CreateCartItem(string bookId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (int.TryParse(userIdString, out int userId))
            {
                CartItemDTO cartItem = new CartItemDTO
                {
                    BookId = bookId,
                    Id = userId, 
                    Quantity = 1
                };

                await _cartItemService.Add(cartItem);
            }
            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string bookId)
        {
            var book = await _service.GetById(bookId);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> AskAI([FromBody] string message)
        {
            if (string.IsNullOrEmpty(message)) return BadRequest("Tin nhắn trống");

            try
            {
                var allBooks = await _service.GetAll();

                var availableBooks = allBooks.Take(50).Select(b => $"- Tên sách: '{b.Title}', Tác giả: {b.Author}, Giá: {b.SellingPrice:N0} VND").ToList();

                string bookContextString = string.Join("\n", availableBooks);

                if (string.IsNullOrEmpty(bookContextString))
                {
                    bookContextString = "Hiện tại cửa hàng đang cập nhật sách, tạm thời chưa có dữ liệu.";
                }

                string aiResponse = await _geminiService.ChatWithAI(message, bookContextString);

                return Json(new { reply = aiResponse });
            }
            catch (Exception ex)
            {
                return Json(new { reply = "Hệ thống đang bận, vui lòng thử lại sau: " + ex.Message });
            }
        }
    }
}
