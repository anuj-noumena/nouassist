using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.LearningDevelopment.Integration.Events;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Enums;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.WorkManagement.TaskManagement.EventHandlers
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

        // These would come from configuration or workflow service
        private readonly Guid _defaultWorkflowId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid _todoStatusId = Guid.Parse("00000000-0000-0000-0000-000000000002");

        public RequestLearningTasksEventHandler(
            ITaskRepository taskRepository,
            ITaskInstanceRepository taskInstanceRepository,
            IDistributedEventBus eventBus,
            ILogger<RequestLearningTasksEventHandler> logger)
        {
            _taskRepository = taskRepository;
            _taskInstanceRepository = taskInstanceRepository;
            _eventBus = eventBus;
            _logger = logger;
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
                var learningTask = new Task(
                    id: Guid.NewGuid(),
                    userId: eventData.UserId,
                    title: eventData.Title,
                    tenantId: eventData.TenantId);

                learningTask.UpdateDescription(eventData.Description ?? $"Complete learning path: {eventData.Title}");
                learningTask.SetCategory("Learning & Development");

                await _taskRepository.InsertAsync(learningTask, autoSave: false);

                var createdTaskIds = new List<Guid>();

                // Create theory task instances
                var theoryTaskIds = await CreateTheoryTaskInstancesAsync(
                    eventData, learningTask.Id);
                createdTaskIds.AddRange(theoryTaskIds);

                // Create practice task instances
                var practiceTaskIds = await CreatePracticeTaskInstancesAsync(
                    eventData, learningTask.Id, theoryTaskIds);
                createdTaskIds.AddRange(practiceTaskIds);

                await _taskInstanceRepository.GetDbContext().SaveChangesAsync();

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
                var taskInstance = new TaskInstance(
                    id: Guid.NewGuid(),
                    taskId: taskId,
                    workflowId: _defaultWorkflowId,
                    workflowStatusId: _todoStatusId,
                    ownerId: eventData.UserId,
                    projectId: null,
                    areaId: null, // Could map to "Personal Development" area
                    tenantId: eventData.TenantId);

                taskInstance.UpdateTitle($"Theory: {theoryTask.Title}");
                taskInstance.UpdateDescription(theoryTask.Description);

                // Set schedule
                taskInstance.SetSchedule(new TaskSchedule
                {
                    StartDate = theoryTask.DueDate.AddDays(-1),
                    DueDate = theoryTask.DueDate
                });

                // Set priority - learning tasks are important but not urgent
                taskInstance.SetPriority(ImportanceUrgencyMatrix.ImportantNotUrgent);

                // Set cognitive load based on phase
                taskInstance.SetCognitiveLoad(GetCognitiveLoadForPhase(theoryTask.Phase.ToString()));

                // Store learning metadata in custom fields
                taskInstance.AddCustomField("LearningPathId", eventData.LearningPathId.ToString());
                taskInstance.AddCustomField("Phase", theoryTask.Phase.ToString());
                taskInstance.AddCustomField("Category", "Theory");

                // Add checklist items
                foreach (var checklistItem in theoryTask.ChecklistItems)
                {
                    taskInstance.AddChecklistItem(checklistItem);
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
            Guid taskId,
            List<Guid> theoryTaskIds)
        {
            var taskIds = new List<Guid>();
            var lastTheoryTaskId = theoryTaskIds.LastOrDefault();

            foreach (var practiceTask in eventData.PracticeTasks)
            {
                var taskInstance = new TaskInstance(
                    id: Guid.NewGuid(),
                    taskId: taskId,
                    workflowId: _defaultWorkflowId,
                    workflowStatusId: _todoStatusId,
                    ownerId: eventData.UserId,
                    projectId: null,
                    areaId: null,
                    tenantId: eventData.TenantId);

                taskInstance.UpdateTitle($"Practice: {practiceTask.Title}");
                taskInstance.UpdateDescription(practiceTask.Description);

                // Set schedule
                taskInstance.SetSchedule(new TaskSchedule
                {
                    StartDate = practiceTask.DueDate.AddDays(-1),
                    DueDate = practiceTask.DueDate
                });

                // Practice tasks might be more urgent
                taskInstance.SetPriority(ImportanceUrgencyMatrix.ImportantUrgent);

                // Practice requires higher cognitive load
                taskInstance.SetCognitiveLoad(CognitiveLoad.High);

                // Store learning metadata
                taskInstance.AddCustomField("LearningPathId", eventData.LearningPathId.ToString());
                taskInstance.AddCustomField("DevelopmentStep", practiceTask.Step.ToString());
                taskInstance.AddCustomField("Category", "Practice");

                // Add checklist items
                foreach (var checklistItem in practiceTask.ChecklistItems)
                {
                    taskInstance.AddChecklistItem(checklistItem);
                }

                // Add dependency: all theory tasks must complete before practice
                if (lastTheoryTaskId != Guid.Empty)
                {
                    taskInstance.AddDependency(new Entities.TaskDependency(
                        dependentTaskId: taskInstance.Id,
                        dependsOnTaskId: lastTheoryTaskId,
                        dependencyType: DependencyType.FinishToStart));
                }

                // Add dependencies from prerequisite tasks
                foreach (var prerequisiteId in practiceTask.PrerequisiteTaskIds)
                {
                    taskInstance.AddDependency(new Entities.TaskDependency(
                        dependentTaskId: taskInstance.Id,
                        dependsOnTaskId: prerequisiteId,
                        dependencyType: DependencyType.FinishToStart));
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
