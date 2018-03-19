using System;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [Flags]
    public enum RawMouseButtonFlags : ushort
    {
        None = 0,

        /// <summary>
        ///     Left button changed to down.
        /// </summary>
        LeftButtonDown = 1,

        /// <summary>
        ///     Left button changed to up.
        /// </summary>
        LeftButtonUp = 2,

        /// <summary>
        ///     Middle button changed to down.
        /// </summary>
        MiddleButtonDown = 16,

        /// <summary>
        ///     Middle button changed to up.
        /// </summary>
        MiddleButtonUp = 32,

        /// <summary>
        ///     Right button changed to down.
        /// </summary>
        RightButtonDown = 4,

        /// <summary>
        ///     Right button changed to up.
        /// </summary>
        RightButtonUp = 8,

        /// <summary>
        ///     XBUTTON1 changed to down.
        /// </summary>
        Button4Down = 64,

        /// <summary>
        ///     XBUTTON1 changed to up.
        /// </summary>
        Button4Up = 128,

        /// <summary>
        ///     XBUTTON2 changed to down.
        /// </summary>
        Button5Down = 256,

        /// <summary>
        ///     XBUTTON2 changed to up.
        /// </summary>
        Button5Up = 512,

        /// <summary>
        ///     Raw input comes from a mouse wheel. The wheel delta is stored in <see cref="RawMouse.ButtonData" />.
        /// </summary>
        Wheel = 1024
    }
}