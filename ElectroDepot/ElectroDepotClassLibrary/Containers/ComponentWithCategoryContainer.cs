using Avalonia.Media.Imaging;
using ElectroDepotClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectroDepotClassLibrary.Containers
{
    public class ComponentWithCategoryContainer
    {
        public Component Component { get; }
        public Category Category { get; }
        public string Name { get { return Component.Name; } }
        public string Manufacturer { get { return Component.Manufacturer; } }
        public string ShortDescription { get { return Component.ShortDescription; } }
        public string LongDescription { get { return Component.LongDescription; } }
        public string DatasheetLink { get { return Component.DatasheetLink; } }
        public Bitmap Image { get { return Component.Image; } }
        public string CategoryName { get {  return Category.Name; } }
        public string CategoryDescription { get { return Category.Description; } }
        public ComponentWithCategoryContainer(Component component, Category category)
        {
            Component = component;
            Category = category;
        }
    }
}
