using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class TimeEstimate : ValueObject
    {
        public int EstimatedMinutes { get; private set; }
        public int? OptimisticMinutes { get; private set; }
        public int? PessimisticMinutes { get; private set; }
        public DateTime EstimatedAt { get; private set; }
        public Guid? EstimatedBy { get; private set; }

        private TimeEstimate() { }

        public TimeEstimate(
            int estimatedMinutes,
            int? optimisticMinutes = null,
            int? pessimisticMinutes = null,
            Guid? estimatedBy = null)
        {
            if (estimatedMinutes <= 0)
            {
                throw new BusinessException("Tasks:InvalidEstimate");
            }

            EstimatedMinutes = estimatedMinutes;
            OptimisticMinutes = optimisticMinutes;
            PessimisticMinutes = pessimisticMinutes;
            EstimatedAt = DateTime.UtcNow;
            EstimatedBy = estimatedBy;
        }

        public TimeSpan EstimatedDuration => TimeSpan.FromMinutes(EstimatedMinutes);
        public TimeSpan? OptimisticDuration => OptimisticMinutes.HasValue 
            ? TimeSpan.FromMinutes(OptimisticMinutes.Value) 
            : null;
        public TimeSpan? PessimisticDuration => PessimisticMinutes.HasValue 
            ? TimeSpan.FromMinutes(PessimisticMinutes.Value) 
            : null;

        // PERT estimate: (Optimistic + 4*Realistic + Pessimistic) / 6
        public int CalculatePertEstimate()
        {
            if (!OptimisticMinutes.HasValue || !PessimisticMinutes.HasValue)
            {
                return EstimatedMinutes;
            }

            return (OptimisticMinutes.Value + (4 * EstimatedMinutes) + PessimisticMinutes.Value) / 6;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return EstimatedMinutes;
            yield return OptimisticMinutes;
            yield return PessimisticMinutes;
            yield return EstimatedAt;
            yield return EstimatedBy;
        }
    }
}
