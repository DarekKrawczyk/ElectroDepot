using ElectroDepotClassLibrary.Models;
using System;

namespace DesktopClient.Containers
{
    public class DetailedItemPurchaseContainer
    {
        private readonly Purchase _purchase;
        private readonly PurchaseItem _purchaseItem;
        private readonly Supplier _supplier;
        public int BoughtAmount { get { return _purchaseItem.Quantity; } }
        public double PricePerUnit { get { return _purchaseItem.PricePerUnit; } }
        public DateOnly PurchaseDate { get { return DateOnly.FromDateTime(_purchase.PurchaseDate); } }
        public string SupplierName { get { return _supplier.Name; } }   
        public DetailedItemPurchaseContainer(Purchase purchase, PurchaseItem purchaseItem, Supplier supplier)
        {
            _supplier = supplier;
            _purchase = purchase;
            _purchaseItem = purchaseItem;
        }
    }
}
