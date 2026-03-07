using BookGate.Application.DTOs;
using BookGate.Application.Services;
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

            // --- BẮT ĐẦU ĐOẠN CODE CẤP COOKIE ---

            // 1. Ghi thông tin người dùng vào các Claim (như viết thẻ tên)
            var claims = new List<Claim>
            {
                // Dòng này rất quan trọng: Nó sẽ gán tên vào biến @User.Identity.Name ngoài View
                new Claim(ClaimTypes.Name, user.Username), // Nếu Entity có FullName thì dùng user.FullName
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role == 0 ? "Admin" : "Member")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            if (user.Role == 0)
            {
                return RedirectToAction("Index", "Book");
            }

            return RedirectToAction("Index", "Member");
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO auth)
        {
            await _service.Register(auth);
            return RedirectToAction("Login");
        }
    }
}
