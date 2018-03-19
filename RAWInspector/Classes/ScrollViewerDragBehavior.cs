using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using JetBrains.Annotations;
using RAWInspector.Interop.Mouse;

namespace RAWInspector.Classes
{
    public sealed class ScrollViewerDragBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty InvertMouseWrapProperty =
            DependencyProperty.Register(nameof(InvertMouseWrap), typeof(bool), typeof(ScrollViewerDragBehavior),
                new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            nameof(IsEnabled), typeof(bool), typeof(ScrollViewerDragBehavior), new PropertyMetadata(default(bool)));

        [PublicAPI]
        public bool InvertMouseWrap
        {
            get => (bool) GetValue(InvertMouseWrapProperty);
            set => SetValue(InvertMouseWrapProperty, value);
        }

        [PublicAPI]
        public bool IsEnabled
        {
            get => (bool) GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        private bool IsEnabledInternal { get; set; }

        [NotNull] private Window Window => Window.GetWindow(AssociatedObject) ?? throw new ArgumentNullException();

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;

            Window.SourceInitialized += (sender, e) => RawMouseHelper.Subscribe((Window) sender, WndProc);
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;

            RawMouseHelper.Unsubscribe(Window, WndProc);
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            UpdateEnabledState(e);
        }

        private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UpdateEnabledState(e);
        }

        private void UpdateEnabledState(MouseEventArgs e)
        {
            IsEnabledInternal = e.MiddleButton == MouseButtonState.Pressed;

            Mouse.Capture(IsEnabledInternal ? AssociatedObject : null);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (!IsEnabled || !IsEnabledInternal)
                return IntPtr.Zero;

            if (!RawMouseHelper.TryGetRawInput(msg, lParam, out var input))
                return IntPtr.Zero;

            if (input.Header.Type != RawInputType.Mouse)
                return IntPtr.Zero;

            var mouse = input.Data.Mouse;
            if (mouse.Flags != RawMouseFlags.Relative)
                return IntPtr.Zero;

            handled = true;

            RawMouseHelper.Wrap(mouse);

            var viewer = AssociatedObject;
            var direction = InvertMouseWrap ? +1.0d : -1.0d;
            var x = mouse.LastX * direction + viewer.HorizontalOffset;
            var y = mouse.LastY * direction + viewer.VerticalOffset;
            var xMax = viewer.ScrollableWidth;
            var yMax = viewer.ScrollableHeight;
            var xOffset = Math.Max(0.0d, Math.Min(xMax, x));
            var yOffset = Math.Max(0.0d, Math.Min(yMax, y));

            viewer.ScrollToHorizontalOffset(xOffset);
            viewer.ScrollToVerticalOffset(yOffset);

            return IntPtr.Zero;
        }
    }
}