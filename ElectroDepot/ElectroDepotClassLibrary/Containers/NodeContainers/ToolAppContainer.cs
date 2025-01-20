using Avalonia.Media.Imaging;

namespace ElectroDepotClassLibrary.Containers.NodeContainers
{
    public class ToolAppContainer
    {
        public string Name { get; set; }
        public Bitmap Image {  get; set; }
        public string ExecutionLink { get; set; }
        public ToolAppContainer(string name, Bitmap image, string executionLink)
        {
            Name = name;
            Image = image;
            ExecutionLink = executionLink;
        }
    }
}
