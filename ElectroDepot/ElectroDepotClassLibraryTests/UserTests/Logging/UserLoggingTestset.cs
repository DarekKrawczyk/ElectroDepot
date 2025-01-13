using Xunit.Abstractions;
using ElectroDepotClassLibrary.Services;
using Microsoft.AspNetCore.Identity;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibraryTests.UserTests.Logging
{
    public class UserLoggingTestset : BaseDBStoreTestset
    {
        private readonly UserLoginService _loggingService;
        public UserLoggingTestset(ITestOutputHelper console) : base(console)
        {
            _loggingService = new UserLoginService(DatabaseStore.UsersStore, new PasswordHasher<User>());
        }

        [Fact]
        public async void Login_ValidData_Success()
        {
            LoggingStatus expected = LoggingStatus.Success;
            string username = "username";
            string password = "password";

            LoggingStatus actual = await _loggingService.Login(username, password);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Login_InvalidData_Failure()
        {
            LoggingStatus expected = LoggingStatus.FailureInvalidCredentials;
            string username = "username";
            string password = "passwordss";

            LoggingStatus actual = await _loggingService.Login(username, password);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Login_UserUnknown_Failure()
        {
            LoggingStatus expected = LoggingStatus.FailureUserDoesntExist;
            string username = "usernamesdasdasd";
            string password = "passwordss";

            LoggingStatus actual = await _loggingService.Login(username, password);

            Assert.Equal(expected, actual);
        }
    }
}
