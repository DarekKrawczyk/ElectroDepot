using ElectroDepotClassLibrary.Models;

namespace ElectroDepotClassLibrary.Containers
{
    public class DetailedComponentContainer
    {
        private readonly Component _component;
        private readonly OwnsComponent _ownedComponent;

        public Component Component { get { return _component; } }
        public OwnsComponent Owned { get { return _ownedComponent; } }
        public int AvailableAmount { get; set; }

        public DetailedComponentContainer(Component component, OwnsComponent ownedComponent, int avaiableAmount)
        {
            _component = component;
            _ownedComponent = ownedComponent;
            AvailableAmount = avaiableAmount;
        }

        public DetailedComponentContainer(DetailedComponentContainer other)
        {
            _component = new Component(other.Component);
            _ownedComponent = new OwnsComponent(other._ownedComponent);
            AvailableAmount = other.AvailableAmount;
        }
    }
}
