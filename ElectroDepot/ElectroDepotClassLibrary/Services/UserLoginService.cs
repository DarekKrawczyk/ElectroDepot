using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using Microsoft.AspNetCore.Identity;

namespace ElectroDepotClassLibrary.Services
{
    public enum LoggingStatus
    {
        Success,
        FailureInvalidCredentials,
        FailureUserDoesntExist,
        FatalError
    }

    public class UserLoginService
    {
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UsersStore _usersStore;
        private User _loggedUser;

        public User UserCredentials
        {
            get
            {
                return _loggedUser;
            }
        }

        public UserLoginService(UsersStore usersStore, IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _usersStore = usersStore;
        }

        public void ClearData()
        {
            _loggedUser = null;
        }

        public async Task<LoggingStatus> Login(string username, string password)
        {
            try
            {
                User fromDB = null;
                _loggedUser = null;

                bool serverRuning = await _usersStore.UsersDP.IsServerRunning();
                if (serverRuning == false)
                {
                    return LoggingStatus.FatalError;
                }

                fromDB = await _usersStore.UsersDP.GetUserByUsername(username);
                if(fromDB == null)
                {
                    return LoggingStatus.FailureUserDoesntExist;
                }

                PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(fromDB, fromDB.Password, password);
                if(result != PasswordVerificationResult.Success)
                {
                    return LoggingStatus.FailureInvalidCredentials;
                }

                _loggedUser = fromDB;
                return LoggingStatus.Success;
            }
            catch(Exception exception)
            {
                return LoggingStatus.FatalError;
            }
        }

        public async Task<LoggingStatus> LoginViaHash(string username, string passwordHash)
        {
            try
            {
                User fromDB = null;
                _loggedUser = null;

                bool serverRuning = await _usersStore.UsersDP.IsServerRunning();
                if (serverRuning == false)
                {
                    return LoggingStatus.FatalError;
                }

                fromDB = await _usersStore.UsersDP.GetUserByUsername(username);
                if (fromDB == null)
                {
                    return LoggingStatus.FailureUserDoesntExist;
                }

                if(fromDB.Password != passwordHash)
                {
                    return LoggingStatus.FailureInvalidCredentials;
                }

                _loggedUser = fromDB;
                return LoggingStatus.Success;
            }
            catch (Exception exception)
            {
                return LoggingStatus.FatalError;
            }
        }
    }
}
