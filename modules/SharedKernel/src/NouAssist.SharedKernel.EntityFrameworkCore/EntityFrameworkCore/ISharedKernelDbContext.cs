using Microsoft.EntityFrameworkCore;
using NouAssist.SharedKernel.Aggregates;
using NouAssist.SharedKernel.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SharedKernel.EntityFrameworkCore;

[ConnectionStringName(SharedKernelDbProperties.ConnectionStringName)]
public interface ISharedKernelDbContext : IEfCoreDbContext
{
    DbSet<Tag> Tags { get; }
    DbSet<TagCategory> TagCategories { get; }
    DbSet<EntityTag> EntityTags { get; }
    DbSet<Context> Contexts { get; }
    DbSet<Area> Areas { get; }
    DbSet<InboxItem> InboxItems { get; }
    DbSet<LogActivity> LogActivities { get; }
}
