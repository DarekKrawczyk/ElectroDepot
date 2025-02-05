using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Services;
using ElectroDepotClassLibrary.Models;
using System.Diagnostics;

namespace DesktopClient.Containers
{
    public partial class SupplierContainer : ObservableObject
    {
        public Supplier Supplier { get; set; }

        [RelayCommand]
        private void ItemClicked()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Supplier.Website,
                UseShellExecute = true
            });
        }

        [RelayCommand]
        public void SearchForItem(string item)
        {
            WebsiteNavigationService.NavigateTo(Supplier.Name, item);
        }

        public SupplierContainer(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
