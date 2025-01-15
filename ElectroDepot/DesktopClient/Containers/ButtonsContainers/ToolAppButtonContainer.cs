using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectroDepotClassLibrary.Containers.NodeContainers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopClient.Containers.ButtonsContainers
{
    public partial class ToolAppButtonContainer : ObservableObject
    {
        private readonly ToolAppContainer _node;
        public ToolAppContainer Node
        {
            get
            {
                return _node;
            }
        }
        public ToolAppButtonContainer(ToolAppContainer node)
        {
            _node = node;
        }

        [RelayCommand]
        public void Execute()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = _node.ExecutionLink,
                UseShellExecute = true
            });
        }
    }
}
