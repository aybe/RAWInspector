using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RAWInspector.Controls
{
    public sealed class LegendTable : Panel
    {
        protected override void OnRender(DrawingContext dc)
        {
            if (InternalChildren.Count % 2 != 0)
                throw new InvalidOperationException("One or more label is missing content to form a pair.");

            base.OnRender(dc);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var width1 = 0.0d;
            var width2 = 0.0d;
            var height = 0.0d;
            for (var i1 = 0; i1 < InternalChildren.Count; i1 += 2)
            {
                var child1 = InternalChildren[i1];
                var child2 = InternalChildren[i1 + 1];

                child1.Measure(availableSize);
                child2.Measure(availableSize);
                var size1 = child1.DesiredSize;
                var size2 = child2.DesiredSize;
                width1 = Math.Max(width1, size1.Width);
                width2 = Math.Max(width2, size2.Width);
                height += Math.Max(size1.Height, size2.Height);
            }

            var size = new Size(width1 + width2, height);

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var width = 0.0d;

            for (var i = 0; i < InternalChildren.Count; i += 2)
            {
                var child = InternalChildren[i];
                width = Math.Max(child.DesiredSize.Width, width);
            }

            var y = 0.0d;

            for (var i = 0; i < InternalChildren.Count; i += 2)
            {
                var child1 = InternalChildren[i];
                var child2 = InternalChildren[i + 1];
                var height = Math.Max(child1.DesiredSize.Height, child2.DesiredSize.Height);
                child1.Arrange(new Rect(0.0d, y, width, height));
                child2.Arrange(new Rect(width, y, finalSize.Width - width, height));
                y += height;
            }

            var size = new Size(finalSize.Width, y);

            return size;
        }
    }
}