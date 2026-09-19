using InsuranceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace InsuranceApp.DataAccess
{
    public class InsuranceAppDbContext: DbContext
    {
        public DbSet<Claim> Claims { get; init; }
        public DbSet<Cover> Covers { get; init; }

        public InsuranceAppDbContext(DbContextOptions options)
            : base(options)
        {
            Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
            //this line is a workaround so i can use the standalone mongodb for the integration test
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Claim>().ToCollection("claims");
            modelBuilder.Entity<Cover>().ToCollection("covers");
        }
    }
}
