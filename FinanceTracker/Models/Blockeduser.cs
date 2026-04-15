using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Blockeduser
{
    public int Blockeduserid { get; set; }

    public int Userid { get; set; }

    public int Targetuserid { get; set; }

    public DateOnly Createdat { get; set; }

    public virtual User Targetuser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
