using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.Authentication
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "FullNameRequired")]
        [StringLength(80, ErrorMessage = "FullNameLength")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        
        [Required(ErrorMessage = "AddressRequired")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "AddressLength")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
    

        [Required(ErrorMessage = "PhoneNumberRequired")]
        [Phone(ErrorMessage = "CorrectPhone")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "DateBirthRequired")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime DateBirth { get; set; }


        [Required(ErrorMessage = "EmailRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "EmailLength")]
        [EmailAddress(ErrorMessage = "CorrectEmail")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "CheckEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; } 


        [Required(ErrorMessage = "PasswordlRequired")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{1,20}$", ErrorMessage = "CorrectPassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Required(ErrorMessage = "PasswordConfirmRequired")]
        [Compare("Password", ErrorMessage = "PasswordConfirmCompare")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}