using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Specification.Aggregates;
using NouAssist.RequirementsManagement.Specification.Enums;
using NouAssist.RequirementsManagement.Specification.Interfaces.Services;
using NouAssist.RequirementsManagement.Specification.Permissions;
using NouAssist.RequirementsManagement.Specification.Policies;
using NouAssist.RequirementsManagement.Specification.Repositories;
using NouAssist.RequirementsManagement.Specification.Requirements.Dtos;
using NouAssist.RequirementsManagement.Specification.Specifications;
using NouAssist.RequirementsManagement.Specification.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using Volo.Abp.Timing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Guids;

namespace NouAssist.RequirementsManagement.Specification.Requirements
{
    [Authorize(RequirementSpecificationManagementPermissions.Requirements.Default)]
    public class RequirementAppService : ApplicationService, IRequirementAppService
    {
        private readonly IRequirementRepository _requirementRepository;
        private readonly IRequirementManager _requirementManager;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public RequirementAppService(
            IRequirementRepository requirementRepository,
            IRequirementManager requirementManager,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _requirementRepository = requirementRepository;
            _requirementManager = requirementManager;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(RequirementSpecificationManagementResource);
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.View)]
        public virtual async Task<RequirementDto> GetAsync(Guid id)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(requirement);
                
                var dto = ObjectMapper.Map<Requirement, RequirementDto>(requirement);
                await EnrichDtoAsync(dto, requirement);
                
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.View)]
        public virtual async Task<PagedResultDto<RequirementDto>> GetListAsync(GetRequirementListDto input)
        {
            try
            {
                var specification = CreateFilterSpecification(input);
                var totalCount = await _requirementRepository.CountAsync(specification);
                
                var requirements = new List<Requirement>();
                if (totalCount > 0)
                {
                    var query = await _requirementRepository.WithDetailsAsync();
                    
                    requirements = await AsyncExecuter.ToListAsync(
                        query.Where(specification.ToExpression())
                             .OrderBy(input.Sorting ?? nameof(Requirement.Identifier.Value))
                             .PageBy(input)
                    );
                }

                var dtos = ObjectMapper.Map<List<Requirement>, List<RequirementDto>>(requirements);
                
                // Enrich DTOs with computed properties
                foreach (var dto in dtos)
                {
                    var requirement = requirements.First(r => r.Id == dto.Id);
                    await EnrichDtoAsync(dto, requirement);
                }

                return new PagedResultDto<RequirementDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.Create)]
        public virtual async Task<RequirementDto> CreateAsync(CreateRequirementDto input)
        {
            try
            {
                var identifier = new RequirementIdentifier(input.Identifier);
                var text = new RequirementText(input.Title, input.Description);
                var currentTime = _clock.Now;
                var currentUserId = _currentUser.Id;
                var currentTenantId = _currentTenant.Id;

                var requirement = await _requirementManager.CreateAsync(
                    identifier,
                    text,
                    input.Type,
                    input.Priority,
                    currentTenantId
                );

                await _requirementRepository.InsertAsync(requirement, autoSave: true);

                var dto = ObjectMapper.Map<Requirement, RequirementDto>(requirement);
                await EnrichDtoAsync(dto, requirement);

                Logger.LogInformation(
                    "Requirement created successfully: {RequirementId} by user {UserId}",
                    requirement.Id,
                    currentUserId);

                return dto;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.Update)]
        public virtual async Task<RequirementDto> UpdateAsync(Guid id, UpdateRequirementDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var newText = new RequirementText(input.Title, input.Description);
                requirement.UpdateText(newText);
                requirement.ChangePriority(input.Priority);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                var dto = ObjectMapper.Map<Requirement, RequirementDto>(requirement);
                await EnrichDtoAsync(dto, requirement);

                Logger.LogInformation(
                    "Requirement updated successfully: {RequirementId} by user {UserId}",
                    requirement.Id,
                    _currentUser.Id);

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                await _requirementManager.ValidateForDeletionAsync(requirement);
                await _requirementRepository.DeleteAsync(requirement, autoSave: true);

                Logger.LogInformation(
                    "Requirement deleted successfully: {RequirementId} by user {UserId}",
                    requirement.Id,
                    _currentUser.Id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ChangeStatus)]
        public virtual async Task<RequirementDto> ChangeStatusAsync(Guid id, ChangeRequirementStatusDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.ChangeStatus(input.Status, input.Reason);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                var dto = ObjectMapper.Map<Requirement, RequirementDto>(requirement);
                await EnrichDtoAsync(dto, requirement);

                Logger.LogInformation(
                    "Requirement status changed: {RequirementId} to {Status} by user {UserId}",
                    requirement.Id,
                    input.Status,
                    _currentUser.Id);

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.View)]
        public virtual async Task<List<string>> GetAllowedStatusTransitionsAsync(Guid id)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(id);
                await CheckResourceAccessAsync(requirement);

                var allowedTransitions = RequirementStatusTransitionPolicy.GetAllowedTransitions(requirement.Status);
                return allowedTransitions.Select(s => s.ToString()).ToList();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.SetScope)]
        public virtual async Task<RequirementDto> SetScopeAsync(Guid id, SetRequirementScopeDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(id, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var currentTime = _clock.Now;
                var currentUserId = _currentUser.Id ?? throw new BusinessException("User ID is required for scope operations");

                requirement.SetScopeStatus(input.ScopeStatus, input.Justification, currentTime, currentUserId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                var dto = ObjectMapper.Map<Requirement, RequirementDto>(requirement);
                await EnrichDtoAsync(dto, requirement);

                Logger.LogInformation(
                    "Requirement scope set: {RequirementId} to {ScopeStatus} by user {UserId}",
                    requirement.Id,
                    input.ScopeStatus,
                    currentUserId);

                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageAcceptanceCriteria)]
        public virtual async Task<AcceptanceCriterionDto> AddAcceptanceCriterionAsync(Guid requirementId, CreateAcceptanceCriterionDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var statement = new CriterionStatement(input.Statement, input.Description, input.Type);
                var criterion = requirement.AddAcceptanceCriterion(statement);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<AcceptanceCriterion, AcceptanceCriterionDto>(criterion);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageAcceptanceCriteria)]
        public virtual async Task<AcceptanceCriterionDto> UpdateAcceptanceCriterionAsync(Guid requirementId, Guid criterionId, UpdateAcceptanceCriterionDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var criterion = requirement.AcceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
                if (criterion == null)
                {
                    throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:AcceptanceCriterionNotFound")
                        .WithData("criterionId", criterionId);
                }

                var newStatement = new CriterionStatement(input.Statement, input.Description, input.Type);
                criterion.UpdateStatement(newStatement);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<AcceptanceCriterion, AcceptanceCriterionDto>(criterion);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageAcceptanceCriteria)]
        public virtual async Task RemoveAcceptanceCriterionAsync(Guid requirementId, Guid criterionId)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.RemoveAcceptanceCriterion(criterionId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageAcceptanceCriteria)]
        public virtual async Task ReorderAcceptanceCriteriaAsync(Guid requirementId, List<Guid> criterionIds)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                // This would require additional methods in the domain layer to support reordering
                // For now, we'll throw a not implemented exception
                throw new NotImplementedException("Acceptance criteria reordering is not yet implemented");
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageBusinessRules)]
        public virtual async Task<AssociatedRuleDto> AddBusinessRuleAsync(Guid requirementId, CreateAssociatedRuleDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var rule = new RuleDescription(input.Name, input.Description);
                var associatedRule = requirement.AddBusinessRule(rule);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<AssociatedRule, AssociatedRuleDto>(associatedRule);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageBusinessRules)]
        public virtual async Task<AssociatedRuleDto> UpdateBusinessRuleAsync(Guid requirementId, Guid ruleId, CreateAssociatedRuleDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var associatedRule = requirement.AssociatedRules.FirstOrDefault(r => r.Id == ruleId);
                if (associatedRule == null)
                {
                    throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:BusinessRuleNotFound")
                        .WithData("ruleId", ruleId);
                }

                var newRule = new RuleDescription(input.Name, input.Description);
                associatedRule.UpdateRule(newRule);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<AssociatedRule, AssociatedRuleDto>(associatedRule);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageBusinessRules)]
        public virtual async Task RemoveBusinessRuleAsync(Guid requirementId, Guid ruleId)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.RemoveBusinessRule(ruleId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageConstraints)]
        public virtual async Task<AssociatedConstraintDto> AddConstraintAsync(Guid requirementId, CreateAssociatedConstraintDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var constraint = new ConstraintStatement(input.Name, input.Statement, input.Type);
                var associatedConstraint = requirement.AddConstraint(constraint);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<AssociatedConstraint, AssociatedConstraintDto>(associatedConstraint);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageConstraints)]
        public virtual async Task<AssociatedConstraintDto> UpdateConstraintAsync(Guid requirementId, Guid constraintId, CreateAssociatedConstraintDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var associatedConstraint = requirement.AssociatedConstraints.FirstOrDefault(c => c.Id == constraintId);
                if (associatedConstraint == null)
                {
                    throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:ConstraintNotFound")
                        .WithData("constraintId", constraintId);
                }

                var newConstraint = new ConstraintStatement(input.Name, input.Statement, input.Type);
                associatedConstraint.UpdateConstraint(newConstraint);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<AssociatedConstraint, AssociatedConstraintDto>(associatedConstraint);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageConstraints)]
        public virtual async Task RemoveConstraintAsync(Guid requirementId, Guid constraintId)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.RemoveConstraint(constraintId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageDataRequirements)]
        public virtual async Task<DataModelReferenceDto> AddDataModelReferenceAsync(Guid requirementId, CreateDataModelReferenceDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var fieldSpec = new DataFieldSpec(input.FieldName, input.DataType, input.IsRequired, input.Description, input.MaxLength);
                var reference = requirement.AddDataModelReference(fieldSpec);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<DataModelReference, DataModelReferenceDto>(reference);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageDataRequirements)]
        public virtual async Task<DataModelReferenceDto> UpdateDataModelReferenceAsync(Guid requirementId, Guid referenceId, CreateDataModelReferenceDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var reference = requirement.DataModelReferences.FirstOrDefault(r => r.Id == referenceId);
                if (reference == null)
                {
                    throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:DataModelReferenceNotFound")
                        .WithData("referenceId", referenceId);
                }

                var newFieldSpec = new DataFieldSpec(input.FieldName, input.DataType, input.IsRequired, input.Description, input.MaxLength);
                reference.UpdateSpecification(newFieldSpec);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<DataModelReference, DataModelReferenceDto>(reference);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageDataRequirements)]
        public virtual async Task RemoveDataModelReferenceAsync(Guid requirementId, Guid referenceId)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.RemoveDataModelReference(referenceId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageInterfaces)]
        public virtual async Task<InterfaceSpecificationDto> AddInterfaceSpecificationAsync(Guid requirementId, CreateInterfaceSpecificationDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var signature = new ApiSignature(input.InterfaceName, input.Endpoint, input.HttpMethod, input.Type, input.Description);
                var specification = requirement.AddInterfaceSpecification(signature);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<InterfaceSpecification, InterfaceSpecificationDto>(specification);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageInterfaces)]
        public virtual async Task<InterfaceSpecificationDto> UpdateInterfaceSpecificationAsync(Guid requirementId, Guid specificationId, CreateInterfaceSpecificationDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var specification = requirement.InterfaceSpecifications.FirstOrDefault(s => s.Id == specificationId);
                if (specification == null)
                {
                    throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:InterfaceSpecificationNotFound")
                        .WithData("specificationId", specificationId);
                }

                var newSignature = new ApiSignature(input.InterfaceName, input.Endpoint, input.HttpMethod, input.Type, input.Description);
                specification.UpdateSignature(newSignature);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<InterfaceSpecification, InterfaceSpecificationDto>(specification);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.ManageInterfaces)]
        public virtual async Task RemoveInterfaceSpecificationAsync(Guid requirementId, Guid specificationId)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.RemoveInterfaceSpecification(specificationId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.LinkToObjectives)]
        public virtual async Task<BusinessObjectiveLinkDto> LinkToBusinessObjectiveAsync(Guid requirementId, CreateBusinessObjectiveLinkDto input)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                var objectiveId = new ObjectiveId(input.ObjectiveId);
                var link = requirement.LinkToBusinessObjective(objectiveId, input.Justification);

                await _requirementRepository.UpdateAsync(requirement, autoSave: true);

                return ObjectMapper.Map<BusinessObjectiveLink, BusinessObjectiveLinkDto>(link);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.LinkToObjectives)]
        public virtual async Task RemoveBusinessObjectiveLinkAsync(Guid requirementId, Guid linkId)
        {
            try
            {
                var requirement = await _requirementRepository.GetAsync(requirementId, includeDetails: true);
                await CheckResourceAccessAsync(requirement);

                requirement.RemoveBusinessObjectiveLink(linkId);
                await _requirementRepository.UpdateAsync(requirement, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.View)]
        public virtual async Task<List<RequirementDto>> SearchAsync(string searchText, GetRequirementListDto filters = null)
        {
            try
            {
                var query = await _requirementRepository.WithDetailsAsync();

                if (!searchText.IsNullOrWhiteSpace())
                {
                    query = query.Where(r => 
                        r.Identifier.Value.Contains(searchText) ||
                        r.Text.Title.Contains(searchText) ||
                        r.Text.Description.Contains(searchText));
                }

                if (filters != null)
                {
                    var specification = CreateFilterSpecification(filters);
                    query = query.Where(specification.ToExpression());
                }

                var requirements = await AsyncExecuter.ToListAsync(
                    query.OrderBy(r => r.Identifier.Value).Take(50) // Limit search results
                );

                var dtos = ObjectMapper.Map<List<Requirement>, List<RequirementDto>>(requirements);
                
                foreach (var dto in dtos)
                {
                    var requirement = requirements.First(r => r.Id == dto.Id);
                    await EnrichDtoAsync(dto, requirement);
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Requirements.View)]
        public virtual async Task<List<RequirementDto>> GetByBusinessObjectiveAsync(Guid objectiveId)
        {
            try
            {
                var requirements = await _requirementRepository.GetByBusinessObjectiveAsync(objectiveId);
                var dtos = ObjectMapper.Map<List<Requirement>, List<RequirementDto>>(requirements);
                
                foreach (var dto in dtos)
                {
                    var requirement = requirements.First(r => r.Id == dto.Id);
                    await EnrichDtoAsync(dto, requirement);
                }

                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ExportData)]
        public virtual async Task<string> ExportAsync(GetRequirementListDto filters, string format = "json")
        {
            try
            {
                var specification = CreateFilterSpecification(filters);
                var requirements = await _requirementRepository.GetListAsync(specification);

                return format.ToLowerInvariant() switch
                {
                    "json" => System.Text.Json.JsonSerializer.Serialize(requirements, new JsonSerializerOptions { WriteIndented = true }),
                    "csv" => await ExportToCsvAsync(requirements),
                    _ => throw new ArgumentException($"Unsupported export format: {format}")
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementSpecificationManagementPermissions.Reporting.ExportData)]
        public virtual async Task<byte[]> ExportAsBinaryAsync(GetRequirementListDto filters, string format = "xlsx")
        {
            try
            {
                var specification = CreateFilterSpecification(filters);
                var requirements = await _requirementRepository.GetListAsync(specification);

                return format.ToLowerInvariant() switch
                {
                    "xlsx" => await ExportToExcelAsync(requirements),
                    "pdf" => await ExportToPdfAsync(requirements),
                    _ => throw new ArgumentException($"Unsupported binary export format: {format}")
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        // Helper methods
        private RequirementSpecification CreateFilterSpecification(GetRequirementListDto input)
        {
            var specifications = new List<RequirementSpecification>();

            if (input.Type.HasValue)
            {
                specifications.Add(new RequirementByTypeSpecification(input.Type.Value));
            }

            if (input.Status.HasValue)
            {
                specifications.Add(new RequirementByStatusListSpecification(input.Status.Value));
            }

            if (input.Priority.HasValue)
            {
                specifications.Add(new RequirementByPrioritySpecification(input.Priority.Value));
            }

            if (input.ScopeStatus.HasValue)
            {
                specifications.Add(new InScopeRequirementSpecification());
            }

            if (input.WithAcceptanceCriteria.HasValue && input.WithAcceptanceCriteria.Value)
            {
                specifications.Add(new RequirementWithAcceptanceCriteriaSpecification());
            }

            if (!input.Filter.IsNullOrWhiteSpace())
            {
                specifications.Add(new RequirementByIdentifierPatternSpecification(input.Filter));
            }

            // Combine all specifications using AND logic
            RequirementSpecification combinedSpec = specifications.FirstOrDefault() ?? new RequirementByTypeSpecification(RequirementType.Functional);
            foreach (var spec in specifications.Skip(1))
            {
                combinedSpec = combinedSpec.And(spec);
            }

            return combinedSpec;
        }

        private async Task EnrichDtoAsync(RequirementDto dto, Requirement requirement)
        {
            dto.CanBeModified = new ModifiableRequirementSpecification().IsSatisfiedBy(requirement);
            dto.AcceptanceCriteriaCount = requirement.AcceptanceCriteria.Count;
            dto.BusinessRulesCount = requirement.AssociatedRules.Count;
            dto.ConstraintsCount = requirement.AssociatedConstraints.Count;

            // TODO: Load user names for scope marker
            if (dto.ScopeMarker != null)
            {
                // dto.ScopeMarker.MarkedByUserName = await GetUserNameAsync(dto.ScopeMarker.MarkedBy);
            }

            await Task.CompletedTask;
        }

        private async Task CheckResourceAccessAsync(Requirement requirement)
        {
            // Check tenant consistency
            if (requirement.TenantId != _currentTenant.Id)
            {
                throw new BusinessException("RequirementSpecificationManagement:ErrorMessage:TenantMismatch");
            }

            // Additional access checks can be implemented here
            await Task.CompletedTask;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in RequirementAppService: {ErrorMessage}. User: {UserId}, Tenant: {TenantId}", 
                exception.Message, 
                _currentUser.Id, 
                _currentTenant.Id);

            await Task.CompletedTask;
        }

        private async Task<string> ExportToCsvAsync(List<Requirement> requirements)
        {
            // CSV export implementation
            var csv = new StringBuilder();
            csv.AppendLine("Identifier,Title,Description,Type,Status,Priority,Version,CreationTime");

            foreach (var req in requirements)
            {
                csv.AppendLine($"{req.Identifier.Value},{req.Text.Title},{req.Text.Description},{req.Type},{req.Status},{req.Priority},{req.Version},{req.CreationTime:yyyy-MM-dd}");
            }

            return await Task.FromResult(csv.ToString());
        }

        private async Task<byte[]> ExportToExcelAsync(List<Requirement> requirements)
        {
            // Excel export implementation (would require a library like EPPlus)
            // For now, return empty byte array
            return await Task.FromResult(Array.Empty<byte>());
        }

        private async Task<byte[]> ExportToPdfAsync(List<Requirement> requirements)
        {
            // PDF export implementation (would require a library like iTextSharp)
            // For now, return empty byte array
            return await Task.FromResult(Array.Empty<byte>());
        }
    }
}
