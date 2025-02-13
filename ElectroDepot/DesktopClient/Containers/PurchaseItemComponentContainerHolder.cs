using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopClient.Navigation;
using DesktopClient.Services;
using DesktopClient.ViewModels;
using ElectroDepotClassLibrary.Containers;
using ExCSS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
        public string CategoryName { get { return PurchaseItem.CategoryName; } }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Range")]
        public int Quantity { get { return PurchaseItem.Quantity; }  set { PurchaseItem.Quantity = value; } }
        public string QuantitySetter 
        { 
            get 
            { 
                return PurchaseItem.Quantity.ToString(); 
            } 
            set 
            { 
                if(value == null)
                {
                    PurchaseItem.Quantity = 1;
                }
                else
                {
                    PurchaseItem.Quantity = int.Parse(value);
                }
                _viewModel.Add_ReevaluateTotalPrice();
            } 
        }
        public Bitmap Image { get { return PurchaseItem.Image; } }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Range")]
        public double UnitPrice 
        { 
            get 
            { 
                return PurchaseItem.UnitPrice; 
            } 
            set 
            { 
                PurchaseItem.UnitPrice = value;
            }
        }

        public string UnitPriceSetter
        {
            get
            {
                return PurchaseItem.UnitPrice.ToString();
            }
            set
            {
                if (value == null)
                {
                    PurchaseItem.UnitPrice = 0;
                }
                else
                {
                    PurchaseItem.UnitPrice = double.Parse(value);
                }
                _viewModel.Add_ReevaluateTotalPrice();
            }
        }
        public ObservableCollection<SupplierContainer> Suppliers { get { return _viewModel.Suppliers; } }
        public PurchaseItemComponentContainerHolder(PurchasesPageViewModel viewModel, PurchaseItemComponentContainer purchaseItem)
        {
            _viewModel = viewModel;
            PurchaseItem = purchaseItem;
        }

        [RelayCommand]
        private void RemoveFromCollection()
        {
            ComponentWithCategoryContainerHolder found = _viewModel.AllComponentsSource.FirstOrDefault(x => x.Component.Component.ID == PurchaseItem.ComponentID);
            if (found != null)
            {
                found.CanAdd = true;
            }

            _viewModel.PurchaseComponentsSource.Remove(this);
            _viewModel.PurchaseComponents.Refresh();
            _viewModel.Add_ReevaluateTotalPrice();
            _viewModel.CartComponentsSizeChanged();
        }

        [RelayCommand]
        public void NavigateToWebsite(string supplier)
        {
            WebsiteNavigationService.NavigateTo(supplier, PurchaseItem.Component.Name);
        }

        [RelayCommand(CanExecute = nameof(CanOpenDatasheet))]
        public void OpenDatasheet()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = PurchaseItem.Component.DatasheetLink,
                UseShellExecute = true
            });
        }

        private bool CanOpenDatasheet()
        {
            return PurchaseItem.Component.DatasheetLink != null && PurchaseItem.Component.DatasheetLink != string.Empty;
        }

        [RelayCommand]
        public void PreviewComponent()
        {
            _viewModel.NavigatePage("Components", NavParam.Create(NavOperation.Preview, PurchaseItem.Component));
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
