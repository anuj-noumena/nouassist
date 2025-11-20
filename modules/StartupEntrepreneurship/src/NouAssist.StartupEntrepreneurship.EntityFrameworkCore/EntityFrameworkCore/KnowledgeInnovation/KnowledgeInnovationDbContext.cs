using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore
{
    [ConnectionStringName(KnowledgeInnovationDbProperties.ConnectionStringName)]
    public class KnowledgeInnovationDbContext : 
        AbpDbContext<KnowledgeInnovationDbContext>, IKnowledgeInnovationDbContext
    {
        // Aggregate Root DbSets
        public DbSet<KnowledgeAsset> KnowledgeAssets { get; set; }
        public DbSet<LearningResource> LearningResources { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<DecisionRecord> DecisionRecords { get; set; }
        public DbSet<MentorProfile> MentorProfiles { get; set; }
        
        // Entity DbSets
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AssetContent> AssetContents { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<IdeaEvaluation> IdeaEvaluations { get; set; }
        public DbSet<DecisionContext> DecisionContexts { get; set; }
        public DbSet<DecisionAlternative> DecisionAlternatives { get; set; }
        public DbSet<DecisionOutcome> DecisionOutcomes { get; set; }
        public DbSet<AdviceRecord> AdviceRecords { get; set; }

        public KnowledgeInnovationDbContext(DbContextOptions<KnowledgeInnovationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // First call base to apply ABP conventions
            base.OnModelCreating(builder);

            // Apply all configurations via extension method
            builder.ConfigureKnowledgeInnovation();
        }
    }
}
