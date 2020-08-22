using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.User
{
    public class UserChangeEmailViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Не указан Email")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина эл. адреса должна быть от 3 до 50 символов")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email уже используется")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}