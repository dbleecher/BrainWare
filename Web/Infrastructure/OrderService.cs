using System;
using System.Collections.Generic;

namespace Web.Infrastructure
{
    using System.Threading.Tasks;
    using Models;

    /// <summary>
    /// class OrderService - performs data retrieval operations to be returned back to the controller
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// Async method used to retrieve a list of Orders back to the controller
        /// </summary>
        /// <param name="CompanyId">Company ID for the Company record to retrieve orders for</param>
        /// <returns></returns>
        public async Task<List<Order>> GetOrdersForCompanyAsync(int CompanyId)
        {
            // Validate input
            if (CompanyId <= 0)
            {
                throw new Exception($"(GetOrdersForCompany) Invalid parameter {CompanyId} in GET request.");
            }

            IBrainWareDataSource db = BrainWareFactory.GetDataSource();

            // Retrieve the company name
            string companyName = db.GetCompanyName(CompanyId);
            if (companyName == null)
            {
                throw new Exception($"No company exists with ID={CompanyId}.");
            }

            // Retrieve orders
            var values = new List<Order>();

            // Retieve all orders for the company sorted by order_id, product_id
            var results = await db.GetOrders(CompanyId);
            if (results != null)
            {
                Order order = null;
                int lastOrderId = 0;
                foreach(var result in results)
                {
                    int orderId = result.OrderId;
                    if (orderId != lastOrderId)
                    {
                        order = new Order()
                        {
                            OrderId = orderId,
                            CompanyName = companyName,
                            Description = result.OrderDescription,
                            OrderProducts = new List<OrderProduct>()
                        };
                        values.Add(order);
                    }

                    decimal price = result.OrderProductPrice ?? 0;
                    int quantity = result.OrderProductQuantity ?? 0;

                    // Add the product to the order
                    order.OrderProducts.Add(new OrderProduct()
                    {
                        OrderId = orderId,
                        ProductId = result.ProductId,
                        Quantity = quantity,
                        Price = price,
                        Product = new Product()
                        {
                            Name = result.ProductName,
                            Price = result.ProductPrice ?? 0
                        }
                    }); ;

                    // Calculate the updated order total for the order
                    order.OrderTotal += (price * quantity);

                    // Save the last orderId to determine if it changes for the next iteration
                    lastOrderId = orderId;
                }
            }

            return values;
        }
    }
}