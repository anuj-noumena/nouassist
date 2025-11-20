namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Mapping
{
    /// <summary>
    /// AutoMapper profile for the Workflow Engine application layer
    /// </summary>
    public class WorkflowEngineApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// Creates a new instance of <see cref="WorkflowEngineApplicationAutoMapperProfile"/>
        /// </summary>
        public WorkflowEngineApplicationAutoMapperProfile()
        {
            // WorkflowDefinition mappings
            CreateMap<WorkflowDefinition, WorkflowDefinitionDto>()
                .ForMember(dest => dest.PerformanceMetrics, opt => opt.MapFrom(src => src.PerformanceMetrics));

            // WorkflowVersion mappings
            CreateMap<WorkflowVersion, WorkflowVersionDto>()
                .ForMember(dest => dest.BpmnDiagram, opt => opt.MapFrom(src => src.BpmnDiagram))
                .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
                .ForMember(dest => dest.Conditions, opt => opt.MapFrom(src => src.Conditions));

            // ActivityDefinition mappings
            CreateMap<ActivityDefinition, ActivityDefinitionDto>();

            // ConditionDefinition mappings
            CreateMap<ConditionDefinition, ConditionDefinitionDto>()
                .ForMember(dest => dest.Expression, opt => opt.MapFrom(src => src.Expression));

            // WorkflowTemplate mappings
            CreateMap<WorkflowTemplate, WorkflowTemplateDto>()
                .ForMember(dest => dest.BpmnDiagram, opt => opt.MapFrom(src => src.BpmnDiagram));

            // WorkflowInstance mappings
            CreateMap<WorkflowInstance, WorkflowInstanceDto>()
                .ForMember(dest => dest.WorkflowDefinitionName, opt => opt.Ignore())
                .ForMember(dest => dest.ActiveTasksCount, opt => opt.Ignore())
                .ForMember(dest => dest.TotalTasksCount, opt => opt.Ignore());

            CreateMap<WorkflowInstance, WorkflowInstanceDetailDto>()
                .IncludeBase<WorkflowInstance, WorkflowInstanceDto>()
                .ForMember(dest => dest.ActivityInstances, opt => opt.MapFrom(src => src.ActivityInstances))
                .ForMember(dest => dest.ExecutionLogs, opt => opt.MapFrom(src => src.ExecutionLogs));

            // ActivityInstance mappings
            CreateMap<ActivityInstance, ActivityInstanceDto>()
                .ForMember(dest => dest.Assignee, opt => opt.MapFrom(src => src.Assignee));

            // WorkflowExecutionLog mappings
            CreateMap<WorkflowExecutionLog, WorkflowExecutionLogDto>();

            // Value Object mappings
            CreateMap<BpmnDiagram, BpmnDiagramDto>();
            CreateMap<ConditionExpression, ConditionExpressionDto>();
            CreateMap<AssigneeReference, AssigneeReferenceDto>();
            CreateMap<PerformanceMetrics, PerformanceMetricsDto>()
                .ForMember(dest => dest.BottleneckActivityName, opt => opt.Ignore());
        }
    }
}
