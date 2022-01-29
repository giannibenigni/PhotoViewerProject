using System.Windows;

namespace PhotoViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowModel model)
        {
            model.Start();
            DataContext = model;
            InitializeComponent();
        }
    }
}
