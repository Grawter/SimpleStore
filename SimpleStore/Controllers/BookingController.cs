using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SimpleStore.Models;
using SimpleStore.ViewModels.Supporting_tools;
using SimpleStore.ViewModels.Booking;

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
            int pageSize = 40;
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
                OrderViewModel orderViewModel = new OrderViewModel
                {
                    Id = order.Id,
                    UserEmail = order.UserEmail,
                    UserPhone = order.UserPhone,
                    UserFullName = order.UserFullName,
                    UserAddress = order.UserFullName,
                    Status = order.Status
                };

                return View(orderViewModel);
            }
            return NotFound("Не найдено");
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Order order = await db.Orders.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (order != null)
                {
                    order.UserEmail = model.UserEmail;
                    order.UserPhone = model.UserPhone;
                    order.UserFullName = model.UserFullName;
                    order.UserAddress = model.UserAddress;
                    order.Status = model.Status;

                    db.Orders.Update(order);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Не найдено");
                }
            }
            return View(model);
        }

    }
}
