using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using ZATApp.Models.Exceptions;
using ZATApp.Models.Common;
using System.Configuration;
using ZATApp.Models.ASPNetIdentity;

namespace ZATApp.Models
{
    /// <summary>
    /// A user of the System who books the ride and does related tasks
    /// </summary>
    public class Rider : User
    {
        /// <summary>
        /// constructor which adds a new rider to the database
        /// </summary>
        /// <param name="name">Full Name of the Rider</param>
        /// <param name="contactNumber">Contact Number of the Rider</param>
        public Rider(NameFormat name, ContactNumberFormat contactNumber) : base(name, contactNumber)
        {
            dbCommand = new SqlCommand("INSERT INTO [RIDERS] ([UId]) VALUES (" + id + ")", dbConnection);
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Rider->Constructor(NameFormat, ContactNumberFormat)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Constructor initializes the values from the database
        /// </summary>
        /// <param name="id">Primary key</param>
        public Rider(long id) : base(id)
        {
            if (Role != ApplicationRoles.Rider)
            {
                throw new PrimaryKeyNotForEntityException(Role.ToString(), "Rider");
            }
        }
        /// <summary>
        /// Method will be used to book a ride for the rider
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        public Ride BookRide(Ride.RideBookingDetails details)
        {
            List<Driver> activeDrivers = new List<Driver>(); //initializing a list to store the active drivers
            foreach (var item in Driver.GetAllDrivers())
            {
                //separating active drivers from the others
                if (item.IsActive && !item.IsBooked)
                {
                    activeDrivers.Add(item);
                }
            }
            if (activeDrivers.Count > 0)
            {
                List<DirverDistance> lstDriversDistance = new List<DirverDistance>();
                foreach (var item in activeDrivers)
                {
                    //formula to calculate distance of the active driver by kilo-meters
                    double distance = (6371 * Math.Acos(Math.Cos((Math.PI * details.PickUpLocation.Latitude) / 180.0) * Math.Cos((Math.PI * item.LastLocation.Latitude) / 180.0) * Math.Cos(((Math.PI * item.LastLocation.Longitude) / 180.0) - ((Math.PI * details.PickUpLocation.Longitude) / 180.0)) + Math.Sin((Math.PI * details.PickUpLocation.Latitude) / 180.0) * Math.Sin((Math.PI * item.LastLocation.Latitude) / 180.0)));
                    if (distance < 8) // '8' is the radius of the distance in kilo-meters
                    {
                        lstDriversDistance.Add(new DirverDistance
                        {
                            Distance = distance,
                            Driver = item
                        });
                    }
                }
                if (lstDriversDistance.Count > 0)
                {
                    var temp = lstDriversDistance.OrderBy(x => x.Distance).ToList(); //getting a sorted list of drivers with their distances
                    //temp.Reverse(); //getting list in descending order
                    Driver rideDriver = temp[0].Driver; //The Driver to assign the new ride, which is nearest to the rider
                    return new Ride(DateTime.Now, details.PickUpLocation, details.Destination, this, rideDriver, details.VehicleType);
                }
                else
                {
                    throw new UnsuccessfullProcessException("Rider->BookRide");
                }
            }
            else
            {
                throw new UnsuccessfullProcessException("Rider->BookRide");
            }
        }
        /// <summary>
        /// Method to be used by the rider in order to give ratings to the driver after a successfull ride
        /// </summary>
        /// <param name="rating">The rating to be given</param>
        /// <param name="driver">Driver to be given the ratings</param>
        public void RateDriver(RatingAndComments rating, Driver driver)
        {
            dbCommand = new SqlCommand("AddRatingsRider", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
            dbCommand.Parameters.Add(new SqlParameter("@dId", System.Data.SqlDbType.BigInt)).Value = driver.UserId;
            dbCommand.Parameters.Add(new SqlParameter("@rating", System.Data.SqlDbType.SmallInt)).Value = rating.Rating;
            dbCommand.Parameters.Add(new SqlParameter("@comment", System.Data.SqlDbType.Text)).Value = rating.Comment;
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Rider->RateDriver", ex);
            }
            dbConnection.Close();
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool MatchCredentials(string userName, string password)
        {
            throw new NotImplementedException();
        }
        public override ApplicationUser RegisterIdentityUser(ApplicationRoles role, string username, string password)
        {
            throw new NotImplementedException();
        }
        public override void ChangePassword(string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        /// <summary>
        /// Method to get a list of all riders present in the database
        /// </summary>
        /// <returns>List of Riders in chronological order</returns>
        public static List<Rider> GetAllRiders()
        {
            List<Rider> lstRiders = new List<Rider>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("SELECT [UId] FROM [RIDERS] ORDER BY [UId]", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstRiders.Add(new Rider((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Rider->GetAllRiders", ex);
            }
            dbConnection.Close();
            return lstRiders;
        }
        private class DirverDistance
        {
            public Driver Driver { get; set; }
            public double Distance { get; set; }
        }
    }
}