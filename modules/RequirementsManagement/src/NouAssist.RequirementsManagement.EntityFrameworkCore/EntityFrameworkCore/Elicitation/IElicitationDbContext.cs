using Microsoft.EntityFrameworkCore;
using NouAssist.RequirementsManagement.Elicitation.Aggregates;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore
{
    [ConnectionStringName(ElicitationDbProperties.ConnectionStringName)]
    public interface IElicitationDbContext : IEfCoreDbContext
    {
        // Aggregate Root DbSets
        DbSet<StakeholderProfile> StakeholderProfiles { get; }
        DbSet<ElicitationSession> ElicitationSessions { get; }
        DbSet<ElicitationArtifact> ElicitationArtifacts { get; }
        
        // Entity DbSets (for direct queries when needed)
        DbSet<ElicitationSessionParticipant> SessionParticipants { get; }
        DbSet<SurveyDefinition> SurveyDefinitions { get; }
        DbSet<SurveyResponse> SurveyResponses { get; }
        DbSet<InterviewRecord> InterviewRecords { get; }
        DbSet<BrainstormingIdea> BrainstormingIdeas { get; }
    }
}
