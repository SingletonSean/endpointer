using System.Windows;

namespace Endpointer.Authentication.Demos.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow(object dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
        }
    }
}
