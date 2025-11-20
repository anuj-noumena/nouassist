using AutoMapper;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.Entities;
using NouAssist.RequirementsManagement.QualityAssurance.QualityMetrics.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.TestCycles.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.TestPlans.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.TestRuns.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.ValueObjects;

namespace NouAssist.RequirementsManagement.QualityAssurance.Mapping
{
    public class QAApplicationAutoMapperProfile : Profile
    {
        public QAApplicationAutoMapperProfile()
        {
            // TestPlan mappings
            CreateMap<TestPlan, TestPlanDto>()
                .ForMember(dest => dest.StrategyApproach, opt => opt.MapFrom(src => src.Strategy.Approach))
                .ForMember(dest => dest.StrategyCoverageDescription, opt => opt.MapFrom(src => src.Strategy.CoverageDescription))
                .ForMember(dest => dest.StrategyOutOfScope, opt => opt.MapFrom(src => src.Strategy.OutOfScope))
                .ForMember(dest => dest.EnvironmentName, opt => opt.MapFrom(src => src.Environment.Name))
                .ForMember(dest => dest.EnvironmentDescription, opt => opt.MapFrom(src => src.Environment.Description))
                .ForMember(dest => dest.EnvironmentUrl, opt => opt.MapFrom(src => src.Environment.Url))
                .ForMember(dest => dest.TestCycles, opt => opt.ExplicitExpansion());
            
            CreateMap<CreateTestPlanDto, TestPlan>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Strategy, opt => opt.MapFrom(src => 
                    new TestingStrategy(src.StrategyApproach, src.StrategyCoverageDescription, src.StrategyOutOfScope)))
                .ForMember(dest => dest.Environment, opt => opt.MapFrom(src => 
                    new EnvironmentConfiguration(src.EnvironmentName, src.EnvironmentDescription, src.EnvironmentUrl)))
                .ForMember(dest => dest.TestCycles, opt => opt.Ignore())
                .ForMember(dest => dest.InScopeRequirementIds, opt => opt.Ignore())
                .IgnoreAuditedProperties();
            
            // TestCycle mappings
            CreateMap<TestCycle, TestCycleDto>()
                .ForMember(dest => dest.TestRuns, opt => opt.ExplicitExpansion());
            
            CreateMap<CreateTestCycleDto, TestCycle>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TestRuns, opt => opt.Ignore())
                .IgnoreAuditedProperties();
            
            // TestRun mappings
            CreateMap<TestRun, TestRunDto>()
                .ForMember(dest => dest.ActualOutcomeDescription, opt => 
                    opt.MapFrom(src => src.ActualOutcome != null ? src.ActualOutcome.Description : null))
                .ForMember(dest => dest.ActualOutcomeLogData, opt => 
                    opt.MapFrom(src => src.ActualOutcome != null ? src.ActualOutcome.LogData : null));
            
            // DefectReport mappings
            CreateMap<DefectReport, DefectDto>()
                .ForMember(dest => dest.Comments, opt => opt.ExplicitExpansion());
            
            CreateMap<CreateDefectDto, DefectReport>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.AffectedRequirementIds, opt => opt.Ignore())
                .IgnoreAuditedProperties();
            
            CreateMap<DefectReport.DefectComment, DefectCommentDto>();
            
            // QualityMetricSnapshot mappings
            CreateMap<QualityMetricSnapshot, QualityMetricDto>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Metric.Type))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Metric.Value))
                .ForMember(dest => dest.Unit, opt => opt.MapFrom(src => src.Metric.Unit))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Metric.Timestamp));
            
            CreateMap<CreateQualityMetricDto, MetricValue>()
                .ConstructUsing(src => new MetricValue(src.Type, src.Value, src.Unit, DateTime.Now));
            
            // Progress mapping
            CreateMap<(int Total, int Passed, int Failed, int Blocked, int NotRun), TestProgressDto>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total))
                .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Passed))
                .ForMember(dest => dest.Failed, opt => opt.MapFrom(src => src.Failed))
                .ForMember(dest => dest.Blocked, opt => opt.MapFrom(src => src.Blocked))
                .ForMember(dest => dest.NotRun, opt => opt.MapFrom(src => src.NotRun));
        }
    }
}
