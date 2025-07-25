using System.Windows;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window
    {
        public AddExpenseWindow(int categoryId)
        {
            InitializeComponent();
            DataContext = new AddExpenseViewModel(this, categoryId);
        }
    }
}
