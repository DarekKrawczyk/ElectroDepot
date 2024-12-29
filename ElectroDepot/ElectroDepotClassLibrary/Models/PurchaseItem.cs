using ElectroDepotClassLibrary.DTOs;

namespace ElectroDepotClassLibrary.Models
{
    public class PurchaseItem
    {
        public int ID { get; set; }
        public int PurchaseID { get; set; }
        public int ComponentID { get; set; }
        public int Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public PurchaseItem(int id, int purchaseID, int componentID, int quantity, double pricePerUnit)
        {
            ID = id;
            PurchaseID = purchaseID;
            ComponentID = componentID;
            Quantity = quantity;
            PricePerUnit = pricePerUnit;
        }

        public override string ToString()
        {
            return $"ID: '{ID}', PurchaseID: '{PurchaseID}', ComponentID: '{ComponentID}', Quantity: '{Quantity}', PricePerUnit: '{PricePerUnit}'";
        }
    }

    internal static class PurchaseItemExtensionMethods
    {
        public static PurchaseItemDTO ToDTO(this PurchaseItem purchaseItem)
        {
            return new PurchaseItemDTO(ID: purchaseItem.ID, PurchaseID: purchaseItem.PurchaseID, ComponentID: purchaseItem.ComponentID, Quantity: purchaseItem.Quantity, PricePerUnit: purchaseItem.PricePerUnit);
        }
        public static UpdatePurchaseItemDTO ToUpdateDTO(this PurchaseItem ownsComponent)
        {
            return new UpdatePurchaseItemDTO(Quantity: ownsComponent.Quantity, PricePerUnit: ownsComponent.PricePerUnit);
        }
        public static CreatePurchaseItemDTO ToCreateDTO(this PurchaseItem ownsComponent)
        {
            return new CreatePurchaseItemDTO(PurchaseID: ownsComponent.PurchaseID, ComponentID: ownsComponent.ComponentID, Quantity: ownsComponent.Quantity, PricePerUnit: ownsComponent.PricePerUnit);
        }
        public static PurchaseItem ToModel(this PurchaseItemDTO purchaseItemDTO)
        {
            return new PurchaseItem(id: purchaseItemDTO.ID, purchaseID: purchaseItemDTO.PurchaseID, componentID: purchaseItemDTO.ComponentID, quantity: purchaseItemDTO.Quantity, pricePerUnit: purchaseItemDTO.PricePerUnit);
        }
    }
}
