using System;
using System.Data.SqlClient;
using ZATAppApi.Models.Exceptions;

namespace ZATAppApi.Models
{
    /// <summary>
    /// Log to store debit and credit information of tghe system
    /// </summary>
    public class AccountingLog : DbModel
    {
        long id;
        DateTime entryTime;
        decimal debit, credit;
        Driver driver;
        /// <summary>
        /// Constructor to initialize values form the database
        /// </summary>
        /// <param name="id">primary key</param>
        public AccountingLog(long id)
        {
            dbCommand = new SqlCommand("GetAccountingLog", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@aId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
                        entryTime = (DateTime)dbReader[1];
                        debit = (decimal)dbReader[2];
                        credit = (decimal)dbReader[3];
                        driver = new Driver(Convert.ToInt64(dbReader[4]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->Constructor(long)", ex);
            }
            dbConnection.Close();

        }
        /// <summary>
        /// Constructor to add new record to the log 
        /// </summary>
        /// <param name="credit">credit w.r.t driver (|Balance-Amount|)</param>
        /// <param name="debit">debit w.r.t driver (Balance+Amount)</param>
        /// <param name="driver">User who's transactions will be managed</param>
        public AccountingLog(decimal credit, decimal debit, Driver driver)
        {
            DateTime entryTime = DateTime.Now;
            dbCommand = new SqlCommand("AddNewAccountingLog", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = driver.UserId;
            dbCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = entryTime;
            dbCommand.Parameters.Add(new SqlParameter("@debit", System.Data.SqlDbType.Money)).Value = debit;
            dbCommand.Parameters.Add(new SqlParameter("@credit", System.Data.SqlDbType.Money)).Value = credit;
            dbConnection.Open();
            try
            {
                id = Convert.ToInt64(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->Constructor(decimal, decimal, Driver)", ex);
            }
            dbConnection.Close();
            this.debit = debit;
            this.credit = credit;
            this.driver = driver;
            this.entryTime = entryTime;
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public long AccountLogId
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// credit w.r.t driver (|Balance-Amount|)
        /// </summary>
        public decimal Credit
        {
            get
            {
                return credit;
            }
        }
        /// <summary>
        /// debit w.r.t driver (Balance+Amount)
        /// </summary>
        public decimal Debit
        {
            get
            {
                return debit;
            }
        }
        /// <summary>
        /// User who's transactions will be managed
        /// </summary>
        public DateTime EntryTime
        {
            get
            {
                return entryTime;
            }
        }
        /// <summary>
        /// Method to get the amount which is due on all drivers
        /// </summary>
        /// <returns></returns>
        public static decimal GetAdminDueCollection()
        {
            decimal total = 0;
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetAdminTotalCollectionDue", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbConnection.Open();
            try
            {
                total = Convert.ToDecimal(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->GetAdminDueCollection", ex);
            }
            dbConnection.Close();
            return total;
        }
        /// <summary>
        /// Method to get the amount which is credited by the drivers to the admin
        /// </summary>
        /// <returns></returns>
        public static decimal GetAdminCreditedAmount()
        {
            decimal total = 0;
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetAdminCreditedAmount", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbConnection.Open();
            try
            {
                total = Convert.ToDecimal(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->GetAdminCreditedAmount", ex);
            }
            dbConnection.Close();
            return total;
        }
        ///// <summary>
        ///// Mehtod to get the amount which is in excess to the admin
        ///// </summary>
        ///// <returns></returns>
        //public static decimal GetAdminCredit()
        //{
        //    decimal credit = 0;
        //    SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        //    SqlCommand dbCommand = new SqlCommand("GetAdminCredit", dbConnection);
        //    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //    dbConnection.Open();
        //    try
        //    {
        //        credit = (decimal)dbCommand.ExecuteScalar();
        //    }
        //    catch (SqlException ex)
        //    {
        //        dbConnection.Close();
        //        throw new DbQueryProcessingFailedException("AccountingLog->GetAdminCredit", ex);
        //    }
        //    dbConnection.Close();
        //    return credit;
        //}
        ///// <summary>
        ///// Mehtod to get the amount which is in excess to the admin by some month
        ///// </summary>
        ///// <param name="month"></param>
        ///// <returns></returns>
        //public static decimal GetAdminCreditByMonth(DateTime month)
        //{
        //    DateTime startDate = new DateTime(month.Year, month.Month, 1);
        //    DateTime endDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
        //    decimal credit = 0;
        //    SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
        //    SqlCommand dbCommand = new SqlCommand("GetAdminCreditByMonth", dbConnection);
        //    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //    dbCommand.Parameters.Add(new SqlParameter("@startDate", System.Data.SqlDbType.DateTime)).Value = startDate;
        //    dbCommand.Parameters.Add(new SqlParameter("@endDate", System.Data.SqlDbType.DateTime)).Value = endDate;
        //    dbConnection.Open();
        //    try
        //    {
        //        credit = (decimal)dbCommand.ExecuteScalar();
        //    }
        //    catch (SqlException ex)
        //    {
        //        dbConnection.Close();
        //        throw new DbQueryProcessingFailedException("AccountingLog->GetAdminCreditByMonth", ex);
        //    }
        //    dbConnection.Close();
        //    return credit;
        //}
        /// <summary>
        /// Method to get the amount which is in due on the drivers by some month
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static decimal GetAdminDueCollectioByMonth(DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
            decimal total = 0;
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetAdminTotalCollectionDueByMonth", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@startDate", System.Data.SqlDbType.DateTime)).Value = startDate;
            dbCommand.Parameters.Add(new SqlParameter("@endDate", System.Data.SqlDbType.DateTime)).Value = endDate;
            dbConnection.Open();
            try
            {
                total = Convert.ToDecimal(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->GetAdminDueCollectioByMonth", ex);
            }
            dbConnection.Close();
            return total;
        }
        /// <summary>
        /// Method to get amount credited by the drivers on the admin
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static decimal GetAdminCreditByMonth(DateTime month)
        {
            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
            decimal total = 0;
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetCreditedAmountByMonth", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@startDate", System.Data.SqlDbType.DateTime)).Value = startDate;
            dbCommand.Parameters.Add(new SqlParameter("@endDate", System.Data.SqlDbType.DateTime)).Value = endDate;
            dbConnection.Open();
            try
            {
                total = Convert.ToDecimal(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->GetAdminCreditByMonth", ex);
            }
            dbConnection.Close();
            return total;
        }
        /// <summary>
        /// Method to get Balance between Debit and Credit for admin
        /// </summary>
        /// <returns></returns>
        public static decimal GetAdminBalance()
        {
            decimal total = 0;
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetAdminBalance", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbConnection.Open();
            try
            {
                total = Convert.ToDecimal(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("AccountingLog->GetAdminBalance", ex);
            }
            dbConnection.Close();
            return total;
        }
    }
}