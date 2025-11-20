using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Enums;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.Integrations.EventHandlers.WorkManagement
{
    /// <summary>
    /// Handles RequestLearningTasksEto to automatically create task instances for learning
    /// </summary>
    public class RequestLearningTasksEventHandler :
        IDistributedEventHandler<RequestLearningTasksEto>,
        ITransientDependency
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly IDistributedEventBus _eventBus;
        private readonly ILogger<RequestLearningTasksEventHandler> _logger;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        // These would come from configuration or workflow service
        private readonly Guid _defaultTaskTypeId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        public RequestLearningTasksEventHandler(
            ITaskRepository taskRepository,
            ITaskInstanceRepository taskInstanceRepository,
            IDistributedEventBus eventBus,
            ILogger<RequestLearningTasksEventHandler> logger,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _taskRepository = taskRepository;
            _taskInstanceRepository = taskInstanceRepository;
            _eventBus = eventBus;
            _logger = logger;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(RequestLearningTasksEto eventData)
        {
            try
            {
                _logger.LogInformation(
                    "Creating task instances for learning path {LearningPathId}",
                    eventData.LearningPathId);

                // Create main learning task template
                var learningTask = new NouAssist.WorkManagement.TaskManagement.Aggregates.Task(
                    id: Guid.NewGuid(),
                    ownerId: eventData.UserId,
                    title: eventData.Title,
                    taskTypeId: _defaultTaskTypeId,
                    tenantId: eventData.TenantId,
                    projectId: null,
                    areaId: null,
                    contextId: null,
                    taskHierarchy: null,
                    defaultImportance: 0.9,
                    defaultUrgency: 0.3);
                
                learningTask.Description = eventData.Description;

                await _taskRepository.InsertAsync(learningTask, autoSave: true);

                var createdTaskIds = new List<Guid>();

                // Create theory task instances
                var theoryTaskIds = await CreateTheoryTaskInstancesAsync(
                    eventData, learningTask.Id);
                createdTaskIds.AddRange(theoryTaskIds);

                // Create practice task instances
                var practiceTaskIds = await CreatePracticeTaskInstancesAsync(
                    eventData, learningTask.Id);
                createdTaskIds.AddRange(practiceTaskIds);

                // Save all changes
                var uow = _unitOfWorkManager.Current;
                if (uow != null)
                {
                    await uow.SaveChangesAsync();
                }

                // Publish completion event
                await _eventBus.PublishAsync(new LearningTasksCreatedEto
                {
                    LearningPathId = eventData.LearningPathId,
                    UserId = eventData.UserId,
                    TenantId = eventData.TenantId,
                    TaskId = learningTask.Id,
                    TaskInstanceIds = createdTaskIds,
                    TheoryTaskIds = theoryTaskIds,
                    PracticeTaskIds = practiceTaskIds,
                    CreatedAt = DateTime.UtcNow
                });

                _logger.LogInformation(
                    "Created {Count} task instances for learning path {LearningPathId}",
                    createdTaskIds.Count, eventData.LearningPathId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error creating task instances for learning path {LearningPathId}",
                    eventData.LearningPathId);
                throw;
            }
        }

        private async Task<List<Guid>> CreateTheoryTaskInstancesAsync(
            RequestLearningTasksEto eventData,
            Guid taskId)
        {
            var taskIds = new List<Guid>();

            foreach (var theoryTask in eventData.TheoryTasks)
            {
                // Learning tasks are important but not urgent
                var importance = 0.9;
                var urgency = 0.3;

                var taskInstance = new TaskInstance(
                    id: Guid.NewGuid(),
                    taskId: taskId,
                    workflowId: _defaultWorkflowId,
                    workflowStatusId: _todoStatusId,
                    ownerId: eventData.UserId,
                    projectId: null,
                    areaId: null,
                    contextId: null,
                    importance: importance,
                    urgency: urgency,
                    tenantId: eventData.TenantId);

                // Store learning metadata in custom fields
                taskInstance.SetCustomFieldValue("Title", $"Theory: {theoryTask.Title}");
                taskInstance.SetCustomFieldValue("Description", theoryTask.Description);
                taskInstance.SetCustomFieldValue("LearningPathId", eventData.LearningPathId.ToString());
                taskInstance.SetCustomFieldValue("Phase", theoryTask.Phase.ToString());
                taskInstance.SetCustomFieldValue("Category", "Theory");
                taskInstance.SetCustomFieldValue("DueDate", theoryTask.DueDate.ToString("O"));
                taskInstance.SetCustomFieldValue("CognitiveLoad", GetCognitiveLoadForPhase(theoryTask.Phase.ToString()).ToString());

                // Store checklist items as JSON or delimited string
                if (theoryTask.ChecklistItems != null && theoryTask.ChecklistItems.Any())
                {
                    taskInstance.SetCustomFieldValue("ChecklistItems", string.Join("|", theoryTask.ChecklistItems));
                }

                await _taskInstanceRepository.InsertAsync(taskInstance, autoSave: false);
                taskIds.Add(taskInstance.Id);

                _logger.LogDebug(
                    "Created theory task instance {TaskInstanceId} for phase {Phase}",
                    taskInstance.Id, theoryTask.Phase);
            }

            return taskIds;
        }

        private async Task<List<Guid>> CreatePracticeTaskInstancesAsync(
            RequestLearningTasksEto eventData,
            Guid taskId)
        {
            var taskIds = new List<Guid>();

            foreach (var practiceTask in eventData.PracticeTasks)
            {
                // Practice tasks might be more urgent
                var importance = 0.9;
                var urgency = 0.9;

                var taskInstance = new TaskInstance(
                    id: Guid.NewGuid(),
                    taskId: taskId,
                    workflowId: _defaultWorkflowId,
                    workflowStatusId: _todoStatusId,
                    ownerId: eventData.UserId,
                    projectId: null,
                    areaId: null,
                    contextId: null,
                    importance: importance,
                    urgency: urgency,
                    tenantId: eventData.TenantId);

                // Store learning metadata
                taskInstance.SetCustomFieldValue("Title", $"Practice: {practiceTask.Title}");
                taskInstance.SetCustomFieldValue("Description", practiceTask.Description);
                taskInstance.SetCustomFieldValue("LearningPathId", eventData.LearningPathId.ToString());
                taskInstance.SetCustomFieldValue("DevelopmentStep", practiceTask.Step.ToString());
                taskInstance.SetCustomFieldValue("Category", "Practice");
                taskInstance.SetCustomFieldValue("DueDate", practiceTask.DueDate.ToString("O"));
                taskInstance.SetCustomFieldValue("CognitiveLoad", CognitiveLoad.High.ToString());

                // Store checklist items
                if (practiceTask.ChecklistItems != null && practiceTask.ChecklistItems.Any())
                {
                    taskInstance.SetCustomFieldValue("ChecklistItems", string.Join("|", practiceTask.ChecklistItems));
                }

                // Store prerequisite task IDs for future dependency handling
                if (practiceTask.PrerequisiteTaskIds != null && practiceTask.PrerequisiteTaskIds.Any())
                {
                    taskInstance.SetCustomFieldValue("PrerequisiteTaskIds", string.Join(",", practiceTask.PrerequisiteTaskIds));
                }

                await _taskInstanceRepository.InsertAsync(taskInstance, autoSave: false);
                taskIds.Add(taskInstance.Id);

                _logger.LogDebug(
                    "Created practice task instance {TaskInstanceId} for step {Step}",
                    taskInstance.Id, practiceTask.Step);
            }

            return taskIds;
        }

        private CognitiveLoad GetCognitiveLoadForPhase(string phase)
        {
            return phase switch
            {
                "PreLearning" => CognitiveLoad.Low,
                "ActiveLearning" => CognitiveLoad.Medium,
                "FeynmanSummary" => CognitiveLoad.Medium,
                "BloomAssessment" => CognitiveLoad.High,
                "SpacedRepetition" => CognitiveLoad.Low,
                "Mastery" => CognitiveLoad.High,
                _ => CognitiveLoad.Medium
            };
        }
    }

    /// <summary>
    /// Response event sent back to LearningDevelopment
    /// </summary>
    [Serializable]
    public class LearningTasksCreatedEto
    {
        public Guid LearningPathId { get; set; }
        public Guid UserId { get; set; }
        public Guid? TenantId { get; set; }
        public Guid TaskId { get; set; }
        public List<Guid> TaskInstanceIds { get; set; } = new();
        public List<Guid> TheoryTaskIds { get; set; } = new();
        public List<Guid> PracticeTaskIds { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
