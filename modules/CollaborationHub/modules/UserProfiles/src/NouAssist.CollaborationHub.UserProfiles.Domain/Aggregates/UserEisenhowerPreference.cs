using System;
using NouAssist.Core.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace NouAssist.CollaborationHub.UserProfiles.Aggregates
{
    public class UserEisenhowerPreference : FullAuditedAggregateRoot<Guid>
    {
        public Guid UserId { get; private set; }
        public double ImportanceThreshold { get; private set; }
        public double UrgencyThreshold { get; private set; }
        public bool UseAutoClassification { get; set; }

        protected UserEisenhowerPreference() { }

        public UserEisenhowerPreference(
            Guid id, 
            Guid userId,
            double? importanceThreshold = null,
            double? urgencyThreshold = null) : base(id)
        {
            UserId = userId;
            ImportanceThreshold = importanceThreshold ?? 0.50;
            UrgencyThreshold = urgencyThreshold ?? 0.50;
            UseAutoClassification = true;

            ValidateThresholds();
        }

        public void UpdateThresholds(double? importance, double? urgency)
        {
            if (importance.HasValue)
            {
                if (importance.Value < ImportanceScore.MinValue || importance.Value > ImportanceScore.MaxValue)
                    throw new BusinessException("Users:InvalidThreshold", 
                        "Importance threshold must be between 0 and 1.");
                ImportanceThreshold = importance.Value;
            }

            if (urgency.HasValue)
            {
                if (urgency.Value < UrgencyScore.MinValue || urgency.Value > UrgencyScore.MaxValue)
                    throw new BusinessException("Users:InvalidThreshold", 
                        "Urgency threshold must be between 0 and 1.");
                UrgencyThreshold = urgency.Value;
            }
        }

        public void EnableAutoClassification()
        {
            UseAutoClassification = true;
        }

        public void DisableAutoClassification()
        {
            UseAutoClassification = false;
        }

        private void ValidateThresholds()
        {
            if (ImportanceThreshold < 0 || ImportanceThreshold > 1)
                throw new BusinessException("Users:InvalidThreshold", 
                    "Importance threshold must be between 0 and 1.");

            if (UrgencyThreshold < 0 || UrgencyThreshold > 1)
                throw new BusinessException("Users:InvalidThreshold", 
                    "Urgency threshold must be between 0 and 1.");
        }
    }
}
