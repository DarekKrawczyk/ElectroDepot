using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Stores
{
    public class ProjectsStore : RootStore
    {
        private readonly ProjectDataProvider _projectDataProvider;
        private readonly ProjectComponentDataProvider _projectComponentDataProvider;
        private List<Project> _projects;
        private List<ProjectComponent> _projectComponents;

        public IEnumerable<Project> Projects { get { return _projects; } }
        public IEnumerable<ProjectComponent> ProjectsComponents { get { return _projectComponents; } }
        public ProjectDataProvider ProjectDP { get { return _projectDataProvider; } }
        public ProjectComponentDataProvider ProjectComponentDP { get { return _projectComponentDataProvider; } }

        public event Action ProjectsLoaded;

        public ProjectsStore(DatabaseStore dbStore, ProjectDataProvider projectDataProvider, ProjectComponentDataProvider projectComponentDataProvider) : base(dbStore)
        {
            _projectDataProvider = projectDataProvider;
            _projectComponentDataProvider = projectComponentDataProvider;
            _projects = new List<Project>();
            _projectComponents = new List<ProjectComponent>();
        }

        public async Task Load()
        {
            _projects.Clear();
            _projectComponents.Clear();

            if (MainStore.UsersStore.LoggedInUser == null) throw new Exception("User not logged in!");

            IEnumerable<Project> projectsFromDB = await _projectDataProvider.GetAllProjectOfUser(MainStore.UsersStore.LoggedInUser);
            foreach(Project project in projectsFromDB)
            {
                Bitmap image = await _projectDataProvider.GetImageOfProjectByID(project);
                project.Image = image;
            }
            _projects.AddRange(projectsFromDB);

            ProjectsLoaded?.Invoke();
        }
    }
}
