using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using Models;

    public class OrderController : ApiController
    {
        [HttpGet]
        public async Task<IEnumerable<Order>> GetOrders(int id = 1)
        {
            try
            {
                var orderService = new OrderService();

                return await orderService.GetOrdersForCompanyAsync(id);
            }
            catch (Exception ex)
            {
                // Log C# exception and rethrow exception to razor page to display error
                string error = "EXCEPTION CAUGHT: " + ex.Message + " " + ex.InnerException ?? "";
                error += ex.StackTrace;
                System.Diagnostics.Trace.TraceError(error);
                throw;
            }
        }
    }
}
