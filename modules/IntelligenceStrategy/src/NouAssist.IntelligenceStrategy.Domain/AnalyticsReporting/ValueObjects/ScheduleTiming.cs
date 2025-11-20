using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Defines the timing for a scheduled task
    /// </summary>
    public class ScheduleTiming : ValueObject
    {
        /// <summary>
        /// Cron expression for the schedule
        /// </summary>
        public string CronExpression { get; private set; }
        
        /// <summary>
        /// Timezone for the schedule
        /// </summary>
        public string TimeZone { get; private set; }
        
        /// <summary>
        /// Start date for the schedule
        /// </summary>
        public DateTime? StartDate { get; private set; }
        
        /// <summary>
        /// End date for the schedule
        /// </summary>
        public DateTime? EndDate { get; private set; }
        
        /// <summary>
        /// Whether the schedule is active
        /// </summary>
        public bool IsActive { get; private set; }
        
        private ScheduleTiming()
        {
        }
        
        public ScheduleTiming(
            string cronExpression,
            string timeZone,
            DateTime? startDate = null,
            DateTime? endDate = null,
            bool isActive = true)
        {
            CronExpression = Check.NotNullOrWhiteSpace(cronExpression, nameof(cronExpression));
            TimeZone = Check.NotNullOrWhiteSpace(timeZone, nameof(timeZone));
            
            if (endDate.HasValue && startDate.HasValue && endDate.Value < startDate.Value)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "endDate");
            }
            
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
        }
        
        public void Activate()
        {
            IsActive = true;
        }
        
        public void Deactivate()
        {
            IsActive = false;
        }
        
        public void UpdateCronExpression(string cronExpression)
        {
            CronExpression = Check.NotNullOrWhiteSpace(cronExpression, nameof(cronExpression));
        }
        
        public void UpdateTimeRange(DateTime? startDate, DateTime? endDate)
        {
            if (endDate.HasValue && startDate.HasValue && endDate.Value < startDate.Value)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "endDate");
            }
            
            StartDate = startDate;
            EndDate = endDate;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CronExpression;
            yield return TimeZone;
            yield return StartDate ?? DateTime.MinValue;
            yield return EndDate ?? DateTime.MaxValue;
            yield return IsActive;
        }
    }
}
