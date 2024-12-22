using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.DataProviders;

namespace ElectroDepotClassLibrary.Stores
{
    public class UsersStore : RootStore
    {
        private readonly UserDataProvider _userDataProvider;
        private User _loggedInUser = new User(id: 1010, name: "test", email: "test@gmail.com", password: "password"); // Raw from DB!

        public User LoggedInUser { get { return _loggedInUser; } }
        public UserDataProvider UsersDP { get { return _userDataProvider; } }

        public event Action UserLoggedIn;
        public event Action UserLoggedOut;
        public event Action UserLoginFailed;

        public UsersStore(DatabaseStore dbStore, UserDataProvider projectDataProvider) : base(dbStore)
        {
            _userDataProvider = projectDataProvider;
        }

        public async Task TryLogoutUser()
        {
            if(_loggedInUser != null)
            {
                _loggedInUser = null;
                UserLoggedOut?.Invoke();
            }
        }

        public async Task TryLoginUser(string username, string password)
        {
            if(_loggedInUser != null)
            {
                _loggedInUser = null;
                UserLoggedOut?.Invoke();
            }

            // TODO: to be implemented when login/register form is ready but for now it will do.
            User userFromDB = await _userDataProvider.GetUserByUsername(username);
            if(userFromDB != null)
            {
                _loggedInUser = userFromDB;
                UserLoggedIn?.Invoke();
            }
            else
            {
                UserLoginFailed?.Invoke();
            }
        }
    }
}
