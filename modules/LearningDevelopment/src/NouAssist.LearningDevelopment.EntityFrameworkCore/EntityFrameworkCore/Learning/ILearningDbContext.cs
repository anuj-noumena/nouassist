using Microsoft.EntityFrameworkCore;
using NouAssist.LearningDevelopment.Learning.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.LearningDevelopment.EntityFrameworkCore.Learning
{
    [ConnectionStringName(LearningDbProperties.ConnectionStringName)]
    public interface ILearningDbContext : IEfCoreDbContext
    {
        DbSet<Topic> Topics { get; }
        DbSet<LearningSession> LearningSessions { get; }
        DbSet<LearningNote> LearningNotes { get; }
        DbSet<Mindmap> Mindmaps { get; }
        DbSet<BookReadingHistory> BookReadingHistories { get; }
    }
}
