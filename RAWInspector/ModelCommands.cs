using System;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace RAWInspector
{
    internal sealed class ModelCommands
    {
        public ModelCommands([NotNull] Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            Close = new ModelCommand<EventArgs>(CloseExecute);

            Drop = new ModelCommand<DragEventArgs>(DropExecute);

            Open = new ModelCommand(OpenExecute);
        }

        private Model Model { get; }

        public ModelCommand<EventArgs> Close { get; }

        public ModelCommand<DragEventArgs> Drop { get; }

        public ModelCommand Open { get; }

        private void CloseExecute(EventArgs e)
        {
            Model.OnCloseRequest();
        }

        private void DropExecute([NotNull] DragEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var paths = (string[]) e.Data.GetData(DataFormats.FileDrop);
            var path = (paths ?? throw new InvalidOperationException()).First();

            OpenFile(path);
        }

        private void OpenExecute()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() != true)
                return;

            OpenFile(dialog.FileName);
        }

        private void OpenFile(string path)
        {
            Storage.TryOpenFile(path, out var stream, out var temporary);

            if (stream != null)
                Model.UpdateStream(stream, temporary);
        }
    }
}