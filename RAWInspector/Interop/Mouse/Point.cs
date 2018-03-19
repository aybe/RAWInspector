using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct Point
    {
        public readonly int X;
        public readonly int Y;

        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}