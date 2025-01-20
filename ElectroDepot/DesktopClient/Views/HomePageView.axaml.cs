using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System;
using System.Linq;

namespace DesktopClient.Views;

public partial class HomePageView : UserControl
{
    public HomePageView()
    {
        InitializeComponent();
        var itemsControl = this.FindControl<ItemsControl>("SuppliersItemsControl");

        // Handle the window's SizeChanged event
        this.SizeChanged += (sender, args) =>
        {
            //var uniformGrid = itemsControl.GetVisualDescendants()
            //    .OfType<UniformGrid>()
            //    .FirstOrDefault();

            //if (uniformGrid != null)
            //{
            //    // Calculate available width
            //    double availableWidth = this.Bounds.Width;

            //    // Define desired item width (e.g., 150 pixels)
            //    double desiredItemWidth = 150;

            //    // Calculate and set the number of columns
            //    int columns = Math.Max(1, (int)(availableWidth / desiredItemWidth));
            //    uniformGrid.Columns = columns;

            //    //uniformGrid.InvalidateMeasure();
            //    //uniformGrid.InvalidateArrange();
            //}
        };
    }

}