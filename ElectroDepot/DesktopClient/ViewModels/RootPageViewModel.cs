using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.ViewModels
{
    public partial class RootPageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private bool _isPanelOpen = false;

        [RelayCommand]
        private void TriggerPane()
        {
            IsPanelOpen = !IsPanelOpen;
        }

        public RootPageViewModel(DatabaseStore databaseStore, Navigator navigator) : base(databaseStore, navigator)
        {
            DatabaseStore.PredefinedImagesStore.Load();
            OnSelectedListItemChanged(Items[0]);
            //CurrentPage = new ComponentsPageViewModel();
            //_currentPage = new HomePageViewModel();
        }

        public ObservableCollection<ListItemTemplate> Items { get; set; } = new()
        {
            new ListItemTemplate(typeof(HomePageViewModel)),
            new ListItemTemplate(typeof(ComponentsPageViewModel)),
            new ListItemTemplate(typeof(ProjectsPageViewModel)),
            new ListItemTemplate(typeof(PurchasesPageViewModel)),
            new ListItemTemplate(typeof(MonitoringPageViewModel)),
            new ListItemTemplate(typeof(ProfilePageViewModel)),
            new ListItemTemplate(typeof(LogoutPageViewModel)),
        };

        [ObservableProperty]
        private ViewModelBase _currentPage;

        partial void OnSelectedListItemChanged(ListItemTemplate value)
        {
            if (value is null)
            {
                return;
            }
            var instance = Activator.CreateInstance(value.ModelType, args: new object[] { DatabaseStore, _navigator});
            if (instance is null)
            {
                return;
            }
            CurrentPage?.Dispose();
            CurrentPage = (ViewModelBase)instance;
        }

        public override void Dispose()
        {
        }

        [ObservableProperty]
        private ListItemTemplate _selectedListItem;
    }

    public class ListItemTemplate
    {
        public Bitmap Icon { get; }
        public string Label { get; }
        public Type ModelType { get; }
        public ListItemTemplate(Type modelType)
        {
            Label = modelType.Name.Replace("PageViewModel", "");
            Icon = ImageHelper.LoadFromResource(new Uri($"avares://DesktopClient/Assets/{Label}_icon.png"));
            ModelType = modelType;
        }
    }
}
