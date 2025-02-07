using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class ProjectContainerHolder : ObservableObject
    {
        private readonly ProjectsPageViewModel _viewModel;
        public Project Project { get; set; }
        public string CreateAtDate { get { return Project.CreatedAt.ToString("d"); } }
        public ProjectContainerHolder(ProjectsPageViewModel viewModel, Project project)
        {
            Project = project;
            _viewModel = viewModel;
        }

        public ProjectContainerHolder(ProjectContainerHolder other)
        {
            _viewModel = other._viewModel;
            Project = new Project(other.Project);
        }

        [RelayCommand]
        private void ItemClicked()
        {
            _viewModel?.CollectionProjectClickedCallback(this); 
        }

        [RelayCommand]
        public async Task PreviewProject()
        {
            await _viewModel.NavigateTab(ComponentTab.Preview);
        }

        [RelayCommand]
        public async Task EditProject()
        {
            await _viewModel.NavigateTab(ComponentTab.Edit);
        }
    }
}
