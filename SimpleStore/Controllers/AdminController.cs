﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SimpleStore.Models;
using SimpleStore.ViewModels.Authorization;
using SimpleStore.ViewModels.Admin;
using SimpleStore.ViewModels.Supporting_tools;
using Microsoft.AspNetCore.Authorization;

namespace SimpleStore.Controllers
{
    [Authorize(Roles = "Admin")] // ограничения к доступу
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

        ///////////// Секция управления ролями

        public async Task<IActionResult> Index(string email, int page = 1, SortState sortOrder = SortState.NamesAsc)
        {
            IQueryable<User> users = _userManager.Users; // передача всех пользователей

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
            int pageSize = 20;
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
        public IActionResult CreateUser() => View("~/Views/Admin/User/CreateUser.cshtml"); // указан полный маршрут, т.к. в папке представлений есть две области: Роли и Пользователь

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] AdmCreateUserViewModel model)
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
                return NotFound("Не найдено");
            }
            AdmEditUserViewModel model = new AdmEditUserViewModel { Id = user.Id, FullName = user.FullName, 
                Address = user.Address, PhoneNumber = user.PhoneNumber, DateBirth = DateTime.Parse(user.DateBirth),
                Email = user.Email };
            
            if (user.Email == User.Identity.Name) // Если попытка изменения собственных данных
                ViewBag.ThisAdmAcc = true;

            return View("~/Views/Admin/User/EditUser.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser([FromForm] AdmEditUserViewModel model, bool ThisAdmAcc)
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
                return NotFound("Не найдено");
            }
            AdmChangePassViewModel model = new AdmChangePassViewModel { Id = user.Id, Email = user.Email };
            return View("~/Views/Admin/User/ChangePassword.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromForm] AdmChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // создание валидатора пароля
                    var _passwordValidator = HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    // создание хэшера пароля
                    var _passwordHasher = HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                    IdentityResult result = await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    
                    if (result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword); // хэшируем пароль
                        await _userManager.UpdateAsync(user); // обновляем данные пользователя
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
            return NotFound("Не найдено");
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser([FromForm] string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }

        ///////////// Секция управления ролями

        public async Task<IActionResult> Roles() => View("~/Views/Admin/Role/Roles.cshtml", await _roleManager.Roles.ToListAsync());

        public IActionResult CreateRole() => View("~/Views/Admin/Role/CreateRole.cshtml");

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromForm] string name = null)
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
            return View("~/Views/Admin/Role/CreateRole.cshtml", name);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId); // получение пользователя
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user); // получение списока ролей пользователя
                var allRoles = _roleManager.Roles.ToList();  // получение всех ролей
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View("~/Views/Admin/Role/EditRole.cshtml", model);
            }

            return NotFound("Не найдено");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole([FromForm] string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получение списока ролей пользователя
                var userRoles = await _userManager.GetRolesAsync(user);
                // получение всех ролей
                var allRoles = _roleManager.Roles.ToList();
                // получение списока ролей, которые были добавлены
                var addedRoles = roles.Except(userRoles);
                // получение ролей, которые были удалены
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

            return NotFound("Не найдено");
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
            return NotFound("Не найдено");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole([FromForm] string id)
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