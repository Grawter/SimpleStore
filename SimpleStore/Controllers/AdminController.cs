using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SimpleStore.Models;
using SimpleStore.ViewModels.Admin;
using SimpleStore.ViewModels.Supporting_tools;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using SimpleStore.ViewModels.Authorization;

namespace SimpleStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string email, int page = 1, SortState sortOrder = SortState.NamesAsc)
        {
            IQueryable<User> users = _userManager.Users;

            // Фильтрация
            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(p => p.Email.Contains(email));
            }

            // Сортировка
            switch (sortOrder)
            {
                case SortState.NamesDesc:
                    users = users.OrderByDescending(s => s.FullName);
                    break;
                case SortState.EmailAsc:
                    users = users.OrderBy(s => s.Email);
                    break;
                case SortState.EmailDesc:
                    users = users.OrderByDescending(s => s.Email);
                    break;
                case SortState.PhoneAsc:
                    users = users.OrderBy(s => s.PhoneNumber);
                    break;
                case SortState.PhoneDesc:
                    users = users.OrderByDescending(s => s.PhoneNumber);
                    break;
                default:
                    users = users.OrderBy(s => s.FullName);
                    break;
            }

            // пагинация
            int pageSize = 7;
            var count = await users.CountAsync();
            var items = await users.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(email),
                Users = items
            };
            return View(indexviewModel);
        }

        [HttpGet]
        public IActionResult CreateUser() => View("~/Views/Admin/User/CreateUser.cshtml");

        [HttpPost]
        public async Task<IActionResult> CreateUser(AdmCreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, FullName = model.FullName,
                    Address = model.Address, PhoneNumber = model.PhoneNumber, DateBirth = model.DateBirth.ToShortDateString()};

                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, "User");

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("~/Views/Admin/User/CreateUser.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            AdmEditUserViewModel model = new AdmEditUserViewModel { Id = user.Id, FullName = user.FullName, 
                Address = user.Address, PhoneNumber = user.PhoneNumber, DateBirth = DateTime.Parse(user.DateBirth),
                Email = user.Email };
            
            if (user.Email == User.Identity.Name)
                ViewBag.ThisAdmAcc = true;

            return View("~/Views/Admin/User/EditUser.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(AdmEditUserViewModel model, bool ThisAdmAcc)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.DateBirth = model.DateBirth.ToShortDateString();
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var aUser = await _userManager.FindByNameAsync(User.Identity.Name); // Определение текущего аккаунта

                        // перезаход после изменения данных. (Для корректной работы с авторизацией у изменённого пользователя)
                        await _signInManager.RefreshSignInAsync(user);
                        if(!ThisAdmAcc)
                            await _signInManager.RefreshSignInAsync(aUser); // Возвращаение на свой изначальный (админский) аккаунт
                        
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View("~/Views/Admin/User/EditUser.cshtml", model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            AdmChangePassViewModel model = new AdmChangePassViewModel { Id = user.Id, Email = user.Email };
            return View("~/Views/Admin/User/ChangePassword.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AdmChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var _passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result =
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View("~/Views/Admin/User/ChangePassword.cshtml", model);
        }

        [HttpGet]
        [ActionName("DeleteUser")]
        public async Task<ActionResult> ConfirmDeleteUser(string id) 
        {
            if (id != null)
            {
                User user = await _userManager.FindByIdAsync(id);
                return View("~/Views/Admin/User/DeleteUser.cshtml", user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        ///////////// Section Role

        public IActionResult Roles() => View("~/Views/Admin/Role/Roles.cshtml", _roleManager.Roles.ToList());

        public IActionResult CreateRole() => View("~/Views/Admin/Role/CreateRole.cshtml");

        [HttpPost]
        public async Task<IActionResult> CreateRole(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("~/Views/Admin/Role/CreateRole.cshtml",name);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string userId)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View("~/Views/Admin/Role/EditRole.cshtml", model);
            }

            return NotFound($"Пользователь не найден/неверный id");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(string userId, List<string> roles)
        {
            // получаем пользователя
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получаем все роли
                var allRoles = _roleManager.Roles.ToList();
                // получаем список ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                var aUser = await _userManager.FindByNameAsync(User.Identity.Name); // Определение текущего аккаунта

                // перезаход после изменения данных. (Для корректной работы с авторизацией у изменённого пользователя)
                await _signInManager.RefreshSignInAsync(user);

                if (aUser.UserName != user.UserName)
                    await _signInManager.RefreshSignInAsync(aUser); // Возвращаение на свой изначальный (админский) аккаунт

                return RedirectToAction("Index");
            }

            return NotFound("Пользователь не найден");
        }

        [HttpGet]
        [ActionName("DeleteRole")]
        public async Task<IActionResult> ConfirmDeleteRole(string id)
        {
            if (id != null)
            {
                IdentityRole role = await _roleManager.FindByIdAsync(id);
                return View("~/Views/Admin/Role/DeleteRole.cshtml", role);
            }
            return NotFound($"Неверный id");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Roles");
        }

    }
}