namespace ElectroDepotClassLibrary.Endpoints
{
    public static class ProjectComponentEndpoints
    {
        public static string Create() => "ElectroDepot/ProjectComponents/Create";
        public static string GetAll() => "ElectroDepot/ProjectComponents/GetAll";
        public static string GetByID(int ID) => $"ElectroDepot/ProjectComponents/GetByID/{ID}";
        public static string GetAllByProject(int ID) => $"ElectroDepot/ProjectComponents/GetAllByProject/{ID}";
        public static string GetAllProjectComponentsOfComponents(int ComponentID) => $"ElectroDepot/ProjectComponents/GetProjectComponentsOfComponents/{ComponentID}";
        public static string GetByEMail(string EMail) => $"ElectroDepot/ProjectComponents/GetUserByEMail/{EMail}";
        public static string Update(int ID) => $"ElectroDepot/ProjectComponents/Update/{ID}";
        public static string Delete(int ID) => $"ElectroDepot/ProjectComponents/Delete/{ID}";
    }
}
