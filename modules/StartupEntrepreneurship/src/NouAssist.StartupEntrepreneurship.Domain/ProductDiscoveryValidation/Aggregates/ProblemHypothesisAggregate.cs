using System;
using System.Collections.Generic;
using System.Linq;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Events;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a problem hypothesis with validation attempts
    /// </summary>
    public class ProblemHypothesis : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public HypothesisStatement Statement { get; private set; }
        public string Context { get; private set; }
        public Guid? RelatedCustomerSegmentId { get; private set; }
        public ValidationStatus Status { get; private set; }
        public DateTime? ValidatedDate { get; private set; }
        public string ValidationSummary { get; private set; }
        public ProblemSolutionFitMetric FitMetric { get; private set; }

        // Entities within the aggregate
        private readonly List<ValidationAttempt> _validationAttempts;

        // Public read-only access
        public IReadOnlyCollection<ValidationAttempt> ValidationAttempts => _validationAttempts.AsReadOnly();

        protected ProblemHypothesis()
        {
            _validationAttempts = new List<ValidationAttempt>();
        }

        public static ProblemHypothesis Create(
            Guid id,
            HypothesisStatement statement,
            string context,
            Guid? relatedCustomerSegmentId = null,
            Guid? tenantId = null)
        {
            return new ProblemHypothesis(id, statement, context, relatedCustomerSegmentId, tenantId);
        }

        private ProblemHypothesis(
            Guid id,
            HypothesisStatement statement,
            string context,
            Guid? relatedCustomerSegmentId,
            Guid? tenantId) : base(id)
        {
            Statement = statement ?? throw new ArgumentNullException(nameof(statement));
            SetContext(context);
            RelatedCustomerSegmentId = relatedCustomerSegmentId;
            TenantId = tenantId;
            Status = ValidationStatus.NotStarted;
            _validationAttempts = new List<ValidationAttempt>();
        }

        public void UpdateStatement(HypothesisStatement statement)
        {
            if (Status == ValidationStatus.Validated || Status == ValidationStatus.Invalidated)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "Cannot update statement of a validated hypothesis");
            }

            Statement = statement ?? throw new ArgumentNullException(nameof(statement));
        }

        public void UpdateContext(string context)
        {
            SetContext(context);
        }

        public void AddValidationAttempt(
            string method,
            string description,
            DateTime plannedStartDate)
        {
            if (Status == ValidationStatus.Validated || Status == ValidationStatus.Invalidated)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "Cannot add validation attempts to a finalized hypothesis");
            }

            var attempt = new ValidationAttempt(
                GuidGenerator.Create(),
                Id,
                method,
                description,
                plannedStartDate);

            _validationAttempts.Add(attempt);
        }

        public void StartValidationAttempt(Guid attemptId, DateTime startDate)
        {
            var attempt = GetValidationAttempt(attemptId);
            attempt.StartValidation(startDate);

            if (Status == ValidationStatus.NotStarted)
            {
                Status = ValidationStatus.InProgress;
            }
        }

        public void CompleteValidationAttempt(
            Guid attemptId,
            DateTime completedDate,
            ValidationStatus status,
            string results)
        {
            var attempt = GetValidationAttempt(attemptId);
            attempt.CompleteValidation(completedDate, status, results);

            // Update hypothesis status based on validation results
            UpdateOverallStatus();
        }

        public void AddMetricToValidationAttempt(Guid attemptId, ValidationMetric metric)
        {
            var attempt = GetValidationAttempt(attemptId);
            attempt.AddMetric(metric);
        }

        public void SetFitMetric(ProblemSolutionFitMetric fitMetric)
        {
            FitMetric = fitMetric ?? throw new ArgumentNullException(nameof(fitMetric));
        }

        public void FinalizeValidation(string validationSummary, DateTime validatedDate)
        {
            if (Status != ValidationStatus.InProgress)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "Cannot finalize validation without validation attempts in progress");
            }

            if (!_validationAttempts.Any(v => v.Status != ValidationStatus.NotStarted))
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "No validation attempts have been started");
            }

            ValidationSummary = validationSummary;
            ValidatedDate = validatedDate;

            AddDistributedEvent(new ProblemHypothesisValidatedEto
            {
                Id = Id,
                HypothesisStatement = Statement.ToFullStatement(),
                Status = Status,
                ValidatedDate = validatedDate,
                TenantId = TenantId
            });
        }

        public int GetCompletedValidationCount()
        {
            return _validationAttempts.Count(v => v.Status == ValidationStatus.Validated ||
                                                   v.Status == ValidationStatus.Invalidated ||
                                                   v.Status == ValidationStatus.Inconclusive);
        }

        public decimal GetValidationProgress()
        {
            if (_validationAttempts.Count == 0) return 0;

            var completed = GetCompletedValidationCount();
            return (decimal)completed / _validationAttempts.Count * 100;
        }

        private void UpdateOverallStatus()
        {
            var completedAttempts = _validationAttempts.Where(v =>
                v.Status == ValidationStatus.Validated ||
                v.Status == ValidationStatus.Invalidated ||
                v.Status == ValidationStatus.Inconclusive).ToList();

            if (completedAttempts.Count == 0)
            {
                return; // Status remains in progress
            }

            var validatedCount = completedAttempts.Count(v => v.Status == ValidationStatus.Validated);
            var invalidatedCount = completedAttempts.Count(v => v.Status == ValidationStatus.Invalidated);
            var inconclusiveCount = completedAttempts.Count(v => v.Status == ValidationStatus.Inconclusive);

            // Simple majority rule
            if (validatedCount > invalidatedCount + inconclusiveCount)
            {
                Status = ValidationStatus.Validated;
            }
            else if (invalidatedCount > validatedCount + inconclusiveCount)
            {
                Status = ValidationStatus.Invalidated;
            }
            else
            {
                Status = ValidationStatus.Inconclusive;
            }
        }

        private ValidationAttempt GetValidationAttempt(Guid attemptId)
        {
            var attempt = _validationAttempts.FirstOrDefault(v => v.Id == attemptId);
            if (attempt == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Validation attempt not found");
            }
            return attempt;
        }

        private void SetContext(string context)
        {
            Context = Check.NotNullOrWhiteSpace(
                context,
                nameof(context),
                maxLength: 1000
            );
        }
    }
}
