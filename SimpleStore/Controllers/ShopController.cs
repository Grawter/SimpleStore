using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;
using SimpleStore.Models.Shop;
using SimpleStore.ViewModels;
using SimpleStore.ViewModels.Supporting_tools;

namespace SimpleStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationContext db;
        public ShopController(ApplicationContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index(string name, string aviability, int page = 1,
             SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 4;

            //фильтрация
            IQueryable<Phone> Phones = db.Phones;

            if (!string.IsNullOrEmpty(name))
            {
                Phones = Phones.Where(p => p.Name.Contains(name));
            }

            if(!string.IsNullOrEmpty(aviability) && aviability !="Все")
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
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Phones = items
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Case() => View(await db.Cases.ToListAsync());

        [HttpGet]
        public async Task<IActionResult> Headphone() => View(await db.Headphones.ToListAsync());

        [HttpGet]
        public async Task<IActionResult> Phone() => View(await db.Phones.ToListAsync());

        [HttpGet]
        public async Task<IActionResult> Powerbank() => View(await db.Powerbanks.ToListAsync());


        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Phone phone)
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
            return RedirectToAction("Index");
        }
    }
}
