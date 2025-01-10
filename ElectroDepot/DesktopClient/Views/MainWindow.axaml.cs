using Avalonia;
using Avalonia.Controls;

namespace DesktopClient.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
        }
    }
}