using System;
using NouAssist.Tasks.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Tasks.Aggregates
{
    public class TaskInstanceTests
    {
        [Fact]
        public void Delegate_Should_SetDelegationInfo()
        {
            // Arrange
            var instance = CreateTaskInstance();
            var delegatedBy = Guid.NewGuid();
            var delegatedTo = Guid.NewGuid();

            // Act
            instance.Delegate(delegatedBy, delegatedTo, "Please handle this");

            // Assert
            instance.IsDelegated.ShouldBeTrue();
            instance.DelegationInfo.DelegatedBy.ShouldBe(delegatedBy);
            instance.DelegationInfo.DelegatedTo.ShouldBe(delegatedTo);
            instance.DelegationInfo.Status.ShouldBe(DelegationStatus.Delegated);
            instance.AssigneeId.ShouldBe(delegatedTo);
        }

        [Fact]
        public void Delegate_Should_Fail_WhenDelegatingToSelf()
        {
            // Arrange
            var instance = CreateTaskInstance();
            var userId = Guid.NewGuid();

            // Act & Assert
            Should.Throw<BusinessException>(() =>
                instance.Delegate(userId, userId));
        }

        [Fact]
        public void AcceptDelegation_Should_UpdateStatus()
        {
            // Arrange
            var instance = CreateTaskInstance();
            var delegatedBy = Guid.NewGuid();
            var delegatedTo = Guid.NewGuid();
            instance.Delegate(delegatedBy, delegatedTo);

            // Act
            instance.AcceptDelegation(delegatedTo, "Will do");

            // Assert
            instance.DelegationInfo.Status.ShouldBe(DelegationStatus.Accepted);
            instance.TaskStatus.ShouldBe(WorkflowStatusCategory.InProgress);
        }

        [Fact]
        public void Block_Should_SetBlockedState()
        {
            // Arrange
            var instance = CreateTaskInstance();

            // Act
            instance.Block("Waiting for requirements");

            // Assert
            instance.IsBlocked.ShouldBeTrue();
            instance.BlockedReason.ShouldBe("Waiting for requirements");
            instance.BlockedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Start_Should_Fail_WhenBlocked()
        {
            // Arrange
            var instance = CreateTaskInstance();
            instance.Block("Waiting");

            // Act & Assert
            Should.Throw<BusinessException>(() =>
                instance.Start(Guid.NewGuid()));
        }

        [Fact]
        public void UpdatePriority_Should_RecalculateQuadrant()
        {
            // Arrange
            var instance = CreateTaskInstance();

            // Act
            instance.UpdatePriority(0.9, 0.9);

            // Assert
            instance.PriorityMatrix.Importance.ShouldBe(0.9);
            instance.PriorityMatrix.Urgency.ShouldBe(0.9);
            instance.PriorityMatrix.Quadrant.ShouldBe(EisenhowerQuadrant.Q1DoFirst);
        }

        private TaskInstance CreateTaskInstance()
        {
            return new TaskInstance(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                0.5,
                0.5);
        }
    }
}
