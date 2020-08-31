using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;
using SimpleStore.ViewModels.Supporting_tools;

namespace SimpleStore.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class EditShopController : Controller
    {
        private readonly ApplicationContext db;
        public EditShopController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> ShowProducts(string type, string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            ViewBag.type = type;

            //фильтрация
            IQueryable<Product> Products = db.Products.Where(p => p.Type == type);

            if (!string.IsNullOrEmpty(name))
            {
                Products = Products.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(aviability) && aviability != "Все")
            {
                Products = Products.Where(p => p.Availability == aviability);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Products = Products.OrderByDescending(s => s.Name);
                    break;
                case SortState.CompanyAsc:
                    Products = Products.OrderBy(s => s.Company);
                    break;
                case SortState.CompanyDesc:
                    Products = Products.OrderByDescending(s => s.Company);
                    break;
                case SortState.CapacityAsc:
                    Products = Products.OrderBy(s => s.Capacity);
                    break;
                case SortState.CapacityDesc:
                    Products = Products.OrderByDescending(s => s.Capacity);
                    break;
                case SortState.PriceAsc:
                    Products = Products.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    Products = Products.OrderByDescending(s => s.Price);
                    break;
                default:
                    Products = Products.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            int pageSize = 5;
            var count = await Products.CountAsync();
            var items = await Products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Products = items
            };
            return View(indexviewModel);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Product product)
        {
            if (product.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(product.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)product.File.Length);
                }
                // установка массива байтов
                product.Image = imageData;
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();


            return RedirectToAction("ShowProducts", new { type = product.Type });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                    return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Product product)
        {
            if (product.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(product.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)product.File.Length);
                }
                // установка массива байтов
                product.Image = imageData;
            }

            db.Products.Update(product);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowProducts", new { type = product.Type });
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<ActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete([FromForm] int? id)
        {
            if (id != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ShowProducts", new { type = product.Type });
                }
            }
            return NotFound();
        }

    }
}