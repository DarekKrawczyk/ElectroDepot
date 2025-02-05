using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Containers
{
    public class PurchaseItemComponentContainer
    {
        private readonly Component _component;
        private readonly PurchaseItem _purchaseItem;
        private readonly Category _category;

        public Component Component { get { return _component; } }  
        public int ComponentID { get { return _component.ID; } }
        public string Name { get { return _component.Name; } }
        public string CategoryName { get { return _category.Name; } }
        public int Quantity { get { return _purchaseItem.Quantity;} set { _purchaseItem.Quantity = value; } }
        public Bitmap Image { get { return _component.Image; } } 
        public double UnitPrice { get { return _purchaseItem.PricePerUnit; } set { _purchaseItem.PricePerUnit = value; } }
        public string UnitPriceAsCurrency { get { return UnitPrice.ToString("C2"); } }
        public PurchaseItem PurchaseItem { get { return _purchaseItem; } }
        public PurchaseItemComponentContainer(Component component, PurchaseItem purchaseItem, Category category)
        {
            _component = component;
            _purchaseItem = purchaseItem;
            _category = category;
        }
    }
}
