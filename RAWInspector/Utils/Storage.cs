using System;
using System.IO;
using System.Windows;
using JetBrains.Annotations;

namespace RAWInspector.Utils
{
    [PublicAPI]
    internal static class Storage
    {
        public static string GetTempPath([NotNull] string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var tempPath = Path.GetTempPath();
            var fileName = Path.GetFileName(path);

            for (var i = 0; i < byte.MaxValue; i++)
            {
                var combine = Path.Combine(tempPath, $"{fileName}.{i}");

                if (!File.Exists(combine))
                    return combine;
            }

            throw new InvalidOperationException("Could not generate a path in temporary folder.");
        }

        public static void TryOpenFile([NotNull] string path, out FileStream stream, out bool temporary)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            temporary = false;
            stream = null;

            try
            {
                stream = File.OpenRead(path);
            }
            catch (Exception)
            {
                // ignored
            }

            if (stream != null)
                return;

            const string text = "Would like to copy it to a temporary location ?";
            const string caption = "File could not be opened";
            var result = MessageBox.Show(text, caption, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                return;

            try
            {
                var fileName = Path.GetFileName(path);
                var tempPath = GetTempPath(fileName);
                File.Copy(path, tempPath);
                temporary = true;
                stream = File.OpenRead(tempPath);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}