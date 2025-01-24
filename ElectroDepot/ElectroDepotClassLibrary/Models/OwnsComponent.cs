using ElectroDepotClassLibrary.DTOs;

namespace ElectroDepotClassLibrary.Models
{
    public class OwnsComponent
    {
        public int ID { get; }
        public int UserID { get; }
        public int ComponentID { get; }
        public int Quantity { get; }
        public OwnsComponent(int id, int userID, int componentID, int quantity)
        {
            ID = id;
            UserID = userID;
            ComponentID = componentID;
            Quantity = quantity;
        }

        public OwnsComponent(OwnsComponent other)
        {
            ID = other.ID;
            UserID = other.UserID;
            ComponentID = other.ComponentID;
            Quantity = other.Quantity;
        }

        public override string ToString()
        {
            return $"ID: '{ID}', UserID: '{UserID}', ComponentID: '{ComponentID}', Quantity: '{Quantity}'";
        }
    }

    internal static class OwnsComponentExtensionMethods
    {
        public static OwnsComponentDTO ToDTO(this OwnsComponent ownsComponent)
        {
            return new OwnsComponentDTO(ID: ownsComponent.ID, UserID: ownsComponent.UserID, ComponentID: ownsComponent.ComponentID, Quantity: ownsComponent.Quantity);
        }
        public static UpdateOwnsComponentDTO ToUpdateDTO(this OwnsComponent ownsComponent)
        {
            return new UpdateOwnsComponentDTO(Quantity: ownsComponent.Quantity);
        }
        public static CreateOwnsComponentDTO ToCreateDTO(this OwnsComponent ownsComponent)
        {
            return new CreateOwnsComponentDTO(UserID: ownsComponent.UserID, ComponentID: ownsComponent.ComponentID, Quantity: ownsComponent.Quantity);
        }
        public static OwnsComponent ToModel(this OwnsComponentDTO ownsComponentDTO)
        {
            return new OwnsComponent(id: ownsComponentDTO.ID, userID: ownsComponentDTO.UserID, componentID: ownsComponentDTO.ComponentID, quantity: ownsComponentDTO.Quantity);
        }
    }
}
