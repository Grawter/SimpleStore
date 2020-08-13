using System.ComponentModel.DataAnnotations;

namespace SimpleStore.Models.Shop
{
    public class Phone
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Не указано название товара")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина названия должна быть от 1 до 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана компания производитель")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина названия должна быть от 1 до 50 символов")]
        [Display(Name = "Производитель")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Не указана компания производитель")]
        [Range(1, 999999999, ErrorMessage = "Допустимые значения 1-31")]
        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Не указано наличие товара")]
        [Display(Name = "Наличие товара")]
        public string Availability { get; set; }

    }
}
