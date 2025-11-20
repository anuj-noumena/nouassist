using System;
using Volo.Abp.Domain.Values;
using NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.ValueObjects
{
    /// <summary>
    /// Value object representing a time estimate for a task or activity
    /// </summary>
    public class TimeEstimate : ValueObject
    {
        /// <summary>
        /// Minimum value in minutes
        /// </summary>
        public int MinMinutes { get; private set; }
        
        /// <summary>
        /// Maximum value in minutes
        /// </summary>
        public int MaxMinutes { get; private set; }
        
        /// <summary>
        /// Most likely value in minutes
        /// </summary>
        public int MostLikelyMinutes { get; private set; }
        
        /// <summary>
        /// Expected value calculated using PERT estimation
        /// </summary>
        public double ExpectedMinutes => (MinMinutes + (4 * MostLikelyMinutes) + MaxMinutes) / 6.0;

        protected TimeEstimate()
        {
        }

        /// <summary>
        /// Creates a new time estimate
        /// </summary>
        /// <param name="minMinutes">Minimum minutes</param>
        /// <param name="mostLikelyMinutes">Most likely minutes</param>
        /// <param name="maxMinutes">Maximum minutes</param>
        public TimeEstimate(int minMinutes, int mostLikelyMinutes, int maxMinutes)
        {
            if (minMinutes < 0 || mostLikelyMinutes < 0 || maxMinutes < 0)
            {
                throw new PersonalProductivityIntelligenceDomainException(
                    "PersonalProductivityIntelligence:Validation:InvalidTimeEstimate",
                    "Time estimates cannot be negative.");
            }

            if (minMinutes > mostLikelyMinutes || mostLikelyMinutes > maxMinutes)
            {
                throw new PersonalProductivityIntelligenceDomainException(
                    "PersonalProductivityIntelligence:Validation:InvalidTimeEstimate",
                    "Time estimates must satisfy: min <= mostLikely <= max.");
            }

            MinMinutes = minMinutes;
            MostLikelyMinutes = mostLikelyMinutes;
            MaxMinutes = maxMinutes;
        }

        /// <summary>
        /// Creates a simple time estimate with the same value for min, most likely, and max
        /// </summary>
        /// <param name="minutes">Minutes for all estimates</param>
        /// <returns>A new TimeEstimate</returns>
        public static TimeEstimate Simple(int minutes)
        {
            return new TimeEstimate(minutes, minutes, minutes);
        }

        /// <summary>
        /// Formats the time estimate as a human-readable string
        /// </summary>
        /// <returns>Formatted time estimate</returns>
        public string Format()
        {
            if (MinMinutes == MostLikelyMinutes && MostLikelyMinutes == MaxMinutes)
            {
                return FormatMinutes(MinMinutes);
            }
            
            return $"{FormatMinutes(MinMinutes)} - {FormatMinutes(MaxMinutes)} (likely {FormatMinutes(MostLikelyMinutes)})";
        }

        private string FormatMinutes(int minutes)
        {
            if (minutes < 60)
            {
                return $"{minutes} min";
            }
            
            int hours = minutes / 60;
            int remainingMinutes = minutes % 60;
            
            if (remainingMinutes == 0)
            {
                return $"{hours} hr";
            }
            
            return $"{hours} hr {remainingMinutes} min";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MinMinutes;
            yield return MostLikelyMinutes;
            yield return MaxMinutes;
        }
    }
}
