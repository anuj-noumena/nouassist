using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Aggregates;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Entities;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Enums;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Exceptions;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Repositories;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Interfaces.Services;
using NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty.Dtos;
using NouAssist.StartupEntrepreneurship.LegalCompliance.Permissions;
using NouAssist.StartupEntrepreneurship.LegalCompliance.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.LegalCompliance.IntellectualProperty
{
    [Authorize(LegalCompliancePermissions.IPAsset.Default)]
    public class IPAssetAppService : ApplicationService, IIPAssetAppService
    {
        private readonly IIPPortfolioManager _ipPortfolioManager;
        private readonly IIntellectualPropertyRepository _ipRepository;
        private readonly ILogger<IPAssetAppService> _logger;

        public IPAssetAppService(
            IIPPortfolioManager ipPortfolioManager,
            IIntellectualPropertyRepository ipRepository,
            ILogger<IPAssetAppService> logger)
        {
            _ipPortfolioManager = ipPortfolioManager;
            _ipRepository = ipRepository;
            _logger = logger;
            LocalizationResource = typeof(LegalComplianceResource);
        }

        [Authorize(LegalCompliancePermissions.IPAsset.View)]
        public virtual async Task<IPAssetDto> GetAsync(Guid id)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(id);
                return ObjectMapper.Map<IntellectualPropertyAsset, IPAssetDto>(ipAsset);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found", id);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.View)]
        public virtual async Task<PagedResultDto<IPAssetListDto>> GetListAsync(GetIPAssetListDto input)
        {
            try
            {
                var query = await _ipRepository.GetQueryableAsync();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    query = query.Where(x => 
                        x.Name.Contains(input.Filter) || 
                        x.Description.Contains(input.Filter) ||
                        EF.Property<string>(x, "Identifier").Contains(input.Filter));
                }

                if (input.Type.HasValue)
                {
                    query = query.Where(x => x.Type == input.Type.Value);
                }

                if (input.Status.HasValue)
                {
                    query = query.Where(x => EF.Property<string>(x, "FilingStatus") == input.Status.Value.ToString());
                }

                if (!string.IsNullOrWhiteSpace(input.Country))
                {
                    query = query.Where(x => EF.Property<string>(x, "JurisdictionCountry") == input.Country);
                }

                if (input.HasDeadlines == true)
                {
                    query = query.Where(x => x.FilingDeadlines.Any());
                }

                var now = Clock.Now;

                if (input.IsExpired == true)
                {
                    query = query.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate.Value < now);
                }
                else if (input.IsExpired == false)
                {
                    query = query.Where(x => !x.ExpirationDate.HasValue || x.ExpirationDate.Value >= now);
                }

                if (input.IsExpiringSoon == true)
                {
                    var thresholdDate = now.AddMonths(6);
                    query = query.Where(x => 
                        x.ExpirationDate.HasValue && 
                        x.ExpirationDate.Value > now && 
                        x.ExpirationDate.Value <= thresholdDate);
                }

                // Get total count
                var totalCount = await AsyncExecuter.CountAsync(query);

                // Apply sorting
                query = query.OrderBy(input.Sorting ?? "Name");

                // Apply paging
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);

                // Execute query
                var ipAssets = await AsyncExecuter.ToListAsync(query);

                // Map and return results
                var items = ObjectMapper.Map<List<IntellectualPropertyAsset>, List<IPAssetListDto>>(ipAssets);
                return new PagedResultDto<IPAssetListDto>(totalCount, items);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.Create)]
        public virtual async Task<IPAssetDto> CreateAsync(CreateIPAssetDto input)
        {
            try
            {
                var ipAsset = await _ipPortfolioManager.CreateIPAssetAsync(
                    input.Name,
                    input.Description,
                    input.Type,
                    input.Identifier,
                    input.IssuingAuthority,
                    input.Country,
                    input.State,
                    input.FilingDate,
                    input.InitialStatus,
                    input.RegistrationDate,
                    input.ExpirationDate);

                return ObjectMapper.Map<IntellectualPropertyAsset, IPAssetDto>(ipAsset);
            }
            catch (DuplicateIPAssetException ex)
            {
                _logger.LogWarning(ex, "Attempt to create duplicate IP asset with identifier {Identifier} of type {Type}", 
                    input.Identifier, input.Type);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:DuplicateIPAsset", input.Identifier, L[$"LegalCompliance:IPType:{input.Type}"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.Update)]
        public virtual async Task<IPAssetDto> UpdateAsync(Guid id, UpdateIPAssetDto input)
        {
            try
            {
                var ipAsset = await _ipPortfolioManager.UpdateIPAssetAsync(
                    id,
                    input.Name,
                    input.Description,
                    input.Country,
                    input.State);

                return ObjectMapper.Map<IntellectualPropertyAsset, IPAssetDto>(ipAsset);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during update", id);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _ipRepository.DeleteAsync(id);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during delete", id);
                // We don't throw here since delete is idempotent
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.Update)]
        public virtual async Task<IPAssetDto> UpdateStatusAsync(Guid id, FilingStatusType newStatus, string notes = null)
        {
            try
            {
                await _ipPortfolioManager.UpdateIPStatusAsync(id, newStatus, notes);
                var ipAsset = await _ipRepository.GetAsync(id);
                return ObjectMapper.Map<IntellectualPropertyAsset, IPAssetDto>(ipAsset);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during status update", id);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.ManageValuation)]
        public virtual async Task SetValuationAsync(Guid id, IPValuationDto input)
        {
            try
            {
                await _ipPortfolioManager.SetIPValuationAsync(
                    id,
                    input.EstimatedValue,
                    input.Currency,
                    input.ValuationMethod,
                    input.ValuationNotes);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during valuation update", id);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.ManageDeadlines)]
        public virtual async Task<IPDeadlineDto> AddDeadlineAsync(Guid ipId, CreateIPDeadlineDto input)
        {
            try
            {
                await _ipPortfolioManager.AddIPFilingDeadlineAsync(
                    ipId,
                    input.Title,
                    input.Deadline,
                    input.Description,
                    input.IsRecurring,
                    input.RecurrencePattern);

                // Fetch the created deadline (we don't have a direct way to access it from the domain service)
                var ipAsset = await _ipRepository.GetAsync(ipId);
                
                // Find the latest deadline by matching criteria
                var deadline = ipAsset.FilingDeadlines
                    .Where(d => d.Title == input.Title && d.Deadline == input.Deadline)
                    .OrderByDescending(d => d.Id) // In case multiple match, get the most recent
                    .FirstOrDefault();

                if (deadline == null)
                {
                    throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:DeadlineCreationFailed"]);
                }

                return ObjectMapper.Map<FilingDeadline, IPDeadlineDto>(deadline);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during deadline addition", ipId);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.ManageDeadlines)]
        public virtual async Task RemoveDeadlineAsync(Guid ipId, Guid deadlineId)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(ipId);
                ipAsset.RemoveFilingDeadline(deadlineId);
                await _ipRepository.UpdateAsync(ipAsset);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during deadline removal", ipId);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.ManageDeadlines)]
        public virtual async Task<IPDeadlineDto> CompleteDeadlineAsync(Guid ipId, Guid deadlineId)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(ipId);
                var deadline = ipAsset.FilingDeadlines.FirstOrDefault(d => d.Id == deadlineId);
                
                if (deadline == null)
                {
                    throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:DeadlineNotFound"]);
                }
                
                deadline.Complete(Clock.Now);
                await _ipRepository.UpdateAsync(ipAsset);
                
                return ObjectMapper.Map<FilingDeadline, IPDeadlineDto>(deadline);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found when completing deadline", ipId);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Business rule violation when completing deadline {DeadlineId} for IP Asset {IpId}", deadlineId, ipId);
                throw new UserFriendlyException(L[ex.Code ?? "LegalCompliance:ErrorMessage:ValidationFailed", ex.Message]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.Update)]
        public virtual async Task<IPAssetDto> RenewAsync(Guid id, DateTime renewalDate, DateTime newExpirationDate, string notes = null)
        {
            try
            {
                var ipAsset = await _ipRepository.GetAsync(id);
                ipAsset.Renew(renewalDate, newExpirationDate, notes);
                await _ipRepository.UpdateAsync(ipAsset);
                
                return ObjectMapper.Map<IntellectualPropertyAsset, IPAssetDto>(ipAsset);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "IP Asset with ID {Id} not found during renewal", id);
                throw new UserFriendlyException(L["LegalCompliance:ErrorMessage:EntityNotFound", L["LegalCompliance:Entity:IntellectualProperty"]]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(LegalCompliancePermissions.IPAsset.View)]
        public virtual async Task<List<IPAssetListDto>> GetApproachingDeadlinesAsync(int daysThreshold = 30)
        {
            try
            {
                var ipAssets = await _ipPortfolioManager.GetIPAssetsWithApproachingDeadlinesAsync(daysThreshold);
                return ObjectMapper.Map<List<IntellectualPropertyAsset>, List<IPAssetListDto>>(ipAssets);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        // Helper method for error handling
        private Task HandleErrorAsync(Exception exception)
        {
            _logger.LogError(exception, "An error occurred in IPAssetAppService");
            return Task.CompletedTask;
        }
    }
}
