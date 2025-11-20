using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NouAssist.BusinessOperations.ProjectExecution.Enums;
using NouAssist.BusinessOperations.ProjectExecution.Permissions;
using NouAssist.BusinessOperations.ProjectExecution.Projects.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Repositories;
using NouAssist.BusinessOperations.ProjectExecution.Reports.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.Services.Interfaces;
using NouAssist.BusinessOperations.ProjectExecution.Specifications;
using NouAssist.BusinessOperations.ProjectExecution.Tasks.Dtos;
using NouAssist.BusinessOperations.ProjectExecution.TimeLogs.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.BusinessOperations.ProjectExecution.Reports
{
    [Authorize(ProjectExecutionPermissions.Reports.Default)]
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProgressTrackingService _progressTrackingService;
        
        public ReportAppService(
            IProjectRepository projectRepository,
            ITaskRepository taskRepository,
            ITimeLogRepository timeLogRepository,
            IProgressTrackingService progressTrackingService)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _timeLogRepository = timeLogRepository;
            _progressTrackingService = progressTrackingService;
        }
        
        [Authorize(ProjectExecutionPermissions.Reports.ViewProjectReports)]
        public async Task<ProjectReportDto> GenerateProjectReportAsync(Guid projectId)
        {
            // Get project with details
            var project = await _projectRepository.GetWithDetailsAsync(projectId);
            
            if (project == null)
            {
                throw new BusinessException($"Project with id {projectId} not found");
            }
            
            // Get tasks
            var tasks = await _taskRepository.GetListByProjectIdAsync(projectId);
            
            // Create base report object
            var report = new ProjectReportDto
            {
                Id = project.Id,
                Name = project.Details.Name,
                Description = project.Details.Description,
                StartDate = project.Details.StartDate,
                PlannedEndDate = project.Details.PlannedEndDate,
                Status = project.Status,
                ActualEndDate = project.ActualEndDate,
                CompletionPercentage = await _progressTrackingService.GetProjectCompletionPercentageAsync(projectId),
                TotalEstimatedHours = tasks.Sum(t => t.Details.EstimatedHours),
                TotalLoggedHours = await _timeLogRepository.GetTotalHoursByProjectIdAsync(projectId),
                TaskCountByStatus = tasks.GroupBy(t => t.Status)
                    .ToDictionary(g => g.Key, g => g.Count()),
                Milestones = ObjectMapper.Map<List<ProjectMilestone>, List<ProjectMilestoneDto>>(project.Milestones.ToList())
            };
            
            // Get time log data
            var timeLogs = await _timeLogRepository.GetListByProjectIdAsync(
                projectId, 
                project.Details.StartDate, 
                project.ActualEndDate ?? Clock.Now);
                
            // Hours by user
            var totalHours = timeLogs.Sum(tl => tl.Details.Hours);
            
            report.HoursByUser = timeLogs
                .GroupBy(tl => tl.UserId)
                .Select(g => new TimeLogUserSummaryDto
                {
                    UserId = g.Key,
                    Hours = g.Sum(tl => tl.Details.Hours),
                    Percentage = totalHours > 0 ? g.Sum(tl => tl.Details.Hours) / totalHours * 100 : 0,
                    UserName = $"User {g.Key}" // Would use a user service in a real implementation
                })
                .ToList();
                
            // Hours by day - group by day and calculate sum
            report.HoursByDay = timeLogs
                .GroupBy(tl => tl.Details.LogDate.Date)
                .Select(g => new TimeLogDailySummaryDto
                {
                    Date = g.Key,
                    Hours = g.Sum(tl => tl.Details.Hours)
                })
                .OrderBy(d => d.Date)
                .ToList();
                
            // Get overdue tasks
            var overdueSpecification = new OverdueTaskSpecification(Clock.Now);
            var overdueTasksQuery = tasks.AsQueryable().Where(overdueSpecification.ToExpression());
            var overdueTasks = overdueTasksQuery.ToList();
            
            report.OverdueTasks = overdueTasks.Select(t => new TaskListDto
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                ProjectName = project.Details.Name,
                PhaseId = t.PhaseId,
                PhaseName = t.PhaseId.HasValue ? 
                    project.Phases.FirstOrDefault(p => p.GetKeys().Contains(t.PhaseId.Value))?.Name : null,
                Name = t.Details.Name,
                EstimatedHours = t.Details.EstimatedHours,
                Priority = t.Details.Priority,
                Status = t.Status,
                DueDate = t.DueDate,
                IsOverdue = true
            }).ToList();
            
            // Get upcoming milestones (not completed and due within next 30 days)
            var today = Clock.Now.Date;
            report.UpcomingMilestones = project.Milestones
                .Where(m => !m.IsCompleted && m.TargetDate >= today && m.TargetDate <= today.AddDays(30))
                .Select(m => ObjectMapper.Map<ProjectMilestone, ProjectMilestoneDto>(m))
                .ToList();
                
            return report;
        }
        
        [Authorize(ProjectExecutionPermissions.Reports.ViewResourceReports)]
        public async Task<ResourceUtilizationReportDto> GenerateResourceUtilizationReportAsync(DateTime startDate, DateTime endDate)
        {
            // Get all time logs within date range
            var allTimeLogs = await AsyncExecuter.ToListAsync(
                (await _timeLogRepository.GetQueryableAsync())
                .Where(tl => tl.Details.LogDate >= startDate && tl.Details.LogDate <= endDate));
                
            // Workdays calculation (for capacity)
            var workDayCount = CountWorkDays(startDate, endDate);
            var standardDailyHours = 8.0; // Standard 8-hour workday
            var standardCapacity = workDayCount * standardDailyHours;
            
            // Group time logs by user
            var userUtilizations = allTimeLogs
                .GroupBy(tl => tl.UserId)
                .Select(g => 
                {
                    var userTotalHours = g.Sum(tl => tl.Details.Hours);
                    
                    // Group by project for this user
                    var projectBreakdown = g.GroupBy(tl => tl.ProjectId)
                        .Select(pg => new ProjectUtilizationDto
                        {
                            ProjectId = pg.Key,
                            Hours = pg.Sum(tl => tl.Details.Hours),
                            Percentage = (pg.Sum(tl => tl.Details.Hours) / userTotalHours) * 100
                        })
                        .ToList();
                        
                    return new UserUtilizationDto
                    {
                        UserId = g.Key,
                        UserName = $"User {g.Key}", // Would use a user service in a real implementation
                        TotalHours = userTotalHours,
                        Capacity = standardCapacity,
                        UtilizationPercentage = (userTotalHours / standardCapacity) * 100,
                        ProjectBreakdown = projectBreakdown
                    };
                })
                .ToList();
                
            // Get project names
            foreach (var user in userUtilizations)
            {
                foreach (var project in user.ProjectBreakdown)
                {
                    var projectEntity = await _projectRepository.GetAsync(project.ProjectId);
                    project.ProjectName = projectEntity.Details.Name;
                }
            }
            
            return new ResourceUtilizationReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                UserUtilizations = userUtilizations
            };
        }
        
        private int CountWorkDays(DateTime startDate, DateTime endDate)
        {
            int workDays = 0;
            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDays++;
                }
            }
            return workDays;
        }
    }
}
