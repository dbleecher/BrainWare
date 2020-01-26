namespace Web.Infrastructure
{
    /// <summary>
    /// class OrderResult - class whose objects are used to store results from
    ///   IBrainWareDataSource-based objects that implement 
    ///   and are returned by the GetOrders() implemented method(s) and
    ///   consumed by the OrderService.
    /// </summary>
	public sealed class OrderResult
	{
        public int OrderId { get; set; }
        public string OrderDescription { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public decimal? OrderProductPrice { get; set; }
        public int? OrderProductQuantity { get; set; }
    }
}