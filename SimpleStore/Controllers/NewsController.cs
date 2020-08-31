using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;

namespace SimpleStore.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationContext db;
        public NewsController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index() => View(await db.News.OrderByDescending(S => S.Id).ToListAsync());

        public async Task<IActionResult> Show(int? id)
        {
            var News = await db.News.FirstOrDefaultAsync(N => N.Id == id);
            return View(News);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Novelty news)
        {
            db.News.Add(news);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Novelty news = await db.News.FirstOrDefaultAsync(p => p.Id == id);
                if (news != null)
                    return View(news);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Novelty news)
        {
            db.News.Update(news);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Novelty news = await db.News.FirstOrDefaultAsync(N => N.Id == id);
                return View(news);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromForm] int? id)
        {
            if (id != null)
            {
                Novelty news = await db.News.FirstOrDefaultAsync(N => N.Id == id);
                if (news != null)
                {
                    db.News.Remove(news);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}