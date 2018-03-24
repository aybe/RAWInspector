using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal sealed class ModelData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Reset()
        {
            X = null;
            Y = null;
            OffsetImage = null;
            OffsetFile = null;

            Int8 = null;
            Int16BE = null;
            Int32BE = null;
            Int64BE = null;
            Int16LE = null;
            Int32LE = null;
            Int64LE = null;

            UInt8 = null;
            UInt16BE = null;
            UInt32BE = null;
            UInt64BE = null;
            UInt16LE = null;
            UInt32LE = null;
            UInt64LE = null;

            UInt8Hex = null;
            UInt16BEHex = null;
            UInt32BEHex = null;
            UInt64BEHex = null;
            UInt16LEHex = null;
            UInt32LEHex = null;
            UInt64LEHex = null;

            UpdateProperties();
        }

        public void Update(
            int x, int y, int width, int height, int offset, [NotNull] byte[] pixels, [CanBeNull] Stream stream)
        {
            if (pixels == null)
                throw new ArgumentNullException(nameof(pixels));

            if (x < 0 || x >= width || y < 0 || y >= height)
            {
                Reset();
                UpdateProperties();
                return;
            }

            var offsetImage = y * width + x;
            var offsetFile = offsetImage + offset;

            X = x;
            Y = y;
            OffsetImage = offsetImage;
            OffsetFile = stream == null || offsetFile < 0 || offsetFile >= stream.Length
                ? default(int?)
                : offsetFile;
            UpdateNumbers(pixels, offsetImage);
            UpdateProperties();
        }

        private void UpdateNumbers([NotNull] byte[] pixels, int offset)
        {
            if (pixels == null)
                throw new ArgumentNullException(nameof(pixels));

            if (offset < 0 || offset >= pixels.Length)
                throw new ArgumentOutOfRangeException(nameof(offset));

            Int8 = (sbyte) pixels[offset];
            UInt8 = pixels[offset];
            UInt8Hex = UInt8?.ToStringHex();

            var i16 = pixels.TryGetInt16(offset);
            var i32 = pixels.TryGetInt32(offset);
            var i64 = pixels.TryGetInt64(offset);
            Int16BE = i16?.ToEndian(Endianness.BigEndian);
            Int32BE = i32?.ToEndian(Endianness.BigEndian);
            Int64BE = i64?.ToEndian(Endianness.BigEndian);
            Int16LE = i16?.ToEndian(Endianness.LittleEndian);
            Int32LE = i32?.ToEndian(Endianness.LittleEndian);
            Int64LE = i64?.ToEndian(Endianness.LittleEndian);

            var u16 = pixels.TryGetUInt16(offset);
            var u32 = pixels.TryGetUInt32(offset);
            var u64 = pixels.TryGetUInt64(offset);
            UInt16BE = u16?.ToEndian(Endianness.BigEndian);
            UInt32BE = u32?.ToEndian(Endianness.BigEndian);
            UInt64BE = u64?.ToEndian(Endianness.BigEndian);
            UInt16LE = u16?.ToEndian(Endianness.LittleEndian);
            UInt32LE = u32?.ToEndian(Endianness.LittleEndian);
            UInt64LE = u64?.ToEndian(Endianness.LittleEndian);

            UInt16BEHex = UInt16BE?.ToStringHex();
            UInt32BEHex = UInt32BE?.ToStringHex();
            UInt64BEHex = UInt64BE?.ToStringHex();
            UInt16LEHex = UInt16LE?.ToStringHex();
            UInt32LEHex = UInt32LE?.ToStringHex();
            UInt64LEHex = UInt64LE?.ToStringHex();
        }

        private void UpdateProperties()
        {
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
            OnPropertyChanged(nameof(OffsetImage));
            OnPropertyChanged(nameof(OffsetFile));

            OnPropertyChanged(nameof(Int8));
            OnPropertyChanged(nameof(Int16BE));
            OnPropertyChanged(nameof(Int32BE));
            OnPropertyChanged(nameof(Int64BE));
            OnPropertyChanged(nameof(Int16LE));
            OnPropertyChanged(nameof(Int32LE));
            OnPropertyChanged(nameof(Int64LE));

            OnPropertyChanged(nameof(UInt8));
            OnPropertyChanged(nameof(UInt16BE));
            OnPropertyChanged(nameof(UInt32BE));
            OnPropertyChanged(nameof(UInt64BE));
            OnPropertyChanged(nameof(UInt16LE));
            OnPropertyChanged(nameof(UInt32LE));
            OnPropertyChanged(nameof(UInt64LE));

            OnPropertyChanged(nameof(UInt8Hex));
            OnPropertyChanged(nameof(UInt16BEHex));
            OnPropertyChanged(nameof(UInt32BEHex));
            OnPropertyChanged(nameof(UInt64BEHex));
            OnPropertyChanged(nameof(UInt16LEHex));
            OnPropertyChanged(nameof(UInt32LEHex));
            OnPropertyChanged(nameof(UInt64LEHex));
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Properties

        public int? X { get; set; }
        public int? Y { get; set; }
        public int? OffsetImage { get; set; }
        public int? OffsetFile { get; set; }
        public sbyte? Int8 { get; set; }

        public short? Int16BE { get; set; }
        public short? Int16LE { get; set; }
        public int? Int32BE { get; set; }
        public int? Int32LE { get; set; }
        public long? Int64BE { get; set; }
        public long? Int64LE { get; set; }

        public byte? UInt8 { get; set; }
        public ushort? UInt16BE { get; set; }
        public ushort? UInt16LE { get; set; }
        public uint? UInt32BE { get; set; }
        public uint? UInt32LE { get; set; }
        public ulong? UInt64BE { get; set; }
        public ulong? UInt64LE { get; set; }
        public string UInt8Hex { get; set; }

        public string UInt16BEHex { get; set; }
        public string UInt16LEHex { get; set; }
        public string UInt32BEHex { get; set; }
        public string UInt32LEHex { get; set; }
        public string UInt64BEHex { get; set; }
        public string UInt64LEHex { get; set; }

        #endregion
    }
}