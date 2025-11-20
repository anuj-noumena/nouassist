using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead.Dtos;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Permissions;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Timing;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Lead
{
    [Authorize(CRMPermissions.Lead.Default)]
    public class LeadAppService : ApplicationService, ILeadAppService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ILeadManager _leadManager;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IClock _clock;
        private readonly ILogger<LeadAppService> _logger;

        public LeadAppService(
            ILeadRepository leadRepository,
            ILeadManager leadManager,
            IOrganizationRepository organizationRepository,
            IClock clock,
            ILogger<LeadAppService> logger)
        {
            _leadRepository = leadRepository;
            _leadManager = leadManager;
            _organizationRepository = organizationRepository;
            _clock = clock;
            _logger = logger;
            
            LocalizationResource = typeof(CRMResource);
        }

        [Authorize(CRMPermissions.Lead.View)]
        public virtual async Task<LeadDto> GetAsync(Guid id)
        {
            try
            {
                var lead = await _leadRepository.GetAsync(id);
                return ObjectMapper.Map<Aggregates.Lead, LeadDto>(lead);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Lead"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.View)]
        public virtual async Task<PagedResultDto<LeadDto>> GetListAsync(GetLeadListDto input)
        {
            try
            {
                // Get queryable and apply filters
                var queryable = await _leadRepository.GetQueryableAsync();
                
                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    queryable = queryable.Where(l => 
                        l.Name.Contains(input.Filter) || 
                        EF.Property<string>(l.ContactInfo, "Email").Contains(input.Filter) ||
                        EF.Property<string>(l.Details, "CompanyName").Contains(input.Filter));
                }
                
                if (input.Status.HasValue)
                {
                    queryable = queryable.Where(l => l.Status == input.Status.Value);
                }
                
                if (input.MinScore.HasValue)
                {
                    queryable = queryable.Where(l => l.Score >= input.MinScore.Value);
                }
                
                if (input.Source.HasValue)
                {
                    queryable = queryable.Where(l => EF.Property<LeadSource>(l.Details, "Source") == input.Source.Value);
                }
                
                if (!string.IsNullOrWhiteSpace(input.Tags))
                {
                    var tags = input.Tags.Split(',').Select(t => t.Trim()).ToList();
                    foreach (var tag in tags)
                    {
                        queryable = queryable.Where(l => l.Tags.Contains(tag));
                    }
                }
                
                if (input.CreatedFrom.HasValue)
                {
                    queryable = queryable.Where(l => l.CreationTime >= input.CreatedFrom.Value);
                }
                
                if (input.CreatedTo.HasValue)
                {
                    queryable = queryable.Where(l => l.CreationTime <= input.CreatedTo.Value);
                }
                
                // Get count
                var totalCount = await AsyncExecuter.CountAsync(queryable);
                
                // Apply sorting and paging
                var leads = await AsyncExecuter.ToListAsync(
                    queryable.OrderBy(input.Sorting ?? "CreationTime DESC")
                        .Skip(input.SkipCount)
                        .Take(input.MaxResultCount));
                
                // Map to DTOs
                var leadDtos = ObjectMapper.Map<List<Aggregates.Lead>, List<LeadDto>>(leads);
                
                return new PagedResultDto<LeadDto>
                {
                    TotalCount = totalCount,
                    Items = leadDtos
                };
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.Create)]
        public virtual async Task<LeadDto> CreateAsync(CreateLeadDto input)
        {
            try
            {
                // Map DTOs to value objects
                var contactInfo = new ContactInfo(
                    input.ContactInfo.Email,
                    input.ContactInfo.Phone,
                    input.ContactInfo.AlternativePhone,
                    input.ContactInfo.Website,
                    input.ContactInfo.Address,
                    input.ContactInfo.City,
                    input.ContactInfo.State,
                    input.ContactInfo.PostalCode,
                    input.ContactInfo.Country);
                
                var details = new LeadDetails(
                    input.Details.JobTitle,
                    input.Details.CompanyName,
                    input.Details.Industry,
                    input.Details.Source,
                    input.Details.ReferralSource,
                    input.Details.CampaignId,
                    input.Details.Notes,
                    input.Details.EstimatedBudget);
                
                // Create lead using domain service
                var lead = await _leadManager.CreateAsync(
                    input.Name,
                    contactInfo,
                    details,
                    LeadStatus.New,
                    input.Score ?? CRMConsts.Defaults.LeadScore);
                
                // Add tags if provided
                if (input.Tags != null && input.Tags.Any())
                {
                    foreach (var tag in input.Tags)
                    {
                        await _leadManager.AddTagAsync(lead.Id, tag);
                    }
                    
                    // Refresh lead to get updated tags
                    lead = await _leadRepository.GetAsync(lead.Id);
                }
                
                return ObjectMapper.Map<Aggregates.Lead, LeadDto>(lead);
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

        [Authorize(CRMPermissions.Lead.Update)]
        public virtual async Task<LeadDto> UpdateAsync(Guid id, UpdateLeadDto input)
        {
            try
            {
                var lead = await _leadRepository.GetAsync(id);
                
                // Update name if provided
                if (!string.IsNullOrWhiteSpace(input.Name))
                {
                    await _leadManager.UpdateAsync(id, name: input.Name);
                }
                
                // Update contact info if provided
                if (input.ContactInfo != null)
                {
                    ContactInfo contactInfo = null;
                    
                    // Only create a new ContactInfo if at least one property is provided
                    if (!string.IsNullOrWhiteSpace(input.ContactInfo.Email) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Phone) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.AlternativePhone) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Website) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Address) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.City) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.State) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.PostalCode) ||
                        !string.IsNullOrWhiteSpace(input.ContactInfo.Country))
                    {
                        // For fields not provided, use existing values
                        contactInfo = new ContactInfo(
                            input.ContactInfo.Email ?? lead.ContactInfo.Email,
                            input.ContactInfo.Phone ?? lead.ContactInfo.Phone,
                            input.ContactInfo.AlternativePhone ?? lead.ContactInfo.AlternativePhone,
                            input.ContactInfo.Website ?? lead.ContactInfo.Website,
                            input.ContactInfo.Address ?? lead.ContactInfo.Address,
                            input.ContactInfo.City ?? lead.ContactInfo.City,
                            input.ContactInfo.State ?? lead.ContactInfo.State,
                            input.ContactInfo.PostalCode ?? lead.ContactInfo.PostalCode,
                            input.ContactInfo.Country ?? lead.ContactInfo.Country);
                        
                        await _leadManager.UpdateAsync(id, contactInfo: contactInfo);
                    }
                }
                
                // Update details if provided
                if (input.Details != null)
                {
                    LeadDetails details = null;
                    
                    // Only create a new LeadDetails if at least one property is provided
                    if (!string.IsNullOrWhiteSpace(input.Details.JobTitle) ||
                        !string.IsNullOrWhiteSpace(input.Details.CompanyName) ||
                        !string.IsNullOrWhiteSpace(input.Details.Industry) ||
                        input.Details.Source.HasValue ||
                        !string.IsNullOrWhiteSpace(input.Details.ReferralSource) ||
                        !string.IsNullOrWhiteSpace(input.Details.CampaignId) ||
                        !string.IsNullOrWhiteSpace(input.Details.Notes) ||
                        input.Details.EstimatedBudget.HasValue)
                    {
                        // For fields not provided, use existing values
                        details = new LeadDetails(
                            input.Details.JobTitle ?? lead.Details.JobTitle,
                            input.Details.CompanyName ?? lead.Details.CompanyName,
                            input.Details.Industry ?? lead.Details.Industry,
                            input.Details.Source ?? lead.Details.Source,
                            input.Details.ReferralSource ?? lead.Details.ReferralSource,
                            input.Details.CampaignId ?? lead.Details.CampaignId,
                            input.Details.Notes ?? lead.Details.Notes,
                            input.Details.EstimatedBudget ?? lead.Details.EstimatedBudget);
                        
                        await _leadManager.UpdateAsync(id, details: details);
                    }
                }
                
                // Update score if provided
                if (input.Score.HasValue)
                {
                    await _leadManager.UpdateScoreAsync(id, input.Score.Value);
                }
                
                // Refresh lead to get updated values
                lead = await _leadRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Lead, LeadDto>(lead);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Lead"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _leadRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.Update)]
        public virtual async Task<LeadDto> ChangeStatusAsync(Guid id, ChangeLeadStatusDto input)
        {
            try
            {
                var lead = await _leadManager.ChangeStatusAsync(id, input.Status);
                return ObjectMapper.Map<Aggregates.Lead, LeadDto>(lead);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Lead"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.Convert)]
        public virtual async Task<LeadConversionResultDto> ConvertToContactAsync(Guid id, ConvertLeadDto input)
        {
            try
            {
                string organizationName = null;
                
                // Verify organization exists if provided
                if (input.OrganizationId.HasValue)
                {
                    var organization = await _organizationRepository.GetAsync(input.OrganizationId.Value);
                    organizationName = organization.Name;
                }
                
                // Convert the lead
                var (lead, contact) = await _leadManager.ConvertToContactAsync(
                    id,
                    input.OrganizationId,
                    input.IsPrimaryContact);
                
                // Return the result
                return new LeadConversionResultDto
                {
                    LeadId = lead.Id,
                    ContactId = contact.Id,
                    ContactName = contact.Name,
                    OrganizationId = contact.OrganizationId,
                    OrganizationName = organizationName
                };
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Lead"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.Update)]
        public virtual async Task<LeadDto> AddTagsAsync(Guid id, LeadTagsDto input)
        {
            try
            {
                var lead = await _leadRepository.GetAsync(id);
                
                foreach (var tag in input.Tags)
                {
                    await _leadManager.AddTagAsync(id, tag);
                }
                
                // Refresh lead to get updated tags
                lead = await _leadRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Lead, LeadDto>(lead);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Lead"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        [Authorize(CRMPermissions.Lead.Update)]
        public virtual async Task<LeadDto> RemoveTagsAsync(Guid id, LeadTagsDto input)
        {
            try
            {
                var lead = await _leadRepository.GetAsync(id);
                
                foreach (var tag in input.Tags)
                {
                    await _leadManager.RemoveTagAsync(id, tag);
                }
                
                // Refresh lead to get updated tags
                lead = await _leadRepository.GetAsync(id);
                
                return ObjectMapper.Map<Aggregates.Lead, LeadDto>(lead);
            }
            catch (BusinessException bex)
            {
                await HandleErrorAsync(bex, LogLevel.Warning);
                throw new UserFriendlyException(L[bex.Code ?? "DefaultError"], bex.Code);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["ResourceNotFound", "Lead"]);
                throw new UserFriendlyException(L["DefaultErrorMessage"]);
            }
        }

        protected virtual Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in LeadAppService: {ErrorMessage}", exception.Message);
            return Task.CompletedTask;
        }
    }
}
