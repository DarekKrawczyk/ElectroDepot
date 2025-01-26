using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ElectroDepotClassLibrary.Stores;
using ElectroDepotClassLibrary.Models;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using DesktopClient.Containers;
using Avalonia.Collections;
using Avalonia.Media.Imaging;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System.IO;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using System.Threading;
using ElectroDepotClassLibrary.Utility;
using System.ComponentModel.DataAnnotations;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System.Diagnostics;
using Avalonia.Input;
using Avalonia;
using DesktopClient.Utils;
using DesktopClient.Navigation;
using DynamicData;
using System.Reactive.Subjects;
using DesktopClient.Services;
using DynamicData.Binding;
using System.Drawing.Printing;
using System.Reactive.Linq;
using DynamicData.Operators;
using System.Reactive.Concurrency;
using Avalonia.Controls.Primitives;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using System.Security.Cryptography.X509Certificates;
using Avalonia.Data;
using Microsoft.Win32;

namespace DesktopClient.ViewModels
{
    public partial class ComponentsPageViewModel : RootNavigatorViewModel, INavParamInterpreter
    {
        #region Tab navigation
        partial void OnSelectedTabChanged(int value)
        {
            Evaluate_AddTabVisibilty();
        }

        public void Evaluate_AddTabVisibilty()
        {
            Evaluate_Add_IsComponentsTabEnabled();
            Evaluate_Add_IsAddTabEnabled();
            Evaluate_Add_IsPreviewTabEnabled();
        }

        [ObservableProperty]
        private bool _add_IsComponentsTabEnabled;

        private void Evaluate_Add_IsComponentsTabEnabled()
        {
            bool isVisible = false;
            if(SelectedTab == 0)
            {
                isVisible = true;
            }
            else if(SelectedTab == 1)
            {
                isVisible = !WasAddFormChanged();
            }
            else if(SelectedTab == 2)
            {
                if(Preview_IsEditing == true)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            else
            {
                isVisible = true;
            }
            Add_IsComponentsTabEnabled = isVisible;
        }
        
        [ObservableProperty]
        private bool _add_IsAddTabEnabled;

        private void Evaluate_Add_IsAddTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = true;
            }
            else if (SelectedTab == 1)
            {
                isVisible = true;
            }
            else if (SelectedTab == 2)
            {
                if (Preview_IsEditing == true)
                {
                    isVisible = false;
                }
                else
                {
                    isVisible = true;
                }
            }
            else
            {
                isVisible = true;
            }
            Add_IsAddTabEnabled = isVisible;
        }

        [ObservableProperty]
        private bool _add_IsPreviewTabEnabled;

        private void Evaluate_Add_IsPreviewTabEnabled()
        {
            bool isVisible = false;
            if (SelectedTab == 0)
            {
                isVisible = false; // TODO: Only if selected component!
            }
            else if (SelectedTab == 1)
            {
                isVisible = false;  // Components was not added so why should it be visible?
            }
            else if (SelectedTab == 2)
            {
                isVisible = true;
            }
            else
            {
                isVisible = true;
            }
            Add_IsPreviewTabEnabled = isVisible;
        }

        private void ClearPreviewData()
        {
            Preview_PreviewedComponent = new DetailedComponentContainer(Components_SelectedComponent);
            Preview_Image = Preview_PreviewedComponent.Image;
            Preview_NameField = Preview_PreviewedComponent.Name;
            Preview_ManufacturerField = Preview_PreviewedComponent.Manufacturer;
            Preview_CategoryField = Preview_PreviewedComponent.Category.Name;
            if(Preview_CategoryField != string.Empty)
            {
                Preview_CategoryComboBoxItem = Preview_CategoryField;
            }
            Preview_DatasheetField = Preview_PreviewedComponent.DatasheetURL;
            Preview_AboutField = Preview_PreviewedComponent.ShortDescription;
            Preview_DescriptionField = Preview_PreviewedComponent.LongDescription;
            if(Preview_DatasheetField != null && Preview_DatasheetField != string.Empty)
            {
                Preview_CanDisplayDatasheet = true;
            }
        }

        private void NavigationPreparePreviewTab()
        {
            ClearPreviewData();
        }

        public async Task NavigateTab(ComponentTab tab)
        {
            switch (tab)
            {
                case ComponentTab.Components:
                    if (SelectedTab == 0) break;    // User is on this Tab so do not change anything.
                    else
                    {
                        if(SelectedTab == 1)
                        {
                            bool wasChanged = WasAddFormChanged();

                            if (wasChanged == true)
                            {
                                string result = await MsBoxService.DisplayMessageBox("It looks like you have unsaved changes. If you cancel this opertaion these changes will be lost. Do you want to proceed?", Icon.Warning);

                                if (result == "Yes")
                                {
                                    // Clear changes and navigate to 'Components' tab.
                                    Add_ClearComponent();
                                    SelectedTab = 0;
                                }
                                else
                                {
                                    // Stay where you are.
                                }
                            }
                            else
                            {
                                SelectedTab = 0;
                            }
                        }
                        else if(SelectedTab == 2)
                        {
                            SelectedTab = 0;
                        }
                        else if (SelectedTab == 3)
                        {
                            SelectedTab = 0;
                        }
                    }
                    break;
                case ComponentTab.Add:
                    SelectedTab = 1;
                    break;
                case ComponentTab.Preview:
                    RefreshSelectedComponentsProjectSource();
                    RefreshSelectedComponentsPurchasesSource();
                    NavigationPreparePreviewTab();
                    //ChangeToPreviewMode();
                    //PrepareForPreview();
                    SelectedTab = 2;
                    break;
                case ComponentTab.Edit:
                    RefreshSelectedComponentsProjectSource();
                    RefreshSelectedComponentsPurchasesSource();
                    NavigationPreparePreviewTab();
                    //ChangeToEditMode();
                    //Modify_ClearDataToDefault();
                    SelectedTab = 2;
                    break;
                default:
                    SelectedTab = 0;
                    break;
            }

        }
        #endregion

        #region Components tab
        [ObservableProperty]
        private DetailedComponentContainer _components_SelectedComponent;

        #endregion

        #region Add tab

