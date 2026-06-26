using BookGate.Application.DTOs;
using BookGate.Application.Services;
using BookGate.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookGate.API.Controllers
{
    public class OrderController : Controller
    {
        private readonly CartItemService _cartIteamService;
        private readonly OrderService _service;
        private readonly OrderDetailService _orderDetailService;
        private readonly MemberBookService _memberBookService;
        private readonly IConfiguration _configuration;
        private readonly GhnService _ghnService;
        public OrderController(OrderService service, OrderDetailService orderDetailService, CartItemService cartIteamService, MemberBookService memberBookService, IConfiguration configuration, GhnService ghnService)
        {
            _service = service;
            _orderDetailService = orderDetailService;
            _cartIteamService = cartIteamService;
            _memberBookService = memberBookService;
            _configuration = configuration;
            _ghnService = ghnService;
            _configuration = configuration;
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
                ViewBag.TotalAmount = subTotal;
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

                ViewBag.TotalAmount = subTotal;

                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(OrderDTO order, string bookId, int quantity = 1, string PaymentMethod = "COD")
        {
            try
            {
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(Id)) return RedirectToAction("Login", "Auth");

                int userId = int.Parse(Id);
                var newOrderId = Guid.NewGuid().ToString();

                order.OrderId = newOrderId;
                order.Id = userId;
                order.OrderDate = DateTime.UtcNow.AddHours(7);
                order.StatusId = "PENDING";
                order.PaymentMethod = PaymentMethod;

                // --- LUỒNG MUA NGAY ---
                if (!string.IsNullOrEmpty(bookId))
                {
                    var book = await _memberBookService.GetById(bookId);
                    if (book.Quantity < quantity)
                    {
                        TempData["Error"] = $"Sách '{book.Title}' chỉ còn {book.Quantity} quyển.";
                        return RedirectToAction("Index");
                    }

                    decimal subTotal = book.SellingPrice * quantity;

                    order.ShippingFee = order.TotalAmount - subTotal;

                    if (PaymentMethod == "VNPAY")
                    {
                        HttpContext.Session.SetString("TempOrder", JsonSerializer.Serialize(order));
                        HttpContext.Session.SetString("TempBookId", bookId);
                        HttpContext.Session.SetInt32("TempQuantity", quantity);
                        HttpContext.Session.SetString("TempType", "BuyNow");

                        return RedirectToVnPay(order.TotalAmount, newOrderId);
                    }
                    await ProcessOrderDatabase(order, "BuyNow", bookId, quantity, userId);
                }
                // --- LUỒNG GIỎ HÀNG ---
                else
                {
                    var cartItems = await _cartIteamService.GetCartItemById(userId);

                    foreach (var item in cartItems)
                    {
                        var bookInCart = await _memberBookService.GetById(item.BookId);

                        if (bookInCart.Quantity < item.Quantity)
                        {
                            TempData["Error"] = $"Sách '{bookInCart.Title}' chỉ còn {bookInCart.Quantity} quyển. Vui lòng cập nhật lại giỏ hàng.";
                            return RedirectToAction("Index", "CartItem");
                        }
                    }

                    decimal subTotal = cartItems.Sum(x => x.SellingPrice * x.Quantity);

                    order.ShippingFee = order.TotalAmount - subTotal;

                    if (PaymentMethod == "VNPAY")
                    {
                        HttpContext.Session.SetString("TempOrder", JsonSerializer.Serialize(order));
                        HttpContext.Session.SetString("TempType", "Cart");

                        return RedirectToVnPay(order.TotalAmount, newOrderId);
                    }

                    await ProcessOrderDatabase(order, "Cart", "", 0, userId);
                }

                TempData["Message"] = "Đặt hàng thành công!";
                return RedirectToAction("Index", "Member");
            }
            catch (Exception ex)
            {
                return Content("Lỗi: " + ex.Message);
            }
        }

        // url vn pay
        private IActionResult RedirectToVnPay(decimal amount, string orderId)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            if (ipAddress == "::1") ipAddress = "127.0.0.1";

            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((long)(amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            DateTime timeNow = DateTime.UtcNow.AddHours(7);
            vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_ExpireDate", timeNow.AddMinutes(15).ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang: " + orderId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", orderId);

            return Redirect(vnpay.CreateRequestUrl(_configuration["VnPay:Url"], _configuration["VnPay:HashSecret"]));
        }

        // --- LUỒNG THANH TOÁN XONG VNPAY CALLBACK ---
        [HttpGet]
        public async Task<IActionResult> PaymentCallback()
        {
            if (Request.Query.Count > 0)
            {
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (var s in Request.Query)
                {
                    if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_")) vnpay.AddResponseData(s.Key, s.Value.ToString());
                }

                bool checkSignature = vnpay.ValidateSignature(Request.Query["vnp_SecureHash"], _configuration["VnPay:HashSecret"]);
                if (checkSignature && vnpay.GetResponseData("vnp_ResponseCode") == "00")
                {
                    // LẤY LẠI THÔNG TIN TỪ SESSION VÀ LƯU VÀO DATABASE
                    var orderJson = HttpContext.Session.GetString("TempOrder");
                    var type = HttpContext.Session.GetString("TempType");

                    if (!string.IsNullOrEmpty(orderJson))
                    {
                        var order = JsonSerializer.Deserialize<OrderDTO>(orderJson);
                        int userId = order.Id;

                        if (type == "BuyNow")
                        {
                            string bookId = HttpContext.Session.GetString("TempBookId");
                            int quantity = HttpContext.Session.GetInt32("TempQuantity") ?? 1;
                            await ProcessOrderDatabase(order, "BuyNow", bookId, quantity, userId);
                        }
                        else
                        {
                            await ProcessOrderDatabase(order, "Cart", "", 0, userId);
                        }

                        // Xóa Session sau khi xong
                        HttpContext.Session.Remove("TempOrder");
                        HttpContext.Session.Remove("TempType");

                        TempData["Message"] = "Thanh toán VNPay thành công và đơn hàng đã được tạo!";
                    }
                }
                else
                {
                    TempData["Error"] = "Thanh toán thất bại hoặc đã bị hủy. Đơn hàng chưa được tạo.";
                }
            }
            return RedirectToAction("Index", "Member");
        }
        private async Task ProcessOrderDatabase(OrderDTO order, string type, string bookId, int quantity, int userId)
        {
            await _service.Add(order);

            if (type == "BuyNow")
            {
                var book = await _memberBookService.GetById(bookId);
                await _orderDetailService.Add(new OrderDetailDTO { OrderDetailId = Guid.NewGuid().ToString(), OrderId = order.OrderId, BookId = book.BookId, Quantity = quantity, UnitPrice = book.SellingPrice });
                book.Quantity -= quantity;
                await _memberBookService.Update(book);
            }
            else
            {
                var cartItems = await _cartIteamService.GetCartItemById(userId);
                foreach (var item in cartItems)
                {
                    await _orderDetailService.Add(new OrderDetailDTO { OrderDetailId = Guid.NewGuid().ToString(), OrderId = order.OrderId, BookId = item.BookId, Quantity = item.Quantity, UnitPrice = item.SellingPrice });
                    var bookInCart = await _memberBookService.GetById(item.BookId);
                    bookInCart.Quantity -= item.Quantity;
                    await _memberBookService.Update(bookInCart);
                    await _cartIteamService.Delete(item.CartItemId);
                }
            }
        }
    
        [HttpGet]
        public async Task<IActionResult> DetailsMember(string id)
        {
            var order = await _orderDetailService.GetOrderDetailById(id);
            var orderInfo = await _service.GetById(id);

            if (orderInfo != null)
            {
                ViewBag.CustomerName = orderInfo?.Fullname;
                ViewBag.OrderDate = orderInfo.OrderDate;
                ViewBag.PaymentMethod = orderInfo.PaymentMethod;
                ViewBag.ShippingFee = orderInfo.ShippingFee;
            }
            return View(order);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAdmin(string id)
        {
            var order = await _orderDetailService.GetOrderDetailById(id);
            var orderInfo = await _service.GetById(id);

            if (orderInfo != null)
            {
                ViewBag.CustomerName = orderInfo?.Fullname;
                ViewBag.OrderDate = orderInfo.OrderDate;
                ViewBag.PaymentMethod = orderInfo.PaymentMethod;
                ViewBag.ShippingFee = orderInfo.ShippingFee;
            }

            return View(order);
        }

        // Ship API - GHN
        [HttpGet]
        public async Task<IActionResult> GetProvinces()
        {
            var data = await _ghnService.GetProvincesAsync();
            return Content(data, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetDistricts(int provinceId)
        {
            var data = await _ghnService.GetDistrictsAsync(provinceId);
            return Content(data, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> GetWards(int districtId)
        {
            var data = await _ghnService.GetWardsAsync(districtId);
            return Content(data, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> CalculateFee(int districtId, string wardCode, int quantity)
        {
            //300 gram mooix quyển sách
            int totalWeight = quantity * 300;

            decimal fee = await _ghnService.CalculateShippingFeeAsync(districtId, wardCode, totalWeight);

            return Json(new { fee = fee });
        }
    }
}
