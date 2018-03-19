using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDevice
    {
        public readonly ushort UsagePage;
        public readonly ushort Usage;
        public readonly uint Flags;
        public readonly IntPtr HwndTarget;

        public RawInputDevice(ushort usagePage, ushort usage, uint flags, IntPtr hwndTarget)
        {
            UsagePage = usagePage;
            Usage = usage;
            Flags = flags;
            HwndTarget = hwndTarget;
        }
    }
}