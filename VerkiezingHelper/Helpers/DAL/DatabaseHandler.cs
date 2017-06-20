using System.Data;
using System.Data.SqlClient;

namespace VerkiezingHelper.Helpers.DAL
{
    public static class DatabaseHandler
    {
        private const string ConnectionString = "...";

        public static DataTable GetData(SqlCommand query)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                query.Connection = conn;
                var table = new DataTable();
                new SqlDataAdapter(query).Fill(table);
                return table;
            }
        }

        public static DataTable GetData(string query)
        {
            var table = new DataTable();
            new SqlDataAdapter(query, ConnectionString).Fill(table);
            return table;
        }

        public static void ExecuteQuery(SqlCommand query)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                query.Connection = conn;
                query.ExecuteNonQuery();
            }
        }

        public static void ExecuteQuery(string query)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                using (var comm = new SqlCommand(query, conn))
                {
                    comm.ExecuteNonQuery();
                }
            }
        }
    }
}