using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    /// <summary>
    ///     Contains information about the state of the mouse.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Explicit)]
    public struct RawMouse
    {
        /// <summary>
        ///     The mouse state.
        /// </summary>
        [FieldOffset(0)] public readonly RawMouseFlags Flags;

        // NOTE union stripped-out, struct inlined

        /// <summary>
        ///     The transition state of the mouse buttons.
        /// </summary>
        [FieldOffset(4)] public readonly RawMouseButtonFlags ButtonFlags;

        /// <summary>
        ///     If <see cref="ButtonFlags" /> is <see cref="RawMouseButtonFlags.Wheel" />, this member is a signed value that
        ///     specifies the wheel delta.
        /// </summary>
        [FieldOffset(6)] public readonly ushort ButtonData;

        /// <summary>
        ///     The raw state of the mouse buttons.
        /// </summary>
        [FieldOffset(8)] public readonly uint RawButtons;

        /// <summary>
        ///     The motion in the X direction. This is signed relative motion or absolute motion, depending on the value of
        ///     <see cref="Flags" />.
        /// </summary>
        [FieldOffset(12)] public readonly int LastX;

        /// <summary>
        ///     The motion in the Y direction. This is signed relative motion or absolute motion, depending on the value of
        ///     <see cref="Flags" />.
        /// </summary>
        [FieldOffset(16)] public readonly int LastY;

        /// <summary>
        ///     The device-specific additional information for the event.
        /// </summary>
        [FieldOffset(20)] public readonly uint ExtraInformation;
    }
}