namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates
{
    /// <summary>
    /// Strategic Pivot aggregate root
    /// Manages strategic pivots and their impact measurement
    /// </summary>
    public class StrategicPivot : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public PivotType Type { get; private set; }
        public PivotStatus Status { get; private set; }
        public PivotRationale Rationale { get; private set; }
        public string Description { get; private set; }
        public Guid InitiatedBy { get; private set; }
        public DateTime InitiatedDate { get; private set; }
        public DateTime TargetCompletionDate { get; private set; }
        public DateTime? ActualCompletionDate { get; private set; }
        public string ActualOutcome { get; private set; }
        public bool WasSuccessful { get; private set; }

        private readonly List<ImpactMetricDefinition> _impactMetrics;
        public IReadOnlyCollection<ImpactMetricDefinition> ImpactMetrics => _impactMetrics.AsReadOnly();

        protected StrategicPivot()
        {
            _impactMetrics = new List<ImpactMetricDefinition>();
        }

        public static StrategicPivot Create(
            Guid id,
            PivotType type,
            PivotRationale rationale,
            string description,
            Guid initiatedBy,
            DateTime initiatedDate,
            DateTime targetCompletionDate,
            Guid? tenantId)
        {
            return new StrategicPivot(id, type, rationale, description, initiatedBy, initiatedDate, targetCompletionDate, tenantId);
        }

        private StrategicPivot(
            Guid id,
            PivotType type,
            PivotRationale rationale,
            string description,
            Guid initiatedBy,
            DateTime initiatedDate,
            DateTime targetCompletionDate,
            Guid? tenantId) : base(id)
        {
            Type = type;
            Rationale = rationale ?? throw new ArgumentNullException(nameof(rationale));
            SetDescription(description);
            InitiatedBy = initiatedBy;
            InitiatedDate = initiatedDate;
            SetTargetCompletionDate(targetCompletionDate);
            TenantId = tenantId;
            Status = PivotStatus.Planned;
            WasSuccessful = false;
            _impactMetrics = new List<ImpactMetricDefinition>();

            AddDistributedEvent(new StrategicPivotInitiatedEto
            {
                Id = Id,
                Type = Type,
                Reason = Rationale.Reason,
                Description = Description,
                InitiatedBy = InitiatedBy,
                TargetCompletionDate = TargetCompletionDate,
                TenantId = TenantId
            });
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: BusinessModelManagementConsts.StrategicPivot.MaxDescriptionLength
            );
        }

        private void SetTargetCompletionDate(DateTime targetCompletionDate)
        {
            if (targetCompletionDate <= InitiatedDate)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidValue")
                    .WithData("propertyName", "TargetCompletionDate");
            }

            TargetCompletionDate = targetCompletionDate;
        }

        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        public void UpdateTargetCompletionDate(DateTime targetCompletionDate)
        {
            if (Status == PivotStatus.Completed)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:InvalidOperation");
            }

            SetTargetCompletionDate(targetCompletionDate);
        }

        public void StartImplementation()
        {
            ValidateStatusTransition(PivotStatus.InProgress);
            Status = PivotStatus.InProgress;
        }

        public void Complete(string actualOutcome, bool wasSuccessful, DateTime completionDate)
        {
            ValidateStatusTransition(PivotStatus.Completed);

            ActualOutcome = Check.NotNullOrWhiteSpace(
                actualOutcome,
                nameof(actualOutcome),
                maxLength: BusinessModelManagementConsts.StrategicPivot.MaxActualOutcomeLength
            );

            Status = PivotStatus.Completed;
            ActualOutcome = actualOutcome;
            WasSuccessful = wasSuccessful;
            ActualCompletionDate = completionDate;

            AddDistributedEvent(new StrategicPivotCompletedEto
            {
                Id = Id,
                Type = Type,
                Status = Status,
                ActualOutcome = ActualOutcome,
                CompletedDate = completionDate,
                WasSuccessful = WasSuccessful,
                TenantId = TenantId
            });
        }

        public void PutOnHold()
        {
            ValidateStatusTransition(PivotStatus.OnHold);
            Status = PivotStatus.OnHold;
        }

        public void Resume()
        {
            if (Status != PivotStatus.OnHold)
            {
                throw new InvalidPivotTransitionException(Status, PivotStatus.InProgress);
            }

            Status = PivotStatus.InProgress;
        }

        public void Abandon()
        {
            if (Status == PivotStatus.Completed)
            {
                throw new InvalidPivotTransitionException(Status, PivotStatus.Abandoned);
            }

            Status = PivotStatus.Abandoned;
        }

        private void ValidateStatusTransition(PivotStatus targetStatus)
        {
            var validTransitions = GetValidTransitions();
            if (!validTransitions.Contains(targetStatus))
            {
                throw new InvalidPivotTransitionException(Status, targetStatus);
            }
        }

        private List<PivotStatus> GetValidTransitions()
        {
            return Status switch
            {
                PivotStatus.Planned => new List<PivotStatus> { PivotStatus.InProgress, PivotStatus.Abandoned },
                PivotStatus.InProgress => new List<PivotStatus> { PivotStatus.Completed, PivotStatus.OnHold, PivotStatus.Abandoned },
                PivotStatus.OnHold => new List<PivotStatus> { PivotStatus.InProgress, PivotStatus.Abandoned },
                PivotStatus.Completed => new List<PivotStatus>(), // No transitions from completed
                PivotStatus.Abandoned => new List<PivotStatus>(), // No transitions from abandoned
                _ => new List<PivotStatus>()
            };
        }

        public ImpactMetricDefinition DefineImpactMetric(
            string metricName,
            string description,
            string unit,
            string measurementMethod,
            decimal? baselineValue = null,
            decimal? targetValue = null)
        {
            if (_impactMetrics.Any(im => im.MetricName == metricName))
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:DuplicateName")
                    .WithData("entityType", "ImpactMetric")
                    .WithData("name", metricName);
            }

            var impactMetric = new ImpactMetricDefinition(
                Guid.NewGuid(),
                Id,
                metricName,
                description,
                unit,
                measurementMethod,
                baselineValue,
                targetValue
            );

            _impactMetrics.Add(impactMetric);
            return impactMetric;
        }

        public void RemoveImpactMetric(Guid metricId)
        {
            var metric = _impactMetrics.FirstOrDefault(im => im.Id == metricId);
            if (metric != null)
            {
                _impactMetrics.Remove(metric);
            }
        }

        public ImpactMetricDefinition GetImpactMetric(Guid metricId)
        {
            return _impactMetrics.FirstOrDefault(im => im.Id == metricId);
        }

        public void RecordImpactMeasurement(
            Guid metricId,
            decimal value,
            DateTime measuredAt,
            string measurementMethod = null,
            string notes = null)
        {
            var metric = GetImpactMetric(metricId);
            if (metric == null)
            {
                throw new BusinessModelManagementDomainException("BusinessModelManagement:ErrorMessage:MetricNotFound")
                    .WithData("metricId", metricId);
            }

            var measurement = new ImpactValue(
                metric.MetricName,
                value,
                metric.Unit,
                measuredAt,
                measurementMethod,
                notes
            );

            metric.AddMeasurement(measurement);
        }

        public double CalculateOverallSuccessRate()
        {
            var metricsWithTargets = _impactMetrics.Where(im => im.TargetValue.HasValue).ToList();
            if (!metricsWithTargets.Any())
                return 0.0;

            var successfulMetrics = 0;
            foreach (var metric in metricsWithTargets)
            {
                var progress = metric.CalculateProgressToTarget();
                if (progress.HasValue && progress.Value >= 100.0)
                {
                    successfulMetrics++;
                }
            }

            return (double)successfulMetrics / metricsWithTargets.Count * 100.0;
        }

        public bool IsOverdue()
        {
            return Status != PivotStatus.Completed && 
                   Status != PivotStatus.Abandoned && 
                   DateTime.UtcNow > TargetCompletionDate;
        }

        public TimeSpan? GetDuration()
        {
            if (ActualCompletionDate.HasValue)
            {
                return ActualCompletionDate.Value - InitiatedDate;
            }

            if (Status == PivotStatus.InProgress)
            {
                return DateTime.UtcNow - InitiatedDate;
            }

            return null;
        }
    }
}
