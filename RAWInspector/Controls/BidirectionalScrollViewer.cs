using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RAWInspector.Controls
{
    internal sealed class BidirectionalScrollViewer : ScrollViewer
    {
        public new IScrollInfo ScrollInfo => base.ScrollInfo;

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled || ScrollInfo == null)
                return;

            var hScroll = Keyboard.Modifiers == ModifierKeys.None;
            var vScroll = Keyboard.Modifiers == ModifierKeys.Shift;

            if (!hScroll && !vScroll)
                return;

            var forward = e.Delta > 0;

            if (hScroll)
            {
                if (forward)
                    ScrollInfo.MouseWheelUp();
                else
                    ScrollInfo.MouseWheelDown();
            }
            else
            {
                if (forward)
                    ScrollInfo.MouseWheelLeft();
                else
                    ScrollInfo.MouseWheelRight();
            }

            e.Handled = true;
        }
    }
}