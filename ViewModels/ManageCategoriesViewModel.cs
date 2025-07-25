using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PRN212_Project.Helpers;
using PRN212_Project.Models;
using PRN212_Project.Services;

namespace PRN212_Project.ViewModels
{
    public class ManageCategoriesViewModel : INotifyPropertyChanged
    {
        private readonly DataService _dataService = new DataService();
        private List<Category> _categories;
        private string _newCategoryName;
        private decimal _newCategoryLimit;

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Category> Categories
        {
            get => _categories;
            set { _categories = value; OnPropertyChanged(); }
        }

        public string NewCategoryName
        {
            get => _newCategoryName;
            set { _newCategoryName = value; OnPropertyChanged(); }
        }

        public decimal NewCategoryLimit
        {
            get => _newCategoryLimit;
            set { _newCategoryLimit = value; OnPropertyChanged(); }
        }

        public ICommand AddCategoryCommand { get; }
        public ICommand EditCategoryCommand { get; }
        public ICommand DeleteCategoryCommand { get; }
        public ICommand SaveCommand { get; }

        public ManageCategoriesViewModel()
        {
            AddCategoryCommand = new RelayCommand(AddCategory, CanAddCategory);
            EditCategoryCommand = new RelayCommand(EditCategory);
            DeleteCategoryCommand = new RelayCommand(DeleteCategory);
            SaveCommand = new RelayCommand(Save);
            LoadCategories();
        }

        private void LoadCategories()
        {
            Categories = _dataService.GetCategories(App.CurrentUserId);
        }

        private bool CanAddCategory(object parameter) => !string.IsNullOrWhiteSpace(NewCategoryName) && NewCategoryLimit > 0;

        private void AddCategory(object parameter)
        {
            _dataService.AddCategory(App.CurrentUserId, NewCategoryName, NewCategoryLimit);
            LoadCategories();
            NewCategoryName = "";
            NewCategoryLimit = 0;
        }

        private void EditCategory(object parameter)
        {
            if (parameter is int categoryId)
            {
                var category = Categories.Find(c => c.Id == categoryId);
                if (category != null)
                {
                    _dataService.UpdateCategory(categoryId, category.Name, category.MonthlyLimit);
                    LoadCategories();
                }
            }
        }

        private void DeleteCategory(object parameter)
        {
            if (parameter is int categoryId)
            {
                _dataService.DeleteCategory(categoryId);
                LoadCategories();
            }
        }

        private void Save(object parameter)
        {
            LoadCategories();
            //Application.Current.Windows[0].Close();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}