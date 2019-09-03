using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ZATAppApi.Models;
using ZATAppApi.Models.Common;

namespace ZATAppApi.Controllers.ApiControllers
{
    public class RidesController : ApiController
    {
        /// <summary>
        /// Action to get information of the ride
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<Ride>))]
        public IHttpActionResult Get(long id)
        {
            try
            {
                Ride ride = new Ride(id);
                return Ok(ride);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to add cordinates to the route of a ride
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="cordinates">Location cordinates of the position</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Rides/{id}/AddRoute")]
        public IHttpActionResult AddRoute([FromUri]long id, [FromUri]Location cordinates)
        {
            try
            {
                Ride ride = new Ride(id);
                ride.AddCordinateToRoute(cordinates);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to add a promo code to the ride
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="code">Promo Code</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(PromoCode))]
        [Route("api/Rides/{id}/AddPromo/{code}")]
        public IHttpActionResult AddPromo(long id, string code)
        {
            try
            {
                Ride ride = new Ride(id);
                return Ok(ride.AddPromo(PromoCode.GetPromoCode(code)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action will be called to cancel a ride
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Rides/{id}/CancelRide")]
        public IHttpActionResult CancelRide(long id)
        {
            try
            {
                Ride ride = new Ride(id);
                ride.CancelRide();
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action will be called to end a ride 
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="dropOffLocation">Location Point at which the ride is ended</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Rides/{id}/EndRide")]
        public IHttpActionResult EndRide([FromUri]long id, [FromUri]Location dropOffLocation)
        {
            try
            {
                Ride ride = new Ride(id);
                ride.EndRide(dropOffLocation);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to be called to get the payment summary of the ride
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Ride.PaymentSummary))]
        [Route("api/Rides/{id}/GetPaymentSummary")]
        public IHttpActionResult GetPaymentSummary([FromUri] long id)
        {
            try
            {
                Ride ride = new Ride(id);
                return Ok(ride.GetPaymentSummary());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action will be called to pay for the ride
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Ride.PaymentSummary))]
        [Route("api/Rides/{id}/Pay")]
        public IHttpActionResult Pay(long id)
        {
            try
            {
                Ride ride = new Ride(id);
                return Ok(ride.Pay());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action will perform a transfer ride method
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(bool))]
        [Route("api/Rides/{id}/TransferRide")]
        public IHttpActionResult TransferRide(long id)
        {
            try
            {
                Ride ride = new Ride(id);
                return Ok(ride.TransferRide());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action will send the Fare Estimation as a response
        /// </summary>
        /// <param name="distance">Distance in meters</param>
        /// <param name="vehicleTypeId">Primary Key of the vehicle type</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Ride.PaymentSummary))]
        [Route("api/Rides/EstimateFare/{distance}/{vehicleTypeId}")]
        public IHttpActionResult EstimateFare(decimal distance, int vehicleTypeId)
        {
            try
            {
                return Ok(Ride.GetFareEstimation(distance, new VehicleType(vehicleTypeId)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
