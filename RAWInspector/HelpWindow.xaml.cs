using System.Windows.Input;

namespace RAWInspector
{
    public partial class HelpWindow
    {
        public HelpWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) => { IsOpen = true; };
            Closed += (sender, args) => { IsOpen = false; };
        }

        public static bool IsOpen { get; private set; }

        private void HelpWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}