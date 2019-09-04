using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace ZATAppApi.Models
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    [DataContract]
    public class DbModel
    {
        static string localConString = @"Data Source=SPARKERZ_BRAIN\SQLEXPRESS;Initial Catalog=ZATAppDb;Integrated Security=True";
        static string azureConString = "";
        readonly public static string CONNECTION_STRING;
        static DbModel()
        {
            CONNECTION_STRING = ConfigurationManager.ConnectionStrings["ZATAppDbConnectionString"].ConnectionString;
        }
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