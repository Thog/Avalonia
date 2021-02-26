using Avalonia.Controls.Automation.Peers;

#nullable enable

namespace Avalonia.Win32.Automation
{
    internal class PopupRootProvider : WindowBaseProvider
    {
        public PopupRootProvider(PopupRootAutomationPeer peer)
            : base(peer)
        {
        }
    }
}
