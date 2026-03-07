using BookGate.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookGate.API.Controllers
{
    public class MemberController : Controller
    {
        private readonly MemberBookService _service;
        public MemberController(MemberBookService service, PublisherService publisherService)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var books = await _service.GetAll();
            return View(books);
        }
    }
}
