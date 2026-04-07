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
        private readonly MemberBookService _memberBookService;


        public OrderStatusController(OrderService service, OrderDetailService orderDetailService, MemberBookService memberBookService)
        {
            _service = service;
            _orderDetailService = orderDetailService;
            _memberBookService = memberBookService;
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
                orders = await _service.GetOrdersWithFilter(null, searchId, status);
            }

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
                orders = await _service.GetAllOrderById(userId);
            }
            else
            {
                orders = await _service.GetOrdersWithFilter(userId, searchId, status);
            }

            // 4. Giữ lại giá trị để hiển thị lên Form lọc (Search Box & Select)
            ViewBag.SearchId = searchId;
            ViewBag.CurrentStatus = status;

            return View(orders);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string orderId, string newStatus)
        {
            var order = await _service.GetById(orderId);
            if (order != null)
            {
                order.StatusId = newStatus;
                await _service.Update(order);
                if (newStatus == "CANCELED")
                {
                    var orderDetails = await _orderDetailService.GetOrderDetailById(orderId);

                    if (orderDetails != null)
                    {
                        foreach (var item in orderDetails)
                        {
                            // Tìm cuốn sách trong kho dựa vào BookId
                            var book = await _memberBookService.GetById(item.BookId);
                            if (book != null)
                            {
                                // Cộng lại số lượng và cập nhật vào Database
                                book.Quantity += item.Quantity;
                                await _memberBookService.Update(book);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("IndexAdmin");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatusMember(string orderId, string newStatus)
        {
            var order = await _service.GetById(orderId);
            if (order != null)
            {
                order.StatusId = newStatus;
                await _service.Update(order);
                if (newStatus == "CANCELED")
                {
                    var orderDetails = await _orderDetailService.GetOrderDetailById(orderId);

                    if (orderDetails != null)
                    {
                        foreach (var item in orderDetails)
                        {
                            // Tìm cuốn sách trong kho dựa vào BookId
                            var book = await _memberBookService.GetById(item.BookId);
                            if (book != null)
                            {
                                // Cộng lại số lượng và cập nhật vào Database
                                book.Quantity += item.Quantity;
                                await _memberBookService.Update(book);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("IndexMember");
        }
    }
}
