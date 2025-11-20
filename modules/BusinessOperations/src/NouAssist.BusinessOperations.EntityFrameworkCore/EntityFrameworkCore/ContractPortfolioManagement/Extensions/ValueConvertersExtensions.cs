using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Converters;
using NouAssist.ContractPortfolioManagement.ValueObjects;

namespace NouAssist.ContractPortfolioManagement.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// Extension methods for configuring complex value objects and collections
    /// </summary>
    public static class ValueConvertersExtensions
    {
        /// <summary>
        /// Configures JSON storage for Portfolio Goals collection
        /// </summary>
        public static void ConfigurePortfolioGoals(this EntityTypeBuilder builder)
        {
            builder.Property("_goalsJson")
                .HasConversion(
                    JsonHelpers.GetJsonValueConverter<PortfolioGoal>())
                .Metadata.SetValueComparer(
                    JsonHelpers.GetJsonValueComparer<PortfolioGoal>());
        }

        /// <summary>
        /// Configures JSON storage for Performance Metrics collection
        /// </summary>
        public static void ConfigurePerformanceMetrics(this EntityTypeBuilder builder)
        {
            builder.Property("_metricsJson")
                .HasConversion(
                    JsonHelpers.GetJsonValueConverter<PerformanceMetric>())
                .Metadata.SetValueComparer(
                    JsonHelpers.GetJsonValueComparer<PerformanceMetric>());
        }

        /// <summary>
        /// Configures JSON storage for Sharing Links collection
        /// </summary>
        public static void ConfigureSharingLinks(this EntityTypeBuilder builder)
        {
            builder.Property("_sharingLinksJson")
                .HasConversion(
                    JsonHelpers.GetJsonValueConverter<SharingLink>())
                .Metadata.SetValueComparer(
                    JsonHelpers.GetJsonValueComparer<SharingLink>());
        }

        /// <summary>
        /// Configures JSON storage for Resource Skill Requirements collection
        /// </summary>
        public static void ConfigureResourceSkillRequirements(this EntityTypeBuilder builder)
        {
            builder.Property("_skillRequirementsJson")
                .HasConversion(
                    JsonHelpers.GetJsonValueConverter<ResourceSkillRequirement>())
                .Metadata.SetValueComparer(
                    JsonHelpers.GetJsonValueComparer<ResourceSkillRequirement>());
        }

        /// <summary>
        /// Configures JSON storage for Affected Sections in AmendmentDetails
        /// </summary>
        public static void ConfigureAffectedSections(this EntityTypeBuilder builder)
        {
            builder.Property("_affectedSectionsJson")
                .HasConversion(
                    v => string.Join(",", v),
                    v => v == null ? new List<string>() : v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(
                    new ValueComparer<List<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c.ToList()));
        }
    }
}
