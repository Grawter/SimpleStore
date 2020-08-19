using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Models.Booking
{
    public class Order
    {
        public int Id { get; set; } // id заказа
        public int ProductId { get; set; } // id товара
        public string UserId { get; set; } // id пользователя
        public string ProductName { get; set; } // название товара
        public int ProductCount { get; set; } // количество товара
        public double ProductPrice { get; set; } // цена в итоге
        public string UserEmail { get; set; } // email заказчика
        public string UserPhone { get; set; } // phone заказчика
        public string UserSurname { get; set; } // фамилия заказчика
        public string UserName { get; set; } // имя заказчика
        public string UserAddress { get; set; } // адрес заказчика
        public string Status { get; set; }  // статус заказа
    }
}
