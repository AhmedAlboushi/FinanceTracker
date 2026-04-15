using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Log
{
    public int Logid { get; set; }

    public int? Userid { get; set; }

    public string Action { get; set; } = null!;

    public string Httpmethod { get; set; } = null!;

    public string Endpoint { get; set; } = null!;

    public short Statuscode { get; set; }

    public short Securitylevel { get; set; }

    public string Ipaddress { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public virtual User? User { get; set; }
}
