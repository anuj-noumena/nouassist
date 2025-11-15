using Microsoft.EntityFrameworkCore;
using NouAssist.SkillDevelopment.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.SkillDevelopment.EntityFrameworkCore
{
    [ConnectionStringName(SkillDevelopmentDbProperties.ConnectionStringName)]
    public interface ISkillDevelopmentDbContext : IEfCoreDbContext
    {
        DbSet<Skill> Skills { get; }
        DbSet<SkillDevelopmentSession> SkillDevelopmentSessions { get; }
        DbSet<SkillLog> SkillLogs { get; }
        DbSet<SkillTree> SkillTrees { get; }
        DbSet<ResourceHistory> ResourceHistories { get; }
    }
}
