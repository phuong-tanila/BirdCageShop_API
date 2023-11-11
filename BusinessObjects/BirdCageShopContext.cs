using System;
using System.Collections.Generic;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects
{
    public partial class BirdCageShopContext : IdentityDbContext<Account>
    {
        public BirdCageShopContext(){}

        //public BirdCageShopContext(DbContextOptions<BirdCageShopContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<Cage> Cages { get; set; } = null!;
        public virtual DbSet<CageComponent> CageComponents { get; set; } = null!;
        public virtual DbSet<Component> Components { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<FeedbackAttachment> FeedbackAttachments { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<SmsOtp> SmsOtps { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;

        public string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var strConn = config["ConnectionStrings:DB"];
            return strConn!;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("server =(local);database=BirdCageShop;uid=sa;pwd=12345;TrustServerCertificate=True");
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cage>(entity =>
            {
                entity.ToTable("Cage");

                entity.HasKey(e => e.Id)
                    .HasName("PK_Cage");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(1000)
                    .IsRequired();

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired();

                entity.Property(e => e.InStock)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(e => e.CustomerDesign)
                    .WithMany(e => e.CustomCages)
                    .HasForeignKey(e => e.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cage_Customer")
                    .IsRequired(false);
            });



            modelBuilder.Entity<CageComponent>(entity =>
            {
                entity.ToTable("CageComponent");

                entity.HasKey(e => e.Id)
                    .HasName("PK_CageComponent");

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.HasOne(e => e.Cage)
                    .WithMany(e => e.CageComponents)
                    .HasForeignKey(e => e.CageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CageComponent_Cage");

                entity.HasOne(e => e.Component)
                    .WithMany(e => e.CageComponents)
                    .HasForeignKey(e => e.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CageComponent_Component");
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.ToTable("Component");

                entity.HasKey(e => e.Id)
                    .HasName("PK_Component");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();
                
                entity.Property(e => e.Type)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.Price)
                    .IsRequired();
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasKey(e => e.Id)
                    .HasName("PK_Customer");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Account");
            });

            modelBuilder.Entity<FeedbackAttachment>(entity =>
            {
                entity.ToTable("FeedbackAttachment");

                entity.HasKey(e => e.Id)
                   .HasName("PK_FeedbackAttachment");

                entity.Property(e => e.Path)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.FeedbackAttachments)
                    .HasForeignKey(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FeedbackAttachment_OrderDetail");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.HasKey(e => e.Id)
                   .HasName("PK_Image");

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .IsRequired();

                entity.HasOne(d => d.Cage)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.CageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Image_Cage");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.HasKey(e => e.Id)
                   .HasName("PK_Order");

                entity.Property(e => e.Address)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .IsRequired();

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .IsRequired();

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Customer");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VoucherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Voucher");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.HasKey(e => e.Id)
                   .HasName("PK_OrderDetail");

                entity.Property(e => e.Price)
                    .IsRequired();

                entity.Property(e => e.Quantity)
                    .IsRequired();

                entity.Property(e => e.Content)
                    .HasMaxLength(100);

                entity.Property(e => e.PostDate)
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Cage)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.CageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetai_Cage")
                    .IsRequired();

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetai_Order");
            });

            modelBuilder.Entity<SmsOtp>(entity =>
            {
                entity.ToTable("SmsOtp");

                entity.HasKey(e => e.Id)
                    .HasName("PK_SMS_OTP");

                entity.Property(e => e.CreateAt)
                    .HasPrecision(6)
                    .HasColumnName("CreateAt");

                entity.Property(e => e.ExpiredAt)
                    .HasPrecision(6)
                    .HasColumnName("ExpiredAt");

                entity.Property(e => e.OtpValue)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("OtpValue");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PhoneNumber");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.HasKey(e => e.Id)
                    .HasName("PK_Voucher");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime");

                entity.Property(e => e.Discount)
                    .IsRequired();

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
