using ElectroDepotClassLibrary.DTOs;
using ElectroDepotClassLibrary.Models;
using Xunit.Abstractions;

namespace ElectroDepotClassLibraryTests.Tests
{
    public class ProjectComponentDataProviderTests : BaseDataProviderTest
    {
        public ProjectComponentDataProviderTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Create()
        {
            try
            {
                // Find Project and Component
                IEnumerable<Project> projects = await ProjectDP.GetAllProjects();
                Assert.NotNull(projects);
                Project? project = projects.FirstOrDefault();
                Assert.NotNull(project);

                IEnumerable<Component> components = await ComponentDP.GetAllComponents();
                Assert.NotNull(components);
                Component? component = components.FirstOrDefault();
                Assert.NotNull(component);

                ProjectComponent projectComponent = new ProjectComponent(id: 0, projectID: project.ID, componentID: component.ID, component: component, quantity: 20);
                bool wasCreated = await ProjectComponentDP.CreateProjectComponent(projectComponent);
                Assert.True(wasCreated);

                Console.WriteLine($"Project component created: {projectComponent.ToString()}");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [Fact]
        public async Task GetAllFromProject()
        {
            try
            {
                IEnumerable<Project> projects = await ProjectDP.GetAllProjects();
                Assert.NotNull(projects);
                Project? project = projects.FirstOrDefault();
                Assert.NotNull(project);

                IEnumerable<ProjectComponent> projectComponents = await ProjectComponentDP.GetAllProjectComponentsOfProject(project);
                Assert.NotNull(projectComponents);

                foreach (ProjectComponent projectComponent in projectComponents)
                {
                    Console.WriteLine($"{projectComponent.ToString()}");
                }
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
                IEnumerable<ProjectComponent> projectComponents = await ProjectComponentDP.GetAllProjectComponents();
                Assert.NotNull(projectComponents);

                foreach (ProjectComponent projectComponent in projectComponents)
                {
                    Console.WriteLine($"{projectComponent.ToString()}");
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
                IEnumerable<ProjectComponent> projectComponents = await ProjectComponentDP.GetAllProjectComponents();
                Assert.NotNull(projectComponents);

                ProjectComponent? projectComp = projectComponents.FirstOrDefault();
                Assert.NotNull(projectComp);

                ProjectComponent projectCompUpdated = new ProjectComponent(id: projectComp.ID, componentID: projectComp.ComponentID, component: null, projectID: projectComp.ProjectID, quantity: 300); ;

                bool wasUpdated = await ProjectComponentDP.UpdateProjectComponent(projectCompUpdated);
                Assert.True(wasUpdated);
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
                IEnumerable<ProjectComponent> projectComponents = await ProjectComponentDP.GetAllProjectComponents();
                Assert.NotNull(projectComponents);
                Assert.NotEmpty(projectComponents);
                ProjectComponent? projectComp = projectComponents.FirstOrDefault();
                Assert.NotNull(projectComp);

                bool wasDeleted = await ProjectComponentDP.DeleteProjectComponent(projectComp);
                Assert.True(wasDeleted);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
