using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZATAppApi.Models;
using Xunit;
using System.Threading;
using ZATAppApi.Models.Exceptions;
using ZATAppApi.Models.Common;
using ZATAppApi.Models.ASPNetIdentity;

namespace ZATAppApi.Tests.Models.Tests
{
    public class UserTests
    {
        [Theory]
        [InlineData("Umar", "Tahir", "+92", "314", "4246498")]
        [InlineData("Tayyab", "Tahir", "+92", "332", "4246498")]
        [InlineData("Usman", "Tahir", "+92", "332", "4313586")]
        public void AddRider_Test(string firstName, string lastName, string countryCode, string companyCode, string phoneNumber)
        {
            //arrange
            //string expectFirstName="Umair";

            //act
            Rider actual = new Rider(new User.NameFormat { FirstName = firstName, LastName = lastName }, new User.ContactNumberFormat(countryCode, companyCode, phoneNumber));

            //assert
            Assert.NotNull(actual);
            Assert.Equal(firstName, actual.FullName.FirstName);
            Assert.Equal(lastName, actual.FullName.LastName);
            Assert.Equal(countryCode, actual.ContactNumber.CountryCode);
            Assert.Equal(companyCode, actual.ContactNumber.CompanyCode);
            Assert.Equal(phoneNumber, actual.ContactNumber.PhoneNumber);
        }
        [Theory]
        [InlineData("Umar", "Tahir", "+92", "301", "2345678", 11)]
        [InlineData("Muhammad", "Tayyab", "+92", "301", "2345678", 10)]
        [InlineData("Usman", "Tahir", "+92", "301", "2345678", 9)]
        public void GetRider_Test(string firstName, string lastName, string countryCode, string companyCode, string phoneNumber, long id)
        {
            //arrange
            //string expectFirstName="Umair";

            //act
            Rider actual = new Rider(id);

            //assert
            Assert.NotNull(actual);
            Assert.Equal(firstName, actual.FullName.FirstName);
            Assert.Equal(lastName, actual.FullName.LastName);
            Assert.Equal(countryCode, actual.ContactNumber.CountryCode);
            Assert.Equal(companyCode, actual.ContactNumber.CompanyCode);
            Assert.Equal(phoneNumber, actual.ContactNumber.PhoneNumber);
        }
        [Fact]
        public void GetRider_NotInDbTest()
        {
            //arrange

            //act
            Rider actual = new Rider(1);

            //assert
            Assert.ThrowsAny<Exception>(() => new Rider(1));
        }
        [Fact]
        public void ChangeIsBlocked_Test()
        {
            //arrange
            //bool expected = true;

            //act
            Rider actual = new Rider(10);
            actual.IsBlocked = true;
            //assert
            Assert.True(actual.IsBlocked);
        }
        [Fact]
        public void UpdateFullName_Test()
        {
            //arrange
            User.NameFormat expected = new User.NameFormat
            {
                FirstName = "Muhammad",
                LastName = "Tayyab"
            };

            //act
            Rider rider = new Rider(10);
            rider.FullName = new User.NameFormat
            {
                FirstName = "Muhammad",
                LastName = "Tayyab"
            };
            User.NameFormat actual = rider.FullName;
            //assert
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData(true, 11)]
        [InlineData(true, 10)]
        [InlineData(true, 9)]
        public void ChangeIsBlockedTrue_Test(bool isBlock, long id)
        {
            //arrange
            //bool expected = true;

            //act
            Rider actual = new Rider(id);
            actual.IsBlocked = isBlock;
            //assert
            Assert.True(actual.IsBlocked);
        }
        [Theory]
        [InlineData(false, 11)]
        [InlineData(false, 10)]
        [InlineData(false, 9)]
        public void ChangeIsBlockedFalse_Test(bool isBlock, long id)
        {
            //arrange
            //bool expected = true;

            //act
            Rider actual = new Rider(id);
            actual.IsBlocked = isBlock;
            //assert
            Assert.False(actual.IsBlocked);
        }
        [Theory]
        [InlineData(true, 11)]
        [InlineData(true, 10)]
        [InlineData(true, 9)]
        public void SetIsActiveTrueAndGetAtTheSameTime_Test(bool IsActive, long id)
        {
            //arrange

            //act
            Rider actual = new Rider(id);
            actual.IsActive = IsActive;
            Assert.True(actual.IsActive);
        }
        [Theory]
        [InlineData(false, 11)]
        [InlineData(false, 10)]
        [InlineData(false, 9)]
        public void SetIsActiveFalseAndGetAtTheSameTime_Test(bool IsActive, long id)
        {
            //arrange

            //act
            Rider actual = new Rider(id);
            actual.IsActive = IsActive;
            //assert
            Assert.False(actual.IsActive);
        }
        [Theory]
        [InlineData("Hello! this is a test SMS", 11)]
        [InlineData("Hello! this is a test SMS", 10)]
        [InlineData("Hello! this is a test SMS", 9)]
        public void SendAndReceiveSMS_Test(string body, long id)
        {
            //arrange
            Sms expected;
            //act
            User user = new User(id);
            expected = user.SendSms(new Sms(DateTime.Now, body));
            //assert
            Assert.Equal(expected.SmsId, user.ReceivedSms()[0].SmsId);
        }
        [Fact]
        public void RegisterIdentityUser_TestForRiderGivesException()
        {
            //arrange

            //act
            Rider rider = new Rider(11);

            //Assert
            Assert.Throws<NotImplementedException>(() => rider.RegisterIdentityUser(User.ApplicationRoles.Rider, "u1786", "12345"));
        }
        [Fact]
        public void MatchCredentials_TestForRiderGivesException()
        {
            //arrange

            //act
            Rider rider = new Rider(11);

            //Assert
            Assert.Throws<NotImplementedException>(() => rider.MatchCredentials("u1786", "12345"));
        }
        [Theory]
        [InlineData("+92", "301", "2345678", 11)]
        [InlineData("+92", "301", "2345678", 10)]
        [InlineData("+92", "301", "2345678", 9)]
        public void UpdateContactNumber_Test(string countryCode, string comapnyCode, string number, long id)
        {
            //arrange
            User.ContactNumberFormat expected = new User.ContactNumberFormat(countryCode, comapnyCode, number);
            //act
            User user = new User(id);
            user.ContactNumber = expected;
            //assert
            Assert.Equal(expected, user.ContactNumber);
        }
        [Theory]
        [InlineData("92", "301", "2345678", 11)]
        [InlineData("+92", "201", "2345678", 10)]
        [InlineData("+92", "301", "a34b678", 9)]
        [InlineData("+2", "201", "2345678", 10)]
        [InlineData("+90", "301", "a34b678", 9)]
        [InlineData("+92", "301", "23456ab", 11)]
        [InlineData("+92", "111", "2345678", 10)]
        [InlineData("+ab", "301", "a34b678", 9)]
        [InlineData("+92", "301", "234567", 10)]
        [InlineData("+92", "301", "23416789", 9)]
        [InlineData("+92", "301", "43416785", 9)]
        public void NotUpdateInValidContactNumber_Test(string countryCode, string comapnyCode, string number, long id)
        {
            //arrange

            //act
            User user = new User(id);
            //assert
            Assert.Throws<ValidationPatternNotMatchException>(() => user.ContactNumber = new User.ContactNumberFormat(countryCode, comapnyCode, number));
        }
        [Fact]
        public void UpdateAndCheckFullName_Test()
        {
            //arrange
            string expectedFirstName = "Tayyab", expectedLastName = "Tahir";
            //act
            Rider rider = new Rider(10);
            rider.FullName = new User.NameFormat { FirstName = "tayyab", LastName = "tahir" };
            //Assert
            Assert.Equal(expectedFirstName, rider.FullName.FirstName);
            Assert.Equal(expectedLastName, rider.FullName.LastName);
        }
        [Fact]
        public void GetIsActiveAfterDelay_TestShouldBeFalse()
        {
            //arrange

            //act
            User user = new User(10);
            //assert
            Thread.Sleep(new TimeSpan(0, 0, 45));
            Assert.False(user.IsActive);
        }
        [Fact]
        public void GetApplicationRoleForRider_Test()
        {
            //arrange
            User.ApplicationRoles expected = User.ApplicationRoles.Rider;
            //act
            Rider rider = new Rider(10);
            //assert
            Assert.Equal(expected, rider.Role);
        }
        [Fact]
        public void GetApplicationUser_TestForRiderWillBeNull()
        {
            //arrange

            //act
            Rider rider = new Rider(10);
            //assert
            Assert.Null(rider.GetApplicationUser());
        }
        [Fact]
        public void GetAllUsers_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<User>>(User.GetAllUsers());
        }
        [Fact]
        public void GetAllRiders_Test()
        {
            //arrange

            //act

            //assert
            Assert.IsType<List<Rider>>(Rider.GetAllRiders());
        }
        [Fact]
        public void ChangePassword_TestForRiderReturnsException()
        {
            //arrange

            //act
            Rider rider = new Rider(10);
            //assert
            Assert.Throws<NotImplementedException>(() => rider.ChangePassword("12424", "12345"));
        }
        [Fact]
        public void AddNewDriver()
        {
            //arrange
            User.NameFormat expectedName = new User.NameFormat { FirstName = "Umair", LastName = "Tahir" };
            User.ContactNumberFormat expectedContact = new User.ContactNumberFormat("+92", "307", "1234567");
            decimal expectedCreditLimit = 1000;
            Location expectedLastLocation = new Location { Latitude = 31.376272m, Longitude = 74.251466m };
            //act
            Driver actual = new Driver(new User.NameFormat { FirstName = "umair", LastName = "TAHIr" }, new User.ContactNumberFormat("+92", "307", "1234567"), 1000, new Location { Latitude = 31.376272m, Longitude = 74.251466m }, "35202-848219-9");
            //assert
            Assert.Equal(expectedName.FirstName, actual.FullName.FirstName);
            Assert.Equal(expectedName.LastName, actual.FullName.LastName);
            Assert.Equal(expectedContact.CountryCode, actual.ContactNumber.CountryCode);
            Assert.Equal(expectedContact.CompanyCode, actual.ContactNumber.CompanyCode);
            Assert.Equal(expectedContact.PhoneNumber, actual.ContactNumber.PhoneNumber);
            Assert.Equal(expectedCreditLimit, actual.CreditLimit);
            Assert.Equal(expectedLastLocation.Latitude, actual.LastLocation.Latitude, 5);
            Assert.Equal(expectedLastLocation.Longitude, actual.LastLocation.Longitude, 5);
            Assert.Equal(User.ApplicationRoles.Driver, actual.Role);
            Assert.False(actual.IsBooked);
            Assert.True(actual.IsCleared);
            Assert.False(actual.IsBlocked);
            Assert.False(actual.IsActive);
            Assert.Equal(0, actual.Balance);
        }
        [Fact]
        public void MakeManualTransaction_Test()
        {
            Driver driver = new Driver(48);
            //arrange
            decimal expectedAmount = 500;
            decimal expectedBalance = driver.Balance + expectedAmount;
            DateTime expectedDateTime = DateTime.Now;
            //act
            ManualTransactionLog actual = driver.MakeManualTransaction(expectedAmount);
            decimal actualBalance = driver.Balance;
            DateTime actualDateTime = actual.TransactionDateTime;
            decimal actualAmount = actual.Amount;
            //assert
            Assert.IsType<ManualTransactionLog>(actual);
            Assert.Equal(expectedBalance, actualBalance, 3);
            Assert.Equal(expectedAmount, actualAmount, 3);
            Assert.Equal(expectedDateTime, actualDateTime, new TimeSpan(400000000));
        }
        [Fact]
        public void GetDriver_Test()
        {
            //arrange
            User.NameFormat expectedName = new User.NameFormat { FirstName = "Umair", LastName = "Tahir" };
            User.ContactNumberFormat expectedContactNumber = new User.ContactNumberFormat("+92", "307", "1234567");
            decimal expectedCreditLimit = 1000;
            decimal expectedBalance = 2500;
            Location expectedLastLocation = new Location { Latitude = 31.37627200m, Longitude = 74.25146600m };
            //act
            Driver actual = new Driver(48);
            //assert
            Assert.Equal(expectedName.FirstName, actual.FullName.FirstName);
            Assert.Equal(expectedName.LastName, actual.FullName.LastName);
            Assert.Equal(expectedContactNumber.CountryCode, actual.ContactNumber.CountryCode);
            Assert.Equal(expectedContactNumber.CompanyCode, actual.ContactNumber.CompanyCode);
            Assert.Equal(expectedContactNumber.PhoneNumber, actual.ContactNumber.PhoneNumber);
            Assert.Equal(expectedCreditLimit, actual.CreditLimit);
            Assert.Equal(expectedBalance, actual.Balance);
            Assert.Equal(expectedLastLocation.Latitude, actual.LastLocation.Latitude);
            Assert.Equal(expectedLastLocation.Longitude, actual.LastLocation.Longitude);
        }
        [Fact]
        public void GetManualTransactions_Test()
        {
            //arrange

            //act
            Driver actual = new Driver(48);
            //assert
            Assert.IsType<List<ManualTransactionLog>>(actual.GetManualTransactions());
        }
        [Theory]
        [InlineData(3, "Was Good", 9)]
        [InlineData(4, "Enjoyed", 10)]
        [InlineData(2, "Not Good", 11)]
        public void RiderRateDriver_Test(int rating, string comment, long riderId)
        {
            //arrange

            //act
            Rider rider = new Rider(riderId);
            rider.RateDriver(new RatingAndComments { Comment = comment, Rating = rating }, new Driver(48));
            //assert

        }
        [Fact]
        public void GetDriverRating_Test()
        {
            //arrange
            decimal expected = Convert.ToDecimal(3.16);

            //act
            Driver actual = new Driver(48);

            //assert
            Assert.Equal(expected, actual.TotalRating, 1);
        }
        [Fact]
        public void GetPropertiesOfNewDriver_Test()
        {
            //arrange

            //act
            Driver actual = new Driver(new User.NameFormat { FirstName = "Aqeel", LastName = "Ahmad" }, new User.ContactNumberFormat("+92", "312", "3456789"), 1500, new Location { Latitude = 31.380035m, Longitude = 74.255463m }, "35202-1234567-8");
            //assert
            Assert.Equal(0, actual.Balance);
            Assert.Equal(0, actual.TotalRating);
        }
        [Fact]
        public void AddAndChangeVehicle_Test()
        {
            //arrange
            Vehicle.RegisterationNumberFormat expectedRegisterationNumber = new Vehicle.RegisterationNumberFormat("Lhr", 1107, 19);
            string expectedModel = "Toyota-Corolla";
            int expectedEngineCC = (int)Vehicle.Engines.CC1800;
            bool expectedIsAc = true;
            Vehicle.Colors expectedColor = Vehicle.Colors.White;
            VehicleType expectedType = new VehicleType(1);
            //act
            Vehicle actual = new Driver(48).AddOrChangeVehicle(expectedRegisterationNumber, expectedModel, expectedEngineCC, expectedIsAc, expectedColor, expectedType);
            //assert
            Assert.Equal(expectedRegisterationNumber.Alphabets, actual.RegisterationNumber.Alphabets);
            Assert.Equal(expectedRegisterationNumber.Number, actual.RegisterationNumber.Number);
            Assert.Equal(expectedRegisterationNumber.Year, actual.RegisterationNumber.Year);
            Assert.Equal(expectedModel, actual.Model);
            Assert.Equal(expectedEngineCC, (int)actual.EngineCC);
            Assert.Equal(expectedColor, actual.VehicleColor);
            Assert.True(expectedIsAc);
        }
        [Fact]
        public void MakeMobileTransaction_Test()
        {
            //arrange
            string expectedRefNumber = "0000111223312";
            DateTime expectedDateTime = DateTime.Now;
            string expectedMobileAccountService = "JazzCash";
            decimal expectedAmount = 570.13m;
            Driver expectedDriver = new Driver(10007);
            decimal expectedBalance = 0;
            //act
            MobileAccountTransactionLog actual = expectedDriver.MakeMobileAccountTransaction(expectedRefNumber, expectedMobileAccountService, expectedAmount);
            //assert
            Assert.Equal(expectedRefNumber, actual.ReferenceNumber);
            Assert.Equal(expectedDateTime, actual.TransactionRegisteredTime, TimeSpan.FromMilliseconds(300));
            Assert.Equal(expectedMobileAccountService, actual.MobileAccountServiceProviderName);
            Assert.Equal(expectedAmount, actual.Amount, 2);
            Assert.StrictEqual(expectedDriver, actual.Driver);
            Assert.Equal(expectedBalance, actual.Driver.Balance, 2);
        }
        [Fact]
        public void MakeMobileTransaction_TestThrowsExceptionForSameReferenceNumber()
        {
            //arrange
            string expectedRefNumber = "0000111223312";
            DateTime expectedDateTime = DateTime.Now;
            string expectedMobileAccountService = "JazzCash";
            decimal expectedAmount = 570.13m;
            Driver expectedDriver = new Driver(10007);
            decimal expectedBalance = 0;
            //act

            //assert
            Assert.Throws<UniqueKeyViolationException>(() => expectedDriver.MakeMobileAccountTransaction(expectedRefNumber, expectedMobileAccountService, expectedAmount));
        }
        [Fact]
        public void GetAllMobileTransactions_Test()
        {
            //arrange

            //act
            Driver actual = new Driver(38);

            //assert
            Assert.IsType<List<MobileAccountTransactionLog>>(actual.GetAllMobileAccountTransactions());
        }
        [Fact]
        public void BookRide_TestForInActiveDrivers()
        {
            //arrange
            Ride.RideBookingDetails expectedDetails = new Ride.RideBookingDetails
            {
                Destination = new Location { Latitude = 31.388526m, Longitude = 74.271920m },
                PickUpLocation = new Location { Latitude = 31.385081m, Longitude = 74.250328m },
                VehicleType = new VehicleType(1)
            };
            //act

            //assert
            Assert.Throws<UnsuccessfullProcessException>(() => new Rider(11).BookRide(expectedDetails));
        }
        [Fact]
        public void BookRide_TestForActiveDrivers()
        {
            //arrange
            Ride.RideBookingDetails expectedDetails = new Ride.RideBookingDetails
            {
                Destination = new Location { Latitude = 31.388526m, Longitude = 74.271920m },
                PickUpLocation = new Location { Latitude = 31.385081m, Longitude = 74.250328m },
                VehicleType = new VehicleType(1)
            };
            //act
            foreach (var item in Driver.GetAllDrivers())
            {
                item.IsActive = true;
            }
            Thread.Sleep(TimeSpan.FromSeconds(15));
            Ride actual = new Rider(10).BookRide(expectedDetails);
            //assert
            Assert.StrictEqual(expectedDetails.Destination, actual.Destination);
            Assert.StrictEqual(expectedDetails.PickUpLocation, actual.PickUpLocation);
        }
        [Fact]
        public void BookRide_TestForActiveDriversButFarFromPickUp()
        {
            //arrange
            Ride.RideBookingDetails expectedDetails = new Ride.RideBookingDetails
            {
                Destination = new Location { Latitude = 31.388526m, Longitude = 74.271920m },
                PickUpLocation = new Location { Latitude = 31.494504m, Longitude = 74.301274m },
                VehicleType = new VehicleType(1)
            };
            //act
            foreach (var item in Driver.GetAllDrivers())
            {
                item.IsActive = true;
            }
            Thread.Sleep(TimeSpan.FromSeconds(15));
            //assert
            Assert.Throws<UnsuccessfullProcessException>(() => new Rider(10).BookRide(expectedDetails));
        }
        [Theory]
        [InlineData(10007, 10)]
        [InlineData(48, 12)]
        public void GetVehicle_Test(long dId, int vId)
        {
            //arrange
            Vehicle expectedVehicle = new Vehicle(vId);
            //act
            Driver actual = new Driver(dId);
            //assert
            Assert.Equal(expectedVehicle.VehicleId, actual.GetVehicle().VehicleId);
        }
        [Theory]
        [InlineData(10007)]
        [InlineData(48)]
        public void GetRatingsAndComments_Test(long dId)
        {
            //arrange

            //act
            Driver actual = new Driver(dId);
            //assert
            Assert.IsType<List<RatingAndComments>>(actual.GetRatingAndComments());
        }
        [Fact]
        public void PickUpRide_Test()
        {
            //arrange
            Location expectedPickUpLocation = new Location { Latitude = 31.385612m, Longitude = 74.249275m };
            //act
            Ride actual = new Ride(1).Driver.PickUpRide(new Ride(1), expectedPickUpLocation);
            //assert
            Assert.Equal(expectedPickUpLocation.Latitude, actual.PickUpLocation.Latitude);
            Assert.Equal(expectedPickUpLocation.Longitude, actual.PickUpLocation.Longitude);
        }
        [Fact]
        public void RegisterIdentityAccount_TestForDriver()
        {
            //arrange
            string expectedUserName = "ut786";
            string expectedPassword = "123456xyz";
            User.ApplicationRoles expectedRole = User.ApplicationRoles.Driver;
            //act
            Driver driver = new Driver(48);
            //ApplicationUser actual = driver.RegisterIdentityUser(expectedRole, expectedUserName, expectedPassword);
            //assert
            //Assert.IsType<ApplicationUser>(driver.RegisterIdentityUser(expectedRole, expectedUserName, expectedPassword));

        }
        [Fact]
        public void RestPassword_TestForUser()
        {
            //arrange

            //act
            User user = new User(48);
            //assert
            //Assert.IsType<ApplicationUser>(user.ResetPassword("123456xyz"));
        }
        [Fact]
        public void GetApplicationUser_TestForRegisteredDriver()
        {
            //arrange

            //act
            Driver driver = new Driver(48);
            //assert
            Assert.NotNull(driver.GetApplicationUser());
        }
        [Fact]
        public void CheckCredentials_TestForRegisteredDriver()
        {
            //arrange

            //act
            Driver driver = new Driver(48);

            //assert
            Assert.True(driver.MatchCredentials("ut786", "123456xyz"));
        }
        [Fact]
        public void CheckCredentials_TestForRegisteredDriverWrongPassword()
        {
            //arrange

            //act
            Driver driver = new Driver(48);

            //assert
            Assert.False(driver.MatchCredentials("ut786", "ab12345"));
        }
        [Fact]
        public void CheckCredentials_TestForUnRegisteredDriver()
        {
            //arrange

            //act
            Driver driver = new Driver(10007);

            //assert
            Assert.Throws<UserNotRegisteredException>(() => driver.MatchCredentials("ut786", "ab12345"));
        }
        [Fact]
        public void ChangePassword_TestForRegisteredDriver()
        {
            //arrange

            //act
            Driver driver = new Driver(48);
            //assert
            driver.ChangePassword("123456xyz", "123456xyz");
        }
        [Fact]
        public void ChangePassword_TestForRegisteredDriverInvalidPassword()
        {
            //arrange

            //act
            Driver driver = new Driver(48);
            //assert
            Assert.Throws<UserNotRegisteredException>(() => driver.ChangePassword("ab123456", "xyz1234"));
        }
        [Fact]
        public void GetCompletedRides_Test()
        {
            //arrange

            //act
            Driver driver = new Driver(48);
            //assert
            Assert.IsType<List<Ride>>(driver.GetCompletedRides());
        }
    }
}
