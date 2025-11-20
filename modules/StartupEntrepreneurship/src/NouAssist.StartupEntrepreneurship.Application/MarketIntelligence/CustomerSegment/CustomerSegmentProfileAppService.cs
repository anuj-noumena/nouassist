using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment.Dtos;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Enums;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Permissions;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.Repositories;
using NouAssist.StartupEntrepreneurship.MarketIntelligence.ValueObjects;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.StartupEntrepreneurship.MarketIntelligence.CustomerSegment
{
    [Authorize(MarketIntelligencePermissions.CustomerSegment.Default)]
    public class CustomerSegmentProfileAppService : ApplicationService, ICustomerSegmentProfileAppService
    {
        private readonly ICustomerSegmentProfileRepository _segmentRepository;
        private readonly ICustomerSegmentProfileService _segmentService;
        private readonly ILogger<CustomerSegmentProfileAppService> _logger;

        public CustomerSegmentProfileAppService(
            ICustomerSegmentProfileRepository segmentRepository,
            ICustomerSegmentProfileService segmentService,
            ILogger<CustomerSegmentProfileAppService> logger)
        {
            _segmentRepository = segmentRepository;
            _segmentService = segmentService;
            _logger = logger;
            
            LocalizationResource = typeof(MarketIntelligenceResource);
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.View)]
        public virtual async Task<CustomerSegmentProfileDto> GetAsync(Guid id)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(id);
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["ErrorMessage:CustomerSegmentNotFound"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.View)]
        public virtual async Task<PagedResultDto<CustomerSegmentProfileDto>> GetListAsync(GetCustomerSegmentListDto input)
        {
            try
            {
                var query = await _segmentRepository.GetQueryableAsync();
                
                // Apply filters
                query = query
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), 
                        x => x.Name.Contains(input.Filter) || 
                             x.Description.Contains(input.Filter))
                    .WhereIf(input.Type.HasValue, x => x.Type == input.Type.Value)
                    .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive.Value)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.AgeRange), 
                        x => EF.Property<string>(x, "Demographics_AgeRange").Contains(input.AgeRange))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Income), 
                        x => EF.Property<string>(x, "Demographics_Income").Contains(input.Income))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Location), 
                        x => EF.Property<string>(x, "Demographics_Location").Contains(input.Location));

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                // Apply sorting
                query = ApplySorting(query, input);
                
                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                // Include characteristics if requested
                if (input.IncludeCharacteristics)
                {
                    query = query.Include(x => x.Characteristics);
                }
                
                // Execute query
                var segments = await AsyncExecuter.ToListAsync(query);
                
                // Map to DTOs
                var segmentDtos = ObjectMapper.Map<List<Aggregates.CustomerSegmentProfile>, List<CustomerSegmentProfileDto>>(segments);
                
                return new PagedResultDto<CustomerSegmentProfileDto>(totalCount, segmentDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Create)]
        public virtual async Task<CustomerSegmentProfileDto> CreateAsync(CreateCustomerSegmentProfileDto input)
        {
            try
            {
                var demographics = ObjectMapper.Map<CreateDemographicsDto, Demographics>(input.Demographics);
                var psychographics = ObjectMapper.Map<CreatePsychographicsDto, Psychographics>(input.Psychographics);
                
                var segment = await _segmentService.CreateSegmentProfileAsync(
                    input.Name,
                    input.Description,
                    input.Type,
                    demographics,
                    psychographics,
                    input.PrimaryNeeds);
                
                // Set additional fields if provided
                if (!string.IsNullOrEmpty(input.PainPoints) || !string.IsNullOrEmpty(input.PreferredChannels))
                {
                    segment.SetPainPoints(input.PainPoints);
                    segment.SetPreferredChannels(input.PreferredChannels);
                    await _segmentRepository.UpdateAsync(segment);
                }
                
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Update)]
        public virtual async Task<CustomerSegmentProfileDto> UpdateAsync(Guid id, UpdateCustomerSegmentProfileDto input)
        {
            try
            {
                var demographics = ObjectMapper.Map<CreateDemographicsDto, Demographics>(input.Demographics);
                var psychographics = ObjectMapper.Map<CreatePsychographicsDto, Psychographics>(input.Psychographics);
                
                var segment = await _segmentService.UpdateSegmentProfileAsync(
                    id,
                    input.Name,
                    input.Description,
                    demographics,
                    psychographics,
                    input.PrimaryNeeds);
                
                // Update additional fields
                segment.SetPainPoints(input.PainPoints);
                segment.SetPreferredChannels(input.PreferredChannels);
                await _segmentRepository.UpdateAsync(segment);
                
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _segmentRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Update)]
        public virtual async Task<CustomerSegmentProfileDto> UpdateMarketSizingAsync(Guid id, UpdateMarketSizingDto input)
        {
            try
            {
                await _segmentService.UpdateMarketSizingAsync(id, input.EstimatedSize, input.EstimatedValue);
                var segment = await _segmentRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.ManageCharacteristics)]
        public virtual async Task<SegmentCharacteristicsDto> AddCharacteristicAsync(Guid id, CreateSegmentCharacteristicDto input)
        {
            try
            {
                await _segmentService.AddSegmentCharacteristicAsync(
                    id,
                    input.CharacteristicName,
                    input.CharacteristicValue,
                    input.Category,
                    input.Importance,
                    input.Notes);
                
                // Get the updated segment to retrieve the newest characteristic
                var segment = await _segmentRepository.GetAsync(id);
                var latestCharacteristic = segment.Characteristics
                    .FirstOrDefault(c => c.CharacteristicName == input.CharacteristicName);
                
                if (latestCharacteristic == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:CharacteristicNotFound"]);
                }
                
                return ObjectMapper.Map<Entities.SegmentCharacteristics, SegmentCharacteristicsDto>(latestCharacteristic);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.ManageCharacteristics)]
        public virtual async Task<SegmentCharacteristicsDto> UpdateCharacteristicAsync(
            Guid segmentId, 
            Guid characteristicId, 
            CreateSegmentCharacteristicDto input)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(segmentId);
                
                segment.UpdateCharacteristic(
                    characteristicId,
                    input.CharacteristicName,
                    input.CharacteristicValue,
                    input.Category,
                    input.Importance,
                    input.Notes);
                
                await _segmentRepository.UpdateAsync(segment);
                
                var updatedCharacteristic = segment.Characteristics.FirstOrDefault(c => c.Id == characteristicId);
                
                if (updatedCharacteristic == null)
                {
                    throw new UserFriendlyException(L["ErrorMessage:CharacteristicNotFound"]);
                }
                
                return ObjectMapper.Map<Entities.SegmentCharacteristics, SegmentCharacteristicsDto>(updatedCharacteristic);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.ManageCharacteristics)]
        public virtual async Task DeleteCharacteristicAsync(Guid segmentId, Guid characteristicId)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(segmentId);
                segment.RemoveCharacteristic(characteristicId);
                await _segmentRepository.UpdateAsync(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Update)]
        public virtual async Task<CustomerSegmentProfileDto> ChangeTypeAsync(Guid id, SegmentType newType)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(id);
                segment.ChangeType(newType);
                await _segmentRepository.UpdateAsync(segment);
                
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Update)]
        public virtual async Task<CustomerSegmentProfileDto> DeactivateAsync(Guid id)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(id);
                segment.Deactivate();
                await _segmentRepository.UpdateAsync(segment);
                
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.Update)]
        public virtual async Task<CustomerSegmentProfileDto> ReactivateAsync(Guid id)
        {
            try
            {
                var segment = await _segmentRepository.GetAsync(id);
                segment.Reactivate();
                await _segmentRepository.UpdateAsync(segment);
                
                return ObjectMapper.Map<Aggregates.CustomerSegmentProfile, CustomerSegmentProfileDto>(segment);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError", bex.Message], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.View)]
        public virtual async Task<List<CustomerSegmentProfileDto>> GetActiveSegmentsAsync()
        {
            try
            {
                var segments = await _segmentService.GetActiveSegmentProfilesAsync();
                return ObjectMapper.Map<List<Aggregates.CustomerSegmentProfile>, List<CustomerSegmentProfileDto>>(segments);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.View)]
        public virtual async Task<List<CustomerSegmentProfileDto>> GetPrimarySegmentsAsync()
        {
            try
            {
                var segments = await _segmentService.GetPrimarySegmentsAsync();
                return ObjectMapper.Map<List<Aggregates.CustomerSegmentProfile>, List<CustomerSegmentProfileDto>>(segments);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(MarketIntelligencePermissions.CustomerSegment.View)]
        public virtual async Task<Dictionary<Guid, decimal>> EvaluateSegmentAttractivenessAsync()
        {
            try
            {
                return await _segmentService.EvaluateSegmentAttractivenessAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        // Helper method for sorting
        private static IQueryable<Aggregates.CustomerSegmentProfile> ApplySorting(
            IQueryable<Aggregates.CustomerSegmentProfile> query, 
            GetCustomerSegmentListDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Default sorting by segment type and then by name
                return query.OrderBy(x => x.Type).ThenBy(x => x.Name);
            }
            
            return query.OrderBy(input.Sorting);
        }

        // Error handling helper
        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in CustomerSegmentProfileAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
