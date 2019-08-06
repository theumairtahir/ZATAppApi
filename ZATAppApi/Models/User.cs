using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using ZATAppApi.Models.Exceptions;
using ZATAppApi.Models.ASPNetIdentity;
using ZATAppApi.App_Start;
using System;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

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
                var result = dbCommand.ExecuteScalar();
                id = Convert.ToInt64(result);
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
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
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
                dbCommand.Parameters.Add(new SqlParameter("@dateTime", System.Data.SqlDbType.DateTime)).Value = DateTime.Now;
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
                    short value = Convert.ToInt16(dbCommand.ExecuteScalar());
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
        /// Method to get the ASP.Net identity for the user
        /// </summary>
        /// <returns></returns>
        public ApplicationUser GetApplicationUser()
        {
            string identityId = null;
            dbCommand = new SqlCommand("SELECT IdentityId FROM USERS_HAVE_IDENTITY WHERE UId=" + this.id, dbConnection);
            dbConnection.Open();
            try
            {
                var result = dbCommand.ExecuteScalar();
                if (result != null)
                {
                    identityId = Convert.ToString(result);
                }
                else
                {
                    dbConnection.Close();
                    return null;
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->ApplcationUser", ex);
            }
            dbConnection.Close();
            var user = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext())).FindById(identityId);
            if (user != null)
            {
                return user;
            }
            throw new UnsuccessfullProcessException("User->Application User");
        }

        /// <summary>
        /// Method to send SMS to the user. This method just adds the values into the database, 
        /// in order to send SMS, there should be some service connected to the system and implement its method.
        /// </summary>
        /// <param name="sms">Text Sms to be sent</param>
        public Sms SendSms(Sms sms)
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
            return sms;
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
        public virtual ApplicationUser RegisterIdentityUser(ApplicationRoles role, string username, string password)
        {
            if (GetApplicationUser() == null)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
                var user = new ApplicationUser() { UserName = username, Email = "test@app.com", PhoneNumber = contactNumber.GetPhoneNumber() };
                ApplicationUserManager manager = new ApplicationUserManager(userStore);
                var result = manager.Create(user, password);
                if (result.Succeeded)
                {
                    user = manager.FindByName(user.UserName);
                    manager.AddToRole(user.Id, role + "");
                    //add to the custom db table to get the relation
                    AddIdentityUserToDb(user.Id);
                    return user;
                }
                else
                {
                    throw new UserNotRegisteredException(result.Errors);
                }
            }
            else
            {
                return GetApplicationUser();
            }
        }

        private void AddIdentityUserToDb(string id)
        {
            dbCommand = new SqlCommand("AddnewIdentityUser", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = this.id; //userId
            dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.NVarChar)).Value = id; //identityId
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("User->AddIdentityUserToDb", ex);
            }
            dbConnection.Close();
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
            if (GetApplicationUser() != null)
            {
                var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());

                ApplicationUserManager manager = new ApplicationUserManager(userStore);
                var user = manager.FindByName(userName);
                if (user != null)
                {
                    var result = manager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password);
                    if (result == PasswordVerificationResult.Success)
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
        /// <param name="oldPassword">Old Password to verify</param>
        /// <param name="newPassword">New password user want to set</param>
        /// 
        public virtual void ChangePassword(string oldPassword, string newPassword)
        {
            var userStore = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new ApplicationUserManager(userStore);
            var user = GetApplicationUser();
            var res = manager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, oldPassword);
            if (res == PasswordVerificationResult.Failed)
            {
                throw new UserNotRegisteredException();
            }
            //var resetToken = manager.GeneratePasswordResetToken(user.Id);
            var result = manager.ChangePassword(user.Id, oldPassword, newPassword);
            if (result.Succeeded)
            {
                throw new UnsuccessfullProcessException("User->ChangePassword");
            }
        }

        /// <summary>
        /// Method to get all the user tuples present in the database in the form of a list
        /// </summary>
        /// <returns></returns>
        public static List<User> GetAllUsers()
        {
            List<User> lstUser = new List<User>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
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
                Regex regexForCountryCode = new Regex(@"[+][1-9][1-9]");
                if (!regexForCountryCode.IsMatch(countryCode))
                {
                    throw new ValidationPatternNotMatchException(countryCode, regexForCountryCode.ToString(), "+92");
                }
                Regex regexForCompanyCode = new Regex(@"[3][0-9][0-9]");
                if (!regexForCompanyCode.IsMatch(companyCode))
                {
                    throw new ValidationPatternNotMatchException(companyCode, regexForCompanyCode.ToString(), "301");
                }
                Regex regexForPhoneNumber = new Regex(@"\b\d{7,7}\b");
                if (!regexForPhoneNumber.IsMatch(phoneNumber))
                {
                    throw new ValidationPatternNotMatchException(phoneNumber, regexForPhoneNumber.ToString(), "1234567");
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
            string firstName, lastName;
            /// <summary>
            /// Person's First Name.
            /// </summary>
            public string FirstName
            {
                get
                {
                    string firstLetterCapital = firstName.Substring(0, 1).ToUpper();
                    string restWordSmall = firstName.Substring(1).ToLower();
                    return firstLetterCapital + restWordSmall;
                }
                set
                {
                    Regex regexForFirstName = new Regex(@"([A-Z]|[a-z])+");
                    if (regexForFirstName.IsMatch(value))
                    {
                        firstName = value;
                    }
                    else
                    {
                        throw new ValidationPatternNotMatchException(firstName, regexForFirstName.ToString(), "Ahmed or ahmed");
                    }
                }
            }
            /// <summary>
            /// Person's Last Name.
            /// </summary>
            public string LastName
            {
                get
                {
                    string firstLetterCapital = lastName.Substring(0, 1).ToUpper();
                    string restWordSmall = lastName.Substring(1).ToLower();
                    return firstLetterCapital + restWordSmall;
                }
                set
                {
                    Regex regexForLastName = new Regex(@"([A-Z]|[a-z])+");
                    if (regexForLastName.IsMatch(value))
                    {
                        lastName = value;
                    }
                    else
                    {
                        throw new ValidationPatternNotMatchException(lastName, regexForLastName.ToString(), "Azam or azam");
                    }
                }
            }
        }
    }
}