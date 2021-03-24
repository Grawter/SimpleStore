using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.Booking
{
    public class OrderViewModel
    {
        public int Id { get; set; } // id заказа


        [Required(ErrorMessage = "Не указан Email")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина эл. адреса должна быть от 3 до 50 символов")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Display(Name = "Email")]
        public string UserEmail { get; set; } // email заказчика


        [Required(ErrorMessage = "Не указан телефон")]
        [Phone(ErrorMessage = "Некорректный телефон")]
        [Display(Name = "Номер телефона")]
        public string UserPhone { get; set; } // телефон заказчика


        [Required(ErrorMessage = "Не указано ФИО")]
        [StringLength(50, ErrorMessage = "Длина ФИО должна быть до 50 символов")]
        [Display(Name = "ФИО")]
        public string UserFullName { get; set; } // фамилия заказчика


        [Required(ErrorMessage = "Не указан адрес")]
        [StringLength(150, MinimumLength = 10, ErrorMessage = "Длина адреса должна быть от 10 до 150 символов")]
        [Display(Name = "Адрес")]
        public string UserAddress { get; set; } // адрес заказчика


        [Required(ErrorMessage = "Не указан статус")]
        [Display(Name = "Статус")]
        public string Status { get; set; }  // статус заказа
    }
}