using System.Collections.Generic;
using Xunit;
using ZATAppApi.Models;

namespace ZATAppApi.Tests.Models.Tests
{
    public class FareTests
    {
        [Fact]
        public void GetAllFares_Tests()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<Fare>>(Fare.GetAllFares());
        }
    }
}
