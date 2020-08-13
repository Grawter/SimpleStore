using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.User
{
    public class UserChangePassViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан старый пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{1,20}$", ErrorMessage = "Пароль должен состоять минимум из 6 символов и " +
            "иметь хотя бы 1 букву из [a-z], 1 букву из [A-Z], одну цифру из [0-9] и один знак")]
        [Display(Name = " Новый пароль")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Не указан пароль для подтверждения")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string ConfirmNewPassword { get; set; }
    }
}
