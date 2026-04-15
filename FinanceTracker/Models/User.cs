using System;
using System.Collections.Generic;

namespace FinanceTracker.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public bool Isactive { get; set; }

    public DateOnly Createdat { get; set; }

    public string? Emailverificationtoken { get; set; }

    public DateTime? Emailverificationexpiresat { get; set; }

    public bool Emailverified { get; set; }

    public string? Refreshtokenhash { get; set; }

    public DateTime? Refreshtokenexpiresat { get; set; }

    public DateTime? Refreshtokenrevokedat { get; set; }

    public virtual ICollection<Blockeduser> BlockeduserTargetusers { get; set; } = new List<Blockeduser>();

    public virtual ICollection<Blockeduser> BlockeduserUsers { get; set; } = new List<Blockeduser>();

    public virtual ICollection<Bugreport> Bugreports { get; set; } = new List<Bugreport>();

    public virtual ICollection<Chatmessage> ChatmessageReceiverusers { get; set; } = new List<Chatmessage>();

    public virtual ICollection<Chatmessage> ChatmessageSenderusers { get; set; } = new List<Chatmessage>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<Friendship> FriendshipFriendusers { get; set; } = new List<Friendship>();

    public virtual ICollection<Friendship> FriendshipUsers { get; set; } = new List<Friendship>();

    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual ICollection<Userwallet> Userwallets { get; set; } = new List<Userwallet>();
}
