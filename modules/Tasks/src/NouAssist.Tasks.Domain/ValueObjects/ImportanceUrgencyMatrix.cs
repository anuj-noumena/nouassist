using System;
using System.Collections.Generic;
using NouAssist.Tasks.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects
{
    public class ImportanceUrgencyMatrix : ValueObject
    {
        public double Importance { get; private set; }
        public double Urgency { get; private set; }
        public EisenhowerQuadrant Quadrant { get; private set; }

        private ImportanceUrgencyMatrix() { }

        public ImportanceUrgencyMatrix(double importance, double urgency)
        {
            if (importance < 0.0 || importance > 1.0)
            {
                throw new BusinessException("Tasks:ImportanceOutOfRange", 
                    "Importance must be between 0.0 and 1.0");
            }

            if (urgency < 0.0 || urgency > 1.0)
            {
                throw new BusinessException("Tasks:UrgencyOutOfRange", 
                    "Urgency must be between 0.0 and 1.0");
            }

            Importance = importance;
            Urgency = urgency;
            Quadrant = CalculateQuadrant(importance, urgency);
        }

        private static EisenhowerQuadrant CalculateQuadrant(double importance, double urgency)
        {
            if (importance >= 0.5 && urgency >= 0.5)
                return EisenhowerQuadrant.Q1DoFirst;
            if (importance >= 0.5 && urgency < 0.5)
                return EisenhowerQuadrant.Q2Schedule;
            if (importance < 0.5 && urgency >= 0.5)
                return EisenhowerQuadrant.Q3Delegate;
            return EisenhowerQuadrant.Q4Eliminate;
        }

        public bool IsHighPriority => Quadrant == EisenhowerQuadrant.Q1DoFirst;
        public bool ShouldSchedule => Quadrant == EisenhowerQuadrant.Q2Schedule;
        public bool ShouldDelegate => Quadrant == EisenhowerQuadrant.Q3Delegate;
        public bool ShouldEliminate => Quadrant == EisenhowerQuadrant.Q4Eliminate;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Importance;
            yield return Urgency;
            yield return Quadrant;
        }
    }
}
