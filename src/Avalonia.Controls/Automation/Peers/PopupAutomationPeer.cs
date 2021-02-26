using System;
using System.Collections.Generic;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;

#nullable enable

namespace Avalonia.Controls.Automation.Peers
{
    public class PopupAutomationPeer : ControlAutomationPeer
    {
        private ControlAutomationPeer?[]? _children;

        public PopupAutomationPeer(Popup owner, AutomationRole role = AutomationRole.None)
            : base(owner, role) 
        {
            owner.Opened += OnOpenedClosed;
            owner.Closed += OnOpenedClosed;
        }

        protected override IReadOnlyList<AutomationPeer>? GetChildrenCore()
        {
            var owner = (Popup)Owner;

            if (owner.IsOpen && 
                ((IVisualTreeHost)owner).Root is Control popupRoot &&
                ((IVisual)owner).IsAttachedToVisualTree)
            {
                _children ??= new ControlAutomationPeer?[1];
                _children[0] = (ControlAutomationPeer)GetOrCreatePeer(popupRoot);
                return _children!;
            }

            return null;
        }

        private void OnOpenedClosed(object sender, EventArgs e)
        {
            ResetChildrenCache();
        }
    }
}
