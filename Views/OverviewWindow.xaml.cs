using System.Windows;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        public OverviewWindow()
        {
            InitializeComponent();
            DataContext = new OverviewViewModel();
        }
    }
}
