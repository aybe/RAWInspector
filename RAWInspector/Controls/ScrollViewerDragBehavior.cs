using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using JetBrains.Annotations;
using RAWInspector.Interop.Mouse;

namespace RAWInspector.Controls
{
    public sealed class ScrollViewerDragBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty InvertMouseWrapProperty =
            DependencyProperty.Register(
                nameof(InvertMouseWrap),
                typeof(bool),
                typeof(ScrollViewerDragBehavior),
                new PropertyMetadata(false)
            );

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register(
                nameof(IsEnabled),
                typeof(bool),
                typeof(ScrollViewerDragBehavior),
                new PropertyMetadata(true)
            );

        public static readonly DependencyProperty ButtonProperty =
            DependencyProperty.Register(
                nameof(Button),
                typeof(MouseButton),
                typeof(ScrollViewerDragBehavior),
                new PropertyMetadata(MouseButton.Middle)
            );

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

        [PublicAPI]
        public MouseButton Button
        {
            get => (MouseButton) GetValue(ButtonProperty);
            set => SetValue(ButtonProperty, value);
        }

        private bool IsEnabledInternal { get; set; }

        private bool IsSubscribed { get; set; }

        private Window Window { get; set; }


        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseDown += AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseUp += AssociatedObject_PreviewMouseUp;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseDown -= AssociatedObject_PreviewMouseDown;
            AssociatedObject.PreviewMouseUp -= AssociatedObject_PreviewMouseUp;

            Unsubscribe();
        }

        private void AssociatedObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Subscribe();
            UpdateEnabledState(e);
        }

        private void AssociatedObject_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            UpdateEnabledState(e);
        }

        private void Subscribe()
        {
            if (IsSubscribed)
                return;

            Window = Window.GetWindow(AssociatedObject);

            if (Window == null)
                throw new ArgumentNullException(nameof(Window));

            RawMouseHelper.Subscribe(Window, WndProc);

            IsSubscribed = true;
        }

        private void Unsubscribe()
        {
            RawMouseHelper.Unsubscribe(Window, WndProc);
        }

        private void UpdateEnabledState(MouseEventArgs e)
        {
            MouseButtonState button;

            switch (Button)
            {
                case MouseButton.Left:
                    button = e.LeftButton;
                    break;
                case MouseButton.Middle:
                    button = e.MiddleButton;
                    break;
                case MouseButton.Right:
                    button = e.RightButton;
                    break;
                case MouseButton.XButton1:
                    button = e.XButton1;
                    break;
                case MouseButton.XButton2:
                    button = e.XButton2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IsEnabledInternal = button == MouseButtonState.Pressed;

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

            if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
                direction *= 2.0d;

            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
                direction *= 2.0d;

            if ((Keyboard.Modifiers & ModifierKeys.Alt) != ModifierKeys.None)
                direction *= 2.0d;

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