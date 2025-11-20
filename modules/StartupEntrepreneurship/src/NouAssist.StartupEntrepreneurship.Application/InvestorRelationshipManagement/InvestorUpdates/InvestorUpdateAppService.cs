using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Entities;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorUpdates
{
    /// <summary>
    /// Application service for investor update management
    /// </summary>
    [Authorize(IRMPermissions.Updates.Default)]
    public class InvestorUpdateAppService : ApplicationService, IInvestorUpdateAppService
    {
        private readonly IInvestorUpdateRepository _updateRepository;
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly IUpdateGenerationService _updateGenerationService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public InvestorUpdateAppService(
            IInvestorUpdateRepository updateRepository,
            IInvestorProfileRepository investorRepository,
            IUpdateGenerationService updateGenerationService,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _updateRepository = updateRepository;
            _investorRepository = investorRepository;
            _updateGenerationService = updateGenerationService;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(IRMResource);
        }

        [Authorize(IRMPermissions.Updates.View)]
        public async Task<InvestorUpdateDto> GetAsync(Guid id)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);
                return await MapToExtendedDtoAsync(update);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
            }
        }

        [Authorize(IRMPermissions.Updates.View)]
        public async Task<InvestorUpdateDto> GetWithDetailsAsync(Guid id)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);
                return await MapToExtendedDtoAsync(update, includeDetails: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
            }
        }

        [Authorize(IRMPermissions.Updates.View)]
        public async Task<PagedResultDto<InvestorUpdateDto>> GetListAsync(GetInvestorUpdateListDto input)
        {
            try
            {
                var queryable = await _updateRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    u => u.Title.Contains(input.Filter!) || u.Subject.Contains(input.Filter!));

                queryable = queryable.WhereIf(input.Status.HasValue, u => u.Status == input.Status.Value);
                queryable = queryable.WhereIf(input.Frequency.HasValue, u => u.Frequency == input.Frequency.Value);
                queryable = queryable.WhereIf(!input.TemplateId.IsNullOrWhiteSpace(), u => u.TemplateId == input.TemplateId);

                // Date filters
                queryable = queryable.WhereIf(input.FromDate.HasValue,
                    u => u.SentDate >= input.FromDate.Value || u.ScheduledDate >= input.FromDate.Value);
                queryable = queryable.WhereIf(input.ToDate.HasValue,
                    u => u.SentDate <= input.ToDate.Value || u.ScheduledDate <= input.ToDate.Value);

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var items = new List<InvestorUpdate>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        queryable.OrderBy(input.Sorting ?? nameof(InvestorUpdate.CreationTime) + " desc")
                                .PageBy(input)
                    );
                }

                var itemDtos = new List<InvestorUpdateDto>();
                foreach (var item in items)
                {
                    var dto = await MapToExtendedDtoAsync(item, includeStatistics: input.IncludeStatistics);
                    itemDtos.Add(dto);
                }

                return new PagedResultDto<InvestorUpdateDto>(totalCount, itemDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Create)]
        public async Task<InvestorUpdateDto> CreateAsync(CreateInvestorUpdateDto input)
        {
            try
            {
                var id = _guidGenerator.Create();
                var currentTenantId = _currentTenant.Id;

                var update = InvestorUpdate.Create(
                    id,
                    input.Title,
                    input.Subject,
                    input.Frequency,
                    input.TemplateId,
                    currentTenantId
                );

                // Add sections
                foreach (var sectionDto in input.Sections)
                {
                    var content = new UpdateContent(
                        sectionDto.Content,
                        sectionDto.ContentType,
                        sectionDto.ContentMetadata
                    );
                    update.AddSection(sectionDto.Title, content, sectionDto.Order);
                }

                // Add metrics
                foreach (var metricDto in input.Metrics)
                {
                    var metric = new MetricValue(
                        metricDto.MetricName,
                        metricDto.MetricValue,
                        metricDto.MetricUnit,
                        metricDto.MetricPreviousValue,
                        metricDto.MetricTargetValue
                    );
                    update.AddMetric(metric, metricDto.SnapshotDate);
                }

                await _updateRepository.InsertAsync(update, autoSave: true);

                return await MapToExtendedDtoAsync(update);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Update)]
        public async Task<InvestorUpdateDto> UpdateAsync(Guid id, UpdateInvestorUpdateDto input)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                update.UpdateTitle(input.Title);
                update.UpdateSubject(input.Subject);

                // Update sections - remove all existing and add new ones
                foreach (var existingSection in update.Sections.ToList())
                {
                    update.RemoveSection(existingSection.Id);
                }

                foreach (var sectionDto in input.Sections)
                {
                    var content = new UpdateContent(
                        sectionDto.Content,
                        sectionDto.ContentType,
                        sectionDto.ContentMetadata
                    );

                    if (sectionDto.Id.HasValue && sectionDto.Id != Guid.Empty)
                    {
                        // Update existing section
                        update.UpdateSection(sectionDto.Id.Value, sectionDto.Title, content, sectionDto.Order);
                    }
                    else
                    {
                        // Add new section
                        update.AddSection(sectionDto.Title, content, sectionDto.Order);
                    }
                }

                // Update metrics - similar approach
                foreach (var metricDto in input.Metrics)
                {
                    var metric = new MetricValue(
                        metricDto.MetricName,
                        metricDto.MetricValue,
                        metricDto.MetricUnit,
                        metricDto.MetricPreviousValue,
                        metricDto.MetricTargetValue
                    );
                    update.AddMetric(metric, metricDto.SnapshotDate);
                }

                await _updateRepository.UpdateAsync(update, autoSave: true);

                return await MapToExtendedDtoAsync(update);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                await _updateRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Update)]
        public async Task<UpdateSectionDto> AddSectionAsync(Guid id, CreateUpdateSectionDto input)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                var content = new UpdateContent(
                    input.Content,
                    input.ContentType,
                    input.ContentMetadata
                );

                update.AddSection(input.Title, content, input.Order);
                await _updateRepository.UpdateAsync(update, autoSave: true);

                // Return the newly added section
                var newSection = update.Sections.OrderByDescending(s => s.Order).First();
                return ObjectMapper.Map<UpdateSection, UpdateSectionDto>(newSection);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Update)]
        public async Task<UpdateSectionDto> UpdateSectionAsync(Guid id, Guid sectionId, UpdateUpdateSectionDto input)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                var content = new UpdateContent(
                    input.Content,
                    input.ContentType,
                    input.ContentMetadata
                );

                update.UpdateSection(sectionId, input.Title, content, input.Order);
                await _updateRepository.UpdateAsync(update, autoSave: true);

                // Return the updated section
                var updatedSection = update.Sections.First(s => s.Id == sectionId);
                return ObjectMapper.Map<UpdateSection, UpdateSectionDto>(updatedSection);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Update)]
        public async Task DeleteSectionAsync(Guid id, Guid sectionId)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                update.RemoveSection(sectionId);
                await _updateRepository.UpdateAsync(update, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Update)]
        public async Task<MetricSnapshotDto> AddMetricAsync(Guid id, CreateMetricSnapshotDto input)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                var metric = new MetricValue(
                    input.MetricName,
                    input.MetricValue,
                    input.MetricUnit,
                    input.MetricPreviousValue,
                    input.MetricTargetValue
                );

                update.AddMetric(metric, input.SnapshotDate);
                await _updateRepository.UpdateAsync(update, autoSave: true);

                // Return the newly added metric
                var newMetric = update.Metrics.OrderByDescending(m => m.SnapshotDate).First();
                return await MapMetricSnapshotToDtoAsync(newMetric);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Update)]
        public async Task DeleteMetricAsync(Guid id, Guid metricId)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                // Note: This would require additional logic in the domain model to remove metrics
                // For now, we'll throw an exception indicating the feature needs implementation
                throw new UserFriendlyException(L["IRM:ErrorMessage:InvalidOperation"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Schedule)]
        public async Task ScheduleAsync(Guid id, ScheduleUpdateDto input)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                update.Schedule(input.ScheduledDate);
                await _updateRepository.UpdateAsync(update, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.Send)]
        public async Task<InvestorUpdateDto> SendAsync(Guid id, SendUpdateDto input)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                // Validate that investors exist
                var investors = await _investorRepository.GetByIdsAsync(input.InvestorIds);
                if (investors.Count != input.InvestorIds.Count)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:InvalidOperation", "Some investors not found"]);
                }

                var sendDate = input.SendDate ?? _clock.Now;
                update.Send(input.InvestorIds, sendDate);
                await _updateRepository.UpdateAsync(update, autoSave: true);

                return await MapToExtendedDtoAsync(update);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.View)]
        public async Task<PagedResultDto<InvestorUpdateDto>> GetScheduledUpdatesAsync(PagedAndSortedResultRequestDto input)
        {
            try
            {
                var cutoffDate = _clock.Now.AddDays(7); // Next 7 days
                var updates = await _updateRepository.GetScheduledUpdatesAsync(cutoffDate);

                var updateDtos = new List<InvestorUpdateDto>();
                foreach (var update in updates.Skip(input.SkipCount).Take(input.MaxResultCount))
                {
                    var dto = await MapToExtendedDtoAsync(update);
                    updateDtos.Add(dto);
                }

                return new PagedResultDto<InvestorUpdateDto>(updates.Count, updateDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.ViewEngagement)]
        public async Task TrackEngagementAsync(Guid id, TrackEngagementDto input)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                update.TrackEngagement(
                    input.InvestorId,
                    input.Type,
                    _clock.Now,
                    input.Details,
                    input.IpAddress,
                    input.UserAgent
                );

                await _updateRepository.UpdateAsync(update, autoSave: true);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "IRM:ErrorMessage:DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.ViewEngagement)]
        public async Task<PagedResultDto<EngagementRecordDto>> GetEngagementRecordsAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                var engagements = update.EngagementRecords
                    .OrderByDescending(e => e.EngagedAt)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var engagementDtos = new List<EngagementRecordDto>();
                foreach (var engagement in engagements)
                {
                    var dto = ObjectMapper.Map<EngagementRecord, EngagementRecordDto>(engagement);
                    
                    // Add investor name
                    var investor = await _investorRepository.FindAsync(engagement.InvestorId);
                    dto.InvestorName = investor?.FullName ?? "Unknown";
                    
                    engagementDtos.Add(dto);
                }

                return new PagedResultDto<EngagementRecordDto>(
                    update.EngagementRecords.Count,
                    engagementDtos
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.ViewEngagement)]
        public async Task<PagedResultDto<DistributionLogDto>> GetDistributionLogsAsync(Guid id, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var update = await _updateRepository.GetWithDetailsAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                var distributions = update.DistributionLogs
                    .OrderByDescending(d => d.AttemptedAt)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var distributionDtos = new List<DistributionLogDto>();
                foreach (var distribution in distributions)
                {
                    var dto = ObjectMapper.Map<DistributionLog, DistributionLogDto>(distribution);
                    
                    // Add investor name
                    var investor = await _investorRepository.FindAsync(distribution.InvestorId);
                    dto.InvestorName = investor?.FullName ?? "Unknown";
                    
                    distributionDtos.Add(dto);
                }

                return new PagedResultDto<DistributionLogDto>(
                    update.DistributionLogs.Count,
                    distributionDtos
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.ViewEngagement)]
        public async Task<decimal> GetEngagementRateAsync(Guid id)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                return update.GetEngagementRate();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.ViewEngagement)]
        public async Task<int> GetDeliveryCountAsync(Guid id)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                return update.GetDeliveryCount();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Updates.ViewEngagement)]
        public async Task<int> GetReadCountAsync(Guid id)
        {
            try
            {
                var update = await _updateRepository.GetAsync(id);

                // Check tenant consistency
                if (update.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                }

                return update.GetReadCount();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorUpdate"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        // Helper Methods
        private async Task<InvestorUpdateDto> MapToExtendedDtoAsync(InvestorUpdate update, bool includeDetails = false, bool includeStatistics = true)
        {
            var dto = ObjectMapper.Map<InvestorUpdate, InvestorUpdateDto>(update);

            if (includeStatistics)
            {
                // Calculate statistics
                dto.SectionCount = update.Sections.Count;
                dto.MetricCount = update.Metrics.Count;
                dto.RecipientCount = update.DistributionLogs.Count;
                dto.DeliveredCount = update.GetDeliveryCount();
                dto.ReadCount = update.GetReadCount();
                dto.EngagementCount = update.GetEngagementCount();
                dto.EngagementRate = update.GetEngagementRate();
            }

            if (includeDetails)
            {
                // Map sections
                dto.Sections = ObjectMapper.Map<List<UpdateSection>, List<UpdateSectionDto>>(update.Sections.ToList());

                // Map metrics with calculated properties
                foreach (var metric in update.Metrics)
                {
                    var metricDto = await MapMetricSnapshotToDtoAsync(metric);
                    dto.Metrics.Add(metricDto);
                }

                // Map distribution logs with investor names
                foreach (var distribution in update.DistributionLogs)
                {
                    var distributionDto = ObjectMapper.Map<DistributionLog, DistributionLogDto>(distribution);
                    var investor = await _investorRepository.FindAsync(distribution.InvestorId);
                    distributionDto.InvestorName = investor?.FullName ?? "Unknown";
                    dto.DistributionLogs.Add(distributionDto);
                }

                // Map engagement records with investor names
                foreach (var engagement in update.EngagementRecords)
                {
                    var engagementDto = ObjectMapper.Map<EngagementRecord, EngagementRecordDto>(engagement);
                    var investor = await _investorRepository.FindAsync(engagement.InvestorId);
                    engagementDto.InvestorName = investor?.FullName ?? "Unknown";
                    dto.EngagementRecords.Add(engagementDto);
                }
            }

            return dto;
        }

        private async Task<MetricSnapshotDto> MapMetricSnapshotToDtoAsync(MetricSnapshot metric)
        {
            var dto = ObjectMapper.Map<MetricSnapshot, MetricSnapshotDto>(metric);

            // Calculate derived properties
            dto.GrowthPercentage = metric.Metric.GetGrowthPercentage();
            dto.TargetAchievementPercentage = metric.Metric.GetTargetAchievementPercentage();

            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in InvestorUpdateAppService: {ErrorMessage}", exception.Message);
        }
    }
}
