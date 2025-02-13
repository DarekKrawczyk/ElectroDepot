using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Services
{
    internal class WebsiteNavigationService
    {
        public static void NavigateTo(string supplier, string component)
        {
            if (string.IsNullOrEmpty(component)) return;
            else
            {
                string url = string.Empty;
                switch (supplier)
                {
                    case "DigiKey":
                        url = "https://www.digikey.com/en/products/result?keywords=" + $"{component.Replace(' ', '+')}";
                        break;
                    case "Botland":
                        url = @"https://botland.store/search?s=" + $"{component.Replace(" ", "%20")}";
                        break;
                    case "Mouser":
                        url = @"https://www.google.com/";
                        break;
                    case "Kamami":
                        url = @"https://kamami.pl/module/jzsphinxsearch/jsssearch?searchstring=" + $"{component.Replace(' ', '+')}" + "&id_lang=1";
                        break;
                    case "M-Salamon":
                        url = @"https://sklep.msalamon.pl/?s=" + $"{component.Replace(' ', '+')}" + "&post_type=product&dgwt_wcas=1";
                        break;
                    case "Allegro":
                        url = @"https://allegro.pl/listing?string=" + $"{component.Replace(" ", "%20")}";
                        break;
                    case "AliExpress":
                        url = @"https://pl.aliexpress.com/w/wholesale-" + $"{component.Replace(' ', '-')}" + @".html?spm=a2g0o.home.search.0";
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

        public static void NavigateTo(string website)
        {
            if (string.IsNullOrEmpty(website)) return;
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = website,
                    UseShellExecute = true
                });
            }
        }
    }
}
