using PRN212_Project.ViewModels;
using System.Windows;
using System.Linq; // Thêm using này nếu chưa có để dùng OfType<Window>().LastOrDefault()

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private RegisterViewModel _viewModel;
        public RegisterWindow()
        {
            InitializeComponent();
            _viewModel = new RegisterViewModel(this);
            DataContext = _viewModel;
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.Register();
            }
        }
    }
}
