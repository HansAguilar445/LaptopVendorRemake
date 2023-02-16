using System;
using System.Collections.Generic;
using LaptopVendorRemake.Models;
using Microsoft.EntityFrameworkCore;

namespace LaptopVendorRemake.Data;

public partial class LaptopVendorContext : DbContext
{
    public LaptopVendorContext()
    {
    }

    public LaptopVendorContext(DbContextOptions<LaptopVendorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Laptop> Laptops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=LaptopVendor;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Laptop>(entity =>
        {
            entity.HasIndex(e => e.BrandId, "IX_Laptops_BrandId");

            entity.HasOne(d => d.Brand).WithMany(p => p.Laptops).HasForeignKey(d => d.BrandId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
