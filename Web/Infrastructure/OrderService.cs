using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    using System.Data;
    using System.Threading.Tasks;
    using Models;

    public class OrderService
    {
        public async Task<List<Order>> GetOrdersForCompanyAsync(int CompanyId)
        {
			// Validate input
			if (CompanyId <= 0)
			{
				throw new HttpRequestValidationException($"(GetOrdersForCompany) Invalid parameter {CompanyId} in GET request.");
			}

			var database = new Database();

			// Get the company name
			String companyName = "";
			using (var readerCompany = database.ExecuteReader($"SELECT TOP 1 name from Company WHERE company_id={CompanyId}"))
			{
				if (readerCompany.Read())
				{
					var recordCompany = (IDataRecord)readerCompany;
					companyName = readerCompany["name"].ToString();
				}
			}

			// Retieve all orders for the company sorted by order_id, product_id
			var sql1 = $@"
SELECT o.order_id, o.description, p.product_id, p.name as pname, p.price as pprice, op.price as opprice, op.quantity
 FROM [order] o (NOLOCK)
 INNER JOIN [orderproduct] op (NOLOCK) on op.order_id = o.order_id
 INNER JOIN [product] p (NOLOCK) on p.product_id = op.product_id
 WHERE o.company_id={CompanyId}
 ORDER BY o.order_id, p.product_id
";

			var values = new List<Order>();

			// Iterate through each query result asynchronously; using statement guarantees reader will be closed
			using (var reader1 = await database.ExecuteReaderAsync(sql1))
			{
				Order order = null;
				int lastOrderId = 0;

				// Read data asynchronously
				while (await reader1.ReadAsync())
				{
					var record1 = (IDataRecord)reader1;

					int orderId = Int32.Parse(record1["order_id"].ToString());
					String description = record1["description"].ToString();

					// If the there is a new order, create the order
					if (orderId != lastOrderId)
					{
						order = new Order()
						{
							OrderId = orderId,
							CompanyName = companyName,
							Description = description,
							OrderProducts = new List<OrderProduct>()
						};
						values.Add(order);
					}

					// Add the product to the order
					if (order != null) // should always be true
					{
						int productId = Int32.Parse(record1["product_id"].ToString());
						decimal orderProductPrice = Decimal.Parse(record1["opprice"].ToString());
						int quantity = Int32.Parse(record1["quantity"].ToString());
						String productName = record1["pname"].ToString();
						decimal productPrice = Decimal.Parse(record1["pprice"].ToString());

						order.OrderProducts.Add(new OrderProduct()
						{
							OrderId = orderId,
							ProductId = productId,
							Price = orderProductPrice,
							Quantity = quantity,
							Product = new Product()
							{
								Name = productName,
								Price = productPrice
							}
						});

						// Calculate the updated order total for the order
						order.OrderTotal = order.OrderTotal + (orderProductPrice * quantity);
					}

					// Save the last orderId to determine if it changes for the next iteration
					lastOrderId = orderId;
				}
			}

			return values;
		}
	}
}