using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Category
{
    public int Categoryid { get; set; }

    public string Categoryname { get; set; } = null!;

    public string Colorhex { get; set; } = null!;

    public string? Iconname { get; set; }

    public int Walletid { get; set; }

    public bool Isactive { get; set; }

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual Wallet Wallet { get; set; } = null!;
}
