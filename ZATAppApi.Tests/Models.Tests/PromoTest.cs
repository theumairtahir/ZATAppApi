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
        [Fact]
        public void IsPromoUsed_Test()
        {
            //arrange

            //act
            PromoCode promo = new PromoCode(2);
            //assert
            Assert.True(promo.IsUsed(new Rider(10)));
        }
        [Fact]
        public void GetPromoCodeByCode_Test()
        {
            //arrange
            PromoCode expected = new PromoCode(2);
            //act
            PromoCode actual = PromoCode.GetPromoCode("TestPromo");
            //assert
            Assert.Equal(expected.PromoId, actual.PromoId);
        }
        [Fact]
        public void GetTotalDiscountsByMonth_Test()
        {
            //arrange
            decimal expected = 46.2m;
            //act
            decimal actual = PromoCode.GetTotalDiscountsByMonth(DateTime.Now);
            //assert
            Assert.Equal(decimal.Round(expected), actual);
        }
        [Fact]
        public void GetAllPromoCodes_Test()
        {
            //arrange

            //act

            //assert

            Assert.IsType<List<PromoCode>>(PromoCode.GetAllPromoCodes());
        }
    }
}
