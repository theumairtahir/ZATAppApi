using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZATApp.Models;
using Xunit;
using ZATApp.Models.Exceptions;

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
