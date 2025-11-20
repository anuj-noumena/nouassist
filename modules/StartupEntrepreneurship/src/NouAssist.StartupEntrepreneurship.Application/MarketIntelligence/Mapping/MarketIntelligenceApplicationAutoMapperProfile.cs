using AutoMapper;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Aggregates;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Competitor.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Entities;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.MarketTrend.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.SwotAnalysis.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.Mapping
{
    public class MarketIntelligenceApplicationAutoMapperProfile : Profile
    {
        public MarketIntelligenceApplicationAutoMapperProfile()
        {
            // Competitor mappings
            CreateMap<Competitor, CompetitorDto>()
                .ForMember(d => d.RecentUpdateLogs, opt => opt.Ignore());
            
            CreateMap<CompetitorInfo, CompetitorInfoDto>();
            CreateMap<CompetitorUpdateLog, CompetitorUpdateLogDto>();
            
            CreateMap<CreateCompetitorInfoDto, CompetitorInfo>()
                .ConstructUsing(src => new CompetitorInfo(
                    src.Website,
                    src.Industry,
                    src.Location,
                    src.EmployeeCount,
                    src.FundingAmount,
                    src.FundingSource
                ));

            // Market Trend mappings
            CreateMap<MarketTrend, MarketTrendDto>()
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.TrendDescription.Title))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.TrendDescription.Description))
                .ForMember(d => d.Keywords, opt => opt.MapFrom(s => s.TrendDescription.Keywords))
                .ForMember(d => d.SourceUrl, opt => opt.MapFrom(s => s.TrendDescription.SourceUrl))
                .ForMember(d => d.ImpactLevel, opt => opt.MapFrom(s => s.ImpactAssessment.Level))
                .ForMember(d => d.ImpactDescription, opt => opt.MapFrom(s => s.ImpactAssessment.Description))
                .ForMember(d => d.ConfidenceScore, opt => opt.MapFrom(s => s.ImpactAssessment.ConfidenceScore))
                .ForMember(d => d.RecommendedActions, opt => opt.MapFrom(s => s.ImpactAssessment.RecommendedActions));
            
            CreateMap<TrendDataSource, TrendDataSourceDto>();
            
            // Customer Segment mappings
            CreateMap<CustomerSegmentProfile, CustomerSegmentProfileDto>();
            CreateMap<Demographics, DemographicsDto>();
            CreateMap<Psychographics, PsychographicsDto>();
            CreateMap<SegmentCharacteristics, SegmentCharacteristicsDto>();
            
            CreateMap<CreateDemographicsDto, Demographics>()
                .ConstructUsing(src => new Demographics(
                    src.AgeRange,
                    src.Gender,
                    src.Income,
                    src.Education,
                    src.Location,
                    src.Occupation
                ));
            
            CreateMap<CreatePsychographicsDto, Psychographics>()
                .ConstructUsing(src => new Psychographics(
                    src.Interests,
                    src.Values,
                    src.Lifestyle,
                    src.Personality,
                    src.Attitudes
                ));

            // SWOT Analysis mappings
            CreateMap<SwotAnalysis, SwotAnalysisDto>();
            CreateMap<SwotItem, SwotItemDto>();
            
            // Service-specific mappings
            CreateMap<Services.OpportunityAlert, OpportunityAlertDto>();
            CreateMap<Services.SwotAnalysisQualityResult, SwotAnalysisQualityDto>();
        }
    }
}
