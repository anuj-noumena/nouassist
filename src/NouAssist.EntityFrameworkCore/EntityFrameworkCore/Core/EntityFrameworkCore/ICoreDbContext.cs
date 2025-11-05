using Microsoft.EntityFrameworkCore;
using NouAssist.Core.Aggregates;
using NouAssist.Core.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Core.EntityFrameworkCore
{
    [ConnectionStringName(CoreDbProperties.ConnectionStringName)]
    public interface ICoreDbContext : IEfCoreDbContext
    {
        DbSet<Tag> Tags { get; }
        DbSet<TagCategory> TagCategories { get; }
        DbSet<EntityTag> EntityTags { get; }
        DbSet<Context> Contexts { get; }
        DbSet<Area> Areas { get; }
        DbSet<InboxItem> InboxItems { get; }
        DbSet<LogActivity> LogActivities { get; }
    }
}
