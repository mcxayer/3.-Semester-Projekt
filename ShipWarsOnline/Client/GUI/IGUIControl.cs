using System.Windows;

namespace Client.GUI
{
    public interface IGUIControl
    {
        FrameworkElement GetElement();
        void OnSelected();
    }
}
