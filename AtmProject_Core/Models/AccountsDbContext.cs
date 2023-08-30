using Microsoft.EntityFrameworkCore;

namespace AtmProject_Core.Models
{
    public class AccountsDbContext:DbContext
    {
        public AccountsDbContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Accounts>().ToTable(tb => tb.HasTrigger("trig_Transactions"));
        }

    }
}
