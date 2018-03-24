using System.Windows.Input;
using JetBrains.Annotations;

namespace RAWInspector.Controls
{
    [PublicAPI]
    public class MouseWheelGesture : MouseGesture
    {
        public MouseWheelGesture() : base(MouseAction.WheelClick)
        {
        }

        public MouseWheelGesture(ModifierKeys modifiers) : base(MouseAction.WheelClick, modifiers)
        {
        }

        public MouseWheelDirection Direction { get; set; }

        public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
        {
            if (!base.Matches(targetElement, inputEventArgs))
                return false;

            if (!(inputEventArgs is MouseWheelEventArgs args))
                return false;

            var delta = args.Delta;

            switch (Direction)
            {
                case MouseWheelDirection.None:
                    return delta == 0;
                case MouseWheelDirection.Up:
                    return delta > 0;
                case MouseWheelDirection.Down:
                    return delta < 0;
                default:
                    return false;
            }
        }
    }
}