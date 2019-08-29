using System;
using System.Collections.Generic;
using ZATApp.Models.Common;
using System.Data.SqlClient;
using ZATApp.Models.Exceptions;
using System.Runtime.Serialization;
using System.Linq;

namespace ZATApp.Models
{
    /// <summary>
    /// A ride is the main entity around which the whole system revolves. A rider books the ride and a drive picks it up.
    /// </summary>
    public class Ride : DbModel
    {
        private long id;
        private DateTime pickUpTime;
        private DateTime dropOffTime;
        private bool isEnded;
        private Location pickUpLocation;
        private Location dropOffLocation;
        private Location destination;
        private Driver driver;
        private Rider rider;
        private VehicleType type;
        private DateTime bookingTime;
        private bool isCanceled;
        /// <summary>
        /// Constructor to initialize values from the database
        /// </summary>
        /// <param name="id"></param>
        public Ride(long id)
        {
            dbCommand = new SqlCommand("GetRide", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader = dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        this.id = id;
                        bookingTime = (DateTime)dbReader[1];
                        isEnded = (bool)dbReader["IsEnded"];
                        try
                        {
                            pickUpTime = Convert.ToDateTime(dbReader[2]);
                        }
                        catch (InvalidCastException)
                        {
                            pickUpTime = DateTime.MinValue;
                        }
                        try
                        {
                            dropOffTime = (DateTime)dbReader[3];
                        }
                        catch (InvalidCastException)
                        {
                            dropOffTime = DateTime.MinValue;
                        }
                        try
                        {
                            dropOffLocation = new Location { Latitude = (decimal)dbReader[10], Longitude = (decimal)dbReader[9] };
                        }
                        catch (InvalidCastException)
                        {
                            dropOffLocation = new Location { Latitude = 0, Longitude = 0 };
                        }
                        pickUpLocation = new Location { Longitude = (decimal)dbReader[5], Latitude = (decimal)dbReader[6] };
                        destination = new Location { Longitude = (decimal)dbReader[7], Latitude = (decimal)dbReader[8] };
                        type = new VehicleType((int)dbReader[11]);
                        driver = new Driver((long)dbReader[13]);
                        rider = new Rider((long)dbReader[12]);
                        isCanceled = (bool)dbReader["isCanceled"];
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Ride->Constructor(long)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Constructor to add new ride data to the database
        /// </summary>
        /// <param name="bookingTime">Time at which the ride is booked by hte user</param>
        /// <param name="pickUpLocation">Initial Location from where the driver ahs to pickUp</param>
        /// <param name="destination">Initial settled destination for the ride</param>
        /// <param name="rider">User who booked the ride</param>
        /// <param name="driver">User who picked-up the ride</param>
        /// <param name="type">Vehicle type of which the rider has ordered the ride</param>
        public Ride(DateTime bookingTime, Location pickUpLocation, Location destination, Rider rider, Driver driver, VehicleType type)
        {
            isEnded = false;
            isCanceled = false;
            dbCommand = new SqlCommand("AddNewRide", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@bookingTime", System.Data.SqlDbType.DateTime)).Value = bookingTime;
            dbCommand.Parameters.Add(new SqlParameter("@pickUpLongitude", System.Data.SqlDbType.Decimal)).Value = pickUpLocation.Longitude;
            dbCommand.Parameters.Add(new SqlParameter("@pickUpLatitude", System.Data.SqlDbType.Decimal)).Value = pickUpLocation.Latitude;
            dbCommand.Parameters.Add(new SqlParameter("@destLongitude", System.Data.SqlDbType.Decimal)).Value = destination.Longitude;
            dbCommand.Parameters.Add(new SqlParameter("@destLatitude", System.Data.SqlDbType.Decimal)).Value = destination.Latitude;
            dbCommand.Parameters.Add(new SqlParameter("@isEnded", System.Data.SqlDbType.Bit)).Value = isEnded;
            dbCommand.Parameters.Add(new SqlParameter("@tId", System.Data.SqlDbType.Int)).Value = type.TypeId;
            dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.Decimal)).Value = rider.UserId;
            dbCommand.Parameters.Add(new SqlParameter("@dId", System.Data.SqlDbType.Decimal)).Value = driver.UserId;
            dbConnection.Open();
            try
            {
                id = Convert.ToInt64(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Ride->Constructor(DateTime, DateTime, Location, Location, Rider, Driver, VehicleType)", ex);
            }
            dbConnection.Close();
            this.bookingTime = bookingTime;
            this.pickUpLocation = pickUpLocation;
            this.destination = destination;
            this.rider = rider;
            this.driver = driver;
            this.type = type;
            pickUpTime = DateTime.MinValue;
            dropOffTime = DateTime.MinValue;
        }

        /// <summary>
        /// Type at which the rider booked the ride
        /// </summary>
        [DataMember]
        public DateTime BookingTime
        {
            get
            {
                return bookingTime;
            }
        }
        /// <summary>
        /// Type of vehicle on which the rider wants to ride
        /// </summary>
        [DataMember]
        public VehicleType VehicleType
        {
            get
            {
                return type;
            }
        }
        /// <summary>
        /// User who books the ride
        /// </summary>
        [DataMember]
        public Rider Rider
        {
            get
            {
                return rider;
            }
        }
        /// <summary>
        /// User who picks up the ride
        /// </summary>
        [DataMember]
        public Driver Driver
        {
            get
            {
                return driver;
            }
            set
            {
                dbCommand = new SqlCommand("UpdateDriverRide", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@dId", System.Data.SqlDbType.BigInt)).Value = value.UserId;
                dbConnection.Open();
                try
                {
                    if (dbCommand.ExecuteNonQuery() == 0)
                    {
                        throw new UpdateUnsuccessfulException("Ride->Driver");
                    }
                }
                catch (SqlException ex)
                {
                    throw new DbQueryProcessingFailedException("Ride->Driver", ex);
                }
                dbConnection.Close();
            }
        }

        /// <summary>
        /// Initial location point to which the ride booked the ride
        /// </summary>
        [DataMember]
        public Location Destination
        {
            get
            {
                return destination;
            }
        }
        /// <summary>
        /// Final location at which the rider ends the ride
        /// </summary>
        [DataMember]
        public Location DropOffLocation
        {
            get
            {
                if (isEnded)
                {
                    return dropOffLocation;
                }
                else
                {
                    return new Location { Latitude = 0, Longitude = 0 };
                }
            }
            private set
            {
                if (isEnded)
                {
                    dbCommand = new SqlCommand("SetDropOffLocation", dbConnection);
                    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                    dbCommand.Parameters.Add(new SqlParameter("@dLatitude", System.Data.SqlDbType.Decimal)).Value = value.Latitude;
                    dbCommand.Parameters.Add(new SqlParameter("@dLongitude", System.Data.SqlDbType.Decimal)).Value = value.Longitude;
                    dbConnection.Open();
                    try
                    {
                        dbCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        dbConnection.Close();
                        throw new DbQueryProcessingFailedException("Ride->DropOffLocation", ex);
                    }
                    dbConnection.Close();
                    dropOffLocation = value;
                }
                else
                {
                    throw new NotAuthorizedToChangeValueExeption("Ride->DropOffLocation", "dropOffLocation");
                }
            }
        }
        /// <summary>
        /// Status of the ride. Will be true is the ride get ended
        /// </summary>
        [DataMember]
        public bool IsEnded
        {
            get
            {
                return isEnded;
            }
            private set
            {
                if (value && !isEnded)
                {
                    dbCommand = new SqlCommand("SetIsEndedRide", dbConnection);
                    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                    dbCommand.Parameters.Add(new SqlParameter("@isEnded", System.Data.SqlDbType.Bit)).Value = value;
                    dbConnection.Open();
                    try
                    {
                        dbCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        dbConnection.Close();
                        throw new DbQueryProcessingFailedException("Ride->IsEnded", ex);
                    }
                    dbConnection.Close();
                    isEnded = value;
                }
            }
        }
        /// <summary>
        /// Time at which the driver ended the ride
        /// </summary>
        [DataMember]
        public DateTime DropOffTime
        {
            get
            {
                if (isEnded)
                {
                    return dropOffTime;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            private set
            {
                if (isEnded)
                {
                    dbCommand = new SqlCommand("SetDropOffTimeRide", dbConnection);
                    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                    dbCommand.Parameters.Add(new SqlParameter("@dTime", System.Data.SqlDbType.DateTime)).Value = value;
                    dbConnection.Open();
                    try
                    {
                        dbCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        dbConnection.Close();
                        throw new DbQueryProcessingFailedException("Ride->DropOffTime", ex);
                    }
                    dbConnection.Close();
                    dropOffTime = value;
                }
                else
                {
                    throw new NotAuthorizedToChangeValueExeption("Ride->DropOffTime", "dropOffTime");
                }
            }
        }
        /// <summary>
        /// Time at which the driver picks up the ride
        /// </summary>
        [DataMember]
        public DateTime PickUpTime
        {
            get
            {
                return pickUpTime;
            }
            set
            {
                dbCommand = new SqlCommand("SetPickUpTimeRide", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@pickUpTime", System.Data.SqlDbType.DateTime)).Value = value;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Ride->PickUpTime", ex);
                }
                dbConnection.Close();
                pickUpTime = value;
            }
        }
        /// <summary>
        /// Initial Location from where the driver picks up the ride
        /// </summary>
        [DataMember]
        public Location PickUpLocation
        {
            get
            {
                return pickUpLocation;
            }
            set
            {
                dbCommand = new SqlCommand("SetPickUpLocation", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@pLatitude", System.Data.SqlDbType.Decimal)).Value = value.Latitude;
                dbCommand.Parameters.Add(new SqlParameter("@pLongitude", System.Data.SqlDbType.Decimal)).Value = value.Longitude;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Ride->PickUpLocation", ex);
                }
                dbConnection.Close();
                pickUpLocation = value;
            }
        }
        /// <summary>
        /// Primay Key
        /// </summary>
        [DataMember]
        public long RideId
        {
            get { return id; }
        }
        /// <summary>
        /// Flag, that will be true if the ride has been canceled
        /// </summary>
        [DataMember]
        public bool IsCanceled
        {
            get
            {
                return isCanceled;
            }
            private set
            {
                if (value && !isCanceled)
                {
                    dbCommand = new SqlCommand("SetIsCanceledRide", dbConnection);
                    dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                    dbCommand.Parameters.Add(new SqlParameter("@isCanceled", System.Data.SqlDbType.Bit)).Value = value;
                    dbConnection.Open();
                    try
                    {
                        dbCommand.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        dbConnection.Close();
                        throw new DbQueryProcessingFailedException("Ride->IsCanceled", ex);
                    }
                    dbConnection.Close();
                    isCanceled = value;
                }
                else
                {
                    throw new NotAuthorizedToChangeValueExeption("Ride->IsCanceled", "isCanceled");
                }
            }
        }
        /// <summary>
        /// Active Promo Code for this ride
        /// </summary>
        [DataMember]
        public PromoCode ActivePromo
        {
            get
            {
                PromoCode promo = null;
                dbCommand = new SqlCommand("GetPromoCodeRide", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbConnection.Open();
                try
                {
                    promo = new PromoCode(Convert.ToInt32(dbCommand.ExecuteScalar()));
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Ride->ActivePromo", ex);
                }
                dbConnection.Close();
                return promo;
            }
        }
        /// <summary>
        /// The Route which is used by the ride
        /// </summary>
        [DataMember]
        public RouteDetails Route
        {
            get
            {
                RouteDetails details = new RouteDetails();
                dbCommand = new SqlCommand("GetRouteForRide", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbConnection.Open();
                try
                {
                    using (dbReader = dbCommand.ExecuteReader())
                    {
                        while (dbReader.Read())
                        {
                            details.Cordinates.Add(new Location { Latitude = (decimal)dbReader["Latitude"], Longitude = (decimal)dbReader["Longitude"] });
                        }
                    }
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Ride->Route", ex);
                }
                dbConnection.Close();
                return details;
            }
        }
        /// <summary>
        /// Method to add cordinate to the route of the ride
        /// </summary>
        /// <param name="cordinate"></param>
        public void AddCordinateToRoute(Location cordinate)
        {
            dbCommand = new SqlCommand("AddRoute", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
            dbCommand.Parameters.Add(new SqlParameter("@latitude", System.Data.SqlDbType.Decimal)).Value = cordinate.Latitude;
            dbCommand.Parameters.Add(new SqlParameter("@longitude", System.Data.SqlDbType.Decimal)).Value = cordinate.Longitude;
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Ride->AddCordinateToRoute", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Method to add a promo code to the ride
        /// </summary>
        /// <param name="promo">Promo code to be added</param>
        /// <returns></returns>
        public PromoCode AddPromo(PromoCode promo)
        {
            if (ActivePromo.Code == null && promo.IsOpen)
            {
                dbCommand = new SqlCommand("AddPromoRide", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@pId", System.Data.SqlDbType.Int)).Value = promo.PromoId;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Ride->AddPromo", ex);
                }
                dbConnection.Close();
                return promo;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Method will be called whenever the user wants to cancel the ride
        /// </summary>
        public void CancelRide()
        {
            IsCanceled = true;
        }
        /// <summary>
        /// Method to be called if the user wants to end the ride
        /// </summary>
        /// <param name="dropOffLocation">Location at which the ride ends</param>
        public void EndRide(Location dropOffLocation)
        {
            IsEnded = true;
            DropOffLocation = dropOffLocation;
            DropOffTime = DateTime.Now;
            AddCordinateToRoute(dropOffLocation);
        }
        /// <summary>
        /// Method to get Payment Summary for the ride
        /// </summary>
        /// 
        /// <returns></returns>
        public PaymentSummary GetPaymentSummary()
        {
            if (!isEnded)
            {
                return null;
            }
            Fare fareInfo = type.GetCurrentFare();
            decimal kmDistance = Convert.ToDecimal(Route.TotalDistance);
            decimal kmFare = fareInfo.DistanceTravelledPerKm * kmDistance; //Calculating the fare by multiplying it with perKm fare
            decimal totalFare = fareInfo.PickUpFare + fareInfo.DropOffFare + kmFare;
            if (ActivePromo.Code == null)
            {
                return new PaymentSummary(totalFare, fareInfo.GSTPercent, fareInfo.ServiceChargesPercent);
            }
            else
            {
                return new PaymentSummary(totalFare, fareInfo.GSTPercent, fareInfo.ServiceChargesPercent, ActivePromo.DiscountPercent);
            }
        }
        /// <summary>
        /// Method to be called whenever the rider pays for the ride
        /// Will be un-executed if user tried to pay for more than one time
        /// </summary>
        public PaymentSummary Pay()
        {
            var paymentSummary = GetPaymentSummary();
            if (isEnded)
            {
                dbCommand = new SqlCommand("PayForRide", dbConnection);
                dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
                dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = rider.UserId;
                dbCommand.Parameters.Add(new SqlParameter("@fId", System.Data.SqlDbType.Int)).Value = type.GetCurrentFare().FareId;
                dbCommand.Parameters.Add(new SqlParameter("@rId", System.Data.SqlDbType.BigInt)).Value = id;
                dbCommand.Parameters.Add(new SqlParameter("@totalFare", System.Data.SqlDbType.Money)).Value = paymentSummary.TotalFare;
                dbCommand.Parameters.Add(new SqlParameter("@gst", System.Data.SqlDbType.SmallMoney)).Value = paymentSummary.GST;
                dbCommand.Parameters.Add(new SqlParameter("@serviceCharges", System.Data.SqlDbType.SmallMoney)).Value = paymentSummary.ServiceCharges;
                dbCommand.Parameters.Add(new SqlParameter("@discount", System.Data.SqlDbType.SmallMoney)).Value = paymentSummary.Discount;
                dbConnection.Open();
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    dbConnection.Close();
                    throw new DbQueryProcessingFailedException("Ride->Pay", ex);
                }
                dbConnection.Close();
                //Discount will be paid by the service provider and will be added as a debit to the accounting log 
                //to be manage in the driver's balance 
                new AccountingLog(paymentSummary.CreditAmount, paymentSummary.DebitAmount, driver);
                return paymentSummary;
            }
            else
            {
                throw new UnsuccessfullProcessException("Ride->Pay");
            }
        }
        /// <summary>
        /// Method to transfer ride from current driver to new driver
        /// </summary>
        /// <returns></returns>
        public bool TransferRide()
        {
            if (pickUpTime == DateTime.MinValue)
            {
                List<Driver> activeDrivers = new List<Driver>(); //initializing a list to store the active drivers
                foreach (var item in Driver.GetAllDrivers())
                {
                    //separating active drivers from the others with the type of the ride selected by the user
                    if ((item.IsActive && !item.IsBooked) && item.GetVehicle().Type.TypeId == VehicleType.TypeId)
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
                        double distance = item.LastLocation.DistanceFromAPoint(PickUpLocation);
                        if (distance < 5) // '5' is the radius of the distance in kilo-meters
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
                        Driver = temp[0].Driver; //The Driver to assign the new ride, which is nearest to the rider
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Method to get Estimated Payment Details for a ride
        /// </summary>
        /// <param name="msDistance">Distance in meters</param>
        /// <param name="type">Type of Vehicle for which want the estimation</param>
        /// <returns></returns>
        public static PaymentSummary GetFareEstimation(decimal msDistance, VehicleType type)
        {
            Fare fareInfo = type.GetCurrentFare();
            decimal kmDistance = msDistance / 1000.0m; //Converting meters (ms) into kilometers (kms)
            decimal kmFare = fareInfo.DistanceTravelledPerKm * kmDistance; //Calculating the fare by multiplying it with perKm fare
            decimal totalFare = fareInfo.PickUpFare + fareInfo.DropOffFare + kmFare;
            return new PaymentSummary(totalFare, fareInfo.GSTPercent, fareInfo.ServiceChargesPercent);
        }
        /// <summary>
        /// List of rides which will be active at the current moment
        /// </summary>
        /// <returns></returns>
        public static List<Ride> GetActiveRides()
        {
            List<Ride> lstRides = new List<Ride>();
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("GetActiveRides", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader = dbCommand.ExecuteReader())
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
                throw new DbQueryProcessingFailedException("Ride->GetActiveRides", ex);
            }
            dbConnection.Close();
            return lstRides;
        }
        /// <summary>
        /// Method to get Total Number of completed rides
        /// </summary>
        /// <returns></returns>
        public static long GetTotalCompletedRides()
        {
            long count = 0;
            SqlConnection dbConnection = new SqlConnection(CONNECTION_STRING);
            SqlCommand dbCommand = new SqlCommand("SELECT COUNT(*) FROM RIDES WHERE IsEnded = 'TRUE'", dbConnection);
            dbConnection.Open();
            try
            {
                count = Convert.ToInt64(dbCommand.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("Ride->GetTotalCompletedRides", ex);
            }
            dbConnection.Close();
            return count;
        }
        /// <summary>
        /// Attributes containing the values of payment for a ride
        /// </summary>
        [DataContract]
        public class PaymentSummary
        {
            decimal totalFare, gstPercent, serviceChargesPercent;
            short discountPercent;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public PaymentSummary(decimal totalFare, decimal gstPercent, decimal serviceChargesPercent, short discountPercent = 0)
            {
                this.totalFare = totalFare;
                this.gstPercent = gstPercent;
                this.serviceChargesPercent = serviceChargesPercent;
                this.discountPercent = discountPercent;
            }
            [DataMember]
            public decimal TotalFare
            {
                get
                {
                    return totalFare;
                }
            }
            [DataMember]
            public decimal GST
            {
                get
                {
                    return decimal.Round((totalFare * gstPercent) / 100, 2);
                }
            }
            [DataMember]
            public decimal ServiceCharges
            {
                get
                {
                    return decimal.Round((totalFare * serviceChargesPercent) / 100, 2);
                }
            }
            [DataMember]
            public decimal Discount
            {
                get
                {
                    decimal discount = (totalFare * discountPercent) / 100;
                    return decimal.Round(discount, 2);
                }
            }
            [DataMember]
            public decimal GTotal
            {
                get
                {
                    decimal gTotal = TotalFare - Discount;
                    return decimal.Round(gTotal);
                }
            }
            [DataMember]
            public decimal DriverAmount
            {
                get
                {
                    return TotalFare - ServiceCharges - GST;
                }
            }
            [DataMember]
            public decimal CreditAmount
            {
                get
                {
                    return ServiceCharges + GST;
                }
            }
            [DataMember]
            public decimal DebitAmount
            {
                get
                {
                    return Discount;
                }
            }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
        /// <summary>
        /// Attributes containing the values neccessary to book a ride
        /// </summary>
        [DataContract]
        public class RideBookingDetails
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            [DataMember]
            public Location PickUpLocation { get; set; }
            [DataMember]
            public Location Destination { get; set; }
            [DataMember]
            public VehicleType VehicleType { get; set; }
        }
        [DataContract]
        public class RouteDetails
        {
            private List<Location> lstCordinates;
            internal RouteDetails()
            {
                lstCordinates = new List<Location>();
            }
            [DataMember]
            public List<Location> Cordinates
            {
                get { return lstCordinates; }
                set { lstCordinates = value; }
            }
            /// <summary>
            /// Method to get total distance of the route
            /// </summary>
            /// <returns></returns>
            [DataMember]
            public double TotalDistance
            {
                get
                {
                    double distance = 0;
                    for (int i = 0; i < lstCordinates.Count - 1; i++)
                    {
                        try
                        {
                            //getting two points between those the distance will be calculated 
                            Location current = lstCordinates[i];
                            Location next = lstCordinates[i + 1];
                            double pointDistance = Location.Distance(current, next);//Distance between two points in kilometers
                            distance += pointDistance;
                        }
                        catch (Exception)
                        {
                            distance += 0;
                        }
                    }
                    return distance;
                }
            }
        }
        private class DirverDistance
        {
            public Driver Driver { get; set; }
            public double Distance { get; set; }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}