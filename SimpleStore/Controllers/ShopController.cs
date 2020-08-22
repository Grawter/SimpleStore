using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleStore.Models;
using SimpleStore.Models.Booking;
using SimpleStore.Models.Shop;
using SimpleStore.ViewModels.Supporting_tools;

namespace SimpleStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationContext db;
        private readonly UserManager<User> _userManager;
        public ShopController(ApplicationContext context, UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> Product(string type, string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {     
            ViewBag.type = type;
            ViewBag.Users = _userManager.Users.ToList();
            
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
        [ActionName("Buy")]
        public async Task<IActionResult> ConfirmBuy(int? ProductId, string UserId, int ProductCount)
        {
            if (ProductId != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                if (product != null)
                {
                    ViewBag.ProductId = ProductId;
                    ViewBag.UserId = UserId;
                    ViewBag.Count = ProductCount;
                    return View(product);
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Buy(int? ProductId, string UserId, int Count)
        {
            if (ProductId != null && UserId != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                User user = await _userManager.FindByIdAsync(UserId);
                if (product != null && user != null)
                {
                    Order order = new Order
                    {
                        ProductId = (int)ProductId,
                        UserId = UserId,
                        ProductName = product.Name,
                        ProductCount = Count,
                        ProductPrice = Count * product.Price,
                        UserEmail = user.Email,
                        UserPhone = user.PhoneNumber,
                        UserSurname = user.Surname,
                        UserName = user.Name,
                        UserAddress = user.Address,
                        Status = "На рассмотрении"
                    };

                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Case", new { type = product.Type });
                }
            }
            return NotFound();
        }

    }
}