        #region Main fields section
        #region Name field
        [RelayCommand]
        public void AddTab_ClearName()
        {
            _hasUserInteractedWithName = false;
            Add_ComponentName = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyNameClipboard))]
        public void AddTab_CopyNameClipboard()
        {
            ClipboardManager.SetText(Add_ComponentName);
        }

        public bool AddTab_CanCopyNameClipboard()
        {
            if (Add_ComponentName != null && Add_ComponentName.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Manufacturer field
        [RelayCommand]
        public void AddTab_ClearManufacturer()
        {
            _hasUserInteractedWithManufacturer = false;
            Add_Manufacturer = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyManufacturerClipboard))]
        public void AddTab_CopyManufacturerClipboard()
        {
            ClipboardManager.SetText(Add_Manufacturer);
        }

        public bool AddTab_CanCopyManufacturerClipboard()
        {
            if (Add_Manufacturer != null && Add_Manufacturer.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Category field
        [RelayCommand]
        public void AddTab_ClearCategory()
        {
            _hasUserInteractedWithCategory = false;
            Add_Category = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyCategoryClipboard))]
        public void AddTab_CopyCategoryClipboard()
        {
            ClipboardManager.SetText(Add_Category as string);
        }

        public bool AddTab_CanCopyCategoryClipboard()
        {
            if (Add_Category != null)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region Datasheet
        [RelayCommand]
        public void AddTab_DatasheetName()
        {
            Add_DatasheetLink = null;
        }

        [RelayCommand(CanExecute = nameof(AddTab_CanCopyDatasheetClipboard))]
        public void AddTab_CopyDatasheetClipboard()
        {
            ClipboardManager.SetText(Add_DatasheetLink);
        }

        public bool AddTab_CanCopyDatasheetClipboard()
        {
            if (Add_DatasheetLink != null && Add_DatasheetLink.Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion
        #endregion

        #region Main buttons section
        private bool WasAddFormChanged()
        {
            bool nameChanged = Add_ComponentName != null && Add_ComponentName != string.Empty;
            bool manufacturerChanged = Add_Manufacturer != null && Add_Manufacturer != string.Empty;
            bool categoryChanged = Add_Category != null;
            bool datasheetChanged = Add_DatasheetLink != null && Add_DatasheetLink != string.Empty;
            //bool imageChanged = CurrentAddPredefinedImage != ?; // TODO: Think about that.
            bool aboutChanged = Add_ShortDescription != null && Add_ShortDescription != string.Empty;
            bool descriptionChanged = Add_FullDescription != null && Add_FullDescription != string.Empty;

            bool ifAny = (nameChanged || manufacturerChanged || categoryChanged || datasheetChanged || aboutChanged || descriptionChanged);
            return ifAny;
        }
        [RelayCommand]
        public async Task Add_Cancel()
        {
            /*  User can 'Cancel' the operation everytime and app should navigate him to 'Components' tab.
             *  If User applied any changes to 'Add' tab 'Form' then he should be warned with 'MessageBox'
             */
            await NavigateTab(ComponentTab.Components);
        }
        #endregion

        #region About section
        [RelayCommand]
        public void AddTab_DownloadAbout()
        {
            // TODO: Implement!
        }

        [RelayCommand]
        public void AddTab_ClearAbout()
        {
            _hasUserInteractedWithDescription = false;
            Add_ShortDescription = null;
        }
        #endregion

        #region Description section
        [RelayCommand]
        public void AddTab_DownloadDescription()
        {
            // TODO: Implement!
        }

        [RelayCommand]
        public void AddTab_ClearDescription()
        {
            Add_FullDescription = null;
        }
        #endregion

        #endregion

        #region Preview Tab
        [ObservableProperty]
        private DetailedComponentContainer _preview_PreviewedComponent;

        partial void OnPreview_PreviewedComponentChanged(DetailedComponentContainer value)
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Checks if all of the controlls are makred as 'ok' and are not edited right now!
        /// </summary>
        /// <returns></returns>
        private bool IsEditFormInPreviewState()
        {
            if (Preview_NameEditing == false && Preview_ManufacturerEditing == false && Preview_CategoryEditing == false && Preview_DatasheetEditing == false &&
                Preview_AboutEditing == false && Preview_DescriptionEditing == false && Preview_ImageEditing == false) return true;
            else return false;
        }

        private bool WasEditFormChanged()
        {
            if (Components_SelectedComponent == null) return false;
            bool nameChanged = Preview_NameField != Components_SelectedComponent.Name;
            bool manufacturerChanged = Preview_ManufacturerField != Components_SelectedComponent.Manufacturer;
            bool categoryChanged = (Preview_CategoryField != Components_SelectedComponent.Category.Name) || (Preview_CategoryComboBoxItem != Components_SelectedComponent.Category.Name);
            bool datasheetChanged = Preview_DatasheetField != Components_SelectedComponent.DatasheetURL;
            bool imageChanged = Components_SelectedComponent.Component.ByteImage != ImageConverterUtility.BitmapToBytes(Preview_Image);
            bool aboutChanged = Preview_AboutField != Components_SelectedComponent.ShortDescription;
            bool descriptionChanged = Preview_DescriptionField != Components_SelectedComponent.LongDescription;

            bool ifAny = (nameChanged || manufacturerChanged || categoryChanged || datasheetChanged || aboutChanged || descriptionChanged);
            return ifAny;
        }

        #region Buttons
        [RelayCommand(CanExecute = nameof(Preview_CanClearAllEditing))]
        public void Preview_ClearAllEditing()
        {
            ClearPreviewData();
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
            Preview_SaveWholeCommand.NotifyCanExecuteChanged();
        }

        public bool Preview_CanClearAllEditing()
        {
            bool canClear = WasEditFormChanged() && IsEditFormInPreviewState();
            return canClear;
        }

        [RelayCommand]
        public async Task Preview_CancelEditing()
        {
            bool wasChanged = WasEditFormChanged();

            if (wasChanged == true)
            {
                string result = await MsBoxService.DisplayMessageBox("It looks like you have unsaved changes. If you cancel this opertaion these changes will be lost. Do you want to proceed?", Icon.Warning);

                if (result == "Yes")
                {
                    ClearPreviewData();
                    ChangeToPreviewMode();
                }
                else
                {
                    // Stay where you are.
                }
            }
            else
            {
                ChangeToPreviewMode();
            }
        }
        #endregion

        #region Name
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_NameEditingSaveChangesCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_NameClearCommand))]
        private string _preview_NameField = string.Empty;

        partial void OnPreview_NameFieldChanged(string value)
        {
            Preview_PreviewedComponent.Name = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_NameNotEditing = true;

        partial void OnPreview_NameNotEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [ObservableProperty]
        private bool _preview_NameEditing;

        [RelayCommand(CanExecute = nameof(Preview_CanNameClear))]
        public void Preview_NameClear()
        {
            Preview_NameField = Components_SelectedComponent.Name;
        }

        public bool Preview_CanNameClear()
        {
            if (Components_SelectedComponent == null || Preview_NameField == Components_SelectedComponent.Name) return false;
            else return true;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanExecuteNameEditingSaveChanges))]
        public void Preview_NameEditingSaveChanges()
        {
            Preview_NameEditing = false;
            Preview_NameNotEditing = true;
        }

        public bool Preview_CanExecuteNameEditingSaveChanges()
        {
            if (Preview_NameField == null || Preview_NameField == string.Empty) return false;
            else return true;
        }

        [RelayCommand]
        public void Preview_NameEditingStart()
        {
            Preview_NameNotEditing = false;
            Preview_NameEditing = true;
        }

        [RelayCommand]
        public async Task Preview_NameCopy()
        {
            await ClipboardManager.SetText(Preview_NameField);
        }
        #endregion

        #region Manufacturer
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ManufacturerEditingSaveChangesCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_ManufacturerClearCommand))]
        private string _preview_ManufacturerField = string.Empty;

        partial void OnPreview_ManufacturerFieldChanged(string value)
        {
            Preview_PreviewedComponent.Manufacturer = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_ManufacturerNotEditing = true;

        [ObservableProperty]
        private bool _preview_ManufacturerEditing;

        partial void OnPreview_ManufacturerEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanManufacturerClear))]
        public void Preview_ManufacturerClear()
        {
            Preview_ManufacturerField = Components_SelectedComponent.Manufacturer;
        }

        public bool Preview_CanManufacturerClear()
        {
            if (Components_SelectedComponent == null || Preview_ManufacturerField == Components_SelectedComponent.Manufacturer) return false;
            else return true;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanExecuteManufacturerEditingSaveChanges))]
        public void Preview_ManufacturerEditingSaveChanges()
        {
            Preview_ManufacturerEditing = false;
            Preview_ManufacturerNotEditing = true;
        }

        public bool Preview_CanExecuteManufacturerEditingSaveChanges()
        {
            if (Preview_ManufacturerField == null || Preview_ManufacturerField == string.Empty) return false;
            else return true;
        }

        [RelayCommand]
        public void Preview_ManufacturerEditingStart()
        {
            Preview_ManufacturerNotEditing = false;
            Preview_ManufacturerEditing = true;
        }

        [RelayCommand]
        public async Task Preview_ManufacturerCopy()
        {
            await ClipboardManager.SetText(Preview_ManufacturerField);
        }
        #endregion

        #region Category
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_CategoryClearCommand))]
        private string _preview_CategoryComboBoxItem;

        [ObservableProperty]
        private string _preview_CategoryField = string.Empty;

        partial void OnPreview_CategoryFieldChanged(string value)
        {
            Preview_PreviewedComponent.Category = DatabaseStore.CategorieStore.Categories.FirstOrDefault(x => x.Name == value);
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_CategoryNotEditing = true;

        [ObservableProperty]
        private bool _preview_CategoryEditing;

        partial void OnPreview_CategoryEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanCategoryClear))]
        public void Preview_CategoryClear()
        {
            Preview_PreviewedComponent.Category = Components_SelectedComponent.Category;
            Preview_CategoryComboBoxItem = Preview_PreviewedComponent.Category.Name;
        }

        public bool Preview_CanCategoryClear()
        {
            if (Components_SelectedComponent != null && Components_SelectedComponent.Category.Name != Preview_CategoryComboBoxItem) return true;
            else return false;
        }

        [RelayCommand]
        public void Preview_CategoryEditingSaveChanges()
        {
            Preview_PreviewedComponent.Category = DatabaseStore.CategorieStore.Categories.FirstOrDefault(x => x.Name == Preview_CategoryComboBoxItem);
            Preview_CategoryField = Preview_PreviewedComponent.Category.Name;
            Preview_CategoryEditing = false;
            Preview_CategoryNotEditing = true;
        }

        [RelayCommand]
        public void Preview_CategoryEditingStart()
        {
            Preview_CategoryNotEditing = false;
            Preview_CategoryEditing = true;
        }

        [RelayCommand]
        public async Task Preview_CategoryCopy()
        {
            await ClipboardManager.SetText(Preview_CategoryField);
        }
        #endregion

        #region About
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_AboutEditingSaveChangesCommand))]
        [NotifyCanExecuteChangedFor(nameof(Preview_AboutClearCommand))]
        private string _preview_AboutField = string.Empty;

        partial void OnPreview_AboutFieldChanged(string value)
        {
            Preview_PreviewedComponent.ShortDescription = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_AboutNotEditing = true;

        [ObservableProperty]
        private bool _preview_AboutEditing;

        partial void OnPreview_AboutEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanAboutClear))]
        public void Preview_AboutClear()
        {
            Preview_AboutField = Components_SelectedComponent.Component.ShortDescription;
        }

        public bool Preview_CanAboutClear()
        {
            if (Components_SelectedComponent != null && Preview_AboutField != Components_SelectedComponent.Component.ShortDescription) return true;
            else return false;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanExecuteAboutEditingSaveChanges))]
        public void Preview_AboutEditingSaveChanges()
        {
            Preview_AboutEditing = false;
            Preview_AboutNotEditing = true;
        }

        public bool Preview_CanExecuteAboutEditingSaveChanges()
        {
            if (Preview_AboutField == null || Preview_AboutField == string.Empty) return false;
            else return true;
        }

        [RelayCommand]
        public void Preview_AboutEditingStart()
        {
            Preview_AboutNotEditing = false;
            Preview_AboutEditing = true;
        }

        [RelayCommand]
        public async Task Preview_AboutCopy()
        {
            await ClipboardManager.SetText(Preview_AboutField);
        }
        #endregion

        #region Description
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_DescriptionRevertCommand))]
        private string _preview_DescriptionField = string.Empty;

        partial void OnPreview_DescriptionFieldChanged(string value)
        {
            Preview_PreviewedComponent.LongDescription = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_DescriptionNotEditing = true;

        [ObservableProperty]
        private bool _preview_DescriptionEditing;

        partial void OnPreview_DescriptionEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        public void Preview_DescriptionClear()
        {
            Preview_DescriptionField = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanDescriptionRevert))]
        public void Preview_DescriptionRevert()
        {
            Preview_DescriptionField = Components_SelectedComponent.Component.LongDescription;
        }

        public bool Preview_CanDescriptionRevert()
        {
            if (Components_SelectedComponent != null && Components_SelectedComponent.Component.LongDescription != Preview_DescriptionField) return true;
            else return false;
        }

        [RelayCommand]
        public void Preview_DescriptionEditingSaveChanges()
        {
            Preview_DescriptionEditing = false;
            Preview_DescriptionNotEditing = true;
        }

        [RelayCommand]
        public void Preview_DescriptionEditingStart()
        {
            Preview_DescriptionNotEditing = false;
            Preview_DescriptionEditing = true;
        }

        [RelayCommand]
        public async Task Preview_DescriptionCopy()
        {
            await ClipboardManager.SetText(Preview_DescriptionField);
        }
        #endregion

        #region Datasheet
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_DatasheetRevertCommand))]
        private string _preview_DatasheetField = string.Empty;

        partial void OnPreview_DatasheetFieldChanged(string value)
        {
            Preview_DatasheetIsValid();
            //Preview_CanDisplayDatasheet = true;
            Preview_PreviewedComponent.DatasheetURL = value;
            //Preview_DatasheetDisplayCommand.NotifyCanExecuteChanged();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_DatasheetNotEditing = true;

        [ObservableProperty]
        private bool _preview_DatasheetEditing;

        partial void OnPreview_DatasheetEditingChanged(bool value)
        {
            Preview_DatasheetIsValid();
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanDatasheetRevert))]
        public void Preview_DatasheetRevert()
        {
            Preview_DatasheetField = Components_SelectedComponent.DatasheetURL;
        }

        private bool _isDatasheetDiplayable;
        private bool _isDatasheetValid;

        public bool Preview_CanDatasheetRevert()
        {
            if (Components_SelectedComponent == null || Preview_DatasheetField == Components_SelectedComponent.DatasheetURL) return false;
            else return true;
        }

        [RelayCommand(CanExecute = nameof(CanExecutePreviewDatasheetDisplay))]
        public async Task Preview_DatasheetDisplay()
        {
            // Command logic
        }

        private bool CanExecutePreviewDatasheetDisplay()
        {
            return _isDatasheetDiplayable;
        }

        private bool Preview_CanDatasheetEditingSaveChanges()
        {
            return _isDatasheetValid;
        }

        public async Task Preview_DatasheetIsValid()
        {
            try
            {
                await Task.Delay(100); // Simulate async operation
                ValidationResult validationResult = await LinkValidator.ValidateDatasheetLinkAsync(Preview_DatasheetField, new ValidationContext(this));
                _isDatasheetValid = validationResult == ValidationResult.Success;
                if(_isDatasheetValid == true && Preview_DatasheetField != null && Preview_DatasheetField != string.Empty)
                {
                    _isDatasheetDiplayable = true;
                }
                else
                {
                    _isDatasheetDiplayable = false;
                }
            }
            catch (Exception)
            {
                _isDatasheetValid = false;
                _isDatasheetDiplayable = false;
            }
            finally
            {
                // Notify the command that the CanExecute state has changed
                Preview_DatasheetDisplayCommand.NotifyCanExecuteChanged();
                Preview_DatasheetEditingSaveChangesCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand]
        public void Preview_DatasheetClear()
        {
            Preview_DatasheetField = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanDatasheetEditingSaveChanges))]
        public void Preview_DatasheetEditingSaveChanges()
        {
            Preview_DatasheetEditing = false;
            Preview_DatasheetNotEditing = true;
        }

        [ObservableProperty]
        private bool _preview_CanDisplayDatasheet;

        [RelayCommand]
        public void Preview_DatasheetEditingStart()
        {
            Preview_DatasheetNotEditing = false;
            Preview_DatasheetEditing = true;
        }

        [RelayCommand]
        public async Task Preview_DatasheetCopy()
        {
            await ClipboardManager.SetText(Preview_DatasheetField);
        }
        #endregion

        #region Image
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_ClearImageCommand))]
        private Bitmap _preview_Image;

        partial void OnPreview_ImageChanged(Bitmap value)
        {
            Preview_PreviewedComponent.Image = value;
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(Preview_SaveWholeCommand))]
        private bool _preview_ImageNotEditing = true;

        [ObservableProperty]
        private bool _preview_ImageEditing;

        partial void OnPreview_ImageEditingChanged(bool value)
        {
            Preview_ClearAllEditingCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        public void Preview_ImageEditingSaveChanges()
        {
            Preview_ImageEditing = false;
            Preview_ImageNotEditing = true;
        }

        [RelayCommand]
        public void Preview_ImageEditingStart()
        {
            Preview_ImageNotEditing = false;
            Preview_ImageEditing = true;
        }

        [RelayCommand(CanExecute = nameof(Preview_CanClearImage))]
        public void Preview_ClearImage()
        {
            Preview_Image = Components_SelectedComponent.Image;
        }

        public bool Preview_CanClearImage()
        {
            if (Preview_PreviewedComponent != null && Preview_PreviewedComponent.Component.ByteImage != ImageConverterUtility.BitmapToBytes(Preview_Image)) return true;
            else return false;  
        }
        #endregion

        private void ChangeToEditMode()
        {
            Preview_IsPreviewing = false;
            Preview_IsEditing = true;

            Preview_DescriptionEditing = false;
            Preview_DescriptionNotEditing = true;

            Preview_AboutEditing = false;
            Preview_AboutNotEditing = true;

            Preview_NameEditing = false;
            Preview_NameNotEditing = true;

            Preview_ManufacturerEditing = false;
            Preview_ManufacturerNotEditing = true;

            Preview_CategoryEditing = false;
            Preview_CategoryNotEditing = true;

            Preview_DatasheetEditing = false;
            Preview_DatasheetNotEditing = true;

            Preview_ImageEditing = false;
            Preview_ImageNotEditing = true;
        }

        private void ChangeToPreviewMode()
        {
            Preview_IsPreviewing = true;
            Preview_IsEditing = false;

            Preview_DescriptionEditing = false;
            Preview_DescriptionNotEditing = true;

            Preview_AboutEditing = false;
            Preview_AboutNotEditing = true;

            Preview_NameEditing = false;
            Preview_NameNotEditing = true;

            Preview_ManufacturerEditing = false;
            Preview_ManufacturerNotEditing = true;

            Preview_CategoryEditing = false;
            Preview_CategoryNotEditing = true;

            Preview_DatasheetEditing = false;
            Preview_DatasheetNotEditing = true;

            Preview_ImageEditing = false;
            Preview_ImageNotEditing = true;
        }

        [RelayCommand]
        public void Preview_EditWhole()
        {
            ChangeToEditMode();
        }

        [RelayCommand(CanExecute = nameof(Preview_CanSaveWhole))]
        public async Task Preview_SaveWhole()
        {
            // TODO: Implement

            try
            {
                Component result = await DatabaseStore.ComponentStore.UpdateComponent(Preview_PreviewedComponent.Component);

                if (result != null)
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("Components modified successfully! Do you to be navigated to 'Components' tab?", Icon.Question);

                    //Add_ClearComponent();
                    if (dialogResult == "Yes")
                    {
                        NavigateTab(ComponentTab.Components);
                    }
                }
                else
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("There was an error while editing component. Try again or contact administrator!", Icon.Error);
                }
            }
            catch (Exception exception)
            {

            }

            ChangeToPreviewMode();
        }

        public bool Preview_CanSaveWhole()
        {
            if(Preview_DescriptionNotEditing == true && Preview_AboutNotEditing == true && Preview_NameNotEditing == true && Preview_ManufacturerNotEditing == true &&
               Preview_CategoryNotEditing == true && Preview_DatasheetNotEditing == true && Preview_ImageNotEditing == true && WasEditFormChanged() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [ObservableProperty]
        private bool _preview_IsPreviewing = true;

        [ObservableProperty]
        private bool _preview_IsEditing;
        #endregion

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int _selectedPageSizeIndex = 0;

        public int FirstPageIndex = 1;
        public int SelectedPageSize = 10;

        partial void OnSelectedPageSizeIndexChanged(int value)
        {
            switch (value)
            {
                case 0:
                    SelectedPageSize = 10;
                    break;
                case 1:
                    SelectedPageSize = 25;
                    break;
                case 2:
                    SelectedPageSize = 50;
                    break;
                case 3:
                    SelectedPageSize = 100;
                    break;
                default:
                    SelectedPageSize = 10;
                    break;  
            }
            _pager.OnNext(new PageRequest(FirstPageIndex, SelectedPageSize));
        }

        #region Data source
        //public List<DetailedComponentContainer> ComponentsSource { get; set; }
        public List<DetailedItemPurchaseContainer> SelectedComponentsPurchasesSource { get; set; }
        public List<DetailedItemProjectContainer> SelectedComponentsProjectSource { get; set; }
        private readonly ComponentHolderService _componentsService;
        private readonly ISubject<PageRequest> _pager;
        private readonly ReadOnlyObservableCollection<DetailedComponentContainer> _components;
        public ReadOnlyObservableCollection<DetailedComponentContainer> ComponentsCollection => _components;
        #endregion

        #region Previous page commands
        [RelayCommand(CanExecute = nameof(CanGoToPreviousPage))]
        public void PreviousPage()
        {
            _pager.OnNext(new PageRequest(_currentPage - 1, SelectedPageSize));
        }

        private bool CanGoToPreviousPage()
        {
            return CurrentPage > FirstPageIndex;
        }
        #endregion
        #region Next page commands
        [RelayCommand(CanExecute = nameof(CanGoToNextPage))]
        public void NextPage()
        {
            _pager.OnNext(new PageRequest(_currentPage + 1, SelectedPageSize));
            
        }

        private bool CanGoToNextPage()
        {
            return CurrentPage < TotalPages;
        }
        #endregion
        #region First page commands
        [RelayCommand(CanExecute = nameof(CanGoToFirstPage))]
        public void FirstPage()
        {
            _pager.OnNext(new PageRequest(FirstPageIndex, SelectedPageSize));
        }

        private bool CanGoToFirstPage()
        {
            return CurrentPage > FirstPageIndex;
        }
        #endregion
        #region Last page commands
        [RelayCommand(CanExecute = nameof(CanGoToLastPage))]
        public void LastPage()
        {
            _pager.OnNext(new PageRequest(_totalPages, SelectedPageSize));
        }

        private bool CanGoToLastPage()
        {
            return CurrentPage < TotalPages;
        }
        #endregion

        [ObservableProperty]
        private bool _isSelectingPredefinedImagePopupOpen;

        [RelayCommand]
        private void OpenPredefinedImages()
        {
            IsSelectingPredefinedImagePopupOpen = true;
        }

        [ObservableProperty]
        private ImageContainer _selectedPredefinedImage;

        [ObservableProperty]
        private Bitmap _currentAddPredefinedImage;

        [ObservableProperty]
        private bool _preview_CanPreview;

        [RelayCommand]
        private void SelectPredefinedImages()
        {
            // default avares://DesktopClient/Assets/DefaultComponentImage.png
            
            if (SelectedPredefinedImage == null)
            {
                // No item so do nothing
            }
            else
            {
                if(Preview_IsEditing == true)
                {
                    Preview_Image = SelectedPredefinedImage.Image;
                }
                else
                {
                    CurrentAddPredefinedImage = SelectedPredefinedImage.Image;
                }
                IsSelectingPredefinedImagePopupOpen = false;
                SelectedPredefinedImage = null;
            }
        }

        [RelayCommand]
        private async void LoadImageFromDevice()
        {
            var file = await DoOpenFilePickerAsync();
            if (file is null) return;

            await using var readStream = await file.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await readStream.CopyToAsync(memoryStream);

            byte[] imageData = memoryStream.ToArray();
            Bitmap imageAsBitmap = ImageConverterUtility.BytesToBitmap(imageData);

            // Load but where
            if (Preview_IsEditing == true)
            {
                Preview_Image = imageAsBitmap;
            }
            else
            {
                CurrentAddPredefinedImage = imageAsBitmap;
            }
        }

        private async Task<IStorageFile?> DoOpenFilePickerAsync()
        {
            if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } provider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            FilePickerOpenOptions options = new FilePickerOpenOptions()
            {
                Title = "Load image",
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new FilePickerFileType("PNG Files") { Patterns = new[] { "*.png" } },
                    new FilePickerFileType("JPEG Files") { Patterns = new[] { "*.jpg", "*.jpeg" } }
                }
            };

            var files = await provider.OpenFilePickerAsync(options);

            return files?.Count >= 1 ? files[0] : null;
        }

        [RelayCommand]
        private void ClosePredefinedImages()
        {
            IsSelectingPredefinedImagePopupOpen = false;
            SelectedPredefinedImage = null;
        }

        [ObservableProperty]
        private string _myText = "XDXDXD";

        #region Observable for data source
        public ObservableCollection<string> Manufacturers { get; set; }
        public ObservableCollection<string> Categories { get; set; }
        public ObservableCollection<SupplierContainer> Suppliers { get; set; } = new ObservableCollection<SupplierContainer>() { new SupplierContainer(new Supplier(0, "XD", "XD", null)) };
        public ObservableCollection<ImageContainer> PredefinedImages { get; set; }

        //public DataGridCollectionView Components { get; set; }
        public DataGridCollectionView PurchasesForSelected { get; set; }
        public DataGridCollectionView ProjectsForSelected{ get; set; }
        #endregion
        #region Observable properties
        [ObservableProperty]
        private bool _previewEnabled = false;

        [ObservableProperty]
        private DetailedComponentContainer _selectedComponent;

        [ObservableProperty]
        private string _searchByNameOrDesc = string.Empty;

        [ObservableProperty]
        private bool _onlyAvailableFlag;

        [ObservableProperty]
        private string _selectedManufacturer;

        [ObservableProperty]
        private string _selectedCategory;

        private bool _hasUserInteractedWithName;
        private bool _hasUserInteractedWithManufacturer;
        private bool _hasUserInteractedWithDescription;
        private bool _hasUserInteractedWithCategory;

        [ObservableProperty]
        private int _selectedTab;
        #region Add component tab
        [ObservableProperty]
        private bool _add_CanAdd = false;

        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if(e.PropertyName == "HasErrors" || e.PropertyName == nameof(Add_ShortDescription) || e.PropertyName == nameof(Add_Category) ||
               e.PropertyName == nameof(Add_ComponentName) || e.PropertyName == nameof(Add_Manufacturer))
            {
                if (_hasUserInteractedWithCategory == false || Add_Category == null ||
                    _hasUserInteractedWithDescription == false || Add_ShortDescription == null || Add_ShortDescription == string.Empty ||
                    _hasUserInteractedWithManufacturer == false || Add_Manufacturer == null || Add_Manufacturer == string.Empty ||
                    _hasUserInteractedWithName == false || Add_ComponentName == null || Add_ComponentName == string.Empty
                    )
                {
                    Add_CanAdd = false;
                }
                else
                {
                    Add_CanAdd = !HasErrors;
                }
            }
            Evaluate_AddTabVisibilty();
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "Name field cannot be empty")]
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyNameClipboardCommand))]
        private string _add_ComponentName = string.Empty;

        partial void OnAdd_ComponentNameChanged(string? oldValue, string newValue)
        {
            _hasUserInteractedWithName = true;
            if(newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_ComponentName));
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyManufacturerClipboardCommand))]
        [Required(ErrorMessage = "Manufacturer fiend cannot be empty")]
        private string _add_Manufacturer;

        partial void OnAdd_ManufacturerChanged(string? oldValue, string newValue)
        {
            _hasUserInteractedWithManufacturer = true;
            if (newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_Manufacturer));
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [Required(ErrorMessage = "Description cannot cannot be empty")]
        private string _add_ShortDescription;

        partial void OnAdd_ShortDescriptionChanged(string? oldValue, string newValue)
        {
            _hasUserInteractedWithDescription = true;
            if (newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_ShortDescription));
            }
        }

        [ObservableProperty]
        //[NotifyDataErrorInfo]
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyCategoryClipboardCommand))]
        [Required(ErrorMessage = "You have to select category")]
        private object _add_Category;

        partial void OnAdd_CategoryChanged(object? oldValue, object newValue)
        {
            _hasUserInteractedWithCategory = true;
            if (newValue != null)
            {
                ValidateProperty(newValue, nameof(Add_Category));
            }
        }

        [ObservableProperty]
        private string _add_FullDescription;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddTab_CopyDatasheetClipboardCommand))]
        //[NotifyDataErrorInfo]
        [CustomValidation(typeof(ComponentsPageViewModel), nameof(ValidateDatasheetLink), ErrorMessage = "Link is invalid")]
        private string _add_DatasheetLink;

        partial void OnAdd_DatasheetLinkChanged(string? oldValue, string newValue)
        {
            ValidateLinkAsync();
        }
        private CancellationTokenSource _cts;
        private async void ValidateLinkAsync()
        {
            // Cancel any ongoing validation
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                // Debounce: Wait for a short period before starting validation
                await Task.Delay(300, token);

                // Perform asynchronous validation
                ValidationResult validationResult = await LinkValidator.ValidateDatasheetLinkAsync(Add_DatasheetLink, new ValidationContext(this));
                string errorMessage = validationResult != null ? validationResult.ErrorMessage : string.Empty;
                ValidateProperty(errorMessage, nameof(Add_DatasheetLink));
            }
            catch (TaskCanceledException)
            {
                // Ignore cancellations caused by rapid input changes
            }
        }

        [RelayCommand]
        private void GoToPreview()
        {
            NavigateTab(ComponentTab.Preview);
        }

        [RelayCommand]
        private void GoToAddNew()
        {
            NavigateTab(ComponentTab.Add);
        }

        public static ValidationResult ValidateDatasheetLink(string name, ValidationContext context)
        {
            if(name == "")
            {
                return ValidationResult.Success;
            }
            else
            {
                return new(name);
            }
        }

        [RelayCommand]
        private void Add_ClearImage()
        {
            CurrentAddPredefinedImage = PredefinedImages[0].Image;
        }

        [RelayCommand]
        private void Add_ImageFromPredefined()
        {
            Console.WriteLine("XD");
        }

        [RelayCommand]
        private void Add_UploadImage()
        {
            Console.WriteLine("UploadImage");
        }

        [RelayCommand]
        private void Add_ClearComponent()
        {
            _hasUserInteractedWithCategory = false;
            _hasUserInteractedWithDescription = false;
            _hasUserInteractedWithManufacturer = false;
            _hasUserInteractedWithName = false;
            CurrentAddPredefinedImage = PredefinedImages[0].Image;
            Add_ComponentName = null;
            Add_Manufacturer = null;
            Add_Category = null;
            Add_FullDescription = null;
            Add_ShortDescription = null;
            Add_DatasheetLink = null;
            _hasUserInteractedWithCategory = false;
            _hasUserInteractedWithDescription = false;
            _hasUserInteractedWithManufacturer = false;
            _hasUserInteractedWithName = false;
            Add_CanAdd = false;
        }

        [RelayCommand]
        private async void Add_AddComponent()
        {
            if (DatabaseStore.ComponentStore.Components.FirstOrDefault(x => x.Name == Add_ComponentName) != null)
            {
                string buttonResult = await MsBoxService.DisplayMessageBox("Component with that name already exists!", Icon.Error);
                return;
            }
            // TODO: implement
            Add_CanAdd = false;

            try
            {
                string categoryName = Add_Category as string;

                Category cat = DatabaseStore.CategorieStore.Categories.FirstOrDefault(x=>x.Name == categoryName);

                string datasheet = Add_DatasheetLink == null ? string.Empty : Add_DatasheetLink;
                string longDesc = Add_FullDescription == null ? string.Empty : Add_FullDescription;

                Component newComponent = new Component(id: 0, cat.ID, category: cat, name: Add_ComponentName, manufacturer: Add_Manufacturer, shortDescription: Add_ShortDescription,
                    longDescription: longDesc, datasheetLink: datasheet, byteImage: ImageConverterUtility.BitmapToBytes(CurrentAddPredefinedImage));

                bool result = await DatabaseStore.ComponentStore.InsertNewComponent(newComponent);

                if(result == true)
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("Components added successfully! Do you want to add another component?", Icon.Question);

                    Add_ClearComponent();
                    if (dialogResult == "No")
                    {
                        NavigateTab(ComponentTab.Components);
                    }
                }
                else
                {
                    string dialogResult = await MsBoxService.DisplayMessageBox("There was an error while adding component. Try again or contact administrator!", Icon.Error);
                }
            }
            catch(Exception exception)
            {

            }
        }

        #endregion
        #region Preview tab
        [RelayCommand]
        private void Preview_LoadDatasheet()
        {
            // TODO: Waiting for WebScraper implementation for AllDatasheets.com
        }

        [RelayCommand]
        private void Preview_LoadFootprint()
        {
            // TODO: Waiting for WebScraper implementation for SnapEda.com
        }
        #endregion
        #region Modify tab
        [ObservableProperty]
        private bool _modify_CanModify;

        [ObservableProperty]
        private string _modify_ComponentName;

        partial void OnModify_ComponentNameChanged(string value)
        {
            if(value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentManufacturer;

        partial void OnModify_ComponentManufacturerChanged(string value)
        {
            if(value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentDatasheetLink;

        partial void OnModify_ComponentDatasheetLinkChanged(string value)
        {
            bool isValid = true; // TODO: Implement checking URL
            if(isValid == true)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentCategory;

        partial void OnModify_ComponentCategoryChanged(string value)
        {
            Modify_EvaluateForm();
        }

        [ObservableProperty]
        private string _modify_ComponentShortDescription;
        partial void OnModify_ComponentShortDescriptionChanged(string value)
        {
            if(value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }

        [ObservableProperty]
        private string _modify_ComponentFullDescription;
        partial void OnModify_ComponentFullDescriptionChanged(string value)
        {
            if (value != string.Empty)
            {
                Modify_EvaluateForm();
            }
        }
        
        [RelayCommand]
        private void Modify_Preview()
        {
            // TODO: waiting for webscraper implementation
        }
        
        [RelayCommand]
        private void Modify_Modify()
        {
            Console.WriteLine("XD");
        }

        [RelayCommand]
        public void NavigateToCollection()
        {
            NavigateTab(ComponentTab.Components);
        }

        [RelayCommand]
        private void Modify_Clear()
        {
            Modify_ClearDataToDefault();
        }

        private void Modify_EvaluateForm()
        {
            bool result = false;
            // TODO: include FullDescription and DatasheetLink in future
            if(Modify_ComponentName != SelectedComponent.Name || Modify_ComponentManufacturer != SelectedComponent.Manufacturer ||
               Modify_ComponentShortDescription != SelectedComponent.ShortDescription  || Modify_ComponentFullDescription != SelectedComponent.ShortDescription || 
               Modify_ComponentCategory != SelectedComponent.Category.Name)
            {
                result = true;
            }
            Modify_CanModify = result;
        }
        #endregion
        #endregion
        #region Observable properties methods

        [RelayCommand]
        private void Preview_CopyToClipboard()
        {
            ClipboardManager.SetText(SelectedComponent.DatasheetURL);
        }

        [RelayCommand]
        private void Preview_OpenDatasheetLink()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = SelectedComponent.DatasheetURL,
                UseShellExecute = true
            });
        }

        private void PrepareForPreview()
        {
            RefreshSelectedComponentsProjectSource();
            RefreshSelectedComponentsPurchasesSource();
            
            if(SelectedComponent.DatasheetURL == null || SelectedComponent.DatasheetURL == string.Empty)
            {
                Preview_CanPreview = false;
            }
            else
            {
                Preview_CanPreview = true;
            }
        }

        partial void OnSelectedComponentChanged(DetailedComponentContainer value)
        {
            if(value == null)
            {
                PreviewEnabled = false;
            }
            else if(PreviewEnabled == false)
            {
                PreviewEnabled = true;  
            }
        }

        [RelayCommand]
        public void ClearSelectedManufacturer()
        {
            SelectedManufacturer = null;
        }

        [RelayCommand]
        public void ClearSelectedCategory()
        {
            SelectedCategory = null;
        }

        [RelayCommand]
        public void ClearSearchBar()
        {
            SearchByNameOrDesc = null;
        }


        private void Modify_ClearDataToDefault()
        {
            Modify_ComponentName = SelectedComponent.Name;
            Modify_ComponentCategory = SelectedComponent.Category.Name;
            Modify_ComponentManufacturer = SelectedComponent.Manufacturer;
            Modify_ComponentShortDescription = SelectedComponent.ShortDescription;
            Modify_ComponentFullDescription = SelectedComponent.LongDescription;
            Modify_ComponentDatasheetLink = string.Empty; //TODO: fix!
        }
        partial void OnSearchByNameOrDescChanged(string value)
        {
            Console.WriteLine(value);
            //Components.Refresh();
        }

        partial void OnOnlyAvailableFlagChanged(bool value)
        {
            //Components.Refresh();
            Console.WriteLine(value);
        }

        partial void OnSelectedManufacturerChanged(string value)
        {
            //Components.Refresh();
            Console.WriteLine($"{value}");
            var user = DatabaseStore.UsersStore.LoggedInUser;
        }

        partial void OnSelectedCategoryChanged(string value)
        {
            //Components.Refresh();
            Console.WriteLine($"{value}");
        }

        [RelayCommand]
        public void ClearAllFiltersAndSorting()
        {
            SelectedCategory = null;
            SelectedManufacturer = null;
            OnlyAvailableFlag = false;
            SearchByNameOrDesc = string.Empty;
            //Components.Refresh();
            Console.WriteLine();
        }

        [ObservableProperty]
        private int _totalItems;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FirstPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(LastPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(NextPageCommand))]
        [NotifyCanExecuteChangedFor(nameof(PreviousPageCommand))]
        private int _currentPage;

        [ObservableProperty]
        private int _totalPages;

        private void PagingUpdate(IPageResponse response)
        {
            TotalItems = response.TotalSize;
            CurrentPage = response.Page;
            TotalPages = response.Pages;
        }

        #endregion
        private static Func<DetailedComponentContainer, bool> BuildFilter(string searchText)
        {
            if (string.IsNullOrEmpty(searchText)) return trade => true;
            return t => t.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        t.ShortDescription.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        t.LongDescription.Contains(searchText, StringComparison.InvariantCultureIgnoreCase);
        }

        private static Func<DetailedComponentContainer, bool> AvailableFilterPredicate(bool available)
        {
            if (available == false) return x => true;
            return x => x.AvailableAmount > 0;
        }

        private static Func<DetailedComponentContainer, bool> ManufacturerFilterPredicate(string manufacturer)
        {
            if (string.IsNullOrEmpty(manufacturer)) return trade => true;
            return t => t.Manufacturer.Contains(manufacturer, StringComparison.InvariantCultureIgnoreCase);
        }
        private static Func<DetailedComponentContainer, bool> CategoryFilterPredicate(string category)
        {
            if (string.IsNullOrEmpty(category)) return trade => true;
            return t => t.Category.Name.Contains(category, StringComparison.InvariantCultureIgnoreCase);
        }

        #region Constructor
        public ComponentsPageViewModel(RootPageViewModel defaultRootPageViewModel, DatabaseStore databaseStore, MessageBoxService msgBoxService) : base(defaultRootPageViewModel, databaseStore, msgBoxService)
        {
            _componentsService = new ComponentHolderService(this, DatabaseStore.ComponentStore);

            _pager = new BehaviorSubject<PageRequest>(new PageRequest(FirstPageIndex, SelectedPageSize));

            var nameFilter = this.WhenValueChanged(t => t.SearchByNameOrDesc)
                //.Throttle(TimeSpan.FromMilliseconds(250))
                .Select(BuildFilter);

            var availableFilter = this.WhenValueChanged(t => t.OnlyAvailableFlag)
                .Select(AvailableFilterPredicate);

            var manufacturerFilter = this.WhenValueChanged(t => t.SelectedManufacturer)
                .Select(ManufacturerFilterPredicate);

            var categoryFilter = this.WhenValueChanged(t => t.SelectedCategory)
                .Select(CategoryFilterPredicate);

            _componentsService.EmployeesConnection()
                .Filter(nameFilter)
                .Filter(availableFilter)
                .Filter(manufacturerFilter)
                .Filter(categoryFilter)
                .Sort(SortExpressionComparer<DetailedComponentContainer>.Ascending(e => e.ID))
                .Page(_pager)
                .Do(change => PagingUpdate(change.Response))
                .ObserveOn(Scheduler.CurrentThread) // Marshals to the current thread (often used for UI updates)
                .Bind(out _components)
                .Subscribe();

            _componentsService.LoadData();

            PredefinedImages = new ObservableCollection<ImageContainer>();

            SelectedComponentsPurchasesSource = new List<DetailedItemPurchaseContainer>();
            SelectedComponentsProjectSource = new List<DetailedItemProjectContainer>();
            PurchasesForSelected = new DataGridCollectionView(SelectedComponentsPurchasesSource);
            ProjectsForSelected = new DataGridCollectionView(SelectedComponentsProjectSource);

            DatabaseStore.PurchaseStore.Load();

            Manufacturers = new ObservableCollection<string>() { };
            DatabaseStore.ComponentStore.Load();
            DatabaseStore.ComponentStore.ComponentsLoaded += ComponentStore_ComponentsLoadedHandler; ;

            Categories = new ObservableCollection<string>() { };
            DatabaseStore.CategorieStore.Load();
            DatabaseStore.CategorieStore.CategoriesLoaded += HandleCategoriesLoaded;

            Suppliers = new ObservableCollection<SupplierContainer>();
            DatabaseStore.SupplierStore.SuppliersLoaded += SuppliersLoadedHandler;
            DatabaseStore.SupplierStore.Load();

            IEnumerable<PredefinedImage> imagesFromDB = DatabaseStore.PredefinedImagesStore.Images;
            foreach (PredefinedImage image in imagesFromDB)
            {
                PredefinedImages.Add(new ImageContainer(image));
            }
            CurrentAddPredefinedImage = PredefinedImages[0].Image;

            Evaluate_AddTabVisibilty();
        }

        private void ComponentStore_ComponentsLoadedHandler()
        {
            _componentsService.LoadData();
        }
        #endregion

        //private void CurrentChangedHandler(object? sender, EventArgs e)
        //{
        //    if(Components.CurrentItem is null)
        //    {
        //        // Unselected
        //        SelectedComponent = null;
        //    }
        //    else
        //    {
        //        SelectedComponent = Components.CurrentItem as DetailedComponentContainer;
        //    }
        //}

        private void SuppliersLoadedHandler()
        {
            Suppliers.Clear();
            foreach (Supplier supplier in DatabaseStore.SupplierStore.Suppliers)
            {
                Suppliers.Add(new SupplierContainer(supplier));
            }
        }

        private void HandleCategoriesLoaded()
        {
            Categories.Clear();
            IEnumerable<Category> categories = DatabaseStore.CategorieStore.Categories;
            foreach(Category category in categories)
            {
                Categories.Add(category.Name);
            }
        }

        private async void RefreshSelectedComponentsProjectSource()
        {
            // TODO: Implement sobe better and faster way with buffor. For now this will work
            // Request data for selected component
            IEnumerable<ProjectComponent> componentsPurchaseItem = await DatabaseStore.ProjectStore.ProjectComponentDP.GetAllProjectComponentsOfComponents(Components_SelectedComponent.Component);
            
            SelectedComponentsProjectSource.Clear();
            foreach(ProjectComponent component in componentsPurchaseItem)
            {
                Project proj = DatabaseStore.ProjectStore.Projects.FirstOrDefault(x=>x.ID == component.ProjectID);
                SelectedComponentsProjectSource.Add(new DetailedItemProjectContainer(proj, component));
            }
            ProjectsForSelected.Refresh();
        }

        private async void RefreshSelectedComponentsPurchasesSource()
        {
            // TODO: Implement sobe better and faster way with buffor. For now this will work
            // Request data for selected component
            IEnumerable<PurchaseItem> componentsPurchaseItems = await DatabaseStore.PurchaseStore.PurchaseItemDP.GetPurchaseItemsFromComponent(Components_SelectedComponent.Component);

            SelectedComponentsPurchasesSource.Clear();
            foreach (PurchaseItem purchaseItem in componentsPurchaseItems)
            {
                Purchase purchase = DatabaseStore.PurchaseStore.Purchases.FirstOrDefault(x => x.ID == purchaseItem.PurchaseID);
                Supplier supplier = DatabaseStore.SupplierStore.Suppliers.FirstOrDefault(x=>x.ID == purchase.SupplierID);
                SelectedComponentsPurchasesSource.Add(new DetailedItemPurchaseContainer(purchase, purchaseItem, supplier));
            }
            PurchasesForSelected.Refresh();
        }

        public void InterpreteNavigationParameter(NavParam navigationParameter)
        {
            switch (navigationParameter.Operation)
            {
                case NavOperation.Add:
                    NavigateTab(ComponentTab.Add);
                    break;
                case NavOperation.Preview:
                    break;
                default:
                    break;
            }
        }

        //public override void Dispose()
        //{
        //    DatabaseStore.CategorieStore.CategoriesLoaded -= HandleCategoriesLoaded;
        //    DatabaseStore.ComponentStore.ComponentsLoaded -= HandleComponentsLoaded;
        //    DatabaseStore.SupplierStore.SuppliersLoaded -= SuppliersLoadedHandler;
        //}
    }
}
