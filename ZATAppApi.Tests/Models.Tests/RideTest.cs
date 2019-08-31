using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZATAppApi.Models;
using ZATAppApi.Models.Exceptions;
using Xunit;
using ZATAppApi.Models.Common;

namespace ZATAppApi.Tests.Models.Tests
{
    public class RideTest
    {
        [Fact]
        public void CancelRide_Test()
        {
            //arrange

            //act
            Ride actual = new Ride(8);
            actual.CancelRide();
            //assert
            Assert.False(actual.Driver.IsBooked);
        }
        [Fact]
        public void AddPromo_Test()
        {
            //arrange
            PromoCode expected = new PromoCode(2);
            //act
            Ride ride = new Ride(1);
            PromoCode actual = ride.AddPromo(expected);
            //assert
            Assert.StrictEqual(expected, actual);
        }
        [Fact]
        public void AddPromo_TestForRideAlreadyHasPromo()
        {
            //arrange
            PromoCode expected = null;
            //act
            Ride ride = new Ride(1);
            PromoCode actual = ride.AddPromo(new PromoCode(2));
            //assert
            Assert.Null(actual);
        }
        [Fact]
        public void EndRide_Test()
        {
            //arrange
            Location expectedDropOffLocation = new Location
            {
                Latitude = 31.390357m,
                Longitude = 74.275457m
            };
            //act
            Ride ride = new Ride(1);
            ride.EndRide(expectedDropOffLocation);
            Location actual = ride.DropOffLocation;
            //assert
            Assert.True(ride.IsEnded);
            Assert.StrictEqual(expectedDropOffLocation, actual);
            Assert.False(ride.Driver.IsBooked);
        }
        [Fact]
        public void GetPaymentSummary_Test()
        {
            //arrange
            decimal expected = 108m;
            //act
            Ride ride = new Ride(1);
            Ride.PaymentSummary actual = ride.GetPaymentSummary();
            //assert
            Assert.Equal(expected, actual.GTotal, 1);
        }
        [Fact]
        public void Pay_Test()
        {
            Ride ride = new Ride(1);
            //arrange
            decimal expectedBalance = ride.Driver.Balance;
            Ride.PaymentSummary paymentSummary = ride.Pay();
            expectedBalance += paymentSummary.DebitAmount - paymentSummary.CreditAmount;
            //act

            decimal actualBalance = ride.Driver.Balance;
            //assert
            Assert.Equal(expectedBalance, actualBalance);
        }
        [Fact]
        public void GetFareEstimation_Test()
        {
            //arrange
            decimal expected = 154;
            //act
            decimal actual = Ride.GetFareEstimation(5600, new VehicleType(1)).TotalFare;
            //assert
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void GetActiveRides_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<Ride>>(Ride.GetActiveRides());
        }
        [Fact]
        public void GetCompletedRides_Test()
        {
            //arrange
            long expected = 1;
            //act
            long actual = Ride.GetTotalCompletedRides();
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
