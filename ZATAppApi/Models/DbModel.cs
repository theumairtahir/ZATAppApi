using System.Configuration;
using System.Data.SqlClient;

namespace ZATAppApi.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class DbModel
    {
        //sql database connection components
        protected SqlConnection dbConnection;
        protected SqlCommand dbCommand;
        protected SqlDataReader dbReader;
        public DbModel()
        {
            dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}