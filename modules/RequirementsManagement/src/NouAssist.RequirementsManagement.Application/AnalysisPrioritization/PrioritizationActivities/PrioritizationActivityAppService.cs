using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities.Dtos;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Aggregates;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Localization;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Permissions;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Repositories;
using NouAssist.RequirementsManagement.AnalysisPrioritization.Services;
using NouAssist.RequirementsManagement.AnalysisPrioritization.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.PrioritizationActivities
{
    [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Default)]
    public class PrioritizationActivityAppService : ApplicationService, IPrioritizationActivityAppService
    {
        private readonly IPrioritizationActivityRepository _prioritizationActivityRepository;
        private readonly IPrioritizationCalculationService _prioritizationCalculationService;

        public PrioritizationActivityAppService(
            IPrioritizationActivityRepository prioritizationActivityRepository,
            IPrioritizationCalculationService prioritizationCalculationService)
        {
            _prioritizationActivityRepository = prioritizationActivityRepository;
            _prioritizationCalculationService = prioritizationCalculationService;

            LocalizationResource = typeof(RequirementsAnalysisToolsResource);
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.View)]
        public virtual async Task<PrioritizationActivityDto> GetAsync(Guid id)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                var dto = ObjectMapper.Map<PrioritizationActivity, PrioritizationActivityDto>(activity);
                dto.VotingProgress = activity.GetVotingProgress();
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.View)]
        public virtual async Task<PagedResultDto<PrioritizationActivityDto>> GetListAsync(GetPrioritizationActivityListDto input)
        {
            try
            {
                var query = await _prioritizationActivityRepository.GetQueryableAsync();

                // Apply filters
                query = query.WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                    x => x.Name.Contains(input.Filter!) || x.Description.Contains(input.Filter!));

                if (input.Status.HasValue)
                {
                    query = query.Where(x => x.Status == input.Status.Value);
                }

                if (input.Method.HasValue)
                {
                    query = query.Where(x => x.Method == input.Method.Value);
                }

                if (input.ParticipantId.HasValue)
                {
                    query = query.Where(x => x.ParticipantIds.Contains(input.ParticipantId.Value));
                }

                if (input.RequirementId.HasValue)
                {
                    query = query.Where(x => x.RequirementIds.Contains(input.RequirementId.Value));
                }

                if (input.ActiveOnly.HasValue && input.ActiveOnly.Value)
                {
                    query = query.Where(x => x.Status == Enums.ActivityStatus.In_Progress || 
                                            x.Status == Enums.ActivityStatus.Voting);
                }

                if (input.CreatedAfter.HasValue)
                {
                    query = query.Where(x => x.CreationTime >= input.CreatedAfter.Value);
                }

                if (input.CreatedBefore.HasValue)
                {
                    query = query.Where(x => x.CreationTime <= input.CreatedBefore.Value);
                }

                var totalCount = await AsyncExecuter.CountAsync(query);

                var items = new List<PrioritizationActivity>();
                if (totalCount > 0)
                {
                    items = await AsyncExecuter.ToListAsync(
                        query.OrderBy(input.Sorting ?? nameof(PrioritizationActivity.CreationTime) + " DESC")
                            .PageBy(input)
                    );
                }

                var dtos = ObjectMapper.Map<List<PrioritizationActivity>, List<PrioritizationActivityDto>>(items);
                
                // Calculate voting progress for each activity
                foreach (var dto in dtos)
                {
                    var activity = items.First(x => x.Id == dto.Id);
                    dto.VotingProgress = activity.GetVotingProgress();
                }

                return new PagedResultDto<PrioritizationActivityDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Create)]
        public virtual async Task<PrioritizationActivityDto> CreateAsync(CreatePrioritizationActivityDto input)
        {
            try
            {
                var activity = PrioritizationActivity.Create(
                    GuidGenerator.Create(),
                    input.Name,
                    input.Description,
                    input.Method,
                    CurrentTenant.Id,
                    input.Instructions
                );

                activity = await _prioritizationActivityRepository.InsertAsync(activity);

                var dto = ObjectMapper.Map<PrioritizationActivity, PrioritizationActivityDto>(activity);
                dto.VotingProgress = activity.GetVotingProgress();
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Update)]
        public virtual async Task<PrioritizationActivityDto> UpdateAsync(Guid id, UpdatePrioritizationActivityDto input)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                
                // Update properties using reflection or direct property access
                typeof(PrioritizationActivity)
                    .GetProperty("Name", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(activity, input.Name);

                typeof(PrioritizationActivity)
                    .GetProperty("Description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(activity, input.Description);

                typeof(PrioritizationActivity)
                    .GetProperty("Instructions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.SetValue(activity, input.Instructions);

                await _prioritizationActivityRepository.UpdateAsync(activity);

                var dto = ObjectMapper.Map<PrioritizationActivity, PrioritizationActivityDto>(activity);
                dto.VotingProgress = activity.GetVotingProgress();
                return dto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            try
            {
                await _prioritizationActivityRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task StartAsync(Guid id)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                activity.Start(Clock.Now);
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task StartVotingAsync(Guid id)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                activity.StartVoting();
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task CompleteAsync(Guid id)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                activity.Complete(Clock.Now);
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task CancelAsync(Guid id)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                activity.Cancel();
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task AddParticipantsAsync(Guid id, ManageParticipantsDto input)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                activity.AddParticipants(input.ParticipantIds);
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task RemoveParticipantsAsync(Guid id, ManageParticipantsDto input)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                
                foreach (var participantId in input.ParticipantIds)
                {
                    activity.RemoveParticipant(participantId);
                }
                
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task AddRequirementsAsync(Guid id, ManageRequirementsDto input)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                activity.AddRequirements(input.RequirementIds);
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Manage)]
        public virtual async Task RemoveRequirementsAsync(Guid id, ManageRequirementsDto input)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                
                foreach (var requirementId in input.RequirementIds)
                {
                    activity.RemoveRequirement(requirementId);
                }
                
                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Vote)]
        public virtual async Task SubmitVoteAsync(Guid id, SubmitVoteDto input)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                var vote = ObjectMapper.Map<VoteValueDto, VoteValue>(input.Vote);

                activity.SubmitVote(
                    CurrentUser.GetId(),
                    input.RequirementId,
                    vote,
                    input.Comment,
                    Clock.Now
                );

                await _prioritizationActivityRepository.UpdateAsync(activity);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.ViewResults)]
        public virtual async Task<PrioritizationResultDto> GetResultsAsync(Guid id)
        {
            try
            {
                var activity = await _prioritizationActivityRepository.GetAsync(id);
                
                if (activity.Status != Enums.ActivityStatus.Completed)
                {
                    throw new BusinessException("RequirementsAnalysisTools:ErrorMessage:ActivityNotCompleted");
                }

                var results = await _prioritizationCalculationService.CalculatePrioritizationResultsAsync(id);
                var consensus = await _prioritizationCalculationService.CalculateConsensusAsync(id);

                var resultDto = new PrioritizationResultDto
                {
                    RequirementScores = results.ToDictionary(
                        kvp => kvp.Key,
                        kvp => ObjectMapper.Map<RankScore, RankScoreDto>(kvp.Value)
                    ),
                    OverallConsensus = consensus,
                    TotalVotes = activity.Votes.Count,
                    ParticipantCount = activity.ParticipantIds.Count
                };

                return resultDto;
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        [Authorize(RequirementsAnalysisToolsPermissions.PrioritizationActivities.Vote)]
        public virtual async Task<PagedResultDto<PrioritizationActivityDto>> GetMyActivitiesAsync(
            PagedAndSortedResultRequestDto input)
        {
            try
            {
                var currentUserId = CurrentUser.GetId();
                var activities = await _prioritizationActivityRepository.GetByParticipantAsync(currentUserId);

                var totalCount = activities.Count;
                var sortedActivities = activities
                    .AsQueryable()
                    .OrderBy(input.Sorting ?? nameof(PrioritizationActivity.CreationTime) + " DESC")
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToList();

                var dtos = ObjectMapper.Map<List<PrioritizationActivity>, List<PrioritizationActivityDto>>(sortedActivities);
                
                // Calculate voting progress for each activity
                foreach (var dto in dtos)
                {
                    var activity = sortedActivities.First(x => x.Id == dto.Id);
                    dto.VotingProgress = activity.GetVotingProgress();
                }

                return new PagedResultDto<PrioritizationActivityDto>(totalCount, dtos);
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
                throw;
            }
        }

        private async Task HandleErrorAsync(Exception exception, LogLevel logLevel = LogLevel.Error)
        {
            Logger.Log(logLevel, exception, 
                "Error in PrioritizationActivityAppService: {ErrorMessage}", 
                exception.Message);
        }
    }
}
