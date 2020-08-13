using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;
using SimpleStore.Models.Shop;

namespace SimpleStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationContext db;
        public ShopController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index() => View(await db.Phones.ToListAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Phone phone)
        {
            db.Phones.Add(phone);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
