using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Integration.Aggregates;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using NouAssist.LearningDevelopment.SkillDevelopment.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore;

[ConnectionStringName(LearningDevelopmentDbProperties.ConnectionStringName)]
public class LearningDevelopmentDbContext : AbpDbContext<LearningDevelopmentDbContext>, ILearningDevelopmentDbContext
{
    // Learning
    public DbSet<Topic> Topics { get; set; }
    public DbSet<LearningSession> LearningSessions { get; set; }
    public DbSet<LearningNote> LearningNotes { get; set; }
    public DbSet<Mindmap> Mindmaps { get; set; }
    public DbSet<BookReadingHistory> BookReadingHistories { get; set; }
    // Skill
    public DbSet<Skill> Skills { get; set; }
    public DbSet<SkillDevelopmentSession> SkillDevelopmentSessions { get; set; }
    public DbSet<SkillLog> SkillLogs { get; set; }
    public DbSet<SkillTree> SkillTrees { get; set; }
    public DbSet<ResourceHistory> ResourceHistories { get; set; }
    // Integration
    public DbSet<LearningPath> LearningPaths { get; set; }
    public DbSet<LearningActivity> LearningActivities { get; set; }

    public LearningDevelopmentDbContext(DbContextOptions<LearningDevelopmentDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureLearningDevelopment();
    }
}
