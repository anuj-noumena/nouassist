using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.Analytics.Aggregates;
using NouAssist.Analytics.Constants;
using NouAssist.Analytics.Entities;
using System;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace NouAssist.Analytics.EntityFrameworkCore.EntityConfigurations
{
    /// <summary>
    /// Entity configuration for DataSource aggregate
    /// </summary>
    public class DataSourceConfiguration : IEntityTypeConfiguration<DataSource>
    {
        public void Configure(EntityTypeBuilder<DataSource> builder)
        {
            builder.ToTable(AnalyticsConsts.ModuleName + "DataSources");
            
            builder.ConfigureByConvention();
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AnalyticsConsts.DataSource.MaxNameLength);
                
            builder.Property(x => x.Description)
                .HasMaxLength(AnalyticsConsts.DataSource.MaxDescriptionLength);
                
            builder.Property(x => x.Type)
                .IsRequired();
                
            builder.Property(x => x.IsActive)
                .IsRequired();
                
            builder.HasIndex(x => new { x.TenantId, x.Name })
                .IsUnique();
                
            builder.HasIndex(x => new { x.TenantId, x.Type, x.IsActive });
            
            // Configure ConnectionString as owned entity
            builder.OwnsOne(
                x => x.ConnectionString,
                connectionString =>
                {
                    connectionString.Property(c => c.Value)
                        .IsRequired()
                        .HasMaxLength(AnalyticsConsts.DataSource.MaxConnectionStringLength)
                        .HasColumnName("ConnectionString");
                }
            );
            
            // Configure Credentials as owned entity
            builder.OwnsOne(
                x => x.Credentials,
                credentials =>
                {
                    credentials.Property(c => c.Username).HasMaxLength(100).HasColumnName("CredentialsUsername");
                    credentials.Property(c => c.EncryptedPassword).HasMaxLength(1000).HasColumnName("CredentialsPassword");
                    credentials.Property(c => c.ApiKey).HasMaxLength(1000).HasColumnName("CredentialsApiKey");
                    credentials.Property(c => c.TokenType).HasMaxLength(50).HasColumnName("CredentialsTokenType");
                    
                    // Store AdditionalSecrets as JSON
                    credentials.Property(c => c.AdditionalSecrets)
                        .HasColumnName("CredentialsSecrets")
                        .HasColumnType("jsonb"); // For PostgreSQL
                }
            );
            
            // Configure DataTransformationStep as owned entity collection
            builder.OwnsMany(
                x => x.TransformationSteps,
                steps =>
                {
                    steps.WithOwner().HasForeignKey("DataSourceId");
                    steps.HasKey(s => s.Id);
                    
                    steps.Property(s => s.Name).IsRequired().HasMaxLength(100);
                    steps.Property(s => s.TransformationType).IsRequired().HasMaxLength(100);
                    steps.Property(s => s.Configuration).IsRequired().HasColumnType("jsonb"); // For PostgreSQL
                    steps.Property(s => s.Order).IsRequired();
                    steps.Property(s => s.IsEnabled).IsRequired();
                    
                    steps.HasIndex(s => s.Order);
                }
            );
            
            // Configure AggregatedDataView as owned entity collection
            builder.OwnsMany(
                x => x.AggregatedDataViews,
                views =>
                {
                    views.WithOwner().HasForeignKey("DataSourceId");
                    views.HasKey(v => v.Id);
                    
                    views.Property(v => v.Name).IsRequired().HasMaxLength(100);
                    views.Property(v => v.ViewDefinition).IsRequired().HasColumnType("jsonb"); // For PostgreSQL
                    views.Property(v => v.RefreshStrategy).IsRequired().HasMaxLength(50);
                    views.Property(v => v.LastRefreshed).IsRequired();
                    views.Property(v => v.IsEnabled).IsRequired();
                    
                    views.HasIndex(v => v.Name);
                }
            );
        }
    }
}
