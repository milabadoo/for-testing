using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Domain.Entity;

namespace Library.Web.Models
{
    public class BooksListViewModel
    {
            public IEnumerable<Book> Books { get; set; }
            public PagingInfo PagingInfo { get; set; }
            public string CurrentCategory { get; set; }
    }
}