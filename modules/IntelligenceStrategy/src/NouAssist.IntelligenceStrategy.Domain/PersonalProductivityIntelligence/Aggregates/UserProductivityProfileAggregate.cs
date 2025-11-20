using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Entities;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Enums;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Events;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Aggregates
{
    /// <summary>
    /// Aggregate root representing a user's productivity profile
    /// </summary>
    public class UserProductivityProfile : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// ID of the user this profile belongs to
        /// </summary>
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Current cognitive load level
        /// </summary>
        public CognitiveLoadLevel CognitiveLoadLevel { get; private set; }
        
        /// <summary>
        /// Numerical value of cognitive load (0-100)
        /// </summary>
        public double CognitiveLoadValue { get; private set; }
        
        /// <summary>
        /// Last time cognitive load was updated
        /// </summary>
        public DateTime CognitiveLoadLastUpdated { get; private set; }
        
        /// <summary>
        /// User preference settings
        /// </summary>
        private readonly List<UserPreferenceSetting> _preferenceSettings;
        
        /// <summary>
        /// Performance data points
        /// </summary>
        private readonly List<PerformanceDataPoint> _performanceData;
        
        /// <summary>
        /// Task completion records
        /// </summary>
        private readonly List<TaskCompletionRecord> _taskCompletionRecords;
        
        /// <summary>
        /// Work pattern model
        /// </summary>
        public WorkPatternModel WorkPattern { get; private set; }
        
        /// <summary>
        /// External context references
        /// </summary>
        private readonly List<ExternalContextRef> _externalContextRefs;
        
        /// <summary>
        /// Public access to preference settings
        /// </summary>
        public IReadOnlyCollection<UserPreferenceSetting> PreferenceSettings => _preferenceSettings.AsReadOnly();
        
        /// <summary>
        /// Public access to performance data
        /// </summary>
        public IReadOnlyCollection<PerformanceDataPoint> PerformanceData => _performanceData.AsReadOnly();
        
        /// <summary>
        /// Public access to task completion records
        /// </summary>
        public IReadOnlyCollection<TaskCompletionRecord> TaskCompletionRecords => _taskCompletionRecords.AsReadOnly();
        
        /// <summary>
        /// Public access to external context references
        /// </summary>
        public IReadOnlyCollection<ExternalContextRef> ExternalContextRefs => _externalContextRefs.AsReadOnly();

        protected UserProductivityProfile()
        {
            _preferenceSettings = new List<UserPreferenceSetting>();
            _performanceData = new List<PerformanceDataPoint>();
            _taskCompletionRecords = new List<TaskCompletionRecord>();
            _externalContextRefs = new List<ExternalContextRef>();
        }

        /// <summary>
        /// Creates a new user productivity profile
        /// </summary>
        /// <param name="id">ID of the profile</param>
        /// <param name="userId">ID of the user</param>
        /// <param name="tenantId">ID of the tenant</param>
        /// <returns>New user productivity profile</returns>
        public static UserProductivityProfile Create(Guid id, Guid userId, Guid? tenantId = null)
        {
            return new UserProductivityProfile(id, userId, tenantId);
        }

        private UserProductivityProfile(Guid id, Guid userId, Guid? tenantId) : base(id)
        {
            UserId = userId;
            TenantId = tenantId;
            CognitiveLoadLevel = CognitiveLoadLevel.Low;
            CognitiveLoadValue = 0;
            CognitiveLoadLastUpdated = DateTime.UtcNow;
            
            _preferenceSettings = new List<UserPreferenceSetting>();
            _performanceData = new List<PerformanceDataPoint>();
            _taskCompletionRecords = new List<TaskCompletionRecord>();
            _externalContextRefs = new List<ExternalContextRef>();
        }

        /// <summary>
        /// Updates the user's cognitive load
        /// </summary>
        /// <param name="loadValue">Load value (0-100)</param>
        /// <param name="timestamp">Timestamp of the update</param>
        public void UpdateCognitiveLoad(double loadValue, DateTime timestamp)
        {
            loadValue = Math.Clamp(loadValue, 0, 100);
            
            var previousLevel = CognitiveLoadLevel;
            CognitiveLoadValue = loadValue;
            CognitiveLoadLastUpdated = timestamp;
            
            // Determine cognitive load level based on value
            CognitiveLoadLevel newLevel;
            if (loadValue < 30)
                newLevel = CognitiveLoadLevel.Low;
            else if (loadValue < 60)
                newLevel = CognitiveLoadLevel.Medium;
            else if (loadValue < 85)
                newLevel = CognitiveLoadLevel.High;
            else
                newLevel = CognitiveLoadLevel.Critical;
            
            bool levelChanged = previousLevel != newLevel;
            CognitiveLoadLevel = newLevel;
            
            // Emit event if level changed
            if (levelChanged)
            {
                AddDistributedEvent(new CognitiveLoadThresholdReachedEto
                {
                    UserId = UserId,
                    TenantId = TenantId,
                    LoadLevel = newLevel,
                    LoadValue = loadValue,
                    IsIncreasing = newLevel > previousLevel,
                    Timestamp = timestamp
                });
            }
        }

        /// <summary>
        /// Sets or updates a user preference setting
        /// </summary>
        /// <param name="key">Preference key</param>
        /// <param name="value">Preference value</param>
        public void SetPreference(string key, string value)
        {
            Check.NotNullOrWhiteSpace(key, nameof(key));
            
            var existingSetting = _preferenceSettings.FirstOrDefault(p => p.Key == key);
            if (existingSetting != null)
            {
                existingSetting.UpdateValue(value);
            }
            else
            {
                _preferenceSettings.Add(new UserPreferenceSetting(key, value));
            }
        }

        /// <summary>
        /// Gets a user preference value
        /// </summary>
        /// <param name="key">Preference key</param>
        /// <returns>Preference value or null if not found</returns>
        public string GetPreference(string key)
        {
            return _preferenceSettings.FirstOrDefault(p => p.Key == key)?.Value;
        }

        /// <summary>
        /// Records a task completion
        /// </summary>
        /// <param name="taskId">ID of the task</param>
        /// <param name="taskName">Name of the task</param>
        /// <param name="estimatedDuration">Estimated duration in minutes</param>
        /// <param name="actualDuration">Actual duration in minutes</param>
        /// <param name="completedAt">Completion timestamp</param>
        /// <param name="taskContext">Task context</param>
        public void RecordTaskCompletion(
            Guid taskId,
            string taskName,
            int estimatedDuration,
            int actualDuration,
            DateTime completedAt,
            TaskContext taskContext = null)
        {
            Check.NotNullOrWhiteSpace(taskName, nameof(taskName));
            
            var record = new TaskCompletionRecord(
                taskId,
                taskName,
                estimatedDuration,
                actualDuration,
                completedAt,
                taskContext);
            
            _taskCompletionRecords.Add(record);
            
            // Add performance data point
            var accuracyPercentage = estimatedDuration > 0 
                ? Math.Min(200, (double)estimatedDuration / actualDuration * 100) 
                : 0;
            
            _performanceData.Add(new PerformanceDataPoint(
                "TimeEstimationAccuracy",
                accuracyPercentage,
                completedAt,
                taskId.ToString()));
        }

        /// <summary>
        /// Updates the work pattern model
        /// </summary>
        /// <param name="patternDescription">Description of the pattern</param>
        /// <param name="patternConfidence">Confidence in the pattern (0-1)</param>
        /// <param name="modelData">Model data</param>
        /// <param name="createdAt">Creation timestamp</param>
        public void UpdateWorkPattern(
            string patternDescription,
            double patternConfidence,
            string modelData,
            DateTime createdAt)
        {
            Check.NotNullOrWhiteSpace(patternDescription, nameof(patternDescription));
            Check.NotNull(modelData, nameof(modelData));
            
            WorkPattern = new WorkPatternModel(
                GuidGenerator.Create(),
                Id,
                patternDescription,
                patternConfidence,
                modelData,
                createdAt);
            
            AddDistributedEvent(new WorkPatternDetectedEto
            {
                UserId = UserId,
                TenantId = TenantId,
                WorkPatternModelId = WorkPattern.Id,
                PatternDescription = patternDescription,
                PatternConfidence = patternConfidence,
                Timestamp = createdAt
            });
        }

        /// <summary>
        /// Adds a performance data point
        /// </summary>
        /// <param name="metricName">Name of the metric</param>
        /// <param name="value">Metric value</param>
        /// <param name="timestamp">Timestamp</param>
        /// <param name="context">Optional context</param>
        public void AddPerformanceDataPoint(
            string metricName, 
            double value, 
            DateTime timestamp, 
            string context = null)
        {
            Check.NotNullOrWhiteSpace(metricName, nameof(metricName));
            
            _performanceData.Add(new PerformanceDataPoint(
                metricName,
                value,
                timestamp,
                context));
        }

        /// <summary>
        /// Adds or updates an external context reference
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">ID of the entity</param>
        /// <param name="properties">Additional properties</param>
        public void AddOrUpdateExternalContext(
            string entityType,
            Guid entityId,
            Dictionary<string, string> properties = null)
        {
            Check.NotNullOrWhiteSpace(entityType, nameof(entityType));
            
            var existingRef = _externalContextRefs.FirstOrDefault(
                r => r.EntityType == entityType && r.EntityId == entityId);
            
            if (existingRef != null)
            {
                // Update properties
                if (properties != null)
                {
                    foreach (var prop in properties)
                    {
                        existingRef.AddProperty(prop.Key, prop.Value);
                    }
                }
            }
            else
            {
                // Add new reference
                _externalContextRefs.Add(new ExternalContextRef(entityType, entityId, properties));
            }
        }

        /// <summary>
        /// Removes an external context reference
        /// </summary>
        /// <param name="entityType">Type of the entity</param>
        /// <param name="entityId">ID of the entity</param>
        public void RemoveExternalContext(string entityType, Guid entityId)
        {
            var existingRef = _externalContextRefs.FirstOrDefault(
                r => r.EntityType == entityType && r.EntityId == entityId);
            
            if (existingRef != null)
            {
                _externalContextRefs.Remove(existingRef);
            }
        }
    }
}
