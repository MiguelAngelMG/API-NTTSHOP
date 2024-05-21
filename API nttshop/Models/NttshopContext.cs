using System;
using System.Collections.Generic;
using API_nttshop.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API_nttshop.Models;

public partial class NttshopContext : DbContext
{
    public NttshopContext()
    {
    }

    public NttshopContext(DbContextOptions<NttshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ManagementUser> ManagementUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=NTTD-BM8GN73\\SQLEXPRESS;user=mmargime;password=miguel;database=nttshop; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ManagementUser>(entity =>
        {
            entity.HasKey(e => e.PkUser).HasName("PK__Manageme__334B06A95BB6E0E4");

            entity.HasIndex(e => e.Login, "UQ__Manageme__5E55825B3EF73519").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Manageme__A9D1053410A898EA").IsUnique();

            entity.Property(e => e.PkUser).HasColumnName("PK_USER");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Languages)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Surname2)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.PkUser).HasName("PK__Users__334B06A9C3F902A6");

            entity.HasIndex(e => e.Login, "UQ__Users__5E55825BE1DBDA95").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105344D9A8400").IsUnique();

            entity.Property(e => e.PkUser).HasColumnName("PK_USER");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Languages)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Province)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Surname1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Surname2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Town)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
