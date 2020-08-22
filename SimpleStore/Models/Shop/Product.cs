using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace SimpleStore.Models.Shop
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указан тип товара")]
        [Display(Name = "Тип товара")]
        public string Type { get; set; }


        [Required(ErrorMessage = "Не указано название товара")]
        [StringLength(50, ErrorMessage = "Длина названия должна быть не больше 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; }


        [StringLength(50, ErrorMessage = "Длина названия должна быть не больше 50 символов")]
        [Display(Name = "Производитель (для телефонов)")]
        public string Company { get; set; }


        [Range(1, 999999999, ErrorMessage = "Допустимые значения цены от 1 до 999999999 руб")]
        [Display(Name = "Электрическая ёмкость заряда (Для powerbank)")]
        public double? Capacity { get; set; }


        [Required(ErrorMessage = "Не указано описание товара")]
        [StringLength(200, ErrorMessage = "Длина описание не должна быть больше 200 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Не указана цена товара")]
        [Range(1, 999999999, ErrorMessage = "Допустимые значения цены от 1 до 999999999 руб")]
        [Display(Name = "Цена")]
        public double Price { get; set; }


        [Required(ErrorMessage = "Не указано наличие товара")]
        [Display(Name = "Наличие товара")]
        public string Availability { get; set; }


        [NotMapped]
        [Display(Name = "Изображение")]
        public IFormFile File { get; set; }


        public byte[] Image { get; set; }
    }
}
