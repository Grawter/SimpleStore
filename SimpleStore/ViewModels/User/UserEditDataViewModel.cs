using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.User
{
    public class UserEditDataViewModel
    {
        public string Id { get; set; }


        [Required(ErrorMessage = "FullNameRequired")]
        [StringLength(80, ErrorMessage = "FullNameLength")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "AddressRequired")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "AddressLength")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }


        [Required(ErrorMessage = "PhoneNumberRequired")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "DateBirthRequired")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime DateBirth { get; set; }
    }
}