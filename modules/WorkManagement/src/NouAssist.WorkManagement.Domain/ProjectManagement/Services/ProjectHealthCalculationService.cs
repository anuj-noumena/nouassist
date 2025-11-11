using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Services
{
    public class ProjectHealthCalculationService : DomainService
    {
        public ProjectHealthReport CalculateProjectHealth(
            Project project,
            List<ProjectDependency> dependencies)
        {
            var report = new ProjectHealthReport
            {
                ProjectId = project.Id,
                CalculatedAt = Clock.Now,
                Status = project.Status,
                Progress = project.Progress
            };

            // Base health from project
            report.BaseHealth = project.Health;
            
            // Factor in dependencies
            var blockedDependencies = dependencies.FindAll(d => 
                d.IsBlocking && d.IsActive && !d.IsResolved);
            
            report.BlockedDependenciesCount = blockedDependencies.Count;
            report.HasCriticalBlockers = blockedDependencies.Count > 0;

            // Calculate overall health
            if (report.HasCriticalBlockers)
            {
                report.OverallHealth = ProjectHealth.Critical;
            }
            else
            {
                report.OverallHealth = project.Health;
            }

            // Add recommendations
            if (project.IsOverdue)
            {
                report.Recommendations.Add("Project is overdue. Consider revising timeline or increasing resources.");
            }

            if (blockedDependencies.Count > 0)
            {
                report.Recommendations.Add($"Resolve {blockedDependencies.Count} blocking dependencies.");
            }

            if (project.Progress < 50 && project.DaysRemaining.HasValue && project.DaysRemaining < 7)
            {
                report.Recommendations.Add("Project is behind schedule with limited time remaining.");
            }

            return report;
        }
    }

    public class ProjectHealthReport
    {
        public Guid ProjectId { get; set; }
        public DateTime CalculatedAt { get; set; }
        public ProjectStatus Status { get; set; }
        public double Progress { get; set; }
        public ProjectHealth BaseHealth { get; set; }
        public ProjectHealth OverallHealth { get; set; }
        public int BlockedDependenciesCount { get; set; }
        public bool HasCriticalBlockers { get; set; }
        public List<string> Recommendations { get; set; } = new();
    }
}
