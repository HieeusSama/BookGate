using BookGate.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookGate.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly OrderService _orderService;
        private readonly BookService _bookService;

        public AdminController(OrderService orderService, BookService bookService)
        {
            _orderService = orderService;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderService.GetAll();
            var allBooks = await _bookService.GetAll();

            ViewBag.PendingOrders = allOrders.Count(o => o.StatusId == "PENDING");
            ViewBag.AwaitingShipment = allOrders.Count(o => o.StatusId == "AWAITING_SHIPMENT");
            ViewBag.ShippingOrders = allOrders.Count(o => o.StatusId == "SHIPPING");

            ViewBag.TotalBooks = allBooks.Count();

            ViewBag.TotalBooks = allBooks.Count();

            var top10Books = allBooks.Take(10).ToList();

            return View(top10Books);
        }
    }
}
