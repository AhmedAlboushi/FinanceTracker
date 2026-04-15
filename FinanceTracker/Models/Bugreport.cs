using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Bugreport
{
    public int Bugreportid { get; set; }

    public int Userid { get; set; }

    public string Description { get; set; } = null!;

    public bool? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
