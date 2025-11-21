using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Enums;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Exceptions;
using NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Managers
{
    /// <summary>
    /// Manager for CustomerInterview aggregate - handles complex business logic
    /// </summary>
    public class CustomerInterviewManager : DomainService
    {
        private readonly ICustomerInterviewRepository _interviewRepository;
        private readonly ILogger<CustomerInterviewManager> _logger;

        public CustomerInterviewManager(
            ICustomerInterviewRepository interviewRepository,
            ILogger<CustomerInterviewManager> logger)
        {
            _interviewRepository = interviewRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new customer interview with validation
        /// </summary>
        public async Task<CustomerInterview> CreateAsync(
            string customerName,
            string customerRole,
            string customerCompany,
            DateTime scheduledDate,
            Guid conductedBy,
            string interviewType,
            string description = null)
        {
            // Validate scheduled date is not in the past
            if (scheduledDate < Clock.Now.AddHours(-1)) // Allow 1 hour buffer
            {
                throw new ProductDiscoveryDomainException(
                    "ProductDiscovery:InvalidScheduledDate",
                    "Scheduled date cannot be in the past.");
            }

            var interview = CustomerInterview.Create(
                GuidGenerator.Create(),
                customerName,
                customerRole,
                customerCompany,
                scheduledDate,
                conductedBy,
                interviewType,
                description,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new customer interview {InterviewId} with {CustomerName} at {Company}",
                interview.Id, customerName, customerCompany);

            return interview;
        }

        /// <summary>
        /// Completes an interview
        /// </summary>
        public async Task<CustomerInterview> CompleteAsync(
            Guid interviewId,
            DateTime actualDate,
            string notes)
        {
            var interview = await _interviewRepository.GetAsync(interviewId);

            // Business rule: Can only complete scheduled or in-progress interviews
            if (interview.Status == InterviewStatus.Completed)
            {
                throw new InvalidInterviewStatusTransitionException(
                    "Interview is already completed.");
            }

            if (interview.Status == InterviewStatus.Cancelled)
            {
                throw new InvalidInterviewStatusTransitionException(
                    "Cannot complete a cancelled interview.");
            }

            interview.Complete(actualDate, notes);
            await _interviewRepository.UpdateAsync(interview);

            _logger.LogInformation(
                "Completed customer interview {InterviewId}",
                interviewId);

            return interview;
        }

        /// <summary>
        /// Cancels an interview
        /// </summary>
        public async Task<CustomerInterview> CancelAsync(
            Guid interviewId,
            string cancellationReason)
        {
            var interview = await _interviewRepository.GetAsync(interviewId);

            // Business rule: Cannot cancel completed interviews
            if (interview.Status == InterviewStatus.Completed)
            {
                throw new InvalidInterviewStatusTransitionException(
                    "Cannot cancel a completed interview.");
            }

            interview.Cancel(cancellationReason);
            await _interviewRepository.UpdateAsync(interview);

            _logger.LogInformation(
                "Cancelled customer interview {InterviewId}. Reason: {Reason}",
                interviewId, cancellationReason);

            return interview;
        }

        /// <summary>
        /// Deletes an interview (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid interviewId, bool hardDelete = false)
        {
            var interview = await _interviewRepository.GetAsync(interviewId);

            // Business rule: Cannot delete completed interviews with insights
            if (interview.Status == InterviewStatus.Completed &&
                interview.KeyInsights.Count > 0 &&
                !hardDelete)
            {
                throw new ProductDiscoveryDomainException(
                    "ProductDiscovery:CannotDeleteInterviewWithInsights",
                    "Cannot delete a completed interview with insights. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _interviewRepository.DeleteAsync(interview);
                _logger.LogWarning("Hard deleted customer interview {InterviewId}", interviewId);
            }
            else
            {
                await _interviewRepository.DeleteAsync(interview, autoSave: true);
                _logger.LogInformation("Soft deleted customer interview {InterviewId}", interviewId);
            }
        }
    }
}
