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
        public int ComponentsCount {  get; set; }
        public string ComponentsCountText
        {
            get { return string.Format("{0} {1}", ComponentsCount, ComponentsCount == 1 ? "component" : "components"); }
        }

        public string Date { get { return Project.CreatedAt.ToString("d"); } }

        public ProjectNodeButtonContainer(RootNavigatorViewModel viewModel, Project project, int componentsCount)
        {
            _viewModel = viewModel;
            Project = project;
            ComponentsCount = componentsCount;
        }

        [RelayCommand]
        private void Execute()
        {
            _viewModel.NavigatePage("Projects", NavParam.Create(NavOperation.Preview, Project));
        }
    }
}
