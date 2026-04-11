using Microsoft.EntityFrameworkCore;
using StudyWebAPI.Domain.Entities;

namespace StudyWebAPI.Infrastructure.Data
{
    public class RemarkDbContext : DbContext
    {
        public DbSet<RemarkEntity> Remarks => Set <RemarkEntity>();
        public RemarkDbContext(DbContextOptions<RemarkDbContext> options) : base(options) { }
        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating (modelBuilder);

            modelBuilder.Entity<RemarkEntity>()
                .HasKey(r => r.Id);


            modelBuilder.Entity<RemarkEntity>()
                .HasIndex(r => r.Title)
                .HasMethod("GIN")
                .IsTsVectorExpressionIndex("russian");
        }
    }
}
