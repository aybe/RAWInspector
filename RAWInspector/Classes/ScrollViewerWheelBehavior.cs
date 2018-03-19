using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using JetBrains.Annotations;

namespace RAWInspector.Classes
{
    public abstract class ScrollViewerWheelBehavior : Behavior<ScrollViewer>
    {
        private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var viewer = AssociatedObject;
            var delta = -e.Delta / Math.Abs(-e.Delta);

            if (!CanExecute())
                return;

            Execute(viewer, delta);

            e.Handled = true;
        }

        protected override void OnAttached()
        {
            AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
        }

        protected abstract bool CanExecute();

        protected abstract void Execute([NotNull] ScrollViewer viewer, int delta);
    }
}