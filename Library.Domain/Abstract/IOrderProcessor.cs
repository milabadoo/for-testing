using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Domain.Entity;

namespace Library.Domain.Abstract
{
    
        public interface IOrderProcessor
        {
            void ProcessOrder(Order order, Details details);
        }
   
}
