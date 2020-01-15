using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Infrastructure
{
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class Database
    {
        private readonly SqlConnection _connection;

        public Database()
        {
            _connection = new SqlConnection("Data Source=DANTOP;Initial Catalog=BrainWare;Integrated Security=SSPI;MultipleActiveResultSets=True;AttachDBFilename=C:\\Projects\\BrainWare\\Web\\App_Data\\BrainWare.mdf");

            _connection.Open();
        }


        public DbDataReader ExecuteReader(string query)
        {
           
            var sqlQuery = new SqlCommand(query, _connection);

            return sqlQuery.ExecuteReader();
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string query)
        {
            var sqlQuery = new SqlCommand(query, _connection);

            return await sqlQuery.ExecuteReaderAsync();
        }


        public int ExecuteNonQuery(string query)
        {
            var sqlQuery = new SqlCommand(query, _connection);

            return sqlQuery.ExecuteNonQuery();
        }

    }
}