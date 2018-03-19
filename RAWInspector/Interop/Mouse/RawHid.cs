using System;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    public struct RawHid
    {
        public readonly uint SizeHid;
        public readonly uint Count;
        public readonly IntPtr RawData;
    }
}