using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal sealed class Model : ViewModelBase, IDisposable
    {
        public Model()
        {
            Commands = new ModelCommands(this);
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

        public WriteableBitmap Bitmap { get; private set; }

        public ModelCommands Commands { get; }

        public string Title
        {
            get
            {
                if (Stream == null)
                    return null;

                var path = Stream.Name;
                var fileName = Path.GetFileName(path);
                var directoryName = Path.GetDirectoryName(path);
                var title = $"{fileName} ({directoryName})";

                return title;
            }
        }

        #endregion

        #region Events

        public event EventHandler CloseRequest;

        public void OnCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Methods
        
        public void UpdateStream([NotNull] FileStream stream)
        {
            Stream?.Dispose();
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            RaisePropertyChanged(() => Stream);
            RaisePropertyChanged(() => Title);
        }

        public void UpdateStream([NotNull] string path)
        {
            if (path == null) 
                throw new ArgumentNullException(nameof(path));

            Streams.Add(path);
        }

        #endregion
    }
}