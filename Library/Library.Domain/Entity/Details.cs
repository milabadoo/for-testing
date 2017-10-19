using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entity
{
    public class Details
    {
        [Key]
        public int DetailId { get; set; }
        public Order Orders { get; set; }

        [Required(ErrorMessage = "Укажите как вас зовут")]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите номер читательского билета")]
        [Display(Name = "Читательский билет")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Удобная дата получения книги")]
        [Display(Name = "Дата")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Время получения")]
        [Display(Name = "Время")]
        public string Time { get; set; }

    }
}
