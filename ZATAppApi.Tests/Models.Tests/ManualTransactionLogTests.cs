using System.Collections.Generic;
using Xunit;
using ZATAppApi.Models;

namespace ZATAppApi.Tests.Models.Tests
{
    public class ManualTransactionLogTests
    {
        [Fact]
        public void GetAllTransactions_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<ManualTransactionLog>>(ManualTransactionLog.GetAllTransactions());
        }
    }
}
