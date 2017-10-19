using Library.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library.Domain.Entity;
using Library.Web.Models;
using Library.Web.HtmlHelpers;


namespace Library.Web.Controllers
{
    public class BookController : Controller
    {
        //
        // GET: /Book/

        private IBookRepository repository;
        public int pageSize = 3;
        public BookController(IBookRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(string category, int page = 1)
        {
            var books = repository.Books
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(book => book.BookId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);

            var booksCount = category == null ?
                    repository.Books.Count() :
                    repository.Books.Count(book => book.Category == category);

            var pageInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = booksCount
            };

            var model = new BooksListViewModel
            {
                Books = books,
                PagingInfo = pageInfo,
                CurrentCategory = category
            };
            return View(model);
        }
   /*     public ViewResult About (Book book, HttpPostedFileBase image = null)
        {
            var books = repository.Books;           

            var model = new BooksListViewModel
            {
                Books = books,
              
            };
            return View(model);
           
           
        }*/

        public FileContentResult GetImage(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                return File(book.ImageData, book.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
	}

    }

