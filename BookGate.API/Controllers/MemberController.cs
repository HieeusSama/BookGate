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
        public async Task<IActionResult> Index()
        {
            var books = await _service.GetAll();
            return View(books);
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
