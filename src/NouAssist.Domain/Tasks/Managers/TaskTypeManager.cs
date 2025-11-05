using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Data;
using NouAssist.Tasks.Exceptions;
using Volo.Abp.Domain.Services;

namespace NouAssist.Tasks.Managers
{
    public class TaskTypeManager : DomainService
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly ILogger<TaskTypeManager> _logger;

        public TaskTypeManager(
            ITaskTypeRepository taskTypeRepository,
            ILogger<TaskTypeManager> logger)
        {
            _taskTypeRepository = taskTypeRepository;
            _logger = logger;
        }

        public async Task<TaskType> CreateAsync(
            string name,
            string description = null,
            string color = null)
        {
            // Check for duplicate name
            var existing = await _taskTypeRepository.FindByNameAsync(name);
            if (existing != null)
            {
                throw new TaskDomainException(
                    "Tasks:DuplicateTaskTypeName",
                    $"A task type with name '{name}' already exists.");
            }

            var taskType = new TaskType(
                GuidGenerator.Create(),
                name,
                description,
                color,
                CurrentTenant.Id);

            _logger.LogInformation("Created task type '{Name}'", name);

            return taskType;
        }
    }
}