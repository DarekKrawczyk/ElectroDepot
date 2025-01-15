using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroDepotClassLibrary.Utility
{
    public static class SupplierColorFiller
    {
        public static Color SupplierDominantColor(Supplier supplier)
        {
            int R = 0, G = 0, B = 0;
            if(supplier != null)
            {
                if(supplier.Name == "Botland")
                {
                    R = 255;
                    G = 0;
                    B = 0;
                }
                else if (supplier.Name == "DigiKey")
                {
                    R = 200;
                    G = 0;
                    B = 0;
                }
                else if (supplier.Name == "Mouser")
                {
                    R = 4;
                    G = 26;
                    B = 200;
                }
                else if (supplier.Name == "Aliexpress")
                {
                    R = 237;
                    G = 100;
                    B = 0;
                }
                else if (supplier.Name == "Kamami")
                {
                    R = 235;
                    G = 10;
                    B = 34;
                }
                else if (supplier.Name == "Allegro")
                {
                    R = 237;
                    G = 66;
                    B = 10;
                }
            }
            return Color.FromArgb(R, G, B);
        }
    }
}
