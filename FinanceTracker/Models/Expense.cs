using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Expense
{
    public int Expenseid { get; set; }

    public int Userid { get; set; }

    public int Walletid { get; set; }

    public int Categoryid { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateOnly Createdat { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
