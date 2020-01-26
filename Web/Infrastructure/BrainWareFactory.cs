using System;
using System.Configuration;
using System.Reflection;

namespace Web.Infrastructure
{
	/// <summary>
	/// BrainWareFactory - class provides helper method(s) used to create objects based on
	///  configuratoin settings.
	/// </summary>
	public class BrainWareFactory
	{
		/// <summary>
		/// Use this factory method to create the data source object where the class name
		///  is configured within the appSettings configuration with key name "IBrainWareDataSource"
		/// </summary>
		/// <returns>IBrainWareDataSource - data source object to retrieve data from</returns>
		public static IBrainWareDataSource GetDataSource()
		{
			// Retrieve the data source name from the configuration
			string dataSourceName = ConfigurationManager.AppSettings["IBrainWareDataSource"].ToString();
			string assemblyName = Assembly.GetExecutingAssembly().GetName().FullName;

			// Use reflection to create the data source object
			return (IBrainWareDataSource)Activator.CreateInstance(assemblyName, dataSourceName).Unwrap();
		}
	}
}