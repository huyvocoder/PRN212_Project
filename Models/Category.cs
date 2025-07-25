using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRN212_Project.Models;

public partial class Category
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public decimal MonthlyLimit { get; set; }

    public decimal? CurrentSpent { get; set; }

    public DateTime ResetDate { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;

    [NotMapped]
    public string BackgroundColor
    {
        get
        {
            if (MonthlyLimit > 0)
            {
                double percentage = (double)CurrentSpent / (double)MonthlyLimit;
                if (percentage >= 1.0)
                {
                    return "LightCoral"; // Vượt quá giới hạn
                }
                else if (percentage >= 0.8)
                {
                    return "LightYellow"; // Đạt 80% giới hạn
                }
            }
            return "LightGray"; // Mặc định hoặc giới hạn bằng 0
        }
    }
}
