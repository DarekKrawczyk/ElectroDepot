﻿namespace ElectroDepotClassLibrary.Endpoints
{
    public static class ComponentEndpoints
    {
        public static string Create() => "ElectroDepot/Components/Create";
        public static string GetAll() => "ElectroDepot/Components/GetAll";
        public static string GetByID(int ID) => $"ElectroDepot/Components/GetComponentByID/{ID}";
        public static string GetByName(string Name) => $"ElectroDepot/Components/GetComponentByName/{Name}";
        public static string GetUserComponents(int UserID) => $"ElectroDepot/Components/GetUserComponents/{UserID}";
        public static string Update(int ID) => $"ElectroDepot/Components/Update/{ID}";
        public static string Delete(int ID) => $"ElectroDepot/Components/Delete/{ID}";
    }
}
