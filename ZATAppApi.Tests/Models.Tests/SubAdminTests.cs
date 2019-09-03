using System.Collections.Generic;
using Xunit;
using ZATAppApi.Models;

namespace ZATAppApi.Tests.Models.Tests
{
    public class SubAdminTests
    {
        [Fact]
        public void MakeSubAdmin_Test()
        {
            //arrange
            User.NameFormat expectedName = new User.NameFormat
            {
                FirstName = "Abdul",
                LastName = "Samad"
            };
            User.ContactNumberFormat expectedContact = new User.ContactNumberFormat("+92", "345", "1234567");
            //act
            SubAdmin actual = new SubAdmin(expectedName, expectedContact);
            //assert
            Assert.StrictEqual(User.ApplicationRoles.SubAdmin, actual.Role);
            Assert.Equal(expectedName, actual.FullName);
            Assert.Equal(expectedContact, actual.ContactNumber);
        }
        [Theory]
        [InlineData("Inner Lahore")]
        [InlineData("Lahore City")]
        [InlineData("Lahore Cantt")]
        public void AddArea_Test(string name)
        {
            //arrange

            //act
            SubAdmin admin = new SubAdmin(10016);

            //assert
            Assert.Equal(name, admin.AddArea(name).AreaName);
        }
        [Fact]
        public void RemoveArea_Test()
        {
            //arrange

            //act
            SubAdmin admin = new SubAdmin(10016);
            admin.RemoveArea(new SubAdmin.Area
            {
                AreaId = 4
            });
            //assert
        }
        [Fact]
        public void GetAllAreas_Test()
        {
            //arrange

            //act
            SubAdmin admin = new SubAdmin(10016);
            List<SubAdmin.Area> actual = admin.GetAllAreas();
            //assert
            Assert.IsType<List<SubAdmin.Area>>(actual);
            Assert.Equal(2, actual.Count);
        }
        [Fact]
        public void AddIdentity_Test()
        {
            //arrange

            //act
            SubAdmin admin = new SubAdmin(10016);
            admin.RegisterIdentityUser(User.ApplicationRoles.SubAdmin, "abdul123", "123456");
            //assert
        }
        [Fact]
        public void GetAllSubAdmins_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<SubAdmin>>(SubAdmin.GetAllSubAdmins());
        }
    }
}
