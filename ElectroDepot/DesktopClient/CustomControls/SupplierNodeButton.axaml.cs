using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media.Imaging;
using DesktopClient.Templates;
using System;

namespace DesktopClient.CustomControls;

public class SupplierNodeButton : Button
{
    /// <summary>
    /// ProjectImage StyledProperty definition
    /// </summary>
    public static readonly StyledProperty<Bitmap> ImageProperty =
        AvaloniaProperty.Register<MenuButton, Bitmap>(nameof(Image), ImageHelper.LoadFromResource(new Uri($"avares://ElectroDepot/Assets/TempLogo.png")));

    /// <summary>
    /// Gets or sets the ProjectImage property. This StyledProperty 
    /// indicates ....
    /// </summary>
    public Bitmap Image
    {
        get => this.GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }
}