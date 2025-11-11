using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.PersonalDevelopment.Habits.Aggregates;
using NouAssist.PersonalDevelopment.Habits.Data;
using NouAssist.PersonalDevelopment.Habits.Permissions;
using NouAssist.PersonalDevelopment.Habits.Services;
using NouAssist.PersonalDevelopment.Habits.Tracking.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.PersonalDevelopment.Habits.Tracking
{
    [Authorize(HabitsPermissions.Tracking.ViewOwn)]
    public class TrackingAppService : ApplicationService, ITrackingAppService
    {
        private readonly IHealthTrackingRepository _healthRepository;
        private readonly IEnergyTrackingRepository _energyRepository;
        private readonly IMoodTrackingRepository _moodRepository;
        private readonly IFocusSessionRepository _focusRepository;
        private readonly WellnessAnalyticsService _wellnessService;

        public TrackingAppService(
            IHealthTrackingRepository healthRepository,
            IEnergyTrackingRepository energyRepository,
            IMoodTrackingRepository moodRepository,
            IFocusSessionRepository focusRepository,
            WellnessAnalyticsService wellnessService)
        {
            _healthRepository = healthRepository;
            _energyRepository = energyRepository;
            _moodRepository = moodRepository;
            _focusRepository = focusRepository;
            _wellnessService = wellnessService;
        }

        // Health Tracking
        public async Task<HealthTrackingDto> GetHealthTrackingAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _healthRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (entry == null)
            {
                return null;
            }

            return ObjectMapper.Map<HealthTrackingEntry, HealthTrackingDto>(entry);
        }

        [Authorize(HabitsPermissions.Tracking.ManageOwn)]
        public async Task<HealthTrackingDto> UpdateHealthTrackingAsync(UpdateHealthTrackingDto input)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _healthRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (entry == null)
            {
                entry = new HealthTrackingEntry(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    CurrentTenant.Id
                );

                if (input.SleepHours.HasValue) entry.LogSleep(input.SleepHours.Value);
                if (input.Notes != null) entry.Notes = input.Notes;

                await _healthRepository.InsertAsync(entry);
            }
            else
            {
                if (input.SleepHours.HasValue) entry.LogSleep(input.SleepHours.Value);
                if (input.Notes != null) entry.Notes = input.Notes;

                await _healthRepository.UpdateAsync(entry);
            }

            return ObjectMapper.Map<HealthTrackingEntry, HealthTrackingDto>(entry);
        }

        [Authorize(HabitsPermissions.Tracking.ManageOwn)]
        public async Task<HealthTrackingDto> LogHealthActionAsync(LogHealthActionDto input)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _healthRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (entry == null)
            {
                entry = new HealthTrackingEntry(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    CurrentTenant.Id
                );
            }

            switch (input.Action)
            {
                case HealthAction.LogSleep:
                    if (input.Value.HasValue)
                    {
                        entry.LogSleep(input.Value.Value);
                    }
                    break;
                case HealthAction.LogWater:
                    entry.LogWater();
                    break;
                case HealthAction.LogPostureCheck:
                    entry.LogPostureCheck();
                    break;
                case HealthAction.LogBreak:
                    entry.LogBreak();
                    break;
            }

            if (entry.Id == Guid.Empty)
            {
                await _healthRepository.InsertAsync(entry);
            }
            else
            {
                await _healthRepository.UpdateAsync(entry);
            }

            return ObjectMapper.Map<HealthTrackingEntry, HealthTrackingDto>(entry);
        }

        // Energy Tracking
        public async Task<EnergyTrackingDto> GetEnergyTrackingAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _energyRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (entry == null)
            {
                return null;
            }

            return ObjectMapper.Map<EnergyTrackingEntry, EnergyTrackingDto>(entry);
        }

        [Authorize(HabitsPermissions.Tracking.ManageOwn)]
        public async Task<EnergyTrackingDto> UpdateEnergyTrackingAsync(UpdateEnergyTrackingDto input)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _energyRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (entry == null)
            {
                entry = new EnergyTrackingEntry(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    input.EnergyLevel,
                    CurrentTenant.Id
                );

                if (input.Note != null) entry.AddNote(input.Note);

                await _energyRepository.InsertAsync(entry);
            }
            else
            {
                entry.SetEnergyLevel(input.EnergyLevel);
                if (input.Note != null) entry.AddNote(input.Note);

                await _energyRepository.UpdateAsync(entry);
            }

            return ObjectMapper.Map<EnergyTrackingEntry, EnergyTrackingDto>(entry);
        }

        // Mood Tracking
        public async Task<MoodTrackingDto> GetMoodTrackingAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _moodRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (entry == null)
            {
                return null;
            }

            return ObjectMapper.Map<MoodTrackingEntry, MoodTrackingDto>(entry);
        }

        [Authorize(HabitsPermissions.Tracking.ManageOwn)]
        public async Task<MoodTrackingDto> UpdateMoodTrackingAsync(UpdateMoodTrackingDto input)
        {
            var ownerId = CurrentUser.GetId();
            var entry = await _moodRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (entry == null)
            {
                entry = new MoodTrackingEntry(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    input.MoodRating,
                    CurrentTenant.Id
                );

                if (input.Note != null) entry.AddNote(input.Note);
                if (input.Tags != null)
                {
                    foreach (var tag in input.Tags)
                    {
                        entry.AddTag(tag);
                    }
                }

                await _moodRepository.InsertAsync(entry);
            }
            else
            {
                entry.SetMoodRating(input.MoodRating);
                if (input.Note != null) entry.AddNote(input.Note);

                await _moodRepository.UpdateAsync(entry);
            }

            return ObjectMapper.Map<MoodTrackingEntry, MoodTrackingDto>(entry);
        }

        // Focus Session
        public async Task<FocusSessionDto> GetFocusSessionAsync(DateTime date)
        {
            var ownerId = CurrentUser.GetId();
            var session = await _focusRepository.GetByOwnerAndDateAsync(ownerId, date);
            
            if (session == null)
            {
                return null;
            }

            return ObjectMapper.Map<FocusSession, FocusSessionDto>(session);
        }

        [Authorize(HabitsPermissions.Tracking.ManageOwn)]
        public async Task<FocusSessionDto> CompletePomodoroAsync(CompletePomodoroDto input)
        {
            var ownerId = CurrentUser.GetId();
            var session = await _focusRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (session == null)
            {
                session = new FocusSession(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    null,
                    CurrentTenant.Id
                );
            }

            session.CompletePomodoro(input.DurationMinutes);

            if (session.Id == Guid.Empty)
            {
                await _focusRepository.InsertAsync(session);
            }
            else
            {
                await _focusRepository.UpdateAsync(session);
            }

            return ObjectMapper.Map<FocusSession, FocusSessionDto>(session);
        }

        [Authorize(HabitsPermissions.Tracking.ManageOwn)]
        public async Task<FocusSessionDto> UpdateFocusSessionAsync(UpdateFocusSessionDto input)
        {
            var ownerId = CurrentUser.GetId();
            var session = await _focusRepository.GetByOwnerAndDateAsync(ownerId, input.Date);

            if (session == null)
            {
                session = new FocusSession(
                    GuidGenerator.Create(),
                    ownerId,
                    input.Date,
                    input.TaskId,
                    CurrentTenant.Id
                );

                if (input.Notes != null) session.Notes = input.Notes;

                await _focusRepository.InsertAsync(session);
            }
            else
            {
                if (input.TaskId.HasValue) session.LinkTask(input.TaskId.Value);
                if (input.Notes != null) session.Notes = input.Notes;

                await _focusRepository.UpdateAsync(session);
            }

            return ObjectMapper.Map<FocusSession, FocusSessionDto>(session);
        }

        // Wellness Report
        [Authorize(HabitsPermissions.Analytics.ViewOwn)]
        public async Task<WellnessReportDto> GetWellnessReportAsync(GetWellnessReportDto input)
        {
            var ownerId = CurrentUser.GetId();
            
            var report = await _wellnessService.GenerateWellnessReportAsync(
                ownerId,
                input.StartDate,
                input.EndDate
            );

            return ObjectMapper.Map<WellnessReport, WellnessReportDto>(report);
        }
    }
}
