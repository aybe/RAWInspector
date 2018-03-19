using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace RAWInspector.Interop.Mouse
{
    [SuppressMessage("ReSharper", "PartialTypeWithSinglePart")]
    internal static partial class NativeMethods
    {
        [DllImport("user32.dll", EntryPoint = "GetRawInputData")]
        public static extern uint GetRawInputData(
            [In] IntPtr hRawInput,
            uint uiCommand,
            IntPtr pData,
            ref uint pcbSize,
            uint cbSizeHeader
        );

        [DllImport("user32.dll", EntryPoint = "RegisterRawInputDevices", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterRawInputDevices(
            [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct, SizeParamIndex = 0)]
            RawInputDevice[] pRawInputDevices,
            uint uiNumDevices,
            uint cbSize
        );

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetClientRect", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect([In] IntPtr hWnd, [Out] out Rect lpRect);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos([Out] out Point lpPoint);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int x, int y);
    }
}