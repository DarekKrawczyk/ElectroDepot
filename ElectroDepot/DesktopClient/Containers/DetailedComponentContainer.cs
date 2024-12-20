﻿using ElectroDepotClassLibrary.Models;

namespace DesktopClient.Containers
{
    public class DetailedComponentContainer
    {
        private readonly Component _component;
        private readonly OwnsComponent _ownedComponent;
        private readonly OwnsComponent _unusedComponent;
        public Category Category { get { return _component.Category; } }
        public int ID { get { return _component.ID; } }
        public int CategoryID { get { return _component.CategoryID; } }
        public string Name { get { return _component.Name; } }
        public string Manufacturer { get { return _component.Manufacturer; } }
        public string Description { get { return _component.Description; } }
        public int OwnedAmount { get { return _ownedComponent.Quantity; } }
        public int AvailableAmount { get { return _unusedComponent.Quantity; } }

        public DetailedComponentContainer(Component component, OwnsComponent ownedComponent, OwnsComponent unusedComponent)
        {
            _component = component;
            _ownedComponent = ownedComponent;
            _unusedComponent = unusedComponent;
        }
    }
}
