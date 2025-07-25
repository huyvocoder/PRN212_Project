using System.Windows;
using System.Windows.Controls;

namespace PRN212_Project.Helpers // ĐẢM BẢO NAMESPACE NÀY CHÍNH XÁC
{
    public static class BindablePasswordBox
    {
        // Định nghĩa Attached Property
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword",
                                                typeof(string),
                                                typeof(BindablePasswordBox),
                                                new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

        // Getter cho Attached Property
        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPasswordProperty);
        }

        // Setter cho Attached Property
        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPasswordProperty, value);
        }

        // Callback khi BoundPassword thay đổi (từ ViewModel ra View)
        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;
            if (passwordBox == null)
            {
                return;
            }

            // Ngăn chặn vòng lặp vô hạn khi cập nhật mật khẩu từ code
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if (!ArePasswordsEqual(passwordBox.Password, (string)e.NewValue))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        // Xử lý sự kiện PasswordChanged của PasswordBox (từ View vào ViewModel)
        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox == null)
            {
                return;
            }

            // Cập nhật BoundPassword khi PasswordBox thay đổi
            SetBoundPassword(passwordBox, passwordBox.Password);
        }

        // Hàm so sánh mật khẩu để tránh cập nhật lại khi không cần thiết
        private static bool ArePasswordsEqual(string p1, string p2)
        {
            if (p1 == null && p2 == null) return true;
            if (p1 == null || p2 == null) return false;
            return p1.Equals(p2);
        }
    }
}
