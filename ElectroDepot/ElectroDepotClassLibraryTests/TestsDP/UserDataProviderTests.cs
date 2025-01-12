using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.Tests
{
    public class UserDataProviderTests : BaseDataProviderTest
    {
        public UserDataProviderTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create()
        {
            try
            {
                User user = new User(id: 0, name: "Darek", email: "Darek@gmail.com", password: "Pieczarka");

                User wasCreated = await UserDP.CreateUser(user);
                Assert.True(wasCreated != null);

                Console.WriteLine($"User created: {user.ToString()}");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task GetAll()
        {
            try
            {
                IEnumerable<User> users = await UserDP.GetAllUsers();
                Assert.NotNull(users);
                foreach (User user in users)
                {
                    Console.WriteLine($"{user.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task GetByUsername()
        {
            try
            {
                User? user = await UserDP.GetUserByUsername("Darek");
                Assert.NotNull(user);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task GetByID()
        {
            try
            {
                IEnumerable<User> allUsers = await UserDP.GetAllUsers();
                Assert.NotNull(allUsers);
                Assert.NotEmpty(allUsers);

                foreach (User singleuser in allUsers)
                {
                    Console.WriteLine(singleuser.ToString());
                }

                int ID = allUsers.FirstOrDefault().ID;

                User? user = await UserDP.GetUserByID(ID);
                Assert.NotNull(user);
                Console.WriteLine(user.ToString());
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task Delete()
        {
            try
            {
                IEnumerable<User> allUsers = await UserDP.GetAllUsers();
                Assert.NotNull(allUsers);
                Assert.NotEmpty(allUsers);

                foreach (User singleuser in allUsers)
                {
                    Console.WriteLine(singleuser.ToString());
                }

                User firstUser = allUsers.FirstOrDefault();

                bool wasDeleted = await UserDP.DeleteUser(firstUser);
                Assert.True(wasDeleted);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
