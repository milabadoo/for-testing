using Library.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Library.Domain.Abstract
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }

        void SaveBook(Book book);
        Book DeleteBook(int bookId);
    }
}
