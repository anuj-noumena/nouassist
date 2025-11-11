using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.PersonalDevelopment.Habits.Entities;
using NouAssist.PersonalDevelopment.Habits.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using DayOfWeek = NouAssist.PersonalDevelopment.Habits.Enums.DayOfWeek;

namespace NouAssist.PersonalDevelopment.Habits.Aggregates
{
    public class Habit : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Title { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid? AreaId { get; private set; }
        public Guid? ContextId { get; private set; }
        public string Description { get; set; }
        public string Goal { get; private set; }
        public HabitType HabitType { get; private set; }
        public bool IsActive { get; private set; }
        public Guid? TenantId { get; private set; }

        private readonly List<DayOfWeek> _scheduledDays = new();
        public IReadOnlyCollection<DayOfWeek> ScheduledDays => _scheduledDays.AsReadOnly();

        private readonly List<HabitCue> _cues = new();
        public IReadOnlyCollection<HabitCue> Cues => _cues.AsReadOnly();

        private readonly List<HabitResponse> _responses = new();
        public IReadOnlyCollection<HabitResponse> Responses => _responses.AsReadOnly();

        private readonly List<HabitReward> _rewards = new();
        public IReadOnlyCollection<HabitReward> Rewards => _rewards.AsReadOnly();

        private readonly List<HabitStrategy> _strategies = new();
        public IReadOnlyCollection<HabitStrategy> Strategies => _strategies.AsReadOnly();

        protected Habit() { /* ORM */ }

        public Habit(
            Guid id,
            Guid ownerId,
            string title,
            string goal,
            HabitType habitType,
            Guid? tenantId = null,
            Guid? areaId = null,
            Guid? contextId = null
        ) : base(id)
        {
            OwnerId = Check.NotNull(ownerId, nameof(ownerId));
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
            Goal = Check.NotNullOrWhiteSpace(goal, nameof(goal), maxLength: 500);
            HabitType = habitType;
            TenantId = tenantId;
            AreaId = areaId;
            ContextId = contextId;
            IsActive = true;
        }

        // Computed Properties
        public bool IsMakeHabit => HabitType == HabitType.Make;
        public bool IsBreakHabit => HabitType == HabitType.Break;
        public bool HasSchedule => _scheduledDays.Any();
        public bool IsEveryday => _scheduledDays.Count == 7;

        // Scheduling
        public void UpdateSchedule(List<DayOfWeek> newSchedule)
        {
            Check.NotNull(newSchedule, nameof(newSchedule));
            
            if (newSchedule.Count > 7)
            {
                throw new BusinessException("Habits:InvalidSchedule", 
                    "Cannot schedule more than 7 days");
            }

            _scheduledDays.Clear();
            _scheduledDays.AddRange(newSchedule.Distinct());
        }

        public bool IsScheduledForDay(DateTime date)
        {
            if (!HasSchedule) return true; // No schedule means everyday
            
            var dayOfWeek = (DayOfWeek)((int)date.DayOfWeek == 0 ? 7 : (int)date.DayOfWeek);
            return _scheduledDays.Contains(dayOfWeek);
        }

        public bool IsScheduledForToday() => IsScheduledForDay(DateTime.UtcNow);

        // State Management
        public void Activate()
        {
            if (IsActive) return;
            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            IsActive = false;
        }

        // Habit Loop Components
        public void AddCue(Guid cueId, CueType cueType, string description, string cueValue = null)
        {
            if (_cues.Any(c => c.Id == cueId))
            {
                throw new BusinessException("Habits:DuplicateCue", 
                    "A cue with this ID already exists");
            }

            var cue = new HabitCue(cueId, Id, cueType, description, cueValue);
            _cues.Add(cue);
        }

        public void RemoveCue(Guid cueId)
        {
            var cue = _cues.FirstOrDefault(c => c.Id == cueId);
            if (cue == null)
            {
                throw new BusinessException("Habits:CueNotFound", 
                    $"Cue with ID '{cueId}' not found");
            }
            _cues.Remove(cue);
        }

        public void AddResponse(Guid responseId, string description, string details = null, int? durationMinutes = null)
        {
            if (_responses.Any(r => r.Id == responseId))
            {
                throw new BusinessException("Habits:DuplicateResponse", 
                    "A response with this ID already exists");
            }

            var response = new HabitResponse(responseId, Id, description, details, durationMinutes);
            _responses.Add(response);
        }

        public void AddReward(Guid rewardId, string description, string craving = null)
        {
            if (_rewards.Any(r => r.Id == rewardId))
            {
                throw new BusinessException("Habits:DuplicateReward", 
                    "A reward with this ID already exists");
            }

            var reward = new HabitReward(rewardId, Id, description, craving);
            _rewards.Add(reward);
        }

        public void AddStrategy(Guid strategyId, HabitStrategyType strategyType, string description)
        {
            var strategy = new HabitStrategy(strategyId, Id, strategyType, description);
            _strategies.Add(strategy);
        }

        // Update Methods
        public void UpdateTitle(string newTitle)
        {
            Title = Check.NotNullOrWhiteSpace(newTitle, nameof(newTitle), maxLength: 200);
        }

        public void UpdateGoal(string newGoal)
        {
            Goal = Check.NotNullOrWhiteSpace(newGoal, nameof(newGoal), maxLength: 500);
        }

        public void AssignToArea(Guid? areaId)
        {
            AreaId = areaId;
        }

        public void AssignToContext(Guid? contextId)
        {
            ContextId = contextId;
        }
    }
}
