using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DesktopClient.Containers
{
    public partial class ProjectContainerHolder : ObservableObject
    {
        private readonly ProjectsPageViewModel _viewModel;
        private readonly List<ProjectComponentHolder> _projectComponents;
        public Project Project { get; set; }

        public List<ProjectComponentHolder> ProjectComponents { get { return _projectComponents; } }    

        public ProjectContainerHolder(ProjectsPageViewModel viewModel, Project project, IEnumerable<ProjectComponentHolder> projectComponents)
        {
            _projectComponents = new List<ProjectComponentHolder>(projectComponents);
            Project = project;
            _viewModel = viewModel;
        }

        [RelayCommand]
        private void ItemClicked()
        {
            _viewModel?.CollectionProjectClickedCallback(this); 
        }
    }
}
