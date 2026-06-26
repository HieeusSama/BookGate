using BookGate.Application.DTOs;
using BookGate.Application.Services;
using BookGate.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BookGate.API.Controllers
{
    public class AuthController : Controller
    {

        private readonly AuthService _service;
        public IActionResult Register() => View();
        public IActionResult Login() => View();
        public AuthController(AuthService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO auth)
        {
            var user = await _service.Login(auth);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role == 0 ? "Admin" : "Member")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (user.Role == 0)
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Member");
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO auth)
        {
            if (!ModelState.IsValid)
            {
                return View(auth);
            }

            bool isEmailExist = await _service.IsEmailRegistered(auth.Email);
            if (isEmailExist)
            {
                ModelState.AddModelError("Email", "Email này đã được đăng ký. Vui lòng sử dụng email khác.");
                return View(auth);
            }

            await _service.Register(auth);
            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Member");
        }
    }
}
