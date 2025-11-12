using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.WorkManagement.Permissions;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Events.Dtos;
using Volo.Abp.Application.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Events
{
    [Authorize(WorkManagementPermissions.ProjectManagement.Events.Default)]
    public class ProjectEventAppService : ApplicationService, IProjectEventAppService
    {
        private readonly IProjectEventRepository _eventRepository;

        public ProjectEventAppService(IProjectEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<ProjectEventDto> GetAsync(Guid id)
        {
            var projectEvent = await _eventRepository.GetAsync(id);
            return ObjectMapper.Map<ProjectEvent, ProjectEventDto>(projectEvent);
        }

        public async Task<List<ProjectEventDto>> GetByProjectAsync(Guid projectId)
        {
            var events = await _eventRepository.GetByProjectIdAsync(projectId);
            return ObjectMapper.Map<List<ProjectEvent>, List<ProjectEventDto>>(events);
        }

        public async Task<List<ProjectEventDto>> GetUpcomingEventsAsync(Guid projectId, int days = 30)
        {
            var fromDate = DateTime.UtcNow;
            var events = await _eventRepository.GetUpcomingEventsAsync(projectId, fromDate);
            return ObjectMapper.Map<List<ProjectEvent>, List<ProjectEventDto>>(events);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Events.Create)]
        public async Task<ProjectEventDto> CreateAsync(CreateEventDto input)
        {
            var projectEvent = new ProjectEvent(
                GuidGenerator.Create(),
                input.ProjectId,
                input.Title,
                input.EventType,
                input.EventDate,
                input.Description);

            if (input.EndDate.HasValue)
            {
                projectEvent.EndDate = input.EndDate;
            }

            projectEvent.IsAllDay = input.IsAllDay;

            if (!string.IsNullOrWhiteSpace(input.Location))
            {
                projectEvent.Location = input.Location;
            }

            projectEvent.RelatedSprintId = input.RelatedSprintId;
            projectEvent.RelatedMilestoneId = input.RelatedMilestoneId;

            await _eventRepository.InsertAsync(projectEvent);

            return ObjectMapper.Map<ProjectEvent, ProjectEventDto>(projectEvent);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Events.Update)]
        public async Task<ProjectEventDto> UpdateAsync(Guid id, UpdateEventDto input)
        {
            var projectEvent = await _eventRepository.GetAsync(id);
            
            projectEvent.Title = input.Title;
            projectEvent.Description = input.Description;
            projectEvent.EventDate = input.EventDate;
            projectEvent.EndDate = input.EndDate;
            projectEvent.IsAllDay = input.IsAllDay;
            projectEvent.Location = input.Location;
            
            await _eventRepository.UpdateAsync(projectEvent);

            return ObjectMapper.Map<ProjectEvent, ProjectEventDto>(projectEvent);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Events.Update)]
        public async Task<ProjectEventDto> RescheduleAsync(Guid id, RescheduleEventDto input)
        {
            var projectEvent = await _eventRepository.GetAsync(id);
            
            projectEvent.Reschedule(input.NewEventDate);
            
            if (input.NewEndDate.HasValue)
            {
                projectEvent.EndDate = input.NewEndDate;
            }
            
            await _eventRepository.UpdateAsync(projectEvent);

            return ObjectMapper.Map<ProjectEvent, ProjectEventDto>(projectEvent);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Events.Update)]
        public async Task<ProjectEventDto> MarkAsCompletedAsync(Guid id)
        {
            var projectEvent = await _eventRepository.GetAsync(id);
            
            projectEvent.MarkAsCompleted();
            
            await _eventRepository.UpdateAsync(projectEvent);

            return ObjectMapper.Map<ProjectEvent, ProjectEventDto>(projectEvent);
        }

        [Authorize(WorkManagementPermissions.ProjectManagement.Events.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _eventRepository.DeleteAsync(id);
        }
    }
}
