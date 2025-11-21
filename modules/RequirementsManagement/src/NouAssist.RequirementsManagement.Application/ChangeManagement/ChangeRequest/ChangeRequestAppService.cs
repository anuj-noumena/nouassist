using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest.Dtos;
using NouAssist.RequirementsManagement.ChangeManagement.Constants;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;
using NouAssist.RequirementsManagement.ChangeManagement.Exceptions;
using NouAssist.RequirementsManagement.ChangeManagement.Localization;
using NouAssist.RequirementsManagement.ChangeManagement.Permissions;
using NouAssist.RequirementsManagement.ChangeManagement.Repositories;
using NouAssist.RequirementsManagement.ChangeManagement.Services;
using NouAssist.RequirementsManagement.ChangeManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Guids;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.ChangeManagement.ChangeRequest
{
    [Authorize(ChangeManagementPermissions.ChangeRequest.Default)]
    public class ChangeRequestAppService : ApplicationService, IChangeRequestAppService
    {
        // --- Dependencies ---
        private readonly IRequirementChangeRequestRepository _changeRequestRepository;
        private readonly IChangeImpactAssessmentService _impactAssessmentService;
        private readonly IApprovalRoutingService _approvalRoutingService;
        private readonly ICurrentUser _currentUser;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClock _clock;

        public ChangeRequestAppService(
            IRequirementChangeRequestRepository changeRequestRepository,
            IChangeImpactAssessmentService impactAssessmentService,
            IApprovalRoutingService approvalRoutingService,
            ICurrentUser currentUser,
            IGuidGenerator guidGenerator,
            IClock clock)
        {
            _changeRequestRepository = changeRequestRepository;
            _impactAssessmentService = impactAssessmentService;
            _approvalRoutingService = approvalRoutingService;
            _currentUser = currentUser;
            _guidGenerator = guidGenerator;
            _clock = clock;
            
            LocalizationResource = typeof(ChangeManagementResource);
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.View)]
        public virtual async Task<ChangeRequestDto> GetAsync(Guid id)
        {
            try
            {
                // Get aggregate with all related entities
                var changeRequest = await _changeRequestRepository.GetWithDetailsAsync(id);
                
                // Map to DTO
                var dto = ObjectMapper.Map<RequirementChangeRequest, ChangeRequestDto>(changeRequest);
                
                // Calculate summary values
                var (totalCost, totalTime) = changeRequest.GetTotalImpact();
                dto.TotalCostImpact = totalCost;
                dto.TotalTimeImpact = totalTime;
                
                // Determine if current user can edit this change request
                dto.CanEdit = await CanUserEditChangeRequestAsync(changeRequest);
                
                // TODO: Enhance with user names from identity service
                dto.SubmitterName = "User " + changeRequest.SubmitterId.ToString().Substring(0, 4);
                foreach (var step in dto.ApprovalSteps)
                {
                    step.ApproverName = "Approver " + step.ApproverId.ToString().Substring(0, 4);
                }
                
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.View)]
        public virtual async Task<PagedResultDto<ChangeRequestDto>> GetListAsync(GetChangeRequestListDto input)
        {
            try
            {
                // Get queryable from repository
                var query = await _changeRequestRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(x => 
                        x.Title.Contains(input.Filter) || 
                        x.Description.Contains(input.Filter) ||
                        EF.Property<string>(x, "RequestIdentifier_Value").Contains(input.Filter));
                }
                
                if (input.Status.HasValue)
                {
                    query = query.Where(x => x.Status == input.Status.Value);
                }
                
                if (input.SubmissionDateMin.HasValue)
                {
                    query = query.Where(x => x.SubmissionDate >= input.SubmissionDateMin.Value);
                }
                
                if (input.SubmissionDateMax.HasValue)
                {
                    query = query.Where(x => x.SubmissionDate <= input.SubmissionDateMax.Value);
                }
                
                if (input.SubmitterId.HasValue)
                {
                    query = query.Where(x => x.SubmitterId == input.SubmitterId.Value);
                }
                
                if (input.AffectedRequirementId.HasValue)
                {
                    query = query.Where(x => 
                        EF.Property<Guid>(x, "AffectedRequirementId") == input.AffectedRequirementId.Value);
                }
                
                if (input.PendingMyApproval == true && _currentUser.Id.HasValue)
                {
                    // Include approval steps for filtering
                    query = query
                        .Include(x => x.ApprovalSteps)
                        .Where(x => x.Status == RequestStatus.PendingApproval)
                        .Where(x => x.ApprovalSteps.Any(a => 
                            a.ApproverId == _currentUser.Id.Value && 
                            a.Decision == ApprovalDecision.Pending));
                }
                
                // Get total count
                long totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                var sorting = input.Sorting ?? "SubmissionDate DESC";
                query = query.OrderBy(sorting);
                
                // Apply paging
                var items = await AsyncExecuter.ToListAsync(query.PageBy(input));
                
                // Include related entities for mapping
                var changeRequests = new List<RequirementChangeRequest>();
                foreach (var item in items)
                {
                    changeRequests.Add(await _changeRequestRepository.GetWithDetailsAsync(item.Id));
                }
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<RequirementChangeRequest>, List<ChangeRequestDto>>(changeRequests);
                
                // Calculate summary values and set edit permissions
                for (int i = 0; i < changeRequests.Count; i++)
                {
                    var (totalCost, totalTime) = changeRequests[i].GetTotalImpact();
                    dtos[i].TotalCostImpact = totalCost;
                    dtos[i].TotalTimeImpact = totalTime;
                    dtos[i].CanEdit = await CanUserEditChangeRequestAsync(changeRequests[i]);
                    
                    // TODO: Enhance with user names from identity service
                    dtos[i].SubmitterName = "User " + changeRequests[i].SubmitterId.ToString().Substring(0, 4);
                }
                
                return new PagedResultDto<ChangeRequestDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Create)]
        public virtual async Task<ChangeRequestDto> CreateAsync(CreateChangeRequestDto input)
        {
            try
            {
                var currentUserId = _currentUser.Id;
                if (!currentUserId.HasValue)
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:UserNotAuthenticated"]);
                }
                
                var currentTime = _clock.Now;
                var newId = _guidGenerator.Create();
                
                // Generate request identifier (e.g., CR-2024-001)
                var requestCount = await _changeRequestRepository.CountAsync() + 1;
                var requestIdentifier = new RequestIdentifier(
                    $"{ChangeManagementConsts.DefaultRequestPrefix}-{currentTime.Year}-{requestCount:D3}");
                
                // Create affected requirement reference
                var affectedRequirement = new AffectedRequirementRef(
                    input.AffectedRequirementId,
                    input.AffectedRequirementVersion);
                
                // Create change request aggregate
                var changeRequest = RequirementChangeRequest.Create(
                    newId,
                    requestIdentifier,
                    input.Title,
                    input.Description,
                    affectedRequirement,
                    currentUserId.Value,
                    currentTime,
                    CurrentTenant.Id);
                
                // Add rationales if provided
                if (input.Rationales != null && input.Rationales.Any())
                {
                    foreach (var rationaleDto in input.Rationales)
                    {
                        changeRequest.AddRationale(rationaleDto.RationaleText, rationaleDto.Category);
                    }
                }
                
                // Save to repository
                await _changeRequestRepository.InsertAsync(changeRequest, autoSave: true);
                
                // Return mapped DTO
                return await GetAsync(changeRequest.Id);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Update)]
        public virtual async Task<ChangeRequestDto> UpdateAsync(Guid id, UpdateChangeRequestDto input)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetAsync(id);
                
                // Check if user can edit this change request
                if (!await CanUserEditChangeRequestAsync(changeRequest))
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:CannotEditChangeRequest"]);
                }
                
                // Update properties
                changeRequest.UpdateTitle(input.Title);
                changeRequest.UpdateDescription(input.Description);
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
                
                // Return updated DTO
                return await GetAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetAsync(id);
                
                // Check if it can be deleted (only if in Submitted or Cancelled status)
                if (changeRequest.Status != RequestStatus.Submitted && changeRequest.Status != RequestStatus.Cancelled)
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:CannotDeleteChangeRequest"]);
                }
                
                // Delete the change request
                await _changeRequestRepository.DeleteAsync(id, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                // If it doesn't exist, that's fine for a delete operation
                return;
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Update)]
        public virtual async Task<ChangeRequestDto> AddRationaleAsync(Guid id, AddChangeRationaleDto input)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetAsync(id);
                
                // Check if user can edit this change request
                if (!await CanUserEditChangeRequestAsync(changeRequest))
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:CannotEditChangeRequest"]);
                }
                
                // Add rationale
                changeRequest.AddRationale(input.RationaleText, input.Category);
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
                
                // Return updated DTO
                return await GetAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Update)]
        public virtual async Task RemoveRationaleAsync(Guid id, Guid rationaleId)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetWithDetailsAsync(id);
                
                // Check if user can edit this change request
                if (!await CanUserEditChangeRequestAsync(changeRequest))
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:CannotEditChangeRequest"]);
                }
                
                // Remove rationale
                changeRequest.RemoveRationale(rationaleId);
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.AssessImpact)]
        public virtual async Task<ChangeRequestDto> AddImpactStatementAsync(Guid id, AddImpactStatementDto input)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetAsync(id);
                
                // Create impact details value object
                var impactDetails = new ImpactDetails(
                    input.ImpactType,
                    input.CostImpact,
                    input.TimeImpactDays,
                    input.Dependencies);
                
                // Add impact statement
                changeRequest.AddImpactStatement(impactDetails, input.Description);
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
                
                // Return updated DTO
                return await GetAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.AssessImpact)]
        public virtual async Task RemoveImpactStatementAsync(Guid id, Guid impactStatementId)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetWithDetailsAsync(id);
                
                // Remove impact statement
                changeRequest.RemoveImpactStatement(impactStatementId);
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Update)]
        public virtual async Task<ChangeRequestDto> ChangeStatusAsync(Guid id, ChangeStatusDto input)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetAsync(id);
                
                // Perform the status transition based on the target status
                switch (input.TargetStatus)
                {
                    case RequestStatus.UnderReview:
                        changeRequest.MoveToUnderReview();
                        break;
                    case RequestStatus.ImpactAssessment:
                        changeRequest.MoveToImpactAssessment();
                        break;
                    case RequestStatus.PendingApproval:
                        changeRequest.MoveToPendingApproval();
                        break;
                    case RequestStatus.InProgress:
                        changeRequest.StartImplementation();
                        break;
                    case RequestStatus.Completed:
                        changeRequest.MarkAsCompleted(_clock.Now);
                        break;
                    case RequestStatus.Cancelled:
                        changeRequest.Cancel(input.Reason);
                        break;
                    default:
                        throw new BusinessException(L["ChangeManagement:ErrorMessage:InvalidStatusTransition"]);
                }
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
                
                // Return updated DTO
                return await GetAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Approve)]
        public virtual async Task<ChangeRequestDto> SubmitApprovalDecisionAsync(Guid id, ApprovalDecisionDto input)
        {
            try
            {
                var currentUserId = _currentUser.Id;
                if (!currentUserId.HasValue)
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:UserNotAuthenticated"]);
                }
                
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetWithDetailsAsync(id);
                
                // Verify the user can approve this step
                var canApprove = await _approvalRoutingService.CanUserApproveAsync(
                    currentUserId.Value, 
                    changeRequest);
                
                if (!canApprove)
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:UnauthorizedApproval"]);
                }
                
                // Create decision reason value object
                var decisionReason = new DecisionReason(input.DecisionReason);
                
                // Process the approval decision
                changeRequest.ProcessApproval(
                    input.ApprovalStepId, 
                    input.Decision, 
                    decisionReason, 
                    _clock.Now);
                
                // Save changes
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
                
                // Return updated DTO
                return await GetAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.Update)]
        public virtual async Task<ChangeRequestDto> StartApprovalProcessAsync(Guid id)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetWithDetailsAsync(id);
                
                // Check if change request is in a state where approval process can start
                if (changeRequest.Status != RequestStatus.UnderReview && 
                    changeRequest.Status != RequestStatus.ImpactAssessment)
                {
                    throw new BusinessException(L["ChangeManagement:ErrorMessage:CannotStartApprovalProcess"]);
                }
                
                // Use the approval routing service to determine approval steps
                await _approvalRoutingService.RouteForApprovalAsync(changeRequest);
                
                // Save changes (should already be saved by the routing service)
                await _changeRequestRepository.UpdateAsync(changeRequest, autoSave: true);
                
                // Return updated DTO
                return await GetAsync(id);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.View)]
        public virtual async Task<List<ChangeRequestDto>> GetSubmittedByMeAsync()
        {
            try
            {
                var currentUserId = _currentUser.Id;
                if (!currentUserId.HasValue)
                {
                    return new List<ChangeRequestDto>();
                }
                
                // Get change requests submitted by the current user
                var changeRequests = await _changeRequestRepository.GetBySubmitterAsync(currentUserId.Value);
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<RequirementChangeRequest>, List<ChangeRequestDto>>(changeRequests);
                
                // Calculate summary values and set edit permissions
                for (int i = 0; i < changeRequests.Count; i++)
                {
                    var (totalCost, totalTime) = changeRequests[i].GetTotalImpact();
                    dtos[i].TotalCostImpact = totalCost;
                    dtos[i].TotalTimeImpact = totalTime;
                    dtos[i].CanEdit = await CanUserEditChangeRequestAsync(changeRequests[i]);
                    
                    // TODO: Enhance with user names from identity service
                    dtos[i].SubmitterName = "User " + changeRequests[i].SubmitterId.ToString().Substring(0, 4);
                }
                
                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.View)]
        public virtual async Task<List<ChangeRequestDto>> GetPendingMyApprovalAsync()
        {
            try
            {
                var currentUserId = _currentUser.Id;
                if (!currentUserId.HasValue)
                {
                    return new List<ChangeRequestDto>();
                }
                
                // Get change requests pending approval by the current user
                var changeRequests = await _changeRequestRepository.GetPendingForApprovalByUserAsync(currentUserId.Value);
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<RequirementChangeRequest>, List<ChangeRequestDto>>(changeRequests);
                
                // Calculate summary values and set edit permissions
                for (int i = 0; i < changeRequests.Count; i++)
                {
                    var (totalCost, totalTime) = changeRequests[i].GetTotalImpact();
                    dtos[i].TotalCostImpact = totalCost;
                    dtos[i].TotalTimeImpact = totalTime;
                    dtos[i].CanEdit = false; // Cannot edit while waiting for approval
                    
                    // TODO: Enhance with user names from identity service
                    dtos[i].SubmitterName = "User " + changeRequests[i].SubmitterId.ToString().Substring(0, 4);
                }
                
                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.View)]
        public virtual async Task<List<ChangeRequestDto>> GetHighImpactChangesAsync()
        {
            try
            {
                // Get high impact change requests
                var changeRequests = await _changeRequestRepository.GetHighImpactChangesAsync();
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<RequirementChangeRequest>, List<ChangeRequestDto>>(changeRequests);
                
                // Calculate summary values and set edit permissions
                for (int i = 0; i < changeRequests.Count; i++)
                {
                    var (totalCost, totalTime) = changeRequests[i].GetTotalImpact();
                    dtos[i].TotalCostImpact = totalCost;
                    dtos[i].TotalTimeImpact = totalTime;
                    dtos[i].CanEdit = await CanUserEditChangeRequestAsync(changeRequests[i]);
                    
                    // TODO: Enhance with user names from identity service
                    dtos[i].SubmitterName = "User " + changeRequests[i].SubmitterId.ToString().Substring(0, 4);
                }
                
                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.View)]
        public virtual async Task<List<ChangeRequestDto>> GetRecentChangesAsync(int maxResultCount = 10)
        {
            try
            {
                // Get queryable from repository
                var query = await _changeRequestRepository.GetQueryableAsync();
                
                // Get recent change requests ordered by submission date
                var recentChanges = await AsyncExecuter.ToListAsync(
                    query.OrderByDescending(x => x.SubmissionDate).Take(maxResultCount));
                
                // Include related entities for mapping
                var changeRequests = new List<RequirementChangeRequest>();
                foreach (var item in recentChanges)
                {
                    changeRequests.Add(await _changeRequestRepository.GetWithDetailsAsync(item.Id));
                }
                
                // Map to DTOs
                var dtos = ObjectMapper.Map<List<RequirementChangeRequest>, List<ChangeRequestDto>>(changeRequests);
                
                // Calculate summary values and set edit permissions
                for (int i = 0; i < changeRequests.Count; i++)
                {
                    var (totalCost, totalTime) = changeRequests[i].GetTotalImpact();
                    dtos[i].TotalCostImpact = totalCost;
                    dtos[i].TotalTimeImpact = totalTime;
                    dtos[i].CanEdit = await CanUserEditChangeRequestAsync(changeRequests[i]);
                    
                    // TODO: Enhance with user names from identity service
                    dtos[i].SubmitterName = "User " + changeRequests[i].SubmitterId.ToString().Substring(0, 4);
                }
                
                return dtos;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(ChangeManagementPermissions.ChangeRequest.AssessImpact)]
        public virtual async Task<ImpactAssessmentResultDto> ConductImpactAssessmentAsync(Guid id)
        {
            try
            {
                // Get the change request
                var changeRequest = await _changeRequestRepository.GetWithDetailsAsync(id);
                
                // Use the impact assessment service
                var assessmentResult = await _impactAssessmentService.ConductImpactAssessmentAsync(changeRequest);
                
                // Map to DTO
                var resultDto = ObjectMapper.Map<ImpactAssessmentResult, ImpactAssessmentResultDto>(assessmentResult);
                
                return resultDto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["ChangeManagement:ErrorMessage:ChangeRequestNotFound"]);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code], bex.Message);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // --- Helper Methods ---

        protected virtual async Task<bool> CanUserEditChangeRequestAsync(RequirementChangeRequest changeRequest)
        {
            // Get current user
            var currentUserId = _currentUser.Id;
            if (!currentUserId.HasValue)
            {
                return false;
            }
            
            // Check status - only certain statuses allow editing
            if (changeRequest.Status == RequestStatus.Completed || 
                changeRequest.Status == RequestStatus.Cancelled)
            {
                return false;
            }
            
            // Check if user is the submitter
            if (changeRequest.SubmitterId == currentUserId.Value)
            {
                // Submitter can edit if in Submitted or UnderReview status
                return changeRequest.Status == RequestStatus.Submitted || 
                       changeRequest.Status == RequestStatus.UnderReview;
            }
            
            // Check if user has permission to edit any change request
            return await AuthorizationService.IsGrantedAsync(ChangeManagementPermissions.ChangeRequest.Update);
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in Change Request Application Service: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
