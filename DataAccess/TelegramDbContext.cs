using Core.Entities;
using DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class TelegramDbContext : DbContext
    {
        public DbSet<TelegramUser> TelegramUser { get; set; }
        public TelegramDbContext(DbContextOptions<TelegramDbContext> options) : base(options){}
     
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TelegramUserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
