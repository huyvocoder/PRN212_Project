using System;
using System.Collections.Generic;

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
}
