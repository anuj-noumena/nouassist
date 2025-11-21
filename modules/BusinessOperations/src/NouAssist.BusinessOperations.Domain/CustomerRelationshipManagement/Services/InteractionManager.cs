using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Aggregates;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Entities;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Enums;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Repositories;
using NouAssist.BusinessOperations.CustomerRelationshipManagement.Services.Interfaces;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;

namespace NouAssist.BusinessOperations.CustomerRelationshipManagement.Services
{
    /// <summary>
    /// Implementation of the interaction management domain service
    /// </summary>
    public class InteractionManager : DomainService, IInteractionManager
    {
        private readonly IInteractionLogRepository _interactionLogRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ILogger<InteractionManager> _logger;

        public InteractionManager(
            IInteractionLogRepository interactionLogRepository,
            IGuidGenerator guidGenerator,
            ILogger<InteractionManager> logger)
        {
            _interactionLogRepository = interactionLogRepository;
            _guidGenerator = guidGenerator;
            _logger = logger;
        }

        public async Task<InteractionLog> CreateInteractionLogAsync(
            string entityType,
            Guid entityId,
            string summary)
        {
            var interactionLog = InteractionLog.Create(
                _guidGenerator.Create(),
                entityType,
                entityId,
                summary,
                CurrentTenant.Id);

            return await _interactionLogRepository.InsertAsync(interactionLog, autoSave: true);
        }

        public async Task<InteractionLog> UpdateSummaryAsync(
            Guid id,
            string summary)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(id);
            interactionLog.UpdateSummary(summary);
            return await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
        }

        public async Task<CommunicationRecord> AddCommunicationRecordAsync(
            Guid interactionLogId,
            InteractionType type,
            string subject,
            string content,
            DateTime communicationTime,
            bool isInbound,
            string sender,
            string recipient)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            
            var record = interactionLog.AddCommunicationRecord(
                type,
                subject,
                content,
                communicationTime,
                isInbound,
                sender,
                recipient);
                
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
            return record;
        }

        public async Task AddAttachmentAsync(
            Guid interactionLogId,
            CommunicationRecord record,
            Guid documentId,
            string fileName,
            string contentType)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            
            var documentInfo = new DocumentId(documentId, fileName, contentType);
            interactionLog.AddAttachmentToRecord(record, documentInfo);
            
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
        }

        public async Task<FollowUpTask> AddFollowUpTaskAsync(
            Guid interactionLogId,
            string title,
            string description,
            DateTime dueDate,
            FollowUpPriority priority,
            Guid? assignedUserId = null)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            
            var task = interactionLog.AddFollowUpTask(
                title,
                description,
                dueDate,
                priority,
                assignedUserId);
                
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
            return task;
        }

        public async Task<FollowUpTask> UpdateFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task,
            string title = null,
            string description = null,
            DateTime? dueDate = null,
            FollowUpPriority? priority = null,
            Guid? assignedUserId = null)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            
            interactionLog.UpdateFollowUpTask(
                task,
                title,
                description,
                dueDate,
                priority,
                assignedUserId);
                
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
            return task;
        }

        public async Task<FollowUpTask> StartFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            interactionLog.StartFollowUpTask(task);
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
            return task;
        }

        public async Task<FollowUpTask> CompleteFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task,
            string completionNotes = null)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            interactionLog.CompleteFollowUpTask(task, completionNotes);
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
            return task;
        }

        public async Task<FollowUpTask> CancelFollowUpTaskAsync(
            Guid interactionLogId,
            FollowUpTask task,
            string cancellationNotes = null)
        {
            var interactionLog = await _interactionLogRepository.GetAsync(interactionLogId);
            interactionLog.CancelFollowUpTask(task, cancellationNotes);
            await _interactionLogRepository.UpdateAsync(interactionLog, autoSave: true);
            return task;
        }

        public async Task<List<InteractionLog>> GetEntityInteractionsAsync(
            Guid entityId,
            string entityType,
            int skipCount = 0,
            int maxResultCount = 10,
            string sorting = null)
        {
            return await _interactionLogRepository.GetForEntityAsync(
                entityId,
                entityType,
                skipCount,
                maxResultCount,
                sorting);
        }
    }
}
