using System;
using System.Collections.Generic;
using FinanceTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Data;

public partial class FinanceTrackerDbContext : DbContext
{
    public FinanceTrackerDbContext()
    {
    }

    public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Blockeduser> Blockedusers { get; set; }

    public virtual DbSet<Bugreport> Bugreports { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Chatmessage> Chatmessages { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Incomesource> Incomesources { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userwallet> Userwallets { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<Walletgoal> Walletgoals { get; set; }

    public virtual DbSet<Walletrole> Walletroles { get; set; }

 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blockeduser>(entity =>
        {
            entity.HasKey(e => e.Blockeduserid).HasName("blockedusers_pkey");

            entity.ToTable("blockedusers");

            entity.HasIndex(e => e.Targetuserid, "ix_blockedusers_targetuserid");

            entity.HasIndex(e => e.Userid, "ix_blockedusers_userid");

            entity.HasIndex(e => new { e.Userid, e.Targetuserid }, "uq_blockedusers_userblocked").IsUnique();

            entity.Property(e => e.Blockeduserid).HasColumnName("blockeduserid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Targetuserid).HasColumnName("targetuserid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Targetuser).WithMany(p => p.BlockeduserTargetusers)
                .HasForeignKey(d => d.Targetuserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("blockedusers_targetuserid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.BlockeduserUsers)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("blockedusers_userid_fkey");
        });

        modelBuilder.Entity<Bugreport>(entity =>
        {
            entity.HasKey(e => e.Bugreportid).HasName("bugreports_pkey");

            entity.ToTable("bugreports");

            entity.HasIndex(e => e.Status, "ix_bugreports_status");

            entity.Property(e => e.Bugreportid).HasColumnName("bugreportid");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Status)
                .HasDefaultValue(false)
                .HasColumnName("status");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Bugreports)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bugreports_userid_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Walletid, "ix_categories_walletid");

            entity.HasIndex(e => new { e.Walletid, e.Categoryname }, "uq_categories_walletname").IsUnique();

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(70)
                .HasColumnName("categoryname");
            entity.Property(e => e.Colorhex)
                .HasMaxLength(7)
                .HasDefaultValueSql("'#000000'::bpchar")
                .IsFixedLength()
                .HasColumnName("colorhex");
            entity.Property(e => e.Iconname)
                .HasMaxLength(50)
                .HasColumnName("iconname");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Walletid).HasColumnName("walletid");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Categories)
                .HasForeignKey(d => d.Walletid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("categories_walletid_fkey");
        });

        modelBuilder.Entity<Chatmessage>(entity =>
        {
            entity.HasKey(e => e.Messageid).HasName("chatmessages_pkey");

            entity.ToTable("chatmessages");

            entity.HasIndex(e => new { e.Senderuserid, e.Receiveruserid, e.Createdat }, "ix_chatmessages_conversation");

            entity.HasIndex(e => e.Isread, "ix_chatmessages_isread");

            entity.Property(e => e.Messageid).HasColumnName("messageid");
            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Isread)
                .HasDefaultValue(false)
                .HasColumnName("isread");
            entity.Property(e => e.Receiveruserid).HasColumnName("receiveruserid");
            entity.Property(e => e.Senderuserid).HasColumnName("senderuserid");

            entity.HasOne(d => d.Receiveruser).WithMany(p => p.ChatmessageReceiverusers)
                .HasForeignKey(d => d.Receiveruserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chatmessages_receiveruserid_fkey");

            entity.HasOne(d => d.Senderuser).WithMany(p => p.ChatmessageSenderusers)
                .HasForeignKey(d => d.Senderuserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chatmessages_senderuserid_fkey");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Expenseid).HasName("expenses_pkey");

            entity.ToTable("expenses");

            entity.HasIndex(e => new { e.Walletid, e.Categoryid, e.Userid }, "ix_expenses_wallet_category");

            entity.HasIndex(e => new { e.Walletid, e.Userid }, "ix_expenses_wallet_user");

            entity.Property(e => e.Expenseid).HasColumnName("expenseid");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Walletid).HasColumnName("walletid");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("expenses_categoryid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("expenses_userid_fkey");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.Walletid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("expenses_walletid_fkey");
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.Friendshipid).HasName("friendships_pkey");

            entity.ToTable("friendships");

            entity.HasIndex(e => e.Frienduserid, "ix_friends_frienduserid");

            entity.HasIndex(e => e.Status, "ix_friends_status");

            entity.HasIndex(e => new { e.Userid, e.Status }, "ix_friends_userid_status");

            entity.HasIndex(e => new { e.Userid, e.Frienduserid }, "uq_friendships").IsUnique();

            entity.Property(e => e.Friendshipid).HasColumnName("friendshipid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Frienduserid).HasColumnName("frienduserid");
            entity.Property(e => e.Status)
                .HasDefaultValue((short)0)
                .HasColumnName("status");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Frienduser).WithMany(p => p.FriendshipFriendusers)
                .HasForeignKey(d => d.Frienduserid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendships_frienduserid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FriendshipUsers)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("friendships_userid_fkey");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Incomeid).HasName("income_pkey");

            entity.ToTable("income");

            entity.HasIndex(e => e.Incomesourceid, "ix_income_incomesourceid");

            entity.HasIndex(e => e.Userid, "ix_income_user");

            entity.HasIndex(e => new { e.Walletid, e.Userid }, "ix_income_wallet_user");

            entity.Property(e => e.Incomeid).HasColumnName("incomeid");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Incomesourceid).HasColumnName("incomesourceid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Walletid).HasColumnName("walletid");

            entity.HasOne(d => d.Incomesource).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.Incomesourceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("income_incomesourceid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("income_userid_fkey");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.Walletid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("income_walletid_fkey");
        });

        modelBuilder.Entity<Incomesource>(entity =>
        {
            entity.HasKey(e => e.Incomesourceid).HasName("incomesources_pkey");

            entity.ToTable("incomesources");

            entity.HasIndex(e => e.Incomesourceid, "ix_incomesource_incomesourceid");

            entity.HasIndex(e => e.Walletid, "ix_incomesources_walletid");

            entity.Property(e => e.Incomesourceid).HasColumnName("incomesourceid");
            entity.Property(e => e.Incomesourcename)
                .HasMaxLength(50)
                .HasColumnName("incomesourcename");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Walletid).HasColumnName("walletid");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Incomesources)
                .HasForeignKey(d => d.Walletid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("incomesources_walletid_fkey");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Logid).HasName("logs_pkey");

            entity.ToTable("logs");

            entity.HasIndex(e => e.Createdat, "ix_logs_createdat");

            entity.HasIndex(e => new { e.Endpoint, e.Httpmethod }, "ix_logs_endpoint_method");

            entity.HasIndex(e => new { e.Endpoint, e.Statuscode }, "ix_logs_endpoint_statuscode");

            entity.HasIndex(e => e.Securitylevel, "ix_logs_securitylevel");

            entity.HasIndex(e => e.Statuscode, "ix_logs_statuscode");

            entity.HasIndex(e => e.Userid, "ix_logs_userid");

            entity.HasIndex(e => new { e.Userid, e.Action }, "ix_logs_userid_action");

            entity.Property(e => e.Logid).HasColumnName("logid");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .HasColumnName("action");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Endpoint)
                .HasMaxLength(255)
                .HasColumnName("endpoint");
            entity.Property(e => e.Httpmethod)
                .HasMaxLength(10)
                .HasColumnName("httpmethod");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .HasColumnName("ipaddress");
            entity.Property(e => e.Securitylevel).HasColumnName("securitylevel");
            entity.Property(e => e.Statuscode).HasColumnName("statuscode");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("logs_userid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Username, "users_username_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasColumnName("email");
            entity.Property(e => e.Emailverificationexpiresat).HasColumnName("emailverificationexpiresat");
            entity.Property(e => e.Emailverificationtoken)
                .HasMaxLength(255)
                .HasColumnName("emailverificationtoken");
            entity.Property(e => e.Emailverified)
                .HasDefaultValue(false)
                .HasColumnName("emailverified");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Passwordhash)
                .HasMaxLength(255)
                .HasColumnName("passwordhash");
            entity.Property(e => e.Refreshtokenexpiresat).HasColumnName("refreshtokenexpiresat");
            entity.Property(e => e.Refreshtokenhash)
                .HasMaxLength(255)
                .HasColumnName("refreshtokenhash");
            entity.Property(e => e.Refreshtokenrevokedat).HasColumnName("refreshtokenrevokedat");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Userwallet>(entity =>
        {
            entity.HasKey(e => e.Userwalletid).HasName("userwallets_pkey");

            entity.ToTable("userwallets");

            entity.HasIndex(e => e.Userid, "ix_userwallets_userid");

            entity.HasIndex(e => e.Walletid, "ix_userwallets_walletid");

            entity.HasIndex(e => e.Walletroleid, "ix_userwallets_walletroleid");

            entity.HasIndex(e => new { e.Userid, e.Walletid }, "uq_userwallets").IsUnique();

            entity.Property(e => e.Userwalletid).HasColumnName("userwalletid");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Walletid).HasColumnName("walletid");
            entity.Property(e => e.Walletroleid)
                .HasDefaultValue((short)1)
                .HasColumnName("walletroleid");

            entity.HasOne(d => d.User).WithMany(p => p.Userwallets)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userwallets_userid_fkey");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Userwallets)
                .HasForeignKey(d => d.Walletid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userwallets_walletid_fkey");

            entity.HasOne(d => d.Walletrole).WithMany(p => p.Userwallets)
                .HasForeignKey(d => d.Walletroleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("userwallets_walletroleid_fkey");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Walletid).HasName("wallets_pkey");

            entity.ToTable("wallets");

            entity.Property(e => e.Walletid).HasColumnName("walletid");
            entity.Property(e => e.Availablebalance)
                .HasPrecision(18, 2)
                .HasColumnName("availablebalance");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Savedbalance)
                .HasPrecision(18, 2)
                .HasColumnName("savedbalance");
            entity.Property(e => e.Walletname)
                .HasMaxLength(50)
                .HasColumnName("walletname");
        });

