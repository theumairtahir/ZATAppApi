using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using ZATApp.Models.Exceptions;

namespace ZATApp.Models
{
    /// <summary>
    /// Transactions done from a mobile acount recorder
    /// </summary>
    public class MobileAccountTransactionLog : DbModel
    {
        long id;
        bool isVerified;
        string refNumber, mobileAccountService;
        DateTime dateTime;
        decimal amount;
        Driver driver;
        /// <summary>
        /// Constructor to initialize values from database
        /// </summary>
        /// <param name="id">Primary Key</param>
        public MobileAccountTransactionLog(long id)
        {
            dbCommand = new SqlCommand("GetMobileAccountTransaction", dbConnection);
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
                        refNumber = (string)dbReader[1];
                        dateTime = (DateTime)dbReader[2];
                        isVerified = (bool)dbReader[3];
                        mobileAccountService = (string)dbReader[4];
                        amount = (decimal)dbReader[5];
                        driver = new Driver((long)dbReader[6]);
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("MobileAccountTransactionLog->Constructor(long)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Constructor to add new record to the database
        /// </summary>
        /// <param name="refNumber">Reference Number of the tansaction, provided by the service provider</param>
        /// <param name="dateTime">date and time of transaction to be added to the system</param>
        /// <param name="isVerified">Status of the transaction. Verified by the Admin on receiving</param>
        /// <param name="mobileAccountService">Name of the service provider</param>
        /// <param name="amount">amount of the transaction</param>
        /// <param name="driver">driver who made the transaction</param>
        public MobileAccountTransactionLog(string refNumber, DateTime dateTime, bool isVerified, string mobileAccountService, decimal amount, Driver driver)
        {
            dbCommand = new SqlCommand("AddNewMobileTransaction", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = driver.UserId;
            dbCommand.Parameters.Add(new SqlParameter("@ref", System.Data.SqlDbType.VarChar)).Value = refNumber;
            dbCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = dateTime;
            dbCommand.Parameters.Add(new SqlParameter("@isVerified", System.Data.SqlDbType.Bit)).Value = isVerified;
            dbCommand.Parameters.Add(new SqlParameter("@mService", System.Data.SqlDbType.VarChar)).Value = mobileAccountService;
            dbCommand.Parameters.Add(new SqlParameter("@amount", System.Data.SqlDbType.Money)).Value = amount;
            dbConnection.Open();
            try
            {
                id = Convert.ToInt64(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    //Unique key handler
                    throw new UniqueKeyViolationException("Cannot add duplicate data.");
                }
                throw new DbQueryProcessingFailedException("MobileAccountTransactionLog->Constructor(string, DateTime, bool, string, decimal, Driver)", ex);
            }
            dbConnection.Close();
            this.refNumber = refNumber;
            this.dateTime = dateTime;
            this.isVerified = isVerified;
            this.mobileAccountService = mobileAccountService;
            this.amount = amount;
            this.driver = driver;
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
        /// Reference Number provided by the Mobile Acount Service Provider on transaction
        /// </summary>
        [DataMember]
        public string ReferenceNumber
        {
            get
            {
                return refNumber;
            }
        }
        /// <summary>
        /// Status of the transaction. To be Verified by the Admin.
        /// </summary>
        [DataMember]
        public bool IsVerified
        {
            get
            {
                return isVerified;
            }
            set
            {
                if (!isVerified)
                {
                    dbCommand = new SqlCommand("SetIsVerifiedMobileTransaction", dbConnection);
                    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.BigInt)).Value = id;
                    dbCommand.Parameters.Add(new SqlParameter("@isVerified", System.Data.SqlDbType.Bit)).Value = value;
                    dbConnection.Open();
                    try
                    {
                        dbCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        dbConnection.Close();
                        throw new DbQueryProcessingFailedException("MobileAccountTransactionLog", ex);
                    }
                    dbConnection.Close();
                    isVerified = value;
                    //whenever user does a transaction it will register as a debit in the accounting register, which will be calculated by adding previous balance and the amount of transaction
                    new AccountingLog(0, amount, driver);
                }
                else
                {
                    throw new NotAuthorizedToChangeValueExeption("MobileAccountTransactionLog->IsVerified", "IsVerified");
                }
            }
        }
        /// <summary>
        /// Name of the Service Provider
        /// </summary>
        [DataMember]
        public string MobileAccountServiceProviderName
        {
            get
            {
                return mobileAccountService;
            }
        }
        /// <summary>
        /// Amount to be transacted
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
        /// Time at which the transaction registered to the system for verification
        /// </summary>
        [DataMember]
        public DateTime TransactionRegisteredTime
        {
            get
            {
                return dateTime;
            }
        }
        /// <summary>
        /// Driver who does the transaction
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
        /// Method to get all mobile account transactions
        /// </summary>
        /// <returns>List of Mobile account transactions in chronological order</returns>
        public static List<MobileAccountTransactionLog> GetAllMobileAccountTransactions()
        {
            List<MobileAccountTransactionLog> lstTransactions = new List<MobileAccountTransactionLog>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetAllMobileTransactions", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                        lstTransactions.Add(new MobileAccountTransactionLog((long)dbReader[0]));
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("MobileAccountTransactionLog->GetAllMobileTransactions", ex);
            }
            dbConnection.Close();
            return lstTransactions;
        }
        /// <summary>
        /// Static Method to get all the unverified mobile account transactions
        /// </summary>
        /// <returns></returns>
        public static List<MobileAccountTransactionLog> GetAllUnverifiedMobileAccountTransactions()
        {
            List<MobileAccountTransactionLog> lstTransactions = new List<MobileAccountTransactionLog>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("SELECT TransactionId FROM MOBILE_ACCOUNT_TRANSACTIONS_LOG WHERE IsVerified = 'FALSE' ORDER BY[DateTime]", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                        lstTransactions.Add(new MobileAccountTransactionLog((long)dbReader[0]));
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("MobileAccountTransactionLog->GetAllMobileTransactions", ex);
            }
            dbConnection.Close();
            return lstTransactions;
        }
    }
}