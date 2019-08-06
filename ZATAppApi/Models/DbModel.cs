using System.Data.SqlClient;

namespace ZATApp.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class DbModel
    {
        readonly public static string CONNECTION_STRING = @"Data Source=DESKTOP-B2ULAE3\SQLEXPRESS;Initial Catalog=ZATAppDb;Integrated Security=True";
        //sql database connection components
        protected SqlConnection dbConnection;
        protected SqlCommand dbCommand;
        protected SqlDataReader dbReader;
        public DbModel()
        {
            dbConnection = new SqlConnection(CONNECTION_STRING);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}