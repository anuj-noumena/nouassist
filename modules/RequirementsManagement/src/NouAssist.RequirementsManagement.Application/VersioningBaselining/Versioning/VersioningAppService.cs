using System;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Localization;
using NouAssist.RequirementsManagement.VersioningBaselining.Permissions;
using NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces;
using NouAssist.RequirementsManagement.VersioningBaselining.Versioning.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Versioning
{
    [Authorize(VersionControlPermissions.Versioning.Default)]
    public class VersioningAppService : ApplicationService, IVersioningAppService
    {
        private readonly IVersioningEngine _versioningEngine;
        private readonly ILogger<VersioningAppService> _logger;

        public VersioningAppService(
            IVersioningEngine versioningEngine,
            ILogger<VersioningAppService> logger)
        {
            _versioningEngine = versioningEngine;
            _logger = logger;
            
            LocalizationResource = typeof(VersionControlResource);
        }

        [Authorize(VersionControlPermissions.Versioning.Create)]
        public virtual async Task<VersionDto> CreateVersionAsync(CreateVersionDto input)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var currentUserName = CurrentUser.Name ?? "Unknown User";
                
                var versionRecord = await _versioningEngine.CreateVersionAsync(
                    input.RequirementId,
                    input.ContentSnapshot,
                    input.ChangeComment,
                    currentUserId,
                    currentUserName,
                    input.ChangeType);
                    
                return ObjectMapper.Map<Entities.RequirementVersionRecord, VersionDto>(versionRecord);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Versioning.View)]
        public virtual async Task<VersionDto> GetVersionAsync(Guid requirementId, string version)
        {
            try
            {
                var versionRecord = await _versioningEngine.GetVersionAsync(requirementId, version);
                return ObjectMapper.Map<Entities.RequirementVersionRecord, VersionDto>(versionRecord);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Versioning.View)]
        public virtual async Task<VersionDto> GetLatestVersionAsync(Guid requirementId)
        {
            try
            {
                var versionRecord = await _versioningEngine.GetLatestVersionAsync(requirementId);
                return ObjectMapper.Map<Entities.RequirementVersionRecord, VersionDto>(versionRecord);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Versioning.View)]
        public virtual async Task<VersionHistoryDto> GetVersionHistoryAsync(GetVersionHistoryDto input)
        {
            try
            {
                var history = await _versioningEngine.GetOrCreateRequirementHistoryAsync(
                    input.RequirementId, 
                    CurrentTenant.Id);
                    
                return ObjectMapper.Map<Aggregates.VersionedRequirementHistory, VersionHistoryDto>(history);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(VersionControlPermissions.Versioning.Compare)]
        public virtual async Task<VersionComparisonResultDto> CompareVersionsAsync(CompareVersionsDto input)
        {
            try
            {
                var comparison = await _versioningEngine.CompareVersionsAsync(
                    input.RequirementId,
                    input.FirstVersion,
                    input.SecondVersion);
                    
                return ObjectMapper.Map<VersionComparisonResult, VersionComparisonResultDto>(comparison);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        protected virtual async Task HandleErrorAsync(Exception exception)
        {
            _logger.LogError(exception, "An error occurred in Versioning App Service: {Message}", exception.Message);
            
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
