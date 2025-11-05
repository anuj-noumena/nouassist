using Microsoft.EntityFrameworkCore;
using NouAssist.Meetings.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Meetings.EntityFrameworkCore
{
    [ConnectionStringName(MeetingsDbProperties.ConnectionStringName)]
    public interface IMeetingsDbContext : IEfCoreDbContext
    {
        DbSet<Meeting> Meetings { get; }
    }
}
