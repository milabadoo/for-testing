using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library.Web.Models
{
    public class ModelBook
    {

  
         public int BookId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publishing { get; set; }
        public int Year { get; set; }
        public int Rating { get; set; }

        public string Description { get; set; }
        public string Category { get; set; }
}

}
