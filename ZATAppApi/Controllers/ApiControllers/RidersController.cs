using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ZATApp.Models;
using ZATAppApi.ApiModels;

namespace ZATAppApi.Controllers.ApiControllers
{
    public class RidersController : ApiController
    {
        /// <summary>
        /// Action returns the information about a rider in the Database
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [ResponseType(typeof(Rider))]
        public IHttpActionResult Get(long id)
        {
            try
            {
                Rider rider = new Rider(id);
                return Ok(rider);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Adds a new rider to the system
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(Rider))]
        public IHttpActionResult Post([FromBody]RiderApiModel value)
        {
            try
            {
                Rider rider = new Rider(value.FullName, new User.ContactNumberFormat(value.CountryCode, value.CompanyCode, value.Number));
                return Ok(rider);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action called if there is need to change the active status of the rider
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Riders/ChangeActiveStatus{id}/{status}")]
        public IHttpActionResult ChangeRiderActiveStatus([FromUri]long id, [FromUri]bool status)
        {
            try
            {
                Rider rider = new Rider(id);
                rider.IsActive = status;
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to be called to book a new ride for a rider
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="vehicleTypeId">Id of the vehicle type for which the ride is being booked</param>
        /// <param name="rideDetails">Details about the ride</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Riders/BookRide/{id}/{vehicleTypeId}")]
        [ResponseType(typeof(Ride))]
        public IHttpActionResult BookRide([FromUri]long id, [FromUri]int vehicleTypeId, [FromBody]Ride.RideBookingDetails rideDetails)
        {
            try
            {
                Rider rider = new Rider(id);
                rideDetails.VehicleType = new VehicleType(vehicleTypeId);
                Ride ride = rider.BookRide(rideDetails);
                return Ok(ride);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to Rate a driver after the completion of the ride
        /// </summary>
        /// <param name="riderId">Primary Key of the Rider</param>
        /// <param name="driverId">Primary Key of the Driver</param>
        /// <param name="rating"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Riders/RateDriver/{riderId}/{driverId}")]
        public IHttpActionResult RateDriver([FromUri]long riderId, [FromUri]long driverId, [FromBody]ZATApp.Models.Common.RatingAndComments rating)
        {
            try
            {
                Rider rider = new Rider(riderId);
                rider.RateDriver(rating, new Driver(driverId));
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to get the sms's received by the rider
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Riders/GetReceivedSms/{id}")]
        [ResponseType(typeof(List<Sms>))]
        public IHttpActionResult GetReceivedSms(long id)
        {
            try
            {
                Rider rider = new Rider(id);
                return Ok(rider.ReceivedSms());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to get the rides completed by the rider
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Riders/GetCompletedRides/{id}")]
        [ResponseType(typeof(List<Ride>))]
        public IHttpActionResult GetCompletedRides(long id)
        {
            try
            {
                Rider rider = new Rider(id);
                return Ok(rider.GetCompletedRides());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
