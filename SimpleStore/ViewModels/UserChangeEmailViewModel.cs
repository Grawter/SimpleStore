using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels
{
    public class UserChangeEmailViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }


        [Required(ErrorMessage = "Не указан Email")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "Email уже используется")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}