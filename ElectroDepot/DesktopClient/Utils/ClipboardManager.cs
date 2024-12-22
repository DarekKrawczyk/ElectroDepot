using Avalonia;
using Avalonia.VisualTree;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;
using System.Threading.Tasks;

namespace DesktopClient.Utils
{
    internal class ClipboardManager
    {
        private static IClipboard Get()
        {
            //Desktop
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            {
                return window.Clipboard!;

            }
            //Android (and iOS?)
            else if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime { MainView: { } mainView })
            {
                var visualRoot = mainView.GetVisualRoot();
                if (visualRoot is TopLevel topLevel)
                {
                    return topLevel.Clipboard!;
                }
            }

            return null!;
        }

        public static async Task<bool> SetText(string text)
        {
            IClipboard clipboard = Get();
            if(clipboard != null)
            {
                await clipboard.SetTextAsync(text);
                return true;
            }
            return false;
        }
    }
}
