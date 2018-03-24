using System.Windows;

namespace RAWInspector
{
    internal class WindowService : IWindowService
    {
        public void ShowHelp()
        {
            if (HelpWindow.IsOpen)
                return;

            var window = new HelpWindow();

            window.Show();
        }

        public void Quit()
        {
            Application.Current.Shutdown();
        }
    }
}