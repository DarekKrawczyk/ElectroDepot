using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.Tests
{
    public class ProjectDataProviderTests : BaseDataProviderTest
    {
        public ProjectDataProviderTests(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public async Task Create()
        {
            try
            {
                // Find any User
                IEnumerable<User> users = await UserDP.GetAllUsers();
                User? user = users.FirstOrDefault();

                Assert.NotNull(user);

                Project project = new Project(id: 0, userID: user.ID, user: user, name: "Stacja meterologiczna", image: null, description: "Na SMIW", createdAt: DateTime.Now);
                Project wasCreate = await ProjectDP.CreateProject(project);
                Assert.True(wasCreate != null);

                Console.WriteLine($"Project created: {project.ToString()}");
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
                IEnumerable<Project> projects = await ProjectDP.GetAllProjects();
                Assert.NotNull(projects);
                foreach (Project project in projects)
                {
                    Console.WriteLine($"{project.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task GetAllFromUser()
        {
            try
            {
                // Find any User
                IEnumerable<User> users = await UserDP.GetAllUsers();
                User? user = users.FirstOrDefault();
                Assert.NotNull(user);

                IEnumerable<Project> projects = await ProjectDP.GetAllProjectOfUser(user);
                Assert.NotNull(projects);
                foreach (Project project in projects)
                {
                    Console.WriteLine($"{project.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task GetAllComponentsFromProject()
        {
            try
            {
                // Find any Project
                IEnumerable<Project> projects = await ProjectDP.GetAllProjects();
                Project? project = projects.FirstOrDefault();
                Assert.NotNull(project);

                IEnumerable<Component> componentsOfProject = await ProjectDP.GetAllComponentsFromProject(project);
                Assert.NotNull(componentsOfProject);
                foreach (Component components in componentsOfProject)
                {
                    Console.WriteLine($"{components.ToString()}");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task Update()
        {
            try
            {
                // Find any User
                IEnumerable<User> users = await UserDP.GetAllUsers();
                User? user = users.FirstOrDefault();
                Assert.NotNull(user);

                IEnumerable<Project> projects = await ProjectDP.GetAllProjectOfUser(user);
                Assert.NotNull(projects);
                Console.WriteLine("Before update");
                foreach (Project project in projects)
                {
                    Console.WriteLine($"{project.ToString()}");
                }

                Project projectUpdated = projects.FirstOrDefault();
                Project projectToSend = new Project(id: 0, userID: projectUpdated.UserID, user: user, name: "Fajna stacja pogodowa", image: null, description: projectUpdated.Description, createdAt: projectUpdated.CreatedAt);

                Project wasChanged = await ProjectDP.UpdateProject(projectToSend);
                Assert.True(wasChanged != null);

                IEnumerable<Project> updatedProjects = await ProjectDP.GetAllProjectOfUser(user);
                Assert.NotNull(updatedProjects);
                Console.WriteLine("\nAfter update");
                foreach (Project project in updatedProjects)
                {
                    Console.WriteLine($"{project.ToString()}");
                }
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
                // Find any Project
                IEnumerable<Project> projects = await ProjectDP.GetAllProjects();
                Assert.NotNull(projects);

                Project? projectToDelete = projects.FirstOrDefault();
                Assert.NotNull(projectToDelete);

                bool wasDeleted = await ProjectDP.DeleteProject(projectToDelete);
                Assert.True(wasDeleted);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
