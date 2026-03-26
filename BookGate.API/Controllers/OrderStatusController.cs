using BookGate.Application.DTOs;
using BookGate.Application.Services;
using BookGate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BookGate.API.Controllers
{
    public class OrderStatusController : Controller
    {
        private readonly OrderService _service;
        private readonly OrderDetailService _orderDetailService;


        public OrderStatusController(OrderService service, OrderDetailService orderDetailService)
        {
            _service = service;
            _orderDetailService = orderDetailService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> IndexAdmin(string? searchId, string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }
            IEnumerable<OrderDTO> orders;

            if (string.IsNullOrEmpty(searchId) && string.IsNullOrEmpty(status))
            {
                orders = await _service.GetAll();
            }
            else
            {
                orders = await _service.GetOrdersWithFilter(searchId, status);
            }

            // 4. Giữ lại giá trị để hiển thị lên Form lọc (Search Box & Select)
            ViewBag.SearchId = searchId;
            ViewBag.CurrentStatus = status;

            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> IndexMember(string? searchId, string? status)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }
            IEnumerable<OrderDTO> orders;

            if (string.IsNullOrEmpty(searchId) && string.IsNullOrEmpty(status))
            {
                orders = await _service.GetAll();
            }
            else
            {
                orders = await _service.GetOrdersWithFilter(searchId, status);
            }

            // 4. Giữ lại giá trị để hiển thị lên Form lọc (Search Box & Select)
            ViewBag.SearchId = searchId;
            ViewBag.CurrentStatus = status;

            return View(orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost] // Phải có attribute này vì form gửi lên bằng method="post"
        public async Task<IActionResult> UpdateStatus(string orderId, string newStatus)
        {
            var order = await _service.GetById(orderId);
            if (order != null)
            {
                order.StatusId = newStatus;
                await _service.Update(order);
            }
            return RedirectToAction("IndexAdmin");
        }

        [HttpPost] // Phải có attribute này vì form gửi lên bằng method="post"
        public async Task<IActionResult> UpdateStatusMember(string orderId, string newStatus)
        {
            var order = await _service.GetById(orderId);
            if (order != null)
            {
                order.StatusId = newStatus;
                await _service.Update(order);
            }
            return RedirectToAction("IndexAdmin");
        }

    }
}
