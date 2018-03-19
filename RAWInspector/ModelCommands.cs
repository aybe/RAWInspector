using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal sealed class ModelCommands
    {
        public ModelCommands([NotNull] Model model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));

            Close = new ModelCommand<EventArgs>(CloseExecute);

            Drop = new ModelCommand<DragEventArgs>(DropExecute);

        }

        private Model Model { get; }

        public ModelCommand<EventArgs> Close { get; }

        public ModelCommand<DragEventArgs> Drop { get; }

        private void CloseExecute(EventArgs e)
        {
            Model.OnCloseRequest();
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        private void DropExecute([NotNull] DragEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var paths = (string[]) e.Data.GetData(DataFormats.FileDrop);
            var path = (paths ?? throw new InvalidOperationException()).First();

            if (Storage.TryOpenFile(path, out var stream))
            {
                Model. UpdateStream(stream);
                return;
            }

            if (Storage.TryCopyFileWizard(path, out stream))
            {
                Model.UpdateStream(stream.Name);
                Model.UpdateStream(stream);
            }
        }  }
}