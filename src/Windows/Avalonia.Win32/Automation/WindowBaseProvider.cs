using System;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Controls.Automation.Peers;
using Avalonia.Win32.Interop.Automation;

#nullable enable

namespace Avalonia.Win32.Automation
{
    internal class WindowBaseProvider : AutomationProvider, IRawElementProviderFragmentRoot
    {
        private AutomationProvider? _focus;

        protected WindowBaseProvider(WindowBaseAutomationPeer peer)
            : base(peer)
        {
            Owner = (WindowImpl)((WindowBase)peer.Owner).PlatformImpl;
        }

        public override IRawElementProviderFragmentRoot? FragmentRoot => this; 
        public WindowImpl Owner { get; }
        
        public IRawElementProviderFragment? ElementProviderFromPoint(double x, double y)
        {
            var p = Owner.PointToClient(new PixelPoint((int)x, (int)y));
            var peer = (WindowBaseAutomationPeer)Peer;
            var found = InvokeSync(() => peer.GetPeerFromPoint(p));
            return found?.PlatformImpl as IRawElementProviderFragment;
        }

        public IRawElementProviderFragment? GetFocus() => _focus;

        public override IRawElementProviderSimple? HostRawElementProvider
        {
            get
            {
                if (Owner.Handle.Handle == IntPtr.Zero)
                    return null;
                var hr = UiaCoreProviderApi.UiaHostProviderFromHwnd(Owner.Handle.Handle, out var result);
                Marshal.ThrowExceptionForHR(hr);
                return result;
            }
        }

        protected override void UpdateCore(bool notify)
        {
            base.UpdateCore(notify);

            var peer = (WindowBaseAutomationPeer)Peer;
            var oldFocus = _focus?.Peer;
            var newFocus = peer.GetFocus();

            if (newFocus != oldFocus)
            {
                var oldProvider = oldFocus?.PlatformImpl as AutomationProvider;
                var newProvider = newFocus?.PlatformImpl as AutomationProvider;
                oldProvider?.UpdateFocus(false);
                newProvider?.UpdateFocus(false);
                _focus = newProvider;

                if (notify)
                {
                    UiaCoreProviderApi.UiaRaiseAutomationEvent(
                        newProvider,
                        (int)UiaEventId.AutomationFocusChanged);
                }
            }
        }
    }
}
