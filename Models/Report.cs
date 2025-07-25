using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class Report
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Month { get; set; } = null!;

    public decimal TotalIncome { get; set; }

    public decimal TotalExpense { get; set; }

    public decimal Balance { get; set; }

    public virtual User User { get; set; } = null!;
}
