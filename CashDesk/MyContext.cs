using CashDesk.Model;
using Microsoft.EntityFrameworkCore;

namespace CashDesk
{
    public class MyContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Deposit> Deposits { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasIndex(i => i.LastName)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // why am I not supposed to check this? 
            //if (!optionsBuilder.IsConfigured)
            //{
                optionsBuilder.UseInMemoryDatabase("DatabaseCashDesk");
            //}
        }
    }
}
