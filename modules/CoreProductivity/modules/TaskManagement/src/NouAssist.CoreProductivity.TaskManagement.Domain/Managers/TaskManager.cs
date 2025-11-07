// src/NouAssist.Tasks.Domain/Managers/TaskManager.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Data;
using NouAssist.Tasks.Exceptions;
using Volo.Abp.Domain.Services;
using Task = NouAssist.Tasks.Aggregates.Task;

namespace NouAssist.Tasks.Managers
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
            Guid? projectId = null,
            string description = null,
            double? defaultImportance = null,
            double? defaultUrgency = null)
        {
            // Check for duplicate title within same project
            if (projectId.HasValue)
            {
                var existingTasks = await _taskRepository.GetListAsync(
                    new Specifications.TasksByProjectSpecification(projectId.Value));
                
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
                CurrentTenant.Id,
                projectId,
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
            string newTitle = null,
            string newDescription = null,
            double? newImportance = null,
            double? newUrgency = null)
        {
            var task = await _taskRepository.GetAsync(taskId);

            if (newTitle != null)
            {
                task.UpdateTitle(newTitle);
            }

            if (newDescription != null)
            {
                task.Description = newDescription;
            }

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
                task.SoftDelete();
                await _taskRepository.UpdateAsync(task);
                _logger.LogInformation("Soft deleted task {TaskId}", taskId);
            }
        }
    }
}
