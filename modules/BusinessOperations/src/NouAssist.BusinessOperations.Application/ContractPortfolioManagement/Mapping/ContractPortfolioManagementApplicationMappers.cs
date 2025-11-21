using System;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Contract;
using NouAssist.BusinessOperations.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.BusinessOperations.ContractPortfolioManagement.ValueObjects;
using NouAssist.ContractPortfolioManagement.Aggregates.Contract;
using NouAssist.ContractPortfolioManagement.Aggregates.Portfolio;
using NouAssist.ContractPortfolioManagement.Contracts.Dtos;
using NouAssist.ContractPortfolioManagement.Entities.Contract;
using NouAssist.ContractPortfolioManagement.Entities.Portfolio;
using NouAssist.ContractPortfolioManagement.Portfolios.Dtos;
using NouAssist.ContractPortfolioManagement.PortfolioRoadmaps.Dtos;
using NouAssist.ContractPortfolioManagement.ValueObjects;
using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace NouAssist.ContractPortfolioManagement.Mapping
{
    #region Contract Mappers

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractAggregateToContractDtoMapper : MapperBase<ContractAggregate, ContractDto>
    {
        public override partial ContractDto Map(ContractAggregate source);

        public override partial void Map(ContractAggregate source, ContractDto destination);

        public override void AfterMap(ContractAggregate source, ContractDto destination)
        {
            destination.TemplateReference = new ContractTemplateReferenceDto
            {
                TemplateId = source.TemplateReference.TemplateId,
                TemplateName = source.TemplateReference.TemplateName,
                Version = source.TemplateReference.Version
            };
            destination.MainDocument = new DocumentIdDto
            {
                Id = source.MainDocumentId.Id,
                RepositoryType = source.MainDocumentId.RepositoryType
            };
            destination.LatestVersion = source.GetLatestVersion() != null ? new ContractVersionDto
            {
                Id = source.GetLatestVersion().Id,
                ContractId = source.GetLatestVersion().ContractId,
                VersionNumber = source.GetLatestVersion().VersionNumber,
                VersionDate = source.GetLatestVersion().VersionDate,
                EffectiveDate = source.GetLatestVersion().EffectiveDate,
                ExpirationDate = source.GetLatestVersion().ExpirationDate,
                Status = source.GetLatestVersion().Status,
                DocumentId = source.GetLatestVersion().DocumentId,
                ChangeNotes = source.GetLatestVersion().ChangeNotes
            } : null;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractAggregateToContractListDtoMapper : MapperBase<ContractAggregate, ContractListDto>
    {
        public override partial ContractListDto Map(ContractAggregate source);

        public override partial void Map(ContractAggregate source, ContractListDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractTemplateReferenceToContractTemplateReferenceDtoMapper : MapperBase<ContractTemplateReference, ContractTemplateReferenceDto>
    {
        public override partial ContractTemplateReferenceDto Map(ContractTemplateReference source);

        public override partial void Map(ContractTemplateReference source, ContractTemplateReferenceDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class DocumentIdToDocumentIdDtoMapper : MapperBase<DocumentId, DocumentIdDto>
    {
        public override partial DocumentIdDto Map(DocumentId source);

        public override partial void Map(DocumentId source, DocumentIdDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class VersionMetadataToVersionMetadataDtoMapper : MapperBase<VersionMetadata, VersionMetadataDto>
    {
        public override partial VersionMetadataDto Map(VersionMetadata source);

        public override partial void Map(VersionMetadata source, VersionMetadataDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AmendmentDetailsToAmendmentDetailsDtoMapper : MapperBase<AmendmentDetails, AmendmentDetailsDto>
    {
        public override partial AmendmentDetailsDto Map(AmendmentDetails source);

        public override partial void Map(AmendmentDetails source, AmendmentDetailsDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AlertConditionToAlertConditionDtoMapper : MapperBase<AlertCondition, AlertConditionDto>
    {
        public override partial AlertConditionDto Map(AlertCondition source);

        public override partial void Map(AlertCondition source, AlertConditionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractVersionToContractVersionDtoMapper : MapperBase<ContractVersion, ContractVersionDto>
    {
        public override partial ContractVersionDto Map(ContractVersion source);

        public override partial void Map(ContractVersion source, ContractVersionDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractPartyToContractPartyDtoMapper : MapperBase<ContractParty, ContractPartyDto>
    {
        public override partial ContractPartyDto Map(ContractParty source);

        public override partial void Map(ContractParty source, ContractPartyDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractAmendmentToContractAmendmentDtoMapper : MapperBase<ContractAmendment, ContractAmendmentDto>
    {
        public override partial ContractAmendmentDto Map(ContractAmendment source);

        public override partial void Map(ContractAmendment source, ContractAmendmentDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractAlertToContractAlertDtoMapper : MapperBase<ContractAlert, ContractAlertDto>
    {
        public override partial ContractAlertDto Map(ContractAlert source);

        public override partial void Map(ContractAlert source, ContractAlertDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ContractClauseToContractClauseDtoMapper : MapperBase<ContractClause, ContractClauseDto>
    {
        public override partial ContractClauseDto Map(ContractClause source);

        public override partial void Map(ContractClause source, ContractClauseDto destination);
    }

    #endregion

    #region Portfolio Mappers

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioAggregateToPortfolioDtoMapper : MapperBase<PortfolioAggregate, PortfolioDto>
    {
        public override partial PortfolioDto Map(PortfolioAggregate source);

        public override partial void Map(PortfolioAggregate source, PortfolioDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioAggregateToPortfolioListDtoMapper : MapperBase<PortfolioAggregate, PortfolioListDto>
    {
        public override partial PortfolioListDto Map(PortfolioAggregate source);

        public override partial void Map(PortfolioAggregate source, PortfolioListDto destination);

        public override void AfterMap(PortfolioAggregate source, PortfolioListDto destination)
        {
            destination.ItemCount = source.Items.Count;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioAggregateToPortfolioPerformanceReportDtoMapper : MapperBase<PortfolioAggregate, PortfolioPerformanceReportDto>
    {
        [MapperIgnoreTarget(nameof(PortfolioPerformanceReportDto.OverallProgress))]
        public override partial PortfolioPerformanceReportDto Map(PortfolioAggregate source);

        public override partial void Map(PortfolioAggregate source, PortfolioPerformanceReportDto destination);

        public override void AfterMap(PortfolioAggregate source, PortfolioPerformanceReportDto destination)
        {
            destination.RiskScore = source.CalculateOverallRiskScore();
            destination.CompliancePercentage = source.CalculateOverallCompliancePercentage();
            destination.TotalItems = source.Items.Count;
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioGoalToPortfolioGoalDtoMapper : MapperBase<PortfolioGoal, PortfolioGoalDto>
    {
        public override partial PortfolioGoalDto Map(PortfolioGoal source);

        public override partial void Map(PortfolioGoal source, PortfolioGoalDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PerformanceMetricToPerformanceMetricDtoMapper : MapperBase<PerformanceMetric, PerformanceMetricDto>
    {
        public override partial PerformanceMetricDto Map(PerformanceMetric source);

        public override partial void Map(PerformanceMetric source, PerformanceMetricDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ItemReferenceToItemReferenceDtoMapper : MapperBase<ItemReference, ItemReferenceDto>
    {
        public override partial ItemReferenceDto Map(ItemReference source);

        public override partial void Map(ItemReference source, ItemReferenceDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RiskAssessmentDataToRiskAssessmentDataDtoMapper : MapperBase<RiskAssessmentData, RiskAssessmentDataDto>
    {
        public override partial RiskAssessmentDataDto Map(RiskAssessmentData source);

        public override partial void Map(RiskAssessmentData source, RiskAssessmentDataDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ComplianceRuleToComplianceRuleDtoMapper : MapperBase<ComplianceRule, ComplianceRuleDto>
    {
        public override partial ComplianceRuleDto Map(ComplianceRule source);

        public override partial void Map(ComplianceRule source, ComplianceRuleDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class SharingLinkToSharingLinkDtoMapper : MapperBase<SharingLink, SharingLinkDto>
    {
        public override partial SharingLinkDto Map(SharingLink source);

        public override partial void Map(SharingLink source, SharingLinkDto destination);

        public override void AfterMap(SharingLink source, SharingLinkDto destination)
        {
            destination.IsExpired = source.IsExpired(DateTime.Now);
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class AccessPermissionsToAccessPermissionsDtoMapper : MapperBase<AccessPermissions, AccessPermissionsDto>
    {
        public override partial AccessPermissionsDto Map(AccessPermissions source);

        public override partial void Map(AccessPermissions source, AccessPermissionsDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioItemToPortfolioItemDtoMapper : MapperBase<PortfolioItem, PortfolioItemDto>
    {
        public override partial PortfolioItemDto Map(PortfolioItem source);

        public override partial void Map(PortfolioItem source, PortfolioItemDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioCategoryToPortfolioCategoryDtoMapper : MapperBase<PortfolioCategory, PortfolioCategoryDto>
    {
        [MapperIgnoreTarget(nameof(PortfolioCategoryDto.ItemCount))]
        public override partial PortfolioCategoryDto Map(PortfolioCategory source);

        public override partial void Map(PortfolioCategory source, PortfolioCategoryDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioRiskToPortfolioRiskDtoMapper : MapperBase<PortfolioRisk, PortfolioRiskDto>
    {
        public override partial PortfolioRiskDto Map(PortfolioRisk source);

        public override partial void Map(PortfolioRisk source, PortfolioRiskDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioComplianceRecordToPortfolioComplianceRecordDtoMapper : MapperBase<PortfolioComplianceRecord, PortfolioComplianceRecordDto>
    {
        public override partial PortfolioComplianceRecordDto Map(PortfolioComplianceRecord source);

        public override partial void Map(PortfolioComplianceRecord source, PortfolioComplianceRecordDto destination);
    }

    #endregion

    #region Portfolio Roadmap Mappers

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioRoadmapAggregateToPortfolioRoadmapDtoMapper : MapperBase<PortfolioRoadmapAggregate, PortfolioRoadmapDto>
    {
        public override partial PortfolioRoadmapDto Map(PortfolioRoadmapAggregate source);

        public override partial void Map(PortfolioRoadmapAggregate source, PortfolioRoadmapDto destination);

        public override void AfterMap(PortfolioRoadmapAggregate source, PortfolioRoadmapDto destination)
        {
            destination.OverallProgress = source.CalculateOverallProgress();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class PortfolioRoadmapAggregateToPortfolioRoadmapListDtoMapper : MapperBase<PortfolioRoadmapAggregate, PortfolioRoadmapListDto>
    {
        [MapperIgnoreTarget(nameof(PortfolioRoadmapListDto.PortfolioName))]
        public override partial PortfolioRoadmapListDto Map(PortfolioRoadmapAggregate source);

        public override partial void Map(PortfolioRoadmapAggregate source, PortfolioRoadmapListDto destination);

        public override void AfterMap(PortfolioRoadmapAggregate source, PortfolioRoadmapListDto destination)
        {
            destination.MilestoneCount = source.Milestones.Count;
            destination.OverallProgress = source.CalculateOverallProgress();
        }
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class MilestoneDateToMilestoneDateDtoMapper : MapperBase<MilestoneDate, MilestoneDateDto>
    {
        public override partial MilestoneDateDto Map(MilestoneDate source);

        public override partial void Map(MilestoneDate source, MilestoneDateDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class ResourceSkillRequirementToResourceSkillRequirementDtoMapper : MapperBase<ResourceSkillRequirement, ResourceSkillRequirementDto>
    {
        public override partial ResourceSkillRequirementDto Map(ResourceSkillRequirement source);

        public override partial void Map(ResourceSkillRequirement source, ResourceSkillRequirementDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class RoadmapMilestoneToRoadmapMilestoneDtoMapper : MapperBase<RoadmapMilestone, RoadmapMilestoneDto>
    {
        [MapperIgnoreTarget(nameof(RoadmapMilestoneDto.OutgoingDependencies))]
        [MapperIgnoreTarget(nameof(RoadmapMilestoneDto.IncomingDependencies))]
        public override partial RoadmapMilestoneDto Map(RoadmapMilestone source);

        public override partial void Map(RoadmapMilestone source, RoadmapMilestoneDto destination);
    }

    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CrossItemDependencyToCrossItemDependencyDtoMapper : MapperBase<CrossItemDependency, CrossItemDependencyDto>
    {
        [MapperIgnoreTarget(nameof(CrossItemDependencyDto.SourceMilestoneTitle))]
        [MapperIgnoreTarget(nameof(CrossItemDependencyDto.TargetMilestoneTitle))]
        public override partial CrossItemDependencyDto Map(CrossItemDependency source);

        public override partial void Map(CrossItemDependency source, CrossItemDependencyDto destination);
    }

    #endregion
}
