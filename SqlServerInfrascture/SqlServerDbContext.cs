using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace SqlServerInfrascture
{
    public partial class SqlServerDbContext : DbContext
    {
        //public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<ProductItem> ProductItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //GET DATABASE FILE DIRECTORY
            var parent = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).FullName;
            var path = Path.Combine(parent, @"SqlServerInfrascture\DataBaseFile\ProductsDB.mdf");

            if (!optionsBuilder.IsConfigured)
            {
                string conn = string.Format("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={0};Integrated Security=True;Connect Timeout=30", path);
                optionsBuilder.UseSqlServer(conn);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ProductItem>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ImageUrl).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
