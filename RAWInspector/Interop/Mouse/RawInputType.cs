using JetBrains.Annotations;

namespace RAWInspector.Interop.Mouse
{
    /// <summary>
    ///     The type of raw input.
    /// </summary>
    [PublicAPI]
    public enum RawInputType : uint
    {
        /// <summary>
        ///     Raw input comes from the mouse.
        /// </summary>
        Mouse = 0,

        /// <summary>
        ///     Raw input comes from the keyboard.
        /// </summary>
        Keyboard = 1,

        /// <summary>
        ///     Raw input comes from some device that is not a keyboard or a mouse.
        /// </summary>
        Hid = 2
    }
}