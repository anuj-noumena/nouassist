using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Enums;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract
{
    /// <summary>
    /// Represents an alert/reminder for a contract
    /// </summary>
    public class ContractAlert : Entity<Guid>
    {
        public Guid ContractId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ContractAlertType AlertType { get; private set; }
        public AlertCondition Condition { get; private set; }
        public bool IsTriggered { get; private set; }
        public DateTime? LastTriggeredTime { get; private set; }

        protected ContractAlert() { }

        public ContractAlert(
            Guid id,
            Guid contractId,
            string title,
            string description,
            ContractAlertType alertType,
            AlertCondition condition)
            : base(id)
        {
            ContractId = contractId;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            AlertType = alertType;
            Condition = condition;
            IsTriggered = false;
            LastTriggeredTime = null;
        }

        public void UpdateAlert(
            string title,
            string description,
            ContractAlertType alertType,
            AlertCondition condition)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Description = description;
            AlertType = alertType;
            Condition = condition;
            
            // Reset triggered state if condition changes
            IsTriggered = false;
            LastTriggeredTime = null;
        }

        public void MarkAsTriggered(DateTime triggerTime)
        {
            IsTriggered = true;
            LastTriggeredTime = triggerTime;
        }

        public void Reset()
        {
            IsTriggered = false;
            LastTriggeredTime = null;
        }

        public bool ShouldTrigger(DateTime currentDate)
        {
            if (IsTriggered && !Condition.IsRecurring)
            {
                return false;
            }

            if (IsTriggered && Condition.IsRecurring && LastTriggeredTime.HasValue)
            {
                // For recurring alerts, check if enough time has passed based on frequency
                switch (Condition.Frequency?.ToLower())
                {
                    case "daily":
                        return (currentDate - LastTriggeredTime.Value).TotalDays >= 1;
                    case "weekly":
                        return (currentDate - LastTriggeredTime.Value).TotalDays >= 7;
                    case "monthly":
                        return (currentDate - LastTriggeredTime.Value).TotalDays >= 30;
                    case "quarterly":
                        return (currentDate - LastTriggeredTime.Value).TotalDays >= 90;
                    case "yearly":
                        return (currentDate - LastTriggeredTime.Value).TotalDays >= 365;
                    default:
                        return false;
                }
            }

            // For normal alerts, check if current date is within the alert window
            var triggerWindow = Condition.TriggerDate.AddDays(-Condition.DaysInAdvance);
            return currentDate >= triggerWindow && currentDate <= Condition.TriggerDate;
        }
    }
}
