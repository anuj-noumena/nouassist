using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.TaskManagement.Aggregates;
using NouAssist.WorkManagement.TaskManagement.Data;
using NouAssist.WorkManagement.TaskManagement.Managers;
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
    [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Default)]
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

            var query = tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
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

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Create)]
        public async Task<TaskDto> CreateAsync(CreateTaskDto input)
        {
            var task = await _taskManager.CreateAsync(
                CurrentUser.Id.Value,
                input.Title,
                CurrentTenant.Id,
                input.ProjectId,
                input.AreaId,
                input.ContextId,
                input.DefaultImportance,
                input.DefaultUrgency
            );

            task.Description = input.Description;
            task.ExpectedResult = input.ExpectedResult;
            task.Notes = input.Notes;
            task.DefaultContextId = input.DefaultContextId;
            task.DefaultAreaId = input.DefaultAreaId;
            task.TaskTypeId = input.TaskTypeId;
            task.DefaultEstimateMinutes = input.DefaultEstimateMinutes;
            task.DefaultDueDate = input.DefaultDueDate;
            task.DefaultWorkflowId = input.DefaultWorkflowId;
            task.DefaultAssigneeId = input.DefaultAssigneeId;

            if (input.DefaultComplexityEstimation != null)
            {
                var complexityEstimation = new ComplexityEstimation(
                    input.DefaultComplexityEstimation.Value,
                    input.DefaultComplexityEstimation.Scale
                );
                task.SetDefaultComplexityEstimation(complexityEstimation);
            }

            if (input.DefaultCognitiveLoad.HasValue)
            {
                task.SetDefaultCognitiveLoad(input.DefaultCognitiveLoad.Value);
            }

            if (input.RecurrenceRule != null)
            {
                var recurrenceRule = new RecurrenceRule(
                    Enum.Parse<RecurrenceFrequency>(input.RecurrenceRule.Frequency),
                    input.RecurrenceRule.Interval,
                    input.RecurrenceRule.EndDate,
                    input.RecurrenceRule.Count,
                    input.RecurrenceRule.DaysOfWeek
                );
                task.SetRecurrence(recurrenceRule);
            }

            // Add checklist items
            if (input.ChecklistItems != null)
            {
                foreach (var itemDto in input.ChecklistItems)
                {
                    task.AddChecklistItem(itemDto.Title, itemDto.IsRequired);
                }
            }

            // Add acceptance criteria
            if (input.AcceptanceCriteria != null)
            {
                foreach (var criterionDto in input.AcceptanceCriteria)
                {
                    task.AddAcceptanceCriterion(
                        criterionDto.Description,
                        criterionDto.Priority
                    );
                }
            }

            await _taskRepository.InsertAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto input)
        {
            var task = await _taskRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Title))
            {
                task.UpdateTitle(input.Title);
            }

            if (input.Description != null)
            {
                task.Description = input.Description;
            }

            if (input.ExpectedResult != null)
            {
                task.ExpectedResult = input.ExpectedResult;
            }

            if (input.Notes != null)
            {
                task.Notes = input.Notes;
            }

            if (input.DefaultImportance.HasValue && input.DefaultUrgency.HasValue)
            {
                task.SetDefaults(input.DefaultImportance.Value, input.DefaultUrgency.Value);
            }

            if (input.DefaultContextId.HasValue)
            {
                task.DefaultContextId = input.DefaultContextId;
            }

            if (input.DefaultAreaId.HasValue)
            {
                task.DefaultAreaId = input.DefaultAreaId;
            }

            if (input.TaskTypeId.HasValue)
            {
                task.TaskTypeId = input.TaskTypeId;
            }

            if (input.DefaultEstimateMinutes.HasValue)
            {
                task.DefaultEstimateMinutes = input.DefaultEstimateMinutes;
            }

            if (input.DefaultDueDate.HasValue)
            {
                task.DefaultDueDate = input.DefaultDueDate;
            }

            if (input.DefaultComplexityEstimation != null)
            {
                var complexityEstimation = new ComplexityEstimation(
                    input.DefaultComplexityEstimation.Value,
                    input.DefaultComplexityEstimation.Scale
                );
                task.SetDefaultComplexityEstimation(complexityEstimation);
            }

            if (input.DefaultCognitiveLoad.HasValue)
            {
                task.SetDefaultCognitiveLoad(input.DefaultCognitiveLoad.Value);
            }

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _taskRepository.DeleteAsync(id);
        }

        // CHECKLIST ITEMS (Updated from ActionSteps)
        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageChecklistItems)]
        public async Task<TaskDto> AddChecklistItemAsync(Guid id, CreateChecklistItemDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.AddChecklistItem(input.Title, input.IsRequired);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageChecklistItems)]
        public async Task<TaskDto> UpdateChecklistItemAsync(Guid id, Guid itemId, CreateChecklistItemDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            var item = task.ChecklistItems.FirstOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                throw new BusinessException("Tasks:ChecklistItemNotFound");
            }

            // Checklist items are simple entities, no Update method in the domain
            // This would require adding an Update method to ChecklistItem entity
            // For now, remove and re-add (or add Update method to entity)
            
            await _taskRepository.UpdateAsync(task);
            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageChecklistItems)]
        public async Task<TaskDto> RemoveChecklistItemAsync(Guid id, Guid itemId)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.RemoveChecklistItem(itemId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageChecklistItems)]
        public async Task<TaskDto> ReorderChecklistItemsAsync(Guid id, Guid[] orderedItemIds)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.ReorderChecklistItems(orderedItemIds.ToList());

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        // ACCEPTANCE CRITERIA
        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageCriteria)]
        public async Task<TaskDto> AddAcceptanceCriterionAsync(Guid id, CreateAcceptanceCriterionDto input)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.AddAcceptanceCriterion(input.Description, input.Priority);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageCriteria)]
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

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.ManageCriteria)]
        public async Task<TaskDto> RemoveAcceptanceCriterionAsync(Guid id, Guid criterionId)
        {
            var task = await _taskRepository.GetWithDetailsAsync(id);

            task.RemoveAcceptanceCriterion(criterionId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        // GTD ORGANIZATION
        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> AssignToAreaAsync(Guid id, Guid areaId)
        {
            var task = await _taskRepository.GetAsync(id);

            task.AssignToArea(areaId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> RemoveFromAreaAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.RemoveFromArea();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> AssignToContextAsync(Guid id, Guid contextId)
        {
            var task = await _taskRepository.GetAsync(id);

            task.AssignToContext(contextId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> RemoveFromContextAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.RemoveFromContext();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> MoveToProjectAsync(Guid id, Guid projectId)
        {
            var task = await _taskRepository.GetAsync(id);

            // This would need a workflow status - needs more context
            // For now, just update the ProjectId
            task.ProjectId = projectId;
            if (task.AreaId.HasValue)
            {
                task.RemoveFromArea();
            }

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> MoveToAreaAsync(Guid id, Guid areaId)
        {
            var task = await _taskRepository.GetAsync(id);

            task.MoveToArea(areaId);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> ActivateAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.Activate();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> DeactivateAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.Deactivate();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Create)]
        public async Task<TaskDto> CloneAsync(Guid id, CloneTaskDto input)
        {
            var originalTask = await _taskRepository.GetWithDetailsAsync(id);

            var clonedTask = await _taskManager.CreateAsync(
                CurrentUser.Id.Value,
                input.NewTitle,
                CurrentTenant.Id,
                input.NewProjectId ?? originalTask.ProjectId,
                input.NewAreaId ?? originalTask.AreaId,
                originalTask.ContextId,
                originalTask.DefaultImportance,
                originalTask.DefaultUrgency
            );

            clonedTask.Description = originalTask.Description;
            clonedTask.ExpectedResult = originalTask.ExpectedResult;
            clonedTask.DefaultContextId = originalTask.DefaultContextId;
            clonedTask.DefaultAreaId = originalTask.DefaultAreaId;
            clonedTask.TaskTypeId = originalTask.TaskTypeId;
            clonedTask.DefaultEstimateMinutes = originalTask.DefaultEstimateMinutes;

            // Clone checklist items if requested
            if (input.IncludeChecklistItems)
            {
                foreach (var item in originalTask.ChecklistItems.OrderBy(i => i.OrderIndex))
                {
                    clonedTask.AddChecklistItem(item.Title, item.IsRequired);
                }
            }

            // Clone acceptance criteria if requested
            if (input.IncludeAcceptanceCriteria)
            {
                foreach (var criterion in originalTask.AcceptanceCriteria)
                {
                    clonedTask.AddAcceptanceCriterion(criterion.Description, criterion.Priority);
                }
            }

            await _taskRepository.InsertAsync(clonedTask);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(clonedTask);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
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

            task.SetRecurrence(recurrenceRule);

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        [Authorize(WorkManagementPermissions.TaskManagement.Tasks.Update)]
        public async Task<TaskDto> RemoveRecurrenceAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            task.ClearRecurrence();

            await _taskRepository.UpdateAsync(task);

            return ObjectMapper.Map<Aggregates.Task, TaskDto>(task);
        }

        public async Task<TaskStatisticsDto> GetStatisticsAsync(Guid id)
        {
            var task = await _taskRepository.GetAsync(id);

            var instances = await _taskInstanceRepository.GetListAsync(
                new BaseSpecification<TaskInstance>(ti => ti.TaskId == id)
            );

            var completedInstances = instances.Where(ti => ti.WorkTracking.IsCompleted).ToList();

            return new TaskStatisticsDto
            {
                TotalInstances = instances.Count,
                ActiveInstances = instances.Count(ti => !ti.WorkTracking.IsCompleted),
                CompletedInstances = completedInstances.Count,
                CancelledInstances = 0, // Need to track cancellation differently
                AverageCompletionTimeMinutes = completedInstances.Any()
                    ? completedInstances.Average(ti => ti.WorkTracking.ActualMinutes)
                    : 0,
                LastInstanceCreatedAt = instances.OrderByDescending(ti => ti.CreationTime)
                    .FirstOrDefault()?.CreationTime
            };
        }

        private ISpecification<Aggregates.Task> CreateListSpecification(GetTaskListDto input)
        {
            ISpecification<Aggregates.Task> spec = new BaseSpecification<Aggregates.Task>(t => true);

            if (input.OwnerId.HasValue)
            {
                spec = spec.And(new TasksByOwnerSpecification(input.OwnerId.Value));
            }

            if (input.ProjectId.HasValue)
            {
                spec = spec.And(new TasksByProjectSpecification(input.ProjectId.Value));
            }

            // GTD Filters
            if (input.AreaId.HasValue)
            {
                spec = spec.And(new BaseSpecification<Aggregates.Task>(t => t.AreaId == input.AreaId.Value));
            }

            if (input.ContextId.HasValue)
            {
                spec = spec.And(new BaseSpecification<Aggregates.Task>(t => t.ContextId == input.ContextId.Value));
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