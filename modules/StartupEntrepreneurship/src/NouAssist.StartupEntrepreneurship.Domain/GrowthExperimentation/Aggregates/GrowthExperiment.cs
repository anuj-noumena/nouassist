using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Constants;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Entities;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Enums;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Events;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.Exceptions;
using NouAssist.StartupEntrepreneurship.GrowthExperimentation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.GrowthExperimentation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a growth experiment
    /// </summary>
    public class GrowthExperiment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Experiment name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Experiment description
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Current status of the experiment
        /// </summary>
        public ExperimentStatus Status { get; private set; }
        
        /// <summary>
        /// Goal of the experiment
        /// </summary>
        public ExperimentGoal Goal { get; private set; }
        
        /// <summary>
        /// Target metrics for the experiment
        /// </summary>
        private readonly List<TargetMetric> _targetMetrics;
        public IReadOnlyCollection<TargetMetric> TargetMetrics => _targetMetrics.AsReadOnly();
        
        /// <summary>
        /// Hypotheses for the experiment
        /// </summary>
        private readonly List<ExperimentHypothesis> _hypotheses;
        public IReadOnlyCollection<ExperimentHypothesis> Hypotheses => _hypotheses.AsReadOnly();
        
        /// <summary>
        /// Variants for the experiment
        /// </summary>
        private readonly List<ExperimentVariant> _variants;
        public IReadOnlyCollection<ExperimentVariant> Variants => _variants.AsReadOnly();
        
        /// <summary>
        /// Start date of the experiment
        /// </summary>
        public DateTime? StartDate { get; private set; }
        
        /// <summary>
        /// End date of the experiment
        /// </summary>
        public DateTime? EndDate { get; private set; }
        
        /// <summary>
        /// Statistical significance of the experiment results
        /// </summary>
        public StatisticalSignificance Significance { get; private set; }
        
        /// <summary>
        /// The winning variant ID, if any
        /// </summary>
        public Guid? WinningVariantId { get; private set; }
        
        /// <summary>
        /// Learnings from the experiment
        /// </summary>
        public ExperimentLearnings Learnings { get; internal set; }
        
        protected GrowthExperiment()
        {
            _targetMetrics = new List<TargetMetric>();
            _hypotheses = new List<ExperimentHypothesis>();
            _variants = new List<ExperimentVariant>();
        }
        
        /// <summary>
        /// Creates a new experiment
        /// </summary>
        public static GrowthExperiment Create(
            Guid id,
            string name, 
            string description, 
            ExperimentGoal goal,
            Guid? tenantId = null)
        {
            var experiment = new GrowthExperiment
            {
                Id = id,
                TenantId = tenantId,
                Status = ExperimentStatus.Draft
            };
            
            experiment.SetName(name);
            experiment.Description = description;
            experiment.Goal = goal;
            
            experiment.AddDistributedEvent(new ExperimentCreatedEto
            {
                Id = experiment.Id,
                Name = experiment.Name,
                Description = experiment.Description,
                Status = experiment.Status,
                TenantId = experiment.TenantId,
                CreationTime = DateTime.UtcNow
            });
            
            return experiment;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name, 
                nameof(name), 
                maxLength: GrowthExperimentationConstants.Experiments.MaxNameLength
            );
        }
        
        /// <summary>
        /// Adds a target metric to the experiment
        /// </summary>
        public void AddTargetMetric(TargetMetric metric)
        {
            if (Status != ExperimentStatus.Draft)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Draft);
            }
            
            if (metric.IsPrimary && _targetMetrics.Any(m => m.IsPrimary))
            {
                // Only one primary metric allowed
                throw new BusinessException("GrowthExperimentation:ErrorMessage:MultiplePrimaryMetrics");
            }
            
            _targetMetrics.Add(metric);
        }
        
        /// <summary>
        /// Adds a hypothesis to the experiment
        /// </summary>
        public void AddHypothesis(string statement, string rationale)
        {
            if (Status != ExperimentStatus.Draft)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Draft);
            }
            
            var hypothesis = new ExperimentHypothesis(Id, statement, rationale);
            _hypotheses.Add(hypothesis);
        }
        
        /// <summary>
        /// Adds a variant to the experiment
        /// </summary>
        public ExperimentVariant AddVariant(
            Guid variantId,
            string name, 
            string description, 
            VariantType type, 
            VariantSetup setup)
        {
            if (Status != ExperimentStatus.Draft)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Draft);
            }
            
            // Check for duplicate name
            if (_variants.Any(v => v.Name == name))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:DuplicateVariantName")
                    .WithData("variantName", name);
            }
            
            // Only one control variant allowed
            if (type == VariantType.Control && _variants.Any(v => v.Type == VariantType.Control))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:MultipleControlVariants");
            }
            
            var variant = new ExperimentVariant(variantId, Id, name, description, type, setup);
            _variants.Add(variant);
            
            return variant;
        }
        
        /// <summary>
        /// Sets the experiment as ready to run
        /// </summary>
        public void SetReady()
        {
            if (Status != ExperimentStatus.Draft)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Draft);
            }
            
            ValidateExperimentConfiguration();
            
            ChangeStatus(ExperimentStatus.Ready);
        }
        
        /// <summary>
        /// Starts the experiment
        /// </summary>
        public void Start(DateTime startDate)
        {
            if (Status != ExperimentStatus.Ready)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Ready);
            }
            
            StartDate = startDate;
            ChangeStatus(ExperimentStatus.Running);
            
            AddDistributedEvent(new ExperimentStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = ExperimentStatus.Ready,
                NewStatus = ExperimentStatus.Running,
                TenantId = TenantId,
                StatusChangeTime = startDate
            });
        }
        
        /// <summary>
        /// Pauses the experiment
        /// </summary>
        public void Pause(DateTime pauseDate)
        {
            if (Status != ExperimentStatus.Running)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Running);
            }
            
            ChangeStatus(ExperimentStatus.Paused);
            
            AddDistributedEvent(new ExperimentStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = ExperimentStatus.Running,
                NewStatus = ExperimentStatus.Paused,
                TenantId = TenantId,
                StatusChangeTime = pauseDate
            });
        }
        
        /// <summary>
        /// Resumes the experiment
        /// </summary>
        public void Resume(DateTime resumeDate)
        {
            if (Status != ExperimentStatus.Paused)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Paused);
            }
            
            ChangeStatus(ExperimentStatus.Running);
            
            AddDistributedEvent(new ExperimentStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = ExperimentStatus.Paused,
                NewStatus = ExperimentStatus.Running,
                TenantId = TenantId,
                StatusChangeTime = resumeDate
            });
        }
        
        /// <summary>
        /// Completes the experiment
        /// </summary>
        public void Complete(DateTime endDate)
        {
            if (Status != ExperimentStatus.Running && Status != ExperimentStatus.Paused)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.Running, ExperimentStatus.Paused);
            }
            
            EndDate = endDate;
            ChangeStatus(ExperimentStatus.AnalysisInProgress);
            
            AddDistributedEvent(new ExperimentStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = Status,
                NewStatus = ExperimentStatus.AnalysisInProgress,
                TenantId = TenantId,
                StatusChangeTime = endDate
            });
        }
        
        /// <summary>
        /// Sets the statistical significance of the experiment results
        /// </summary>
        public void SetSignificance(StatisticalSignificance significance, Guid? winningVariantId)
        {
            if (Status != ExperimentStatus.AnalysisInProgress)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.AnalysisInProgress);
            }
            
            Significance = significance;
            
            if (winningVariantId.HasValue)
            {
                if (!_variants.Any(v => v.Id == winningVariantId.Value))
                {
                    throw new BusinessException("GrowthExperimentation:ErrorMessage:VariantNotFound")
                        .WithData("variantId", winningVariantId.Value)
                        .WithData("experimentId", Id);
                }
                
                WinningVariantId = winningVariantId.Value;
            }
        }
        
        /// <summary>
        /// Sets the experiment as fully completed with analysis
        /// </summary>
        public void FinalizeAnalysis(ExperimentLearnings learnings)
        {
            if (Status != ExperimentStatus.AnalysisInProgress)
            {
                throw new ExperimentInvalidStateException(Id, Status, ExperimentStatus.AnalysisInProgress);
            }
            
            if (Significance == null)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:SignificanceRequired");
            }
            
            Learnings = learnings;
            ChangeStatus(ExperimentStatus.Completed);
            
            int durationInDays = 0;
            if (StartDate.HasValue && EndDate.HasValue)
            {
                durationInDays = (int)(EndDate.Value - StartDate.Value).TotalDays;
            }
            
            AddDistributedEvent(new ExperimentCompletedEto
            {
                Id = Id,
                Name = Name,
                IsSignificant = Significance.IsSignificant,
                WinningVariantId = WinningVariantId,
                TenantId = TenantId,
                CompletionTime = DateTime.UtcNow,
                DurationInDays = durationInDays
            });
        }
        
        /// <summary>
        /// Terminates the experiment before completion
        /// </summary>
        public void Terminate(DateTime terminationDate)
        {
            if (Status == ExperimentStatus.Completed || Status == ExperimentStatus.Terminated)
            {
                throw new ExperimentInvalidStateException(Id, Status, 
                    ExperimentStatus.Draft, ExperimentStatus.Ready, 
                    ExperimentStatus.Running, ExperimentStatus.Paused, 
                    ExperimentStatus.AnalysisInProgress);
            }
            
            EndDate = terminationDate;
            ChangeStatus(ExperimentStatus.Terminated);
            
            AddDistributedEvent(new ExperimentStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = Status,
                NewStatus = ExperimentStatus.Terminated,
                TenantId = TenantId,
                StatusChangeTime = terminationDate
            });
        }
        
        /// <summary>
        /// Sets a hypothesis result as supported or not based on experiment results
        /// </summary>
        public void SetHypothesisResult(string hypothesisStatement, bool isSupported)
        {
            var hypothesis = _hypotheses.FirstOrDefault(h => h.Statement == hypothesisStatement);
            
            if (hypothesis == null)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:HypothesisNotFound")
                    .WithData("statement", hypothesisStatement);
            }
            
            hypothesis.SetIsSupported(isSupported);
        }
        
        private void ChangeStatus(ExperimentStatus newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
            
            AddDistributedEvent(new ExperimentStatusChangedEto
            {
                Id = Id,
                Name = Name,
                PreviousStatus = oldStatus,
                NewStatus = newStatus,
                TenantId = TenantId,
                StatusChangeTime = DateTime.UtcNow
            });
        }
        
        private void ValidateExperimentConfiguration()
        {
            // Check if we have at least one metric
            if (!_targetMetrics.Any())
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:NoMetricsDefined");
            }
            
            // Check if we have at least one primary metric
            if (!_targetMetrics.Any(m => m.IsPrimary))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:NoPrimaryMetricDefined");
            }
            
            // Check if we have at least one hypothesis
            if (!_hypotheses.Any())
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:NoHypothesisDefined");
            }
            
            // Check if we have minimum required variants
            if (_variants.Count < GrowthExperimentationConstants.Experiments.MinVariantsCount)
            {
                throw new InsufficientVariantsException(
                    Id, 
                    _variants.Count, 
                    GrowthExperimentationConstants.Experiments.MinVariantsCount
                );
            }
            
            // Check if we have a control variant
            if (!_variants.Any(v => v.Type == VariantType.Control))
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:NoControlVariant");
            }
            
            // Check if traffic allocation sums to approximately 1
            double totalAllocation = _variants.Sum(v => v.Setup.TrafficAllocation);
            if (Math.Abs(totalAllocation - 1.0) > 0.001)
            {
                throw new BusinessException("GrowthExperimentation:ErrorMessage:InvalidTrafficAllocation")
                    .WithData("totalAllocation", totalAllocation);
            }
        }
    }
}
