// filepath: c:\Users\DAT DAT\Documents\GitHub\LAPTRINHWEB\Controllers\AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using LAPTRINHWEB.Services;
using LAPTRINHWEB.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace LAPTRINHWEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly TourDbContext _context;

        public AccountController(IAuthService authService, TourDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ email và mật khẩu.";
                return View();
            }

            var user = await _authService.LoginAsync(email, password);
            
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.ID_User.ToString()),
                    new Claim(ClaimTypes.Name, user.Full_Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Role_Name)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirect based on role
                switch (user.Role.Role_Name)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                    case "Manager":
                        return RedirectToAction("Index", "Dashboard", new { area = "Manager" });
                    case "TourGuide":
                        return RedirectToAction("Index", "Dashboard", new { area = "TourGuide" });
                    default:
                        return RedirectToLocal(returnUrl);
                }
            }

            ViewBag.ErrorMessage = "Email hoặc mật khẩu không đúng.";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.Roles = await _context.Roles
                .Where(r => r.Is_Active)
                .OrderBy(r => r.Role_Name)
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string fullName, 
            string email, 
            string password, 
            string confirmPassword,
            string phone,
            string address,
            DateTime? dateOfBirth,
            string gender,
            int roleId)
        {
            ViewBag.Roles = await _context.Roles
                .Where(r => r.Is_Active)
                .OrderBy(r => r.Role_Name)
                .ToListAsync();

            // Validation
            var errors = new List<string>();

            if (string.IsNullOrEmpty(fullName))
                errors.Add("Họ tên là bắt buộc");

            if (string.IsNullOrEmpty(email))
                errors.Add("Email là bắt buộc");
            else if (!IsValidEmail(email))
                errors.Add("Email không hợp lệ");

            if (string.IsNullOrEmpty(password))
                errors.Add("Mật khẩu là bắt buộc");
            else if (password.Length < 6)
                errors.Add("Mật khẩu phải có ít nhất 6 ký tự");

            if (password != confirmPassword)
                errors.Add("Mật khẩu xác nhận không khớp");

            if (await _authService.EmailExistsAsync(email))
                errors.Add("Email đã tồn tại");

            if (roleId == 0)
            {
                var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Role_Name == "Customer");
                if (customerRole != null)
                    roleId = customerRole.ID_Role;
                else
                    errors.Add("Không tìm thấy vai trò mặc định");
            }

            if (errors.Any())
            {
                ViewBag.ErrorMessages = errors;
                ViewBag.FormData = new 
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Address = address,
                    DateOfBirth = dateOfBirth,
                    Gender = gender,
                    RoleId = roleId
                };
                return View();
            }

            var user = new User
            {
                Full_Name = fullName,
                Email = email,
                Password_Hash = _authService.HashPassword(password),
                Phone = phone,
                Address = address,
                Date_Of_Birth = dateOfBirth,
                Gender = gender,
                ID_Role = roleId,
                Created_Date = DateTime.Now,
                Is_Active = true,
                Email_Verified = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}