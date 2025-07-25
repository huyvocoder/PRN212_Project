using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PRN212_Project.Models;

namespace PRN212_Project.Services
{
    public class DataService
    {
        // Bảng tạm để lưu mã xác thực email (giả lập, có thể thay bằng MemoryCache)
        private static readonly Dictionary<string, string> _emailVerifications = new Dictionary<string, string>();

        public User AuthenticateUser(string username, string password)
        {
            using var context = new Prn212ProjectContext();
            return context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public bool RegisterUser(string username, string password, string email)
        {
            using var context = new Prn212ProjectContext();
            if (context.Users.Any(u => u.Username == username || u.Email == email))
                return false;
            var user = new User { Username = username, Password = password, Email = email, Balance = 0 };
            context.Users.Add(user);
            context.SaveChanges();
            return true;
        }

        public string GetPasswordByEmail(string email)
        {
            using var context = new Prn212ProjectContext();
            var user = context.Users.FirstOrDefault(u => u.Email == email);
            return user?.Password;
        }

        public List<Category> GetCategories(int userId)
        {
            using var context = new Prn212ProjectContext();
            var categories = context.Categories.Where(c => c.UserId == userId).ToList();
            foreach (var category in categories)
            {
                if (category.ResetDate <= DateTime.Now)
                {
                    category.CurrentSpent = 0;
                    category.ResetDate = DateTime.Now.AddMonths(1).Date;
                    context.SaveChanges();
                }
            }
            return categories;
        }

        public List<Transaction> GetTransactions(int userId, string monthFilter = null)
        {
            using var context = new Prn212ProjectContext();
            var query = context.Transactions
                .Include(t => t.Category)
                .Where(t => t.UserId == userId);
            if (!string.IsNullOrEmpty(monthFilter) && monthFilter != "All Months")
            {
                var yearMonth = monthFilter.Split(' ');
                var monthName = yearMonth[0];
                var year = int.Parse(yearMonth[1]);
                var month = DateTime.ParseExact(monthName, "MMMM", System.Globalization.CultureInfo.InvariantCulture).Month;
                query = query.Where(t => t.Date.Year == year && t.Date.Month == month);
            }
            return query.OrderByDescending(t => t.Date).ToList();
        }

        public List<Report> GetReports(int userId)
        {
            using var context = new Prn212ProjectContext();
            return context.Reports.Where(r => r.UserId == userId).ToList();
        }

        public List<Notification> GetNotifications(int userId)
        {
            using var context = new Prn212ProjectContext();
            return context.Notifications.Where(n => n.UserId == userId).OrderByDescending(n => n.Date).ToList();
        }

        public void AddIncome(int userId, decimal amount, string note, DateTime date)
        {
            using var context = new Prn212ProjectContext();
            var user = context.Users.First(u => u.Id == userId);
            user.Balance += amount;
            context.Transactions.Add(new Transaction
            {
                UserId = userId,
                Amount = amount,
                Note = note,
                Date = date,
                Type = "Thu" // Đã đổi từ "Thu" sang "Income"
            });
            context.SaveChanges();
        }

        public void AddExpense(int userId, int categoryId, decimal amount, string note, DateTime date)
        {
            using var context = new Prn212ProjectContext();
            var user = context.Users.First(u => u.Id == userId);
            var category = context.Categories.First(c => c.Id == categoryId);
            user.Balance -= amount;
            category.CurrentSpent += amount;
            context.Transactions.Add(new Transaction
            {
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Note = note,
                Date = date,
                Type = "Chi" // Đã đổi từ "Chi" sang "Expense"
            });
            if (category.CurrentSpent >= category.MonthlyLimit * 0.8m)
            {
                context.Notifications.Add(new Notification
                {
                    UserId = userId,
                    Message = $"Category {category.Name} reached {(category.CurrentSpent / category.MonthlyLimit * 100):F0}% of limit", // Đã dịch
                    Date = DateTime.Now,
                    IsRead = false
                });
            }
            context.SaveChanges();
        }

        // ĐIỀU CHỈNH: Phương thức AddCategory để trả về bool và kiểm tra trùng lặp
        public bool AddCategory(int userId, string name, decimal limit)
        {
            using var context = new Prn212ProjectContext();
            try
            {
                // Kiểm tra xem danh mục đã tồn tại cho người dùng này chưa
                if (context.Categories.Any(c => c.UserId == userId && c.Name == name))
                {
                    return false; // Danh mục đã tồn tại
                }

                context.Categories.Add(new Category
                {
                    UserId = userId,
                    Name = name,
                    MonthlyLimit = limit,
                    ResetDate = DateTime.Now.AddMonths(1).Date
                });
                context.SaveChanges();
                return true; // Thêm thành công
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                System.Diagnostics.Debug.WriteLine($"Error adding category: {ex.Message}");
                return false; // Xảy ra lỗi
            }
        }

        public void UpdateCategory(int categoryId, string name, decimal limit)
        {
            using var context = new Prn212ProjectContext();
            var category = context.Categories.First(c => c.Id == categoryId);
            category.Name = name;
            category.MonthlyLimit = limit;
            context.SaveChanges();
        }

        public void DeleteCategory(int categoryId)
        {
            using var context = new Prn212ProjectContext();
            var category = context.Categories.First(c => c.Id == categoryId);
            context.Categories.Remove(category);
            context.SaveChanges();
        }

        public string GetAIAdvice(int userId, string input)
        {
            using var context = new Prn212ProjectContext();
            var categories = context.Categories.Where(c => c.UserId == userId).ToList();
            var totalSpent = categories.Sum(c => c.CurrentSpent);
            if (string.IsNullOrWhiteSpace(input))
                return "Please enter a question or describe your financial situation.";
            if (input.ToLower().Contains("save"))
                return $"You have spent {totalSpent:C} this month. Consider reducing expenses in {categories.OrderByDescending(c => c.CurrentSpent).First().Name}.";
            return "General advice: Track your spending regularly and set reasonable limits.";
        }

        public User GetUserByUsernameAndEmail(string username, string email)
        {
            using var context = new Prn212ProjectContext();
            return context.Users.FirstOrDefault(u => u.Username == username && u.Email == email);
        }

        public void UpdatePassword(string username, string newPassword)
        {
            using var context = new Prn212ProjectContext();
            var user = context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                user.Password = newPassword;
                context.SaveChanges();
            }
        }

        public void StoreVerificationCode(string email, string code)
        {
            _emailVerifications[email] = code;
        }

        public bool VerifyEmailCode(string email, string code)
        {
            if (_emailVerifications.TryGetValue(email, out string storedCode) && storedCode == code)
            {
                _emailVerifications.Remove(email); // Xóa mã sau khi xác thực
                return true;
            }
            return false;
        }
    }
}
