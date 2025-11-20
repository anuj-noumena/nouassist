using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore; // Required for EF.Property
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Entities;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Enums;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Exceptions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Permissions;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect.Dtos;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Repositories;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Timing;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Prospect
{
    [Authorize(FundraisingPipelinePermissions.Prospect.Default)]
    public class ProspectAppService : ApplicationService, IProspectAppService
    {
        private readonly IProspectRepository _prospectRepository;
        private readonly IFundraisingRoundRepository _fundraisingRoundRepository;
        private readonly IProspectScoringService _prospectScoringService;
        private readonly IOpportunityForecaster _opportunityForecaster;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IClock _clock;

        public ProspectAppService(
            IProspectRepository prospectRepository,
            IFundraisingRoundRepository fundraisingRoundRepository,
            IProspectScoringService prospectScoringService,
            IOpportunityForecaster opportunityForecaster,
            IGuidGenerator guidGenerator,
            IClock clock)
        {
            _prospectRepository = prospectRepository;
            _fundraisingRoundRepository = fundraisingRoundRepository;
            _prospectScoringService = prospectScoringService;
            _opportunityForecaster = opportunityForecaster;
            _guidGenerator = guidGenerator;
            _clock = clock;
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.View)]
        public async Task<ProspectDto> GetAsync(Guid id)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }
                
                var dto = ObjectMapper.Map<Aggregates.Prospect, ProspectDto>(prospect);
                
                if (prospect.FundraisingRoundId.HasValue)
                {
                    try
                    {
                        var round = await _fundraisingRoundRepository.GetAsync(prospect.FundraisingRoundId.Value);
                        dto.FundraisingRoundName = round?.Name;
                    }
                    catch (EntityNotFoundException)
                    {
                        Logger.LogWarning("Fundraising round with ID {RoundId} not found for prospect {ProspectId}", prospect.FundraisingRoundId.Value, id);
                        dto.FundraisingRoundName = null; // Or some placeholder like "Round Not Found"
                    }
                }
                
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving prospect with ID {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.View)]
        public async Task<PagedResultDto<ProspectDto>> GetListAsync(GetProspectListDto input)
        {
            try
            {
                var query = await _prospectRepository.WithDetailsAsync();
                
                if (!string.IsNullOrWhiteSpace(input.Filter))
                {
                    var filterLower = input.Filter.ToLower();
                    query = query.Where(p => 
                        p.Name.ToLower().Contains(filterLower) || 
                        p.Company.ToLower().Contains(filterLower) ||
                        p.Title.ToLower().Contains(filterLower) ||
                        (p.Description != null && p.Description.ToLower().Contains(filterLower)) ||
                        (p.ContactInfo.Email != null && p.ContactInfo.Email.ToLower().Contains(filterLower))
                        );
                }
                
                if (input.Status.HasValue)
                {
                    // EF.Property is used to access properties of owned types in a query
                    query = query.Where(p => EF.Property<ProspectStatus>(p.PipelineStatus, "Status") == input.Status.Value);
                }
                
                if (input.FundraisingRoundId.HasValue)
                {
                    query = query.Where(p => p.FundraisingRoundId == input.FundraisingRoundId);
                }
                
                if (input.HighValueOnly == true)
                {
                    // Assuming a default minimum value or one from configuration
                    var highValueSpec = new Specifications.HighValueProspectSpecification(); 
                    query = query.Where(highValueSpec.ToExpression());
                }
                
                if (input.NeedsFollowUpOnly == true)
                {
                     // Assuming a default max days or one from configuration
                    var needsFollowUpSpec = new Specifications.NeedsFollowUpProspectSpecification();
                    query = query.Where(needsFollowUpSpec.ToExpression());
                }
                
                query = ApplySorting(query, input);
                
                var totalCount = await AsyncExecuter.CountAsync(query);
                
                query = query.Skip(input.SkipCount).Take(input.MaxResultCount);
                
                var prospects = await AsyncExecuter.ToListAsync(query);
                var dtos = ObjectMapper.Map<List<Aggregates.Prospect>, List<ProspectDto>>(prospects);
                
                await PopulateFundraisingRoundNames(dtos);
                
                return new PagedResultDto<ProspectDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving prospect list");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Create)]
        public async Task<ProspectDto> CreateAsync(CreateProspectDto input)
        {
            try
            {
                if (input.FundraisingRoundId.HasValue)
                {
                    var roundExists = await _fundraisingRoundRepository.FindAsync(input.FundraisingRoundId.Value);
                    if (roundExists == null)
                    {
                        throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", input.FundraisingRoundId.Value]);
                    }
                }
                
                var contactInfo = ObjectMapper.Map<CreateContactInfoDto, ContactInfo>(input.ContactInfo);
                
                var prospect = Aggregates.Prospect.Create(
                    _guidGenerator.Create(),
                    input.Name,
                    input.Company,
                    input.Title,
                    contactInfo,
                    input.Description,
                    CurrentTenant.Id);
                
                if (input.InitialStatus.HasValue && input.InitialStatus.Value != ProspectStatus.New)
                {
                    prospect.UpdateStatus(input.InitialStatus.Value, _clock.Now);
                }
                
                if (input.FundraisingRoundId.HasValue)
                {
                    prospect.AssignToRound(input.FundraisingRoundId.Value);
                }
                
                await _prospectRepository.InsertAsync(prospect, autoSave: true);

                // Initial score and forecast calculation
                await _prospectScoringService.UpdateAllScoresAsync(prospect.Id);
                // Assuming a default base amount for initial forecast or requiring it
                await _opportunityForecaster.UpdateForecastAsync(prospect.Id, 0m); // Use 0m or a configured default

                var createdProspect = await _prospectRepository.GetWithDetailsAsync(prospect.Id);
                var dto = ObjectMapper.Map<Aggregates.Prospect, ProspectDto>(createdProspect);
                await PopulateFundraisingRoundNames(new List<ProspectDto> { dto });
                
                return dto;
            }
            catch (UserFriendlyException) // Re-throw known user-friendly exceptions
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error creating prospect");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)]
        public async Task<ProspectDto> UpdateAsync(Guid id, UpdateProspectDto input)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }

                if (input.FundraisingRoundId.HasValue)
                {
                     var roundExists = await _fundraisingRoundRepository.FindAsync(input.FundraisingRoundId.Value);
                    if (roundExists == null)
                    {
                        throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:FundraisingRoundNotFound", input.FundraisingRoundId.Value]);
                    }
                }

                var contactInfo = ObjectMapper.Map<UpdateContactInfoDto, ContactInfo>(input.ContactInfo);
                prospect.UpdateBasicInfo(input.Name, input.Company, input.Title, input.Description, contactInfo);

                if (input.FundraisingRoundId.HasValue)
                {
                    if (prospect.FundraisingRoundId != input.FundraisingRoundId)
                    {
                        prospect.AssignToRound(input.FundraisingRoundId.Value);
                    }
                }
                else
                {
                    if (prospect.FundraisingRoundId.HasValue)
                    {
                        prospect.RemoveFromRound();
                    }
                }
                
                await _prospectRepository.UpdateAsync(prospect, autoSave: true);
                
                var updatedProspect = await _prospectRepository.GetWithDetailsAsync(id);
                var dto = ObjectMapper.Map<Aggregates.Prospect, ProspectDto>(updatedProspect);
                await PopulateFundraisingRoundNames(new List<ProspectDto> { dto });

                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating prospect with ID {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                var prospect = await _prospectRepository.FindAsync(id);
                if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }
                await _prospectRepository.DeleteAsync(id, autoSave: true);
            }
            catch (EntityNotFoundException)
            {
                // If already not found, it's fine, or you can choose to throw.
                // For idempotency, not throwing an error can be acceptable.
                Logger.LogWarning("Attempted to delete a prospect that was not found: {ProspectId}", id);
                return;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting prospect with ID {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)]
        public async Task<ProspectDto> UpdateStatusAsync(Guid id, UpdateProspectStatusDto input)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                 if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }

                prospect.UpdateStatus(input.Status, _clock.Now);
                await _prospectRepository.UpdateAsync(prospect, autoSave: true);

                await _prospectScoringService.UpdateAllScoresAsync(id);
                await _opportunityForecaster.UpdateForecastAsync(id, prospect.OpportunityForecast?.ExpectedAmount ?? 0m);

                var updatedProspect = await _prospectRepository.GetWithDetailsAsync(id);
                var dto = ObjectMapper.Map<Aggregates.Prospect, ProspectDto>(updatedProspect);
                await PopulateFundraisingRoundNames(new List<ProspectDto> { dto });
                
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (InvalidProspectStatusTransitionException ex)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:InvalidProspectStatusTransition", ex.Data["currentStatus"], ex.Data["newStatus"]]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating status for prospect {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)]
        public async Task<InteractionLogDto> AddInteractionAsync(Guid id, CreateInteractionDto input)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                 if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }
                
                var interaction = prospect.AddInteraction(input.Type, input.Summary, input.InteractionDate, input.Notes);
                await _prospectRepository.UpdateAsync(prospect, autoSave: true);
                
                await _prospectScoringService.UpdateAllScoresAsync(id); // Recalculate scores after new interaction
                
                return ObjectMapper.Map<InteractionLog, InteractionLogDto>(interaction);
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error adding interaction for prospect {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)]
        public async Task<InteractionLogDto> UpdateInteractionAsync(Guid id, Guid interactionId, UpdateInteractionDto input)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                 if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }

                var interaction = prospect.Interactions.FirstOrDefault(i => i.Id == interactionId);
                if (interaction == null)
                {
                    throw new EntityNotFoundException(typeof(InteractionLog), interactionId);
                }
                
                interaction.UpdateDetails(input.Type, input.Summary, input.InteractionDate, input.Notes);
                await _prospectRepository.UpdateAsync(prospect, autoSave: true);

                await _prospectScoringService.UpdateAllScoresAsync(id);
                
                return ObjectMapper.Map<InteractionLog, InteractionLogDto>(interaction);
            }
            catch (EntityNotFoundException ex)
            {
                if (ex.EntityType == typeof(Aggregates.Prospect))
                    throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
                else
                    throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:InteractionNotFound", interactionId]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating interaction {InteractionId} for prospect {ProspectId}", interactionId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)] // Or Delete permission if more granular
        public async Task DeleteInteractionAsync(Guid id, Guid interactionId)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                 if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }

                var interaction = prospect.Interactions.FirstOrDefault(i => i.Id == interactionId);
                if (interaction == null)
                {
                    // Idempotency: if not found, it's already "deleted"
                    Logger.LogWarning("Interaction {InteractionId} not found for prospect {ProspectId} during delete.", interactionId, id);
                    return;
                }
                
                // The ORM (EF Core) should handle removal from the collection.
                // We need to ensure the Prospect aggregate's _interactions list is mutable or provides a method.
                // For this example, assuming direct removal from the list is handled by the ORM.
                // If not, the Prospect aggregate would need a RemoveInteraction(Guid interactionId) method.
                var interactionsList = prospect.Interactions as List<InteractionLog>;
                if (interactionsList != null)
                {
                    interactionsList.Remove(interaction);
                }
                else
                {
                    // This case should ideally not happen if _interactions is initialized as List<T>
                    // and exposed as IReadOnlyCollection<T>
                    Logger.LogError("Could not remove interaction {InteractionId} as the underlying collection was not a List.", interactionId);
                    throw new UserFriendlyException("Could not remove interaction due to internal collection type.");
                }

                await _prospectRepository.UpdateAsync(prospect, autoSave: true);
                await _prospectScoringService.UpdateAllScoresAsync(id);
            }
            catch (EntityNotFoundException)
            {
                 throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error deleting interaction {InteractionId} for prospect {ProspectId}", interactionId, id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.View)]
        public async Task<List<InteractionLogDto>> GetInteractionsAsync(Guid id)
        {
            try
            {
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }
                return ObjectMapper.Map<List<InteractionLog>, List<InteractionLogDto>>(prospect.Interactions.ToList());
            }
            catch (EntityNotFoundException)
            {
                 throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving interactions for prospect {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)]
        public async Task<ProspectDto> UpdateScoresAsync(Guid id)
        {
            try
            {
                await _prospectScoringService.UpdateAllScoresAsync(id);
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }
                var dto = ObjectMapper.Map<Aggregates.Prospect, ProspectDto>(prospect);
                await PopulateFundraisingRoundNames(new List<ProspectDto> { dto });
                return dto;
            }
            catch (EntityNotFoundException)
            {
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (ProspectNotFoundException) // Domain service might throw this
            {
                 throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating scores for prospect {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.Update)]
        public async Task<ProspectDto> UpdateForecastAsync(Guid id, decimal baseAmount)
        {
            try
            {
                await _opportunityForecaster.UpdateForecastAsync(id, baseAmount);
                var prospect = await _prospectRepository.GetWithDetailsAsync(id);
                 if (prospect == null)
                {
                    throw new EntityNotFoundException(typeof(Aggregates.Prospect), id);
                }
                var dto = ObjectMapper.Map<Aggregates.Prospect, ProspectDto>(prospect);
                await PopulateFundraisingRoundNames(new List<ProspectDto> { dto });
                return dto;
            }
            catch (EntityNotFoundException)
            {
                 throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
             catch (ProspectNotFoundException) // Domain service might throw this
            {
                 throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:ProspectNotFound", id]);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error updating forecast for prospect {ProspectId}", id);
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.View)]
        public async Task<List<ProspectDto>> GetHighValueProspectsAsync()
        {
            try
            {
                // Using a default value, consider making this configurable
                var prospects = await _prospectRepository.GetHighValueProspectsAsync(); 
                var dtos = ObjectMapper.Map<List<Aggregates.Prospect>, List<ProspectDto>>(prospects);
                await PopulateFundraisingRoundNames(dtos);
                return dtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving high value prospects");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        [Authorize(FundraisingPipelinePermissions.Prospect.View)]
        public async Task<List<ProspectDto>> GetProspectsNeedingFollowUpAsync()
        {
            try
            {
                // Using a default value, consider making this configurable
                var prospects = await _prospectRepository.GetProspectsNeedingFollowUpAsync(); 
                var dtos = ObjectMapper.Map<List<Aggregates.Prospect>, List<ProspectDto>>(prospects);
                await PopulateFundraisingRoundNames(dtos);
                return dtos;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error retrieving prospects needing follow-up");
                throw new UserFriendlyException(L["FundraisingPipeline:ErrorMessage:GenericError"]);
            }
        }

        // Helper methods
        private IQueryable<Aggregates.Prospect> ApplySorting(IQueryable<Aggregates.Prospect> query, PagedAndSortedResultRequestDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Default sorting
                return query.OrderByDescending(p => p.CreationTime);
            }
            
            // Custom sorting logic based on DTO properties
            // This needs careful mapping from DTO property names to Domain entity property names
            // Example: if input.Sorting is "name asc", "status desc"
            // For simplicity, we'll use DynamicLinq here. Be cautious with exposing raw property names.
            // A safer approach would be a switch statement or dictionary mapping.
            try
            {
                 // Map DTO sort fields to entity sort fields
                var sortField = input.Sorting.Split(' ')[0].ToLowerInvariant();
                var sortDirection = input.Sorting.Split(' ').Length > 1 ? input.Sorting.Split(' ')[1] : "asc";

                string entitySortField = sortField switch
                {
                    "name" => "Name",
                    "company" => "Company",
                    "status" => "PipelineStatus.Status", // This might be tricky with owned types and DynamicLinq
                    "overallscore" => "Score.OverallScore", // Also tricky
                    "creationtime" => "CreationTime",
                    _ => "CreationTime" // Default
                };
                
                // Special handling for owned type properties if DynamicLinq struggles
                if (sortField == "status")
                {
                    query = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy(p => EF.Property<ProspectStatus>(p.PipelineStatus, "Status"))
                        : query.OrderByDescending(p => EF.Property<ProspectStatus>(p.PipelineStatus, "Status"));
                }
                else if (sortField == "overallscore")
                {
                     query = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase)
                        ? query.OrderBy(p => EF.Property<int>(p.Score, "OverallScore"))
                        : query.OrderByDescending(p => EF.Property<int>(p.Score, "OverallScore"));
                }
                else
                {
                    query = query.OrderBy($"{entitySortField} {sortDirection}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Could not apply sorting '{Sorting}', using default.", input.Sorting);
                query = query.OrderByDescending(p => p.CreationTime);
            }
            return query;
        }

        private async Task PopulateFundraisingRoundNames(List<ProspectDto> dtos)
        {
            var roundIds = dtos
                .Where(d => d.FundraisingRoundId.HasValue)
                .Select(d => d.FundraisingRoundId.Value)
                .Distinct()
                .ToList();

            if (!roundIds.Any()) return;

            var rounds = await _fundraisingRoundRepository.GetListAsync(r => roundIds.Contains(r.Id));
            var roundNameMap = rounds.ToDictionary(r => r.Id, r => r.Name);

            foreach (var dto in dtos)
            {
                if (dto.FundraisingRoundId.HasValue && roundNameMap.TryGetValue(dto.FundraisingRoundId.Value, out var roundName))
                {
                    dto.FundraisingRoundName = roundName;
                }
            }
        }
    }
}
