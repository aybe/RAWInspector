using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using JetBrains.Annotations;

namespace RAWInspector.Classes
{
    public sealed class ScrollViewerZoomBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
        }

        private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;

            var viewer = AssociatedObject;
            var delta = -e.Delta / Math.Abs(-e.Delta);

            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                Zoom(viewer, delta);
            }
            else
            {
                Scroll(viewer, delta);
            }
        }

        private static void Zoom([NotNull] ScrollViewer viewer, int add)
        {
            if (viewer == null)
                throw new ArgumentNullException(nameof(viewer));

            if (!(viewer.Content is FrameworkElement element))
                throw new ArgumentNullException(nameof(element));

            if (!(element.LayoutTransform is ScaleTransform transform))
                throw new ArgumentNullException(nameof(transform));

            var scaleX = Math.Max(1.0d, Math.Min(9999.0d, transform.ScaleX + -add));
            var scaleY = Math.Max(1.0d, Math.Min(9999.0d, transform.ScaleY + -add));

            transform.ScaleX = scaleX;
            transform.ScaleY = scaleY;
        }

        private void Scroll([NotNull] ScrollViewer viewer, double delta)
        {
            if (viewer == null)
                throw new ArgumentNullException(nameof(viewer));

            var h = Keyboard.Modifiers == ModifierKeys.Shift;
            var v = Keyboard.Modifiers == ModifierKeys.None;

            var extents = h ? viewer.ScrollableWidth : viewer.ScrollableHeight;
            var offset1 = h ? viewer.HorizontalOffset : viewer.VerticalOffset;
            var offset2 = Math.Max(0.0d, Math.Min(extents, offset1 + delta * 48));

            if (h)
            {
                viewer.ScrollToHorizontalOffset(offset2);
            }
            else if (v)
            {
                viewer.ScrollToVerticalOffset(offset2);
            }
        }
    }
}