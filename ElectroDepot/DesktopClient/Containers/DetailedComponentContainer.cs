using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.Models;

namespace DesktopClient.Containers
{
    public class DetailedComponentContainer
    {
        private readonly Component _component;
        private readonly OwnsComponent _ownedComponent;
        private readonly OwnsComponent _unusedComponent;
        public Component Component { get { return _component; } }
        public Category Category { get { return _component.Category; } }
        public Bitmap Image { get { return _component.Image; } }
        public int ID { get { return _component.ID; } }
        public int CategoryID { get { return _component.CategoryID; } }
        public string Name { get { return _component.Name; } }
        public string Manufacturer { get { return _component.Manufacturer; } }
        public string ShortDescription { get { return _component.ShortDescription; } }
        public string LongDescription { get { return _component.LongDescription; } }
        public string DatasheetURL { get { return _component.DatasheetLink; } } 
        public int OwnedAmount { get { return _ownedComponent.Quantity; } }
        public int AvailableAmount { get { return _unusedComponent.Quantity; } }
        public int UsedInProjects { get {  return _ownedComponent.Quantity - _unusedComponent.Quantity; } }

        public DetailedComponentContainer(Component component, OwnsComponent ownedComponent, OwnsComponent unusedComponent)
        {
            _component = component;
            _ownedComponent = ownedComponent;
            _unusedComponent = unusedComponent;
        }
    }
}
