using Microsoft.EntityFrameworkCore;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.EntityFrameworkCore
{
    [ConnectionStringName(KnowledgeInnovationDbProperties.ConnectionStringName)]
    public interface IKnowledgeInnovationDbContext : IEfCoreDbContext
    {
        // Aggregate Root DbSets
        DbSet<KnowledgeAsset> KnowledgeAssets { get; }
        DbSet<LearningResource> LearningResources { get; }
        DbSet<Idea> Ideas { get; }
        DbSet<DecisionRecord> DecisionRecords { get; }
        DbSet<MentorProfile> MentorProfiles { get; }
        
        // Entity DbSets
        DbSet<Tag> Tags { get; }
        DbSet<AssetContent> AssetContents { get; }
        DbSet<UserProgress> UserProgresses { get; }
        DbSet<IdeaEvaluation> IdeaEvaluations { get; }
        DbSet<DecisionContext> DecisionContexts { get; }
        DbSet<DecisionAlternative> DecisionAlternatives { get; }
        DbSet<DecisionOutcome> DecisionOutcomes { get; }
        DbSet<AdviceRecord> AdviceRecords { get; }
    }
}
