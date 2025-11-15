using Microsoft.EntityFrameworkCore;
using NouAssist.Learning.Aggregates;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.Learning.EntityFrameworkCore
{
    [ConnectionStringName(LearningDbProperties.ConnectionStringName)]
    public class LearningDbContext 
        : AbpDbContext<LearningDbContext>, ILearningDbContext
    {
        public DbSet<Topic> Topics { get; set; }
        public DbSet<LearningSession> LearningSessions { get; set; }
        public DbSet<LearningNote> LearningNotes { get; set; }
        public DbSet<Mindmap> Mindmaps { get; set; }
        public DbSet<BookReadingHistory> BookReadingHistories { get; set; }

        public LearningDbContext(DbContextOptions<LearningDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // IMPORTANT: Call base first to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations defined in this assembly
            builder.ConfigureLearning(this);
        }
    }
}
