using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Library.Domain.Entity;
using Library.Domain.Abstract;

namespace Library.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Book> Books
        {
            get { return context.Books; }
        }

        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else
            {
                var dbEntry = context.Books.FirstOrDefault(b => b.BookId == book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Author = book.Author;
                    dbEntry.Publishing = book.Publishing;
                    dbEntry.Year = book.Year;
                    //dbEntry.Rating = book.Rating;

                    dbEntry.Description = book.Description;

                    dbEntry.Category = book.Category;
                    dbEntry.ImageData = book.ImageData;
                    dbEntry.ImageMimeType = book.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public Book DeleteBook(int bookId)
        {
            var dbEntry = context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        
    }
}
