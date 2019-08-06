using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ZATAppApi.Models.Common;
using ZATAppApi.Models.Exceptions;

namespace ZATAppApi.Models
{
    /// <summary>
    /// A user of the system who picks up the ride and does related tasks
    /// </summary>
    public class Driver : User
    {
        Location lastLocation;
        decimal creditLimit;
        /// <summary>
        /// Constructor to add new Driver to the database
        /// </summary>
        /// <param name="name">Full Name of the driver</param>
        /// <param name="contactNumber">Contact Number of the driver</param>
        /// <param name="creditLimit">Credit Limit to which the driver can use the app, after that the acount will be blocked</param>
        /// <param name="lastLocation">Last known location of the driver</param>
        public Driver(NameFormat name, ContactNumberFormat contactNumber, decimal creditLimit, Location lastLocation) : base(name, contactNumber)
        {
            dbCommand = new SqlCommand("AddNewDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
            dbCommand.Parameters.Add(new SqlParameter("@creditLimit", System.Data.SqlDbType.Money)).Value = creditLimit;
            dbCommand.Parameters.Add(new SqlParameter("@lLongitude", System.Data.SqlDbType.Decimal)).Value = lastLocation.Longitude;
            dbCommand.Parameters.Add(new SqlParameter("@lLatitude", System.Data.SqlDbType.Decimal)).Value = lastLocation.Latitude;
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->Constructor(NameFormat, ContactNumberFormat, decimal, Location)", ex);
            }
            dbConnection.Close();
            this.creditLimit = creditLimit;
            this.lastLocation = lastLocation;
        }
        /// <summary>
        /// Constructor to initialize a driver's data from the database
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Driver(long id) : base(id)
        {
            dbCommand = new SqlCommand("GetDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        creditLimit = (decimal)dbReader[1];
                        lastLocation = new Location
                        {
                            Longitude = Convert.ToDouble(dbReader[2]),
                            Latitude = Convert.ToDouble(dbReader[3])
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->Constructor(long)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Limit of amount, for a driver, to be unpaid to the admin 
        /// </summary>
        public decimal CreditLimit
        {
            get
            {
                return creditLimit;
            }
            set
            {
                dbCommand = new SqlCommand("SetCreditLimit", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@creditLimit", System.Data.SqlDbType.Money)).Value = value;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Driver->CreditLimit", ex);
                }
                dbConnection.Close();
                creditLimit = value;
            }
        }
        /// <summary>
        /// Last known Location of the Driver
        /// </summary>
        public Location LastLocation
        {
            get
            {
                return lastLocation;
            }
        }
        /// <summary>
        /// Flag to check if the driver is booked to a ride or not.
        /// </summary>
        public bool IsBooked
        {
            get
            {
                bool isBooked = false;
                dbCommand = new SqlCommand("GetIsBookedDriver", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
                dbConnection.Open();
                try
                {
                    var result = dbCommand.ExecuteScalar();
                    if (result != null)
                    {
                        isBooked = Convert.ToBoolean(result);
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Driver->IsBooked", ex);
                }
                dbConnection.Close();
                return isBooked;
            }
        }
        /// <summary>
        /// Balance of the amount of the driver to service provider
        /// </summary>
        public decimal Balance
        {
            get
            {
                decimal balance = 0;
                dbCommand = new SqlCommand("GetBalanceDriver", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.Money)).Value = id;
                dbConnection.Open();
                try
                {
                    balance = (decimal)dbCommand.ExecuteScalar();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Driver->Balance", ex);
                }
                dbConnection.Close();
                return balance;
            }
        }
        /// <summary>
        /// Returns true if the Balance of the user is greater than the credit limit
        /// </summary>
        public bool IsCleared
        {
            get
            {
                return (Balance > -1 * creditLimit);
            }
        }
        /// <summary>
        /// Method to get the vehicle which owned by the driver
        /// </summary>
        /// <returns></returns>
        public Vehicle GetVehicle()
        {
            Vehicle vehicle;
            dbCommand = new SqlCommand("GetVehicleDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                var result = dbCommand.ExecuteScalar();
                if (result != null)
                {
                    vehicle = new Vehicle(Convert.ToInt32(dbCommand.ExecuteScalar()));
                }
                else
                {
                    vehicle = null;
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->Vehicle", ex);
            }
            dbConnection.Close();
            return vehicle;
        }
        /// <summary>
        /// Method to get all ratings and comments given to the driver
        /// </summary>
        /// <returns></returns>
        public List<RatingAndComments> GetRatingAndComments()
        {
            List<RatingAndComments> lstRatings = new List<RatingAndComments>();
            dbCommand = new SqlCommand("GetRatingsDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@dId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstRatings.Add(new RatingAndComments
                        {
                            Comment = (string)dbReader["Comment"],
                            Rating = (short)dbReader["Rating"],
                            Rider = new Rider((long)dbReader[2])
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->GetRatingAndComments", ex);
            }
            dbConnection.Close();
            return lstRatings;
        }
        /// <summary>
        /// Method to get total rating for the driver
        /// </summary>
        /// <returns></returns>
        public decimal TotalRating
        {
            get
            {
                decimal totalRating = 0;
                dbCommand = new SqlCommand("GetTotalRatingsDriver", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@dId", System.Data.SqlDbType.BigInt)).Value = id;
                dbConnection.Open();
                try
                {
                    totalRating = Convert.ToDecimal(dbCommand.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Driver->GetTotalRating", ex);
                }
                catch (InvalidCastException)
                {

                }
                dbConnection.Close();
                return decimal.Round(totalRating, 2);
            }
        }

        /// <summary>
        /// Method use to add a new or change if already had a vehicle. Changing a vehicle will delete the previous one.
        /// </summary>
        /// <param name="registerationNumber">Registeration Number of the vehicle provided by the government</param>
        /// <param name="model">model or make of the vehicle</param>
        /// <param name="engineCC">Engine of the vehicle</param>
        /// <param name="isAc">Whether if the vehicle is air conditioned</param>
        /// <param name="color">Color of the card</param>
        /// <param name="type">Type of the vehicle</param>
        /// <returns></returns>
        public Vehicle AddOrChangeVehicle(Vehicle.RegisterationNumberFormat registerationNumber, string model, int engineCC, bool isAc, Vehicle.Colors color, VehicleType type)
        {
            //this code will delete previous vehicle information if exists
            dbCommand = new SqlCommand("SetVehicleDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->AddOrChangeVehicle", ex);
            }
            dbConnection.Close();
            //this code will create a new vehicle for the driver
            return new Vehicle(registerationNumber, model, engineCC, isAc, color, type, this);
        }
        /// <summary>
        /// Method to make manual transaction of amount for the driver
        /// </summary>
        /// <param name="amount">Amount to be transacted</param>
        public ManualTransactionLog MakeManualTransaction(decimal amount)
        {
            if (amount > 0)
            {
                return new ManualTransactionLog(amount, DateTime.Now, this);
            }
            else
            {
                throw new InvalidArgumentException("Amount cannot be less than 0 for a Manual Transaction.");
            }
        }
        /// <summary>
        /// Method to get list of Manual Transactions made by the driver
        /// </summary>
        /// <returns>List of Manual Transaction in chronological order</returns>
        public List<ManualTransactionLog> GetManualTransactions()
        {
            List<ManualTransactionLog> lstTransactions = new List<ManualTransactionLog>();
            dbCommand = new SqlCommand("GetManualTransactionsDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstTransactions.Add(new ManualTransactionLog((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->GetManualTransactions", ex);
            }
            dbConnection.Close();
            return lstTransactions;
        }
        /// <summary>
        /// Method to add mobile account transaction information to the database
        /// </summary>
        /// <param name="refNumber">Reference number provided by the Service Provider on transaction</param>
        /// <param name="mobileAccountService">Name of the Service through which the transaction is being processed</param>
        /// <param name="amount">Amount to be transacted</param>
        public MobileAccountTransactionLog MakeMobileAccountTransaction(string refNumber, string mobileAccountService, decimal amount)
        {
            return new MobileAccountTransactionLog(refNumber, DateTime.Now, false, mobileAccountService, amount, this);
        }
        /// <summary>
        /// Method to get a list of transaction made through the mobile account by the driver
        /// </summary>
        /// <returns>List of Mobile Account Transactions in chronological order</returns>
        public List<MobileAccountTransactionLog> GetAllMobileAccountTransactions()
        {
            List<MobileAccountTransactionLog> lstTransactions = new List<MobileAccountTransactionLog>();
            dbCommand = new SqlCommand("GetAllMobileTransactionsDriver", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstTransactions.Add(new MobileAccountTransactionLog((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Driver->GetAllMobileTransactions", ex);
            }
            dbConnection.Close();
            return lstTransactions;
        }
        /// <summary>
        /// Method to be called when driver picks up a ride from a certain location
        /// </summary>
        /// <param name="ride">Ride which is being picked</param>
        /// <param name="pickUpLocation">Location from which the driver picked the ride</param>
        /// <returns></returns>
        public Ride PickUpRide(Ride ride, Location pickUpLocation)
        {
            try
            {
                ride.PickUpLocation = pickUpLocation;
                ride.PickUpTime = DateTime.Now;
                return ride;
            }
            catch (DbQueryProcessingFailedException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Method to check whether if the driver is cleared to Pick-Up a ride
        /// </summary>
        /// <returns></returns>

        /// <summary>
        /// Method to get list of all drivers present in the database
        /// </summary>
        /// <returns>List of drivers in chronological order</returns>
        public static List<Driver> GetAllDrivers()
        {
            List<Driver> lstDrivers = new List<Driver>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("SELECT UId FROM DRIVERS ORDER BY UId DESC", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstDrivers.Add(new Driver((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Drivers->GetAllDrivers", ex);
            }
            dbConnection.Close();
            return lstDrivers;
        }
    }
}