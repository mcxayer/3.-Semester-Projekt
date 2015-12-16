using System.Windows;
using System.Windows.Controls;

namespace Client.GUI.Controls
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuControl : UserControl, IGUIControl
    {
        public MainMenuControl()
        {
            InitializeComponent();
        }

        private void OnStartMultiplayerButtonClicked(object sender, RoutedEventArgs e)
        {
            GUIFacade.Instance.GotoLogin();
        }

        public void OnSelected()
        {
            // Nothing
        }

        public FrameworkElement GetElement()
        {
            return this;
        }
    }
}
