using System.Windows;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for AddIncomeWindow.xaml
    /// </summary>
    public partial class AddIncomeWindow : Window
    {
        public AddIncomeWindow()
        {
            InitializeComponent();
            this.DataContext = new AddIncomeViewModel(this);
        }
    }
}
