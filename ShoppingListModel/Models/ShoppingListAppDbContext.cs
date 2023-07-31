using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoppingListModel.Models;

public partial class ShoppingListAppDbContext : DbContext
{
    public ShoppingListAppDbContext()
    {
    }

    public ShoppingListAppDbContext(DbContextOptions<ShoppingListAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    public virtual DbSet<ShoppingListDetail> ShoppingListDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ShoppingListAppDb;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasDefaultValueSql("(N'No Category')");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.HasIndex(e => e.CategoryId, "IX_Product_CategoryId");

            entity.Property(e => e.Name).HasMaxLength(30);

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasForeignKey(d => d.CategoryId);
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.ToTable("ShoppingList");

            entity.HasIndex(e => e.UserId, "IX_ShoppingList_UserId");

            entity.Property(e => e.ShoppingListName).HasMaxLength(30);

            entity.HasOne(d => d.User).WithMany(p => p.ShoppingLists).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ShoppingListDetail>(entity =>
        {
            entity.HasKey(e => new { e.ShoppingListId, e.ProductId });

            entity.HasIndex(e => e.ProductId, "IX_ShoppingListDetails_ProductId");

            entity.Property(e => e.Note).HasMaxLength(100);

            entity.HasOne(d => d.Product).WithMany(p => p.ShoppingListDetails).HasForeignKey(d => d.ProductId);

            entity.HasOne(d => d.ShoppingList).WithMany(p => p.ShoppingListDetails).HasForeignKey(d => d.ShoppingListId);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.IsAdmin)
                .IsRequired()
                .HasDefaultValueSql("(CONVERT([bit],(0)))");
            entity.Property(e => e.Name).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(128);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
