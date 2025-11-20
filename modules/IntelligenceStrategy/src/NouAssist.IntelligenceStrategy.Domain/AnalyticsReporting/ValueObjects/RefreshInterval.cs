using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.IntelligenceStrategy.AnalyticsReporting.ValueObjects
{
    /// <summary>
    /// Represents a time interval for refreshing data
    /// </summary>
    public class RefreshInterval : ValueObject
    {
        /// <summary>
        /// Interval value
        /// </summary>
        public int Value { get; private set; }
        
        /// <summary>
        /// Unit of the interval (seconds, minutes, hours, days)
        /// </summary>
        public string Unit { get; private set; }
        
        /// <summary>
        /// Whether automatic refresh is enabled
        /// </summary>
        public bool AutoRefreshEnabled { get; private set; }
        
        private RefreshInterval()
        {
        }
        
        public RefreshInterval(int value, string unit, bool autoRefreshEnabled = true)
        {
            if (value < 0)
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "value");
            }
            
            Unit = Check.NotNullOrWhiteSpace(unit, nameof(unit));
            
            // Validate unit is one of the allowed values
            if (!IsValidUnit(unit))
            {
                throw new BusinessException("AnalyticsReporting:Validation:InvalidValue")
                    .WithData("propertyName", "unit");
            }
            
            Value = value;
            Unit = unit;
            AutoRefreshEnabled = autoRefreshEnabled;
        }
        
        public void EnableAutoRefresh()
        {
            AutoRefreshEnabled = true;
        }
        
        public void DisableAutoRefresh()
        {
            AutoRefreshEnabled = false;
        }
        
        /// <summary>
        /// Convert the interval to seconds
        /// </summary>
        public int ToSeconds()
        {
            return Unit switch
            {
                "seconds" => Value,
                "minutes" => Value * 60,
                "hours" => Value * 3600,
                "days" => Value * 86400,
                _ => Value
            };
        }
        
        private bool IsValidUnit(string unit)
        {
            return unit switch
            {
                "seconds" => true,
                "minutes" => true,
                "hours" => true,
                "days" => true,
                _ => false
            };
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
            yield return Unit;
            yield return AutoRefreshEnabled;
        }
    }
}
