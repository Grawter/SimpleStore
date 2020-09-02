using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.User
{
    public class UserChangeEmailViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "EmailLength")]
        [EmailAddress(ErrorMessage = "EmailCorrect")]
        [Remote(action: "CheckEmail", controller: "Account", ErrorMessage = "CheckEmail")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}