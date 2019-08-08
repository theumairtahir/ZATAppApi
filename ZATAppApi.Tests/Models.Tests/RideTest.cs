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
    public class RideTest
    {
        [Fact]
        public void CancelRide_Test()
        {
            //arrange

            //act
            Ride actual = new Ride(5);
            actual.CancelRide();
            //assert
            Assert.False(actual.Driver.IsBooked);
        }
    }
}
