using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace p0class.Entities
{
    public partial class MattStringer0Context : DbContext
    {
        public MattStringer0Context()
        {
        }

        public MattStringer0Context(DbContextOptions<MattStringer0Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<LineItem> LineItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<StoreFront> StoreFronts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("pk_customer");

                entity.ToTable("customer");

                entity.Property(e => e.CId).HasColumnName("c_id");

                entity.Property(e => e.CAddr)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("c_addr");

                entity.Property(e => e.CEmail)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("c_email");

                entity.Property(e => e.CName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("c_name");
            });

            modelBuilder.Entity<LineItem>(entity =>
            {
                entity.HasKey(e => e.LId)
                    .HasName("pk_line_items");

                entity.ToTable("line_items");

                entity.Property(e => e.LId).HasColumnName("l_id");

                entity.Property(e => e.LOrder).HasColumnName("l_order");

                entity.Property(e => e.LProd).HasColumnName("l_prod");

                entity.Property(e => e.LQuantity).HasColumnName("l_quantity");

                entity.Property(e => e.LStorefront).HasColumnName("l_storefront");

                entity.HasOne(d => d.LOrderNavigation)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(d => d.LOrder)
                    .HasConstraintName("fk_line_items_orders");

                entity.HasOne(d => d.LProdNavigation)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(d => d.LProd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_line_items_products");

                entity.HasOne(d => d.LStorefrontNavigation)
                    .WithMany(p => p.LineItems)
                    .HasForeignKey(d => d.LStorefront)
                    .HasConstraintName("fk_line_items_store_front");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OId)
                    .HasName("pk_orders");

                entity.ToTable("orders");

                entity.Property(e => e.OId).HasColumnName("o_id");

                entity.Property(e => e.Customer).HasColumnName("customer");

                entity.Property(e => e.OLoc)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("o_loc");

                entity.Property(e => e.OPrice)
                    .HasColumnType("decimal(20, 2)")
                    .HasColumnName("o_price");

                entity.Property(e => e.OStore).HasColumnName("o_store");

                entity.HasOne(d => d.CustomerNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Customer)
                    .HasConstraintName("fk_orders_customer");

                entity.HasOne(d => d.OStoreNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.OStore)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_orders_store_front");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.PId)
                    .HasName("pk_products");

                entity.ToTable("products");

                entity.Property(e => e.PId).HasColumnName("p_id");

                entity.Property(e => e.PCategory)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("p_category");

                entity.Property(e => e.PDesc)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("p_desc");

                entity.Property(e => e.PName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("p_name");

                entity.Property(e => e.PPrice)
                    .HasColumnType("decimal(15, 2)")
                    .HasColumnName("p_price");
            });

            modelBuilder.Entity<StoreFront>(entity =>
            {
                entity.HasKey(e => e.SId)
                    .HasName("pk_store_front");

                entity.ToTable("store_front");

                entity.Property(e => e.SId).HasColumnName("s_id");

                entity.Property(e => e.SAddr)
                    .HasMaxLength(90)
                    .IsUnicode(false)
                    .HasColumnName("s_addr");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
