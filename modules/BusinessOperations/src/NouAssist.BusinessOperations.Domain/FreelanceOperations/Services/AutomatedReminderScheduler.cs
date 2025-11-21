using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.FreelanceOperations.Enums;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Repositories;
using NouAssist.BusinessOperations.FreelanceOperations.Interfaces.Services;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.FreelanceOperations.Services
{
    /// <summary>
    /// Domain service for scheduling automated reminders
    /// </summary>
    public class AutomatedReminderScheduler : DomainService, IAutomatedReminderScheduler
    {
        private readonly IFreelanceProjectRepository _projectRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProposalRepository _proposalRepository;
        private readonly IRemindersRepository _remindersRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<AutomatedReminderScheduler> _logger;

        public AutomatedReminderScheduler(
            IFreelanceProjectRepository projectRepository,
            IClientRepository clientRepository,
            IProposalRepository proposalRepository,
            IRemindersRepository remindersRepository,
            IGuidGenerator guidGenerator,
            ILogger<AutomatedReminderScheduler> logger)
        {
            _projectRepository = projectRepository;
            _clientRepository = clientRepository;
            _proposalRepository = proposalRepository;
            _remindersRepository = remindersRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task ScheduleProjectRemindersAsync(Guid projectId)
        {
            try
            {
                var project = await _projectRepository.GetAsync(projectId);
                
                // Set up reminders based on project state
                
                // Remind about project start date if in future
                if (project.StartDate.HasValue && project.StartDate.Value > DateTime.UtcNow)
                {
                    await _remindersRepository.CreateReminderAsync(
                        _guidGenerator.Create(),
                        "Project Start",
                        $"Project '{project.Name}' is starting on {project.StartDate.Value:d}",
                        project.StartDate.Value.AddDays(-1),
                        project.Id,
                        "FreelanceProject"
                    );
                }
                
                // Remind about project end date approaching
                if (project.EndDate.HasValue && project.Status == ProjectStatus.Active)
                {
                    await _remindersRepository.CreateReminderAsync(
                        _guidGenerator.Create(),
                        "Project End Approaching",
                        $"Project '{project.Name}' is scheduled to end on {project.EndDate.Value:d}",
                        project.EndDate.Value.AddDays(-3),
                        project.Id,
                        "FreelanceProject"
                    );
                }
                
                // Remind about overdue milestones
                foreach (var milestone in project.Milestones)
                {
                    if (milestone.Deadline.HasValue && !milestone.IsCompleted)
                    {
                        // Reminder 2 days before deadline
                        await _remindersRepository.CreateReminderAsync(
                            _guidGenerator.Create(),
                            "Milestone Deadline Approaching",
                            $"Milestone '{milestone.Details.Title}' for project '{project.Name}' is due on {milestone.Deadline.Value:d}",
                            milestone.Deadline.Value.AddDays(-2),
                            milestone.Id,
                            "ProjectMilestone"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to schedule reminders for project {ProjectId}", projectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ScheduleClientFollowUpRemindersAsync(Guid clientId)
        {
            try
            {
                var client = await _clientRepository.GetAsync(clientId);
                
                // Schedule reminders for pending follow-ups
                foreach (var record in client.GetPendingFollowUps())
                {
                    if (record.FollowUpDate.HasValue)
                    {
                        await _remindersRepository.CreateReminderAsync(
                            _guidGenerator.Create(),
                            "Client Follow-Up",
                            $"Follow up with client '{client.Name}' regarding '{record.CommunicationDetails.Subject}'",
                            record.FollowUpDate.Value,
                            record.Id,
                            "ClientCommunicationRecord"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to schedule follow-up reminders for client {ClientId}", clientId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task ScheduleProposalRemindersAsync(Guid proposalId)
        {
            try
            {
                var proposal = await _proposalRepository.GetAsync(proposalId);
                
                // Schedule reminder for proposal expiration
                if (proposal.ExpirationDate.HasValue && 
                    (proposal.Status == ProposalStatus.Sent || proposal.Status == ProposalStatus.Viewed))
                {
                    await _remindersRepository.CreateReminderAsync(
                        _guidGenerator.Create(),
                        "Proposal Expiring",
                        $"Proposal '{proposal.Title}' is expiring on {proposal.ExpirationDate.Value:d}",
                        proposal.ExpirationDate.Value.AddDays(-1),
                        proposal.Id,
                        "Proposal"
                    );
                }
                
                // Schedule reminder to follow up if sent but not viewed after a few days
                if (proposal.Status == ProposalStatus.Sent && proposal.SentDate.HasValue)
                {
                    await _remindersRepository.CreateReminderAsync(
                        _guidGenerator.Create(),
                        "Proposal Follow-Up",
                        $"Follow up on proposal '{proposal.Title}' that was sent but not yet viewed",
                        proposal.SentDate.Value.AddDays(3),
                        proposal.Id,
                        "Proposal"
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to schedule reminders for proposal {ProposalId}", proposalId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Dictionary<string, object>>> GetDueRemindersAsync(Guid userId)
        {
            try
            {
                var dueReminders = await _remindersRepository.GetDueRemindersAsync(userId);
                
                return dueReminders
                    .Select(r => new Dictionary<string, object>
                    {
                        ["id"] = r.Id,
                        ["title"] = r.Title,
                        ["message"] = r.Message,
                        ["dueDate"] = r.DueDate,
                        ["entityId"] = r.EntityId,
                        ["entityType"] = r.EntityType,
                        ["isCompleted"] = r.IsCompleted
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get due reminders for user {UserId}", userId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task MarkReminderCompletedAsync(Guid reminderId)
        {
            try
            {
                await _remindersRepository.MarkReminderCompletedAsync(reminderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark reminder {ReminderId} as completed", reminderId);
                throw;
            }
        }
    }
}
