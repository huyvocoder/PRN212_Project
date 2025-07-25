using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Models;
using PRN212_Project.Services;
using PRN212_Project.Views;
using System.Windows;
using System.Linq;

namespace PRN212_Project.ViewModels
{
    public class OverviewViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private decimal _balance;
        private List<Category> _categories;

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

        public ICommand AddIncomeCommand { get; }
        public ICommand AddExpenseCommand { get; }
        public ICommand ChangePasswordCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand AddCategoryCommand { get; }
        public ICommand ShowCategoryHistoryCommand { get; }
        public ICommand ShowIncomeHistoryCommand { get; }

        public OverviewViewModel()
        {
            AddIncomeCommand = new RelayCommand(OpenAddIncome);
            AddExpenseCommand = new RelayCommand(AddExpense);
            ChangePasswordCommand = new RelayCommand(OpenChangePassword);
            LogoutCommand = new RelayCommand(Logout);
            AddCategoryCommand = new RelayCommand(OpenAddCategory);
            ShowCategoryHistoryCommand = new RelayCommand(OpenCategoryHistory);
            ShowIncomeHistoryCommand = new RelayCommand(OpenIncomeHistory);

            LoadData();
        }

        private void LoadData()
        {
            if (App.CurrentUserId == 0)
            {
                System.Diagnostics.Debug.WriteLine("LoadData: No user logged in, skipping.");
                return;
            }
            using var context = new Prn212ProjectContext();
            var user = context.Users.Find(App.CurrentUserId);
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine($"LoadData: User with Id {App.CurrentUserId} not found.");
                return;
            }
            Balance = user.Balance;
            Categories = _dataService.GetCategories(App.CurrentUserId).Select(c =>
            {
                var transactions = _dataService.GetTransactions(App.CurrentUserId, "All Months");
                System.Diagnostics.Debug.WriteLine($"LoadData: Category={c.Name}, Id={c.Id}, MonthlyLimit={c.MonthlyLimit}");
                c.CurrentSpent = transactions
                    .Where(t => t.CategoryId == c.Id && t.Type.ToLower() == "chi") // Thay "expense" bằng "chi"
                    .Sum(t => t.Amount == null ? 0: t.Amount);
                System.Diagnostics.Debug.WriteLine($"LoadData: Category={c.Name}, CurrentSpent={c.CurrentSpent}");
                return c;
            }).ToList();
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
                System.Diagnostics.Debug.WriteLine($"AddExpense: Opening for CategoryId={categoryId}");
                var window = new AddExpenseWindow(categoryId);
                window.ShowDialog();
                LoadData();
            }
        }

        private void OpenChangePassword(object parameter)
        {
            var window = new ChangePasswordWindow();
            window.ShowDialog();
        }

        private void Logout(object parameter)
        {
            App.CurrentUserId = 0;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is OverviewWindow)?.Close();
        }

        private void OpenAddCategory(object parameter)
        {
            var window = new AddCategoryWindow();
            window.ShowDialog();
            LoadData();
        }

        private void OpenCategoryHistory(object parameter)
        {
            if (parameter is int categoryId)
            {
                var historyWindow = new TransactionHistoryWindow(categoryId: categoryId);
                historyWindow.ShowDialog();
            }
        }

        private void OpenIncomeHistory(object parameter)
        {
            var historyWindow = new TransactionHistoryWindow(isIncomeHistory: true);
            historyWindow.ShowDialog();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}