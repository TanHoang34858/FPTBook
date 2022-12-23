using System;
using System.Collections.Generic;
using System.Text;
using IBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IBook.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
             : base(options)
        {
        }
        public DbSet<OrderBookDetail> OrderBookDetails { get; set; }
        public DbSet<OrderBook> OrderBooks { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            #region SachNhaXuatBan
            //modelBuilder.Entity<BookPublisher>()
            //    .HasKey(x => new { x.BookID, x.PublisherID });
            //modelBuilder.Entity<BookPublisher>()
            //    .HasOne(x => x.Book)
            //    .WithMany(x => x.BookPublishers)
            //    .HasForeignKey(x => x.BookID);
            //modelBuilder.Entity<BookPublisher>()
            //    .HasOne(x => x.Publisher)
            //    .WithMany(x => x.BookPublishers)
            //    .HasForeignKey(x => x.PublisherID);
            #endregion
            #region OrderBookDetail
            modelBuilder.Entity<OrderBookDetail>()
                .HasKey(x => new { x.OrderBookID, x.BookID });
            modelBuilder.Entity<OrderBookDetail>()
                .HasOne(x => x.Book)
                .WithMany(x => x.OrderBookDetails)
                .HasForeignKey(x => x.BookID);
            modelBuilder.Entity<OrderBookDetail>()
                .HasOne(x => x.OrderBook)
                .WithMany(x => x.OrderBookDetails)
                .HasForeignKey(x => x.OrderBookID);
            #endregion
            modelBuilder.Entity<Author>()
                .HasKey(x => x.ID);
            modelBuilder.Entity<Book>()
                .HasOne(x => x.Author)
                .WithMany(x => x.Books);
            modelBuilder.Entity<Category>()
                .HasKey(x => x.ID);
            modelBuilder.Entity<Book>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Books);
            modelBuilder.Entity<OrderBook>()
                .HasKey(x => x.OrderID);
            modelBuilder.Entity<Book>()
                .HasMany(x => x.OrderBookDetails)
                .WithOne(x => x.Book);
            modelBuilder.Entity<OrderBook>()
                .HasMany(x => x.OrderBookDetails)
                .WithOne(x => x.OrderBook);
            modelBuilder.Entity<Book>()
                .HasOne(x => x.Publisher)
                .WithMany(x => x.Books);
        }
    }
}
