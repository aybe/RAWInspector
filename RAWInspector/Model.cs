using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight;
using JetBrains.Annotations;
using RAWInspector.Utils;

namespace RAWInspector
{
    internal sealed class Model : ViewModelBase, IDisposable
    {
        public Model()
        {
            Commands = new ModelCommands(this);
            Data = new ModelData();

            ResetToDefaults();

            PropertyChanged += (sender, args) =>
            {
                var propertyName = args.PropertyName;

                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (propertyName)
                {
                    case nameof(BitmapWidth):
                    case nameof(BitmapOffset):
                    case nameof(BitmapZoom):
                        Debug.WriteLine($@"{nameof(PropertyChanged)}: {propertyName}");
                        UpdateBitmap();
                        Commands.UpdateData.Execute(null);
                        break;
                }
            };
        }

        #region IDisposable Members

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

        #endregion

        #region Private properties

        private FileStream Stream { get; set; }

        private List<string> Streams { get; } = new List<string>();

        private byte[] BitmapPixels { get; set; }

        #endregion

        #region Public properties

        private BitmapSource _bitmap;
        private int _bitmapHeight;
        private int _bitmapOffset;
        private string _bitmapTitle;
        private int _bitmapWidth;
        private int _bitmapZoom;
        private ModelData _data;

        public BitmapSource Bitmap
        {
            get => _bitmap;
            private set => Set(ref _bitmap, value);
        }

        public int BitmapHeight
        {
            get => _bitmapHeight;
            private set => Set(ref _bitmapHeight, value);
        }

        public int BitmapOffset
        {
            get => _bitmapOffset;
            set => Set(ref _bitmapOffset, value);
        }

        public string BitmapTitle
        {
            get => _bitmapTitle;
            set => Set(ref _bitmapTitle, value);
        }

        public int BitmapWidth
        {
            get => _bitmapWidth;
            set => Set(ref _bitmapWidth, Math.Max(1, Math.Min(4096, value)));
        }

        public int BitmapZoom
        {
            get => _bitmapZoom;
            set => Set(ref _bitmapZoom, Math.Max(1, Math.Min(100, value)));
        }

        public ModelCommands Commands { get; }

        public ModelData Data
        {
            get => _data;
            set => Set(ref _data, value);
        }

        public IWindowService WindowService { get; set; }

        #endregion

        #region Events

        public event EventHandler Close;

        public void OnClose()
        {
            Close?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        public void ResetToDefaults()
        {
            BitmapOffset = 0;
            BitmapWidth = 128;
            BitmapZoom = 4;
        }

        private void UpdateBitmap()
        {
            var stream = Stream;
            if (stream == null)
                return;

            var offset = BitmapOffset;
            var bytesBefore = offset < 0 ? Math.Abs(offset) : 0;
            var bytesStream = Math.Max(0, stream.Length - Math.Max(0, offset));
            var bytes = bytesBefore + bytesStream;

            var format = PixelFormats.Indexed8;
            var width = BitmapWidth;
            var stride = width * ((format.BitsPerPixel + 7) / 8);
            var height = (int) Math.Max(1, Math.Min(4096, bytes / stride + (bytes % stride == 0 ? 0 : 1)));
            var pixels = new byte[stride * height];
            var pattern = Pattern.DeadBeef;

            for (var i = 0; i < Math.Min(pixels.Length - 1, -offset); i++)
                pixels[i] = pattern[i % pattern.Count];

            stream.Position = Math.Max(0, offset);

            var read = stream.Read(pixels, bytesBefore, (int) Math.Min(pixels.Length - bytesBefore, bytesStream));

            for (var i = 0; i < pixels.Length - bytes; i++)
                pixels[bytesBefore + read + i] = pattern[i % pattern.Count];

            Bitmap = BitmapSource.Create(width, height, 96, 96, format, BitmapPalettes.Halftone256, pixels, stride);
            BitmapHeight = height;
            BitmapPixels = pixels;
        }

        public void UpdateHoverInfo(int x, int y)
        {
            Data.Update(x, y, BitmapWidth, BitmapHeight, BitmapOffset, BitmapPixels, Stream);
        }

        public void UpdateStream([NotNull] FileStream stream, bool temporary)
        {
            Stream?.Dispose();
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            BitmapTitle = $"{Path.GetFileName(Stream.Name)} ({Path.GetDirectoryName(Stream.Name)})";

            if (temporary)
                Streams.Add(Stream.Name);

            UpdateBitmap();
        }

        #endregion
    }
}