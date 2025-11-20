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
    /// Aggregate root representing a specific AI prediction
    /// </summary>
    public class PredictionInstance : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the user this prediction is for
        /// </summary>
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Type of prediction
        /// </summary>
        public string PredictionType { get; protected set; }
        
        /// <summary>
        /// Title or summary of the prediction
        /// </summary>
        public string Title { get; protected set; }
        
        /// <summary>
        /// Textual description of the prediction
        /// </summary>
        public string Description { get; protected set; }
        
        /// <summary>
        /// Confidence level of the prediction
        /// </summary>
        public PredictionConfidenceLevel ConfidenceLevel { get; protected set; }
        
        /// <summary>
        /// Numeric confidence value (0-1)
        /// </summary>
        public double ConfidenceValue { get; protected set; }
        
        /// <summary>
        /// When the prediction was made
        /// </summary>
        public DateTime PredictionTime { get; protected set; }
        
        /// <summary>
        /// When the prediction relates to (future time)
        /// </summary>
        public DateTime? TargetTime { get; protected set; }
        
        /// <summary>
        /// Target entity type (Task, Project, etc.)
        /// </summary>
        public string TargetEntityType { get; protected set; }
        
        /// <summary>
        /// Target entity ID
        /// </summary>
        public Guid? TargetEntityId { get; protected set; }
        
        /// <summary>
        /// Time estimate if this is a duration prediction
        /// </summary>
        public TimeEstimate TimeEstimate { get; protected set; }
        
        /// <summary>
        /// Risk probability if this is a risk prediction
        /// </summary>
        public RiskProbability RiskProbability { get; protected set; }
        
        /// <summary>
        /// Outcome value if this is a numeric prediction
        /// </summary>
        public double? OutcomeValue { get; protected set; }
        
        /// <summary>
        /// Model metadata for this prediction
        /// </summary>
        public PredictionModelMetadata ModelMetadata { get; protected set; }
        
        /// <summary>
        /// Explainability logs for prediction transparency
        /// </summary>
        private readonly List<ExplainabilityLog> _explainabilityLogs;
        
        /// <summary>
        /// Public access to explainability logs
        /// </summary>
        public IReadOnlyCollection<ExplainabilityLog> ExplainabilityLogs => _explainabilityLogs.AsReadOnly();

        protected PredictionInstance()
        {
            _explainabilityLogs = new List<ExplainabilityLog>();
        }

        /// <summary>
        /// Creates a new time duration prediction
        /// </summary>
        public static PredictionInstance CreateDurationPrediction(
            Guid id,
            Guid userId,
            string title,
            string description,
            TimeEstimate timeEstimate,
            PredictionConfidenceLevel confidenceLevel,
            double confidenceValue,
            string targetEntityType,
            Guid targetEntityId,
            DateTime predictionTime,
            Guid? tenantId = null)
        {
            var instance = new PredictionInstance(
                id,
                userId,
                "Duration",
                title,
                description,
                confidenceLevel,
                confidenceValue,
                predictionTime,
                targetEntityType,
                targetEntityId,
                tenantId);
            
            instance.TimeEstimate = timeEstimate;
            return instance;
        }

        /// <summary>
        /// Creates a new risk prediction
        /// </summary>
        public static PredictionInstance CreateRiskPrediction(
            Guid id,
            Guid userId,
            string title,
            string description,
            RiskProbability riskProbability,
            PredictionConfidenceLevel confidenceLevel,
            double confidenceValue,
            string targetEntityType,
            Guid targetEntityId,
            DateTime predictionTime,
            Guid? tenantId = null)
        {
            var instance = new PredictionInstance(
                id,
                userId,
                "Risk",
                title,
                description,
                confidenceLevel,
                confidenceValue,
                predictionTime,
                targetEntityType,
                targetEntityId,
                tenantId);
            
            instance.RiskProbability = riskProbability;
            return instance;
        }

        /// <summary>
        /// Creates a new numeric outcome prediction
        /// </summary>
        public static PredictionInstance CreateNumericPrediction(
            Guid id,
            Guid userId,
            string title,
            string description,
            double outcomeValue,
            PredictionConfidenceLevel confidenceLevel,
            double confidenceValue,
            string targetEntityType,
            Guid? targetEntityId,
            DateTime predictionTime,
            DateTime? targetTime = null,
            Guid? tenantId = null)
        {
            var instance = new PredictionInstance(
                id,
                userId,
                "Numeric",
                title,
                description,
                confidenceLevel,
                confidenceValue,
                predictionTime,
                targetEntityType,
                targetEntityId,
                tenantId);
            
            instance.OutcomeValue = outcomeValue;
            instance.TargetTime = targetTime;
            return instance;
        }

        private PredictionInstance(
            Guid id,
            Guid userId,
            string predictionType,
            string title,
            string description,
            PredictionConfidenceLevel confidenceLevel,
            double confidenceValue,
            DateTime predictionTime,
            string targetEntityType,
            Guid? targetEntityId,
            Guid? tenantId) : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            PredictionType = Check.NotNullOrWhiteSpace(predictionType, nameof(predictionType));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            ConfidenceLevel = confidenceLevel;
            ConfidenceValue = Math.Clamp(confidenceValue, 0, 1);
            PredictionTime = predictionTime;
            TargetEntityType = targetEntityType;
            TargetEntityId = targetEntityId;
            
            _explainabilityLogs = new List<ExplainabilityLog>();
            
            // Publish event
            AddDistributedEvent(new PredictionMadeEto
            {
                UserId = userId,
                TenantId = tenantId,
                PredictionId = id,
                PredictionType = predictionType,
                ConfidenceLevel = confidenceLevel,
                TargetEntityId = targetEntityId,
                TargetEntityType = targetEntityType,
                Timestamp = predictionTime
            });
        }

        /// <summary>
        /// Sets the model metadata for this prediction
        /// </summary>
        public void SetModelMetadata(
            string modelName,
            string modelVersion,
            string features,
            Dictionary<string, double> featureImportance)
        {
            ModelMetadata = new PredictionModelMetadata(modelName, modelVersion, features);
            
            if (featureImportance != null)
            {
                foreach (var feature in featureImportance)
                {
                    ModelMetadata.AddFeatureImportance(feature.Key, feature.Value);
                }
            }
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
        /// Records the actual outcome to compare against prediction
        /// </summary>
        public void RecordActualOutcome(double actualValue, DateTime timestamp)
        {
            // Implementation could track prediction accuracy for later model improvement
        }
    }
}
