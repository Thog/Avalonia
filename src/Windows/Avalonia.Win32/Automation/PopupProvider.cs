using Avalonia.Controls.Automation.Peers;
using Avalonia.Controls.Primitives;
using Avalonia.Win32.Interop.Automation;

#nullable enable

namespace Avalonia.Win32.Automation
{
    internal class PopupProvider : AutomationProvider, IRawElementProviderFragmentRoot
    {
        public PopupProvider(
            AutomationPeer peer,
            UiaControlTypeId controlType,
            WindowImpl visualRoot,
            IRawElementProviderFragmentRoot parentWindow) 
            : base(peer, controlType, visualRoot, parentWindow)
        {
            var control = (PopupRoot)((ControlAutomationPeer)peer).Owner;
            Owner = (PopupImpl)control.PlatformImpl;
        }

        public PopupImpl Owner { get; }

        public override IRawElementProviderSimple HostRawElementProvider
        {
            get
            {
                var popup = (PopupRoot)((ControlAutomationPeer)Peer).Owner;
                var hwnd = popup.PlatformImpl.Handle.Handle;
                UiaCoreProviderApi.UiaHostProviderFromHwnd(hwnd, out var result);
                return result;
            }
        }

        public IRawElementProviderFragment? ElementProviderFromPoint(double x, double y)
        {
            var p = Owner.PointToClient(new PixelPoint((int)x, (int)y));
            var peer = (PopupRootAutomationPeer)Peer;
            var found = InvokeSync(() => peer.GetPeerFromPoint(p));
            return found?.PlatformImpl as IRawElementProviderFragment;
        }

        public IRawElementProviderFragment GetFocus() => FragmentRoot.GetFocus();
    }
}
