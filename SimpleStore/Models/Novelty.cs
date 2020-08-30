using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleStore.Models
{
    public class Novelty
    {
        public int Id { get; set; } // id новости


        [Required(ErrorMessage = "Не указан заголовок")]
        [StringLength(70, ErrorMessage = "Длина заголовка должна быть не больше 70 символов")]
        [Display(Name = "Заголовок")]
        public string Title { get; set; } // заголов


        [Required(ErrorMessage = "Не указан текст")]
        [StringLength(200, ErrorMessage = "Длина заголовка должна быть не больше 200 символов")]
        [Display(Name = "Текст")]


        public string Message { get; set; } // Содержание
        

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } // дата публикации
    }
}
