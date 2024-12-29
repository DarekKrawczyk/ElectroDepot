using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Containers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class PurchaseItemComponentContainerHolder : ObservableValidator
    {
        private readonly PurchasesPageViewModel _viewModel;
        public PurchaseItemComponentContainer PurchaseItem { get; }
        public string Name { get { return PurchaseItem.Name; } }
        public string CategoryName { get { return PurchaseItem.Name; } }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Range")]
        public int Quantity { get { return PurchaseItem.Quantity; }  set { PurchaseItem.Quantity = value; } }
        public Bitmap Image { get { return PurchaseItem.Image; } }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Range")]
        public double UnitPrice { get { return PurchaseItem.UnitPrice; } set { PurchaseItem.UnitPrice = value; } }
        public PurchaseItemComponentContainerHolder(PurchasesPageViewModel viewModel, PurchaseItemComponentContainer purchaseItem)
        {
            _viewModel = viewModel;
            PurchaseItem = purchaseItem;
        }

        [RelayCommand]
        private void RemoveFromCollection()
        {
            Console.WriteLine();
            _viewModel.PurchaseComponentsSource.Remove(this);
            _viewModel.PurchaseComponents.Refresh();
        }
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == "HasErrors")
            {
                Console.WriteLine();
            }
        }
    }
}
