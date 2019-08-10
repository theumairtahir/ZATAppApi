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
    public class AccountingLogTests
    {
        [Fact]
        public void InitializeAccountingLog_Test()
        {
            //arrange
            decimal expectedDebit = 2546.20m;
            decimal expectedCredit = 41.58m;
            //act
            AccountingLog actual = new AccountingLog(10006);
            //assert
            Assert.Equal(expectedDebit, actual.Debit);
            Assert.Equal(expectedCredit, actual.Credit);
        }
        [Fact]
        public void GetAdminDueCollection_Test()
        {
            //arrange 
            decimal expectedAmount = 0;
            //act
            decimal actual = AccountingLog.GetAdminDueCollection();
            //assert
            Assert.Equal(expectedAmount, actual);
        }
        [Fact]
        public void GetAdminDueCollectionByMonth_Test()
        {
            //arrange
            decimal expected = 0;
            //act
            decimal actual = AccountingLog.GetAdminDueCollectioByMonth(DateTime.Now);
            //assert
            Assert.Equal(expected, actual);
        }
    }
}
