using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ZATApp.Models.Exceptions;

namespace ZATApp.Models
{
    /// <summary>
    /// Fare is associated with every type of ride, will be taken after the ride ends
    /// </summary>
    public class Fare : DbModel
    {
        private int id;
        private decimal pickUpFare;
        private decimal dropOffFare;
        private decimal gst;
        private decimal serviceCharges;
        private decimal distanceTravelled;
        private DateTime date;
        private VehicleType type;
        /// <summary>
        /// Constructor to add new fare info to the database
        /// </summary>
        /// <param name="pickUpFare">Fare to be due on a ride pick-up</param>
        /// <param name="dropOffFare">Fare to be due on a ride drop-off</param>
        /// <param name="gstPercent">Percent of government applied tax on the ride</param>
        /// <param name="serviceChargePercent">Percent of charges taken by the service provider on every ride</param>
        /// <param name="distanceTravelledPerKmFee">Amount to be taken on every km of ride</param>
        /// <param name="date">Date on which the fare updated</param>
        /// <param name="type">Type of vehicle on which the fare is applicable</param>
        public Fare(decimal pickUpFare, decimal dropOffFare, decimal gstPercent, decimal serviceChargePercent, decimal distanceTravelledPerKmFee, DateTime date, VehicleType type)
        {
            dbCommand = new SqlCommand("AddNewFare", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@pickupFare", System.Data.SqlDbType.SmallMoney)).Value = pickUpFare;
            dbCommand.Parameters.Add(new SqlParameter("@dropoffFare", System.Data.SqlDbType.SmallMoney)).Value = dropOffFare;
            dbCommand.Parameters.Add(new SqlParameter("@gst", System.Data.SqlDbType.Decimal)).Value = gstPercent;
            dbCommand.Parameters.Add(new SqlParameter("@sCharges", System.Data.SqlDbType.Decimal)).Value = serviceChargePercent;
            dbCommand.Parameters.Add(new SqlParameter("@distanceTravelledPerKmFee", System.Data.SqlDbType.SmallMoney)).Value = distanceTravelledPerKmFee;
            dbCommand.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.Date)).Value = date;
            dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.Int)).Value = type.TypeId;
            dbConnection.Open();
            try
            {
                id = Convert.ToInt32(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("VehicleType->Constructor(string, FareInfo)", ex);
            }
            dbConnection.Close();
            this.pickUpFare = pickUpFare;
            this.dropOffFare = dropOffFare;
            gst = gstPercent;
            serviceCharges = serviceChargePercent;
            distanceTravelled = distanceTravelledPerKmFee;
            this.date = date;
            this.type = type;
        }
        /// <summary>
        /// Constructor ro initialize values from the database
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Fare(int id)
        {
            dbCommand = new SqlCommand("GetFare", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@fId", System.Data.SqlDbType.Int)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
                        pickUpFare = (decimal)dbReader[1];
                        dropOffFare = (decimal)dbReader[2];
                        gst = (decimal)dbReader[3];
                        serviceCharges = (decimal)dbReader[4];
                        distanceTravelled = (decimal)dbReader[5];
                        date = (DateTime)dbReader[6];
                        type = new VehicleType((int)dbReader[7]);
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Fare->Constructor(int)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Type of vehicle on which the fare is applicable
        /// </summary>
        public VehicleType VehicleType
        {
            get
            {
                return type;
            }
        }
        /// <summary>
        /// Date on which the fare updated for the vehicle type
        /// </summary>
        public DateTime Date
        {
            get { return date; }
        }
        /// <summary>
        /// Amount taken on per km distance of ride
        /// </summary>
        public decimal DistanceTravelledPerKm
        {
            get { return distanceTravelled; }
        }
        /// <summary>
        /// Serivce Charges percentage will be duducted by the admin
        /// </summary>
        public decimal ServiceChargesPercent
        {
            get { return serviceCharges; }
        }
        /// <summary>
        /// Percentage of Governemnt applied genral sales tax
        /// </summary>
        public decimal GSTPercent
        {
            get { return gst; }
        }
        /// <summary>
        /// Fixed amount taken on every drop-off of the ride
        /// </summary>
        public decimal DropOffFare
        {
            get { return dropOffFare; }
        }
        /// <summary>
        /// Fixed amount taken on every pick-up of the ride
        /// </summary>
        public decimal PickUpFare
        {
            get { return pickUpFare; }
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public int FareId
        {
            get { return id; }
        }
        /// <summary>
        /// Static Method to get all fares present in the database
        /// </summary>
        /// <returns>List of Fares in chronological order</returns>
        public static List<Fare> GetAllFares()
        {
            List<Fare> lstFares = new List<Fare>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("SELECT FareId FROM FARES ORDER BY FareId DESC", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstFares.Add(new Fare((int)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Fare->GetAllFares", ex);
            }
            dbConnection.Close();
            return lstFares;
        }
    }
}