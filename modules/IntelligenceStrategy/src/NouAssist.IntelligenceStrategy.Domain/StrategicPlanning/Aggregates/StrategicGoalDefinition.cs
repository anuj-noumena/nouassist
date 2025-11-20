namespace NouAssist.IntelligenceStrategy.StrategicPlanning.Aggregates
{
    /// <summary>
    /// Strategic Goal Definition aggregate root.
    /// Manages high-level strategic goals with their targets and progress tracking.
    /// </summary>
    public class StrategicGoalDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public GoalDescription Description { get; private set; }
        public GoalStatus Status { get; private set; }
        public GoalPriority Priority { get; private set; }
        public PlanningHorizon Horizon { get; private set; }
        public DateTime TargetDate { get; private set; }
        public Guid? OwnerId { get; private set; }
        public string Notes { get; private set; }
        public List<string> Tags { get; private set; }

        private readonly List<GoalTarget> _targets;
        public IReadOnlyCollection<GoalTarget> Targets => _targets.AsReadOnly();

        protected StrategicGoalDefinition()
        {
            _targets = new List<GoalTarget>();
            Tags = new List<string>();
        }

        /// <summary>
        /// Factory method to create a new strategic goal
        /// </summary>
        public static StrategicGoalDefinition Create(
            Guid id,
            string title,
            string description,
            GoalPriority priority,
            PlanningHorizon horizon,
            DateTime targetDate,
            Guid? ownerId = null,
            Guid? tenantId = null)
        {
            return new StrategicGoalDefinition(id, title, description, priority, horizon, targetDate, ownerId, tenantId);
        }

        private StrategicGoalDefinition(
            Guid id,
            string title,
            string description,
            GoalPriority priority,
            PlanningHorizon horizon,
            DateTime targetDate,
            Guid? ownerId,
            Guid? tenantId) : base(id)
        {
            Description = new GoalDescription(title, description);
            Priority = priority;
            Horizon = horizon;
            SetTargetDate(targetDate);
            OwnerId = ownerId;
            TenantId = tenantId;
            Status = GoalStatus.Draft;
            _targets = new List<GoalTarget>();
            Tags = new List<string>();

            AddDistributedEvent(new StrategicGoalDefinedEto
            {
                Id = Id,
                Title = title,
                Description = description,
                Priority = priority,
                Horizon = horizon,
                TargetDate = targetDate,
                TenantId = tenantId,
                CreationTime = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Adds a measurable target to the strategic goal
        /// </summary>
        public void AddTarget(string metricName, decimal targetValue, string unit, DateTime targetDate, string notes = null)
        {
            if (Status == GoalStatus.Completed || Status == GoalStatus.Cancelled || Status == GoalStatus.Archived)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.Active);
            }

            var metric = new TargetMetric(metricName, targetValue, unit, targetDate);
            var target = new GoalTarget(Guid.NewGuid(), Id, metric, notes);
            
            _targets.Add(target);

            AddDistributedEvent(new GoalTargetSetEto
            {
                Id = target.Id,
                GoalId = Id,
                MetricName = metricName,
                TargetValue = targetValue,
                Unit = unit,
                TargetDate = targetDate,
                TenantId = TenantId,
                CreationTime = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Updates progress for a specific target
        /// </summary>
        public void UpdateTargetProgress(Guid targetId, decimal currentValue, DateTime measurementDate)
        {
            var target = _targets.FirstOrDefault(t => t.Id == targetId);
            if (target == null)
            {
                throw new EntityNotFoundException(typeof(GoalTarget), targetId);
            }

            target.UpdateProgress(currentValue, measurementDate);
        }

        /// <summary>
        /// Activates the strategic goal
        /// </summary>
        public void Activate(DateTime activationDate)
        {
            if (Status != GoalStatus.Approved)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.Active);
            }

            if (!_targets.Any())
            {
                throw new BusinessException("StrategicPlanning:ErrorMessage:CannotActivateGoalWithoutTargets");
            }

            Status = GoalStatus.Active;
        }

        /// <summary>
        /// Submits the goal for review
        /// </summary>
        public void SubmitForReview()
        {
            if (Status != GoalStatus.Draft)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.UnderReview);
            }

            Status = GoalStatus.UnderReview;
        }

        /// <summary>
        /// Approves the strategic goal
        /// </summary>
        public void Approve()
        {
            if (Status != GoalStatus.UnderReview)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.Approved);
            }

            Status = GoalStatus.Approved;
        }

        /// <summary>
        /// Marks the goal as completed
        /// </summary>
        public void Complete(DateTime completionDate)
        {
            if (Status != GoalStatus.Active)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.Completed);
            }

            Status = GoalStatus.Completed;
        }

        /// <summary>
        /// Puts the goal on hold
        /// </summary>
        public void PutOnHold(string reason)
        {
            if (Status != GoalStatus.Active)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.OnHold);
            }

            Status = GoalStatus.OnHold;
            UpdateNotes($"Put on hold: {reason}");
        }

        /// <summary>
        /// Resumes the goal from hold status
        /// </summary>
        public void Resume()
        {
            if (Status != GoalStatus.OnHold)
            {
                throw new InvalidGoalStatusTransitionException(Status, GoalStatus.Active);
            }

            Status = GoalStatus.Active;
        }

        /// <summary>
        /// Adds a tag to the goal
        /// </summary>
        public void AddTag(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Tag cannot be empty");

            if (Tags.Count >= StrategicPlanningConsts.MaxTagsPerEntity)
                throw new BusinessException("Maximum number of tags exceeded");

            var normalizedTag = tag.Trim().ToLowerInvariant();
            if (!Tags.Contains(normalizedTag))
            {
                Tags.Add(normalizedTag);
            }
        }

        /// <summary>
        /// Updates the goal's notes
        /// </summary>
        public void UpdateNotes(string notes)
        {
            Notes = Check.Length(notes, nameof(notes), StrategicPlanningConsts.MaxNotesLength);
        }

        /// <summary>
        /// Updates the goal priority
        /// </summary>
        public void UpdatePriority(GoalPriority priority)
        {
            Priority = priority;
        }

        /// <summary>
        /// Assigns ownership of the goal
        /// </summary>
        public void AssignOwner(Guid ownerId)
        {
            OwnerId = ownerId;
        }

        private void SetTargetDate(DateTime targetDate)
        {
            if (targetDate <= DateTime.UtcNow)
            {
                throw new BusinessException("StrategicPlanning:ErrorMessage:InvalidTargetDate");
            }
            TargetDate = targetDate;
        }

        /// <summary>
        /// Calculates overall progress percentage
        /// </summary>
        public decimal OverallProgress
        {
            get
            {
                if (!_targets.Any()) return 0;
                
                var achievedTargets = _targets.Count(t => t.IsAchieved);
                return (decimal)achievedTargets / _targets.Count * 100;
            }
        }

        /// <summary>
        /// Checks if the goal is on track based on target dates
        /// </summary>
        public bool IsOnTrack
        {
            get
            {
                if (!_targets.Any()) return false;
                
                var now = DateTime.UtcNow;
                var totalDays = (TargetDate - CreationTime).TotalDays;
                var elapsedDays = (now - CreationTime).TotalDays;
                var expectedProgress = Math.Max(0, elapsedDays / totalDays * 100);
                
                return OverallProgress >= (decimal)expectedProgress;
            }
        }
    }
}
