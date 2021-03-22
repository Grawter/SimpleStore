using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SimpleStore.Models;
using Microsoft.AspNetCore.Authorization;

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
            if (id != null)
            {
                var News = await db.News.FirstOrDefaultAsync(N => N.Id == id);
                return View(News);
            }
            else
                return NotFound("Не найдено");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Novelty news)
        {
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Ошибка при добавлении");
            }
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Novelty news = await db.News.FirstOrDefaultAsync(p => p.Id == id);
                if (news != null)
                    return View(news);
            }
            return NotFound("Не найдено");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Novelty news)
        {
            db.News.Update(news);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Novelty news = await db.News.FirstOrDefaultAsync(N => N.Id == id);
                return View(news);
            }
            return NotFound("Не найдено");
        }

        [Authorize(Roles = "Admin, Moderator")]
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
            return NotFound("Не найдено");
        }

    }
}