using Microsoft.EntityFrameworkCore;
using SjxLogistics.Models.DatabaseModels;

namespace SjxLogistics.Data
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Riders> Riders { get; set; }
        public DbSet<SjxLogistics.Models.DatabaseModels.Bikes> Bikes { get; set; }
        public DbSet<SjxLogistics.Models.DatabaseModels.Order> Order { get; set; }
        public DbSet<SjxLogistics.Models.DatabaseModels.Notifications> Notifications { get; set; }
        public DbSet<SjxLogistics.Models.DatabaseModels.Audit> Audit { get; set; }
        public DbSet<SjxLogistics.Models.DatabaseModels.Drafts> Drafts { get; set; }
        public DbSet<SjxLogistics.Models.DatabaseModels.NewInfo> NewInfo { get; set; }
    }
}
