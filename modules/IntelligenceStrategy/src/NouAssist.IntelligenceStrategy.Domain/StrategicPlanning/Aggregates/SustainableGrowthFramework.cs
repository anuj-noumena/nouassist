using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Constants;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Entities;
using NouAssist.IntelligenceStrategy.StrategicPlanning.Enums;
using NouAssist.IntelligenceStrategy.StrategicPlanning.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Domain.Entities;

namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates
{
    /// <summary>
    /// Sustainable Growth Framework aggregate root.
    /// Manages ESG goals, impact measurements, and long-term sustainability metrics.
    /// </summary>
    public class SustainableGrowthFramework : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime FrameworkStartDate { get; private set; }
        public DateTime FrameworkEndDate { get; private set; }
        public bool IsActive { get; private set; }

        private readonly List<EsgGoalRecord> _esgGoals;
        public IReadOnlyCollection<EsgGoalRecord> EsgGoals => _esgGoals.AsReadOnly();

        protected SustainableGrowthFramework()
        {
            _esgGoals = new List<EsgGoalRecord>();
        }

        /// <summary>
        /// Factory method to create a new sustainable growth framework
        /// </summary>
        public static SustainableGrowthFramework Create(
            Guid id,
            string name,
            string description,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId = null)
        {
            return new SustainableGrowthFramework(id, name, description, startDate, endDate, tenantId);
        }

        private SustainableGrowthFramework(
            Guid id,
            string name,
            string description,
            DateTime startDate,
            DateTime endDate,
            Guid? tenantId) : base(id)
        {
            SetName(name);
            SetDescription(description);
            SetFrameworkPeriod(startDate, endDate);
            TenantId = tenantId;
            IsActive = true;
            _esgGoals = new List<EsgGoalRecord>();
        }

        /// <summary>
        /// Adds an ESG goal to the framework
        /// </summary>
        public void AddEsgGoal(
            EsgCategory category,
            string title,
            string description,
            string metricName,
            decimal targetValue,
            string unit,
            DateTime targetDate)
        {
            if (!IsActive)
            {
                throw new BusinessException("Cannot add goals to an inactive framework");
            }

            var metric = new TargetMetric(metricName, targetValue, unit, targetDate);
            var esgGoal = new EsgGoalRecord(Guid.NewGuid(), Id, category, title, description, metric);
            
            _esgGoals.Add(esgGoal);
        }

        /// <summary>
        /// Records a measurement for an ESG goal
        /// </summary>
        public void RecordEsgMeasurement(Guid esgGoalId, decimal value, DateTime measurementDate)
        {
            var esgGoal = _esgGoals.FirstOrDefault(g => g.Id == esgGoalId);
            if (esgGoal == null)
            {
                throw new EntityNotFoundException(typeof(EsgGoalRecord), esgGoalId);
            }

            esgGoal.RecordMeasurement(value, measurementDate);
        }

        /// <summary>
        /// Calculates overall ESG score
        /// </summary>
        public decimal CalculateOverallEsgScore()
        {
            if (!_esgGoals.Any()) return 0;

            var categoryScores = new Dictionary<EsgCategory, decimal>();
            var categoryCounts = new Dictionary<EsgCategory, int>();

            foreach (var goal in _esgGoals)
            {
                if (!categoryScores.ContainsKey(goal.Category))
                {
                    categoryScores[goal.Category] = 0;
                    categoryCounts[goal.Category] = 0;
                }

                categoryScores[goal.Category] += goal.ProgressPercentage;
                categoryCounts[goal.Category]++;
            }

            // Calculate average score for each category
            var averageScores = categoryScores.Select(kvp => 
                categoryCounts[kvp.Key] > 0 ? kvp.Value / categoryCounts[kvp.Key] : 0).ToList();

            return averageScores.Any() ? averageScores.Average() : 0;
        }

        /// <summary>
        /// Gets ESG goals by category
        /// </summary>
        public IEnumerable<EsgGoalRecord> GetEsgGoalsByCategory(EsgCategory category)
        {
            return _esgGoals.Where(g => g.Category == category);
        }

        /// <summary>
        /// Gets goals that are behind schedule
        /// </summary>
        public IEnumerable<EsgGoalRecord> GetGoalsBehindSchedule()
        {
            return _esgGoals.Where(g => !g.IsOnTrack);
        }

        /// <summary>
        /// Activates the framework
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }

        /// <summary>
        /// Deactivates the framework
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: StrategicPlanningConsts.MaxFrameworkNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: StrategicPlanningConsts.MaxEsgGoalDescriptionLength
            );
        }

        private void SetFrameworkPeriod(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                throw new BusinessException("Framework end date must be after start date");
            }

            FrameworkStartDate = startDate;
            FrameworkEndDate = endDate;
        }
    }
}
