using CashDesk.Model;
using Microsoft.EntityFrameworkCore;

namespace CashDesk
{
    class MyContext : DbContext
    {
        public DbSet<Member> Members { get; set; }
        //public DbSet<Deposit> Deposits { get; set; }
        //public DbSet<Membership> Memberships { get; set; }
        //public DbSet<DepositStatistics> DepStats { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasIndex(i => i.LastName)
                .IsUnique();
            //modelBuilder.Entity<Deposit>();
            //modelBuilder.Entity<Membership>();
            //modelBuilder.Entity<DepositStatistics>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0");
            }
        }
    }
}
