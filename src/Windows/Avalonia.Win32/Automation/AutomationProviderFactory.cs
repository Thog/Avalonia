using System;
using Avalonia.Controls.Automation.Peers;
using Avalonia.Threading;

#nullable enable

namespace Avalonia.Win32.Automation
{
    internal static class AutomationProviderFactory
    {
        public static AutomationProvider Create(AutomationPeer peer)
        {
            Dispatcher.UIThread.VerifyAccess();

            var result = peer switch
            {
                WindowAutomationPeer window => new WindowProvider(window),
                PopupRootAutomationPeer popup => new PopupRootProvider(popup),
                ControlAutomationPeer control => new AutomationProvider(control),
                _ => throw new NotSupportedException("Unexpected AutomationPeer type."),
            };

            var _ = result.Update(notify: false);
            return result;
        }
    }
}
