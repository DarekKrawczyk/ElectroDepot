using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectroDepotClassLibrary.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public partial class DetailedPurchaseContainerHolder : ObservableObject
    {
        private readonly DetailedPurchaseContainer _purchaseContainer;
        public DetailedPurchaseContainer Container { get { return _purchaseContainer; } } 
        public DetailedPurchaseContainerHolder(DetailedPurchaseContainer purchaseContainer)
        {
            _purchaseContainer = purchaseContainer;
        }

        [RelayCommand]
        public void PreviewPurchaseCommand()
        {
            Console.WriteLine();
        }
    }
}
