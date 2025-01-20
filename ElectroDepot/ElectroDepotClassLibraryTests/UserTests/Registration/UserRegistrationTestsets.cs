using Xunit.Abstractions;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Services;
using Moq;

namespace ElectroDepotClassLibraryTests.UserTests.Registration
{
    public class UserRegistrationTestsets : BaseDBStoreTestset
    {
        private readonly UserRegisterService _registerService;
        public UserRegistrationTestsets(ITestOutputHelper console) : base(console)
        {
            _registerService = new UserRegisterService(DatabaseStore.UsersStore);
        }

        [Fact]
        public async void Register_ValidData_Success()
        {
            RegistrationStatus expected = RegistrationStatus.Success;
            string username = "username";
            string password = "password";
            string confirmPassword = "password";
            string email = "valid.email@gmail.com";
            string name = "test";

            RegistrationStatus actual = await _registerService.Register(username, password, confirmPassword, email, name);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public async void Register_UserExists_Failure()
        {
            RegistrationStatus expected = RegistrationStatus.FailureUsernameTaken;
            string username = "test";
            string password = "password";
            string confirmPassword = "password";
            string email = "valid.email@gmail.com";
            string name = "test";

            RegistrationStatus actual = await _registerService.Register(username, password, confirmPassword, email, name);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public async void Register_EmailExists_Failure()
        {
            RegistrationStatus expected = RegistrationStatus.FailureEmailTaken;
            string username = It.IsAny<string>();
            string password = "password";
            string confirmPassword = "password";
            string email = "test@gmail.com";
            string name = "name";

            RegistrationStatus actual = await _registerService.Register(username, password, confirmPassword, email, name);

            Assert.Equal(actual, expected);
        }

        [Fact]
        public async void Register_PasswordDontMatch_Failure()
        {
            RegistrationStatus expected = RegistrationStatus.FailurePasswordDontMath;
            string username = It.IsAny<string>();
            string password = "password1";
            string confirmPassword = "password1123123";
            string email = "tesdsst@gmail.com";
            string name = "name";

            RegistrationStatus actual = await _registerService.Register(username, password, confirmPassword, email, name);

            Assert.Equal(actual, expected);
        }
    }
}
