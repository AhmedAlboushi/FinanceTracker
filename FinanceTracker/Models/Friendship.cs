using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Friendship
{
    public int Friendshipid { get; set; }

    public int Userid { get; set; }

    public int Frienduserid { get; set; }

    public short Status { get; set; }

    public DateOnly Createdat { get; set; }

    public virtual User Frienduser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
