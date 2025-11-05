using System;
using NouAssist.SharedKernel.Enums;
using NouAssist.SharedKernel.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.SharedKernel.Services
{
    public class EisenhowerMatrixService : DomainService
    {
        public EisenhowerQuadrant DetermineQuadrant(ImportanceScore importance, UrgencyScore urgency)
        {
            return importance.DetermineQuadrant(urgency);
        }

        public EisenhowerQuadrant DetermineQuadrant(double importance, double urgency)
        {
            var importanceScore = new ImportanceScore(importance);
            var urgencyScore = new UrgencyScore(urgency);
            return importanceScore.DetermineQuadrant(urgencyScore);
        }

        public string GetQuadrantDescription(EisenhowerQuadrant quadrant)
        {
            return quadrant switch
            {
                EisenhowerQuadrant.Q1DoFirst => "Do First - Urgent and Important",
                EisenhowerQuadrant.Q2Schedule => "Schedule - Important but Not Urgent",
                EisenhowerQuadrant.Q3Delegate => "Delegate - Urgent but Not Important",
                EisenhowerQuadrant.Q4Eliminate => "Eliminate - Neither Urgent nor Important",
                _ => "Unknown"
            };
        }

        public string GetQuadrantAction(EisenhowerQuadrant quadrant)
        {
            return quadrant switch
            {
                EisenhowerQuadrant.Q1DoFirst => "Do it now - these are crises and deadlines",
                EisenhowerQuadrant.Q2Schedule => "Schedule it - plan when you'll do this",
                EisenhowerQuadrant.Q3Delegate => "Delegate it - let someone else handle this",
                EisenhowerQuadrant.Q4Eliminate => "Eliminate it - stop doing these activities",
                _ => "Unknown"
            };
        }

        public (double importance, double urgency) CalculateScoresFromDeadline(
            DateTime? deadline,
            double baseImportance = 0.5)
        {
            if (!deadline.HasValue)
                return (baseImportance, 0.3);

            var daysUntilDeadline = (deadline.Value - Clock.Now).TotalDays;

            var urgency = daysUntilDeadline switch
            {
                <= 1 => 0.95,
                <= 3 => 0.85,
                <= 7 => 0.70,
                <= 14 => 0.50,
                <= 30 => 0.30,
                _ => 0.15
            };

            return (baseImportance, urgency);
        }
    }
}
