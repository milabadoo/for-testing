using Library.Domain.Concrete;
using Library.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Library.Domain.DAL
{
    public class EFDbContextInitializer : DropCreateDatabaseIfModelChanges<EFDbContext>
    {
        protected override void Seed(EFDbContext context)
        {
            //context.Books.Add(new Book
            //{

            //});
            //context.SaveChanges();
        }
    }
}


