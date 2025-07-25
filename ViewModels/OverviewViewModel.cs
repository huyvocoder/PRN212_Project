using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Models;
using PRN212_Project.Services;
using PRN212_Project.Views;
using System.Windows; // Cần thiết để sử dụng Window

namespace PRN212_Project.ViewModels
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private decimal _balance;
        private List<Category> _categories;
        private List<Transaction> _transactions;
        private List<Notification> _notifications;
        private string _selectedMonthFilter;
        private string _aiInput;
        private string _aiAdvice;

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Balance
        {
            get => _balance;
            set { _balance = value; OnPropertyChanged(); }
        }
        public List<Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }
        public List<Transaction> Transactions
        {
            get => _transactions;
            set { _transactions = value; OnPropertyChanged(); }
        }
        public List<Notification> Notifications
        {
            get => _notifications;
            set { _notifications = value; OnPropertyChanged(); }
        }
        public List<string> MonthFilters => new List<string> { "All Months", "July 2025", "June 2025", "May 2025" };
        public string SelectedMonthFilter
        {
            get => _selectedMonthFilter;
            set
            {
                _selectedMonthFilter = value;
                LoadTransactions();
                OnPropertyChanged();
            }
        }
        public string AIInput
        {
            get => _aiInput;
            set { _aiInput = value; OnPropertyChanged(); }
        }
        public string AIAdvice
        {
            get => _aiAdvice;
            set { _aiAdvice = value; OnPropertyChanged(); }
        }

        public ICommand AddIncomeCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand ManageCategoriesCommand { get; }
        public ICommand GetAIAdviceCommand { get; }
        public ICommand ChangePasswordCommand { get; } // THÊM: Command cho đổi mật khẩu

        public OverviewViewModel()
        {
            AddIncomeCommand = new RelayCommand(OpenAddIncome);
            AddExpenseCommand = new RelayCommand(AddExpense);
            ManageCategoriesCommand = new RelayCommand(OpenManageCategories);
            GetAIAdviceCommand = new RelayCommand(GetAIAdvice);
            ChangePasswordCommand = new RelayCommand(OpenChangePassword); // THÊM: Khởi tạo Command
            LoadData();
        }

        private void LoadData()
        {
            if (App.CurrentUserId == 0) return;
            using var context = new Prn212ProjectContext();
            var user = context.Users.Find(App.CurrentUserId);
            Balance = user.Balance;
            Categories = new DataService().GetCategories(App.CurrentUserId);
            LoadTransactions();
            Notifications = new DataService().GetNotifications(App.CurrentUserId);
        }

        private void LoadTransactions()
        {
            Transactions = new DataService().GetTransactions(App.CurrentUserId, SelectedMonthFilter);
        }

        private void OpenAddIncome(object parameter)
        {
            var window = new AddIncomeWindow();
            window.ShowDialog();
            LoadData();
        }

        private void AddExpense(object parameter)
        {
            if (parameter is int categoryId)
            {
                var window = new AddExpenseWindow(categoryId);
                window.ShowDialog();
                LoadData();
            }
        }

        private void OpenManageCategories(object parameter)
        {
            var window = new ManageCategoriesWindow();
            window.ShowDialog();
            LoadData();
        }

        private void OpenChangePassword(object parameter) // THÊM: Phương thức để mở cửa sổ đổi mật khẩu
        {
            var window = new ChangePasswordWindow();
            window.ShowDialog();
            // Không cần LoadData() ở đây vì đổi mật khẩu không ảnh hưởng đến dữ liệu hiển thị
        }

        private void GetAIAdvice(object parameter)
        {
            AIAdvice = new DataService().GetAIAdvice(App.CurrentUserId, AIInput);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
