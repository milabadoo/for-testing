using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Library.Domain.Entity
{
    class Client
    {
        [Key]
        public int ClientId { get; set; }
        public int AbonId { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Secondname { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string Sex { get; set; }

    }
}
