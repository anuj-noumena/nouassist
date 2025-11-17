using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.SkillDevelopment
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
