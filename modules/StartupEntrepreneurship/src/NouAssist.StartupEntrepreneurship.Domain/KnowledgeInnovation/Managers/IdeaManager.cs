using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Aggregates;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Enums;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Exceptions;
using NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Repositories;
using Volo.Abp.Domain.Services;

namespace NouAssist.StartupEntrepreneurship.KnowledgeInnovation.Managers
{
    /// <summary>
    /// Manager for Idea aggregate - handles complex business logic
    /// </summary>
    public class IdeaManager : DomainService
    {
        private readonly IIdeaRepository _ideaRepository;
        private readonly ILogger<IdeaManager> _logger;

        public IdeaManager(
            IIdeaRepository ideaRepository,
            ILogger<IdeaManager> logger)
        {
            _ideaRepository = ideaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new idea with validation
        /// </summary>
        public async Task<Idea> CreateAsync(
            string title,
            string description,
            Guid submittedBy,
            string category = null,
            string tags = null)
        {
            // Check for duplicate title
            var existing = await _ideaRepository.FindByTitleAsync(
                title,
                CurrentTenant.Id);

            if (existing != null)
            {
                throw new DuplicateTitleException(
                    $"An idea with title '{title}' already exists.");
            }

            var idea = Idea.Create(
                GuidGenerator.Create(),
                title,
                description,
                submittedBy,
                category,
                tags,
                CurrentTenant.Id);

            _logger.LogInformation(
                "Created new idea {IdeaId} with title '{Title}' submitted by {SubmittedBy}",
                idea.Id, idea.Title, submittedBy);

            return idea;
        }

        /// <summary>
        /// Updates idea with validation
        /// </summary>
        public async Task<Idea> UpdateAsync(
            Guid ideaId,
            string newTitle = null,
            string newDescription = null,
            string newCategory = null)
        {
            var idea = await _ideaRepository.GetAsync(ideaId);

            // Business rule: Cannot update implemented ideas
            if (idea.Status == IdeaStatus.Implemented)
            {
                throw new InvalidStatusTransitionException(
                    "Cannot update an implemented idea.");
            }

            // Check for duplicate title if being changed
            if (!string.IsNullOrWhiteSpace(newTitle) && idea.Title != newTitle)
            {
                var existing = await _ideaRepository.FindByTitleAsync(
                    newTitle,
                    CurrentTenant.Id);

                if (existing != null && existing.Id != ideaId)
                {
                    throw new DuplicateTitleException(
                        $"An idea with title '{newTitle}' already exists.");
                }
            }

            idea.UpdateBasicInfo(
                newTitle ?? idea.Title,
                newDescription ?? idea.Description,
                newCategory ?? idea.Category);

            await _ideaRepository.UpdateAsync(idea);

            _logger.LogInformation(
                "Updated idea {IdeaId}",
                ideaId);

            return idea;
        }

        /// <summary>
        /// Advances idea status with validation
        /// </summary>
        public async Task<Idea> AdvanceStatusAsync(
            Guid ideaId,
            IdeaStatus newStatus,
            string notes = null)
        {
            var idea = await _ideaRepository.GetAsync(ideaId);

            // Validate status transition
            if (!CanTransitionToStatus(idea.Status, newStatus))
            {
                throw new InvalidStatusTransitionException(
                    $"Cannot transition from {idea.Status} to {newStatus}");
            }

            idea.UpdateStatus(newStatus, notes);
            await _ideaRepository.UpdateAsync(idea);

            _logger.LogInformation(
                "Advanced idea {IdeaId} to status {NewStatus}",
                ideaId, newStatus);

            return idea;
        }

        /// <summary>
        /// Approves an idea for implementation
        /// </summary>
        public async Task<Idea> ApproveAsync(
            Guid ideaId,
            Guid approvedBy,
            string approvalNotes = null)
        {
            var idea = await _ideaRepository.GetAsync(ideaId);

            // Business rule: Can only approve ideas in Review status
            if (idea.Status != IdeaStatus.UnderReview)
            {
                throw new InvalidStatusTransitionException(
                    $"Can only approve ideas in UnderReview status. Current status: {idea.Status}");
            }

            idea.Approve(approvedBy, approvalNotes, Clock.Now);
            await _ideaRepository.UpdateAsync(idea);

            _logger.LogInformation(
                "Approved idea {IdeaId} by {ApprovedBy}",
                ideaId, approvedBy);

            return idea;
        }

        /// <summary>
        /// Rejects an idea
        /// </summary>
        public async Task<Idea> RejectAsync(
            Guid ideaId,
            Guid rejectedBy,
            string rejectionReason)
        {
            var idea = await _ideaRepository.GetAsync(ideaId);

            // Business rule: Cannot reject implemented ideas
            if (idea.Status == IdeaStatus.Implemented)
            {
                throw new InvalidStatusTransitionException(
                    "Cannot reject an implemented idea.");
            }

            if (string.IsNullOrWhiteSpace(rejectionReason))
            {
                throw new KnowledgeInnovationDomainException(
                    "KnowledgeInnovation:RejectionReasonRequired",
                    "A rejection reason must be provided.");
            }

            idea.Reject(rejectedBy, rejectionReason, Clock.Now);
            await _ideaRepository.UpdateAsync(idea);

            _logger.LogInformation(
                "Rejected idea {IdeaId} by {RejectedBy}. Reason: {Reason}",
                ideaId, rejectedBy, rejectionReason);

            return idea;
        }

        /// <summary>
        /// Validates if a status transition is allowed
        /// </summary>
        private bool CanTransitionToStatus(IdeaStatus currentStatus, IdeaStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                // From Submitted
                (IdeaStatus.Submitted, IdeaStatus.UnderReview) => true,
                (IdeaStatus.Submitted, IdeaStatus.Rejected) => true,

                // From UnderReview
                (IdeaStatus.UnderReview, IdeaStatus.Approved) => true,
                (IdeaStatus.UnderReview, IdeaStatus.Rejected) => true,

                // From Approved
                (IdeaStatus.Approved, IdeaStatus.InProgress) => true,
                (IdeaStatus.Approved, IdeaStatus.Rejected) => true,

                // From InProgress
                (IdeaStatus.InProgress, IdeaStatus.Implemented) => true,
                (IdeaStatus.InProgress, IdeaStatus.Rejected) => true,

                // Same status
                _ when currentStatus == newStatus => true,

                _ => false
            };
        }

        /// <summary>
        /// Deletes an idea (soft delete)
        /// </summary>
        public async Task DeleteAsync(Guid ideaId, bool hardDelete = false)
        {
            var idea = await _ideaRepository.GetAsync(ideaId);

            // Business rule: Cannot delete implemented ideas
            if (idea.Status == IdeaStatus.Implemented && !hardDelete)
            {
                throw new KnowledgeInnovationDomainException(
                    "KnowledgeInnovation:CannotDeleteImplementedIdea",
                    "Cannot delete an implemented idea. Use hard delete if necessary.");
            }

            if (hardDelete)
            {
                await _ideaRepository.DeleteAsync(idea);
                _logger.LogWarning("Hard deleted idea {IdeaId}", ideaId);
            }
            else
            {
                await _ideaRepository.DeleteAsync(idea, autoSave: true);
                _logger.LogInformation("Soft deleted idea {IdeaId}", ideaId);
            }
        }
    }
}
