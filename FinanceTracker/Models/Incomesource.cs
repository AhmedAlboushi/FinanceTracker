using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Incomesource
{
    public int Incomesourceid { get; set; }

    public string Incomesourcename { get; set; } = null!;

    public bool Isactive { get; set; }

    public int Walletid { get; set; }

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual Wallet Wallet { get; set; } = null!;
}
