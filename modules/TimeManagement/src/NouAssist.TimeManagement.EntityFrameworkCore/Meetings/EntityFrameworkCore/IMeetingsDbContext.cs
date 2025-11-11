using Microsoft.EntityFrameworkCore;
using NouAssist.TimeManagement.Meetings.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TimeManagement.Meetings.EntityFrameworkCore
{
    [ConnectionStringName(MeetingsDbProperties.ConnectionStringName)]
    public interface IMeetingsDbContext : IEfCoreDbContext
    {
        DbSet<Meeting> Meetings { get; }
    }
}
