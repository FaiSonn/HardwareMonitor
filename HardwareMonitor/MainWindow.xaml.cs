using System.Windows;

namespace HardwareMonitor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow help = new HelpWindow();
            help.ShowDialog();
        }
    }
}