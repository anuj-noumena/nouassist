using System;
using System.Collections.Generic;
using NouAssist.BusinessOperations.ResourceInventoryManagement.Exceptions;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents a time slot for resource booking
    /// </summary>
    public class BookingTimeSlot : ValueObject
    {
        /// <summary>
        /// Start time of the booking
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// End time of the booking
        /// </summary>
        public DateTime EndTime { get; private set; }

        private BookingTimeSlot()
        {
        }

        /// <summary>
        /// Creates a new booking time slot
        /// </summary>
        /// <param name="startTime">Start time</param>
        /// <param name="endTime">End time</param>
        public BookingTimeSlot(DateTime startTime, DateTime endTime)
        {
            if (endTime <= startTime)
            {
                throw new RIMDomainException("RIM:Validation:InvalidDateRange");
            }

            StartTime = startTime;
            EndTime = endTime;
        }

        /// <summary>
        /// Checks if this time slot overlaps with another time slot
        /// </summary>
        /// <param name="other">Other time slot</param>
        /// <returns>True if the time slots overlap</returns>
        public bool OverlapsWith(BookingTimeSlot other)
        {
            if (other == null)
            {
                return false;
            }

            // Check if one booking starts during the other booking
            return (StartTime >= other.StartTime && StartTime < other.EndTime) ||
                   (other.StartTime >= StartTime && other.StartTime < EndTime);
        }

        /// <summary>
        /// Gets the duration of the time slot
        /// </summary>
        /// <returns>Time span duration</returns>
        public TimeSpan GetDuration()
        {
            return EndTime - StartTime;
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return StartTime;
            yield return EndTime;
        }

        /// <summary>
        /// Returns the string representation of the time slot
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return $"{StartTime:g} - {EndTime:g}";
        }
    }
}
