using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Defects.Dtos;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Permissions;
using NouAssist.RequirementsManagement.QualityAssurance.Repositories;
using NouAssist.RequirementsManagement.QualityAssurance.Services;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.QualityAssurance.Defects
{
    [Authorize(QAPermissions.Defect.Default)]
    public class DefectAppService : ApplicationService, IDefectAppService
    {
        private readonly IDefectReportRepository _defectRepository;
        private readonly IDefectManager _defectManager;
        private readonly ILogger<DefectAppService> _logger;
        private readonly ICurrentUser _currentUser;

        public DefectAppService(
            IDefectReportRepository defectRepository,
            IDefectManager defectManager,
            ILogger<DefectAppService> logger,
            ICurrentUser currentUser)
        {
            _defectRepository = defectRepository;
            _defectManager = defectManager;
            _logger = logger;
            _currentUser = currentUser;
            LocalizationResource = typeof(QAResource);
        }

        [Authorize(QAPermissions.Defect.Create)]
        public virtual async Task<DefectDto> CreateAsync(CreateDefectDto input)
        {
            try
            {
                var defect = await _defectManager.CreateAsync(
                    input.Title,
                    input.Description,
                    input.StepsToReproduce,
                    input.ExpectedOutcome,
                    input.ActualOutcome,
                    input.Severity,
                    input.Priority,
                    input.Environment,
                    input.Version,
                    input.TestRunId,
                    input.TestCycleId,
                    input.TestPlanId);
                
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.Update)]
        public virtual async Task<DefectDto> UpdateAsync(Guid id, UpdateDefectDto input)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(id);
                
                defect.UpdateBasicInfo(
                    input.Title,
                    input.Description,
                    input.StepsToReproduce,
                    input.ExpectedOutcome,
                    input.ActualOutcome,
                    input.Severity,
                    input.Priority,
                    input.Environment,
                    input.Version);
                
                await _defectRepository.UpdateAsync(defect);
                
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _defectRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<DefectDto> GetAsync(Guid id)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(id);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["Defect:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<DefectDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var defect = await _defectRepository.GetWithDetailsAsync(id);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect, 
                    opt => opt.IncludeMembers(d => d.Comments));
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["Defect:NotFound", id]);
                }
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<PagedResultDto<DefectDto>> GetListAsync(GetDefectListDto input)
        {
            try
            {
                var queryable = await _defectRepository.GetQueryableAsync();
                
                // Apply filters
                queryable = queryable
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), 
                        d => d.Title.Contains(input.Filter) || d.Description.Contains(input.Filter))
                    .WhereIf(input.TestPlanId.HasValue,
                        d => d.TestPlanId == input.TestPlanId.Value)
                    .WhereIf(input.TestCycleId.HasValue,
                        d => d.TestCycleId == input.TestCycleId.Value)
                    .WhereIf(input.Status.HasValue,
                        d => d.Status == input.Status.Value)
                    .WhereIf(input.Severity.HasValue,
                        d => d.Severity == input.Severity.Value)
                    .WhereIf(input.Priority.HasValue,
                        d => d.Priority == input.Priority.Value)
                    .WhereIf(input.AssigneeId.HasValue,
                        d => d.AssigneeId == input.AssigneeId.Value)
                    .WhereIf(!input.Version.IsNullOrWhiteSpace(),
                        d => d.Version == input.Version);
                
                // Get count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                queryable = queryable
                    .OrderBy(input.Sorting ?? nameof(DefectReport.CreationTime) + " desc")
                    .PageBy(input);
                
                // Execute query and map to DTOs
                var defects = await AsyncExecuter.ToListAsync(queryable);
                var defectDtos = ObjectMapper.Map<List<DefectReport>, List<DefectDto>>(defects);
                
                return new PagedResultDto<DefectDto>(totalCount, defectDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<List<DefectDto>> GetByTestCycleIdAsync(Guid testCycleId)
        {
            try
            {
                var defects = await _defectRepository.GetByTestCycleIdAsync(testCycleId);
                return ObjectMapper.Map<List<DefectReport>, List<DefectDto>>(defects);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<List<DefectDto>> GetByTestPlanIdAsync(Guid testPlanId)
        {
            try
            {
                var defects = await _defectRepository.GetByTestPlanIdAsync(testPlanId);
                return ObjectMapper.Map<List<DefectReport>, List<DefectDto>>(defects);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<List<DefectDto>> GetMyDefectsAsync()
        {
            try
            {
                if (!_currentUser.Id.HasValue)
                {
                    return new List<DefectDto>();
                }
                
                var defects = await _defectRepository.GetByAssigneeIdAsync(_currentUser.Id.Value);
                return ObjectMapper.Map<List<DefectReport>, List<DefectDto>>(defects);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<List<DefectDto>> GetByStatusAsync(DefectStatus status)
        {
            try
            {
                var defects = await _defectRepository.GetByStatusAsync(status);
                return ObjectMapper.Map<List<DefectReport>, List<DefectDto>>(defects);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.View)]
        public virtual async Task<List<DefectDto>> GetHighPriorityDefectsAsync()
        {
            try
            {
                var defects = await _defectManager.GetHighPriorityDefectsAsync();
                return ObjectMapper.Map<List<DefectReport>, List<DefectDto>>(defects);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.Assign)]
        public virtual async Task<DefectDto> AssignAsync(Guid id, AssignDefectDto input)
        {
            try
            {
                var defect = await _defectManager.AssignAsync(id, input.AssigneeId);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.ChangeStatus)]
        public virtual async Task<DefectDto> MarkAsInProgressAsync(Guid id)
        {
            try
            {
                var defect = await _defectManager.MarkAsInProgressAsync(id);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.ChangeStatus)]
        public virtual async Task<DefectDto> MarkAsFixedAsync(Guid id, MarkDefectAsFixedDto input)
        {
            try
            {
                var defect = await _defectManager.MarkAsFixedAsync(id, input.FixCommitId);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.VerifyFix)]
        public virtual async Task<DefectDto> VerifyFixAsync(Guid id, VerifyDefectFixDto input)
        {
            try
            {
                var defect = await _defectManager.VerifyFixAsync(id, input.FixedInVersion);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.VerifyFix)]
        public virtual async Task<DefectDto> RejectFixAsync(Guid id, string reason)
        {
            try
            {
                Check.NotNullOrWhiteSpace(reason, nameof(reason));
                
                var defect = await _defectManager.RejectFixAsync(id, reason);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.ChangeStatus)]
        public virtual async Task<DefectDto> CloseAsync(Guid id)
        {
            try
            {
                var defect = await _defectManager.CloseAsync(id);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.ChangeStatus)]
        public virtual async Task<DefectDto> ReopenAsync(Guid id, string reason)
        {
            try
            {
                Check.NotNullOrWhiteSpace(reason, nameof(reason));
                
                var defect = await _defectManager.ReopenAsync(id, reason);
                return ObjectMapper.Map<DefectReport, DefectDto>(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.Comment)]
        public virtual async Task<DefectCommentDto> AddCommentAsync(AddDefectCommentDto input)
        {
            try
            {
                var comment = await _defectManager.AddCommentAsync(input.DefectId, input.Text);
                return ObjectMapper.Map<DefectReport.DefectComment, DefectCommentDto>(comment);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.Update)]
        public virtual async Task LinkRequirementAsync(Guid id, Guid requirementId)
        {
            try
            {
                await _defectManager.LinkRequirementAsync(id, requirementId);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(QAPermissions.Defect.Update)]
        public virtual async Task UnlinkRequirementAsync(Guid id, Guid requirementId)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(id);
                defect.RemoveAffectedRequirement(requirementId);
                await _defectRepository.UpdateAsync(defect);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in DefectAppService: {ErrorMessage}", exception.Message);
            
            if (exception is BusinessException businessException)
            {
                throw new UserFriendlyException(
                    L[businessException.Code ?? "Error:UnknownBusinessError"], 
                    businessException.Code, 
                    businessException.Data);
            }
            
            return Task.CompletedTask;
        }
    }
}
