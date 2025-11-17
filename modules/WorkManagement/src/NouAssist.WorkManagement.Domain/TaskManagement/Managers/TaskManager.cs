// src/NouAssist.Tasks.Domain/Managers/TaskManager.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Exceptions;
using NouAssist.WorkManagement.TaskManagement.Specifications;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.TaskManagement.Managers
{
    /// <summary>
    /// Manager for Task aggregate - handles complex business logic
    /// </summary>
    public class TaskManager : DomainService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<TaskManager> _logger;

        public TaskManager(
            ITaskRepository taskRepository,
            ILogger<TaskManager> logger)
        {
            _taskRepository = taskRepository;
            _logger = logger;
        }

        public async Task<Aggregates.Task> CreateAsync(
            Guid ownerId,
            string title,
            Guid taskType,
            Guid? projectId = null,
            Guid? areaId = null,
            Guid? contextId = null,
            TaskHierarchy ? taskHierarchy = null,
            string description = null,
            double? defaultImportance = null,
            double? defaultUrgency = null)
        {
            // Check for duplicate title within same project
            if (projectId.HasValue)
            {
                var existingTasks = await _taskRepository.GetListAsync(
                    new TasksByProjectSpecification(projectId.Value).ToExpression());

                if (existingTasks.Any(t => t.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new TaskDomainException(
                        "Tasks:DuplicateTitle",
                        $"A task with title '{title}' already exists in this project.");
                }
            }

            var task = new Aggregates.Task(
                GuidGenerator.Create(),
                ownerId,
                title,
                taskType,
                CurrentTenant.Id,
                projectId,
                areaId: areaId,
                contextId: contextId,
                taskHierarchy: taskHierarchy,
                defaultImportance ?? TasksConsts.DefaultImportance,
                defaultUrgency ?? TasksConsts.DefaultUrgency);
            
            task.Description = description;

            _logger.LogInformation(
                "Created new task blueprint {TaskId} with title '{Title}'",
                task.Id, task.Title);

            return task;
        }

        public async Task<Aggregates.Task> UpdateAsync(
            Guid taskId,
            string? newTitle = null,
            string? newDescription = null,
            string? expectedResult = null,
            string? newNotes = null,
            double? newImportance = null,
            double? newUrgency = null)
        {
            var task = await _taskRepository.GetAsync(taskId);

            task.UpdateContent(newTitle, newDescription, expectedResult, newNotes);

            if (newImportance.HasValue && newUrgency.HasValue)
            {
                task.SetImportanceUrgency(newImportance.Value, newUrgency.Value);
            }

            await _taskRepository.UpdateAsync(task);

            return task;
        }

        public async void DeleteAsync(Guid taskId, bool hardDelete = false)
        {
            var task = await _taskRepository.GetAsync(taskId);

            if (hardDelete)
            {
                _taskRepository.DeleteAsync(task);
                _logger.LogWarning("Hard deleted task {TaskId}", taskId);
            }
            else
            {
                task.IsDeleted = true;
                await _taskRepository.UpdateAsync(task);
                _logger.LogInformation("Soft deleted task {TaskId}", taskId);
            }
        }
    }
}