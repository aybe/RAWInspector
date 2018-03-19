using System;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [Flags]
    public enum RawMouseFlags : ushort
    {
        /// <summary>
        ///     Mouse movement data is relative to the last mouse position.
        /// </summary>
        Relative = 0,

        /// <summary>
        ///     Mouse movement data is based on absolute position.
        /// </summary>
        Absolute = 1,

        /// <summary>
        ///     Mouse coordinates are mapped to the virtual desktop (for a multiple monitor system).
        /// </summary>
        VirtualDesktop = 2,

        /// <summary>
        ///     Mouse attributes changed; application needs to query the mouse attributes.
        /// </summary>
        AttributesChanged = 4
    }
}