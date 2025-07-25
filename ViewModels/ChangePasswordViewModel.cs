using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net.Mail;
using System.Windows;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Models;
using PRN212_Project.Services;
using System.Linq;

namespace PRN212_Project.ViewModels
{
    public class ChangePasswordViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private readonly Window _currentWindow;
        private int _currentUserId;

        private string _currentPassword;
        private string _newPassword;
        private string _confirmNewPassword;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                _currentPassword = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanChangePassword)); // Vẫn thông báo để cập nhật trạng thái nút
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(); // Thông báo thay đổi cho NewPassword
                OnPropertyChanged(nameof(CanChangePassword)); // Vẫn thông báo để cập nhật trạng thái nút
            }
        }

        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set
            {
                _confirmNewPassword = value;
                OnPropertyChanged(); // Thông báo thay đổi cho ConfirmNewPassword
                OnPropertyChanged(nameof(CanChangePassword)); // Vẫn thông báo để cập nhật trạng thái nút
            }
        }

        public ICommand ChangePasswordCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public bool CanChangePassword
        {
            get
            {
                // CHỈ KIỂM TRA CÁC TRƯỜNG CÓ ĐƯỢC ĐIỀN HAY KHÔNG ĐỂ KÍCH HOẠT NÚT
                bool isCurrentPasswordFilled = !string.IsNullOrWhiteSpace(CurrentPassword);
                bool isNewPasswordFilled = !string.IsNullOrWhiteSpace(NewPassword);
                bool isConfirmNewPasswordFilled = !string.IsNullOrWhiteSpace(ConfirmNewPassword);

                System.Diagnostics.Debug.WriteLine($"DEBUG: CanChangePassword (Button Enablement) Check:");
                System.Diagnostics.Debug.WriteLine($"  CurrentPassword Filled: {isCurrentPasswordFilled}");
                System.Diagnostics.Debug.WriteLine($"  NewPassword Filled: {isNewPasswordFilled}");
                System.Diagnostics.Debug.WriteLine($"  ConfirmNewPassword Filled: {isConfirmNewPasswordFilled}");
                System.Diagnostics.Debug.WriteLine($"  Overall CanChangePassword: {isCurrentPasswordFilled && isNewPasswordFilled && isConfirmNewPasswordFilled}");

                return isCurrentPasswordFilled &&
                       isNewPasswordFilled &&
                       isConfirmNewPasswordFilled;
            }
        }

        public ChangePasswordViewModel()
        {
            _currentUserId = App.CurrentUserId;
            ChangePasswordCommand = new RelayCommand(ExecuteChangePassword, param => CanChangePassword);
            CancelCommand = new RelayCommand(Cancel);
        }

        public ChangePasswordViewModel(Window currentWindow) : this()
        {
            _currentWindow = currentWindow;
        }

        private void ExecuteChangePassword(object parameter)
        {
            try
            {
                // KIỂM TRA TÍNH HỢP LỆ KHI NÚT ĐƯỢC BẤM
                if (string.IsNullOrWhiteSpace(CurrentPassword) ||
                    string.IsNullOrWhiteSpace(NewPassword) ||
                    string.IsNullOrWhiteSpace(ConfirmNewPassword))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                if (NewPassword != ConfirmNewPassword)
                {
                    MessageBox.Show("New password and confirmation do not match.");
                    return;
                }

                // Nếu bạn muốn kiểm tra độ dài mật khẩu, hãy thêm vào đây:
                // if (NewPassword.Length < 6)
                // {
                //     MessageBox.Show("New password must be at least 6 characters long.");
                //     return;
                // }

                using var context = new Prn212ProjectContext();
                var user = context.Users.FirstOrDefault(u => u.Id == _currentUserId && u.Password == CurrentPassword);

                if (user != null)
                {
                    user.Password = NewPassword;
                    context.SaveChanges();
                    MessageBox.Show("Password changed successfully.");
                    _currentWindow?.Close();
                }
                else
                {
                    MessageBox.Show("Current password is incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Cancel(object parameter)
        {
            _currentWindow?.Close();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
