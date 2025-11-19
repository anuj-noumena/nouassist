using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using Volo.Abp.Domain.Services;

namespace NouAssist.PersonalDevelopment.Habits.Services
{
    /// <summary>
    /// Domain service for coordinating daily planning activities
    /// </summary>
    public class DailyPlanningService : DomainService
    {
        private readonly IDailyIntentionRepository _intentionRepository;
        private readonly IDailyQuoteRepository _quoteRepository;
        private readonly IDailyReflectionRepository _reflectionRepository;
        private readonly IHabitRepository _habitRepository;
        private readonly IRoutineRepository _routineRepository;

        public DailyPlanningService(
            IDailyIntentionRepository intentionRepository,
            IDailyQuoteRepository quoteRepository,
            IDailyReflectionRepository reflectionRepository,
            IHabitRepository habitRepository,
            IRoutineRepository routineRepository
        )
        {
            _intentionRepository = intentionRepository;
            _quoteRepository = quoteRepository;
            _reflectionRepository = reflectionRepository;
            _habitRepository = habitRepository;
            _routineRepository = routineRepository;
        }

        public async Task<DailyPlan> CreateDailyPlanAsync(Guid ownerId, DateTime date)
        {
            var plan = new DailyPlan
            {
                OwnerId = ownerId,
                Date = date.Date,
                CreatedAt = Clock.Now
            };

            // Get or create daily intention
            var intention = await _intentionRepository.GetByOwnerAndDateAsync(ownerId, date);
            if (intention == null)
            {
                intention = new DailyIntention(GuidGenerator.Create(), ownerId, date);
                plan.Intention = intention;
                plan.HasIntention = false;
            }
            else
            {
                plan.Intention = intention;
                plan.HasIntention = intention.HasIntentions;
            }

            // Get daily quote
            var quote = await _quoteRepository.GetByOwnerAndDateAsync(ownerId, date);
            if (quote == null)
            {
                // Could generate or fetch a random quote
                quote = await _quoteRepository.GetRandomQuoteAsync(ownerId);
            }
            plan.Quote = quote;

            // Get scheduled habits for the day
            var scheduledHabits = await _habitRepository.GetScheduledForDateAsync(ownerId, date);
            plan.ScheduledHabits = scheduledHabits;

            // Get scheduled routines for the day
            var activeRoutines = await _routineRepository.GetActiveRoutinesAsync(ownerId);
            plan.ScheduledRoutines = activeRoutines.Where(r => IsScheduledForDate(r, date)).ToList();

            // Check if previous day has reflection
            var previousReflection = await _reflectionRepository.GetByOwnerAndDateAsync(
                ownerId, 
                date.AddDays(-1)
            );
            plan.HasPreviousReflection = previousReflection != null && previousReflection.IsComplete;

            // Calculate daily focus time estimate
            plan.EstimatedFocusMinutes = CalculateEstimatedFocusTime(plan);

            return plan;
        }

        private bool IsScheduledForDate(Routine routine, DateTime date)
        {
            if (!routine.HasSchedule)
            {
                return true;
            }

            var dayOfWeek = (int)date.DayOfWeek;
            if (dayOfWeek == 0)
            {
                dayOfWeek = 7;
            }

            return routine.ScheduledDays.Contains(dayOfWeek);
        }

        private int CalculateEstimatedFocusTime(DailyPlan plan)
        {
            int total = 0;

            // Estimate from routines
            foreach (var routine in plan.ScheduledRoutines)
            {
                total += routine.Steps.Sum(s => s.EstimatedDurationMinutes ?? 5);
            }

            // Add buffer for habits (assume 10 min each)
            total += plan.ScheduledHabits.Count * 10;

            return total;
        }
    }

    public class DailyPlan
    {
        public Guid OwnerId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DailyIntention Intention { get; set; }
        public DailyQuote Quote { get; set; }
        public List<Habit> ScheduledHabits { get; set; } = new();
        public List<Routine> ScheduledRoutines { get; set; } = new();
        public bool HasIntention { get; set; }
        public bool HasPreviousReflection { get; set; }
        public int EstimatedFocusMinutes { get; set; }
    }
}
