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
        public void AddFare_Test()
        {
            //arrange

            //act
            VehicleType type = new VehicleType(1);
            Fare actual = type.UpdateFare(new VehicleType.FareInfo
            {
                DistanceTravelledPerKmFee = 15,
                DropOffFee = 20,
                Gst = 7,
                PickUpFee = 50,
                ServiceCharges = 20
            });
            //assert
            Assert.IsType<Fare>(actual);
        }
    }
}
