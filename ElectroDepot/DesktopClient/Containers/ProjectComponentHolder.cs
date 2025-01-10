using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers
{
    public class ProjectComponentHolder
    {
        private readonly Component _component;
        private readonly ProjectComponent _projectComponent;
        public int Quantity { get { return _projectComponent.Quantity; } }
        public string Name { get { return _component.Name; } }
        public string Manufacturer { get { return _component.Manufacturer; } }
        public string ShortDescription { get { return _component.ShortDescription; } }
        public string LongDescription { get { return _component.LongDescription; } }
        public Bitmap Image { get { return _component.Image; } }
        public string CategoryName { get { return _component.Category.Name; } }
        public ProjectComponentHolder(Component component, ProjectComponent projectComponent)
        {
            _projectComponent = projectComponent;
            _component = component;
        }
    }
}
