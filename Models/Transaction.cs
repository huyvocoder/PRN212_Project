using System;
using System.Collections.Generic;

namespace PRN212_Project.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public decimal Amount { get; set; }

    public string? Note { get; set; }

    public DateTime Date { get; set; }

    public string Type { get; set; } = null!;

    public virtual Category? Category { get; set; }

    public virtual User User { get; set; } = null!;
}
