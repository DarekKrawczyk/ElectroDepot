namespace ElectroDepotClassLibrary.Endpoints
{
    public class PredefinedImageEndpoint
    {
        public static string Create() => "ElectroDepot/PredefinedImages/Create";
        public static string GetAllPredefinedImages() => $"ElectroDepot/PredefinedImages/GetAllPredefinedImages";
        public static string Update(int ID) => $"ElectroDepot/PredefinedImages/Update/{ID}";
        public static string Delete(int ID) => $"ElectroDepot/PredefinedImages/Delete/{ID}";
    }
}
