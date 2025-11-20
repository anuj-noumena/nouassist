using System;
using AutoMapper;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Contracts.Dtos;
using NouAssist.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.Portfolios.Dtos;
using NouAssist.ContractPortfolioManagement.PortfolioRoadmaps.Dtos;
using NouAssist.ContractPortfolioManagement.ValueObjects;

namespace NouAssist.ContractPortfolioManagement.Mapping;

/// <summary>
/// AutoMapper profile for Contract Portfolio Management module
/// </summary>
public class ContractPortfolioManagementApplicationAutoMapperProfile : Profile
{
    public ContractPortfolioManagementApplicationAutoMapperProfile()
    {
        #region Contract Mappings

        // Contract Aggregate
        CreateMap<ContractAggregate, ContractDto>()
            .ForMember(d => d.TemplateReference, opt => opt.MapFrom(s => s.TemplateReference))
            .ForMember(d => d.MainDocument, opt => opt.MapFrom(s => s.MainDocumentId))
            .ForMember(d => d.LatestVersion, opt => opt.MapFrom(s => s.GetLatestVersion()));

        CreateMap<ContractAggregate, ContractListDto>();

        // Contract Value Objects
        CreateMap<ContractTemplateReference, ContractTemplateReferenceDto>();
        CreateMap<CreateContractTemplateReferenceDto, ContractTemplateReference>()
            .ConstructUsing(dto => new ContractTemplateReference(dto.TemplateId, dto.TemplateName, dto.Version));

        CreateMap<DocumentId, DocumentIdDto>();
        CreateMap<CreateDocumentIdDto, DocumentId>()
            .ConstructUsing(dto => new DocumentId(dto.Id, dto.RepositoryType));

        CreateMap<VersionMetadata, VersionMetadataDto>();
        
        CreateMap<AmendmentDetails, AmendmentDetailsDto>();
        CreateMap<CreateContractAmendmentDto, AmendmentDetails>()
            .ConstructUsing(dto => new AmendmentDetails(dto.Title, dto.Description, dto.EffectiveDate, dto.AffectedSections));

        CreateMap<AlertCondition, AlertConditionDto>();
        CreateMap<CreateContractAlertDto, AlertCondition>()
            .ConstructUsing(dto => new AlertCondition(dto.TriggerDate, dto.DaysInAdvance, dto.IsRecurring, dto.Frequency));

        // Contract Entities
        CreateMap<ContractVersion, ContractVersionDto>();
        CreateMap<ContractParty, ContractPartyDto>();
        CreateMap<ContractAmendment, ContractAmendmentDto>();
        CreateMap<ContractAlert, ContractAlertDto>();
        CreateMap<ContractClause, ContractClauseDto>();

        #endregion

        #region Portfolio Mappings

        // Portfolio Aggregate
        CreateMap<PortfolioAggregate, PortfolioDto>();
        CreateMap<PortfolioAggregate, PortfolioListDto>()
            .ForMember(d => d.ItemCount, opt => opt.MapFrom(s => s.Items.Count));
        
        CreateMap<PortfolioAggregate, PortfolioPerformanceReportDto>()
            .ForMember(d => d.OverallProgress, opt => opt.Ignore())
            .ForMember(d => d.RiskScore, opt => opt.MapFrom(s => s.CalculateOverallRiskScore()))
            .ForMember(d => d.CompliancePercentage, opt => opt.MapFrom(s => s.CalculateOverallCompliancePercentage()))
            .ForMember(d => d.TotalItems, opt => opt.MapFrom(s => s.Items.Count));

        // Portfolio Value Objects
        CreateMap<PortfolioGoal, PortfolioGoalDto>();
        CreateMap<CreatePortfolioGoalDto, PortfolioGoal>()
            .ConstructUsing(dto => new PortfolioGoal(dto.Title, dto.Description, dto.TargetDate));

        CreateMap<PerformanceMetric, PerformanceMetricDto>();
        CreateMap<CreatePerformanceMetricDto, PerformanceMetric>()
            .ConstructUsing(dto => new PerformanceMetric(dto.Name, dto.Description, dto.Target, dto.Actual, dto.Unit, DateTime.Now));

        CreateMap<ItemReference, ItemReferenceDto>();
        CreateMap<AddPortfolioItemDto, ItemReference>()
            .ConstructUsing(dto => new ItemReference(dto.ItemId, dto.ItemType, dto.ItemName));

        CreateMap<RiskAssessmentData, RiskAssessmentDataDto>();
        CreateMap<CreatePortfolioRiskDto, RiskAssessmentData>()
            .ConstructUsing(dto => new RiskAssessmentData(dto.Level, dto.Description, dto.MitigationPlan, dto.Probability, dto.Impact, DateTime.Now));

        CreateMap<ComplianceRule, ComplianceRuleDto>();
        
        CreateMap<SharingLink, SharingLinkDto>()
            .ForMember(d => d.IsExpired, opt => opt.MapFrom(s => s.IsExpired(DateTime.Now)));
        
        CreateMap<CreateSharingLinkDto, AccessPermissions>()
            .ConstructUsing(dto => new AccessPermissions(dto.CanView, dto.CanEdit, dto.CanShare, dto.CanDelete));
            
        CreateMap<AccessPermissions, AccessPermissionsDto>();

        // Portfolio Entities
        CreateMap<PortfolioItem, PortfolioItemDto>();
        CreateMap<PortfolioCategory, PortfolioCategoryDto>()
            .ForMember(d => d.ItemCount, opt => opt.Ignore());
        CreateMap<PortfolioRisk, PortfolioRiskDto>();
        CreateMap<PortfolioComplianceRecord, PortfolioComplianceRecordDto>();

        #endregion

        #region Portfolio Roadmap Mappings

        // Roadmap Aggregate
        CreateMap<PortfolioRoadmapAggregate, PortfolioRoadmapDto>()
            .ForMember(d => d.OverallProgress, opt => opt.MapFrom(s => s.CalculateOverallProgress()));
        
        CreateMap<PortfolioRoadmapAggregate, PortfolioRoadmapListDto>()
            .ForMember(d => d.MilestoneCount, opt => opt.MapFrom(s => s.Milestones.Count))
            .ForMember(d => d.OverallProgress, opt => opt.MapFrom(s => s.CalculateOverallProgress()))
            .ForMember(d => d.PortfolioName, opt => opt.Ignore());

        // Roadmap Value Objects
        CreateMap<MilestoneDate, MilestoneDateDto>();
        CreateMap<ResourceSkillRequirement, ResourceSkillRequirementDto>();
        CreateMap<CreateResourceSkillRequirementDto, ResourceSkillRequirement>()
            .ConstructUsing(dto => new ResourceSkillRequirement(dto.SkillName, dto.RequiredLevel, dto.IsMandatory, dto.Description));

        // Roadmap Entities
        CreateMap<RoadmapMilestone, RoadmapMilestoneDto>()
            .ForMember(d => d.OutgoingDependencies, opt => opt.Ignore())
            .ForMember(d => d.IncomingDependencies, opt => opt.Ignore());
            
        CreateMap<CrossItemDependency, CrossItemDependencyDto>()
            .ForMember(d => d.SourceMilestoneTitle, opt => opt.Ignore())
            .ForMember(d => d.TargetMilestoneTitle, opt => opt.Ignore());

        #endregion
    }
}
