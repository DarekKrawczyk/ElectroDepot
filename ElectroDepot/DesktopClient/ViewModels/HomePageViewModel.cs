using System;
using System.Collections.ObjectModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using DesktopClient.Containers;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Models;
using Avalonia.Controls;
using DesktopClient.Navigation;
using ElectroDepotClassLibrary.Containers;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore.SkiaSharpView.Extensions;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Drawing;
using System.IO;
using ElectroDepotClassLibrary.Containers.NodeContainers;
using DesktopClient.Containers.ButtonsContainers;
using System.Globalization;
using DesktopClient.Services;
using System.Threading.Tasks;
using ElectroDepotClassLibrary.Utility;

namespace DesktopClient.ViewModels
{
    public partial class HomePageViewModel : RootNavigatorViewModel
    {
        [ObservableProperty]
        private string _currentDate = DateTime.Now.ToString("D", new CultureInfo("en-US"));

        private string UsersName
        {
            get
            {
                return DatabaseStore.UsersStore.LoggedInUser.Name;
            }
        }

        [ObservableProperty]
        private string _userName;

        [RelayCommand]
        public void AddSupplier()
        {
            // TODO: Implement!
        }

        [RelayCommand]
        public void NavigateProjectsAdd()
        {
            NavigatePage("Projects", NavParam.Create(NavOperation.Add, null));
        }

        [RelayCommand]
        public void NavigatePurchasesAdd()
        {
            NavigatePage("Purchases", NavParam.Create(NavOperation.Add, null));
        }

        public ObservableCollection<ToolAppButtonContainer> Tools { get; set; } = new ObservableCollection<ToolAppButtonContainer>()
        {
            //new ToolAppContainer("KiCad", null, "D:\\KiCAD\\bin\\kicad.exe"), TODO: 
            new ToolAppButtonContainer(new ToolAppContainer("EEVBlog",  ImageHelper.LoadFromResource(new Uri($"avares://ElectroDepot/Assets/Icons/Tools/eevblog.png")), "https://www.eevblog.com/")),
            new ToolAppButtonContainer(new ToolAppContainer("EEStack",  ImageHelper.LoadFromResource(new Uri($"avares://ElectroDepot/Assets/Icons/Tools/eestack.png")), "https://electronics.stackexchange.com/")),
            new ToolAppButtonContainer(new ToolAppContainer("GitHub",  ImageHelper.LoadFromResource(new Uri($"avares://ElectroDepot/Assets/Icons/Tools/github.png")), "https://github.com/")),
        };
        public ObservableCollection<SupplierContainer> Suppliers { get; set; }
        public ObservableCollection<ComponentContainer> Components { get; set; }
        public ObservableCollection<ProjectNodeButtonContainer> Projects { get; set; }
        public ObservableCollection<PurchaseNodeButtonContainer> Purchases {  get; set; }

        private static int _index = 0;
        private static string[] _names = ["Maria", "Susan", "Charles", "Fiona", "George"];

        public ObservableCollection<ISeries> Series { get; set; } = new();

        [Obsolete("DO NOT USE THIS! THIS IS JUST FOR AVALONIA DESIGNER!")]
        public HomePageViewModel(RootPageViewModel defaultRootPageViewModel, MessageBoxService msgBoxService) : base(defaultRootPageViewModel, null, null, null)
        {
            if (Design.IsDesignMode)
            {
                Suppliers = new ObservableCollection<SupplierContainer>();
                Components = new ObservableCollection<ComponentContainer>();
                Projects = new ObservableCollection<ProjectNodeButtonContainer>();
                Purchases = new ObservableCollection<PurchaseNodeButtonContainer>();
            }
        }

