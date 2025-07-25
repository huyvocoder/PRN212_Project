using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Services;
using System.Windows; // Cần thiết để sử dụng Window

namespace PRN212_Project.ViewModels
{
    public class AddIncomeViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private decimal _amount;
        private string _note;
        private Window _currentWindow;

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

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddIncomeViewModel()
        {
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);
        }

        public AddIncomeViewModel(Window currentWindow) : this()
        {
            _currentWindow = currentWindow;
        }

        private bool CanSave(object parameter) => Amount > 0;

        private void Save(object parameter)
        {
            _dataService.AddIncome(App.CurrentUserId, Amount, Note, DateTime.Now);
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
