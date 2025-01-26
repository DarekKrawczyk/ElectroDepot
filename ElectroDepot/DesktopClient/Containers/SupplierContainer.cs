using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
            if (string.IsNullOrEmpty(item)) return;
            else
            {
                string url = string.Empty;
                switch (Supplier.Name)
                {
                    case "DigiKey":
                        url = "https://www.digikey.com/en/products/result?keywords=" + $"{item.Replace(' ', '+')}";
                        break;
                    case "Botland":
                        url = @"https://botland.store/search?s=" + $"{item.Replace(" ", "%20")}";
                        break;
                    case "Mouser":
                        url = @"https://www.google.com/";
                        break;
                    case "Kamami":
                        url = @"https://kamami.pl/module/jzsphinxsearch/jsssearch?searchstring=" + $"{item.Replace(' ', '+')}" + "&id_lang=1";
                        break;
                    case "M-Salamon":
                        url = @"https://sklep.msalamon.pl/?s=" + $"{item.Replace(' ', '+')}" + "&post_type=product&dgwt_wcas=1";
                        break;
                    case "Allegro":
                        url = @"https://allegro.pl/listing?string=" + $"{item.Replace(" ", "%20")}";
                        break;
                    case "AliExpress":
                        url = @"https://pl.aliexpress.com/w/wholesale-" + $"{item.Replace(' ', '-')}" + @".html?spm=a2g0o.home.search.0";
                        break;
                    default:
                        url = @"https://www.google.com/";
                        break;
                }
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }

        public SupplierContainer(Supplier supplier)
        {
            Supplier = supplier;
        }
    }
}
