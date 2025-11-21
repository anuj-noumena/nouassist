using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.Traceability.Aggregates;
using NouAssist.RequirementsManagement.Traceability.Enums;
using NouAssist.RequirementsManagement.Traceability.Exceptions;
using NouAssist.RequirementsManagement.Traceability.Localization;
using NouAssist.RequirementsManagement.Traceability.Permissions;
using NouAssist.RequirementsManagement.Traceability.Repositories;
using NouAssist.RequirementsManagement.Traceability.Services.Interfaces;
using NouAssist.RequirementsManagement.Traceability.TraceLink.Dtos;
using NouAssist.RequirementsManagement.Traceability.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Timing;

namespace NouAssist.RequirementsManagement.Traceability.TraceLink
{
    /// <summary>
    /// Application service for trace link management
    /// </summary>
    [Authorize(TraceabilityManagementPermissions.TraceLinks.Default)]
    public class TraceLinkAppService : ApplicationService, ITraceLinkAppService
    {
        private readonly ITraceLinkManager _traceLinkManager;
        private readonly ITraceLinkRepository _traceLinkRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClock _clock;
        private readonly ILogger<TraceLinkAppService> _logger;

        public TraceLinkAppService(
            ITraceLinkManager traceLinkManager,
            ITraceLinkRepository traceLinkRepository,
            IGuidGenerator guidGenerator,
            IClock clock,
            ILogger<TraceLinkAppService> logger)
        {
            _traceLinkManager = traceLinkManager;
            _traceLinkRepository = traceLinkRepository;
            _guidGenerator = guidGenerator;
            _clock = clock;
            _logger = logger;
            
            LocalizationResource = typeof(TraceabilityManagementResource);
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceLinks.Create)]
        public virtual async Task<TraceLinkDto> CreateAsync(CreateTraceLinkDto input)
        {
            try
            {
                // Create source and target artifact identifiers
                var sourceArtifactId = new ArtifactIdentifier(
                    input.SourceArtifactId,
                    input.SourceArtifactType,
                    input.SourceArtifactVersion,
                    input.SourceArtifactUri);
                    
                var targetArtifactId = new ArtifactIdentifier(
                    input.TargetArtifactId,
                    input.TargetArtifactType,
                    input.TargetArtifactVersion,
                    input.TargetArtifactUri);
                
                // Create the trace link using domain service
                var traceLink = await _traceLinkManager.CreateAsync(
                    sourceArtifactId,
                    input.SourceArtifactName,
                    targetArtifactId,
                    input.TargetArtifactName,
                    input.LinkType,
                    input.LinkDirection,
                    input.CustomLinkType,
                    input.Description);
                
                // Map to DTO and return
                return ObjectMapper.Map<TraceLink, TraceLinkDto>(traceLink);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error:Unknown"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:TraceLink:CreateFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceLinks.View)]
        public virtual async Task<TraceLinkDto> GetAsync(Guid id)
        {
            try
            {
                var traceLink = await _traceLinkRepository.GetAsync(id);
                return ObjectMapper.Map<TraceLink, TraceLinkDto>(traceLink);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["Error:TraceLink:NotFound"]);
                }
                throw new UserFriendlyException(L["Error:TraceLink:GetFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceLinks.View)]
        public virtual async Task<PagedResultDto<TraceLinkDto>> GetListAsync(GetTraceLinkListDto input)
        {
            try
            {
                // Get queryable with included details
                var query = await _traceLinkRepository.WithDetailsAsync();
                
                // Apply filters
                query = ApplyFilters(query, input);
                
                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting and paging
                query = query.OrderBy(input.Sorting ?? "creationTime desc")
                    .PageBy(input);
                
                // Execute query and map results
                var traceLinks = await AsyncExecuter.ToListAsync(query);
                var dtos = ObjectMapper.Map<List<TraceLink>, List<TraceLinkDto>>(traceLinks);
                
                return new PagedResultDto<TraceLinkDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["Error:TraceLink:ListFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceLinks.Update)]
        public virtual async Task<TraceLinkDto> UpdateAsync(Guid id, UpdateTraceLinkDto input)
        {
            try
            {
                // Update the trace link using domain service
                var traceLink = await _traceLinkManager.UpdateAsync(
                    id,
                    input.LinkType,
                    input.LinkDirection,
                    input.CustomLinkType,
                    input.Description);
                
                // Map to DTO and return
                return ObjectMapper.Map<TraceLink, TraceLinkDto>(traceLink);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error:Unknown"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["Error:TraceLink:NotFound"]);
                }
                throw new UserFriendlyException(L["Error:TraceLink:UpdateFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceLinks.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                // Delete the trace link using domain service
                await _traceLinkManager.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["Error:TraceLink:NotFound"]);
                }
                throw new UserFriendlyException(L["Error:TraceLink:DeleteFailed"]);
            }
        }

        /// <inheritdoc />
        [Authorize(TraceabilityManagementPermissions.TraceLinks.Verify)]
        public virtual async Task<TraceLinkDto> VerifyAsync(Guid id)
        {
            try
            {
                // Get current user ID
                var currentUserId = CurrentUser.Id ?? throw new UserFriendlyException(L["Error:UserNotLoggedIn"]);
                
                // Verify the trace link using domain service
                var traceLink = await _traceLinkManager.VerifyAsync(id, currentUserId);
                
                // Map to DTO and return
                return ObjectMapper.Map<TraceLink, TraceLinkDto>(traceLink);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "Error:Unknown"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                {
                    throw new UserFriendlyException(L["Error:TraceLink:NotFound"]);
                }
                throw new UserFriendlyException(L["Error:TraceLink:VerifyFailed"]);
            }
        }

        /// <summary>
        /// Applies filters to the query based on input DTO
        /// </summary>
        private IQueryable<TraceLink> ApplyFilters(IQueryable<TraceLink> query, GetTraceLinkListDto input)
        {
            // Filter by text (description or artifact names)
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(x => 
                    x.Description.Contains(input.Filter) ||
                    x.SourceArtifact.Name.Contains(input.Filter) ||
                    x.TargetArtifact.Name.Contains(input.Filter));
            }
            
            // Filter by link type
            if (input.LinkType.HasValue)
            {
                query = query.Where(x => x.LinkType == input.LinkType.Value);
            }
            
            // Filter by link direction
            if (input.LinkDirection.HasValue)
            {
                query = query.Where(x => x.LinkDirection == input.LinkDirection.Value);
            }
            
            // Filter by source artifact ID
            if (!string.IsNullOrWhiteSpace(input.SourceArtifactId))
            {
                query = query.Where(x => x.SourceArtifact.ArtifactIdentifier.Id == input.SourceArtifactId);
            }
            
            // Filter by source artifact type
            if (input.SourceArtifactType.HasValue)
            {
                query = query.Where(x => x.SourceArtifact.ArtifactIdentifier.Type == input.SourceArtifactType.Value);
            }
            
            // Filter by target artifact ID
            if (!string.IsNullOrWhiteSpace(input.TargetArtifactId))
            {
                query = query.Where(x => x.TargetArtifact.ArtifactIdentifier.Id == input.TargetArtifactId);
            }
            
            // Filter by target artifact type
            if (input.TargetArtifactType.HasValue)
            {
                query = query.Where(x => x.TargetArtifact.ArtifactIdentifier.Type == input.TargetArtifactType.Value);
            }
            
            // Filter by suspect status
            if (input.OnlySuspect.HasValue && input.OnlySuspect.Value)
            {
                query = query.Where(x => x.IsSuspect);
            }
            
            return query;
        }

        /// <summary>
        /// Handles and logs errors
        /// </summary>
        private Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            _logger.Log(logLevel, exception, "Error in TraceLinkAppService: {Message}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
