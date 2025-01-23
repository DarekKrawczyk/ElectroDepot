using Avalonia.Controls;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsBox.Avalonia.Enums;

namespace DesktopClient.Services
{
    public class MessageBoxService
    {
        private Window _window;
        public bool CanDisplay
        {
            get
            {
                return _window != null;
            }
        }
        public MessageBoxService()
        {

        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private MessageBoxCustomParams GetDefaultParameter(string message, Icon icon)
        {
            MessageBoxCustomParams defaultParams = new MessageBoxCustomParams
            {
                ButtonDefinitions = new List<ButtonDefinition>{
                                        new ButtonDefinition { Name = "Yes", },
                                        new ButtonDefinition { Name = "No", },
                    },
                ContentTitle = "ElectroDepot",
                ContentMessage = message,
                Icon = icon,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = false,
                MaxWidth = 500,
                MaxHeight = 800,
                SizeToContent = SizeToContent.WidthAndHeight,
                ShowInCenter = true,
                Topmost = true,
            };
            return defaultParams;
        }

        public async Task<string> DisplayMessageBox(string message, Icon icon)
        {
            var box = MessageBoxManager.GetMessageBoxCustom(GetDefaultParameter(message, icon));

            string result = await box.ShowWindowDialogAsync(_window);
            return result;
        }
    }
}
