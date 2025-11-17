using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using Volo.Abp.Domain.Services;
using TaskAggregate = NouAssist.WorkManagement.TaskManagement.Aggregates.Task;

namespace NouAssist.WorkManagement.TaskManagement.Services
{
    /// <summary>
    /// Orchestrates operations across Task and its sub-aggregates
    /// </summary>
    public class TaskAggregateOrchestrationService : DomainService
    {
        private readonly ITaskChecklistRepository _taskChecklistRepository;
        private readonly ITaskAcceptanceCriteriaRepository _taskAcceptanceCriteriaRepository;

        public TaskAggregateOrchestrationService(
            ITaskChecklistRepository taskChecklistRepository,
            ITaskAcceptanceCriteriaRepository taskAcceptanceCriteriaRepository)
        {
            _taskChecklistRepository = taskChecklistRepository;
            _taskAcceptanceCriteriaRepository = taskAcceptanceCriteriaRepository;
        }

        /// <summary>
        /// Creates a complete Task blueprint with all sub-aggregates
        /// </summary>
        public async Task<TaskAggregate> CreateTaskWithSubAggregatesAsync(
            Guid taskId,
            Guid ownerId,
            string title,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            // Create main Task aggregate
            var task = new TaskAggregate(
                taskId,
                ownerId,
                title,
                tenantId);

            // Create TaskChecklist aggregate
            var checklist = new TaskChecklist(
                GuidGenerator.Create(),
                taskId,
                tenantId);
            await _taskChecklistRepository.InsertAsync(checklist, cancellationToken: cancellationToken);

            // Create TaskAcceptanceCriteria aggregate
            var acceptanceCriteria = new TaskAcceptanceCriteria(
                GuidGenerator.Create(),
                taskId,
                tenantId);
            await _taskAcceptanceCriteriaRepository.InsertAsync(acceptanceCriteria, cancellationToken: cancellationToken);

            return task;
        }

        /// <summary>
        /// Deletes a Task and all its related sub-aggregates
        /// </summary>
        public async System.Threading.Tasks.Task DeleteTaskWithSubAggregatesAsync(
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            // Delete TaskChecklist
            var checklist = await _taskChecklistRepository.GetByTaskIdAsync(taskId, cancellationToken);
            if (checklist != null)
            {
                await _taskChecklistRepository.DeleteAsync(checklist, cancellationToken: cancellationToken);
            }

            // Delete TaskAcceptanceCriteria
            var acceptanceCriteria = await _taskAcceptanceCriteriaRepository.GetByTaskIdAsync(taskId, cancellationToken);
            if (acceptanceCriteria != null)
            {
                await _taskAcceptanceCriteriaRepository.DeleteAsync(acceptanceCriteria, cancellationToken: cancellationToken);
            }
        }
    }

    /// <summary>
    /// Orchestrates operations across TaskInstance and its sub-aggregates
    /// </summary>
    public class TaskInstanceAggregateOrchestrationService : DomainService
    {
        private readonly ITaskChecklistCompletionRepository _checklistCompletionRepository;
        private readonly ITaskAcceptanceCompletionRepository _acceptanceCompletionRepository;
        private readonly ITaskActionStepsCompletionRepository _actionStepsCompletionRepository;
        private readonly ITaskDependenciesRepository _dependenciesRepository;
        private readonly ITaskChecklistRepository _taskChecklistRepository;
        private readonly ITaskAcceptanceCriteriaRepository _taskAcceptanceCriteriaRepository;

        public TaskInstanceAggregateOrchestrationService(
            ITaskChecklistCompletionRepository checklistCompletionRepository,
            ITaskAcceptanceCompletionRepository acceptanceCompletionRepository,
            ITaskActionStepsCompletionRepository actionStepsCompletionRepository,
            ITaskDependenciesRepository dependenciesRepository,
            ITaskChecklistRepository taskChecklistRepository,
            ITaskAcceptanceCriteriaRepository taskAcceptanceCriteriaRepository)
        {
            _checklistCompletionRepository = checklistCompletionRepository;
            _acceptanceCompletionRepository = acceptanceCompletionRepository;
            _actionStepsCompletionRepository = actionStepsCompletionRepository;
            _dependenciesRepository = dependenciesRepository;
            _taskChecklistRepository = taskChecklistRepository;
            _taskAcceptanceCriteriaRepository = taskAcceptanceCriteriaRepository;
        }

