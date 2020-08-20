﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        
        [Required(ErrorMessage = "Не указан Email")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина эл. адреса должна быть от 3 до 50 символов")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email уже используется")]
        [Display(Name = "Email")]
        public string UserEmail { get; set; } // email заказчика


        [Required(ErrorMessage = "Не указан телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [Display(Name = "Номер телефона")]
        public string UserPhone { get; set; } // телефон заказчика


        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина фамилии должна быть от 1 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string UserSurname { get; set; } // фамилия заказчика


        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 50 символов")]
        [Display(Name = "Имя")]
        public string UserName { get; set; } // имя заказчика


        [Required(ErrorMessage = "Не указан адрес")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Длина адреса должна быть от 1 до 150 символов")]
        [Display(Name = "Адрес")]
        public string UserAddress { get; set; } // адрес заказчика



        [Required(ErrorMessage = "Не указан статус")]
        [Display(Name = "Статус")]
        public string Status { get; set; }  // статус заказа
    }
}