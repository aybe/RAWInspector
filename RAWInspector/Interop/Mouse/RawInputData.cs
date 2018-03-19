using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [StructLayout(LayoutKind.Explicit)]
    public struct RawInputData
    {
        [FieldOffset(0)] public RawMouse Mouse;

        [FieldOffset(0)] public RawKeyboard Keyboard;

        [FieldOffset(0)] public RawHid HID;
    }
}