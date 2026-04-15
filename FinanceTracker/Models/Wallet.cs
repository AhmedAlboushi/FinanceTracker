using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Wallet
{
    public int Walletid { get; set; }

    public string Walletname { get; set; } = null!;

    public decimal Savedbalance { get; set; }

    public decimal Availablebalance { get; set; }

    public DateOnly Createdat { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<Incomesource> Incomesources { get; set; } = new List<Incomesource>();

    public virtual ICollection<Userwallet> Userwallets { get; set; } = new List<Userwallet>();

    public virtual ICollection<Walletgoal> Walletgoals { get; set; } = new List<Walletgoal>();
}
