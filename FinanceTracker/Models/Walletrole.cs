using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Walletrole
{
    public short Walletroleid { get; set; }

    public string Rolename { get; set; } = null!;

    public virtual ICollection<Userwallet> Userwallets { get; set; } = new List<Userwallet>();
}
