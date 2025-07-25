using System.Windows;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
            DataContext = new ChangePasswordViewModel(this);
        }
    }
}
