using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboard
    {
        public readonly ushort MakeCode;
        public readonly ushort Flags;
        public readonly ushort Reserved;
        public readonly ushort VKey;
        public readonly uint Message;
        public readonly uint ExtraInformation;
    }
}