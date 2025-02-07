using DesktopClient.Containers;
using DesktopClient.ViewModels;
using DynamicData;
using ElectroDepotClassLibrary.DataProviders;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    internal class ProjectHolderService
    {
        private readonly ProjectsPageViewModel _viewModel;
        private readonly ProjectsStore _projectsStore;
        private readonly ISourceCache<ProjectContainerHolder, int> _projects;

        public event Action DataLoaded;

        public ProjectHolderService(ProjectsPageViewModel viewModel, ProjectsStore projectsStore)
        {
            _viewModel = viewModel;
            _projectsStore = projectsStore;
            _projects = new SourceCache<ProjectContainerHolder, int>(e => e.Project.ID);
        }

        public IObservable<IChangeSet<ProjectContainerHolder, int>> EmployeesConnection() => _projects.Connect();

        public DateTimeOffset MaxYear()
        {
            return _projects.Items.Max(x=>x.Project.CreatedAt);
        }

        public DateTimeOffset MinYear()
        {
            return _projects.Items.Min(x => x.Project.CreatedAt);
        }

        public async Task LoadData()
        {
            _projects.Clear();

            IEnumerable<Project> projectsFromDB = await _projectsStore.ProjectDP.GetAllProjectOfUser(_projectsStore.MainStore.UsersStore.LoggedInUser);

            foreach(Project project in projectsFromDB)
            {
                project.Image = await _projectsStore.ProjectDP.GetImageOfProjectByID(project);
                _projects.AddOrUpdate(new ProjectContainerHolder(_viewModel, project));
            }

            DataLoaded?.Invoke();   
        }
    }
}
