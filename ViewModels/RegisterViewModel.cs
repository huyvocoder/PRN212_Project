using System.ComponentModel;
using System.Net.Mail;
using System.Windows;
using PRN212_Project.Helpers;
using PRN212_Project.Services;
using PRN212_Project.Models;

namespace PRN212_Project.ViewModels
{
    public class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private string _username;
        private string _email;
        private string _verificationCode;
        private string _confirmPassword;
        private string _password; // Thuộc tính mới cho mật khẩu chính
        private Window _currentWindow; // THÊM: Trường để lưu tham chiếu cửa sổ

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }

        public string VerificationCode
        {
            get => _verificationCode;
            set
            {
                _verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }

        public string Password // Thuộc tính mới cho mật khẩu chính
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                OnPropertyChanged(nameof(IsRegisterEnabled));
            }
        }

        public bool IsRegisterEnabled
        {
            get
            {
                // CHỈ KIỂM TRA CÁC TRƯỜNG CÓ ĐƯỢC ĐIỀN HAY KHÔNG ĐỂ KÍCH HOẠT NÚT
                bool isUsernameFilled = !string.IsNullOrWhiteSpace(Username);
                bool isEmailFilled = !string.IsNullOrWhiteSpace(Email);
                bool isVerificationCodeFilled = !string.IsNullOrWhiteSpace(VerificationCode);
                bool isPasswordFilled = !string.IsNullOrWhiteSpace(Password); // Kiểm tra thuộc tính Password mới
                bool isConfirmPasswordFilled = !string.IsNullOrWhiteSpace(ConfirmPassword);

                System.Diagnostics.Debug.WriteLine($"DEBUG: IsRegisterEnabled Check:");
                System.Diagnostics.Debug.WriteLine($"  Username: '{Username}' -> Filled: {isUsernameFilled}");
                System.Diagnostics.Debug.WriteLine($"  Email: '{Email}' -> Filled: {isEmailFilled}");
                System.Diagnostics.Debug.WriteLine($"  VerificationCode: '{VerificationCode}' -> Filled: {isVerificationCodeFilled}");
                System.Diagnostics.Debug.WriteLine($"  Password: '{Password}' -> Filled: {isPasswordFilled}");
                System.Diagnostics.Debug.WriteLine($"  ConfirmPassword: '{ConfirmPassword}' -> Filled: {isConfirmPasswordFilled}");
                System.Diagnostics.Debug.WriteLine($"  Overall IsRegisterEnabled: {isUsernameFilled && isEmailFilled && isVerificationCodeFilled && isPasswordFilled && isConfirmPasswordFilled}");

                return isUsernameFilled && isEmailFilled && isVerificationCodeFilled && isPasswordFilled && isConfirmPasswordFilled;
            }
        }

        public System.Windows.Input.ICommand SendVerificationCommand { get; private set; }
        public System.Windows.Input.ICommand CancelCommand { get; private set; }

        public RegisterViewModel()
        {
            // Constructor mặc định, có thể được gọi nếu không có tham số nào được truyền
            // Trong trường hợp này, _currentWindow sẽ là null, nên cần cẩn thận khi sử dụng
            // _currentWindow = Application.Current.Windows.OfType<Window>().LastOrDefault(); // XÓA DÒNG NÀY
            SendVerificationCommand = new RelayCommand(SendVerification, CanSendVerification);
            CancelCommand = new RelayCommand(Cancel);
        }

        // THÊM: Constructor mới để nhận tham chiếu cửa sổ
        public RegisterViewModel(Window currentWindow) : this() // Gọi constructor mặc định để khởi tạo Commands
        {
            _currentWindow = currentWindow;
        }

        private bool CanSendVerification(object parameter)
        {
            System.Diagnostics.Debug.WriteLine($"CanSendVerification: Email='{Email}'");
            return !string.IsNullOrWhiteSpace(Email);
        }

        private bool IsEmailAvailable(string email)
        {
            using var context = new Prn212ProjectContext();
            bool available = !context.Users.Any(u => u.Email == email);
            System.Diagnostics.Debug.WriteLine($"IsEmailAvailable: {email} -> {available}");
            return available;
        }

        private void SendVerification(object parameter)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"SendVerification: Email='{Email}'");
                if (string.IsNullOrWhiteSpace(Email))
                {
                    MessageBox.Show("Please enter an email.");
                    return;
                }
                if (!IsEmailAvailable(Email))
                {
                    MessageBox.Show("Email already exists.");
                    return;
                }
                string verificationCode = System.Guid.NewGuid().ToString().Substring(0, 6);
                _dataService.StoreVerificationCode(Email, verificationCode);
                SendVerificationEmail(Email, verificationCode);
                MessageBox.Show($"A verification code has been sent to {Email}.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void SendVerificationEmail(string toEmail, string verificationCode)
        {
            string fromEmail = "huyvo0234@gmail.com";
            string fromPassword = "vjtvixgupafxjjxl";
            try
            {
                MailMessage message = new MailMessage(fromEmail, toEmail);
                message.Subject = "Email Verification";
                message.Body = $"Your verification code is: {verificationCode}.";
                message.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(fromEmail, fromPassword);
                smtp.Send(message);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Failed to send verification code: {ex.Message}");
                throw;
            }
        }

        public bool VerifyEmailCode(string email, string code)
        {
            return _dataService.VerifyEmailCode(email, code); // Giả định phương thức này tồn tại
        }

        public void Register() // Xóa tham số 'string password'
        {
            try
            {
                // KIỂM TRA TÍNH HỢP LỆ KHI NÚT ĐƯỢC BẤM
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                    string.IsNullOrWhiteSpace(VerificationCode) || string.IsNullOrWhiteSpace(Password) ||
                    string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                // Nếu bạn muốn kiểm tra độ dài mật khẩu, hãy thêm vào đây:
                // if (Password.Length < 6)
                // {
                //     MessageBox.Show("Password must be at least 6 characters.");
                //     return;
                // }

                System.Diagnostics.Debug.WriteLine($"Register: Password='{Password}'");
                if (_dataService.VerifyEmailCode(Email, VerificationCode))
                {
                    if (_dataService.RegisterUser(Username, Password, Email)) // Sử dụng this.Password
                    {
                        MessageBox.Show("Registration successful.");
                        _currentWindow?.Close(); // SỬ DỤNG _currentWindow để đóng cửa sổ
                    }
                    else
                    {
                        MessageBox.Show("Registration failed. Username or Email may already exist.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid verification code.");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void Cancel(object parameter)
        {
            _currentWindow?.Close(); // SỬ DỤNG _currentWindow để đóng cửa sổ
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
