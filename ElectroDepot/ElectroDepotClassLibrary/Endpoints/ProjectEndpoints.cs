namespace ElectroDepotClassLibrary.Endpoints
{
    public static class ProjectEndpoints
    {
        public static string Create() => "ElectroDepot/Projects/Create";
        public static string GetAll() => "ElectroDepot/Projects/GetAll";
        public static string GetAllOfUser(int ID) => $"ElectroDepot/Projects/GetAllOfUser/{ID}";
        public static string GetAllComponentsFromProject(int ID) => $"ElectroDepot/Projects/GetAllComponentsFromProject/{ID}";
        public static string GetByID(int ID) => $"ElectroDepot/Projects/GetByID/{ID}";
        public static string GetImageOfProjectByID(int ID) => $"ElectroDepot/Projects/GetImageOfProjectByID/{ID}";
        public static string GetProjectOfProjectComponent(int ID) => $"ElectroDepot/Projects/GetProjectOfProjectComponent/{ID}";
        public static string GetPriceByID(int ID) => $"ElectroDepot/Projects/GetPriceByID/{ID}";
        public static string Update(int ID) => $"ElectroDepot/Projects/Update/{ID}";
        public static string Delete(int ID) => $"ElectroDepot/Projects/Delete/{ID}";
    }
}
