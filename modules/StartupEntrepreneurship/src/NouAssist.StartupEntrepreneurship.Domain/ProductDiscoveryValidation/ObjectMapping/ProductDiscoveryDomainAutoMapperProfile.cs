namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ObjectMapping
{
    /// <summary>
    /// AutoMapper profile for Product Discovery domain
    /// </summary>
    public class ProductDiscoveryDomainAutoMapperProfile : Profile
    {
        public ProductDiscoveryDomainAutoMapperProfile()
        {
            CreateMap<CustomerInterview, CustomerInterviewCreatedEto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ScheduledDate, opt => opt.MapFrom(src => src.ScheduledDate))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId));

            CreateMap<CustomerInterview, CustomerInterviewCompletedEto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.CompletedDate, opt => opt.MapFrom(src => src.CompletedDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.InsightCount, opt => opt.MapFrom(src => src.Insights.Count))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId));

            CreateMap<ProblemHypothesis, ProblemHypothesisValidatedEto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.HypothesisStatement, opt => opt.MapFrom(src => src.Statement.ToFullStatement()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ValidatedDate, opt => opt.MapFrom(src => src.ValidatedDate ?? DateTime.UtcNow))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId));

            CreateMap<FeatureDefinition, FeatureDefinitionPrioritizedEto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority))
                .ForMember(dest => dest.MvpStatus, opt => opt.MapFrom(src => src.MvpStatus))
                .ForMember(dest => dest.PrioritizedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.TenantId, opt => opt.MapFrom(src => src.TenantId));

            // Value object mappings
            CreateMap<InterviewGuide, InterviewGuide>()
                .ConstructUsing(src => new InterviewGuide(src.Objective, src.Questions, src.EstimatedDuration));

            CreateMap<FeedbackTag, FeedbackTag>()
                .ConstructUsing(src => new FeedbackTag(src.Name, src.Color, src.Category));

            CreateMap<HypothesisStatement, HypothesisStatement>()
                .ConstructUsing(src => new HypothesisStatement(src.Problem, src.Target, src.Solution, src.ExpectedOutcome));

            CreateMap<ValidationMetric, ValidationMetric>()
                .ConstructUsing(src => new ValidationMetric(src.Name, src.Description, src.TargetValue, src.Unit, src.ActualValue));

            CreateMap<PrototypeLink, PrototypeLink>()
                .ConstructUsing(src => new PrototypeLink(src.Url, src.Title, src.Type, src.AccessInstructions));

            CreateMap<FeedbackRating, FeedbackRating>()
                .ConstructUsing(src => new FeedbackRating(src.Value, src.MaxValue, src.Category, src.Comment));

            CreateMap<PriorityInput, PriorityInput>()
                .ConstructUsing(src => new PriorityInput(src.Impact, src.Effort, src.Confidence, src.Rationale));

            CreateMap<ProblemSolutionFitMetric, ProblemSolutionFitMetric>()
                .ConstructUsing(src => new ProblemSolutionFitMetric(
                    src.ProblemClarity, src.SolutionEffectiveness, 
                    src.CustomerValidation, src.MarketDemand, src.Notes));
        }
    }
}
