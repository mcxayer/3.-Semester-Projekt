using System.Windows;

namespace Client.GUI
{
    /// <summary>
    /// Interaction logic for DialogBox.xaml
    /// </summary>
    public partial class DialogTextBox : Window
    {
        public DialogTextBox()
        {
            InitializeComponent();
        }

        public string ResponseValue { get { return tbValue.Text; } set { tbValue.Text = value; } }

        public string Information { get { return lblInfo.Content as string; } set { lblInfo.Content = value; } }

        private void OnOkButtonClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
