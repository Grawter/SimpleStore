using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleStore.ViewModels;
using SimpleStore.Models;
using Microsoft.AspNetCore.Identity;

namespace SimpleStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name, Surname = model.SurName,
                    Address = model.Address, PhoneNumber = model.PhoneNumber, Day = model.Day, Mount = model.Mount, Year = model.Year };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // установка куки
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
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
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> CheckEmail(string Email)
        {
            var res = await _userManager.FindByEmailAsync(Email);

            if (res != null) // если найдено совпадение емайлов
                return Json(false);
           
            return Json(true);
        }
    }
}
