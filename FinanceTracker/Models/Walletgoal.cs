using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Walletgoal
{
    public int Walletgoalid { get; set; }

    public int Walletid { get; set; }

    public string Goalname { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Allocatedamount { get; set; }

    public decimal Targetamount { get; set; }

    public DateOnly? Targetdate { get; set; }

    public bool Iscompleted { get; set; }

    public short Priority { get; set; }

    public string? Goalimageurl { get; set; }

    public DateOnly Createdat { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}
