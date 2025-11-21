using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Branching.Dtos;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using NouAssist.RequirementsManagement.VersioningBaselining.Localization;
using NouAssist.RequirementsManagement.VersioningBaselining.Permissions;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Branching
{
    [Authorize(VersionControlPermissions.Branching.Default)]
    public class BranchingAppService : ApplicationService, IBranchingAppService
    {
        private readonly IBranchingAndMergingService _branchingService;
        private readonly IRequirementBranchRepository _branchRepository;
        private readonly ILogger<BranchingAppService> _logger;

        public BranchingAppService(
            IBranchingAndMergingService branchingService,
            IRequirementBranchRepository branchRepository,
            ILogger<BranchingAppService> logger)
        {
            _branchingService = branchingService;
            _branchRepository = branchRepository;
            _logger = logger;
            
            LocalizationResource = typeof(VersionControlResource);
        }

        [Authorize(VersionControlPermissions.Branching.Create)]
        public virtual async Task<BranchDto> CreateAsync(CreateBranchDto input)
        {
            try
            {
                RequirementBranch branch;
                
                if (input.SourceType == "Baseline")
                {
                    branch = await _branchingService.CreateBranchFromBaselineAsync(
                        input.Name,
                        input.Description,
                        input.SourceId,
                        CurrentTenant.Id);
                }
                else if (input.SourceType == "Branch")
                {
                    branch = await _branchingService.CreateBranchFromBranchAsync(
                        input.Name,
                        input.Description,
                        input.SourceId,
                        CurrentTenant.Id);
                }
                else
                {
                    throw new BusinessException("VersionControl:ErrorMessage:InvalidSourceType")
                        .WithData("sourceType", input.SourceType);
                }
                
                return ObjectMapper.Map<RequirementBranch, BranchDto>(branch);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.Update)]
        public virtual async Task<BranchDto> UpdateAsync(Guid id, UpdateBranchDto input)
        {
            try
            {
                var branch = await _branchRepository.GetAsync(id);
                
                branch.Update(input.Description);
                
                await _branchRepository.UpdateAsync(branch);
                
                return ObjectMapper.Map<RequirementBranch, BranchDto>(branch);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.View)]
        public virtual async Task<BranchDto> GetAsync(Guid id)
        {
            try
            {
                var branch = await _branchRepository.GetAsync(id);
                return ObjectMapper.Map<RequirementBranch, BranchDto>(branch);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.View)]
        public virtual async Task<PagedResultDto<BranchDto>> GetListAsync(GetBranchListDto input)
        {
            try
            {
                var queryable = await _branchRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(b => EF.Property<string>(b, "Name").Contains(input.Filter));
                }
                
                if (input.Status.HasValue)
                {
                    queryable = queryable.Where(b => b.Status == input.Status.Value);
                }
                
                if (input.SourceId.HasValue)
                {
                    queryable = queryable.Where(b => b.SourceId == input.SourceId.Value);
                }
                
                if (!string.IsNullOrWhiteSpace(input.SourceType))
                {
                    queryable = queryable.Where(b => b.SourceType == input.SourceType);
                }
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? "Name")
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);
                    
                // Execute query
                var branches = await AsyncExecuter.ToListAsync(queryable);
                
                // Map to DTOs
                var dtoList = ObjectMapper.Map<List<RequirementBranch>, List<BranchDto>>(branches);
                
                return new PagedResultDto<BranchDto>(totalCount, dtoList);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.Delete)]
        public virtual async Task<BranchDto> AbandonAsync(Guid id)
        {
            try
            {
                await _branchingService.AbandonBranchAsync(id);
                
                var branch = await _branchRepository.GetAsync(id);
                return ObjectMapper.Map<RequirementBranch, BranchDto>(branch);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.Merge)]
        public virtual async Task<MergeOperationDto> InitiateMergeAsync(Guid sourceBranchId, InitiateMergeDto input)
        {
            try
            {
                var mergeOperation = await _branchingService.InitiateMergeAsync(
                    sourceBranchId,
                    input.TargetBranchId);
                    
                return ObjectMapper.Map<MergeOperation, MergeOperationDto>(mergeOperation);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.Merge)]
        public virtual async Task<MergeOperationDto> CompleteMergeAsync(Guid mergeOperationId, CompleteMergeDto input)
        {
            try
            {
                var mergeResult = MergeResult.Success(input.MergedRequirementCount);
                
                var mergeOperation = await _branchingService.CompleteMergeAsync(
                    mergeOperationId,
                    mergeResult);
                    
                return ObjectMapper.Map<MergeOperation, MergeOperationDto>(mergeOperation);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Branching.Merge)]
        public virtual async Task<MergeOperationDto> AbortMergeAsync(Guid mergeOperationId, AbortMergeDto input)
        {
            try
            {
                var mergeOperation = await _branchingService.AbortMergeAsync(
                    mergeOperationId,
                    input.Reason);
                    
                return ObjectMapper.Map<MergeOperation, MergeOperationDto>(mergeOperation);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception)
        {
            _logger.LogError(exception, "An error occurred in Branching App Service: {Message}", exception.Message);
            
            if (exception is BusinessException businessException)
            {
                throw new UserFriendlyException(
                    L[businessException.Code],
                    businessException.Code,
                    businessException.Data);
            }
        }
    }
}
