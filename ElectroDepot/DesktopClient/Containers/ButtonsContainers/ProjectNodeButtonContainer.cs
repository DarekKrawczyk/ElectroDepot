using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;

namespace DesktopClient.Containers.ButtonsContainers
{
    public partial class ProjectNodeButtonContainer : ObservableObject
    {
        private readonly RootNavigatorViewModel _viewModel;
        public Project Project { get; set; }

        public ProjectNodeButtonContainer(RootNavigatorViewModel viewModel, Project project)
        {
            _viewModel = viewModel;
            Project = project;
        }

        [RelayCommand]
        private void Execute()
        {
            _viewModel.NavigatePage("Projects", NavParam.Create(NavOperation.Preview, Project));
        }
    }
}
