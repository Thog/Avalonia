using System;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;

#nullable enable

namespace Avalonia.Controls.Automation.Peers
{
    public class PopupRootAutomationPeer : ControlAutomationPeer
    {
        public PopupRootAutomationPeer(PopupRoot owner, AutomationRole role = AutomationRole.Window)
            : base(owner, role) 
        {
            owner.Closed += OnClosed;
        }

        public AutomationPeer? GetPeerFromPoint(Point p)
        {
            return Owner.GetVisualAt(p)?
                .FindAncestorOfType<Control>(includeSelf: true) is Control c ?
                    GetOrCreatePeer(c) : null;
        }

        protected override AutomationPeer? GetParentCore()
        {
            if (Owner.GetLogicalParent() is Control parent)
            {
                return GetOrCreatePeer(parent);
            }

            return null;
        }

        private void OnClosed(object sender, EventArgs e)
        {
            ((PopupRoot)Owner).Closed -= OnClosed;
            InvalidatePlatformImpl();
        }
    }
}
