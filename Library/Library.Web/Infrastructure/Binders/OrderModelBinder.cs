using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Library.Domain.Entity;
using System.Web.Mvc;

namespace Library.Web.Infrastructure.Binders
{
    public class OrderModelBinder : IModelBinder
    {
        private const string sessionKey = "Order";

        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            // Получить объект Cart из сеанса
            Order order = null;
            if (controllerContext.HttpContext.Session != null)
            {
                order = (Order)controllerContext.HttpContext.Session[sessionKey];
            }

            // Создать объект Cart если он не обнаружен в сеансе
            if (order == null)
            {
                order = new Order();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = order;
                }
            }

            // Возвратить объект Cart
            return order;

        }
    }
}