using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZATApp.Models;
using ZATApp.Models.Exceptions;
using Xunit;

namespace ZATAppApi.Tests.Models.Tests
{
    public class VehicleTypeTest
    {
        [Fact]
        public void AddOrUpdateFare_Test()
        {
            //arrange

            //act
            VehicleType type = new VehicleType(1);
            Fare actual = type.UpdateFare(new VehicleType.FareInfo
            {
                DistanceTravelledPerKmFee = 50,
                DropOffFee = 22,
                Gst = 12,
                PickUpFee = 50,
                ServiceCharges = 20
            });
            //assert
            Assert.IsType<Fare>(actual);
        }
        [Fact]
        public void AddNewVehicleType_Test()
        {
            //arrange
            VehicleType.FareInfo expected = new VehicleType.FareInfo
            {
                DistanceTravelledPerKmFee = 40,
                DropOffFee = 20,
                PickUpFee = 40,
                Gst = 5,
                ServiceCharges = 15
            };
            //act
            VehicleType actual = new VehicleType("Auto-Rickshaw", expected);
            //assert
            Assert.Equal(expected.DistanceTravelledPerKmFee, actual.GetCurrentFare().DistanceTravelledPerKm);
        }
        [Fact]
        public void GetCurrentFare_Test()
        {
            //arrange
            Fare expected = new Fare(9);
            //act
            Fare actual = new VehicleType(1).GetCurrentFare();
            //assert
            Assert.StrictEqual(expected.FareId, actual.FareId);
        }
        [Fact]
        public void GetAllVehicleTypes_Test()
        {
            //arrange

            //act

            //assert

            Assert.IsType<List<VehicleType>>(VehicleType.GetAllVehicleTypes());
        }
    }
}
