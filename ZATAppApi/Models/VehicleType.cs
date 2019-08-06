using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ZATApp.Models.Exceptions;

namespace ZATApp.Models
{
    /// <summary>
    /// Type of vehicle i.e. Car, Bike, Auto-Rickshaw
    /// </summary>
    public class VehicleType : DbModel
    {
        private int id;
        private string name;
        /// <summary>
        /// Constructor to initialize values from database
        /// </summary>
        /// <param name="id">Primary Key</param>
        public VehicleType(int id)
        {
            dbCommand = new SqlCommand("GetVehicleType", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.Int)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
                        name = (string)dbReader[1];
                    }

                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("VehicleType->Constructor(int)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Constructor to add new vehicle type to the database
        /// </summary>
        /// <param name="name">Name of the type</param>
        /// <param name="fareInfo">Information of the fare</param>
        public VehicleType(string name, FareInfo fareInfo)
        {
            dbCommand = new SqlCommand("AddNewVehicleType", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = name;
            dbCommand.Parameters.Add(new SqlParameter("@pickupFare", System.Data.SqlDbType.SmallMoney)).Value = fareInfo.PickUpFee;
            dbCommand.Parameters.Add(new SqlParameter("@dropoffFare", System.Data.SqlDbType.SmallMoney)).Value = fareInfo.DropOffFee;
            dbCommand.Parameters.Add(new SqlParameter("@gst", System.Data.SqlDbType.Decimal)).Value = fareInfo.Gst;
            dbCommand.Parameters.Add(new SqlParameter("@sCharges", System.Data.SqlDbType.Decimal)).Value = fareInfo.ServiceCharges;
            dbCommand.Parameters.Add(new SqlParameter("@distanceTravelledPerKmFee", System.Data.SqlDbType.SmallMoney)).Value = fareInfo.DistanceTravelledPerKmFee;
            dbCommand.Parameters.Add(new SqlParameter("@date", System.Data.SqlDbType.Date)).Value = DateTime.Now;
            dbConnection.Open();
            try
            {
                id = (int)dbCommand.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("VehicleType->Constructor(string, FareInfo)", ex);
            }
            dbConnection.Close();
            this.name = name;
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public int TypeId
        {
            get { return id; }
        }
        /// <summary>
        /// Name of the type
        /// </summary>
        public string Name
        {
            get { return name; }
        }
        /// <summary>
        /// Method to update fare information about the vehicle type
        /// </summary>
        /// <param name="fareInfo">Information about fare</param>
        /// <returns></returns>
        public Fare UpdateFare(FareInfo fareInfo)
        {
            return new Fare(fareInfo.PickUpFee, fareInfo.DropOffFee, fareInfo.Gst, fareInfo.ServiceCharges, fareInfo.DistanceTravelledPerKmFee, DateTime.Now, this);
        }
        /// <summary>
        /// Get information about fare associated with the vehicle type currently
        /// </summary>
        /// <returns></returns>
        public Fare GetCurrentFare()
        {
            Fare fare;
            dbCommand = new SqlCommand("GetFareVehicleType", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@vId", System.Data.SqlDbType.Int)).Value = id;
            dbConnection.Open();
            try
            {
                fare = new Fare((int)dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("VehicleType->GetCurrentFare", ex);
            }
            dbConnection.Close();
            return fare;
        }
        /// <summary>
        /// Method to get all vehicle types present in the database
        /// </summary>
        /// <returns></returns>
        public static List<VehicleType> GetAllVehicleTypes()
        {
            List<VehicleType> lstVehicleTypes = new List<VehicleType>();
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            SqlCommand dbCommand = new SqlCommand("GetAllVehicleTypes", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstVehicleTypes.Add(new VehicleType((int)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("VehicleType->GetAllVehicleTypes", ex);
            }
            dbConnection.Close();
            return lstVehicleTypes;
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public struct FareInfo
        {
            public decimal PickUpFee { get; set; }
            public decimal DropOffFee { get; set; }
            public decimal Gst { get; set; }
            public decimal ServiceCharges { get; set; }
            public decimal DistanceTravelledPerKmFee { get; set; }
            public DateTime DateOfInclusion { get; set; }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}