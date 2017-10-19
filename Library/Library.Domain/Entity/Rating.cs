using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Library.Domain.Entity
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }
        public int RatingCount { get; set; }
    }
}
