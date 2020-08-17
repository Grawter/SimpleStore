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
    public class ShopController : Controller
    {
        private readonly ApplicationContext db;
        public ShopController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index() => View();

        [HttpGet]
        public async Task<IActionResult> Case(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 5;

            //фильтрация
            IQueryable<Case> Cases = db.Cases;

            if (!string.IsNullOrEmpty(name))
            {
                Cases = Cases.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(aviability) && aviability != "Все")
            {
                Cases = Cases.Where(p => p.Availability == aviability);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Cases = Cases.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    Cases = Cases.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    Cases = Cases.OrderByDescending(s => s.Price);
                    break;
                default:
                    Cases = Cases.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            var count = await Cases.CountAsync();
            var items = await Cases.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Cases = items
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Headphone(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc) 
        {
            int pageSize = 5;

            //фильтрация
            IQueryable<Headphone> Headphones = db.Headphones;

            if (!string.IsNullOrEmpty(name))
            {
                Headphones = Headphones.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(aviability) && aviability != "Все")
            {
                Headphones = Headphones.Where(p => p.Availability == aviability);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Headphones = Headphones.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    Headphones = Headphones.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    Headphones = Headphones.OrderByDescending(s => s.Price);
                    break;
                default:
                    Headphones = Headphones.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            var count = await Headphones.CountAsync();
            var items = await Headphones.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Headphones = items
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Phone(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
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
        public async Task<IActionResult> Powerbank(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 5;

            //фильтрация
            IQueryable<Powerbank> Powerbanks = db.Powerbanks;

            if (!string.IsNullOrEmpty(name))
            {
                Powerbanks = Powerbanks.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(aviability) && aviability != "Все")
            {
                Powerbanks = Powerbanks.Where(p => p.Availability == aviability);
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    Powerbanks = Powerbanks.OrderByDescending(s => s.Name);
                    break;
                case SortState.PriceAsc:
                    Powerbanks = Powerbanks.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    Powerbanks = Powerbanks.OrderByDescending(s => s.Price);
                    break;
                case SortState.CapacityAsc:
                    Powerbanks = Powerbanks.OrderBy(s => s.Capacity);
                    break;
                case SortState.CapacityDesc:
                    Powerbanks = Powerbanks.OrderByDescending(s => s.Capacity);
                    break;
                default:
                    Powerbanks = Powerbanks.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            var count = await Powerbanks.CountAsync();
            var items = await Powerbanks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Powerbanks = items
            };
            return View(viewModel);
        }

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
