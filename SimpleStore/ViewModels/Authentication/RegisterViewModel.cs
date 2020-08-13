using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.Authentication
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина имени должна быть от 1 до 50 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "Не указана фамилия")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина фамилии должна быть от 1 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string SurName { get; set; }

        
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


        [Required(ErrorMessage = "Не указан Email")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина эл. адреса должна быть от 3 до 50 символов")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email уже используется")]
        [Display(Name = "Email")]
        public string Email { get; set; } 


        [Required(ErrorMessage = "Не указан пароль")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{1,20}$", ErrorMessage = "Пароль должен состоять минимум из 6 символов и " +
            "иметь хотя бы 1 букву из [a-z], 1 букву из [A-Z], одну цифру из [0-9] и один знак")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Не указан пароль для подтверждения")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}