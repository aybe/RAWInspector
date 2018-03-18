using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal sealed class Model : ViewModelBase, IDisposable
    {
        public Model()
        {
            Drop = new RelayCommand<DragEventArgs>(DropExecute);
        }

        public void Dispose()
        {
            Stream?.Dispose();

            foreach (var stream in Streams)
            {
                try
                {
                    File.Delete(stream);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        #region Properties

        private FileStream Stream { get; set; }

        private List<string> Streams { get; } = new List<string>();

        public string Title
        {
            get
            {
                if (Stream == null)
                    return "No file loaded";

                var path = Stream.Name;
                var fileName = Path.GetFileName(path);
                var directoryName = Path.GetDirectoryName(path);
                var title = $"{fileName} ({directoryName})";

                return title;
            }
        }

        #endregion

        #region Commands

        public RelayCommand<DragEventArgs> Drop { get; }

        private void DropExecute([NotNull] DragEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            var paths = (string[]) e.Data.GetData(DataFormats.FileDrop);
            var path = (paths ?? throw new InvalidOperationException()).First();

            if (!TryOpenFile(path, out var stream) && !TryCopyFileWizard(path, out stream))
                return;

            Stream?.Dispose();
            Stream = stream;
            RaisePropertyChanged(() => Stream);
            RaisePropertyChanged(() => Title);
        }

        #endregion

        #region File helpers

        private static bool TryOpenFile([NotNull] string path, out FileStream result)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            result = null;

            try
            {
                result = File.OpenRead(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private static bool TryCopyFile([NotNull] string path, out string result)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            result = null;

            try
            {
                var tempFileName = Path.GetTempFileName();
                File.Copy(path, tempFileName, true);
                result = tempFileName;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool TryCopyFileWizard([NotNull] string path, out FileStream result)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            result = null;

            const string boxText = "Do you want to copy it to a temporary location and open it ?";
            const string boxCapt = "File could not be opened";

            if (MessageBox.Show(boxText, boxCapt, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return false;

            if (!TryCopyFile(path, out var copy))
            {
                MessageBox.Show("Couldn't copy file to temporary location.", "Error", MessageBoxButton.OK);
                return false;
            }

            Streams.Add(copy);

            if (!TryOpenFile(copy, out result))
            {
                MessageBox.Show("Couldn't open file.", "Error", MessageBoxButton.OK);
                return false;
            }

            return true;
        }

        #endregion
    }
}