using Microsoft.EntityFrameworkCore;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Core.EntityFrameworkCore
{
    [ConnectionStringName(CoreDbProperties.ConnectionStringName)]
    public class CoreDbContext : AbpDbContext<CoreDbContext>, ICoreDbContext
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagCategory> TagCategories { get; set; }
        public DbSet<EntityTag> EntityTags { get; set; }
        public DbSet<Context> Contexts { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<InboxItem> InboxItems { get; set; }
        public DbSet<LogActivity> LogActivities { get; set; }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureCore(this);
        }
    }
}
