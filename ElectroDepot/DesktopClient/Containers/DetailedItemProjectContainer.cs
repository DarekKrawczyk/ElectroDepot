using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;
using System;
using System.Net.NetworkInformation;

namespace DesktopClient.Containers
{
    public partial class DetailedItemProjectContainer : ObservableObject
    {
        private readonly Project _project;
        private readonly ComponentsPageViewModel _viewModel;
        private readonly ProjectComponent _projectComponent;
        public string ProjectName { get {  return _project.Name; } }
        public int UsedInProject { get { return _projectComponent.Quantity; } }
        public DetailedItemProjectContainer(ComponentsPageViewModel viewModel, Project project, ProjectComponent projectComponent)
        {
            _project = project;
            _projectComponent = projectComponent;
            _viewModel = viewModel;
        }

        [RelayCommand]
        public void NavigateToProject()
        {
            _viewModel.NavigatePage("Projects", NavParam.Create(NavOperation.Preview, _project));
        }
    }
}
