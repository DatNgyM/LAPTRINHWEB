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

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/User/Index
        public async Task<IActionResult> Index(string searchString, string role, string sortOrder, int page = 1, int pageSize = 10)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentRole"] = role;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["EmailSortParam"] = sortOrder == "email" ? "email_desc" : "email";

            // Lấy tất cả người dùng từ UserManager
            var allUsers = await _userManager.Users.ToListAsync();

            // Lọc theo search string nếu có
            var filteredUsers = allUsers;

            if (!String.IsNullOrEmpty(searchString))
            {
                filteredUsers = allUsers.Where(u =>
                    u.UserName.Contains(searchString) ||
                    u.Email.Contains(searchString) ||
                    u.PhoneNumber != null && u.PhoneNumber.Contains(searchString) ||
                    u.Full_Name != null && u.Full_Name.Contains(searchString)
                ).ToList();
            }

            // Lọc theo vai trò nếu có
            if (!String.IsNullOrEmpty(role))
            {
                var usersInRole = new List<ApplicationUser>();
                foreach (var user in filteredUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, role))
                    {
                        usersInRole.Add(user);
                    }
                }
                filteredUsers = usersInRole;
            }

            // Sắp xếp
            IEnumerable<ApplicationUser> sortedUsers = sortOrder switch
            {
                "name_desc" => filteredUsers.OrderByDescending(u => u.UserName),
                "email" => filteredUsers.OrderBy(u => u.Email),
                "email_desc" => filteredUsers.OrderByDescending(u => u.Email),
                _ => filteredUsers.OrderBy(u => u.UserName)
            };

            // Phân trang
            int totalItems = sortedUsers.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var paginatedUsers = sortedUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Lưu vai trò của người dùng vào ViewBag
            var userRoles = new Dictionary<string, List<string>>();

            foreach (var user in paginatedUsers)
            {
                userRoles[user.Id] = (await _userManager.GetRolesAsync(user)).ToList();
            }
            ViewBag.UserRoles = userRoles;

            // Lấy tất cả các roles
            var allRoles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = allRoles;

            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = page;

            // Hiển thị thông báo từ TempData nếu có
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            return View(paginatedUsers);
        }

        // GET: Admin/User/Create
        public async Task<IActionResult> Add()
        {
            // Chỉ hiển thị role Manager và TourGuide trong ViewBag
            var managedRoles = await _roleManager.Roles
                .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                .ToListAsync();

            ViewBag.Roles = managedRoles;
            return View();
        }

        // POST: Admin/User/Create
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

            // Lưu thông tin role hiện tại vào ViewBag
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.CurrentRole = roles.FirstOrDefault();

            // Chỉ hiển thị role Manager và TourGuide trong ViewBag
            var managedRoles = await _roleManager.Roles
                .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                .ToListAsync();
            ViewBag.Roles = managedRoles;

            return View(user);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string email, string phoneNumber, string fullName,
                                            string address, string newRole, string newPassword, bool emailConfirmed)
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

            // Kiểm tra nếu người dùng chọn role mới và nó không phải là Manager hoặc TourGuide
            if (!string.IsNullOrEmpty(newRole) && newRole != "Manager" && newRole != "TourGuide")
            {
                ModelState.AddModelError("", "Chỉ được phép đặt vai trò Manager hoặc TourGuide");

                var currentRoles = await _userManager.GetRolesAsync(user);
                ViewBag.CurrentRole = currentRoles.FirstOrDefault();

                var managedRoles = await _roleManager.Roles
                    .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                    .ToListAsync();
                ViewBag.Roles = managedRoles;

                return View(user);
            }

            if (ModelState.IsValid)
            {
                user.Email = email;
                user.UserName = email;
                user.PhoneNumber = phoneNumber;
                user.Full_Name = fullName;
                user.Address = address;
                user.EmailConfirmed = emailConfirmed;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Cập nhật role nếu có thay đổi
                    var roles = await _userManager.GetRolesAsync(user);

                    // Không cho phép thay đổi role nếu người dùng là Admin
                    if (!string.IsNullOrEmpty(newRole) && !roles.Contains("Admin"))
                    {
                        await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                        await _userManager.AddToRoleAsync(user, newRole);
                    }

                    // Reset password nếu người dùng nhập password mới
                    if (!String.IsNullOrEmpty(newPassword))
                    {
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, newPassword);
                    }

                    TempData["SuccessMessage"] = "Đã cập nhật thông tin người dùng thành công!";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var currentUserRoles = await _userManager.GetRolesAsync(user);
            ViewBag.CurrentRole = currentUserRoles.FirstOrDefault();

            var availableRoles = await _roleManager.Roles
                .Where(r => r.Name == "Manager" || r.Name == "TourGuide")
                .ToListAsync();
            ViewBag.Roles = availableRoles;

            return View(user);
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

            // Không cho phép xóa Admin
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản Admin";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);

            return View(user);
        }

        // POST: Admin/User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                // Kiểm tra xem có phải Admin không
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    TempData["ErrorMessage"] = "Không thể xóa tài khoản Admin";
                    return RedirectToAction(nameof(Index));
                }

                // Khóa tài khoản thay vì xóa
                var result = await _userManager.SetLockoutEnabledAsync(user, true);
                if (result.Succeeded)
                {
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                    TempData["SuccessMessage"] = "Đã vô hiệu hóa người dùng thành công!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Có lỗi xảy ra khi vô hiệu hóa người dùng.";
                }

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
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

            ViewBag.UserRoles = await _userManager.GetRolesAsync(user);

            return View(user);
        }
    }
}