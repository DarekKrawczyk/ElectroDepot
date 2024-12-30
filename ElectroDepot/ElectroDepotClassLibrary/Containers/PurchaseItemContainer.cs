using System.Globalization;
using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Containers
{
    public class PurchaseItemContainer
    {
        private readonly PurchaseItem _purchaseItem;
        private readonly Component _component;
        public int Quantity { get {  return _purchaseItem.Quantity; } }
        public double PricePerUnit { get {  return _purchaseItem.PricePerUnit; } }
        public string PricePerUnitAsCurrency { get {  return _purchaseItem.PricePerUnit.ToString("C", new CultureInfo("pl-PL")); } }
        public string Name { get { return _component.Name; } }
        public string Manufacturer { get {  return _component.Manufacturer; } }
        public Bitmap Image { get {  return _component.Image; } }
        public PurchaseItemContainer(PurchaseItem purchaseItem, Component component)
        {
            _purchaseItem = purchaseItem;
            _component = component;
        }
    }
}
