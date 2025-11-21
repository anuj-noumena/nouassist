using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Constants;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Exceptions;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.ValueObjects
{
    /// <summary>
    /// Element of a go-to-market plan
    /// </summary>
    public class GtmPlanElement : ValueObject
    {
        public GtmChannelType ChannelType { get; private set; }
        public string ChannelName { get; private set; }
        public string Description { get; private set; }
        public string TargetAudience { get; private set; }
        public decimal EstimatedCost { get; private set; }
        public string Currency { get; private set; }
        public int Priority { get; private set; } // 1 = highest priority
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public List<string> KeyMetrics { get; private set; }

        protected GtmPlanElement()
        {
            KeyMetrics = new List<string>();
        }

        public GtmPlanElement(
            GtmChannelType channelType,
            string channelName,
            string description,
            string targetAudience,
            decimal estimatedCost,
            string currency,
            int priority,
            DateTime startDate,
            DateTime? endDate = null,
            List<string> keyMetrics = null)
        {
            ChannelType = channelType;
            SetChannelName(channelName);
            SetDescription(description);
            SetTargetAudience(targetAudience);
            SetEstimatedCost(estimatedCost);
            SetCurrency(currency);
            SetPriority(priority);
            StartDate = startDate;
            EndDate = endDate;
            KeyMetrics = keyMetrics ?? new List<string>();
        }

        private void SetChannelName(string channelName)
        {
            ChannelName = Check.NotNullOrWhiteSpace(
                channelName,
                nameof(channelName),
                maxLength: BusinessModelManagementConsts.GoToMarket.MaxChannelNameLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: BusinessModelManagementConsts.GoToMarket.MaxChannelDescriptionLength
            );
        }

        private void SetTargetAudience(string targetAudience)
        {
            TargetAudience = Check.NotNullOrWhiteSpace(
                targetAudience,
                nameof(targetAudience),
                maxLength: BusinessModelManagementConsts.GoToMarket.MaxTargetAudienceLength
            );
        }

        private void SetEstimatedCost(decimal estimatedCost)
        {
            if (estimatedCost < 0)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:Validation:PositiveNumber")
                    .WithData("propertyName", "EstimatedCost");
            }
            EstimatedCost = estimatedCost;
        }

        private void SetCurrency(string currency)
        {
            Currency = Check.NotNullOrWhiteSpace(currency, nameof(currency), maxLength: 3);
        }

        private void SetPriority(int priority)
        {
            if (priority < 1)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "Priority")
                    .WithData("value", priority);
            }
            Priority = priority;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ChannelType;
            yield return ChannelName;
            yield return Description;
            yield return TargetAudience;
            yield return EstimatedCost;
            yield return Currency;
            yield return Priority;
            yield return StartDate.Date;
            yield return EndDate?.Date;
            foreach (var metric in KeyMetrics.OrderBy(m => m))
            {
                yield return metric;
            }
        }
    }
}
