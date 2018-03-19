using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    [PublicAPI]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal static class NativeConstants
    {
        public const int RID_INPUT = 0x10000003;
        public const int RIDEV_NOLEGACY = 0x30;
        public const int WM_INPUT = 0xFF;
    }
}