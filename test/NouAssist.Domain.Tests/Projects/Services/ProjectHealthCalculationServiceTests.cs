using System;
using System.Collections.Generic;
using NouAssist.Projects.Aggregates;
using NouAssist.Projects.Entities;
using NouAssist.Projects.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.Projects.Services
{
    public class ProjectHealthCalculationServiceTests
    {
        private readonly ProjectHealthCalculationService _service;

        public ProjectHealthCalculationServiceTests()
        {
            _service = new ProjectHealthCalculationService();
        }

        [Fact]
        public void Should_Calculate_Health_Without_Dependencies()
        {
            // Arrange
            var project = CreateActiveProject(progress: 70);
            var dependencies = new List<ProjectDependency>();

            // Act
            var report = _service.CalculateProjectHealth(project, dependencies);

            // Assert
            report.ShouldNotBeNull();
            report.ProjectId.ShouldBe(project.Id);
            report.OverallHealth.ShouldBe(ProjectHealth.Healthy);
            report.HasCriticalBlockers.ShouldBeFalse();
        }

        [Fact]
        public void Should_Identify_Critical_Blockers()
        {
            // Arrange
            var project = CreateActiveProject(progress: 70);
            var dependencies = new List<ProjectDependency>
            {
                CreateBlockingDependency(project.Id)
            };

            // Act
            var report = _service.CalculateProjectHealth(project, dependencies);

            // Assert
            report.HasCriticalBlockers.ShouldBeTrue();
            report.OverallHealth.ShouldBe(ProjectHealth.Critical);
            report.BlockedDependenciesCount.ShouldBe(1);
        }

        [Fact]
        public void Should_Add_Recommendations_For_Overdue_Project()
        {
            // Arrange
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Overdue Project",
                null,
                dueDate: DateTime.UtcNow.AddDays(-1));
            
            project.Start();

            // Act
            var report = _service.CalculateProjectHealth(project, new List<ProjectDependency>());

            // Assert
            report.Recommendations.ShouldContain(r => r.Contains("overdue"));
        }

        private static Project CreateActiveProject(double progress = 50)
        {
            var project = new Project(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Project",
                null,
                startDate: DateTime.UtcNow,
                dueDate: DateTime.UtcNow.AddDays(30));

            project.Start();
            project.UpdateProgress(progress);

            return project;
        }

        private static ProjectDependency CreateBlockingDependency(Guid projectId)
        {
            return new ProjectDependency(
                Guid.NewGuid(),
                projectId,
                Guid.NewGuid(),
                ProjectDependencyType.FinishToStart,
                null,
                isBlocking: true);
        }
    }
}
