using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Services;
using Microsoft.AspNetCore.Identity;

namespace ElectroDepotClassLibrary.Stores
{
    public class UsersStore : RootStore
    {
        private readonly UserLoginService _loginService;
        private readonly UserRegisterService _registerService;
        private readonly UserDataProvider _userDataProvider;
        private User _loggedInUser;

        //public User LoggedInUser { get { return _loggedInUser; } }
        public User LoggedInUser { get { return new User(1011, "username", "password", "email", "Darius"); } }
        public UserDataProvider UsersDP { get { return _userDataProvider; } }

        public event Action UserLoggedIn;
        public event Action UserLoggedOut;

        public UsersStore(DatabaseStore dbStore, UserDataProvider projectDataProvider) : base(dbStore)
        {
            _userDataProvider = projectDataProvider;
            _loginService = new UserLoginService(this, new PasswordHasher<User>());
            _registerService = new UserRegisterService(this);
        }

        public void UserLogout()
        {
            _loggedInUser = null;
            _loginService.ClearData();
            UserLoggedOut?.Invoke();
        }

        public async Task<LoggingStatus> UserLogin(string username, string password)
        {
            UserLogout();

            LoggingStatus status = await _loginService.Login(username, password);

            if(status == LoggingStatus.Success)
            {
                _loggedInUser = _loginService.UserCredentials;
                UserLoggedIn?.Invoke();
            }

            return status;
        }

        public async Task<RegistrationStatus> UserRegister(string username, string password, string confirmpassword, string email, string name)
        {
            RegistrationStatus status = await _registerService.Register(username, password, confirmpassword, email, name);
            return status;
        }
    }
}
