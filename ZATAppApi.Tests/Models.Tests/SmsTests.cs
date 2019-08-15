using System.Collections.Generic;
using Xunit;
using ZATApp.Models;

namespace ZATAppApi.Tests.Models.Tests
{
    public class SmsTests
    {
        [Theory]
        [InlineData(1, "Hello! this is a test SMS")]
        [InlineData(2, "Hello! this is a test SMS")]
        [InlineData(3, "Hello! this is a test SMS")]
        [InlineData(4, "Hello! this is a test SMS")]
        [InlineData(5, "Hello! this is a test SMS")]
        [InlineData(6, "Hello! this is a test SMS")]
        [InlineData(17, "Hello! this is a test SMS")]
        [InlineData(18, "Hello! this is a test SMS")]
        [InlineData(10002, "Hello! this is a test SMS")]
        [InlineData(10003, "Hello! this is a test SMS")]
        [InlineData(10004, "Hello! this is a test SMS")]
        [InlineData(10005, "Hello! this is a test SMS")]
        [InlineData(10006, "Hello! this is a test SMS")]
        public void GetSms_Test(long expectedId, string expectedBody)
        {
            Sms sms = new Sms(expectedId);
            //arrange

            //act
            long actualId = sms.SmsId;
            string actualBody = sms.Body;
            //assert
            Assert.Equal(expectedId, actualId);
            Assert.Equal(expectedBody, actualBody);
        }
        [Theory]

        [InlineData(9, 7)]
        [InlineData(9, 11)]
        [InlineData(9, 14)]
        [InlineData(9, 15)]
        [InlineData(9, 18)]
        [InlineData(9, 10002)]
        [InlineData(9, 10007)]
        [InlineData(10, 5)]
        [InlineData(10, 8)]
        [InlineData(10, 9)]
        [InlineData(10, 12)]
        [InlineData(10, 17)]
        [InlineData(10, 20)]
        [InlineData(10, 10004)]
        [InlineData(10, 10006)]
        [InlineData(11, 6)]
        [InlineData(11, 10)]
        [InlineData(11, 13)]
        [InlineData(11, 16)]
        [InlineData(11, 19)]
        [InlineData(11, 10003)]
        [InlineData(11, 10005)]
        public void GetReceivers_Test(long expectedUserId, long expectedSmsId)
        {
            Sms sms = new Sms(expectedSmsId);
            //arrange

            //act
            long actualUserId = sms.GetReceivers()[0].UserId;
            long actualSmsId = sms.SmsId;
            //assert
            Assert.Equal(expectedSmsId, actualSmsId);
            Assert.Equal(expectedUserId, actualUserId);
        }
        [Fact]
        public void GetAllSms_Test()
        {
            //arrange

            //act

            //assert

            Assert.IsType<List<Sms>>(Sms.GetAllSms());
        }
    }
}
