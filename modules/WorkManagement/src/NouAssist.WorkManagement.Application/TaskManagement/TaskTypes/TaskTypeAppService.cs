using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.SharedKernel.Specifications;
using NouAssist.Tasks.Aggregates;
using NouAssist.Tasks.Data;
using NouAssist.Tasks.Managers;
using NouAssist.Tasks.Permissions;
using NouAssist.Tasks.Specifications;
using NouAssist.Tasks.TaskTypes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;
using Task = System.Threading.Tasks.Task;

namespace NouAssist.Tasks.TaskTypes
{
    [Authorize(TasksPermissions.TaskTypes.Default)]
    public class TaskTypeAppService : ApplicationService, ITaskTypeAppService
    {
        private readonly ITaskTypeRepository _taskTypeRepository;
        private readonly TaskTypeManager _taskTypeManager;

        public TaskTypeAppService(
            ITaskTypeRepository taskTypeRepository,
            TaskTypeManager taskTypeManager)
        {
            _taskTypeRepository = taskTypeRepository;
            _taskTypeManager = taskTypeManager;
        }

        public async Task<TaskTypeDto> GetAsync(Guid id)
        {
            var taskType = await _taskTypeRepository.GetWithCustomFieldsAsync(id);
            return ObjectMapper.Map<TaskType, TaskTypeDto>(taskType);
        }

        public async Task<PagedResultDto<TaskTypeDto>> GetListAsync(GetTaskTypeListDto input)
        {
            var spec = CreateListSpecification(input);

            var taskTypes = await _taskTypeRepository.GetListAsync(spec.ToExpression());
            var totalCount = await _taskTypeRepository.CountAsync(spec.ToExpression());

            // Apply sorting and paging
            var query = taskTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Sorting))
            {
                query = query.OrderBy(input.Sorting);
            }
            else
            {
                query = query.OrderBy(tt => tt.Name);
            }

            var pagedTypes = query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<TaskTypeDto>(
                totalCount,
                ObjectMapper.Map<List<TaskType>, List<TaskTypeDto>>(pagedTypes)
            );
        }

        public async Task<ListResultDto<TaskTypeDto>> GetActiveListAsync()
        {
            var taskTypes = await _taskTypeRepository.GetActiveTaskTypesAsync();
            return new ListResultDto<TaskTypeDto>(
                ObjectMapper.Map<List<TaskType>, List<TaskTypeDto>>(taskTypes)
            );
        }

        [Authorize(TasksPermissions.TaskTypes.Create)]
        public async Task<TaskTypeDto> CreateAsync(CreateTaskTypeDto input)
        {
            var taskType = await _taskTypeManager.CreateAsync(
                input.Name,
                input.Description,
                input.Color
            );

            await _taskTypeRepository.InsertAsync(taskType);

            return ObjectMapper.Map<TaskType, TaskTypeDto>(taskType);
        }

        [Authorize(TasksPermissions.TaskTypes.Update)]
        public async Task<TaskTypeDto> UpdateAsync(Guid id, UpdateTaskTypeDto input)
        {
            var taskType = await _taskTypeRepository.GetAsync(id);

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                taskType.UpdateName(input.Name);
            }

            if (input.Description != null)
            {
                taskType.UpdateDescription(input.Description);
            }

            if (!string.IsNullOrWhiteSpace(input.Color))
            {
                taskType.UpdateColor(input.Color);
            }

            await _taskTypeRepository.UpdateAsync(taskType);

            return ObjectMapper.Map<TaskType, TaskTypeDto>(taskType);
        }

        [Authorize(TasksPermissions.TaskTypes.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _taskTypeRepository.DeleteAsync(id);
        }

        [Authorize(TasksPermissions.TaskTypes.Update)]
        public async Task<TaskTypeDto> ActivateAsync(Guid id)
        {
            var taskType = await _taskTypeRepository.GetAsync(id);

            taskType.Activate();

            await _taskTypeRepository.UpdateAsync(taskType);

            return ObjectMapper.Map<TaskType, TaskTypeDto>(taskType);
        }

        [Authorize(TasksPermissions.TaskTypes.Update)]
        public async Task<TaskTypeDto> DeactivateAsync(Guid id)
        {
            var taskType = await _taskTypeRepository.GetAsync(id);

            taskType.Deactivate();

            await _taskTypeRepository.UpdateAsync(taskType);

            return ObjectMapper.Map<TaskType, TaskTypeDto>(taskType);
        }

        private ISpecification<TaskType> CreateListSpecification(GetTaskTypeListDto input)
        {
            // Start with "always true" specification
            ISpecification<TaskType> spec = new BaseSpecification<TaskType>(tt => true);

            if (input.IsActive.HasValue)
            {
                if (input.IsActive.Value)
                {
                    spec = spec.And(new ActiveTaskTypesSpecification());
                }
                else
                {
                    spec = spec.And(new BaseSpecification<TaskType>(tt => !tt.IsActive));
                }
            }

            if (!string.IsNullOrWhiteSpace(input.SearchTerm))
            {
                var searchTerm = input.SearchTerm.ToLower();
                spec = spec.And(new BaseSpecification<TaskType>(tt =>
                    tt.Name.ToLower().Contains(searchTerm) ||
                    (tt.Description != null && tt.Description.ToLower().Contains(searchTerm))
                ));
            }

            return spec;
        }
    }
}
