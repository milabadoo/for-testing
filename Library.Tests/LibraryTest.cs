using FluentAssertions;
using Library.Domain.Abstract;
using Library.Domain.Entity;
using Library.Web.Controllers;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using System.Web.Mvc;
using Library.Web.Models;

namespace Library.Tests
{
    public class LibraryTest
    {
        [Fact]
        public void Can_Paginate()
        {
            // Организация (arrange)
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= ""},
                new Book { BookId = 2, Name = "Книга2", Category= ""},
                new Book { BookId = 3, Name = "Книга3", Category= ""},
                new Book { BookId = 4, Name = "Книга4", Category= ""},
                new Book { BookId = 5, Name = "Книга5", Category= ""}
            });

            var controller = new BookController(mock.Object)
            {
                pageSize = 3
            };

            // Действие (act)
            var result = ((BooksListViewModel)controller.List("", 2).Model).Books.ToList();

            // Утверждение (assert)
            result.Count.Should().Be(2);
            result[0].Name.Should().Be("Книга4");
            result[1].Name.Should().Be("Книга5");
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= ""},
                new Book { BookId = 2, Name = "Книга2", Category= ""},
                new Book { BookId = 3, Name = "Книга3", Category= ""},
                new Book { BookId = 4, Name = "Книга4", Category= ""},
                new Book { BookId = 5, Name = "Книга5", Category= ""}
            });
            var controller = new BookController(mock.Object)
            {
                pageSize = 3
            };

            // Act
            var result = ((BooksListViewModel)controller.List("", 2).Model).PagingInfo;

            // Assert
            result.CurrentPage.Should().Be(2);
            result.ItemsPerPage.Should().Be(3);
            result.TotalItems.Should().Be(5);
            result.TotalPages.Should().Be(2);
        }

        [Fact]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= "Cat1"},
                new Book { BookId = 2, Name = "Книга2", Category= "Cat2"},
                new Book { BookId = 3, Name = "Книга3", Category= "Cat1"},
                new Book { BookId = 4, Name = "Книга4", Category= "Cat2"},
                new Book { BookId = 5, Name = "Книга5", Category= "Cat3"}
            });

            var controller = new BookController(mock.Object)
            {
                pageSize = 3
            };

            // Action
            var result = ((BooksListViewModel)controller.List("Cat2", 1).Model).Books.ToList();

            // Assert
            result.Count.Should().Be(2);
            result[0].Name.Should().Be("Книга2");
            result[1].Name.Should().Be("Книга4");
        }

        [Fact]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= "Cat1"},
                new Book { BookId = 2, Name = "Книга2", Category= "Cat2"},
                new Book { BookId = 3, Name = "Книга3", Category= "Cat1"},
                new Book { BookId = 4, Name = "Книга4", Category= "Cat2"},
                new Book { BookId = 5, Name = "Книга5", Category= "Cat3"}
            });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            results.Count().Should().Be(3);
            results[0].Should().Be("Cat1");
            results[1].Should().Be("Cat2");
            results[2].Should().Be("Cat3");
        }

        [Fact]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= "Cat1"},
                new Book { BookId = 2, Name = "Книга2", Category= "Cat2"},
                new Book { BookId = 3, Name = "Книга3", Category= "Cat1"},
                new Book { BookId = 4, Name = "Книга4", Category= "Cat2"},
                new Book { BookId = 5, Name = "Книга5", Category= "Cat3"}
            });

            var controller = new BookController(mock.Object)
            {
                pageSize = 3
            };

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((BooksListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((BooksListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((BooksListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            res1.Should().Be(2);
            res2.Should().Be(2);
            res3.Should().Be(1);
            resAll.Should().Be(5);
        }

        [Fact]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category = "Cat1" },
            }.AsQueryable());

            var mockProcessor = new Mock<IOrderProcessor>();

            // Организация - создание корзины
            var cart = new Order();

            // Организация - создание контроллера
            var controller = new OrderController(mock.Object, mockProcessor.Object);

            // Действие - добавить игру в корзину
            controller.AddToOrder(cart, 1, null);

            // Утверждение
            cart.LineCollection.Count().Should().Be(1);
            cart.LineCollection.ToList()[0].Book.BookId.Should().Be(1);
        }

        [Fact]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category = "Cat1" },
            }.AsQueryable());

            var mockProcessor = new Mock<IOrderProcessor>();

            // Организация - создание корзины
            var cart = new Order();

            // Организация - создание контроллера
            var controller = new OrderController(mock.Object, mockProcessor.Object);

            // Действие - добавить игру в корзину
            var result = controller.AddToOrder(cart, 2, "myUrl");

            // Утверждение
            result.RouteValues["action"].Should().Be("Index");
            result.RouteValues["returnUrl"].Should().Be("myUrl");
        }

        [Fact]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            var cart = new Order();

            // Организация - создание контроллера
            var target = new OrderController(null, null);

            // Действие - вызов метода действия Index()
            var result = (OrderIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            result.Order.Should().Be(cart);
            result.ReturnUrl.Should().Be("myUrl");
        }
    }
}