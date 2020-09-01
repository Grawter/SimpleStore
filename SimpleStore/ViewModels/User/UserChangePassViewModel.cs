using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.User
{
    public class UserChangePassViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "OldPasswordRequired")]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "NewPasswordRequired")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9]).{1,20}$", ErrorMessage = "CorrectPassword")]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "ConfirmNewPasswordRequired")]
        [Compare("NewPassword", ErrorMessage = "PasswordConfirmCompare")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string ConfirmNewPassword { get; set; }
    }
}