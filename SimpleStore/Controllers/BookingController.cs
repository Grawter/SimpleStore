using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;
using SimpleStore.ViewModels.Supporting_tools;

namespace SimpleStore.Controllers
{
    [Authorize(Roles = "Admin, Moderator")]
    public class BookingController : Controller
    {
        private readonly ApplicationContext db;
        private readonly UserManager<User> _userManager;
        public BookingController(ApplicationContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index (int id, string email, string status, int page = 1, SortState sortOrder = SortState.IdDesc)
        {
            IQueryable<Order> orders = db.Orders;

            // Фильтрация
            if (id != 0)
            {
                orders = orders.Where(p => p.Id == id);
            }

            if (!string.IsNullOrEmpty(email))
            {
                orders = orders.Where(p => p.UserEmail.Contains(email));
            }

            if (!string.IsNullOrEmpty(status) && status != "Все")
            {
                orders = orders.Where(p => p.Status == status);
            }

            // Сортировка
            switch (sortOrder)
            {
                case SortState.IdAsc:
                    orders = orders.OrderBy(s => s.Id);
                    break;
                case SortState.IdProductAsc:
                    orders = orders.OrderBy(s => s.ProductId);
                    break;
                case SortState.IdProductDesc:
                    orders = orders.OrderByDescending(s => s.ProductId);
                    break;
                case SortState.NameAsc:
                    orders = orders.OrderBy(s => s.ProductName);
                    break;
                case SortState.NameDesc:
                    orders = orders.OrderByDescending(s => s.ProductName);
                    break;
                case SortState.CountAsc:
                    orders = orders.OrderBy(s => s.ProductCount);
                    break;
                case SortState.CountDesc:
                    orders = orders.OrderByDescending(s => s.ProductCount);
                    break;
                case SortState.PriceAsc:
                    orders = orders.OrderBy(s => s.ProductPrice);
                    break;
                case SortState.PriceDesc:
                    orders = orders.OrderByDescending(s => s.ProductPrice);
                    break;
                case SortState.NamesAsc:
                    orders = orders.OrderBy(s => s.UserFullName);
                    break;
                case SortState.NamesDesc:
                    orders = orders.OrderByDescending(s => s.UserFullName);
                    break;
                case SortState.EmailAsc:
                    orders = orders.OrderBy(s => s.UserEmail);
                    break;
                case SortState.EmailDesc:
                    orders = orders.OrderByDescending(s => s.UserEmail);
                    break;
                case SortState.PhoneAsc:
                    orders = orders.OrderBy(s => s.UserPhone);
                    break;
                case SortState.PhoneDesc:
                    orders = orders.OrderByDescending(s => s.UserPhone);
                    break;
                case SortState.AddressAsc:
                    orders = orders.OrderBy(s => s.UserAddress);
                    break;
                case SortState.AddressDesc:
                    orders = orders.OrderByDescending(s => s.UserAddress);
                    break;
                case SortState.StatusAsc:
                    orders = orders.OrderBy(s => s.Status);
                    break;
                case SortState.StatusDesc:
                    orders = orders.OrderByDescending(s => s.Status);
                    break;
                default:
                    orders = orders.OrderByDescending(s => s.Id);
                    break;
            }

            // пагинация
            int pageSize = 7;
            var count = await orders.CountAsync();
            var items = await orders.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel indexviewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(id, email, status),
                Orders = items
            };
            return View(indexviewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Order order = await db.Orders.FirstOrDefaultAsync(p => p.Id == id);

            if (order != null)
            {
                return View(order);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Order order)
        {
            if (order != null)
            {
                db.Orders.Update(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
