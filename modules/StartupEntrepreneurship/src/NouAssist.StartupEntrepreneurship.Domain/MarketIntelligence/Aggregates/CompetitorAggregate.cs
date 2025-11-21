using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Constants;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Events;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a competitor and their monitoring activities
    /// </summary>
    public class Competitor : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public CompetitorInfo CompetitorInfo { get; private set; }
        public CompetitorStatus Status { get; private set; }
        public CompetitorThreatLevel ThreatLevel { get; private set; }
        public string Notes { get; private set; }
        public string Tags { get; private set; }
        public DateTime LastAnalyzed { get; private set; }
        public decimal CompetitiveScore { get; private set; }

        // Navigation properties for entities within the aggregate
        private readonly List<CompetitorUpdateLog> _updateLogs;
        public IReadOnlyCollection<CompetitorUpdateLog> UpdateLogs => _updateLogs.AsReadOnly();

        protected Competitor()
        {
            _updateLogs = new List<CompetitorUpdateLog>();
        }

        /// <summary>
        /// Creates a new competitor
        /// </summary>
        public static Competitor Create(
            Guid id,
            string name,
            string description,
            CompetitorInfo competitorInfo,
            CompetitorThreatLevel threatLevel = CompetitorThreatLevel.Low,
            Guid? tenantId = null,
            DateTime? creationTime = null)
        {
            return new Competitor(id, name, description, competitorInfo, threatLevel, tenantId, creationTime);
        }

        private Competitor(
            Guid id,
            string name,
            string description,
            CompetitorInfo competitorInfo,
            CompetitorThreatLevel threatLevel,
            Guid? tenantId,
            DateTime? creationTime) : base(id)
        {
            SetName(name);
            SetDescription(description);
            CompetitorInfo = competitorInfo ?? throw new ArgumentNullException(nameof(competitorInfo));
            ThreatLevel = threatLevel;
            TenantId = tenantId;
            Status = CompetitorStatus.Active;
            LastAnalyzed = creationTime ?? DateTime.UtcNow;
            CompetitiveScore = 0.5m; // Default neutral score
            _updateLogs = new List<CompetitorUpdateLog>();

            // Publish domain event
            AddDistributedEvent(new CompetitorCreatedEto
            {
                Id = Id,
                Name = Name,
                Website = CompetitorInfo.Website,
                ThreatLevel = ThreatLevel,
                Industry = CompetitorInfo.Industry,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates competitor information
        /// </summary>
        public void UpdateInfo(string name, string description, CompetitorInfo competitorInfo)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.Null(competitorInfo, nameof(competitorInfo));

            var previousName = Name;
            SetName(name);
            SetDescription(description);
            CompetitorInfo = competitorInfo;

            // Log the update
            AddUpdateLog(
                UpdateType.GeneralInfo,
                "Competitor Information Updated",
                $"Updated competitor information for {previousName}",
                DateTime.UtcNow,
                ImpactLevel.Low
            );
        }

        /// <summary>
        /// Changes the threat level and publishes an event
        /// </summary>
        public void ChangeThreatLevel(CompetitorThreatLevel newThreatLevel, string reason)
        {
            var previousThreatLevel = ThreatLevel;
            
            if (previousThreatLevel == newThreatLevel)
                return;

            ThreatLevel = newThreatLevel;

            // Log the change
            AddUpdateLog(
                UpdateType.Strategy,
                "Threat Level Changed",
                reason ?? "Threat level assessment updated",
                DateTime.UtcNow,
                GetImpactLevelFromThreat(newThreatLevel)
            );

            // Publish domain event
            AddDistributedEvent(new CompetitorThreatLevelChangedEto
            {
                Id = Id,
                CompetitorName = Name,
                PreviousThreatLevel = previousThreatLevel,
                NewThreatLevel = newThreatLevel,
                ChangeReason = reason,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the competitive score based on analysis
        /// </summary>
        public void UpdateCompetitiveScore(decimal score, DateTime analysisTime)
        {
            if (score < MarketIntelligenceConsts.Validation.MinConfidenceScore ||
                score > MarketIntelligenceConsts.Validation.MaxConfidenceScore)
            {
                throw new BusinessException("MarketIntelligence:Validation:InvalidScore")
                    .WithData("min", MarketIntelligenceConsts.Validation.MinConfidenceScore)
                    .WithData("max", MarketIntelligenceConsts.Validation.MaxConfidenceScore);
            }

            CompetitiveScore = score;
            LastAnalyzed = analysisTime;
        }

        /// <summary>
        /// Adds an update log entry
        /// </summary>
        public void AddUpdateLog(
            UpdateType type,
            string title,
            string description,
            DateTime updateDate,
            ImpactLevel impactLevel = ImpactLevel.Low,
            string source = null,
            string tags = null,
            string additionalData = null)
        {
            var updateLog = new CompetitorUpdateLog(
                GuidGenerator.Create(),
                Id,
                type,
                title,
                description,
                updateDate,
                impactLevel,
                source,
                tags,
                additionalData
            );

            _updateLogs.Add(updateLog);
        }

        /// <summary>
        /// Changes competitor status
        /// </summary>
        public void ChangeStatus(CompetitorStatus newStatus)
        {
            if (Status == newStatus)
                return;

            var previousStatus = Status;
            Status = newStatus;

            if (newStatus == CompetitorStatus.Archived)
            {
                AddUpdateLog(
                    UpdateType.GeneralInfo,
                    "Competitor Archived",
                    $"Competitor status changed from {previousStatus} to {newStatus}",
                    DateTime.UtcNow,
                    ImpactLevel.Medium
                );
            }
        }

        /// <summary>
        /// Gets recent update logs
        /// </summary>
        public IReadOnlyList<CompetitorUpdateLog> GetRecentUpdates(int count = 10)
        {
            return _updateLogs
                .OrderByDescending(x => x.UpdateDate)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Checks if competitor is actively monitored
        /// </summary>
        public bool IsActivelyMonitored()
        {
            return Status == CompetitorStatus.Active;
        }

        /// <summary>
        /// Gets the competition intensity based on threat level and recent activity
        /// </summary>
        public string GetCompetitionIntensity()
        {
            var recentUpdates = _updateLogs.Count(x => x.UpdateDate >= DateTime.UtcNow.AddDays(-30));
            
            return ThreatLevel switch
            {
                CompetitorThreatLevel.Critical => "Very High",
                CompetitorThreatLevel.High when recentUpdates > 5 => "High",
                CompetitorThreatLevel.High => "Medium-High",
                CompetitorThreatLevel.Medium when recentUpdates > 10 => "Medium-High",
                CompetitorThreatLevel.Medium => "Medium",
                CompetitorThreatLevel.Low when recentUpdates > 15 => "Medium",
                _ => "Low"
            };
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: MarketIntelligenceConsts.Competitor.MaxNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: MarketIntelligenceConsts.Competitor.MaxDescriptionLength
            );
        }

        private static ImpactLevel GetImpactLevelFromThreat(CompetitorThreatLevel threatLevel)
        {
            return threatLevel switch
            {
                CompetitorThreatLevel.Critical => ImpactLevel.Critical,
                CompetitorThreatLevel.High => ImpactLevel.High,
                CompetitorThreatLevel.Medium => ImpactLevel.Medium,
                _ => ImpactLevel.Low
            };
        }
    }
}
