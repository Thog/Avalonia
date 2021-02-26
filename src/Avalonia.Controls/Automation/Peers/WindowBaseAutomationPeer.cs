using System.ComponentModel;
using Avalonia.Input;
using Avalonia.VisualTree;

#nullable enable

namespace Avalonia.Controls.Automation.Peers
{
    public class WindowBaseAutomationPeer : ControlAutomationPeer
    {
        private Control? _focus;

        public WindowBaseAutomationPeer(WindowBase owner)
            : base(owner, AutomationRole.Window)
        {
        }

        public AutomationPeer? GetFocus() => _focus is object ? GetOrCreatePeer(_focus) : null;

        public AutomationPeer? GetPeerFromPoint(Point p)
        {
            return Owner.GetVisualAt(p)?
                .FindAncestorOfType<Control>(includeSelf: true) is Control c ?
                    GetOrCreatePeer(c) : null;
        }

        protected override string GetNameCore() => Owner.GetValue(Window.TitleProperty);
        protected override AutomationPeer? GetParentCore() => null;

        protected void StartTrackingFocus()
        {
            KeyboardDevice.Instance.PropertyChanged += KeyboardDevicePropertyChanged;
            OnFocusChanged(KeyboardDevice.Instance.FocusedElement);
        }

        protected void StopTrackingFocus()
        {
            KeyboardDevice.Instance.PropertyChanged -= KeyboardDevicePropertyChanged;
        }

        private void OnFocusChanged(IInputElement? focus)
        {
            var oldFocus = _focus;
            _focus = focus?.VisualRoot == Owner ? focus as Control : null;
            if (_focus != oldFocus)
                InvalidateProperties();
        }

        private void KeyboardDevicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(KeyboardDevice.FocusedElement))
            {
                OnFocusChanged(KeyboardDevice.Instance.FocusedElement);
            }
        }
    }
}