        /// <summary>
        /// Creates a TaskInstance with all completion sub-aggregates initialized from blueprint
        /// </summary>
        public async Task<TaskInstance> CreateTaskInstanceWithSubAggregatesAsync(
            TaskInstance taskInstance,
            Guid taskId,
            CancellationToken cancellationToken = default)
        {
            var tenantId = taskInstance.TenantId;

            // 1. Create TaskChecklistCompletion from blueprint
            var taskChecklist = await _taskChecklistRepository.GetByTaskIdAsync(taskId, cancellationToken);
            if (taskChecklist != null && taskChecklist.HasItems)
            {
                var checklistCompletion = new TaskChecklistCompletion(
                    GuidGenerator.Create(),
                    taskInstance.Id,
                    taskChecklist.Id,
                    tenantId);

                var checklistItemIds = taskChecklist.Items.Select(i => i.Id).ToList();
                checklistCompletion.InitializeFromBlueprint(checklistItemIds);

                await _checklistCompletionRepository.InsertAsync(checklistCompletion, cancellationToken: cancellationToken);
            }

            // 2. Create TaskAcceptanceCompletion from blueprint
            var taskAcceptanceCriteria = await _taskAcceptanceCriteriaRepository.GetByTaskIdAsync(taskId, cancellationToken);
            if (taskAcceptanceCriteria != null && taskAcceptanceCriteria.HasCriteria)
            {
                var acceptanceCompletion = new TaskAcceptanceCompletion(
                    GuidGenerator.Create(),
                    taskInstance.Id,
                    taskAcceptanceCriteria.Id,
                    tenantId);

                var criterionIds = taskAcceptanceCriteria.Criteria.Select(c => c.Id).ToList();
                acceptanceCompletion.InitializeFromBlueprint(criterionIds);

                await _acceptanceCompletionRepository.InsertAsync(acceptanceCompletion, cancellationToken: cancellationToken);
            }

            // 3. Create TaskActionStepsCompletion (if action steps exist in blueprint - would need separate aggregate)
            var actionStepsCompletion = new TaskActionStepsCompletion(
                GuidGenerator.Create(),
                taskInstance.Id,
                tenantId);
            await _actionStepsCompletionRepository.InsertAsync(actionStepsCompletion, cancellationToken: cancellationToken);

            // 4. Create TaskDependencies
            var dependencies = new TaskDependencies(
                GuidGenerator.Create(),
                taskInstance.Id,
                tenantId);
            await _dependenciesRepository.InsertAsync(dependencies, cancellationToken: cancellationToken);

            return taskInstance;
        }

        /// <summary>
        /// Deletes a TaskInstance and all its related sub-aggregates
        /// </summary>
        public async System.Threading.Tasks.Task DeleteTaskInstanceWithSubAggregatesAsync(
            Guid taskInstanceId,
            CancellationToken cancellationToken = default)
        {
            // Delete TaskChecklistCompletion
            var checklistCompletion = await _checklistCompletionRepository.GetByTaskInstanceIdAsync(taskInstanceId, cancellationToken);
            if (checklistCompletion != null)
            {
                await _checklistCompletionRepository.DeleteAsync(checklistCompletion, cancellationToken: cancellationToken);
            }

            // Delete TaskAcceptanceCompletion
            var acceptanceCompletion = await _acceptanceCompletionRepository.GetByTaskInstanceIdAsync(taskInstanceId, cancellationToken);
            if (acceptanceCompletion != null)
            {
                await _acceptanceCompletionRepository.DeleteAsync(acceptanceCompletion, cancellationToken: cancellationToken);
            }

            // Delete TaskActionStepsCompletion
            var actionStepsCompletion = await _actionStepsCompletionRepository.GetByTaskInstanceIdAsync(taskInstanceId, cancellationToken);
            if (actionStepsCompletion != null)
            {
                await _actionStepsCompletionRepository.DeleteAsync(actionStepsCompletion, cancellationToken: cancellationToken);
            }

            // Delete TaskDependencies
            var dependencies = await _dependenciesRepository.GetByTaskInstanceIdAsync(taskInstanceId, cancellationToken);
            if (dependencies != null)
            {
                await _dependenciesRepository.DeleteAsync(dependencies, cancellationToken: cancellationToken);
            }
        }

        /// <summary>
        /// Updates progress on TaskInstance based on checklist completion percentage
        /// </summary>
        public async System.Threading.Tasks.Task UpdateProgressFromChecklistAsync(
            Guid taskInstanceId,
            TaskInstance taskInstance,
            CancellationToken cancellationToken = default)
        {
            var checklistCompletion = await _checklistCompletionRepository.GetByTaskInstanceIdAsync(taskInstanceId, cancellationToken);
            if (checklistCompletion != null && checklistCompletion.TotalItems > 0)
            {
                var progress = checklistCompletion.GetCompletionPercentage();
                taskInstance.UpdateProgress(progress);
            }
        }
    }
}
