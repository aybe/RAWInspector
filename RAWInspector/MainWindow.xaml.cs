using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace RAWInspector
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Debugger.IsAttached)
            {
                Loaded += (sender, args) =>
                {
                    // var model = (Model) DataContext;
                    // model.UpdateStream(File.OpenRead(@"c:\testbig.bin"), false);
                };
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            (DataContext as IDisposable)?.Dispose();
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.None;

            var format = DataFormats.FileDrop;

            if (!e.Data.GetDataPresent(format))
                return;

            var data = e.Data.GetData(format);

            var paths = (string[]) data;
            if (paths == null)
                return;

            var path = paths.First();

            if (!File.Exists(path))
                return;

            e.Effects = DragDropEffects.All;
        }

        private void OnCloseRequest(object sender, EventArgs e)
        {
            Close();
        }
    }
}