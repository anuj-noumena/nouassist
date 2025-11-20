using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.RequirementsManagement.Specification.Requirements.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.RequirementsManagement.Specification.Requirements
{
    /// <summary>
    /// Application service interface for requirement management
    /// </summary>
    public interface IRequirementAppService : IApplicationService
    {
        // CRUD operations
        Task<RequirementDto> GetAsync(Guid id);
        Task<PagedResultDto<RequirementDto>> GetListAsync(GetRequirementListDto input);
        Task<RequirementDto> CreateAsync(CreateRequirementDto input);
        Task<RequirementDto> UpdateAsync(Guid id, UpdateRequirementDto input);
        Task DeleteAsync(Guid id);

        // Status management
        Task<RequirementDto> ChangeStatusAsync(Guid id, ChangeRequirementStatusDto input);
        Task<List<string>> GetAllowedStatusTransitionsAsync(Guid id);

        // Scope management
        Task<RequirementDto> SetScopeAsync(Guid id, SetRequirementScopeDto input);

        // Acceptance criteria management
        Task<AcceptanceCriterionDto> AddAcceptanceCriterionAsync(Guid requirementId, CreateAcceptanceCriterionDto input);
        Task<AcceptanceCriterionDto> UpdateAcceptanceCriterionAsync(Guid requirementId, Guid criterionId, UpdateAcceptanceCriterionDto input);
        Task RemoveAcceptanceCriterionAsync(Guid requirementId, Guid criterionId);
        Task ReorderAcceptanceCriteriaAsync(Guid requirementId, List<Guid> criterionIds);

        // Business rules management
        Task<AssociatedRuleDto> AddBusinessRuleAsync(Guid requirementId, CreateAssociatedRuleDto input);
        Task<AssociatedRuleDto> UpdateBusinessRuleAsync(Guid requirementId, Guid ruleId, CreateAssociatedRuleDto input);
        Task RemoveBusinessRuleAsync(Guid requirementId, Guid ruleId);

        // Constraints management
        Task<AssociatedConstraintDto> AddConstraintAsync(Guid requirementId, CreateAssociatedConstraintDto input);
        Task<AssociatedConstraintDto> UpdateConstraintAsync(Guid requirementId, Guid constraintId, CreateAssociatedConstraintDto input);
        Task RemoveConstraintAsync(Guid requirementId, Guid constraintId);

        // Data requirements management
        Task<DataModelReferenceDto> AddDataModelReferenceAsync(Guid requirementId, CreateDataModelReferenceDto input);
        Task<DataModelReferenceDto> UpdateDataModelReferenceAsync(Guid requirementId, Guid referenceId, CreateDataModelReferenceDto input);
        Task RemoveDataModelReferenceAsync(Guid requirementId, Guid referenceId);

        // Interface specifications management
        Task<InterfaceSpecificationDto> AddInterfaceSpecificationAsync(Guid requirementId, CreateInterfaceSpecificationDto input);
        Task<InterfaceSpecificationDto> UpdateInterfaceSpecificationAsync(Guid requirementId, Guid specificationId, CreateInterfaceSpecificationDto input);
        Task RemoveInterfaceSpecificationAsync(Guid requirementId, Guid specificationId);

        // Business objective linking
        Task<BusinessObjectiveLinkDto> LinkToBusinessObjectiveAsync(Guid requirementId, CreateBusinessObjectiveLinkDto input);
        Task RemoveBusinessObjectiveLinkAsync(Guid requirementId, Guid linkId);

        // Search and filtering
        Task<List<RequirementDto>> SearchAsync(string searchText, GetRequirementListDto filters = null);
        Task<List<RequirementDto>> GetByBusinessObjectiveAsync(Guid objectiveId);

        // Export and reporting
        Task<string> ExportAsync(GetRequirementListDto filters, string format = "json");
        Task<byte[]> ExportAsBinaryAsync(GetRequirementListDto filters, string format = "xlsx");
    }
}
