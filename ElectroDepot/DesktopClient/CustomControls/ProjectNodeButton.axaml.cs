using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using DesktopClient.Templates;
using System;

namespace DesktopClient.CustomControls;

public class ProjectNodeButton : Button
{
    public static readonly StyledProperty<string> ProjectNameProperty = AvaloniaProperty.Register<MenuButton, string>(nameof(ProjectName), "Podlewaczka na kiju xD");

    public string ProjectName
    {
        get { return GetValue(ProjectNameProperty); }
        set { SetValue(ProjectNameProperty, value); }
    }

    public static readonly StyledProperty<Bitmap> ProjectImageProperty = AvaloniaProperty.Register<MenuButton, Bitmap>(nameof(ProjectImage), ImageHelper.LoadFromResource(new Uri($"avares://ElectroDepot/Assets/TempProjectImage.png")));

    public Bitmap ProjectImage
    {
        get => this.GetValue(ProjectImageProperty);
        set => SetValue(ProjectImageProperty, value);
    }

    public static readonly StyledProperty<string> ProjectDescriptionProperty = AvaloniaProperty.Register<MenuButton, string>(nameof(ProjectDescription), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");

    public string ProjectDescription
    {
        get { return GetValue(ProjectDescriptionProperty); }
        set { SetValue(ProjectDescriptionProperty, value); }
    }
}