        public HomePageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService messageBoxService, ApplicationConfig appConfig) : base(defaultRootPageViewModel, databaseStore, messageBoxService, appConfig)
        {
            Suppliers = new ObservableCollection<SupplierContainer>();
            DatabaseStore.SupplierStore.SuppliersLoaded += SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.SuppliersReloadNotNecessary += SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.ReloadSuppliersData();

            Components = new ObservableCollection<ComponentContainer>();
            DatabaseStore.ComponentStore.ComponentsLoaded += ComponentsLoadedHandler;
            DatabaseStore.ComponentStore.ReloadComponentsData();

            Projects = new ObservableCollection<ProjectNodeButtonContainer>();
            DatabaseStore.ProjectStore.ProjectsLoaded += ProjectsLoadedHandler;
            DatabaseStore.ProjectStore.Load();

            Purchases = new ObservableCollection<PurchaseNodeButtonContainer>();
            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded += PurchaseStore_DetailedPurchaseContainersLoadedHandler;
            DatabaseStore.PurchaseStore.DetailedPurchaseContainersLoaded += PurchaseStore_DetailedPurchaseContainersLoadedHandler_SupplierChart;
            DatabaseStore.PurchaseStore.LoadDetailedPurchaseContainers();

            UserName = $"Welcome, {UsersName}!";
        }

        private async void PurchaseStore_DetailedPurchaseContainersLoadedHandler_SupplierChart()
        {
            AdjustSeries();
        }

        private async void AdjustSeries()
        {
            var groupedBySupplier = Purchases.GroupBy(x => x.Node.Supplier);
            var result = groupedBySupplier.Select(x => new
            {
                Supplier = x.Key,
                TotalSpendings = x.Sum(y => y.Node.TotalPrice),
            }).ToList();

            double sumTotalSpendings = result.Sum(x => x.TotalSpendings);

            // Clear and populate the series reactively
            Series.Clear();

            foreach (var value in result)
            {
                Series.Add(new PieSeries<double>
                {
                    Values = new[] { value.TotalSpendings },
                    Name = value.Supplier.Name,
                    DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                    DataLabelsSize = 15,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black)
                    {
                        SKTypeface = SKTypeface.FromFamilyName("Arial"),
                    },
                    DataLabelsFormatter =
                        point =>
                        {
                            var pv = point.Coordinate.PrimaryValue;
                            var sv = point.StackedValue!;

                            var a = $"{sv.Share:P2}";
                            return $"{value.Supplier.Name}";
                        },
                    ToolTipLabelFormatter = point => $"{point.StackedValue.Share:P2}{Environment.NewLine}{value.TotalSpendings} pln"
                });
            }
        }


        private async void PurchaseStore_DetailedPurchaseContainersLoadedHandler()
        {
            Purchases.Clear();
            IEnumerable<DetailedPurchaseContainer> purchasesFromDB = DatabaseStore.PurchaseStore.DetailedPurchaseContainers.OrderByDescending(x=>x.PurchaseDate);
            foreach (DetailedPurchaseContainer purchase in purchasesFromDB)
            {
                Purchases.Add(new PurchaseNodeButtonContainer(this, purchase));
            }
        }

        private async void ProjectsLoadedHandler()
        {
            Projects.Clear();
            List<Project> projectsFromDB = DatabaseStore.ProjectStore.Projects.OrderByDescending(x=>x.CreatedAt).ToList();
            foreach (Project project in projectsFromDB)
            {
                IEnumerable<Component> sd = await DatabaseStore.ProjectStore.ProjectDP.GetAllComponentsFromProject(project);
                Projects.Add(new ProjectNodeButtonContainer(this, project, sd.Count()));
            }
        }

        private async void ComponentsLoadedHandler()
        {
            Components.Clear();
            foreach (Component component in DatabaseStore.ComponentStore.Components)
            {
                int categoryID = component.CategoryID;
                Category foundCategory = await DatabaseStore.CategorieStore.DB.GetCategoryByID(categoryID);
                
                component.Category = foundCategory;
                
                Components.Add(new ComponentContainer(component));
            }
        }

        private async void SuppliersLoadedHandler()
        {
            Suppliers.Clear();
            foreach(Supplier supplier in DatabaseStore.SupplierStore.Suppliers)
            {
                Suppliers.Add(new SupplierContainer(supplier));
            }
        }

    }
}
