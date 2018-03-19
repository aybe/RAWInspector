using System;
using System.IO;
using System.Windows;
using JetBrains.Annotations;

namespace RAWInspector
{
    internal static class Storage
    {
        private static string GetTempPath([NotNull] string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var tempPath = Path.GetTempPath();
            var fileName = Path.GetFileName(path);

            for (var i = 0; i < Byte.MaxValue; i++)
            {
                var combine = Path.Combine(tempPath, $"{fileName}.{i}");

                if (!File.Exists(combine))
                    return combine;
            }

            throw new InvalidOperationException("Could not generate a path in temporary folder.");
        }

        public static bool TryOpenFile([NotNull] string path, out FileStream result)
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

        public static bool TryCopyFile([NotNull] string path, out string result)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            result = null;

            try
            {
                var tempFileName = GetTempPath(path);
                File.Copy(path, tempFileName, true);
                result = tempFileName;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryCopyFileWizard([NotNull] string path, out FileStream result)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            result = null;

            const string boxText = "Do you want to copy it to a temporary location and open it ?";
            const string boxCapt = "File could not be opened";

            if (MessageBox.Show(boxText, boxCapt, MessageBoxButton.OKCancel) != MessageBoxResult.OK)
                return false;

            if (!Storage.TryCopyFile(path, out var copy))
            {
                MessageBox.Show("Couldn't copy file to temporary location.", "Error", MessageBoxButton.OK);
                return false;
            }

            if (!Storage.TryOpenFile(copy, out result))
            {
                MessageBox.Show("Couldn't open file.", "Error", MessageBoxButton.OK);
                return false;
            }

            return true;
        }
    }
}