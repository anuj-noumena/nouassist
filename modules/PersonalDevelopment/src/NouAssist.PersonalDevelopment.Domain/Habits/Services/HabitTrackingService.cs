using System;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Data;
using NouAssist.PersonalDevelopment.Habits.Entities;
using NouAssist.PersonalDevelopment.Habits.Events;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace NouAssist.PersonalDevelopment.Habits.Services
{
    public class HabitTrackingService : DomainService
    {
        private readonly IHabitRepository _habitRepository;
        private readonly IHabitTrackingRepository _trackingRepository;
        private readonly HabitAnalyticsService _analyticsService;

        public HabitTrackingService(
            IHabitRepository habitRepository,
            IHabitTrackingRepository trackingRepository,
            HabitAnalyticsService analyticsService
        )
        {
            _habitRepository = habitRepository;
            _trackingRepository = trackingRepository;
            _analyticsService = analyticsService;
        }

        public async Task<HabitTrackingEntry> CompleteHabitAsync(
            Guid habitId,
            DateTime logDate,
            Guid ownerId,
            string notes = null
        )
        {
            var habit = await _habitRepository.GetAsync(habitId);

            if (!habit.IsActive)
            {
                throw new BusinessException("Habits:InactiveHabit", 
                    "Cannot log tracking for inactive habit");
            }

            if (!habit.IsScheduledForDay(logDate))
            {
                throw new BusinessException("Habits:NotScheduled", 
                    "Habit is not scheduled for this day");
            }

            var existingEntry = await _trackingRepository.GetByHabitAndDateAsync(habitId, logDate);

            if (existingEntry != null)
            {
                if (existingEntry.Completed)
                {
                    return existingEntry; // Already completed
                }

                existingEntry.MarkAsCompleted();
                if (!string.IsNullOrWhiteSpace(notes))
                {
                    existingEntry.AddNotes(notes);
                }

                await UpdateStreakInEvent(existingEntry, habitId);
                return await _trackingRepository.UpdateAsync(existingEntry);
            }

            var newEntry = new HabitTrackingEntry(
                GuidGenerator.Create(),
                ownerId,
                habitId,
                logDate,
                completed: true,
                habit.TenantId
            );

            if (!string.IsNullOrWhiteSpace(notes))
            {
                newEntry.AddNotes(notes);
            }

            newEntry.MarkAsCompleted();
            await UpdateStreakInEvent(newEntry, habitId);

            return await _trackingRepository.InsertAsync(newEntry);
        }

        private async Task UpdateStreakInEvent(HabitTrackingEntry entry, Guid habitId)
        {
            var history = await _trackingRepository.GetHistoryAsync(habitId);
            var completedHistory = history.Where(h => h.Completed).ToList();
            var currentStreak = _analyticsService.CalculateCurrentStreak(completedHistory);

            // Update the domain event with the calculated streak
            var domainEvents = entry.GetDistributedEvents();
            foreach (var evt in domainEvents)
            {
                if (evt.EventData is HabitCompletedEto eto)
                {
                    eto.CurrentStreak = currentStreak;
                }
            }
        }
    }
}
