using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a specific AI recommendation
    /// </summary>
    public class RecommendationInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the user this recommendation is for
        /// </summary>
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Type of recommendation
        /// </summary>
        public RecommendationType RecommendationType { get; protected set; }
        
        /// <summary>
        /// Title or summary of the recommendation
        /// </summary>
        public string Title { get; protected set; }
        
        /// <summary>
        /// Detailed description of the recommendation
        /// </summary>
        public string Description { get; protected set; }
        
        /// <summary>
        /// When the recommendation was generated
        /// </summary>
        public DateTime GeneratedAt { get; protected set; }
        
        /// <summary>
        /// When the recommendation expires/becomes irrelevant
        /// </summary>
        public DateTime? ExpiresAt { get; protected set; }
        
        /// <summary>
        /// Priority score for this recommendation
        /// </summary>
        public PriorityScore Priority { get; protected set; }
        
        /// <summary>
        /// Whether the user has viewed this recommendation
        /// </summary>
        public bool IsViewed { get; private set; }
        
        /// <summary>
        /// Whether the user has acted on this recommendation
        /// </summary>
        public bool IsActedOn { get; private set; }
        
        /// <summary>
        /// When the user acted on this recommendation
        /// </summary>
        public DateTime? ActedOnAt { get; private set; }
        
        /// <summary>
        /// Whether the user has dismissed this recommendation
        /// </summary>
        public bool IsDismissed { get; private set; }
        
        /// <summary>
        /// When the user dismissed this recommendation
        /// </summary>
        public DateTime? DismissedAt { get; private set; }
        
        /// <summary>
        /// User feedback on this recommendation (1-5)
        /// </summary>
        public int? UserRating { get; private set; }
        
        /// <summary>
        /// Target entity type (if applicable)
        /// </summary>
        public string TargetEntityType { get; protected set; }
        
        /// <summary>
        /// Target entity ID (if applicable)
        /// </summary>
        public Guid? TargetEntityId { get; protected set; }
        
        /// <summary>
        /// Rules that generated this recommendation
        /// </summary>
        private readonly List<RecommendationRule> _rules;
        
        /// <summary>
        /// Explainability logs for recommendation transparency
        /// </summary>
        private readonly List<ExplainabilityLog> _explainabilityLogs;
        
        /// <summary>
        /// Public access to rules
        /// </summary>
        public IReadOnlyCollection<RecommendationRule> Rules => _rules.AsReadOnly();
        
        /// <summary>
        /// Public access to explainability logs
        /// </summary>
        public IReadOnlyCollection<ExplainabilityLog> ExplainabilityLogs => _explainabilityLogs.AsReadOnly();

        protected RecommendationInstance()
        {
            _rules = new List<RecommendationRule>();
            _explainabilityLogs = new List<ExplainabilityLog>();
        }

        /// <summary>
        /// Creates a new recommendation instance
        /// </summary>
        public static RecommendationInstance Create(
            Guid id,
            Guid userId,
            RecommendationType recommendationType,
            string title,
            string description,
            int priorityValue,
            DateTime generatedAt,
            DateTime? expiresAt = null,
            string targetEntityType = null,
            Guid? targetEntityId = null,
            Guid? tenantId = null)
        {
            var instance = new RecommendationInstance(
                id,
                userId,
                recommendationType,
                title,
                description,
                priorityValue,
                generatedAt,
                expiresAt,
                targetEntityType,
                targetEntityId,
                tenantId);
            
            return instance;
        }

        private RecommendationInstance(
            Guid id,
            Guid userId,
            RecommendationType recommendationType,
            string title,
            string description,
            int priorityValue,
            DateTime generatedAt,
            DateTime? expiresAt,
            string targetEntityType,
            Guid? targetEntityId,
            Guid? tenantId) : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            RecommendationType = recommendationType;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            Priority = new PriorityScore(priorityValue);
            GeneratedAt = generatedAt;
            ExpiresAt = expiresAt;
            TargetEntityType = targetEntityType;
            TargetEntityId = targetEntityId;
            
            IsViewed = false;
            IsActedOn = false;
            IsDismissed = false;
            
            _rules = new List<RecommendationRule>();
            _explainabilityLogs = new List<ExplainabilityLog>();
            
            // Publish event
            AddDistributedEvent(new RecommendationGeneratedEto
            {
                UserId = userId,
                TenantId = tenantId,
                RecommendationId = id,
                RecommendationType = recommendationType,
                Title = title,
                PriorityScore = priorityValue,
                Timestamp = generatedAt
            });
        }

        /// <summary>
        /// Adds a rule that contributed to this recommendation
        /// </summary>
        public void AddRule(
            string ruleName,
            string ruleDescription,
            double contributionWeight)
        {
            Check.NotNullOrWhiteSpace(ruleName, nameof(ruleName));
            
            _rules.Add(new RecommendationRule(
                ruleName,
                ruleDescription,
                contributionWeight));
        }

        /// <summary>
        /// Adds an explainability log entry
        /// </summary>
        public void AddExplainabilityLog(
            string factorName,
            string explanation,
            double impact,
            string evidenceType,
            string evidenceData)
        {
            Check.NotNullOrWhiteSpace(factorName, nameof(factorName));
            Check.NotNullOrWhiteSpace(explanation, nameof(explanation));
            
            _explainabilityLogs.Add(new ExplainabilityLog(
                factorName,
                explanation,
                impact,
                evidenceType,
                evidenceData));
        }

        /// <summary>
        /// Marks the recommendation as viewed
        /// </summary>
        public void MarkAsViewed()
        {
            IsViewed = true;
            LastModificationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the recommendation as acted on
        /// </summary>
        /// <param name="actedOnAt">When the user acted on it</param>
        public void MarkAsActedOn(DateTime actedOnAt)
        {
            IsActedOn = true;
            ActedOnAt = actedOnAt;
            LastModificationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Marks the recommendation as dismissed
        /// </summary>
        /// <param name="dismissedAt">When the user dismissed it</param>
        public void MarkAsDismissed(DateTime dismissedAt)
        {
            IsDismissed = true;
            DismissedAt = dismissedAt;
            LastModificationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Sets the user's rating for this recommendation
        /// </summary>
        /// <param name="rating">Rating (1-5)</param>
        public void SetUserRating(int rating)
        {
            UserRating = Math.Clamp(rating, 1, 5);
            LastModificationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Checks if the recommendation is expired
        /// </summary>
        /// <returns>True if expired</returns>
        public bool IsExpired()
        {
            return ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
        }
    }
}
