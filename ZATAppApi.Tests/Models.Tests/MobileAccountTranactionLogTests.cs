using System.Collections.Generic;
using Xunit;
using ZATAppApi.Models;
using ZATAppApi.Models.Exceptions;

namespace ZATAppApi.Tests.Models.Tests
{
    public class MobileAccountTranactionLogTests
    {
        [Fact]
        public void VerifyTransaction_Test()
        {
            //arrange
            Driver driver = new Driver(10007);
            decimal expected = driver.Balance + 570.13m;
            //act
            MobileAccountTransactionLog mobileLog = new MobileAccountTransactionLog(1);
            mobileLog.IsVerified = true;
            decimal actual = driver.Balance;
            //assert
            Assert.True(mobileLog.IsVerified);
            Assert.Equal(expected, actual, 0);
        }
        [Fact]
        public void VerifyTransaction_TestForAlreadyVerified()
        {
            //arrange

            //act
            MobileAccountTransactionLog mobileLog = new MobileAccountTransactionLog(1);
            //assert
            Assert.Throws<NotAuthorizedToChangeValueExeption>(() => mobileLog.IsVerified = true);
        }
        [Fact]
        public void GetAllTransactions_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<MobileAccountTransactionLog>>(MobileAccountTransactionLog.GetAllMobileAccountTransactions());
        }
        [Fact]
        public void GetAllUnverifiedTransactions_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<MobileAccountTransactionLog>>(MobileAccountTransactionLog.GetAllUnverifiedMobileAccountTransactions());
        }
    }
}
