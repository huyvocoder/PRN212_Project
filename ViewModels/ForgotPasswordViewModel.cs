using System;
using System.Net.Mail;
using System.Windows;
using PRN212_Project.Helpers;
using PRN212_Project.Services;

namespace PRN212_Project.ViewModels
{
    public class ForgotPasswordViewModel
    {
        private readonly DataService _dataService = new DataService();
        public string Username { get; set; }
        public string Email { get; set; }
        public System.Windows.Input.ICommand SendPasswordCommand { get; private set; }
        public System.Windows.Input.ICommand CancelCommand { get; private set; }
        private Window _currentWindow;

        public ForgotPasswordViewModel()
        {
            SendPasswordCommand = new RelayCommand(SendPassword, CanSendPassword);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ForgotPasswordViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;
            SendPasswordCommand = new RelayCommand(SendPassword, CanSendPassword);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSendPassword(object parameter) => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Email);

        private void SendPassword(object parameter)
        {
            try
            {
                var user = _dataService.GetUserByUsernameAndEmail(Username, Email);
                if (user != null)
                {
                    string newPassword = Guid.NewGuid().ToString().Substring(0, 8); // Tạo mật khẩu mới
                    _dataService.UpdatePassword(Username, newPassword); // Cập nhật mật khẩu trong DB
                    SendResetEmail(Email, newPassword); // Gửi mật khẩu mới qua email
                    MessageBox.Show($"A new password has been sent to {Email}.");
                    _currentWindow?.Close();
                }
                else
                {
                    MessageBox.Show("Username or Email does not exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void SendResetEmail(string toEmail, string newPassword)
        {
            string fromEmail = "huyvo0234@gmail.com";
            string fromPassword = "vjtvixgupafxjjxl";

            try
            {
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "Your New Password";
                message.Body = $"Your new password is: {newPassword}. Please log in and change it.";
                message.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword);

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send new password: {ex.Message}");
                throw;
            }
        }

        private void Cancel(object parameter)
        {
            _currentWindow?.Close();
        }

        public Window CurrentWindow
        {
            get => _currentWindow;
            set => _currentWindow = value;
        }
    }
}