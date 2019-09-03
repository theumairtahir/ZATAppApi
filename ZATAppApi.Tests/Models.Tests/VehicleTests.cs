using System.Collections.Generic;
using ZATAppApi.Models;
using Xunit;

namespace ZATAppApi.Tests.Models.Tests
{
    public class VehicleTests
    {
        [Fact]
        public void GetAllVehicles_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<Vehicle>>(Vehicle.GetAllVehicles());
        }
    }
}
