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

        ///////////////// Section Case
        [HttpGet]
        public async Task<IActionResult> ShowCases(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            // фильтрация
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
            int pageSize = 5;
            var count = await Cases.CountAsync();
            var items = await Cases.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Cases = items
            };
            return View("~/Views/EditShop/Case/ShowCases.cshtml", indexviewModel);
        }

        [HttpGet]
        public IActionResult CreateCase() => View("~/Views/EditShop/Case/CreateCase.cshtml");
        // добавлена папка в модель

        [HttpPost]
        public async Task<IActionResult> CreateCase(Case casing)
        {
            if (casing.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(casing.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)casing.File.Length);
                }
                // установка массива байтов
                casing.Image = imageData;
            }

            db.Cases.Add(casing);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowCases");
        }

        [HttpGet]
        public async Task<IActionResult> EditCase(int? id)
        {
            if (id != null)
            {
                Case casing = await db.Cases.FirstOrDefaultAsync(p => p.Id == id);
                if (casing != null)
                    return View("~/Views/EditShop/Case/EditCase.cshtml", casing);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditCase(Case casing)
        {
            if (casing.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(casing.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)casing.File.Length);
                }
                // установка массива байтов
                casing.Image = imageData;
            }

            db.Cases.Update(casing);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowCases");
        }

        [HttpGet]
        [ActionName("DeleteCase")]
        public async Task<ActionResult> ConfirmDeleteCase(int? id)
        {
            if (id != null)
            {
                Case casing = await db.Cases.FirstOrDefaultAsync(p => p.Id == id);
                return View("~/Views/EditShop/Case/DeleteCase.cshtml", casing);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCase(int? id)
        {
            if (id != null)
            {
                Case casing = await db.Cases.FirstOrDefaultAsync(p => p.Id == id);
                if (casing != null)
                {
                    db.Cases.Remove(casing);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ShowCases");
                }
            }
            return NotFound();
        }

        ///////////////// Section Headphone
        [HttpGet]
        public async Task<IActionResult> ShowHeadphones(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
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
            int pageSize = 5;
            var count = await Headphones.CountAsync();
            var items = await Headphones.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Headphones = items
            };
            return View("~/Views/EditShop/Headphone/ShowHeadphones.cshtml", indexviewModel);
        }

        [HttpGet]
        public IActionResult CreateHeadphone() => View("~/Views/EditShop/Headphone/CreateHeadphone.cshtml");
        // добавлена папка в модель

        [HttpPost]
        public async Task<IActionResult> CreateHeadphone(Headphone headphone)
        {
            if (headphone.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(headphone.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)headphone.File.Length);
                }
                // установка массива байтов
                headphone.Image = imageData;
            }

            db.Headphones.Add(headphone);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowHeadphones");
        }

        [HttpGet]
        public async Task<IActionResult> EditHeadphone(int? id)
        {
            if (id != null)
            {
                Headphone headphone = await db.Headphones.FirstOrDefaultAsync(p => p.Id == id);
                if (headphone != null)
                    return View("~/Views/EditShop/Headphone/EditHeadphone.cshtml", headphone);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditHeadphone(Headphone headphone)
        {
            if (headphone.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(headphone.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)headphone.File.Length);
                }
                // установка массива байтов
                headphone.Image = imageData;
            }

            db.Headphones.Update(headphone);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowHeadphones");
        }

        [HttpGet]
        [ActionName("DeleteHeadphone")]
        public async Task<ActionResult> ConfirmDeleteHeadphone(int? id)
        {
            if (id != null)
            {
                Headphone headphone = await db.Headphones.FirstOrDefaultAsync(p => p.Id == id);
                return View("~/Views/EditShop/Headphone/DeleteHeadphone.cshtml", headphone);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteHeadphone(int? id)
        {
            if (id != null)
            {
                Headphone headphone = await db.Headphones.FirstOrDefaultAsync(p => p.Id == id);
                if (headphone != null)
                {
                    db.Headphones.Remove(headphone);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ShowHeadphones");
                }
            }
            return NotFound();
        }

        ///////////////// Section Phone
        [HttpGet]
        public async Task<IActionResult> ShowPhones(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
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
            int pageSize = 5;
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

        ///////////////// Section Powerbank
        [HttpGet]
        public async Task<IActionResult> ShowPowerbanks(string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
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
                case SortState.CapacityAsc:
                    Powerbanks = Powerbanks.OrderBy(s => s.Capacity);
                    break;
                case SortState.CapacityDesc:
                    Powerbanks = Powerbanks.OrderByDescending(s => s.Capacity);
                    break;
                case SortState.PriceAsc:
                    Powerbanks = Powerbanks.OrderBy(s => s.Price);
                    break;
                case SortState.PriceDesc:
                    Powerbanks = Powerbanks.OrderByDescending(s => s.Price);
                    break;
                default:
                    Powerbanks = Powerbanks.OrderBy(s => s.Name);
                    break;
            }

            // пагинация
            int pageSize = 5;
            var count = await Powerbanks.CountAsync();
            var items = await Powerbanks.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name, aviability),
                Powerbanks = items
            };
            return View("~/Views/EditShop/Powerbank/ShowPowerbanks.cshtml", indexviewModel);
        }

        [HttpGet]
        public IActionResult CreatePowerbank() => View("~/Views/EditShop/Powerbank/CreatePowerbank.cshtml"); // полный путь, т.к. для большей наглядности и сортировки в
        // добавлена папка в модель

        [HttpPost]
        public async Task<IActionResult> CreatePowerbank(Powerbank powerbank)
        {
            if (powerbank.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(powerbank.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)powerbank.File.Length);
                }
                // установка массива байтов
                powerbank.Image = imageData;
            }

            db.Powerbanks.Add(powerbank);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowPowerbanks"); // RedirectToAction по дефолту перебрасывает в метод контроллера, поэтому путь указвать не нужно
        }

        [HttpGet]
        public async Task<IActionResult> EditPowerbank(int? id)
        {
            if (id != null)
            {
                Powerbank powerbank = await db.Powerbanks.FirstOrDefaultAsync(p => p.Id == id);
                if (powerbank != null)
                    return View("~/Views/EditShop/Powerbank/EditPowerbank.cshtml", powerbank);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditPowerbank(Powerbank powerbank)
        {
            if (powerbank.File != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(powerbank.File.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)powerbank.File.Length);
                }
                // установка массива байтов
                powerbank.Image = imageData;
            }

            db.Powerbanks.Update(powerbank);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowPowerbanks");
        }

        [HttpGet]
        [ActionName("DeletePowerbank")]
        public async Task<ActionResult> ConfirmDeletePowerbank(int? id)
        {
            if (id != null)
            {
                Powerbank powerbank = await db.Powerbanks.FirstOrDefaultAsync(p => p.Id == id);
                return View("~/Views/EditShop/Powerbank/DeletePowerbank.cshtml", powerbank);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeletePowerbank(int? id)
        {
            if (id != null)
            {
                Powerbank powerbank = await db.Powerbanks.FirstOrDefaultAsync(p => p.Id == id);
                if (powerbank != null)
                {
                    db.Powerbanks.Remove(powerbank);
                    await db.SaveChangesAsync();
                    return RedirectToAction("ShowPowerbanks");
                }
            }
            return NotFound();
        }
    }
}