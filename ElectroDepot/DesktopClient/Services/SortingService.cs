using Avalonia.Controls;
using DesktopClient.Containers;
using DesktopClient.ViewModels;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    public class SortingService : AbstractNotifyPropertyChanged
    {
        private readonly IList<SortContainer> _sortItems = new ObservableCollection<SortContainer>
        {
            new SortContainer("Default", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Ascending(l => l.Container.ID)),

            new SortContainer("Price ascending", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Ascending(l => l.Container.TotalPrice)),

            new SortContainer("Price descending", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Descending(l => l.Container.TotalPrice)),

            new SortContainer("Date ascending", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Ascending(l => l.Container.PurchaseDate)),

            new SortContainer("Date descending", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Descending(l => l.Container.PurchaseDate)),

           new SortContainer("Components ascending", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Ascending(l => l.Container.ComponentsQuantity)),

            new SortContainer("Components descending", SortExpressionComparer<DetailedPurchaseContainerHolder>
                .Descending(l => l.Container.ComponentsQuantity)),
        };

        private SortContainer _selectedItem;


        public SortingService()
        {
            SelectedItem = _sortItems[0];
        }

        public SortContainer SelectedItem
        {
            get => _selectedItem;
            set => SetAndRaise(ref _selectedItem, value);
        }

        public IEnumerable<SortContainer> SortItems => _sortItems;

        public void ApplySorting(SortingType sortingType)
        {
            switch (sortingType)
            {
                case SortingType.Default:
                    SelectedItem = _sortItems[0];
                    break;
                case SortingType.PriceAsc:
                    SelectedItem = _sortItems[1];
                    break;
                case SortingType.PriceDesc:
                    SelectedItem = _sortItems[2];
                    break;
                case SortingType.DateAsc:
                    SelectedItem = _sortItems[3];
                    break;
                case SortingType.DateDesc:
                    SelectedItem = _sortItems[4];
                    break;
                case SortingType.ComponentsAsc:
                    SelectedItem = _sortItems[5];
                    break;
                case SortingType.ComponentsDesc:
                    SelectedItem = _sortItems[6];
                    break;
            }
        }
    }
}
