using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Services;
using PRN212_Project.Models;
using System.Windows;
using System.Linq; // Thêm using này để sử dụng LINQ

namespace PRN212_Project.ViewModels
{
    public class AddExpenseViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private int _categoryId;
        private decimal _amount;
        private string _note;
        private Window _currentWindow;
        private string _categoryName; // THÊM: Thuộc tính mới để lưu tên danh mục

        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(); }
        }

        public string Note
        {
            get => _note;
            set { _note = value; OnPropertyChanged(); }
        }

        public string CategoryName // THÊM: Getter cho tên danh mục
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddExpenseViewModel(Window currentWindow, int categoryId)
        {
            _currentWindow = currentWindow;
            _categoryId = categoryId;

            // THÊM: Lấy tên danh mục từ categoryId
            using (var context = new Prn212ProjectContext())
            {
                var category = context.Categories.FirstOrDefault(c => c.Id == _categoryId);
                CategoryName = category != null ? category.Name : "Unknown Category";
            }

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private bool CanSave(object parameter) => Amount > 0;

        private void Save(object parameter)
        {
            _dataService.AddExpense(App.CurrentUserId, _categoryId, Amount, Note, DateTime.Now);
            _currentWindow?.Close();
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
