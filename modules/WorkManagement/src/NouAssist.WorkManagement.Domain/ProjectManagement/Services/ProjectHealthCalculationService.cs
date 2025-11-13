using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.WorkManagement.ProjectManagement.Aggregates;
using NouAssist.WorkManagement.ProjectManagement.Data;
using NouAssist.WorkManagement.ProjectManagement.Entities;
using NouAssist.WorkManagement.ProjectManagement.Enums;
using Volo.Abp.Domain.Services;

namespace NouAssist.WorkManagement.ProjectManagement.Services
{
    /// <summary>
    /// IMPROVED: Better separation of concerns and testability
    /// </summary>
    public class ProjectHealthCalculationService : DomainService
    {
        // Add interfaces for better testability
        private readonly IProjectDependencyRepository _dependencyRepository;
        private readonly IProjectRepository _projectRepository;

        public ProjectHealthCalculationService(
            IProjectDependencyRepository dependencyRepository,
            IProjectRepository projectRepository)
        {
            _dependencyRepository = dependencyRepository;
            _projectRepository = projectRepository;
        }

        public async Task<ProjectHealthReport> CalculateProjectHealthAsync(Guid projectId)
        {
            var project = await _projectRepository.GetWithDetailsAsync(projectId);
            var dependencies = await _dependencyRepository.GetByProjectIdAsync(projectId);

            return CalculateProjectHealth(project, dependencies);
        }

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
            var blockedDependencies = dependencies
                .Where(d => d.IsBlocking && d.IsActive && !d.IsResolved)
                .ToList();

            report.BlockedDependenciesCount = blockedDependencies.Count;
            report.HasCriticalBlockers = blockedDependencies.Any();

            // Factor in overdue dependencies
            var overdueDependencies = dependencies
                .Where(d => d.IsOverdue)
                .ToList();

            report.OverdueDependenciesCount = overdueDependencies.Count;

            // Calculate overall health with all factors
            report.OverallHealth = CalculateOverallHealth(
                project,
                report.BlockedDependenciesCount,
                report.OverdueDependenciesCount);

            // Generate recommendations
            report.Recommendations = GenerateRecommendations(project, report);

            return report;
        }

        private ProjectHealth CalculateOverallHealth(
            Project project,
            int blockedCount,
            int overdueCount)
        {
            // Critical if has blockers or project is critical
            if (blockedCount > 0 || project.Health == ProjectHealth.Critical)
            {
                return ProjectHealth.Critical;
            }

            // At risk if has overdue dependencies or project is at risk
            if (overdueCount > 0 || project.Health == ProjectHealth.AtRisk)
            {
                return ProjectHealth.AtRisk;
            }

            return project.Health;
        }

        private List<string> GenerateRecommendations(Project project, ProjectHealthReport report)
        {
            var recommendations = new List<string>();

            if (project.IsOverdue)
            {
                recommendations.Add(
                    "Project is overdue. Consider revising timeline or increasing resources.");
            }

            if (report.BlockedDependenciesCount > 0)
            {
                recommendations.Add(
                    $"Resolve {report.BlockedDependenciesCount} blocking dependencies.");
            }

            if (report.OverdueDependenciesCount > 0)
            {
                recommendations.Add(
                    $"Address {report.OverdueDependenciesCount} overdue dependencies.");
            }

            if (project.Progress < 50 && project.Timeline.TimeRemaining.HasValue && 
                project.Timeline.TimeRemaining.Value.Days < 7)
            {
                recommendations.Add(
                    "Project is behind schedule with limited time remaining. Consider scope reduction or deadline extension.");
            }

            if (project.HasSuccessCriteria && project.SuccessCriteriaCompletionRate < 30 &&
                project.Progress > 70)
            {
                recommendations.Add(
                    "Project progress is high but success criteria completion is low. Review and update criteria.");
            }

            return recommendations;
        }
    }

    /// <summary>
    /// IMPROVED: More comprehensive health report
    /// </summary>
    public class ProjectHealthReport
    {
        public Guid ProjectId { get; set; }
        public DateTime CalculatedAt { get; set; }
        public ProjectStatus Status { get; set; }
        public double Progress { get; set; }
        
        // Health scores
        public ProjectHealth BaseHealth { get; set; }
        public ProjectHealth OverallHealth { get; set; }
        
        // Dependency metrics
        public int BlockedDependenciesCount { get; set; }
        public int OverdueDependenciesCount { get; set; }
        public bool HasCriticalBlockers { get; set; }
        
        // Success criteria metrics
        public double SuccessCriteriaCompletionRate { get; set; }
        public int UnmetRequiredCriteriaCount { get; set; }
        
        // Timeline metrics
        public bool IsOverdue { get; set; }
        public int? DaysRemaining { get; set; }
        public double TimeProgress { get; set; }
        public double WorkProgress { get; set; }
        
        // Recommendations
        public List<string> Recommendations { get; set; } = new();
        
        // Risk factors
        public List<RiskFactor> RiskFactors { get; set; } = new();
        
        public HealthScore GetHealthScore()
        {
            return OverallHealth switch
            {
                ProjectHealth.Healthy => HealthScore.Good,
                ProjectHealth.AtRisk => HealthScore.Warning,
                ProjectHealth.Critical => HealthScore.Critical,
                ProjectHealth.Completed => HealthScore.Completed,
                _ => HealthScore.Unknown
            };
        }
    }

    public class RiskFactor
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public RiskLevel Level { get; set; }
        public string Mitigation { get; set; }
    }

    public enum RiskLevel
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    public enum HealthScore
    {
        Unknown = 0,
        Good = 1,
        Warning = 2,
        Critical = 3,
        Completed = 4
    }
}