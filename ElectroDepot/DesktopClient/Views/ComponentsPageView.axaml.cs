using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace DesktopClient.Views;

public partial class ComponentsPageView : UserControl
{
    public ComponentsPageView()
    {
        InitializeComponent();
        HorizontalScrollViewer.PointerWheelChanged += PointerWheelChangedHandler;
    }

    private void PointerWheelChangedHandler(object? sender, Avalonia.Input.PointerWheelEventArgs e)
    {
        if (sender is ScrollViewer scrollViewer)
        {
            scrollViewer.Offset = new Vector(scrollViewer.Offset.X - e.Delta.Y * 50, scrollViewer.Offset.Y);
            e.Handled = true;
        }
    }

    private void DataGrid_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        var source = e.Source;
        if (source is Border)
        {
            Console.WriteLine("xD");
        }
    }
}