using Microsoft.EntityFrameworkCore;
using System;
using Transactions.API.Models;

namespace Transactions.API.Db
{
    public class TransactionsDbContext : DbContext
    {
        public DbSet<Transactions.API.Models.User> Users { get; set; }
        public DbSet<Transactions.API.Models.Account> Accounts { get; set; }
        public DbSet<Transactions.API.Models.Card> Cards { get; set; }
        public DbSet<Transactions.API.Models.Category> Categories { get; set; }
        public DbSet<Transactions.API.Models.Transaction> Transactions { get; set; }   
        public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transactions.API.Models.User>()
                .HasOne(u => u.Account)
                .WithOne(a => a.User)
                .HasForeignKey<Account>(u => u.UserId);

            modelBuilder.Entity<Transactions.API.Models.Card>()
                .HasOne(c => c.Account)
                .WithMany(a => a.Cards)
                .HasForeignKey(c => c.AccountId);

            modelBuilder.Entity<Transactions.API.Models.Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .IsRequired(false);

            modelBuilder.Entity<Transactions.API.Models.Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId);

            modelBuilder.Entity<Transactions.API.Models.Transaction>()
                .HasOne(t => t.Category)
                .WithOne(c => c.Transaction)
                .HasForeignKey<Category>(t => t.CategoryId);
        }
    }
}
