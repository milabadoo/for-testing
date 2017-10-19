using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Domain.Entity;
namespace Library.Web.Models
{
    public class OrderIndexViewModel
    {
        public Order Order { get; set; }
        public string ReturnUrl { get; set; }
    }
}

 