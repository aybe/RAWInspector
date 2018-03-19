using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    /// <summary>
    ///     Hooks to raw mouse input and provides wrapping.
    /// </summary>
    public static class RawMouseHelper
    {
        /// <summary>
        ///     Subscribe a window to process raw input: hooks to specified WNDPROC and registers raw keyboard/mouse.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="hook"></param>
        public static void Subscribe([NotNull] Window window, [NotNull] HwndSourceHook hook)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            if (hook == null)
                throw new ArgumentNullException(nameof(hook));

            var source = GetHwndSource(window);

            source.AddHook(hook);

            var devices = new[]
            {
                new RawInputDevice(0x01, 0x02, 0x00, source.Handle), // mouse
                new RawInputDevice(0x01, 0x06, 0x00, source.Handle) // keyboard
            };

            var size = (uint) Marshal.SizeOf<RawInputDevice>();

            if (!NativeMethods.RegisterRawInputDevices(devices, (uint) devices.Length, size))
                throw new Win32Exception("Could not register raw input devices.");
        }

        /// <summary>
        ///     Unsubscribe a window from processing raw input and unhooks from specified WNDPROC.
        /// </summary>
        /// <param name="window"></param>
        /// <param name="hook"></param>
        public static void Unsubscribe([NotNull] Window window, [NotNull] HwndSourceHook hook)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            if (hook == null)
                throw new ArgumentNullException(nameof(hook));

            var source = GetHwndSource(window);

            source.RemoveHook(hook);
        }

        /// <summary>
        ///     Tries to get raw input from a WM_INPUT message.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="lParam"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetRawInput(int msg, IntPtr lParam, out RawInput result)
        {
            result = default(RawInput);

            if (msg != NativeConstants.WM_INPUT)
                return false;

            const uint command = NativeConstants.RID_INPUT;

            var size = 0u;

            var sizeOf = (uint) Marshal.SizeOf<RawInputHeader>();
            if (NativeMethods.GetRawInputData(lParam, command, IntPtr.Zero, ref size, sizeOf) != 0)
                throw new InvalidOperationException("Could not get get raw input data.");

            var bytes = new byte[size];
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var pData = handle.AddrOfPinnedObject();
            if (NativeMethods.GetRawInputData(lParam, command, pData, ref size, sizeOf) == unchecked((uint) -1))
                throw new InvalidOperationException("Could not get get raw input data.");

            result = Marshal.PtrToStructure<RawInput>(pData);

            handle.Free();

            return true;
        }

        /// <summary>
        ///     Wraps mouse position at screen edges.
        /// </summary>
        /// <param name="mouse"></param>
        public static void Wrap(RawMouse mouse)
        {
            if (mouse.Flags != RawMouseFlags.Relative)
                return;

            var window = NativeMethods.GetDesktopWindow();

            if (!NativeMethods.GetClientRect(window, out var rect))
                throw new Win32Exception("Could not get client rect.");

            if (!NativeMethods.GetCursorPos(out var pos))
                throw new Win32Exception("Could not get cursor position.");

            var xMax = rect.Right - 1;
            var yMax = rect.Bottom - 1;
            var x1 = pos.X + mouse.LastX;
            var y1 = pos.Y + mouse.LastY;
            var x2 = x1 < 0 ? xMax : (x1 > xMax ? 0 : x1);
            var y2 = y1 < 0 ? yMax : (y1 > yMax ? 0 : y1);

            if (x2 == x1 && y2 == y1)
                return;

            if (!NativeMethods.SetCursorPos(x2, y2))
                throw new Win32Exception("Could not set cursor position.");
        }

        [NotNull]
        private static HwndSource GetHwndSource([NotNull] Window window)
        {
            if (window == null)
                throw new ArgumentNullException(nameof(window));

            var presentationSource = PresentationSource.FromVisual(window);
            if (presentationSource == null)
                throw new ArgumentNullException(nameof(presentationSource));

            if (!(presentationSource is HwndSource hwndSource))
                throw new ArgumentNullException(nameof(hwndSource));

            return hwndSource;
        }
    }
}