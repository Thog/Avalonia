using System;

#nullable enable

namespace Avalonia.Controls.Automation.Peers
{
    public class WindowAutomationPeer : WindowBaseAutomationPeer
    {
        public WindowAutomationPeer(Window owner)
            : base(owner)
        {
            if (owner.IsVisible)
                StartTrackingFocus();
            else
                owner.Opened += OnOpened;
            owner.Closed += OnClosed;
        }

        private void OnOpened(object sender, EventArgs e)
        {
            ((Window)Owner).Opened -= OnOpened;
            StartTrackingFocus();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            ((Window)Owner).Closed -= OnClosed;
            StopTrackingFocus();
            InvalidatePlatformImpl();
        }
    }
}


