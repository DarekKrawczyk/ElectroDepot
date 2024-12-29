using ElectroDepotClassLibrary.DTOs;

namespace ElectroDepotClassLibrary.Models
{
    public class ProjectComponent
    {
        public int ID { get; }
        public int ComponentID { get; }
        public int ProjectID { get; }
        public int Quantity { get; }
        public Component Component { get; }
        public ProjectComponent(int id, int projectID, int componentID, Component component, int quantity)
        {
            ID = id;
            ProjectID = projectID;
            ComponentID = componentID;
            Component = component;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"ID: '{ID}', ProjectID: '{ProjectID}', ComponentID: '{ComponentID}', Quantity: '{Quantity}'";
        }
    }

    internal static class ProjectComponentExtensionMethods
    {
        public static ProjectComponentDTO ToDTO(this ProjectComponent ownsComponent)
        {
            return new ProjectComponentDTO(ID: ownsComponent.ID, ProjectID: ownsComponent.ProjectID, ComponentID: ownsComponent.ComponentID, Quantity: ownsComponent.Quantity);
        }
        public static UpdateProjectComponentDTO ToUpdateDTO(this ProjectComponent ownsComponent)
        {
            return new UpdateProjectComponentDTO(Quantity: ownsComponent.Quantity);
        }
        public static CreateProjectComponentDTO ToCreateDTO(this ProjectComponent ownsComponent)
        {
            return new CreateProjectComponentDTO(ProjectID: ownsComponent.ProjectID, ComponentID: ownsComponent.ComponentID, Quantity: ownsComponent.Quantity);
        }
        public static ProjectComponent ToModel(this ProjectComponentDTO projectComponentDTO)
        {
            return new ProjectComponent(id: projectComponentDTO.ID, projectID: projectComponentDTO.ProjectID, componentID: projectComponentDTO.ComponentID, component: null, quantity: projectComponentDTO.Quantity);
        }
    }
}
