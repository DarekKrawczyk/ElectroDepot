namespace ElectroDepotClassLibrary.Endpoints
{
    public static class ComponentEndpoints
    {
        public static string Create() => "ElectroDepot/Components/Create";
        public static string GetAll() => "ElectroDepot/Components/GetAll";
        public static string GetByID(int ID) => $"ElectroDepot/Components/GetComponentByID/{ID}";
        public static string GetByIDWithImage(int ID) => $"ElectroDepot/Components/GetComponentByIDWithImage/{ID}";
        public static string GetByName(string Name) => $"ElectroDepot/Components/GetComponentByName/{Name}";
        public static string GetUserComponents(int UserID) => $"ElectroDepot/Components/GetUserComponents/{UserID}";
        public static string GetPurchaseItemsFromComponent(int ComponentID) => $"ElectroDepot/Components/GetPurchaseItemsFromComponent/{ComponentID}";
        public static string GetAvailableComponentsFromUser(int UserID) => $"ElectroDepot/Components/GetAvailableComponentsFromUser/{UserID}";
        public static string GetAvailableComponentsFromUserWithImage(int UserID) => $"ElectroDepot/Components/GetAvailableComponentsFromUserWithImage/{UserID}";
        public static string Update(int ID) => $"ElectroDepot/Components/Update/{ID}";
        public static string Delete(int ID) => $"ElectroDepot/Components/Delete/{ID}";
    }
}
