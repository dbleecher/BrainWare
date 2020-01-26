using System;
using System.Collections.Generic;

namespace Web.Models
{
    using System.Security.AccessControl;

    /// <summary>
    /// class Order - class provides top-level order information for a given order
    ///     passed to the razor page to supply order data to render
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }

        public string CompanyName { get; set; }

        public string Description { get; set; }

        public decimal OrderTotal { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }

    }


    /// <summary>
    /// class OrderProduct - class provides order information for a given order/product
    ///     combination used to supply the order quantity and price of a given product
    ///     for a given order, passed to the razor page to render.
    /// </summary>
    public class OrderProduct
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    
        public int Quantity { get; set; }

        public decimal Price { get; set; }

    }

    /// <summary>
    /// class Product - class provides the current name and price 
    ///     of a given product passed to the razor page to render.
    /// </summary>
    public class Product
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}