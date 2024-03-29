﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using SimpleStore.Models;
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

        public async Task<IActionResult> Index()
        {
            List<Novelty> News = await db.News.ToListAsync();
            if (News != null)
            {
                var LastNews = News.LastOrDefault();
                ViewBag.LastNews = LastNews;
            }
            return View(await _userManager.Users.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Product(string type, string name, string aviability, int page = 1, SortState sortOrder = SortState.NameAsc)
        {
            ViewBag.type = type; // передача типа товара в представление
            ViewBag.Users = _userManager.Users.ToList(); // передача списка пользователей в представление

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
            int pageSize = 20; // кол-во отображаемых объектов на одной странице
            var count = await Products.CountAsync(); // общее кол-во объектов
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

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpGet]
        [ActionName("Buy")]
        public async Task<IActionResult> ConfirmBuy(int? ProductId, string UserId, int ProductCount, string preorder ="false")
        {
            if (ProductId != null)
            {
                Product product = await db.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
                if (product != null)
                {
                    ViewBag.preorder = preorder;
                    ViewBag.ProductId = ProductId;
                    ViewBag.UserId = UserId;
                    ViewBag.Count = ProductCount;
                    return View(product);
                }
            }
            return NotFound("Не найдено");
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        [HttpPost]
        public async Task<IActionResult> Buy([FromForm] int? ProductId, string UserId, int Count)
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
                        UserFullName = user.FullName,
                        UserAddress = user.Address,
                        Status = "На рассмотрении"
                    };

                    db.Orders.Add(order);
                    await db.SaveChangesAsync();
                    return RedirectToAction("MyOrders", "Home", new { UserId = UserId });
                }
            }
            return NotFound("Не найдено");
        }

        public IActionResult UnknownUser(string FullPath = null)
        {
            ViewBag.FullPath = FullPath;
            return View();
        }

    }
}