using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using ZATApp.Models;
using ZATApp.Models.Common;

namespace ZATAppApi.Controllers.ApiControllers
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class DriversController : ApiController
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Action to get information of a driver 
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Driver))]
        public IHttpActionResult Get(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to change the password for a driver
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="oldPassword">Old Password</param>
        /// <param name="newPassword">New Password</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Drivers/{id}/ChangePassword/{oldPassword}/{newPassword}")]
        public IHttpActionResult ChangePassword([FromUri]long id, [FromUri]string oldPassword, [FromUri]string newPassword)
        {
            try
            {
                Driver driver = new Driver(id);
                driver.ChangePassword(oldPassword, newPassword);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to check the driver's credentials
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Drivers/{id}/MatchCredentials/{username}/{password}")]
        [ResponseType(typeof(bool))]
        public IHttpActionResult MatchCredentials([FromUri] long id, [FromUri]string username, [FromUri]string password)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.MatchCredentials(username, password));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to get all mobile account transactions done by the driver
        /// </summary>
        /// <param name="id">Primary Key for Driver</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<MobileAccountTransactionLog>))]
        [Route("api/Drivers/{id}/GetAllMobileTransactions")]
        public IHttpActionResult GetAllMobileTransactions(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.GetAllMobileAccountTransactions());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to get all mobile account transactions done by the driver
        /// </summary>
        /// <param name="id">Primary Key for Driver</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<ManualTransactionLog>))]
        [Route("api/Drivers/{id}/GetAllManualTransactions")]
        public IHttpActionResult GetAllManualTransactions(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.GetManualTransactions());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to get All Rides Completed by the Driver
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<Ride>))]
        [Route("api/Drivers/{id}/GetCompletedRides")]
        public IHttpActionResult GetCompletedRides(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.GetCompletedRides());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action which will return Ratings and comments relating to the driver
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<RatingAndComments>))]
        [Route("api/Drivers/{id}/GetRatingsAndComments")]
        public IHttpActionResult GetRatingAndComments(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.GetRatingAndComments());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to get the vehicle details of the driver
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Vehicle))]
        [Route("api/Drivers/{id}/GetVehicle")]
        public IHttpActionResult GetVehicle(long id)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.GetVehicle());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to make a transaction via mobile account
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <param name="refNumber">Reference Number of the transaction (Provided by the Mobile Account Service)</param>
        /// <param name="mobileAccountService">Mobile Account Service Provider Name</param>
        /// <param name="amount">Amount of transaction</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(MobileAccountTransactionLog))]
        [Route("api/Drivers/{id}/MakeMobileAccountTransaction/{refNumber}/{mobileAccountService}/{amount}")]
        public IHttpActionResult MakeMobileTransaction(long id, string refNumber, string mobileAccountService, decimal amount)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.MakeMobileAccountTransaction(refNumber, mobileAccountService, amount));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        /// <summary>
        /// Action to be used to pick up a ride by the driver from a particular point
        /// </summary>
        /// <param name="id">Pirmary Key</param>
        /// <param name="rideId">Primary Key for the ride to be picked up</param>
        /// <param name="pickUpLocation">Location from where is the ride is being picked up</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(Ride))]
        [Route("api/Drivers/{id}/PickUpRide/{rideId}")]
        public IHttpActionResult PickUpRide([FromUri]long id, [FromUri]long rideId, [FromBody]Location pickUpLocation)
        {
            try
            {
                Driver driver = new Driver(id);
                return Ok(driver.PickUpRide(new Ride(rideId), pickUpLocation));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
