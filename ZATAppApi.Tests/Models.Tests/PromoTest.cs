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
    public class PromoTest
    {
        [Fact]
        public void CreateNewPromo_Test()
        {
            //arrange
            string expectedCode = "TestPromo";
            short expectedDiscount = 30;
            //act
            PromoCode actual = new PromoCode(expectedCode, expectedDiscount);
            //assert
            Assert.Equal(expectedCode, actual.Code);
            Assert.Equal(expectedDiscount, actual.DiscountPercent);
        }
        [Fact]
        public void CreateNewPromo_TestForUnUniqueCode()
        {
            //arrange

            //act
            //PromoCode actual = new PromoCode("TestPromo", 50);
            //assert
            Assert.Throws<UniqueKeyViolationException>(() => new PromoCode("TestPromo", 50));
        }
    }
}
