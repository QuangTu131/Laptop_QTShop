using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShoppingLaptop_QT.Models;
using ShoppingLaptop_QT.Repository;

namespace ShoppingLaptop_QT.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/User")]
    public class UserController : Controller
    {
		private readonly UserManager<AppUserModel> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly DataContext _dataContext;

		public UserController(DataContext context ,UserManager<AppUserModel> userManager,RoleManager<IdentityRole> roleManager)
        {      
			_userManager = userManager;
			_roleManager = roleManager;
			_dataContext = context;
        }
		[HttpGet]
		[Route("Index")]
		public async Task<IActionResult> Index()
		{
			var usersWithRoles = await (from u in _dataContext.Users
									   join ur in _dataContext.UserRoles on u.Id equals ur.UserId
									   join r in _dataContext.Roles on ur.RoleId equals r.Id
									   select new {User=u,RoleName=r.Name})
								.ToListAsync();

			return View(usersWithRoles);
		}

		[HttpGet]
		[Route("Create")]
		public async Task<IActionResult> Create()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(new AppUserModel());
		}

		[HttpGet]
		[Route("Edit")]
		public async Task<IActionResult> Edit(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			return View(user);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Edit")]
		public async Task<IActionResult> Edit(string Id,AppUserModel user)
		{
			var existingUser = await _userManager.FindByIdAsync(Id); // lay user dua vao Id
			if(existingUser == null)
			{
				return NotFound();
			}
			if (ModelState.IsValid)
			{
				//Update other user properties (Excluding password)
				existingUser.UserName = user.UserName;
				existingUser.Email = user.Email;
				existingUser.PhoneNumber = user.PhoneNumber;
				existingUser.RoleId = user.RoleId;

				var updateUserResult = await _userManager.UpdateAsync(existingUser); // thuc hien update
				if (updateUserResult.Succeeded)
				{
					return RedirectToAction("Index", "User");
				}
				else
				{
					AddIdentityError(updateUserResult);
					return View(existingUser);
				}
			}
			var roles = await _roleManager.Roles.ToListAsync();
			ViewBag.Roles = new SelectList(roles, "Id", "Name");
			//Model validation failed
			TempData["error"] = "Model validation failes!";
			var errors = ModelState.Values.SelectMany(v=>v.Errors.Select(e => e.ErrorMessage)).ToList();
			string errorMessage = string.Join("\n", errors);
			return View(existingUser);
		}

		private void AddIdentityError(IdentityResult identityResult)
		{
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Create")]
        public async Task<IActionResult> Create(AppUserModel user)
        {
            if (ModelState.IsValid)
            {
                var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash); // Tạo user
                if (createUserResult.Succeeded)
                {
                    var createUser = await _userManager.FindByEmailAsync(user.Email); // Tìm user dựa trên email
                    var role = await _roleManager.FindByIdAsync(user.RoleId); // Lấy role từ RoleId của user

                    if (role == null)
                    {
                        ModelState.AddModelError(string.Empty, "Vai trò không tồn tại.");
                        return View(user);
                    }

                    // Gán quyền cho user
                    var addToRoleResult = await _userManager.AddToRoleAsync(createUser, role.Name);
                    if (!addToRoleResult.Succeeded)
                    {
                        foreach (var error in addToRoleResult.Errors) // Đúng lỗi cần lặp
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(user);
                    }

                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(user);
                }
            }
            else
            {
                TempData["error"] = "Model có một số thứ đang bị lỗi";
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                string errorMessage = string.Join("\n", errors);
                ModelState.AddModelError(string.Empty, errorMessage);
            }

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id", "Name");
            return View(user);
        }


        [HttpGet]
		[Route("Delete")]
		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return NotFound();
			}
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}
			var deleteResult = await _userManager.DeleteAsync(user);
			if (!deleteResult.Succeeded)
			{
				return View("Error");
			}
			TempData["success"] = "User đã được xóa thành công";
			return RedirectToAction("Index");
		}

	}
}
