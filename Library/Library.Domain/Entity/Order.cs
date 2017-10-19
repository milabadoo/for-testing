using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Library.Domain.Entity
{
    public class Order
    {
        public Order()
        {
            LineCollection = new List<OrderLine>();
        }

        [Key]
        public int OrderId { get; set; }

        public List<OrderLine> LineCollection { get; private set; }

        public void AddItem(Book book, int quantity)
        {
            var line = LineCollection
                .Where(g => g.Book.BookId == book.BookId)
                .FirstOrDefault();

            if (line == null)
            {
                LineCollection.Add(new OrderLine
                {
                    Book = book,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Book book)
        {
            LineCollection.RemoveAll(l => l.Book.BookId == book.BookId);
        }

        public void Clear()
        {
            LineCollection.Clear();
        }
    }

    public class OrderLine
    {
        [Key]
        public int OrderLineId { get; set; }
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}