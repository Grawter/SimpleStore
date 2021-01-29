using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SimpleStore.Models;
using SimpleStore.ViewModels.Authentication;

namespace SimpleStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register(string FullPath = null)
        {
            return View( new RegisterViewModel { FullPath = FullPath } );
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // противодействия подделке межсайтовых запросов
        public async Task<IActionResult> Register([FromForm] RegisterViewModel model)
        {
            if (ModelState.IsValid) // Если введённые данные валидны
            {
                User user = new User { Email = model.Email, UserName = model.Email, FullName = model.FullName,
                    Address = model.Address, PhoneNumber = model.PhoneNumber, DateBirth = model.DateBirth.ToShortDateString() };
                var result = await _userManager.CreateAsync(user, model.Password); // добавление пользователя через пароль

                if (await _roleManager.FindByNameAsync("Admin") == null)
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));

                if (await _roleManager.FindByNameAsync("Moderator") == null)
                    await _roleManager.CreateAsync(new IdentityRole("Moderator"));

                if (await _roleManager.FindByNameAsync("User") == null)
                    await _roleManager.CreateAsync(new IdentityRole("User"));

                await _userManager.AddToRoleAsync(user, "User");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false); // установка куки авторизации
                    // проверка, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.FullPath) && Url.IsLocalUrl(model.FullPath))
                    {
                        return Redirect(model.FullPath);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        
        [AcceptVerbs("Get", "Post")] // для указания нескольких обрабатываемых типов запросов
        public async Task<IActionResult> CheckEmail(string Email)
        {
            var res = await _userManager.FindByEmailAsync(Email);

            if (res != null) // если найдено совпадение емайлов
                return Json(false);
           
            return Json(true);
        }

        [HttpGet]
        public IActionResult Login(string FullPath = null)
        {
            return View(new LoginViewModel { FullPath = FullPath });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // проверка, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.FullPath) && Url.IsLocalUrl(model.FullPath))
                    {
                        return Redirect(model.FullPath);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль/Incorrect username and (or) password");
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // удаляем аутентификационные куки
            return RedirectToAction("Index", "Home");
        }

    }
}