using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using NouAssist.Tasks.Aggregates;
using Shouldly;
using Xunit;

namespace NouAssist.Tasks.Services
{
    public class TaskInstanceProcessingServiceTests
    {
        private readonly TaskInstanceProcessingService _service;

        public TaskInstanceProcessingServiceTests()
        {
            _service = new TaskInstanceProcessingService(NullLogger<TaskInstanceProcessingService>.Instance);
        }

        [Fact]
        public void CreateTaskInstanceFromBlueprint_Should_CreateValidInstance()
        {
            // Arrange
            var task = CreateTaskBlueprint();
            var projectId = Guid.NewGuid();
            var statusId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();

            // Act
            var instance = _service.CreateTaskInstanceFromBlueprint(
                task, projectId, statusId, ownerId);

            // Assert
            instance.ShouldNotBeNull();
            instance.TaskId.ShouldBe(task.Id);
            instance.ProjectId.ShouldBe(projectId);
            instance.OwnerId.ShouldBe(ownerId);
            instance.PriorityMatrix.Importance.ShouldBe(task.DefaultImportance);
            instance.PriorityMatrix.Urgency.ShouldBe(task.DefaultUrgency);
            instance.StepCompletions.Count.ShouldBe(task.ActionSteps.Count);
        }

        [Fact]
        public void ProcessTaskCompletion_Should_CompleteTask_WhenValid()
        {
            // Arrange
            var task = CreateTaskBlueprint();
            var instance = CreateTaskInstance(task);
            var userId = Guid.NewGuid();

            // Complete all steps
            foreach (var step in instance.StepCompletions)
            {
                instance.CompleteStep(step.TaskActionStepId, userId);
            }

            // Act
            var result = _service.ProcessTaskCompletion(
                instance, task, userId, "Done", "All good");

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.ProcessedTaskInstance.IsCompleted.ShouldBeTrue();
            result.ProcessedTaskInstance.Resolution.ShouldBe("Done");
        }

        [Fact]
        public void ProcessTaskCompletion_Should_Fail_WhenRequiredStepsNotCompleted()
        {
            // Arrange
            var task = CreateTaskBlueprint(addRequiredStep: true);
            var instance = CreateTaskInstance(task);
            var userId = Guid.NewGuid();

            // Don't complete any steps

            // Act
            var result = _service.ProcessTaskCompletion(
                instance, task, userId);

            // Assert
            result.IsSuccessful.ShouldBeFalse();
            result.Errors.ShouldNotBeEmpty();
        }

        #region Test Helpers

        private Task CreateTaskBlueprint(bool addRequiredStep = false)
        {
            var task = new Task(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Task",
                null,
                null,
                0.8,
                0.6);

            task.AddActionStep("Step 1", isRequired: addRequiredStep);
            task.AddActionStep("Step 2");
            task.AddAcceptanceCriterion("Criterion 1");

            return task;
        }

        private TaskInstance CreateTaskInstance(Task task)
        {
            var instance = new TaskInstance(
                Guid.NewGuid(),
                task.Id,
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                task.DefaultImportance,
                task.DefaultUrgency);

            var stepIds = new List<Guid>();
            foreach (var step in task.ActionSteps)
            {
                stepIds.Add(step.Id);
            }
            instance.InitializeStepCompletions(stepIds);

            return instance;
        }

        #endregion
    }
}
