using Volo.Abp.Users;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using NouAssist.PersonalDevelopment.Habits.Habits.Dtos;
using NouAssist.PersonalDevelopment.Permissions;
using NouAssist.PersonalDevelopment.Habits.Planning.Dtos;
using NouAssist.PersonalDevelopment.Habits.Routines.Dtos;
using NouAssist.PersonalDevelopment.Habits.Services;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Habits.Planning
{
    [Authorize(PersonalDevelopmentPermissions.Habit.Tracking.ViewOwn)]
    public class PlanningAppService : ApplicationService, IPlanningAppService
    {
        private readonly IDailyReflectionRepository _reflectionRepository;
        private readonly IDailyIntentionRepository _intentionRepository;
        private readonly IDailyQuoteRepository _quoteRepository;
        private readonly DailyPlanningService _planningService;

        public PlanningAppService(
            IDailyReflectionRepository reflectionRepository,
            IDailyIntentionRepository intentionRepository,
            IDailyQuoteRepository quoteRepository,
            DailyPlanningService planningService)
        {
            _reflectionRepository = reflectionRepository;
            _intentionRepository = intentionRepository;
            _quoteRepository = quoteRepository;
            _planningService = planningService;
        }

        public async Task<DailyPlanDto> GetDailyPlanAsync(GetDailyPlanDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var plan = await _planningService.CreateDailyPlanAsync(ownerId, input.Date);

            var planDto = new DailyPlanDto
            {
                Date = plan.Date,
                Intention = ObjectMapper.Map<DailyIntention, DailyIntentionDto>(plan.Intention),
                Quote = plan.Quote != null ? ObjectMapper.Map<DailyQuote, DailyQuoteDto>(plan.Quote) : null,
                ScheduledHabits = ObjectMapper.Map<System.Collections.Generic.List<Habit>, System.Collections.Generic.List<HabitDto>>(plan.ScheduledHabits),
                ScheduledRoutines = ObjectMapper.Map<System.Collections.Generic.List<Routine>, System.Collections.Generic.List<RoutineDto>>(plan.ScheduledRoutines),
                HasIntention = plan.HasIntention,
                HasPreviousReflection = plan.HasPreviousReflection,
                EstimatedFocusMinutes = plan.EstimatedFocusMinutes,
                CreatedAt = plan.CreatedAt
            };

            return planDto;
        }

        public async Task<DailyReflectionDto> GetReflectionAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var reflection = await _reflectionRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (reflection == null)
            {
                return null;
            }

            return ObjectMapper.Map<DailyReflection, DailyReflectionDto>(reflection);
        }

        [Authorize(PersonalDevelopmentPermissions.Habit.Tracking.ManageOwn)]
        public async Task<DailyReflectionDto> UpdateReflectionAsync(UpdateDailyReflectionDto input)
        {
            var ownerId = CurrentUser.GetId();
            var reflection = await _reflectionRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (reflection == null)
            {
                reflection = new DailyReflection(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    CurrentTenant.Id
                );
            }

            if (input.Rating.HasValue)
            {
                reflection.SetRating(input.Rating.Value);
            }

            reflection.UpdateReflections(
                input.WentWell,
                input.ToImprove,
                input.Learned,
                input.Gratitude
            );

            if (input.TomorrowPriorities != null)
            {
                reflection.SetTomorrowPriorities(input.TomorrowPriorities);
            }

            if (reflection.Id == Guid.Empty)
            {
                await _reflectionRepository.InsertAsync(reflection);
            }
            else
            {
                await _reflectionRepository.UpdateAsync(reflection);
            }

            return ObjectMapper.Map<DailyReflection, DailyReflectionDto>(reflection);
        }

        public async Task<DailyIntentionDto> GetIntentionAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var intention = await _intentionRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (intention == null)
            {
                return null;
            }

            return ObjectMapper.Map<DailyIntention, DailyIntentionDto>(intention);
        }

        [Authorize(PersonalDevelopmentPermissions.Habit.Tracking.ManageOwn)]
        public async Task<DailyIntentionDto> UpdateIntentionAsync(UpdateDailyIntentionDto input)
        {
            var ownerId = CurrentUser.GetId();
            var intention = await _intentionRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (intention == null)
            {
                intention = new DailyIntention(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    CurrentTenant.Id
                );

                if (input.Intentions != null)
                {
                    foreach (var intentionText in input.Intentions)
                    {
                        intention.AddIntention(intentionText);
                    }
                }

                await _intentionRepository.InsertAsync(intention);
            }
            else
            {
                intention.ClearIntentions();
                if (input.Intentions != null)
                {
                    foreach (var intentionText in input.Intentions)
                    {
                        intention.AddIntention(intentionText);
                    }
                }

                await _intentionRepository.UpdateAsync(intention);
            }

            return ObjectMapper.Map<DailyIntention, DailyIntentionDto>(intention);
        }

        public async Task<DailyQuoteDto> GetQuoteAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var quote = await _quoteRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (quote == null)
            {
                return null;
            }

            return ObjectMapper.Map<DailyQuote, DailyQuoteDto>(quote);
        }

        [Authorize(PersonalDevelopmentPermissions.Habit.Tracking.ManageOwn)]
        public async Task<DailyQuoteDto> MarkQuoteAsViewedAsync(Guid id)
        {
            var quote = await _quoteRepository.GetAsync(id);
            
            if (quote.OwnerId != CurrentUser.GetId())
            {
                throw new Volo.Abp.BusinessException("Habits:NotOwner", "You don't have permission to mark this quote as viewed");
            }

            quote.MarkAsViewed();
            await _quoteRepository.UpdateAsync(quote);

            return ObjectMapper.Map<DailyQuote, DailyQuoteDto>(quote);
        }
    }
}
