using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleStore.Models;
using SimpleStore.ViewModels;

namespace SimpleStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        [HttpGet]
        public IActionResult Settings() => View(_userManager.Users.ToList());

        [HttpGet]
        public async Task<IActionResult> EditData(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserEditDataViewModel model = new UserEditDataViewModel
            {
                Id = user.Id,
                Name = user.Name,
                SurName = user.Surname,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Day = user.Day,
                Mount = user.Mount,
                Year = user.Year,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditData(UserEditDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Surname = model.SurName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Day = model.Day;
                    user.Mount = model.Mount;
                    user.Year = model.Year;

                    var result = await _userManager.UpdateAsync(user);
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
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeEmail(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserChangeEmailViewModel model = new UserChangeEmailViewModel { Id = user.Id, Name = user.Name, SurName = user.Surname, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeEmail(UserChangeEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Account");
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
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserChangePassViewModel model = new UserChangePassViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(UserChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Account");
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
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}