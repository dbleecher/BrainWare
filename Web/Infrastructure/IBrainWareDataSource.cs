using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Infrastructure
{
	/// <summary>
	/// IBrainWareDataSource - Interface provides definitions
	///	  for data retrieval methods used for a data source classes.
	///	Note, actual class used is defined in the appSettings configuration
	///	  given key, "IBrainWareDataSource"; and objects are instatiated using
	///	  the BrainWareFactory (see BrainWareFactory.GetDataSource()).
	/// </summary>
	public interface IBrainWareDataSource
	{
		string GetCompanyName(int companyId);
		Task<List<OrderResult>> GetOrders(int companyId);
	}
}
