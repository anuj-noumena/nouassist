using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Integration.Aggregates;
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
}
