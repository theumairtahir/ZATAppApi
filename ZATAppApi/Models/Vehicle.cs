using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ZATApp.Models.Exceptions
{
    /// <summary>
    /// Driver has a vehicle by which it picks up the ride
    /// </summary>
    public class Vehicle : DbModel
    {
        private int id;
        private bool isAc;
        private RegisterationNumberFormat registerationNumber;
        private string model;
        private Colors vehicleColor;
        private short engineCC;
        private Driver driver;
        private VehicleType type;
        /// <summary>
        /// Constructor to add a new vehicle
        /// </summary>
        /// <param name="registerationNumber">Registeration Number of the vehicle provided by the government</param>
        /// <param name="model">model or make of the vehicle</param>
        /// <param name="engineCC">Engine of the vehicle</param>
        /// <param name="isAc">Whether if the vehicle is air conditioned</param>
        /// <param name="color">Color of the Vehicle</param>
        /// <param name="type">Type of the vehicle</param>
        /// <param name="driver">Driver who owns the vehicle</param>
        public Vehicle(RegisterationNumberFormat registerationNumber, string model, int engineCC, bool isAc, Colors color, VehicleType type, Driver driver)
        {
            dbCommand = new SqlCommand("AddNewVehicle", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = driver.UserId;
            dbCommand.Parameters.Add(new SqlParameter("@rNum", System.Data.SqlDbType.SmallInt)).Value = registerationNumber.Number;
            dbCommand.Parameters.Add(new SqlParameter("@rAlpha", System.Data.SqlDbType.VarChar)).Value = registerationNumber.Alphabets;
            dbCommand.Parameters.Add(new SqlParameter("@rYear", System.Data.SqlDbType.SmallInt)).Value = registerationNumber.Year;
            dbCommand.Parameters.Add(new SqlParameter("@model", System.Data.SqlDbType.VarChar)).Value = model;
            dbCommand.Parameters.Add(new SqlParameter("@color", System.Data.SqlDbType.Int)).Value = color;
            dbCommand.Parameters.Add(new SqlParameter("@engine", System.Data.SqlDbType.SmallInt)).Value = engineCC;
            dbCommand.Parameters.Add(new SqlParameter("@isAc", System.Data.SqlDbType.Bit)).Value = isAc;
            dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.Int)).Value = type.TypeId;
            dbConnection.Open();
            try
            {
                id = Convert.ToInt32(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Vehicle->Constructor(RegisterationNumberFormat, string, int, bool, Colors, VehicleType, Driver)", ex);
            }
            dbConnection.Close();
            this.registerationNumber = registerationNumber;
            this.engineCC = (short)engineCC;
            this.model = model;
            this.isAc = isAc;
            vehicleColor = color;
            this.driver = driver;
            this.type = type;
        }
        /// <summary>
        /// Constructor to initialize values from the database
        /// </summary>
        /// <param name="id">Primary Key</param>
        public Vehicle(int id)
        {
            dbCommand = new SqlCommand("GetVehicle", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@vId", System.Data.SqlDbType.Int)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = (int)dbReader[0];
                        registerationNumber = new RegisterationNumberFormat((string)dbReader["RAlphabets"], (short)dbReader["RNumber"], (short)dbReader["RYear"]);
                        model = (string)dbReader[4];
                        vehicleColor = (Colors)dbReader[5];
                        engineCC = (short)dbReader[6];
                        isAc = (bool)dbReader[7];
                        driver = new Driver((long)dbReader[8]);
                        type = new VehicleType((int)dbReader[9]);
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Vehicle->Consturctor(int)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Type to which the vehicle belongs
        /// </summary>
        public VehicleType Type
        {
            get { return type; }
            set
            {
                dbCommand = new SqlCommand("SetTypeVehicle", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@vId", System.Data.SqlDbType.Int)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.Int)).Value = value.TypeId;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Vehicle->Type", ex);
                }
                dbConnection.Close();
                type = value;
            }
        }
        /// <summary>
        /// Driver who owns the car
        /// </summary>
        public Driver Driver
        {
            get { return driver; }
        }

        /// <summary>
        /// Engine power of the vehicle
        /// </summary>
        public Engines EngineCC
        {
            get { return (Engines)engineCC; }
        }
        /// <summary>
        /// Color of the Vehicle
        /// </summary>
        public Colors VehicleColor
        {
            get { return vehicleColor; }
        }

        /// <summary>
        /// Model or make of the car
        /// </summary>
        public string Model
        {
            get { return model; }
        }
        /// <summary>
        /// Registeration Number provided by the government
        /// </summary>
        public RegisterationNumberFormat RegisterationNumber
        {
            get { return registerationNumber; }
        }
        /// <summary>
        /// Value indicates the air condition in the vehicle
        /// </summary>
        public bool IsAC
        {
            get { return isAc; }
            set
            {
                dbCommand = new SqlCommand("SetIsACVehicle", dbConnection);
                dbCommand.Parameters.Add(new SqlParameter("@vId", System.Data.SqlDbType.Int)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@isAc", System.Data.SqlDbType.Bit)).Value = value;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Vehicle->IsAC", ex);
                }
                dbConnection.Close();
                isAc = value;
            }
        }
        /// <summary>
        /// Primary Key
        /// </summary>
        public int VehicleId
        {
            get { return id; }
        }
        /// <summary>
        /// Method to get all vehicles present in the database
        /// </summary>
        /// <returns></returns>
        public static List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> lstVehicle = new List<Vehicle>();
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            SqlCommand dbCommand = new SqlCommand("SELECT VehicleId FROM [VEHICLES]", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstVehicle.Add(new Vehicle((int)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Vehicle->GetAllVehicles", ex);
            }
            dbConnection.Close();
            return lstVehicle;
        }
        /// <summary>
        /// Vehicle's Registeration Number Format
        /// </summary>
        public class RegisterationNumberFormat
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            private short number;
            private string alphabets;
            private short year;
            public RegisterationNumberFormat(string alphabets, short number, short year)
            {
                if (alphabets.Length > 3)
                {
                    throw new ValueLengthExceedsException(alphabets, 3);
                }
                this.alphabets = alphabets;
                this.number = number;
                this.year = year;
            }

            public short Year
            {
                get { return year; }
                set { year = value; }
            }

            public short Number
            {
                get { return number; }
                set { number = value; }
            }

            public string Alphabets
            {
                get
                {
                    return alphabets.ToUpper();
                }
                set { alphabets = value; }
            }
            public string GetFormattedNumber()
            {
                return Alphabets + "-" + number + "-" + year;
            }

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
        /// <summary>
        /// Vehicle's Colors
        /// </summary>
        public enum Colors
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            White = 0xffffff,
            Red = 0xff0000,
            Grey = 0x666666,
            Black = 0x000000,
            Yellow = 0xffff00,
            Green = 0x00ff00,
            Blue = 0x0000ff,
            Sliver = 0xcccccc,
            Golden = 0xffcc00,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
        /// <summary>
        /// Engine's CC for a vehicle 
        /// </summary>
        public enum Engines
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            CC70 = 70,
            CC100 = 100,
            CC125 = 125,
            CC150 = 150,
            CC200 = 200,
            CC250 = 250,
            CC600 = 660,
            CC800 = 800,
            CC1000 = 1000,
            CC1300 = 1300,
            CC1500 = 1500,
            CC1700 = 1700,
            CC1800 = 1800,
            CC2000 = 2000,
            CC2200 = 2200,
            CC2400 = 2400
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
    }
}