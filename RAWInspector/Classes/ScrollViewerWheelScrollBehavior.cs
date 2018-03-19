using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JetBrains.Annotations;

namespace RAWInspector.Classes
{
    public sealed class ScrollViewerWheelScrollBehavior : ScrollViewerWheelBehavior
    {
        public static readonly DependencyProperty HorizontalModifierProperty = DependencyProperty.Register(
            nameof(HorizontalModifier), typeof(ModifierKeys), typeof(ScrollViewerWheelScrollBehavior),
            new PropertyMetadata(ModifierKeys.Shift));

        public static readonly DependencyProperty VerticalModifierProperty = DependencyProperty.Register(
            nameof(VerticalModifier), typeof(ModifierKeys), typeof(ScrollViewerWheelScrollBehavior),
            new PropertyMetadata(ModifierKeys.None));

        [PublicAPI]
        public ModifierKeys HorizontalModifier
        {
            get => (ModifierKeys) GetValue(HorizontalModifierProperty);
            set => SetValue(HorizontalModifierProperty, value);
        }

        [PublicAPI]
        public ModifierKeys VerticalModifier
        {
            get => (ModifierKeys) GetValue(VerticalModifierProperty);
            set => SetValue(VerticalModifierProperty, value);
        }

        private bool CanHorizontal => Keyboard.Modifiers == HorizontalModifier;

        private bool CanVertical => Keyboard.Modifiers == VerticalModifier;

        protected override bool CanExecute()
        {
            return CanHorizontal || CanVertical;
        }

        protected override void Execute(ScrollViewer viewer, int delta)
        {
            if (viewer == null)
                throw new ArgumentNullException(nameof(viewer));

            var h = CanHorizontal;
            var v = CanVertical;

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