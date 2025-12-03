using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Coding_Tracker.Infrastructure
{
    public class RecordDbContext : DbContext
    {
        private readonly ConfigurationManager config = new ConfigManager().Config;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(config["ConnectionStrings:DefaultConnection"]);
        }
        public DbSet<CodingSession> Sessions { get; set; }
    }
}