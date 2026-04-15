using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Userwallet
{
    public int Userwalletid { get; set; }

    public int Userid { get; set; }

    public int Walletid { get; set; }

    public short Walletroleid { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;

    public virtual Walletrole Walletrole { get; set; } = null!;
}
