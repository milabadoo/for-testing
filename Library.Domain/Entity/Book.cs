using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entity
{
    public class Book
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int BookId { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Пожалуйста, введите название книги")]
        public string Name { get; set; }

        [Display(Name = "Автор")]
        [Required(ErrorMessage = "Пожалуйста, введите автора")]
        public string Author { get; set; }

        [Display(Name = "Издательство")]
        [Required(ErrorMessage = "Пожалуйста, введите название издательства")]
        public string Publishing { get; set; }

        [Display(Name = "Год издания")]
        [Required(ErrorMessage = "Пожалуйста, введите год издания")]
        public int Year { get; set; }

        //[Required(ErrorMessage = "Пожалуйста, введите рейтинг")]
        public Rating Rating { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Пожалуйста, введите описание")]
        public string Description { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Category { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
