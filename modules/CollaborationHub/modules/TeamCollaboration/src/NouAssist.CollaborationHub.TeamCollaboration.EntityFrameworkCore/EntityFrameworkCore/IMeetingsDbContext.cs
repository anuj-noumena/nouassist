using Microsoft.EntityFrameworkCore;
using NouAssist.CollaborationHub.TeamCollaboration.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CollaborationHub.TeamCollaboration.EntityFrameworkCore
{
    [ConnectionStringName(MeetingsDbProperties.ConnectionStringName)]
    public interface IMeetingsDbContext : IEfCoreDbContext
    {
        DbSet<Meeting> Meetings { get; }
    }
}
