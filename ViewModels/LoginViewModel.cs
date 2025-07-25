using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Services;
using PRN212_Project.Views;

namespace PRN212_Project.ViewModels
{
    public class LoginViewModel
    {
        private readonly DataService _dataService = new DataService();
        public string Username { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand ForgotPasswordCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login, CanLogin);
            RegisterCommand = new RelayCommand(OpenRegister);
            ForgotPasswordCommand = new RelayCommand(OpenForgotPassword);
            ChangePasswordCommand = new RelayCommand(OpenChangePassword);
        }

        private bool CanLogin(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            var canExecute = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(passwordBox?.Password);
            CommandManager.InvalidateRequerySuggested();
            return canExecute;
        }

        private void Login(object parameter)
        {
            var passwordBox = parameter as PasswordBox;
            try
            {
                var user = _dataService.AuthenticateUser(Username, passwordBox.Password);
                if (user != null)
                {
                    App.CurrentUserId = user.Id;
                    var overviewWindow = new OverviewWindow();
                    overviewWindow.Show();
                    if (Application.Current.Windows.Count > 0 && Application.Current.Windows[0] is Window currentWindow)
                    {
                        currentWindow.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void OpenRegister(object parameter)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private void OpenForgotPassword(object parameter)
        {
            var forgotPasswordWindow = new ForgotPasswordWindow();
            forgotPasswordWindow.ShowDialog();
        }

        private void OpenChangePassword(object parameter)
        {
            var changePasswordWindow = new ChangePasswordWindow();
            changePasswordWindow.ShowDialog();
        }
    }
}