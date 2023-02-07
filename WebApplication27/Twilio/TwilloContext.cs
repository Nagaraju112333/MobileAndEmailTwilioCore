using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApplication27.Twilio;

public partial class TwilloContext : DbContext
{
    public TwilloContext()
    {
    }
    public TwilloContext(DbContextOptions<TwilloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserRegister> UserRegisters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRegister>(entity =>
        {
            entity.HasKey(e => e.RegisterId).HasName("PK__UserRegi__B91FAB798E5FF159");

            entity.ToTable("UserRegister");

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Otp).HasColumnName("OTP");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("Phone_Number");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