        modelBuilder.Entity<Walletgoal>(entity =>
        {
            entity.HasKey(e => e.Walletgoalid).HasName("walletgoals_pkey");

            entity.ToTable("walletgoals");

            entity.HasIndex(e => e.Walletid, "ix_walletgoals_walletid");

            entity.Property(e => e.Walletgoalid).HasColumnName("walletgoalid");
            entity.Property(e => e.Allocatedamount)
                .HasPrecision(18, 2)
                .HasColumnName("allocatedamount");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Goalimageurl)
                .HasMaxLength(500)
                .HasColumnName("goalimageurl");
            entity.Property(e => e.Goalname)
                .HasMaxLength(50)
                .HasColumnName("goalname");
            entity.Property(e => e.Iscompleted)
                .HasDefaultValue(false)
                .HasColumnName("iscompleted");
            entity.Property(e => e.Priority)
                .HasDefaultValue((short)1)
                .HasColumnName("priority");
            entity.Property(e => e.Targetamount)
                .HasPrecision(18, 2)
                .HasColumnName("targetamount");
            entity.Property(e => e.Targetdate).HasColumnName("targetdate");
            entity.Property(e => e.Walletid).HasColumnName("walletid");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Walletgoals)
                .HasForeignKey(d => d.Walletid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("walletgoals_walletid_fkey");
        });

        modelBuilder.Entity<Walletrole>(entity =>
        {
            entity.HasKey(e => e.Walletroleid).HasName("walletroles_pkey");

            entity.ToTable("walletroles");

            entity.HasIndex(e => e.Rolename, "walletroles_rolename_key").IsUnique();

            entity.Property(e => e.Walletroleid).HasColumnName("walletroleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
