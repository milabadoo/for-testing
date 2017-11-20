using FluentAssertions;
using Library.Domain.Abstract;
using Library.Domain.Concrete;
using Library.Domain.Entity;
using Library.Web.Controllers;
using Library.Web.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Xunit;

namespace Library.Tests
{

    public class AdminTests
    {

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Count_Books(int value)
        {
            var t = new EFDbContext();
            var books = t.Books.Count();
            books.Should().NotBe(value);
            books.Should().Be(11);
        }


        [Fact]
       // [InlineData("Oxxxy")]
       // [InlineData("Оруэл")]
       // [InlineData("Замятиг")]
        
        public void Add_Book()
        {
            var t = new EFDbContext();
            var count = t.Books.Count();
            var b = new Book
            {
                BookId = 17,
                Author = "Oxxxy",
                Name = "Gorgorod",
                Publishing = "BookingMashine",
                Year = 2015,
                Description = "Любовная история посреди антиутопии."

                
                
            };

            t.Books.Add(b);
            t.SaveChanges();            
            var result = t.Books.Count();          
            result.Should().Be(count +1);
        }
        

       [Fact]
        public void Delete_Book()
        {
            var t = new EFDbContext();
            var r = new EFBookRepository();
            //var b = new Book;
            var count = t.Books.Count();
            var books = t.Books.ToList();
            var id = books.LastOrDefault();

            if (id != null)
                r.DeleteBook(id.BookId);

            t.SaveChanges();

            var result = t.Books.Count();
            result.Should().Be(count - 1);
        }
       

        [Theory]
        [InlineData(5, "Зарубежная проза", "1984", "О дивный новый мир", 5)]
        [InlineData(2, "Российская проза", "Отцы и дети", "Война и мир. Том 1-2", 2)]
        [InlineData(2, "Зарубежная проза", "1984", "О дивный новый мир", 2)]
       

        public void CanSort(int value, string valuestr, string first, string second, int kol)
        {
            var t = new EFDbContext();
            //private IKernel kernel;
            var r = new EFBookRepository();
            var controller = new BookController (r)
            {
                pageSize = value
            };

            // Action
            var result = ((BooksListViewModel)controller.List(valuestr, 1).Model).Books.ToList();

            // Assert
            result.Count.Should().Be(kol);
            result[0].Name.Should().Be(first);
            result[1].Name.Should().Be(second);
           
        }

        

        [Theory]
        [InlineData(1, "Джордж Оруэлл")]
        [InlineData(2, "Олдос Хаксли")]
        [InlineData(3, "Чак Паланик")]

        public void FindName(int numb, string str)
        {
            var t = new EFDbContext();
     
            var result = t.Books.Find(numb).Author;
            result.Should().Be(str);
        }

        [Theory]
        [InlineData(2011, "Neoclassic")]
        [InlineData(2016, "Эксмо")]
        [InlineData(2006, "АСТ")]

        public void FindPubl(int numb, string str)
        {
            var t = new EFDbContext();
            var result = 0;
            for (int i = 1; i < 10; i++)
                if (t.Books.Find(i).Publishing == str)
                    result = t.Books.Find(i).Year;
            result.Should().Be(numb);
        }


        [Theory]
        [InlineData("Книга")]
        [InlineData(null)]
        [InlineData("111")]
        [InlineData("returnUrl")]
        public void Index_Contains_All_Games(string name)
        {
            // Организация - создание имитированного хранилища данных
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = name},
                new Book { BookId = 2, Name = "Книга2"},
                new Book { BookId = 3, Name = "Книга3"},
                new Book { BookId = 4, Name = "Книга4"},
                new Book { BookId = 5, Name = "Книга5"}
            });

            // Организация - создание контроллера
            var controller = new AdminController(mock.Object);

            // Действие
            var result = ((IEnumerable<Book>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            result.Count().Should().Be(5);
            result[0].Name.Should().Be(name);
            result[1].Name.Should().Be("Книга2");
            result[2].Name.Should().Be("Книга3");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        //[InlineData("returnUrl")]
        public void Can_Edit_Game(int value)
        {
            // Организация - создание имитированного хранилища данных
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                 new Book { BookId = 1, Name = "Книга1"},
                new Book { BookId = 2, Name = "Книга2"},
                new Book { BookId = 3, Name = "Книга3"},
                new Book { BookId = 4, Name = "Книга4"},
                new Book { BookId = 5, Name = "Книга5"}
            });

            // Организация - создание контроллера
            var controller = new AdminController(mock.Object);

            // Действие
            var book1 = controller.Edit(value).ViewData.Model as Book;
          
            // Assert
            book1.BookId.Should().Be(value);
           
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("TestAgain")]
        public void Can_Save_Valid_Changes(String str)
        {
            // Организация - создание имитированного хранилища данных
            var mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            var book = new Book { Name = str };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveBook(book));

            // Утверждение - проверка типа результата метода
            result.Should().NotBeOfType(typeof(ViewResult));
        }

        [Theory]
        [InlineData("Test")]
        [InlineData("TestAgain")]
        public void Cannot_Save_Invalid_Changes(String str)
        {
            // Организация - создание имитированного хранилища данных
            var mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            var book = new Book { Name = str };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());

            // Утверждение - проверка типа результата метода
            result.Should().BeOfType(typeof(ViewResult));
        }
    }
}
