using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using JetBrains.Annotations;

namespace RAWInspector.Classes
{
    public sealed class ScrollViewerWheelZoomBehavior : ScrollViewerWheelBehavior
    {
        public static readonly DependencyProperty ZoomingModifierProperty = DependencyProperty.Register(
            nameof(ZoomingModifier), typeof(ModifierKeys), typeof(ScrollViewerWheelZoomBehavior),
            new PropertyMetadata(ModifierKeys.Control));

        [PublicAPI]
        public ModifierKeys ZoomingModifier
        {
            get => (ModifierKeys) GetValue(ZoomingModifierProperty);
            set => SetValue(ZoomingModifierProperty, value);
        }

        protected override bool CanExecute()
        {
            return Keyboard.Modifiers == ZoomingModifier;
        }

        protected override void Execute(ScrollViewer viewer, int delta)
        {
            if (viewer == null)
                throw new ArgumentNullException(nameof(viewer));

            if (!(viewer.Content is FrameworkElement element))
                throw new ArgumentNullException(nameof(element));

            if (!(element.LayoutTransform is ScaleTransform transform))
                throw new ArgumentNullException(nameof(transform));

            var scaleX = Math.Max(1.0d, Math.Min(9999.0d, transform.ScaleX - delta));
            var scaleY = Math.Max(1.0d, Math.Min(9999.0d, transform.ScaleY - delta));

            transform.ScaleX = scaleX;
            transform.ScaleY = scaleY;
        }
    }
}