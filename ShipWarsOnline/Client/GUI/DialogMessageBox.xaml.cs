using System.Windows;

namespace Client.GUI
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogMessageBox : Window
    {
        public DialogMessageBox()
        {
            InitializeComponent();
        }

        public string Information { get { return lblInfo.Content as string; } set { lblInfo.Content = value; } }

        private void OnOkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
