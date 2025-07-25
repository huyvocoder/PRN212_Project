using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Models;
using PRN212_Project.Services;
using System.Linq;

namespace PRN212_Project.ViewModels
{
    public class TransactionHistoryViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private readonly Window _currentWindow;
        private List<Transaction> _transactions;
        private string _historyTitle;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Transaction> Transactions
        {
            get => _transactions;
            set { _transactions = value; OnPropertyChanged(); }
        }

        public string HistoryTitle
        {
            get => _historyTitle;
            set { _historyTitle = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; }

        public TransactionHistoryViewModel(Window currentWindow, int? categoryId, bool isIncomeHistory)
        {
            _currentWindow = currentWindow;
            CloseCommand = new RelayCommand(CloseWindow);
            LoadTransactions(categoryId, isIncomeHistory);
        }

        private void LoadTransactions(int? categoryId, bool isIncomeHistory)
        {
            if (App.CurrentUserId == 0) return;

            if (isIncomeHistory)
            {
                Transactions = _dataService.GetTransactions(App.CurrentUserId, "All Months")
                                           .Where(t => t.Type == "Thu")
                                           .ToList();
                HistoryTitle = "Income History";
            }
            else if (categoryId.HasValue)
            {
                using (var context = new Prn212ProjectContext())
                {
                    var category = context.Categories.FirstOrDefault(c => c.Id == categoryId.Value);
                    HistoryTitle = $"Expense History for {category?.Name ?? "Unknown Category"}";
                }
                Transactions = _dataService.GetTransactions(App.CurrentUserId, "All Months")
                                           .Where(t => t.CategoryId == categoryId.Value && t.Type == "Chi")
                                           .ToList();
            }
            else
            {
                Transactions = new List<Transaction>(); // Should not happen if logic is correct
                HistoryTitle = "Transaction History";
            }
        }

        private void CloseWindow(object parameter)
        {
            _currentWindow?.Close();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
