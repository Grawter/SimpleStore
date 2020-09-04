using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.Authentication
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "EmailRequired")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "PasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }


        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }


        public string FullPath { get; set; }
    }
}