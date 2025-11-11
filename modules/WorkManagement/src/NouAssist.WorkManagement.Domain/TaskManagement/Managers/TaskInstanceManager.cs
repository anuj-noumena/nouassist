using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Data;
using NouAssist.Tasks.Entities;
using NouAssist.Tasks.Exceptions;
using NouAssist.Tasks.Policies;
using NouAssist.Tasks.Services;
using NouAssist.Tasks.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.Tasks.Managers
{
    /// <summary>
    /// Manager for TaskInstance aggregate - handles complex business logic
    /// </summary>
    public class TaskInstanceManager : DomainService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly TaskInstanceProcessingService _processingService;
        private readonly TaskDependencyService _dependencyService;
        private readonly ITaskStartPolicy _taskStartPolicy;
        private readonly ILogger<TaskInstanceManager> _logger;

        public TaskInstanceManager(
            ITaskRepository taskRepository,
            ITaskInstanceRepository taskInstanceRepository,
            TaskInstanceProcessingService processingService,
            TaskDependencyService dependencyService,
            ITaskStartPolicy taskStartPolicy,
            ILogger<TaskInstanceManager> logger)
        {
            _taskRepository = taskRepository;
            _taskInstanceRepository = taskInstanceRepository;
            _processingService = processingService;
            _dependencyService = dependencyService;
            _taskStartPolicy = taskStartPolicy;
            _logger = logger;
        }

        public async Task<TaskInstance> CreateFromBlueprintAsync(
            Guid taskId,
            Guid projectId,
            Guid workflowStatusId,
            Guid ownerId,
            Guid? assigneeId = null,
            DateTime? plannedDueDate = null)
        {
            var taskBlueprint = await _taskRepository.GetWithDetailsAsync(taskId);

            if (!taskBlueprint.IsActive)
            {
                throw new TaskInvalidStateException(
                    "Cannot create instance from inactive task blueprint.");
            }

            var instance = _processingService.CreateTaskInstanceFromBlueprint(
                taskBlueprint,
                projectId,
                workflowStatusId,
                ownerId,
                assigneeId,
                plannedDueDate);

            instance = await _taskInstanceRepository.InsertAsync(instance);

            _logger.LogInformation(
                "Created task instance {InstanceId} from blueprint {TaskId}",
                instance.Id, taskId);

            return instance;
        }

        public async Task<TaskInstance> StartAsync(Guid taskInstanceId, Guid startedBy)
        {
            var instance = await _taskInstanceRepository.GetAsync(taskInstanceId);

            // Check if blocked by dependencies
            var canStart = await CanStartTaskAsync(instance);
            if (!canStart)
            {
                throw new TaskInvalidStateException(
                    "Cannot start task due to unmet dependencies.");
            }

            instance.Start(_taskStartPolicy, startedBy);
            await _taskInstanceRepository.UpdateAsync(instance);

            return instance;
        }

        public async Task<TaskInstance> CompleteAsync(
            Guid taskInstanceId,
            Guid completedBy,
            string resolution = null,
            string resolutionNote = null)
        {
            var instance = await _taskInstanceRepository.GetWithDetailsAsync(taskInstanceId);
            var taskBlueprint = await _taskRepository.GetWithDetailsAsync(instance.TaskId);

            var result = _processingService.ProcessTaskCompletion(
                instance,
                taskBlueprint,
                completedBy,
                resolution,
                resolutionNote);

            if (!result.IsSuccessful)
            {
                throw new TaskInvalidStateException(
                    $"Cannot complete task: {string.Join(", ", result.Errors)}");
            }

            await _taskInstanceRepository.UpdateAsync(result.ProcessedTaskInstance);

            return result.ProcessedTaskInstance;
        }

        public async Task<TaskInstance> DelegateAsync(
            Guid taskInstanceId,
            Guid fromUserId,
            Guid toUserId,
            DateTime? dueBy = null,
            bool followupRequired = true,
            DateTime? followupAt = null,
            string note = null)
        {
            var instance = await _taskInstanceRepository.GetAsync(taskInstanceId);
            instance.Delegate(fromUserId,
                toUserId
                );
            // Create delegation entity
            // var delegation = new TaskDelegation(
            //     GuidGenerator.Create(),
            //     taskInstanceId,
            //     fromUserId,
            //     toUserId,
            //     dueBy,
            //     followupRequired,
            //     followupAt,
            //     note);
            //
            // // Update task instance
            // instance.AddDelegationReference(delegation.Id);

            await _taskInstanceRepository.UpdateAsync(instance);

            _logger.LogInformation(
                "Delegated task instance {TaskInstanceId} from {FromUser} to {ToUser}",
                taskInstanceId, fromUserId, toUserId);

            return instance;
        }

        private async Task<bool> CanStartTaskAsync(TaskInstance instance)
        {
            // Get all task instance dependencies
            // This would need a dependency repository
            var dependencies = new List<TaskDependency>();
            var dependentInstances = new List<TaskInstance>();

            return _dependencyService.CanStartTask(instance, dependentInstances, dependencies);
        }
    }
}
