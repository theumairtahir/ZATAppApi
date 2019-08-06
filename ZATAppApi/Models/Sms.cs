using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ZATApp.Models.Exceptions;

namespace ZATApp.Models
{
    /// <summary>
    /// Class which represent the text sms of the application
    /// </summary>
    public class Sms : DbModel
    {
        long id;
        DateTime sentDateTime;
        string body;
        /// <summary>
        /// constructor to initialize the values from the databse
        /// </summary>
        /// <param name="id">primary key</param>
        public Sms(long id)
        {
            dbCommand = new SqlCommand("GetSms", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@smsId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
                        sentDateTime = (DateTime)dbReader[1];
                        body = (string)dbReader[2];
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Sms->Constructor(long)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// A Text Sms to be sent to the user
        /// </summary>
        /// <param name="sentDateTime">time at which the sms is being sent</param>
        /// <param name="body">Textual body of the SMS</param>
        public Sms(DateTime sentDateTime, string body)
        {
            dbCommand = new SqlCommand("AddNewSms", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = sentDateTime;
            dbCommand.Parameters.Add(new SqlParameter("@body", System.Data.SqlDbType.Text)).Value = body;
            dbConnection.Open();
            try
            {
                id = Convert.ToInt64(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Sms->Constructor(DateTime, string)", ex);
            }
            dbConnection.Close();
            this.sentDateTime = sentDateTime;
            this.body = body;
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public long SmsId
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// Date and time at which the sms has been sent
        /// </summary>
        public DateTime SentDateTime
        {
            get
            {
                return sentDateTime;
            }
        }
        /// <summary>
        /// Textual Body of the SMS
        /// </summary>
        public string Body
        {
            get
            {
                return body;
            }
        }
        /// <summary>
        /// Method to get all the users who have received the SMS
        /// </summary>
        /// <returns></returns>
        public List<User> GetReceivers()
        {
            List<User> lstUsers = new List<User>();
            dbCommand = new SqlCommand("GetReceiversSms", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@smsId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    lstUsers.Add(new User((long)dbReader[0]));
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Sms->GetReceivers", ex);
            }
            dbConnection.Close();
            return lstUsers;
        }
        /// <summary>
        /// Static method to get all sms present in the database.
        /// </summary>
        /// <returns></returns>
        public static List<Sms> GetAllSms()
        {
            List<Sms> lstSMS = new List<Sms>();
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            SqlCommand dbCommand = new SqlCommand("SELECT * FROM SMSES", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    lstSMS.Add(new Sms((long)dbReader[0]));
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Sms->GetAllSms", ex);
            }
            dbConnection.Close();
            return lstSMS;
        }
    }
}