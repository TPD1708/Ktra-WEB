using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using KiemTraWeb.Data;

namespace KiemTraWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly KiemTraWebContext _context;

        public AccountController(KiemTraWebContext context)
        {
            _context = context;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string mssv, string? returnUrl = null)
        {
            var sinhVien = await _context.SinhViens.FirstOrDefaultAsync(s => s.MaSV == mssv);

            if (sinhVien != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, sinhVien.HoTen),
                    new Claim("MaSV", sinhVien.MaSV),
                    new Claim(ClaimTypes.Role, "SinhVien"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("Index", "DangKyHocPhan");
            }

            ModelState.AddModelError(string.Empty, "Mã số sinh viên không hợp lệ");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
