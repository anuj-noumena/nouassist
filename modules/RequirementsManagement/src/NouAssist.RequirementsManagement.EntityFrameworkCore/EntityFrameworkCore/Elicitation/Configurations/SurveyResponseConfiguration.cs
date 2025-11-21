using System.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.RequirementsManagement.Elicitation.Entities;
using NouAssist.RequirementsManagement.Elicitation.ValueObjects;
using Newtonsoft.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.RequirementsManagement.Elicitation.EntityFrameworkCore.Configurations
{
    public class SurveyResponseConfiguration : IEntityTypeConfiguration<SurveyResponse>
    {
        public void Configure(EntityTypeBuilder<SurveyResponse> builder)
        {
            builder.ConfigureByConvention();

            builder.ToTable(ElicitationDbProperties.DbTablePrefix + "SurveyResponses", 
                           ElicitationDbProperties.DbSchema);

            builder.HasKey(x => x.Id);

            // Properties
            builder.Property(x => x.SurveyId)
                .IsRequired();

            builder.Property(x => x.RespondentId)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.LastModifiedAt)
                .IsRequired(false);

            // ResponseData Value Object as JSON
            builder.Property(x => x.ResponseData)
                .HasConversion(
                    v => JsonConvert.SerializeObject(v, Formatting.None),
                    v => JsonConvert.DeserializeObject<ResponseData>(v))
                .HasColumnType("jsonb"); // PostgreSQL JSON column type

            // Indexes
            builder.HasIndex(x => x.SurveyId)
                .HasDatabaseName("IX_SurveyResponses_SurveyId");

            builder.HasIndex(x => x.RespondentId)
                .HasDatabaseName("IX_SurveyResponses_RespondentId");

            builder.HasIndex(x => x.CreatedAt)
                .HasDatabaseName("IX_SurveyResponses_CreatedAt");

            // Composite indexes
            builder.HasIndex(x => new { x.SurveyId, x.RespondentId })
                .IsUnique()
                .HasDatabaseName("IX_SurveyResponses_Survey_Respondent");
        }
    }
}
