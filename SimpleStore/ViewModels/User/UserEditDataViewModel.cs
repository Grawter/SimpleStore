﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.User
{
    public class UserEditDataViewModel
    {
        public string Id { get; set; }


        [Required(ErrorMessage = "Не указано ФИО")]
        [StringLength(80, ErrorMessage = "Длина ФИО должна быть от 1 до 50 символов")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "Не указан адрес")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Длина адреса должна быть от 1 до 150 символов")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Не указан телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Не указан день рождения")]
        [Range(1, 31, ErrorMessage = "Допустимые значения 1-31")]
        [Display(Name = "День рождения")]
        public int Day { get; set; }


        [Required(ErrorMessage = "Не указан месяц рождения")]
        [Range(1, 12, ErrorMessage = "Допустимые значения 1-12")]
        [Display(Name = "Месяц рождения")]
        public int Mount { get; set; }


        [Required(ErrorMessage = "Не указан год рождения")]
        [Range(1900, 2002, ErrorMessage = "Допустимые значения 1900-2002")]
        [Display(Name = "Год рождения")]
        public int Year { get; set; }
    }
}
