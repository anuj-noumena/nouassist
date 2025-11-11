using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Managers;
using NouAssist.WorkManagement.TaskManagement.Permissions;
using NouAssist.WorkManagement.TaskManagement.Specifications;
using NouAssist.WorkManagement.TaskManagement.Tasks.Dtos;
using NouAssist.WorkManagement.TaskManagement.ValueObjects;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.WorkManagement.TaskManagement.Tasks
{
    [Authorize(TasksPermissions.Tasks.Default)]
    public class TaskAppService : ApplicationService, ITaskAppService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskInstanceRepository _taskInstanceRepository;
        private readonly TaskManager _taskManager;

        public TaskAppService(
            ITaskRepository taskRepository,
            ITaskInstanceRepository taskInstanceRepository,
            TaskManager taskManager)
        {
            _taskRepository = taskRepository;
            _taskInstanceRepository = taskInstanceRepository;
            _taskManager = taskManager;
        }

        public async Task<TaskDto> GetAsync(Guid id)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);
            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        public async Task<PagedResultDto<TaskDto>> GetListAsync(GetTaskListDto input)
        {
            var spec = CreateListSpecification(input);

            var tasks = await _taskRepository.GetListAsync(spec);
            var totalCount = await _taskRepository.CountAsync(spec.ToExpression());

            // Apply sorting and paging
            var query = tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                // Use System.Linq.Dynamic.Core for dynamic sorting
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderByDescending(t => t.CreationTime);
            }

            var pagedTasks = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<TaskDto>(
                totalCount,
                ObjectMapper.Map<List<Aggregates.Task>, List<TaskDto>>(pagedTasks)
            );
        }

        [Authorize(TasksPermissions.Tasks.Create)]
        public async Task<TaskDto> CreateAsync(CreateTaskDto input)
        {
            // Use domain manager to create task
            var task = await _taskManager.CreateAsync(
                CurrentUser.Id.Value,
                input.Title,
                input.ProjectId,
                input.Description,
                input.DefaultImportance,
                input.DefaultUrgency
            );

            // Set additional properties
            task.Notes = input.Notes;
            task.ExpectedResult = input.ExpectedResult;
            task.DefaultContextId = input.DefaultContextId;
            task.DefaultAreaId = input.DefaultAreaId;
            task.DefaultSprintId = input.DefaultSprintId;
            task.TaskTypeId = input.TaskTypeId;
            task.EstimateMinutes = input.EstimateMinutes;
            task.DefaultDueDate = input.DefaultDueDate;

            // Add recurrence rule if provided
            if (input.RecurrenceRule != null)
            {
                var recurrenceRule = new RecurrenceRule(
                    Enum.Parse<RecurrenceFrequency>(input.RecurrenceRule.Frequency),
                    input.RecurrenceRule.Interval,
                    input.RecurrenceRule.EndDate,
                    input.RecurrenceRule.Count,
                    input.RecurrenceRule.DaysOfWeek
                );
                task.MakeRecurring(recurrenceRule);
            }

            // Add action steps
            if (input.ActionSteps != null)
            {
                foreach (var stepDto in input.ActionSteps)
                {
                    task.AddActionStep(
                        stepDto.Title,
                        stepDto.Notes,
                        stepDto.IsRequired,
                        stepDto.Sequence
                    );
                }
            }

            // Add acceptance criteria
            if (input.AcceptanceCriteria != null)
            {
                foreach (var criterionDto in input.AcceptanceCriteria)
                {
                    task.AddAcceptanceCriterion(
                        criterionDto.Description,
                        criterionDto.Priority,
                        criterionDto.GivenContext,
                        criterionDto.WhenAction,
                        criterionDto.ThenOutcome
                    );
                }
            }

            await _taskRepository.InsertAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.Update)]
        public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto input)
        {
            var task = await _taskRepository.GetAsync(id);

            // Use domain manager for updates
            await _taskManager.UpdateAsync(
                id,
                input.Title,
                input.Description,
                input.DefaultImportance,
                input.DefaultUrgency
            );

            // Update other properties
            if (input.Notes != null)
            {
                task.Notes = input.Notes;
            }

            if (input.ExpectedResult != null)
            {
                task.ExpectedResult = input.ExpectedResult;
            }

            if (input.DefaultContextId.HasValue)
            {
                task.DefaultContextId = input.DefaultContextId;
            }

            if (input.DefaultAreaId.HasValue)
            {
                task.DefaultAreaId = input.DefaultAreaId;
            }

            if (input.DefaultSprintId.HasValue)
            {
                task.DefaultSprintId = input.DefaultSprintId;
            }

            if (input.TaskTypeId.HasValue)
            {
                task.TaskTypeId = input.TaskTypeId;
            }

            if (input.EstimateMinutes.HasValue)
            {
                task.EstimateMinutes = input.EstimateMinutes;
            }

            if (input.DefaultDueDate.HasValue)
            {
                task.DefaultDueDate = input.DefaultDueDate;
            }

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            _taskManager.DeleteAsync(id, hardDelete: false);
        }

        [Authorize(TasksPermissions.Tasks.ManageActionSteps)]
        public async Task<TaskDto> AddActionStepAsync(Guid id, CreateTaskActionStepDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.AddActionStep(
                input.Title,
                input.Notes,
                input.IsRequired,
                input.Sequence
            );

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.ManageActionSteps)]
        public async Task<TaskDto> UpdateActionStepAsync(Guid id, Guid stepId, CreateTaskActionStepDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            var step = task.ActionSteps.FirstOrDefault(s => s.Id == stepId);
            if (step == null)
            {
                throw new BusinessException("Tasks:ActionStepNotFound");
            }

            step.Update(input.Title, input.Notes, input.IsRequired);
            await _taskRepository.UpdateAsync(task);
            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.ManageActionSteps)]
        public async Task<TaskDto> RemoveActionStepAsync(Guid id, Guid stepId)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.RemoveActionStep(stepId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.ManageActionSteps)]
        public async Task<TaskDto> ReorderActionStepsAsync(Guid id, Guid[] orderedStepIds)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.ReorderActionSteps(orderedStepIds.ToList());

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.ManageCriteria)]
        public async Task<TaskDto> AddAcceptanceCriterionAsync(Guid id, CreateAcceptanceCriterionDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.AddAcceptanceCriterion(
                input.Description,
                input.Priority,
                input.GivenContext,
                input.WhenAction,
                input.ThenOutcome
            );

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.ManageCriteria)]
        public async Task<TaskDto> UpdateAcceptanceCriterionAsync(Guid id, Guid criterionId,
            CreateAcceptanceCriterionDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            var criterion = task.AcceptanceCriteria.FirstOrDefault(c => c.Id == criterionId);
            if (criterion == null)
            {
                throw new BusinessException("Tasks:AcceptanceCriterionNotFound");
            }

            criterion.Update(
                input.Description,
                input.Priority,
                input.GivenContext,
                input.WhenAction,
                input.ThenOutcome,
                input.TestScenario
            );

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.ManageCriteria)]
        public async Task<TaskDto> RemoveAcceptanceCriterionAsync(Guid id, Guid criterionId)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.RemoveAcceptanceCriterion(criterionId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.Update)]
        public async Task<TaskDto> ActivateAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.Activate();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.Update)]
        public async Task<TaskDto> DeactivateAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.Deactivate();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.Create)]
        public async Task<TaskDto> CloneAsync(Guid id, CloneTaskDto input)
        {
            var originalTask = await _taskRepository.GetWithDetailsAsync(id);

            var clonedTask = await _taskManager.CreateAsync(
                CurrentUser.Id.Value,
                input.NewTitle,
                input.NewProjectId ?? originalTask.ProjectId,
                originalTask.Description,
                originalTask.DefaultImportance,
                originalTask.DefaultUrgency
            );

            // Copy properties
            clonedTask.Notes = originalTask.Notes;
            clonedTask.ExpectedResult = originalTask.ExpectedResult;
            clonedTask.DefaultContextId = originalTask.DefaultContextId;
            clonedTask.DefaultAreaId = originalTask.DefaultAreaId;
            clonedTask.TaskTypeId = originalTask.TaskTypeId;
            clonedTask.EstimateMinutes = originalTask.EstimateMinutes;

            // Clone action steps if requested
            if (input.IncludeActionSteps)
            {
                foreach (var step in originalTask.ActionSteps.OrderBy(s => s.Sequence))
                {
                    clonedTask.AddActionStep(step.Title, step.Notes, step.IsRequired, step.Sequence);
                }
            }

            // Clone acceptance criteria if requested
            if (input.IncludeAcceptanceCriteria)
            {
                foreach (var criterion in originalTask.AcceptanceCriteria)
                {
                    clonedTask.AddAcceptanceCriterion(
                        criterion.Description,
                        criterion.Priority,
                        criterion.GivenContext,
                        criterion.WhenAction,
                        criterion.ThenOutcome
                    );
                }
            }

            await _taskRepository.InsertAsync(clonedTask);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(clonedTask);
        }

        [Authorize(TasksPermissions.Tasks.Update)]
        public async Task<TaskDto> SetRecurrenceAsync(Guid id, RecurrenceRuleDto input)
        {
            var task = await _taskRepository.GetAsync(id);

            var recurrenceRule = new RecurrenceRule(
                Enum.Parse<RecurrenceFrequency>(input.Frequency),
                input.Interval,
                input.EndDate,
                input.Count,
                input.DaysOfWeek
            );

            task.MakeRecurring(recurrenceRule);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(TasksPermissions.Tasks.Update)]
        public async Task<TaskDto> RemoveRecurrenceAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.RemoveRecurrence();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        public async Task<TaskStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            var instances = await _taskInstanceRepository.GetListAsync(
                new BaseSpecification<TaskInstance>(ti => ti.TaskId == id)
            );

            var completedInstances = instances.Where(ti => ti.IsCompleted).ToList();

            return new TaskStatisticsDto
            {
                TotalInstances = instances.Count,
                ActiveInstances = instances.Count(ti => !ti.IsCompleted && !ti.IsCancelled),
                CompletedInstances = completedInstances.Count,
                CancelledInstances = instances.Count(ti => ti.IsCancelled),
                AverageCompletionTimeMinutes = completedInstances.Any()
                    ? completedInstances.Average(ti => ti.ActualMinutes)
                    : 0,
                LastInstanceCreatedAt = instances.OrderByDescending(ti => ti.CreationTime)
                    .FirstOrDefault()?.CreationTime
            };
        }

        private ISpecification<Aggregates.Task> CreateListSpecification(GetTaskListDto input)
        {
            // Start with "always true" specification
            ISpecification<Aggregates.Task> spec = new BaseSpecification<Aggregates.Task>(t => true);

            if (input.OwnerId.HasValue)
            {
                spec = spec.And(new TasksByOwnerSpecification(input.OwnerId.Value));
            }

            if (input.ProjectId.HasValue)
            {
                spec = spec.And(new TasksByProjectSpecification(input.ProjectId.Value));
            }

            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = spec.And(new ActiveTasksSpecification());
                }
                else
                {
                    spec = spec.And(new BaseSpecification<Aggregates.Task>(t => !t.IsActive));
                }
            }

            if (input.IsRecurring.HasValue)
            {
                if (input.IsRecurring.Value)
                {
                    spec = spec.And(new RecurringTasksSpecification());
                }
                else
                {
                    spec = spec.And(new BaseSpecification<Aggregates.Task>(t => !t.IsRecurring));
                }
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                var searchTerm = input.SearchTerm.ToLower();
                spec = spec.And(new BaseSpecification<Aggregates.Task>(t =>
                    t.Title.ToLower().Contains(searchTerm) ||
                    (t.Description != null && t.Description.ToLower().Contains(searchTerm))
                ));
            }

            return spec;
        }
    }
}