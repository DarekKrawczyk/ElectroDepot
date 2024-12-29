using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroDepotClassLibrary.Containers
{
    public class DetailedPurchaseContainer
    {
        private readonly Purchase _purchase;
        private readonly Supplier _supplier;
        private readonly IEnumerable<PurchaseItem> _purchasedItems;
        public int ID { get { return _purchase.ID; } }
        public DateTime PurchaseDate { get { return _purchase.PurchaseDate; } }
        public string PurchaseDateAsDate { get { return PurchaseDate.ToString("D"); } }
        public double TotalPrice { get { return _purchase.TotalPrice; } }
        public int ComponentsQuantity { get { return _purchasedItems.Count(); } }
        public string SupplierName { get { return _supplier.Name; } }
        public string SupplierWebsite { get { return _supplier.Website; } }

        public DetailedPurchaseContainer(Purchase purchase, Supplier supplier, IEnumerable<PurchaseItem> purchasedItems)
        {
            _purchase = purchase;
            _supplier = supplier;
            _purchasedItems = purchasedItems;
        }
    }
}
