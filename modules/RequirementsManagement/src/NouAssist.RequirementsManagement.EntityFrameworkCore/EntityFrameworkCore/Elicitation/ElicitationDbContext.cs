using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore
{
    [ConnectionStringName(ElicitationDbProperties.ConnectionStringName)]
    public class ElicitationDbContext : AbpDbContext<ElicitationDbContext>, IElicitationDbContext
    {
        // Aggregate Root DbSets
        public DbSet<StakeholderProfile> StakeholderProfiles { get; set; }
        public DbSet<ElicitationSession> ElicitationSessions { get; set; }
        public DbSet<ElicitationArtifact> ElicitationArtifacts { get; set; }
        
        // Entity DbSets
        public DbSet<ElicitationSessionParticipant> SessionParticipants { get; set; }
        public DbSet<SurveyDefinition> SurveyDefinitions { get; set; }
        public DbSet<SurveyResponse> SurveyResponses { get; set; }
        public DbSet<InterviewRecord> InterviewRecords { get; set; }
        public DbSet<BrainstormingIdea> BrainstormingIdeas { get; set; }

        public ElicitationDbContext(DbContextOptions<ElicitationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigureElicitation(this);
        }
    }
}
