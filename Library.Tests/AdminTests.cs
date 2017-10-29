using FluentAssertions;
using Library.Domain.Abstract;
using Library.Domain.Entity;
using Library.Web.Controllers;
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
        [Fact]
        public void Index_Contains_All_Games()
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
            var result = ((IEnumerable<Book>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            result.Count().Should().Be(5);
            result[0].Name.Should().Be("Книга1");
            result[1].Name.Should().Be("Книга2");
            result[2].Name.Should().Be("Книга3");
        }

        [Fact]
        public void Can_Edit_Game()
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
            var book1 = controller.Edit(1).ViewData.Model as Book;
            var book2 = controller.Edit(2).ViewData.Model as Book;
            var book3 = controller.Edit(3).ViewData.Model as Book;

            // Assert
            book1.BookId.Should().Be(1);
            book2.BookId.Should().Be(2);
            book3.BookId.Should().Be(3);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            var mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            var book = new Book { Name = "Test" };

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(book);

            // Утверждение - проверка того, что к хранилищу производится обращение
            mock.Verify(m => m.SaveBook(book));

            // Утверждение - проверка типа результата метода
            result.Should().NotBeOfType(typeof(ViewResult));
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Организация - создание имитированного хранилища данных
            var mock = new Mock<IBookRepository>();

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Организация - создание объекта Game
            var game = new Book { Name = "Test" };

            // Организация - добавление ошибки в состояние модели
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка сохранения товара
            ActionResult result = controller.Edit(game);

            // Утверждение - проверка того, что обращение к хранилищу НЕ производится 
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());

            // Утверждение - проверка типа результата метода
            result.Should().BeOfType(typeof(ViewResult));
        }
    }
}
