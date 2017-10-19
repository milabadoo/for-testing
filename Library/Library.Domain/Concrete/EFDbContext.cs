using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Domain.Entity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Library.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Details> Details { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}
