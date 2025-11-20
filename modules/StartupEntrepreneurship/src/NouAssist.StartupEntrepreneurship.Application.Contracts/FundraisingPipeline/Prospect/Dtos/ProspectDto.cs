using System;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using Volo.Abp.Application.Dtos;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos
{
    public class ProspectDto : AuditedEntityDto<Guid>
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ContactInfoDto ContactInfo { get; set; }
        public PipelineStatusDto PipelineStatus { get; set; }
        public ProspectScoreDto Score { get; set; }
        public OpportunityForecastDto OpportunityForecast { get; set; }
        public Guid? FundraisingRoundId { get; set; }
        public string FundraisingRoundName { get; set; }
    }

    public class ContactInfoDto
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class PipelineStatusDto
    {
        public ProspectStatus Status { get; set; }
        public string StatusName => Status.ToString();
        public DateTime ChangedAt { get; set; }
        public int DaysInCurrentStatus { get; set; }
        public double ConversionProbability { get; set; }
    }

    public class ProspectScoreDto
    {
        public int EngagementScore { get; set; }
        public int FitScore { get; set; }
        public int TimelinessScore { get; set; }
        public int OverallScore { get; set; }
    }

    public class OpportunityForecastDto
    {
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal ExpectedAmount { get; set; }
        public double Probability { get; set; }
        public decimal WeightedAmount { get; set; }
        public DateTime ForecastedClosingDate { get; set; }
    }
}
