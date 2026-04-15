using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class Chatmessage
{
    public int Messageid { get; set; }

    public int Senderuserid { get; set; }

    public int Receiveruserid { get; set; }

    public string Content { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public bool Isread { get; set; }

    public virtual User Receiveruser { get; set; } = null!;

    public virtual User Senderuser { get; set; } = null!;
}
