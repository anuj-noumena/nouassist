using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using Volo.Abp.Domain.Services;

namespace NouAssist.PersonalDevelopment.Habits.Services
{
    public class RoutineSchedulingService : DomainService
    {
        private readonly IRoutineRepository _routineRepository;

        public RoutineSchedulingService(IRoutineRepository routineRepository)
        {
            _routineRepository = routineRepository;
        }

        public async Task<List<Routine>> GetScheduledRoutinesForDateAsync(
            Guid ownerId,
            DateTime date
        )
        {
            var activeRoutines = await _routineRepository.GetActiveRoutinesAsync(ownerId);
            
            return activeRoutines
                .Where(r => IsScheduledForDate(r, date))
                .OrderBy(r => r.TimeOfDay)
                .ToList();
        }

        public bool IsScheduledForDate(Routine routine, DateTime date)
        {
            if (!routine.HasSchedule)
            {
                return true; // No schedule means every day
            }

            var dayOfWeek = (int)date.DayOfWeek;
            // Convert Sunday (0) to 7 for consistency
            if (dayOfWeek == 0)
            {
                dayOfWeek = 7;
            }

            return routine.ScheduledDays.Contains(dayOfWeek);
        }

        public int CalculateEstimatedDuration(Routine routine)
        {
            return routine.Steps.Sum(s => s.EstimatedDurationMinutes ?? 5);
        }

        public RoutineScheduleAnalysis AnalyzeRoutineSchedule(List<Routine> routines)
        {
            var analysis = new RoutineScheduleAnalysis();

            analysis.TotalRoutines = routines.Count;
            analysis.ActiveRoutines = routines.Count(r => r.IsActive);
            analysis.RoutinesWithSchedule = routines.Count(r => r.HasSchedule);
            analysis.EverydayRoutines = routines.Count(r => r.IsEveryday);

            // Analyze by time of day
            analysis.MorningRoutines = routines.Count(r => 
                r.TimeOfDay?.ToLower().Contains("morning") == true);
            analysis.EveningRoutines = routines.Count(r => 
                r.TimeOfDay?.ToLower().Contains("evening") == true);

            // Calculate total estimated time
            analysis.TotalEstimatedMinutes = routines.Sum(r => CalculateEstimatedDuration(r));

            // Find busiest day
            var dayFrequency = new Dictionary<int, int>();
            for (int day = 1; day <= 7; day++)
            {
                dayFrequency[day] = routines.Count(r => 
                    !r.HasSchedule || r.ScheduledDays.Contains(day));
            }
            analysis.BusiestDay = dayFrequency.OrderByDescending(kvp => kvp.Value).First().Key;

            return analysis;
        }
    }

    public class RoutineScheduleAnalysis
    {
        public int TotalRoutines { get; set; }
        public int ActiveRoutines { get; set; }
        public int RoutinesWithSchedule { get; set; }
        public int EverydayRoutines { get; set; }
        public int MorningRoutines { get; set; }
        public int EveningRoutines { get; set; }
        public int TotalEstimatedMinutes { get; set; }
        public int BusiestDay { get; set; }
    }
}
