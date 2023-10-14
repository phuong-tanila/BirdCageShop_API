using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObjects.Models
{
    public partial class BirdCageShopContext : IdentityDbContext<Account>
    {
        public BirdCageShopContext()
        {
        }

        //public BirdCageShopContext(DbContextOptions<BirdCageShopContext> options)
        //    : base(options)
        //{
        //}

        //public virtual DbSet<Account> Accounts { get; set; } = null!;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=BirdCageShop;User=sa;Password=12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Account>(entity =>
            //{
            //    entity.ToTable("Account");

            //    entity.HasIndex(e => e.PhoneNumber, "UQ__Account__17A35CA435B48A95")
            //        .IsUnique();

            //    entity.Property(e => e.Id)
            //        .HasMaxLength(32)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Password)
            //        .HasMaxLength(20)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PhoneNumber)
            //        .HasMaxLength(10)
            //        .IsUnicode(false)
            //        .HasColumnName("Phone_Number");

            //    entity.Property(e => e.Role)
            //        .HasMaxLength(2)
            //        .IsUnicode(false);
            //});

            modelBuilder.Entity<Cage>(entity =>
            {
                entity.ToTable("Cage");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Create_Date");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .IsUnicode(false)
                    .HasColumnName("Image_Path");

                entity.Property(e => e.InStock).HasColumnName("In_Stock");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

          

            modelBuilder.Entity<CageComponent>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ComponentId })
                    .HasName("PK__Cage_Com__8454509EA6C724D8");

                entity.ToTable("Cage_Component");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ComponentId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cage)
                    .WithMany(p => p.CageComponents)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKCage_Compo677311");

                entity.HasOne(d => d.Component)
                    .WithMany(p => p.CageComponents)
                    .HasForeignKey(d => d.ComponentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKCage_Compo294962");
            });

            modelBuilder.Entity<Component>(entity =>
            {
                entity.ToTable("Component");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .IsUnicode(false)
                    .HasColumnName("Image_Path");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.AccountId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.BirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Birth_Date");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Last_Name");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKCustomer95429");
            });

            modelBuilder.Entity<FeedbackAttachment>(entity =>
            {
                entity.ToTable("Feedback_Attachment");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.OrderDetailId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Path).IsUnicode(false);

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.FeedbackAttachments)
                    .HasForeignKey(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKFeedback_A30837");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CageId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ImagePath)
                    .IsUnicode(false)
                    .HasColumnName("Image_Path");

                entity.HasOne(d => d.Cage)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.CageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKImage760268");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.CustomerId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Delivery_Date");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Order_Date");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Payment_Date");

                entity.Property(e => e.ShipFee).HasColumnName("Ship_Fee");

                entity.Property(e => e.VoucherId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKOrder556775");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FKOrder661550");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.ToTable("OrderDetail");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.CageId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.Content).IsUnicode(false);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.PostDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Post_Date");

                entity.HasOne(d => d.Cage)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.CageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKOrderDetai237898");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FKOrderDetai762072");
            });

            modelBuilder.Entity<SmsOtp>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__sms_otp__AEE3543598D1704F");

                entity.ToTable("Sms_otp");

                entity.Property(e => e.Id).HasColumnName("otp_id");

                entity.Property(e => e.CreateAt)
                    .HasPrecision(6)
                    .HasColumnName("create_at");

                entity.Property(e => e.ExpiredAt)
                    .HasPrecision(6)
                    .HasColumnName("expired_at");

                entity.Property(e => e.OtpValue)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("otp_value");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("phone_number");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.Id)
                    .HasMaxLength(32)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionPoint).HasColumnName("Condition_Point");

                entity.Property(e => e.EffectiveDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Effective_Date");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Expiration_Date");

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
