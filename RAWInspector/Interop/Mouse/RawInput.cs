using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    /// <summary>
    ///     Contains the raw input from a device.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInput
    {
        /// <summary>
        ///     The raw input data.
        /// </summary>
        public RawInputHeader Header;

        /// <summary>
        ///     <see cref="RawInputData.Mouse" /> if the data comes from a mouse, <see cref="RawInputData.Keyboard" /> if the data
        ///     comes from a keyboard, <see cref="RawInputData.HID" /> if the data comes from an HID.
        /// </summary>
        public RawInputData Data;
    }
}