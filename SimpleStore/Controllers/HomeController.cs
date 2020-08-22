using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SimpleStore.Models;
using SimpleStore.ViewModels.User;
using SimpleStore.Models.Booking;
using Microsoft.EntityFrameworkCore;
using SimpleStore.ViewModels.Supporting_tools;

namespace SimpleStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext db;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context, UserManager<User> userManager)
        {
            _logger = logger;
            db = context;
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
                FullName = user.FullName,
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
                    user.FullName = model.FullName;
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
            UserChangeEmailViewModel model = new UserChangeEmailViewModel { Id = user.Id, FullName = user.FullName, Email = user.Email };
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

        public async Task<IActionResult> MyOrders(string UserId, int page = 1)
        {
            IQueryable<Order> orders = db.Orders.Where(p => p.UserId == UserId);
            orders = orders.OrderByDescending(s => s.Id);

            // пагинация
            int pageSize = 7;
            var count = await orders.CountAsync();
            var items = await orders.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                Orders = items
            };
            return View(indexviewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}