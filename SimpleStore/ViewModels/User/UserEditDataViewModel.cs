using System;
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
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Длина адреса должна быть от 10 до 150 символов")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Не указан телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime DateBirth { get; set; }
    }
}
