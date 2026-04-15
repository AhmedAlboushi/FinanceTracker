using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Income
{
    public int Incomeid { get; set; }

    public decimal Amount { get; set; }

    public int Walletid { get; set; }

    public int Userid { get; set; }

    public int Incomesourceid { get; set; }

    public string? Description { get; set; }

    public DateOnly Createdat { get; set; }

    public virtual Incomesource Incomesource { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
