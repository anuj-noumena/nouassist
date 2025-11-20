using AutoMapper;
using NouAssist.PersonalProductivityIntelligence.Aggregates;
using NouAssist.PersonalProductivityIntelligence.Dtos.Prediction;
using NouAssist.PersonalProductivityIntelligence.Dtos.Recommendation;
using NouAssist.PersonalProductivityIntelligence.Dtos.UserProfile;
using NouAssist.PersonalProductivityIntelligence.Entities;
using NouAssist.PersonalProductivityIntelligence.ValueObjects;

namespace NouAssist.PersonalProductivityIntelligence
{
    /// <summary>
    /// AutoMapper profile for Personal Productivity Intelligence
    /// </summary>
    public class PersonalProductivityIntelligenceApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of <see cref="PersonalProductivityIntelligenceApplicationAutoMapperProfile"/>
        /// </summary>
        public PersonalProductivityIntelligenceApplicationAutoMapperProfile()
        {
            // UserProductivityProfile mappings
            CreateMap<UserProductivityProfile, UserProfileDto>()
                .ForMember(dest => dest.RecentCognitiveLoadHistory, opt => opt.MapFrom(src => 
                    src.CognitiveLoadHistory))
                .ForMember(dest => dest.Preferences, opt => opt.MapFrom(src => 
                    src.Preferences))
                .ForMember(dest => dest.RecentPerformanceData, opt => opt.MapFrom(src => 
                    src.PerformanceData))
                .ForMember(dest => dest.RecentTaskCompletions, opt => opt.MapFrom(src => 
                    src.TaskCompletionRecords))
                .ForMember(dest => dest.WorkPatternModels, opt => opt.MapFrom(src => 
                    src.WorkPatternModels));
            
            CreateMap<CognitiveLoadMetric, CognitiveLoadMetricDto>();
            CreateMap<UserPreferenceSetting, UserPreferenceSettingDto>();
            CreateMap<PerformanceDataPoint, PerformanceDataPointDto>();
            CreateMap<TaskCompletionRecord, TaskCompletionRecordDto>();
            CreateMap<WorkPatternModel, WorkPatternModelDto>();
            
            CreateMap<ExternalContextRef, ExternalContextRefDto>();
            CreateMap<ExternalContextRefDto, ExternalContextRef>()
                .ConstructUsing(src => new ExternalContextRef(
                    src.ProjectId, 
                    src.TeamId, 
                    src.DepartmentId, 
                    src.AdditionalRefType, 
                    src.AdditionalRefId));
            
            CreateMap<TaskContext, TaskContextDto>();
            CreateMap<RecordTaskCompletionDto, TaskContext>()
                .ConstructUsing(src => new TaskContext(
                    src.TaskId,
                    src.Title,
                    src.ProjectId,
                    src.TaskType,
                    src.PriorityLevel,
                    src.Tags,
                    src.DueDate));
                    
            // PredictionInstance mappings
            CreateMap<PredictionInstance, PredictionDto>();
            CreateMap<PredictionModelMetadata, PredictionModelMetadataDto>();
            CreateMap<FeatureImportance, FeatureImportanceDto>();
            
            // RecommendationInstance mappings
            CreateMap<RecommendationInstance, RecommendationDto>()
                .ForMember(dest => dest.LatestFeedback, opt => opt.MapFrom(src => 
                    src.GetMostRecentFeedback()));
            CreateMap<RecommendationRule, RecommendationRuleDto>();
            CreateMap<ExplainabilityLog, ExplainabilityLogDto>();
            CreateMap<FeedbackRecord, FeedbackRecordDto>();
        }
    }
}
