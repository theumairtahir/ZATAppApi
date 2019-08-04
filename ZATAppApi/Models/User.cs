using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ZATAppApi.Models.Exceptions;
using ZATAppApi.Models.ASPNetIdentity;
using ZATAppApi.App_Start;
using System;

namespace ZATAppApi.Models
{
    /// <summary>
    /// User is the main entity of the system which interacts with the system in different ways.
    /// </summary>
    public class User : DbModel
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        protected long id;
        bool isBlocked;
        NameFormat name;
        ContactNumberFormat contactNumber;
        /// <summary>
        /// Constructor to add new user data tuple to the database
        /// </summary>
        /// <param name="name">Full Name of the user</param>
        /// <param name="contactNumber">Contact Number of the user</param>
        protected User(NameFormat name, ContactNumberFormat contactNumber)
        {
            dbCommand = new SqlCommand("AddNewUser", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@fName", System.Data.SqlDbType.VarChar)).Value = name.FirstName;
            dbCommand.Parameters.Add(new SqlParameter("@lName", System.Data.SqlDbType.VarChar)).Value = name.LastName;
            dbCommand.Parameters.Add(new SqlParameter("@cCode", System.Data.SqlDbType.VarChar)).Value = contactNumber.CountryCode;
            dbCommand.Parameters.Add(new SqlParameter("@comCode", System.Data.SqlDbType.VarChar)).Value = contactNumber.CompanyCode;
            dbCommand.Parameters.Add(new SqlParameter("@phoneNumber", System.Data.SqlDbType.VarChar)).Value = contactNumber.PhoneNumber;
            dbConnection.Open();
            try
            {
                id = (long)dbCommand.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->Constructor(NameFormat,ContactNumberFormat)", ex);
            }
            dbConnection.Close();
            this.name = name;
            this.contactNumber = contactNumber;
        }
        /// <summary>
        /// Constructor to initialize values of a user from the database
        /// </summary>
        /// <param name="id">Primary key</param>
        public User(long id)
        {
            dbCommand = new SqlCommand("GetUser", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = (long)dbReader[0];
                        name = new NameFormat
                        {
                            FirstName = (string)dbReader[1],
                            LastName = (string)dbReader[2]
                        };
                        contactNumber = new ContactNumberFormat((string)dbReader[3], (string)dbReader[4], (string)dbReader[5]);
                        isBlocked = (bool)dbReader[6];
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->Constructor(long)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Primary key
        /// </summary>
        public long UserId
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// Property's value indicated wheater the user is authorized to use the application or not.
        /// </summary>
        public bool IsBlocked
        {
            get
            {
                return isBlocked;
            }
            set
            {
                dbCommand = new SqlCommand("UpdateIsBlockedUser", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@isBlocked", System.Data.SqlDbType.Bit)).Value = value;
                dbConnection.Open();
                try
                {
                    if (dbCommand.ExecuteNonQuery() == 0)
                    {
                        throw new UpdateUnsuccessfulException("User->IsBlocked");
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->IsBlocked", ex);
                }
                dbConnection.Close();
                isBlocked = value;
            }
        }
        /// <summary>
        /// Full Name of the user
        /// </summary>
        public NameFormat FullName
        {
            get
            {
                return name;
            }
            set
            {
                dbCommand = new SqlCommand("UpdateNameUser", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@fName", System.Data.SqlDbType.VarChar)).Value = value.FirstName;
                dbCommand.Parameters.Add(new SqlParameter("@lName", System.Data.SqlDbType.VarChar)).Value = value.LastName;
                dbConnection.Open();
                try
                {
                    if (dbCommand.ExecuteNonQuery() == 0)
                    {
                        throw new UpdateUnsuccessfulException("User->FullName");
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->FullName", ex);
                }
                dbConnection.Close();
                name = value;
            }
        }
        /// <summary>
        /// Contact Number of the user
        /// </summary>
        public ContactNumberFormat ContactNumber
        {
            get
            {
                return contactNumber;
            }
            set
            {
                dbCommand = new SqlCommand("UpdateContactUser", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@cCode", System.Data.SqlDbType.VarChar)).Value = value.CountryCode;
                dbCommand.Parameters.Add(new SqlParameter("@comCode", System.Data.SqlDbType.VarChar)).Value = value.CompanyCode;
                dbCommand.Parameters.Add(new SqlParameter("@phone", System.Data.SqlDbType.VarChar)).Value = value.PhoneNumber;
                dbConnection.Open();
                try
                {
                    if (dbCommand.ExecuteNonQuery() == 0)
                    {
                        throw new UpdateUnsuccessfulException("User->ContactNumber");
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->ContactNumber", ex);
                }
                dbConnection.Close();
                contactNumber = value;
            }
        }
        /// <summary>
        /// Shows that the user is active on the application
        /// </summary>
        public bool IsActive
        {
            get
            {
                bool isActive;
                ActiveStatus activeStatus = new ActiveStatus();
                dbCommand = new SqlCommand("GetIsActiveDriver", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
                dbConnection.Open();
                try
                {
                    using (dbReader = dbCommand.ExecuteReader())
                    {
                        while (dbReader.Read())
                        {
                            activeStatus = new ActiveStatus { StatusTime = (DateTime)dbReader[1], IsActive = (bool)dbReader[2] };
                        }
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->IsActive", ex);
                }
                dbConnection.Close();
                if (activeStatus.IsActive && activeStatus.StatusTime > DateTime.Now.AddSeconds(-45))
                {
                    isActive = activeStatus.IsActive;
                }
                else
                {
                    isActive = false;
                }
                return isActive;
            }
            set
            {
                dbCommand = new SqlCommand("SetIsActiveUser", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@isActive", System.Data.SqlDbType.Bit)).Value = value;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->IsActive", ex);
                }
                dbConnection.Close();

            }
        }
        /// <summary>
        /// Role of the user to the application
        /// </summary>
        public ApplicationRoles Role
        {
            get
            {
                ApplicationRoles role;
                dbCommand = new SqlCommand("GetUserRole", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
                dbConnection.Open();
                try
                {
                    short value = (short)dbCommand.ExecuteScalar();
                    if (value > 1 && value <= 4)
                    {
                        role = (ApplicationRoles)value;
                    }
                    else
                    {
                        throw new MalValueArrivedException("User->Role");
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->Role", ex);
                }
                dbConnection.Close();
                return role;
            }
        }
        /// <summary>
        /// Gets ASP.Net Identity user associated with this user.
        /// </summary>
        public ApplicationUser ApplicationUser
        {
            get
            {
                string id = null;
                dbCommand = new SqlCommand("SELECT IdentityId FROM USERS_HAVE_IDENTITY WHERE UId=" + id, dbConnection);
                dbConnection.Open();
                try
                {
                    id = (string)dbCommand.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("User->ApplcationUser", ex);
                }
                dbConnection.Close();
                var user = new ApplicationUserManager(new UserStore<ApplicationUser>()).FindById(id);
                if (user != null)
                {
                    return user;
                }
                throw new UnsuccessfullProcessException("User->Application User");
            }
        }
        /// <summary>
        /// Method to send SMS to the user. This method just adds the values into the database, 
        /// in order to send SMS, there should be some service connected to the system and implement its method.
        /// </summary>
        /// <param name="sms">Text Sms to be sent</param>
        public void SendSms(Sms sms)
        {
            dbCommand = new SqlCommand("SendSMS", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbCommand.Parameters.Add(new SqlParameter("@smsId", System.Data.SqlDbType.BigInt)).Value = sms.SmsId;
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->SendSms", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Method to get the list of SMS received by the user
        /// </summary>
        /// <returns></returns>
        public List<Sms> ReceivedSms()
        {
            List<Sms> lstSms = new List<Sms>();
            dbCommand = new SqlCommand("GetReceivedSMS", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstSms.Add(new Sms((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->ReceivedSms", ex);
            }
            dbConnection.Close();
            return lstSms;
        }
        /// <summary>
        /// Method to register a new account credentials for the user.
        /// </summary>
        /// <param name="role">User's role to the application</param>
        /// <param name="username">User's unique username</param>
        /// <param name="password">User's password to get log into the system</param>
        public ApplicationUser RegisterIdentityUser(ApplicationRoles role, string username, string password)
        {
            if (ApplicationUser != null)
            {
                var userStore = new UserStore<ApplicationUser>();
                var user = new ApplicationUser() { UserName = username, Email = "test@app.com", PhoneNumber = contactNumber.GetPhoneNumber() };
                ApplicationUserManager manager = new ApplicationUserManager(userStore);
                var result = manager.CreateAsync(user, password);
                if (result.Result.Succeeded)
                {
                    user = manager.FindByName(user.UserName);
                    manager.AddToRole(user.Id, role + "");
                    return user;
                }
                else
                {
                    throw new UserNotRegisteredException();
                }
            }
            else
            {
                return ApplicationUser;
            }
        }
        /// <summary>
        /// Method to return true if it sucessfully matched the credentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool MatchCredentials(string userName, string password)
        {
            bool flag = false;
            if (ApplicationUser != null)
            {
                var userStore = new UserStore<ApplicationUser>();

                ApplicationUserManager manager = new ApplicationUserManager(userStore);
                var user = manager.FindByName(userName);
                if (user != null)
                {
                    var result = manager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                    if (result== PasswordVerificationResult.Success)
                    {
                        flag = true;
                    }
                }
                else
                {
                    throw new UserNotRegisteredException();
                }
            }
            else
            {
                throw new UserNotRegisteredException();
            }
            return flag;
        }
        /// <summary>
        /// Method to change password for the user
        /// </summary>
        /// <param name="newPassword">new password user want to set</param>
        public void ChangePassword(string newPassword)
        {
            var userStore = new UserStore<ApplicationUser>();
            var manager = new ApplicationUserManager(userStore);
            var result = manager.ResetPassword(ApplicationUser.Id, null, newPassword);
        }

        /// <summary>
        /// Method to get all the user tuples present in the database in the form of a list
        /// </summary>
        /// <returns></returns>
        public static List<User> GetAllUsers()
        {
            List<User> lstUser = new List<User>();
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            SqlCommand dbCommand = new SqlCommand("SELECT * FROM USERS", dbConnection);
            dbConnection.Open();
            using (SqlDataReader dbReader = dbCommand.ExecuteReader())
            {
                while (dbReader.Read())
                {
                    lstUser.Add(new User((long)dbReader[0]));
                }
            }
            dbConnection.Close();
            return lstUser;
        }
        /// <summary>
        /// Method to get Rides Completed by the user
        /// </summary>
        /// <returns></returns>
        public virtual List<Ride> GetCompletedRides()
        {
            List<Ride> lstRides = new List<Ride>();
            dbCommand = new SqlCommand("GetCompletedRidesUser", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstRides.Add(new Ride((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->GetCompletedRides", ex);
            }
            dbConnection.Close();
            return lstRides;
        }
        /// <summary>
        /// Shows the active status of a user
        /// </summary>
        public struct ActiveStatus
        {
            /// <summary>
            /// Flag to check the active status
            /// </summary>
            public bool IsActive { get; set; }
            /// <summary>
            /// Represents the time at which the user updated it's active status
            /// </summary>
            public DateTime StatusTime { get; set; }
        }
        /// <summary>
        /// Enum which decide the role for the application
        /// </summary>
        public enum ApplicationRoles
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            Admin = 1,
            SubAdmin = 2,
            Driver = 3,
            Rider = 4
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
        /// <summary>
        /// Class to store user's contact details.
        /// </summary>
        public class ContactNumberFormat
        {
            string phoneNumber, countryCode, companyCode;
            /// <summary>
            /// Constructor to initiate contact number class values
            /// </summary>
            public ContactNumberFormat(string countryCode, string companyCode, string phoneNumber)
            {
                if (countryCode.Length > 3)
                {
                    throw new ValueLengthExceedsException(countryCode, 3);
                }
                if (companyCode.Length > 3)
                {
                    throw new ValueLengthExceedsException(companyCode, 3);
                }
                if (phoneNumber.Length > 7)
                {
                    throw new ValueLengthExceedsException(companyCode, 3);
                }
                this.companyCode = companyCode;
                this.countryCode = countryCode;
                this.phoneNumber = phoneNumber;
            }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public string CountryCode
            {
                get
                {
                    return countryCode;
                }
            }
            public string CompanyCode
            {
                get
                {
                    return companyCode;
                }
            }
            public string PhoneNumber
            {
                get
                {
                    return phoneNumber;
                }
            }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            /// <summary>
            /// Method which will return the full phone number.
            /// </summary>
            /// <returns></returns>
            public string GetPhoneNumber()
            {
                return countryCode + companyCode + phoneNumber;
            }
            /// <summary>
            /// Method to get phone number in (+xx-xxx-xxxxxxx) format
            /// </summary>
            /// <returns></returns>
            public string GetInternationalFormatedPhoneNumber()
            {
                return countryCode + "-" + companyCode + "-" + phoneNumber;
            }
            /// <summary>
            /// Method to get phone number in (0xxx-xxxxxxx)
            /// </summary>
            /// <returns></returns>
            public string GetLocalFormatedPhoneNumber()
            {
                return "0" + companyCode + "-" + phoneNumber;
            }
        }
        /// <summary>
        /// Full name for a person
        /// </summary>
        public struct NameFormat
        {
            /// <summary>
            /// Person's First Name.
            /// </summary>
            public string FirstName { get; set; }
            /// <summary>
            /// Person's Last Name.
            /// </summary>
            public string LastName { get; set; }
        }
    }
}