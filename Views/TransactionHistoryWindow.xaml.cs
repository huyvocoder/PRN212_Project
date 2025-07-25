using System.Windows;
using PRN212_Project.ViewModels;

namespace PRN212_Project.Views
{
    /// <summary>
    /// Interaction logic for TransactionHistoryWindow.xaml
    /// </summary>
    public partial class TransactionHistoryWindow : Window
    {
        public TransactionHistoryWindow(int? categoryId = null, bool isIncomeHistory = false)
        {
            InitializeComponent();
            DataContext = new TransactionHistoryViewModel(this, categoryId, isIncomeHistory);
        }
    }
}
