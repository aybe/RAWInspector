using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using JetBrains.Annotations;

namespace RAWInspector.Controls
{
    public sealed partial class BitmapControl : INotifyPropertyChanged
    {
        public const string ControlKey = nameof(BitmapControl) + "_PART_Image";

        public BitmapControl()
        {
            InitializeComponent();

            NameScope.SetNameScope(ContextMenu, NameScope.GetNameScope(this));

            Loaded += (sender, e) =>
            {
                if (DataContext is Model model)
                    model.ScrollInfo = Viewer.ScrollInfo;
            };
        }

        #region Event handlers

        private void ImageOnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Screen = e.GetPosition(sender as Image);
        }

        #endregion

        #region Properties

        private Point _screen;

        public Point Screen
        {
            get => _screen;
            private set
            {
                if (value.Equals(_screen)) return;
                _screen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}