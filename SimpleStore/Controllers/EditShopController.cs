using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;
using SimpleStore.Models.Shop;
using SimpleStore.ViewModels.Supporting_tools;

namespace SimpleStore.Controllers
{
    public class EditShopController : Controller
    {
        private readonly ApplicationContext db;
        public EditShopController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowPhones(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 5;

            //фильтрация
            IQueryable<Phone> Phones = db.Phones;

            if (!string.IsNullOrEmpty(name))
            {
                Phones = Phones.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(aviability) && aviability != "Все")
            {
                Phones = Phones.Where(p => p.Availability == aviability);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Phones = Phones.OrderByDescending(s => s.Name);
                    break;
                case SortState.CompanyAsc:
                    Phones = Phones.OrderBy(s => s.Company);
                    break;
                case SortState.CompanyDesc:
                    Phones = Phones.OrderByDescending(s => s.Company);
                    break;
                case SortState.PriceAsc:
                    Phones = Phones.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    Phones = Phones.OrderByDescending(s => s.Price);
                    break;
                default:
                    Phones = Phones.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            var count = await Phones.CountAsync();
            var items = await Phones.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Phones = items
            };
            return View("~/Views/EditShop/Phone/ShowPhones.cshtml", indexviewModel);
        }

       [HttpGet]
        public IActionResult CreatePhone() => View("~/Views/EditShop/Phone/CreatePhone.cshtml"); // полный путь, т.к. для большей наглядности и сортировки в
        // добавлена папка в модель

        [HttpPost]
        public async Task<IActionResult> CreatePhone(Phone phone)
        {
            if (phone.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(phone.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)phone.File.Length);
                }
                // установка массива байтов
                phone.Image = imageData;
            }

            db.Phones.Add(phone);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowPhones"); // RedirectToAction по дефолту перебрасывает в метод контроллера, поэтому путь указвать не нужно
        }

        [HttpGet]
        public async Task<IActionResult> EditPhone(int? id)
        {
            if (id != null)
            {
                Phone phone = await db.Phones.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                    return View("~/Views/EditShop/Phone/EditPhone.cshtml", phone);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditPhone(Phone phone)
        {
            if (phone.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(phone.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)phone.File.Length);
                }
                // установка массива байтов
                phone.Image = imageData;
            }

            db.Phones.Update(phone);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowPhones");
        }

        [HttpGet]
        [ActionName("DeletePhone")]
        public async Task<ActionResult> ConfirmDeletePhone(int? id)
        {
            if (id != null)
            {
                Phone phone = await db.Phones.FirstOrDefaultAsync(p => p.Id == id);
                return View("~/Views/EditShop/Phone/DeletePhone.cshtml", phone);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeletePhone(int? id)
        {
            if (id != null)
            {
                Phone phone = await db.Phones.FirstOrDefaultAsync(p => p.Id == id);
                if (phone != null)
                {
                    db.Phones.Remove(phone);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ShowPhones");
                }
            }
            return NotFound();
        }

    }
}
