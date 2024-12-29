using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Containers;
using ElectroDepotClassLibrary.Models;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows.Input;

namespace DesktopClient.CustomControls;

public class ImageSelector : ListBox
{
    public static readonly StyledProperty<string> MyTextProperty = AvaloniaProperty.Register<ImageSelector, string>(
        nameof(MyText), "empty", false, BindingMode.TwoWay);

    public string MyText
    {
        get => GetValue(MyTextProperty);
        set => SetValue(MyTextProperty, value);
    }


    public static readonly StyledProperty<DataGridCollectionView> FilteredImagesProperty =
    AvaloniaProperty.Register<ImageSelector, DataGridCollectionView>(nameof(FilteredImages));

    public DataGridCollectionView FilteredImages
    {
        get => GetValue(FilteredImagesProperty);
        set => SetValue(FilteredImagesProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<ImageContainer>> ImageContainersProperty =
        AvaloniaProperty.Register<ImageSelector, ObservableCollection<ImageContainer>>(
            nameof(ImageContainers));


    public ObservableCollection<ImageContainer> ImageContainers
    {
        get => GetValue(ImageContainersProperty);
        set => SetValue(ImageContainersProperty, value);
    }

    public static readonly StyledProperty<ImageContainer> SelectedImageProperty =
        AvaloniaProperty.Register<ImageSelector, ImageContainer>(nameof(SelectedImage), defaultBindingMode: BindingMode.TwoWay);

    public ImageContainer SelectedImage
    {
        get => GetValue(SelectedImageProperty);
        set => SetValue(SelectedImageProperty, value);
    }


    private TextBox searchTextBox;
    private ListBox imagesListBox;
    private Button selectButton;
    //public ImageSelector()
    //{
    //    //Images = new ObservableCollection<ImageContainer>();

    //    DataContext = this;

    //    FilteredImages = new DataGridCollectionView(ImageContainers);
    //    FilteredImages.Filter = (object item) =>
    //    {
    //        if (item != null && item is ImageContainer imgContainer)
    //        {
    //            if (searchTextBox != null)
    //            {
    //                if (imgContainer.Name.ToLower().Contains(searchTextBox!.Text.ToLower(), StringComparison.InvariantCulture))
    //                {
    //                    return true;
    //                }
    //                else
    //                {
    //                    return false;
    //                }
    //            }
    //            else
    //            {
    //                return true;
    //            }
    //        }
    //        return true;
    //    };
    //}

    // Define dependency properties for the commands
    public static readonly StyledProperty<ICommand?> SelectCommandProperty =
        AvaloniaProperty.Register<ImageSelector, ICommand?>(nameof(SelectCommand));

    public static readonly StyledProperty<ICommand?> CancelCommandProperty =
        AvaloniaProperty.Register<ImageSelector, ICommand?>(nameof(CancelCommand));

    // CLR property for SelectCommand
    public ICommand? SelectCommand
    {
        get => GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    // CLR property for CancelCommand
    public ICommand? CancelCommand
    {
        get => GetValue(CancelCommandProperty);
        set => SetValue(CancelCommandProperty, value);
    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        FilteredImages.Refresh();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        var inputtext = e.NameScope.Find<Control>("TextBoxInput");
        var imagesListBoxx = e.NameScope.Find<Control>("ImagesListBox");
        var selectButtonn = e.NameScope.Find<Control>("SelectButton");

        if (inputtext != null)
        {
            searchTextBox = (inputtext as TextBox);
        }

        if (imagesListBoxx != null)
        {
            imagesListBox = (imagesListBoxx as ListBox);
        }

        if (selectButtonn != null)
        {
            selectButton = (selectButtonn as Button);
        }

        //Images = new ObservableCollection<ImageContainer>();

        //DataContext = this;

        FilteredImages = new DataGridCollectionView(ImageContainers);
        FilteredImages.Filter = (object item) =>
        {
            if (item != null && item is ImageContainer imgContainer)
            {
                if (searchTextBox != null && searchTextBox.Text != null)
                {
                    if (imgContainer.Name.ToLower().Contains(searchTextBox.Text.ToLower(), StringComparison.InvariantCulture))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            return true;
        };


        EvaluateSelection();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        searchTextBox.TextChanged += ImageSelector_TextChanged;
        imagesListBox.SelectionChanged += ImagesListBox_SelectionChanged;
    }

    private void ImagesListBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        EvaluateSelection();
    }

    private void EvaluateSelection()
    {
        if (imagesListBox.SelectedItem == null)
        {
            SelectedImage = null;
        }
        else
        {
            SelectedImage = imagesListBox.SelectedItem as ImageContainer;
        }

        if (imagesListBox.SelectedItem != null)
        {
            selectButton.IsEnabled = true;
        }
        else
        {
            selectButton.IsEnabled = false;
        }
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        searchTextBox.TextChanged -= ImageSelector_TextChanged;
        imagesListBox.SelectionChanged -= ImagesListBox_SelectionChanged;
    }

    private void ImageSelector_TextChanged(object? sender, TextChangedEventArgs e)
    {
        FilteredImages.Refresh();
        EvaluateSelection();
    }
}