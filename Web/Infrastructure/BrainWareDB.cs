using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.EF;

namespace Web.Infrastructure
{
    /// <summary>
    /// class BrainWareDB - methods used to perfrom data retrieval operations on a database data source
    /// </summary>
    public class BrainWareDB : IBrainWareDataSource
    {
        private readonly BrainWareDBDataContext _context;

        /// <summary>
        /// BrainWareDB() - constructor for BrainWareDB class
        ///     - creates DataContext used in linq queries
        /// </summary>
        public BrainWareDB()
        {
            _context = new BrainWareDBDataContext();
        }

        /// <summary>
        /// GetCompanyName() - Retrieve the company name from the database
        ///     based on company Id
        /// </summary>
        /// <param name="companyId">Company Id of company to lookup name for</param>
        /// <returns></returns>
        public string GetCompanyName(int companyId)
        {
            var company = from c in _context.Companies
                          where c.company_id == companyId
                          select c.name;
            return company.First();
        }


        /// <summary>
        /// GetOrders() - async method used to retrieve orders and related records
        ///     for a given company from the database
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<List<OrderResult>> GetOrders(int companyId)
        {
            var orders = from o in _context.Orders
                         join op in _context.orderproducts on o.order_id equals op.order_id
                         join p in _context.Products on op.product_id equals p.product_id
                         where o.company_id == companyId
                         orderby o.order_id, p.product_id
                         select new OrderResult
                         {
                             OrderId = o.order_id,
                             OrderDescription = o.description,
                             ProductId = p.product_id,
                             ProductName = p.name,
                             ProductPrice = p.price,
                             OrderProductPrice = op.price,
                             OrderProductQuantity = op.quantity
                         };
            IQueryable<OrderResult> results = await Task.FromResult(orders);

            return results.ToList<OrderResult>();
        }
    }
}