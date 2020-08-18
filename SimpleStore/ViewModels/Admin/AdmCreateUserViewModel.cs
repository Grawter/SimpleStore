using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.Admin
{
    public class AdmCreateUserViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [Display(Name = "Имя")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Не указана фамилия")]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }


        [Required(ErrorMessage = "Не указан адрес")]
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


        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email уже используется")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Не указан пароль")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{1,20}$", ErrorMessage = "Пароль должен состоять минимум из 6 символов и " +
            "иметь хотя бы 1 букву из [a-z], 1 букву из [A-Z], одну цифру из [0-9]")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
    }
}