// test/NouAssist.Workflow.Domain.Tests/Services/WorkflowValidationServiceTests.cs

using System;
using NouAssist.Workflow.Entities;
using NouAssist.Workflow.Enums;
using Shouldly;
using Xunit;

namespace NouAssist.Workflow.Services
{
    public class WorkflowValidationServiceTests
    {
        private readonly WorkflowValidationService _service;

        public WorkflowValidationServiceTests()
        {
            _service = new WorkflowValidationService();
        }

        [Fact]
        public void Should_Validate_Valid_Workflow()
        {
            // Arrange
            var workflow = CreateValidWorkflow();

            // Act
            var result = _service.ValidateWorkflow(workflow);

            // Assert
            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        }

        [Fact]
        public void Should_Fail_Workflow_Without_Statuses()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "Empty Workflow",
                Guid.NewGuid());

            // Act
            var result = _service.ValidateWorkflow(workflow);

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.Contains("at least one status"));
        }

        [Fact]
        public void Should_Warn_About_No_Terminal_Status()
        {
            // Arrange
            var workflow = new Aggregates.Workflow(
                Guid.NewGuid(),
                "No Terminal Workflow",
                Guid.NewGuid());

            var status = new WorkflowStatus(
                Guid.NewGuid(),
                workflow.Id,
                "In Progress",
                WorkflowStatusCategory.InProgress,
                isInitial: true);

            workflow.AddStatus(status);

            // Act
            var result = _service.ValidateWorkflow(workflow);

            // Assert
            result.Warnings.ShouldContain(w => w.Contains("terminal status"));
        }

        private Aggregates.Workflow CreateValidWorkflow()
        {
            var workflowId = Guid.NewGuid();
            var initialStatusId = Guid.NewGuid();

            var workflow = new Aggregates.Workflow(
                workflowId,
                "Valid Workflow",
                initialStatusId);

            var todoStatus = new WorkflowStatus(
                initialStatusId,
                workflowId,
                "To Do",
                WorkflowStatusCategory.Todo,
                isInitial: true);

            var doneStatus = new WorkflowStatus(
                Guid.NewGuid(),
                workflowId,
                "Done",
                WorkflowStatusCategory.Done);

            workflow.AddStatus(todoStatus);
            workflow.AddStatus(doneStatus);

            var transition = new WorkflowTransition(
                Guid.NewGuid(),
                workflowId,
                "Complete",
                todoStatus.Id,
                doneStatus.Id);

            workflow.AddTransition(transition);

            return workflow;
        }
    }
}