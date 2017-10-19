using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Library.Domain.Entity;
using Library.Domain.Abstract;
using Library.Web.Models;


namespace Library.Web.Controllers
{
    public class OrderController : Controller
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor;

        public OrderController(IBookRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
		
		public ViewResult Checkout() {
            return View(new Details());
        }

        [HttpPost]
        public ViewResult Checkout(Order order, Details details)
        {
            if (order.LineCollection.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(order, details);
                order.Clear();
                return View("Completed");
            }
            else
            {
                return View(details);
            }
        }
        public ViewResult Index(Order order, string returnUrl)
        {
            return View(new OrderIndexViewModel
            {
                Order = order,
                ReturnUrl = returnUrl
            });
        }
        
             

        public RedirectToRouteResult AddToOrder(Order order, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                order.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });

        
        }

        public RedirectToRouteResult RemoveFromOrder(Order order, int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                order.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Order GetOrder()
        {
            Order order = (Order)Session["Order"];
            if (order == null)
            {
                order = new Order();
                Session["Order"] = order;
            }
            return order;
        }

        public PartialViewResult Summary(Order order)
        {
            return PartialView(order);
        }

        
    }
}
