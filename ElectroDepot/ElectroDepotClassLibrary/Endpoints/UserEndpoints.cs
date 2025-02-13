namespace ElectroDepotClassLibrary.Endpoints
{
    public static class UserEndpoints
    {
        public static string Create() => "ElectroDepot/Users/Create";
        public static string GetAll() => "ElectroDepot/Users/GetAll";
        public static string GetServerStatus() => "ElectroDepot/Users/ServerStatus";
        public static string GetByID(int ID) => $"ElectroDepot/Users/GetUserByID/{ID}";
        public static string GetByUsername(string Name) => $"ElectroDepot/Users/GetUserByUsername/{Name}";
        public static string GetByEMail(string EMail) => $"ElectroDepot/Users/GetUserByEMail/{EMail}";
        public static string Update(int ID) => $"ElectroDepot/Users/Update/{ID}";
        public static string Delete(int ID) => $"ElectroDepot/Users/Delete/{ID}";
    }
}
