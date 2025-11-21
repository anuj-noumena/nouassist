using System;
using System.Collections.Generic;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Constants;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Entities
{
    /// <summary>
    /// Entity representing an attempt to validate a hypothesis
    /// </summary>
    public class ValidationAttempt : Entity<Guid>
    {
        public Guid HypothesisId { get; private set; }
        public string Method { get; private set; }
        public string Description { get; private set; }
        public DateTime PlannedStartDate { get; private set; }
        public DateTime? ActualStartDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public ValidationStatus Status { get; private set; }
        public string Results { get; private set; }
        public string Notes { get; private set; }
        public List<ValidationMetric> Metrics { get; private set; }

        protected ValidationAttempt() 
        { 
            Metrics = new List<ValidationMetric>();
        }

        internal ValidationAttempt(
            Guid id,
            Guid hypothesisId,
            string method,
            string description,
            DateTime plannedStartDate) : base(id)
        {
            HypothesisId = hypothesisId;
            SetMethod(method);
            SetDescription(description);
            PlannedStartDate = plannedStartDate;
            Status = ValidationStatus.NotStarted;
            Metrics = new List<ValidationMetric>();
        }

        internal void StartValidation(DateTime startDate)
        {
            if (Status != ValidationStatus.NotStarted)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation", 
                    "Validation has already been started");
            }

            ActualStartDate = startDate;
            Status = ValidationStatus.InProgress;
        }

        internal void CompleteValidation(DateTime completedDate, ValidationStatus status, string results)
        {
            if (Status != ValidationStatus.InProgress)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation", 
                    "Validation must be in progress to complete");
            }

            if (status == ValidationStatus.NotStarted || status == ValidationStatus.InProgress)
            {
                throw new BusinessException("ProductDiscovery:Validation:InvalidValue", 
                    "Invalid completion status");
            }

            CompletedDate = completedDate;
            Status = status;
            Results = results;
        }

        internal void AddMetric(ValidationMetric metric)
        {
            if (metric == null)
                throw new ArgumentNullException(nameof(metric));

            Metrics.Add(metric);
        }

        internal void UpdateNotes(string notes)
        {
            Notes = notes;
        }

        private void SetMethod(string method)
        {
            Method = Check.NotNullOrWhiteSpace(
                method,
                nameof(method),
                maxLength: 100
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxValidationNotesLength
            );
        }
    }
}
