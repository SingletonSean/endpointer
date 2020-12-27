using System.Windows;

namespace Endpointer.Demos.WPF
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
