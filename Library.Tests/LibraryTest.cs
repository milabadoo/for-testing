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
        [Theory]
        [InlineData(1, "Книга2")]
        [InlineData(2, "Книга3")]
        [InlineData(3, "Книга4")]
        [InlineData(4, "Книга5")]
        public void Can_Paginate(int value, string valuestr)
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
                pageSize = value
            };

            // Действие (act)
            var result = ((BooksListViewModel)controller.List("", 2).Model).Books.ToList();

            // Утверждение (assert)
           // result.Count.Should().Be(2);
            result[0].Name.Should().Be(valuestr);
            //result[1].Name.Should().Be(valuestr);
        }

        [Theory]
        [InlineData(1, 3, 5)]
        [InlineData(2, 3, 3)]
        [InlineData(3, 1, 2)]
        [InlineData(4, 2, 2)]
        public void Can_Send_Pagination_View_Model(int value, int number, int total)
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
                pageSize = value
            };

            // Act
            var result = ((BooksListViewModel)controller.List("", number).Model).PagingInfo;

            // Assert
            result.CurrentPage.Should().Be(number);
            result.ItemsPerPage.Should().Be(value);
            result.TotalItems.Should().Be(5);
            result.TotalPages.Should().Be(total);
        }

        [Theory]
        [InlineData(5, "Category1", "Книга1", "Книга3")]
        [InlineData(2, "Category2", "Книга2", "Книга4")]
        [InlineData(3, "Category3", "Книга5", "Книга6")]
        [InlineData(4, "Category1", "Книга1", "Книга3")]
        public void Can_Filter_Games(int value, string valuestr, string first, string second)
        {
            // Организация (arrange)
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= "Category1"},
                new Book { BookId = 2, Name = "Книга2", Category= "Category2"},
                new Book { BookId = 3, Name = "Книга3", Category= "Category1"},
                new Book { BookId = 4, Name = "Книга4", Category= "Category2"},
                new Book { BookId = 5, Name = "Книга5", Category= "Category3"},
                new Book { BookId = 6, Name = "Книга6", Category= "Category3"}
            });

            var controller = new BookController(mock.Object)
            {
                pageSize = value
            };

            // Action
            var result = ((BooksListViewModel)controller.List(valuestr, 1).Model).Books.ToList();

            // Assert
            result.Count.Should().Be(2);
            result[0].Name.Should().Be(first);
            result[1].Name.Should().Be(second);
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

        [Theory]
        [InlineData("Category1", 2)]
        [InlineData("Category2", 3)]
        [InlineData("Category3", 1)]
        [InlineData(null, 6)]
        public void Generate_Category_Specific_Game_Count(string cat, int value)
        {
            /// Организация (arrange)
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category= "Category1"},
                new Book { BookId = 2, Name = "Книга2", Category= "Category2"},
                new Book { BookId = 3, Name = "Книга3", Category= "Category1"},
                new Book { BookId = 4, Name = "Книга4", Category= "Category2"},
                new Book { BookId = 5, Name = "Книга5", Category= "Category3"},
                new Book { BookId = 5, Name = "Книга5", Category= "Category2"}
            });

            var controller = new BookController(mock.Object)
            {
                pageSize = 3
            };

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((BooksListViewModel)controller.List(cat).Model).PagingInfo.TotalItems;
           
            //int resAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            res1.Should().Be(value);
            
            //resAll.Should().Be(5);
        }

        [Theory]
        
        [InlineData(1, 1)]
        
        
        public void Can_Add_To_Cart(int val, int kol)
        {
            // Организация - создание имитированного хранилища
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category = "Cat1" },
                 new Book { BookId = 2, Name = "Книга2", Category = "Cat2" },
            }.AsQueryable());

            var mockProcessor = new Mock<IOrderProcessor>();

            // Организация - создание корзины
            var cart = new Order();

            // Организация - создание контроллера
            var controller = new OrderController(mock.Object, mockProcessor.Object);

            // Действие - добавить игру в корзину
            controller.AddToOrder(cart, val, null);

            // Утверждение
            cart.LineCollection.Count().Should().Be(kol);
            cart.LineCollection.ToList()[0].Book.BookId.Should().Be(kol);
        }
        
        /*
        [Fact]
        public void Can_Add_To_Cart2()
        {
             //Организация - создание имитированного хранилища
            var mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1", Category = "Cat1" },
                 new Book { BookId = 2, Name = "Книга2", Category = "Cat2" },
            }.AsQueryable());
            

            //var mockProcessor = new Mock<IOrderProcessor>();

            // Организация - создание корзины
            var cart = new Order();

            // Организация - создание контроллера
            var controller = new OrderController(mock.Object, mockProcessor.Object);

            // Действие - добавить игру в корзину
            controller.AddToOrder(cart, val, null);

            // Утверждение
            cart.LineCollection.Count().Should().Be(kol);
            cart.LineCollection.ToList()[0].Book.BookId.Should().Be(kol);
        }
        */

        [Theory]
        [InlineData("action", "Index")]
        [InlineData("returnUrl", "myUrl")]
        //[InlineData(1, 1)]
        
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen(string f, string s)
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
            result.RouteValues[f].Should().Be(s);
            result.RouteValues[f].Should().Be(s);
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

        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            var mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            var cart = new Order();

            // Организация - создание деталей о доставке
            var shippingDetails = new Details();

            // Организация - создание контроллера
            var controller = new OrderController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Order>(), It.IsAny<Details>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            result.ViewName.Should().Be("");
            // Утверждение - проверка, что-представлению передана неверная модель
            result.ViewData.ModelState.IsValid.Should().Be(false);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            var mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            var cart = new Order();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            var controller = new OrderController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new Details());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Order>(), It.IsAny<Details>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            result.ViewName.Should().Be("");

            // Утверждение - проверка, что-представлению передана неверная модель
            result.ViewData.ModelState.IsValid.Should().Be(false);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            var mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            var cart = new Order();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            var controller = new OrderController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new Details());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Order>(), It.IsAny<Details>()), Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            result.ViewName.Should().Be("Completed");

            // Утверждение - проверка, что представлению передается допустимая модель
            result.ViewData.ModelState.IsValid.Should().Be(true);
        }
        
    }
}