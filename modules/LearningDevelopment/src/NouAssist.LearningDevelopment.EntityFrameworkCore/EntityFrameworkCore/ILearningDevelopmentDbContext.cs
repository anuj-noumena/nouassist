using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore;

[ConnectionStringName(LearningDevelopmentDbProperties.ConnectionStringName)]
public interface ILearningDevelopmentDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    // Integration
    DbSet<LearningPath> LearningPaths { get; }
    DbSet<LearningActivity> LearningActivities { get; }
    
    // Learning
    DbSet<Topic> Topics { get; }
    DbSet<LearningSession> LearningSessions { get; }
    DbSet<LearningNote> LearningNotes { get; }
    DbSet<Mindmap> Mindmaps { get; }
    DbSet<BookReadingHistory> BookReadingHistories { get; }
    
    // Skill
    DbSet<Skill> Skills { get; }
    DbSet<SkillDevelopmentSession> SkillDevelopmentSessions { get; }
    DbSet<SkillLog> SkillLogs { get; }
    DbSet<SkillTree> SkillTrees { get; }
    DbSet<ResourceHistory> ResourceHistories { get; }
}
