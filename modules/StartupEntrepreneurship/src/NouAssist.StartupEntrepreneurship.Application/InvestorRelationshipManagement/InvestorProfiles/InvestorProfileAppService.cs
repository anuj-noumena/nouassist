using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Enums;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles.Dtos;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Permissions;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Repositories;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Services;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.Specifications;
using NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.StartupEntrepreneurship.InvestorRelationshipManagement.InvestorProfiles
{
    /// <summary>
    /// Application service for investor profile management
    /// </summary>
    [Authorize(IRMPermissions.Investors.Default)]
    public class InvestorProfileAppService : ApplicationService, IInvestorProfileAppService
    {
        private readonly IInvestorProfileRepository _investorRepository;
        private readonly IInvestorScoringService _scoringService;
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentTenant _currentTenant;
        private readonly IClock _clock;
        private readonly IGuidGenerator _guidGenerator;

        public InvestorProfileAppService(
            IInvestorProfileRepository investorRepository,
            IInvestorScoringService scoringService,
            ICurrentUser currentUser,
            ICurrentTenant currentTenant,
            IClock clock,
            IGuidGenerator guidGenerator)
        {
            _investorRepository = investorRepository;
            _scoringService = scoringService;
            _currentUser = currentUser;
            _currentTenant = currentTenant;
            _clock = clock;
            _guidGenerator = guidGenerator;

            LocalizationResource = typeof(IRMResource);
        }

        [Authorize(IRMPermissions.Investors.View)]
        public async Task<InvestorProfileDto> GetAsync(Guid id)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(id);
                return await MapToExtendedDtoAsync(investor);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
            }
        }

        [Authorize(IRMPermissions.Investors.View)]
        public async Task<PagedResultDto<InvestorProfileDto>> GetListAsync(GetInvestorProfileListDto input)
        {
            try
            {
                var queryable = await _investorRepository.GetQueryableAsync();

                // Apply filters
                queryable = queryable.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    i => i.FirstName.Contains(input.Filter!) ||
                         i.LastName.Contains(input.Filter!) ||
                         i.CompanyName.Contains(input.Filter!) ||
                         i.ContactDetails.Email.Contains(input.Filter!));

                queryable = queryable.WhereIf(input.Type.HasValue, i => i.Type == input.Type.Value);
                queryable = queryable.WhereIf(input.Status.HasValue, i => i.Status == input.Status.Value);
                queryable = queryable.WhereIf(!input.CompanyName.IsNullOrWhiteSpace(),
                    i => i.CompanyName.Contains(input.CompanyName!));

                // Apply investment amount filters using a specification
                if (input.MinInvestmentAmount.HasValue || input.MaxInvestmentAmount.HasValue)
                {
                    queryable = queryable.Where(i => i.Investments.Any(inv => inv.IsActive &&
                        (!input.MinInvestmentAmount.HasValue || inv.Commitment.InvestedAmount >= input.MinInvestmentAmount.Value) &&
                        (!input.MaxInvestmentAmount.HasValue || inv.Commitment.InvestedAmount <= input.MaxInvestmentAmount.Value)));
                }

                // Apply relationship score filters
                queryable = queryable.WhereIf(input.MinRelationshipScore.HasValue,
                    i => i.RelationshipScore.Score >= input.MinRelationshipScore.Value);
                queryable = queryable.WhereIf(input.MaxRelationshipScore.HasValue,
                    i => i.RelationshipScore.Score <= input.MaxRelationshipScore.Value);

                // Apply recent activity filter
                if (input.HasRecentActivity.HasValue)
                {
                    var cutoffDate = _clock.Now.AddDays(-90);
                    if (input.HasRecentActivity.Value)
                    {
                        queryable = queryable.Where(i => i.LastInteractionDate >= cutoffDate);
                    }
                    else
                    {
                        queryable = queryable.Where(i => i.LastInteractionDate < cutoffDate || i.LastInteractionDate == null);
                    }
                }

                // Group filters require additional logic
                if (input.GroupIds.Any())
                {
                    var investorsInGroups = new List<Guid>();
                    foreach (var groupId in input.GroupIds)
                    {
                        var groupInvestors = await _investorRepository.GetInvestorsByGroupAsync(groupId);
                        investorsInGroups.AddRange(groupInvestors.Select(i => i.Id));
                    }
                    queryable = queryable.Where(i => investorsInGroups.Contains(i.Id));
                }

                var totalCount = await AsyncExecuter.CountAsync(queryable);

                var items = new List<InvestorProfile>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        queryable.OrderBy(input.Sorting ?? nameof(InvestorProfile.LastName))
                                .PageBy(input)
                    );
                }

                var itemDtos = new List<InvestorProfileDto>();
                foreach (var item in items)
                {
                    var dto = await MapToExtendedDtoAsync(item);
                    itemDtos.Add(dto);
                }

                return new PagedResultDto<InvestorProfileDto>(totalCount, itemDtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.Create)]
        public async Task<InvestorProfileDto> CreateAsync(CreateInvestorProfileDto input)
        {
            try
            {
                // Validate email uniqueness
                if (!await _investorRepository.IsEmailUniqueAsync(input.Email))
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:DuplicateInvestorEmail", input.Email]);
                }

                var id = _guidGenerator.Create();
                var currentTime = _clock.Now;
                var currentUserId = _currentUser.Id;
                var currentTenantId = _currentTenant.Id;

                // Create investor profile
                var investor = InvestorProfile.Create(
                    id,
                    input.FirstName,
                    input.LastName,
                    input.Email,
                    input.Type,
                    input.CompanyName,
                    input.Title,
                    currentTenantId
                );

                // Update additional contact details if provided
                if (!input.Phone.IsNullOrWhiteSpace() || !input.LinkedIn.IsNullOrWhiteSpace() ||
                    !input.Website.IsNullOrWhiteSpace() || !input.Address.IsNullOrWhiteSpace())
                {
                    var contactDetails = new ContactDetails(
                        input.Email,
                        input.Phone,
                        input.LinkedIn,
                        input.Website,
                        input.Address
                    );
                    investor.UpdateContactDetails(contactDetails);
                }

                // Update communication preferences
                var communicationPreferences = new CommunicationPreferences(
                    input.PreferredChannel,
                    input.UpdateFrequency,
                    input.AllowMarketing,
                    input.CommunicationTags
                );
                investor.UpdateCommunicationPreferences(communicationPreferences);

                // Update notes if provided
                if (!input.Notes.IsNullOrWhiteSpace())
                {
                    investor.UpdateNotes(input.Notes);
                }

                await _investorRepository.InsertAsync(investor, autoSave: true);

                return await MapToExtendedDtoAsync(investor);
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

        [Authorize(IRMPermissions.Investors.Update)]
        public async Task<InvestorProfileDto> UpdateAsync(Guid id, UpdateInvestorProfileDto input)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(id);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                // Validate email uniqueness
                if (!await _investorRepository.IsEmailUniqueAsync(input.Email, id))
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:DuplicateInvestorEmail", input.Email]);
                }

                // Update basic information
                investor.UpdateName(input.FirstName, input.LastName);
                investor.UpdateCompanyInfo(input.CompanyName, input.Title);

                // Update contact details
                var contactDetails = new ContactDetails(
                    input.Email,
                    input.Phone,
                    input.LinkedIn,
                    input.Website,
                    input.Address
                );
                investor.UpdateContactDetails(contactDetails);

                // Update communication preferences
                var communicationPreferences = new CommunicationPreferences(
                    input.PreferredChannel,
                    input.UpdateFrequency,
                    input.AllowMarketing,
                    input.CommunicationTags
                );
                investor.UpdateCommunicationPreferences(communicationPreferences);

                // Update notes
                investor.UpdateNotes(input.Notes);

                await _investorRepository.UpdateAsync(investor, autoSave: true);

                return await MapToExtendedDtoAsync(investor);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(id);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                await _investorRepository.DeleteAsync(id, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.View)]
        public async Task<PagedResultDto<InvestorInteractionDto>> GetInteractionsAsync(Guid investorId, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var interactions = investor.Interactions
                    .OrderByDescending(i => i.InteractionDate)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var interactionDtos = ObjectMapper.Map<List<InvestorInteraction>, List<InvestorInteractionDto>>(interactions);

                return new PagedResultDto<InvestorInteractionDto>(
                    investor.Interactions.Count,
                    interactionDtos
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task<InvestorInteractionDto> AddInteractionAsync(Guid investorId, CreateInvestorInteractionDto input)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                investor.AddInteraction(
                    input.Type,
                    input.Subject,
                    input.InteractionDate,
                    input.Notes,
                    input.Outcome,
                    input.RelatedEntityId,
                    input.RelatedEntityType
                );

                await _investorRepository.UpdateAsync(investor, autoSave: true);

                // Return the newly added interaction
                var newInteraction = investor.Interactions
                    .OrderByDescending(i => i.CreationTime)
                    .First();

                return ObjectMapper.Map<InvestorInteraction, InvestorInteractionDto>(newInteraction);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task DeleteInteractionAsync(Guid investorId, Guid interactionId)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency and interaction existence
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                // Note: This would require additional logic in the domain model to remove interactions
                // For now, we'll throw an exception indicating the feature needs implementation
                throw new UserFriendlyException(L["IRM:ErrorMessage:InvalidOperation"]);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.View)]
        public async Task<PagedResultDto<InvestmentRecordDto>> GetInvestmentsAsync(Guid investorId, PagedAndSortedResultRequestDto input)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var investments = investor.Investments
                    .OrderByDescending(i => i.Commitment.InvestmentDate)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var investmentDtos = ObjectMapper.Map<List<InvestmentRecord>, List<InvestmentRecordDto>>(investments);

                return new PagedResultDto<InvestmentRecordDto>(
                    investor.Investments.Count,
                    investmentDtos
                );
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task<InvestmentRecordDto> AddInvestmentAsync(Guid investorId, CreateInvestmentRecordDto input)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var commitment = new CommitmentDetails(
                    input.InvestedAmount,
                    input.CommittedAmount,
                    input.OwnershipPercentage,
                    input.InvestmentRound,
                    input.InvestmentDate
                );

                investor.AddInvestment(commitment, input.DocumentReference);

                await _investorRepository.UpdateAsync(investor, autoSave: true);

                // Return the newly added investment
                var newInvestment = investor.Investments
                    .OrderByDescending(i => i.CreationTime)
                    .First();

                return ObjectMapper.Map<InvestmentRecord, InvestmentRecordDto>(newInvestment);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task ActivateInvestmentAsync(Guid investorId, Guid investmentId)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var investment = investor.Investments.FirstOrDefault(i => i.Id == investmentId);
                if (investment == null)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Investment"]);
                }

                investment.Activate();
                await _investorRepository.UpdateAsync(investor, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task DeactivateInvestmentAsync(Guid investorId, Guid investmentId)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(investorId);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                var investment = investor.Investments.FirstOrDefault(i => i.Id == investmentId);
                if (investment == null)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", "Investment"]);
                }

                investment.Deactivate();
                await _investorRepository.UpdateAsync(investor, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task UpdateRelationshipScoreAsync(Guid id, UpdateRelationshipScoreDto input)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(id);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                investor.UpdateRelationshipScore(input.Score, input.Notes);
                await _investorRepository.UpdateAsync(investor, autoSave: true);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.ManageRelationships)]
        public async Task<int> RecalculateRelationshipScoreAsync(Guid id)
        {
            try
            {
                var asOfDate = _clock.Now;
                var newScore = await _scoringService.RecalculateScoreAsync(id, asOfDate);

                var investor = await _investorRepository.GetAsync(id);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                investor.UpdateRelationshipScore(newScore.Score, newScore.Notes);
                await _investorRepository.UpdateAsync(investor, autoSave: true);

                return newScore.Score;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                if (ex is EntityNotFoundException)
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        [Authorize(IRMPermissions.Investors.Update)]
        public async Task UpdateStatusAsync(Guid id, InvestorStatus status)
        {
            try
            {
                var investor = await _investorRepository.GetAsync(id);

                // Check tenant consistency
                if (investor.TenantId != _currentTenant.Id)
                {
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                }

                investor.UpdateStatus(status);
                await _investorRepository.UpdateAsync(investor, autoSave: true);
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
                    throw new UserFriendlyException(L["IRM:ErrorMessage:EntityNotFound", L["IRM:Entity:InvestorProfile"]]);
                throw new UserFriendlyException(L["IRM:ErrorMessage:DefaultErrorMessage"]);
            }
        }

        public async Task<bool> IsEmailUniqueAsync(string email, Guid? excludeId = null)
        {
            return await _investorRepository.IsEmailUniqueAsync(email, excludeId);
        }

        // Helper Methods
        private async Task<InvestorProfileDto> MapToExtendedDtoAsync(InvestorProfile investor)
        {
            var dto = ObjectMapper.Map<InvestorProfile, InvestorProfileDto>(investor);

            // Calculate computed properties
            dto.TotalInvestedAmount = investor.GetTotalInvestedAmount();
            dto.TotalCommittedAmount = investor.GetTotalCommittedAmount();
            dto.TotalOwnershipPercentage = investor.GetTotalOwnershipPercentage();
            dto.ActiveInvestmentCount = investor.Investments.Count(i => i.IsActive);
            dto.InteractionCount = investor.Interactions.Count;
            dto.RecentInteractionCount = investor.GetInteractionCount(TimeSpan.FromDays(90));

            return dto;
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, "Error in InvestorProfileAppService: {ErrorMessage}", exception.Message);
        }
    }
}
