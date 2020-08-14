using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace SimpleStore.Models.Shop
{
    public class Powerbank
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Не указано название товара")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Длина названия должна быть от 1 до 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Не указана цена товара")]
        [Range(1, 999999999, ErrorMessage = "Допустимые значения 1-999999999")]
        [Display(Name = "Цена")]
        public double Price { get; set; }


        [Required(ErrorMessage = "Не указана электрическая ёмкость заряда товара")]
        [Display(Name = "Электрическая ёмкость заряда")]
        public double Capacity { get; set; }


        [Required(ErrorMessage = "Не указано наличие товара")]
        [Display(Name = "Наличие товара")]
        public string Availability { get; set; }


        [NotMapped]
        [Display(Name = "Изображение")]
        public IFormFile File { get; set; }


        public byte[] Image { get; set; }
    }
}
