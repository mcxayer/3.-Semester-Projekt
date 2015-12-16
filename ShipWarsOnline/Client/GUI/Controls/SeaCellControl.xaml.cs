using Game.ReadOnly;
using System.Windows.Controls;

namespace Client.GUI.Controls
{
    /// <summary>
    /// Interaction logic for SeaCellControl.xaml
    /// </summary>
    public partial class SeaCellControl : UserControl
    {
        public ReadOnlySeaCell Cell { get; set; }

        public SeaCellControl()
        {
            InitializeComponent();
        }
    }
}
