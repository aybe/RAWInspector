using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    /// <summary>
    ///     The raw input data.
    /// </summary>
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputHeader
    {
        /// <summary>
        ///     The type of raw input.
        /// </summary>
        public readonly RawInputType Type;

        /// <summary>
        ///     The size, in bytes, of the entire input packet of data. This includes <see cref="RawInput" /> plus possible extra
        ///     input reports in the <see cref="RawHid" /> variable length array.
        /// </summary>
        public readonly uint Size;

        /// <summary>
        ///     A handle to the device generating the raw input data.
        /// </summary>
        public readonly IntPtr Device;

        /// <summary>
        ///     The value passed in the wParam parameter of the WM_INPUT message.
        /// </summary>
        public readonly uint wParam;
    }
}