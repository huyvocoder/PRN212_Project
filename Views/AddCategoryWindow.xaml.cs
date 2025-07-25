using System.Windows;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        public AddCategoryWindow()
        {
            InitializeComponent();
            DataContext = new AddCategoryViewModel(this);
        }
    }
}
