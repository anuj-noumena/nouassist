using System;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.MarketingChannels.Dtos
{
    public class ChannelMetricDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MetricType Type { get; set; }
        public double Value { get; set; }
        public double TargetValue { get; set; }
        public double? PreviousValue { get; set; }
        public double? ChangePercentage { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
