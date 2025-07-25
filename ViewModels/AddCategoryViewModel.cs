using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Services;

namespace PRN212_Project.ViewModels
{
    public class AddCategoryViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private readonly Window _currentWindow;
        private string _categoryName;
        private decimal _monthlyLimit;

        public event PropertyChangedEventHandler PropertyChanged;

        public string CategoryName
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSave)); }
        }

        public decimal MonthlyLimit
        {
            get => _monthlyLimit;
            set { _monthlyLimit = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSave)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public bool CanSave => !string.IsNullOrWhiteSpace(CategoryName) && MonthlyLimit > 0;

        public AddCategoryViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;
            SaveCommand = new RelayCommand(Save, param => CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object parameter)
        {
            try
            {
                if (_dataService.AddCategory(App.CurrentUserId, CategoryName, MonthlyLimit))
                {
                    MessageBox.Show("Category added successfully.");
                    _currentWindow?.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add category. Category name might already exist.");
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
