using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ZATAppApi.Models.Exceptions;

namespace ZATAppApi.Models
{
    /// <summary>
    /// Manual Transactions recorder
    /// </summary>
    public class ManualTransactionLog : DbModel
    {
        long id;
        decimal amount;
        DateTime dateTime;
        Driver driver;
        /// <summary>
        /// Constructor to add new record for transaction
        /// </summary>
        /// <param name="amount">Amount of Transaction</param>
        /// <param name="dateTime">Date and time of the Transaction</param>
        /// <param name="driver">User who made the transaction</param>
        public ManualTransactionLog(decimal amount, DateTime dateTime, Driver driver)
        {
            dbCommand = new SqlCommand("AddNewTransaction", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = driver.UserId;
            dbCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = dateTime;
            dbCommand.Parameters.Add(new SqlParameter("@amount", System.Data.SqlDbType.Money)).Value = amount;
            dbConnection.Open();
            try
            {
                var result = dbCommand.ExecuteScalar();
                id = Convert.ToInt64(result);
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("ManualTransactionlog->Constructor(decimal, DateTime, Driver)", ex);
            }
            dbConnection.Close();
            this.amount = amount;
            this.dateTime = dateTime;
            this.driver = driver;
            //As manual transaction don't need any verification so it will add the transaction to the accounting register stright away
            //whenever user does a transaction it will register as a debit in the accounting register, which will be calculated by adding previous balance and the amount of transaction
            new AccountingLog(0, amount, driver);
        }
        /// <summary>
        /// Constructor to initialize values from the database
        /// </summary>
        /// <param name="id">Primary Key</param>
        public ManualTransactionLog(long id)
        {
            dbCommand = new SqlCommand("GetTransaction", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
                        dateTime = (DateTime)dbReader[1];
                        amount = (decimal)dbReader[2];
                        driver = new Driver((long)dbReader[3]);
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("ManualTransactionLog->Constructor(long)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Primary key
        /// </summary>
        [DataMember]
        public long TransactionId
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// Date and Time at which the transaction made
        /// </summary>
        [DataMember]
        public DateTime TransactionDateTime
        {
            get
            {
                return dateTime;
            }
        }
        /// <summary>
        /// Amount of transaction
        /// </summary>
        [DataMember]
        public decimal Amount
        {
            get
            {
                return amount;
            }
        }
        /// <summary>
        /// Driver who made the transaction
        /// </summary>
        [DataMember]
        public Driver Driver
        {
            get
            {
                return driver;
            }
        }
        /// <summary>
        /// Method which returns the list of all transactions made in the system
        /// </summary>
        /// <returns>Sorted List of Transactions by date in chronological order</returns>
        public static List<ManualTransactionLog> GetAllTransactions()
        {
            List<ManualTransactionLog> lstTransactionLog = new List<ManualTransactionLog>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("SELECT [TransactionId] FROM [MANUAL_TRANSACTIONS_LOG] ORDER BY [DateTime] DESC", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstTransactionLog.Add(new ManualTransactionLog((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("ManualTransactionLog->GetAllTransactions", ex);
            }
            dbConnection.Close();
            return lstTransactionLog;
        }
    }
}