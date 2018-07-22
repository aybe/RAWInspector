using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.Win32;
using RAWInspector.Controls;
using RAWInspector.Utils;

namespace RAWInspector
{
    internal sealed class ModelCommands
    {
        public ModelCommands([NotNull] Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            Close = new ModelCommand<EventArgs>(s => { Model.WindowService?.Quit(); });

            Drop = new ModelCommand<DragEventArgs>(e =>
            {
                if (e == null)
                    throw new ArgumentNullException(nameof(e));

                var paths = (string[]) e.Data.GetData(DataFormats.FileDrop);
                var path = (paths ?? throw new InvalidOperationException()).First();

                TryOpenFile(path);
            });

            Help = new ModelCommand(() => { Model.WindowService?.ShowHelp(); });

            Open = new ModelCommand(() =>
            {
                var dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                    TryOpenFile(dialog.FileName);
            });

            SetBitmapOffset = new ModelCommand<int>(s =>
            {
                int i;

                switch (s)
                {
                    case int.MinValue:
                        i = -Model.BitmapWidth;
                        break;
                    case int.MaxValue:
                        i = +Model.BitmapWidth;
                        break;
                    default:
                        i = s;
                        break;
                }

                Model.BitmapOffset += i;
            });

            SetBitmapWidth = new ModelCommand<int>(s => { Model.BitmapWidth += s; });

            SetBitmapZoom = new ModelCommand<int>(s => { Model.BitmapZoom += s; });

            ResetToDefaults = new ModelCommand(() => { Model.ResetToDefaults(); });

            UpdateData = new ModelCommand(() =>
            {
                if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
                    return;

                if (Mouse.DirectlyOver is Image image && image.Tag as string == BitmapControl.ControlKey)
                {
                    var position = Mouse.GetPosition(image);
                    UpdateHoverInfo(position);
                }
                else
                {
                    Model.Data.Reset();
                }
            });

            SetZeroPosition = new ModelCommand<Point>(s =>
            {
                UpdateHoverInfo(s);

                var offset = Model.Data.OffsetFile;
                if (offset == null)
                    throw new ArgumentNullException(nameof(offset));

                Model.BitmapOffset = offset.Value;
                Model.ScrollInfo.SetVerticalOffset(0.0d);
            });
        }

        private Model Model { get; }

        public ModelCommand<EventArgs> Close { get; }

        public ModelCommand<DragEventArgs> Drop { get; }

        public ModelCommand Help { get; }

        public ModelCommand Open { get; }

        public ModelCommand ResetToDefaults { get; set; }

        public ModelCommand<int> SetBitmapOffset { get; set; }

        public ModelCommand<int> SetBitmapWidth { get; }

        public ModelCommand<int> SetBitmapZoom { get; }

        public ModelCommand<Point> SetZeroPosition { get; }

        public ModelCommand UpdateData { get; }

        private void TryOpenFile([NotNull] string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            Storage.TryOpenFile(path, out var stream, out var temporary);

            if (stream != null)
                Model.UpdateStream(stream, temporary);
        }

        private void UpdateHoverInfo(Point point)
        {
            var x = (int) Math.Floor(point.X);
            var y = (int) Math.Floor(point.Y);
            Model.UpdateHoverInfo(x, y);
        }
    }
}