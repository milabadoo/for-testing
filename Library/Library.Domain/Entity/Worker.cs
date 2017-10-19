using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entity
{
    class Worker
    {
        [Key]
        public int WorkerId { get; set; }
        public int Login { get; set; }
       
        public string Password { get; set; }
        
    }
}
