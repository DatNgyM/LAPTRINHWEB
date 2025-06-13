using LAPTRINHWEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAPTRINHWEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager,
                             RoleManager<IdentityRole> roleManager,
                             SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,
                                             string role, int page = 1)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParam"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["CurrentPage"] = page;
            ViewData["CurrentRole"] = role;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var users = _userManager.Users.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) ||
                                        u.Email.Contains(searchString) ||
                                        u.PhoneNumber.Contains(searchString) ||
                                        u.Full_Name.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(role))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                var userIds = usersInRole.Select(u => u.Id);
                users = users.Where(u => userIds.Contains(u.Id));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    users = users.OrderByDescending(u => u.UserName);
                    break;
                case "email":
                    users = users.OrderBy(u => u.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(u => u.Email);
                    break;
                default:
                    users = users.OrderBy(u => u.UserName);
                    break;
            }

            int pageSize = 10;
            var count = await users.CountAsync();
            var items = await users.Skip((page - 1) * pageSize)
                                 .Take(pageSize).ToListAsync();

            ViewData["TotalPages"] = (int)Math.Ceiling(count / (double)pageSize);

            var userRoles = new Dictionary<string, List<string>>();
            foreach (var user in items)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.ToList();
            }

            ViewBag.UserRoles = userRoles;

            var allRoles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = allRoles;

            return View(items);
        }

        // GET: Admin/User/Add
        public async Task<IActionResult> Add()
        {
            // Chỉ hiển thị role Manager và TourGuide trong ViewBag
            var managedRoles = await _roleManager.Roles
                .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                .ToListAsync();

            ViewBag.Roles = managedRoles;
            return View();
        }

        // POST: Admin/User/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string email, string phoneNumber, string password, string confirmPassword,
                                            string fullName, string address, string role, bool emailConfirmed = true)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email và mật khẩu là bắt buộc");

                var managedRoles = await _roleManager.Roles
                    .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                    .ToListAsync();
                ViewBag.Roles = managedRoles;

                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Mật khẩu và xác nhận mật khẩu không khớp nhau");

                var managedRoles = await _roleManager.Roles
                    .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                    .ToListAsync();
                ViewBag.Roles = managedRoles;

                return View();
            }

            // Giới hạn chỉ cho phép chọn role Manager hoặc TourGuide
            if (role != "Manager" && role != "TourGuide")
            {
                ModelState.AddModelError("", "Chỉ được phép thêm người dùng với vai trò Manager hoặc TourGuide");

                var managedRoles = await _roleManager.Roles
                    .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                    .ToListAsync();
                ViewBag.Roles = managedRoles;

                return View();
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Full_Name = fullName,
                    Address = address,
                    EmailConfirmed = emailConfirmed,
                    Created_Date = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, role);

                    TempData["SuccessMessage"] = "Đã tạo người dùng mới thành công!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var availableRoles = await _roleManager.Roles
                .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                .ToListAsync();
            ViewBag.Roles = availableRoles;

            return View();
        }

        // GET: Admin/User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy vai trò hiện tại của user
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.CurrentRole = userRoles.FirstOrDefault() ?? "Chưa có vai trò";

            // Chỉ hiển thị role Manager và TourGuide để chọn
            var managedRoles = await _roleManager.Roles
                .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                .ToListAsync();

            ViewBag.Roles = managedRoles;

            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string email, string phoneNumber,
                                            string fullName, string address, string newRole,
                                            string newPassword, bool emailConfirmed = true)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin cơ bản
            user.Email = email;
            user.UserName = email;
            user.PhoneNumber = phoneNumber;
            user.Full_Name = fullName;
            user.Address = address;
            user.EmailConfirmed = emailConfirmed;

            // Cập nhật thông tin
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                ViewBag.CurrentRole = userRoles.FirstOrDefault() ?? "Chưa có vai trò";

                var managedRoles = await _roleManager.Roles
                    .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                    .ToListAsync();
                ViewBag.Roles = managedRoles;

                return View(user);
            }

            // Cập nhật role nếu có sự thay đổi
            if (!string.IsNullOrEmpty(newRole))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, newRole);
            }

            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrEmpty(newPassword))
            {
                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (removePasswordResult.Succeeded)
                {
                    var addPasswordResult = await _userManager.AddPasswordAsync(user, newPassword);
                    if (!addPasswordResult.Succeeded)
                    {
                        foreach (var error in addPasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }

                        var userRoles = await _userManager.GetRolesAsync(user);
                        ViewBag.CurrentRole = userRoles.FirstOrDefault() ?? "Chưa có vai trò";

                        var managedRoles = await _roleManager.Roles
                            .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                            .ToListAsync();
                        ViewBag.Roles = managedRoles;

                        return View(user);
                    }
                }
            }

            TempData["SuccessMessage"] = "Cập nhật thông tin người dùng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra xem user có phải là Admin không
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản Admin!";
                return RedirectToAction(nameof(Index));
            }

            // Lấy vai trò của user để hiển thị
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;

            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra xem user có phải là Admin không
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (isAdmin)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản Admin!";
                return RedirectToAction(nameof(Index));
            }

            // Thay vì xóa, chúng ta sẽ khóa tài khoản để giữ lại lịch sử
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now.AddYears(100));

            TempData["SuccessMessage"] = "Đã khóa tài khoản người dùng thành công!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Lấy vai trò của user
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;

            return View(user);
        }
    }
}