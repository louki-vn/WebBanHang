namespace WebBanHang_API.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Shop : DbContext
    {
        public Shop()
            : base("name=Shop")
        {
        }

        public virtual DbSet<BRAND> BRANDs { get; set; }
        public virtual DbSet<CART> CARTs { get; set; }
        public virtual DbSet<CART_ITEM> CART_ITEM { get; set; }
        public virtual DbSet<CATEGORY> CATEGORies { get; set; }
        public virtual DbSet<CREDENTIAL_S> CREDENTIAL_S { get; set; }
        public virtual DbSet<ITEM_SOLD> ITEM_SOLD { get; set; }
        public virtual DbSet<MEMBER> MEMBERs { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }
        public virtual DbSet<PRODUCT_GROUP> PRODUCT_GROUP { get; set; }
        public virtual DbSet<ROLE_S> ROLE_S { get; set; }
        public virtual DbSet<SALE> SALES { get; set; }
        public virtual DbSet<TRANSACTION> TRANSACTIONs { get; set; }
        public virtual DbSet<USER_GROUP_S> USER_GROUP_S { get; set; }
        public virtual DbSet<REPORT> REPORTs { get; set; }
        public virtual DbSet<REVIEW> REVIEWs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CART_ITEM>()
                .Property(e => e.size)
                .IsUnicode(false);

            modelBuilder.Entity<CATEGORY>()
                .HasMany(e => e.PRODUCTs)
                .WithRequired(e => e.CATEGORY)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CREDENTIAL_S>()
                .Property(e => e.usergroup_id)
                .IsUnicode(false);

            modelBuilder.Entity<CREDENTIAL_S>()
                .Property(e => e.role_id)
                .IsUnicode(false);

            modelBuilder.Entity<ITEM_SOLD>()
                .Property(e => e.size)
                .IsUnicode(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.GroupId)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.size)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT_GROUP>()
                .HasMany(e => e.CATEGORies)
                .WithRequired(e => e.PRODUCT_GROUP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ROLE_S>()
                .Property(e => e.role_id)
                .IsUnicode(false);

            modelBuilder.Entity<ROLE_S>()
                .HasMany(e => e.CREDENTIAL_S)
                .WithRequired(e => e.ROLE_S)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TRANSACTION>()
                .Property(e => e.member_phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<USER_GROUP_S>()
                .Property(e => e.usergroup_id)
                .IsUnicode(false);

            modelBuilder.Entity<USER_GROUP_S>()
                .HasMany(e => e.CREDENTIAL_S)
                .WithRequired(e => e.USER_GROUP_S)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<USER_GROUP_S>()
                .HasMany(e => e.MEMBERs)
                .WithOptional(e => e.USER_GROUP_S)
                .HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<REVIEW>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<REVIEW>()
                .Property(e => e.date_post)
                .IsUnicode(false);
        }
    }
}
