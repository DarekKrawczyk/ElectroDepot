using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectroDepotClassLibrary.Models;
using System;
using System.Net.NetworkInformation;

namespace DesktopClient.Containers
{
    public partial class DetailedItemProjectContainer : ObservableObject
    {
        private readonly Project _project;
        private readonly ProjectComponent _projectComponent;
        public string ProjectName { get {  return _project.Name; } }
        public int UsedInProject { get { return _projectComponent.Quantity; } }
        public DetailedItemProjectContainer(Project project, ProjectComponent projectComponent)
        {
            _project = project;
            _projectComponent = projectComponent;
        }

        [RelayCommand]
        public void NavigateToProject()
        {
            Console.WriteLine();
        }
    }
}
