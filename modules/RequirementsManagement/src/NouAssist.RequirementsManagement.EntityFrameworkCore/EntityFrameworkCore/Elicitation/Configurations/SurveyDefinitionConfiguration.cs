using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Newtonsoft.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class SurveyDefinitionConfiguration : IEntityTypeConfiguration<SurveyDefinition>
    {
        public void Configure(EntityTypeBuilder<SurveyDefinition> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "SurveyDefinitions", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.SessionId)
                .IsRequired();

            builder.Property(x => x.IsDeployed)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.DeploymentDate)
                .IsRequired(false);

            // QuestionnaireStructure Value Object as JSON
            builder.Property(x => x.Questionnaire)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, Formatting.None),
                    v => JsonConvert.DeserializeObject<QuestionnaireStructure>(v))
                .HasColumnType("jsonb"); // PostgreSQL JSON column type

            // Relationships
            builder.HasMany(x => x.Responses)
                .WithOne()
                .HasForeignKey(r => r.SurveyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(x => x.SessionId)
                .IsUnique()
                .HasDatabaseName("IX_SurveyDefinitions_SessionId");

            builder.HasIndex(x => x.IsDeployed)
                .HasDatabaseName("IX_SurveyDefinitions_IsDeployed");

            builder.HasIndex(x => x.DeploymentDate)
                .HasDatabaseName("IX_SurveyDefinitions_DeploymentDate");
        }
    }
}
