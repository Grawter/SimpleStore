using System.ComponentModel.DataAnnotations;

namespace SimpleStore.ViewModels.Admin
{
    public class AdmChangePassViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите новый пароль")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{1,20}$", ErrorMessage = "Пароль должен состоять минимум из 6 символов и " +
            "иметь хотя бы 1 букву из [a-z], 1 букву из [A-Z], одну цифру из [0-9] и один знак")]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }
    }
}
