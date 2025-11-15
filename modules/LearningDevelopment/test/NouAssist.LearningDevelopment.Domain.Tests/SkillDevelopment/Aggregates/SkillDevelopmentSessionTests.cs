using System;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Events;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.SkillDevelopment.Tests.Aggregates
{
    public class SkillDevelopmentSessionTests
    {
        [Fact]
        public void Should_CreateSession_WithInitialAssessmentStep()
        {
            // Arrange & Act
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            // Assert
            session.ShouldNotBeNull();
            session.CurrentStep.ShouldBe(DevelopmentStep.Assessment);
            session.Completed.ShouldBeFalse();
            session.Milestones.ShouldContainKey(DevelopmentStep.Assessment);
        }

        [Fact]
        public void AdvanceToNextStep_Should_MoveToPlanning_AfterAssessment()
        {
            // Arrange
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());
            
            session.CompleteCurrentStep();

            // Act
            session.AdvanceToNextStep();

            // Assert
            session.CurrentStep.ShouldBe(DevelopmentStep.Planning);
            session.Milestones.ShouldContainKey(DevelopmentStep.Planning);
        }

        [Fact]
        public void AdvanceToNextStep_Should_ThrowException_WhenStepNotCompleted()
        {
            // Arrange
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            // Act & Assert
            Should.Throw<BusinessException>(() => session.AdvanceToNextStep())
                .Code.ShouldBe("SkillDevelopment:CurrentStepNotCompleted");
        }

        [Fact]
        public void AdvanceToNextStep_Should_CompleteSession_AfterMasteryStep()
        {
            // Arrange
            var session = CreateSessionAtMasteryStep();
            session.CompleteCurrentStep();

            // Act
            session.AdvanceToNextStep();

            // Assert
            session.Completed.ShouldBeTrue();
            session.OverallProficiencyGain.ShouldNotBeNull();
        }

        [Fact]
        public void Pause_Should_SetPausedState()
        {
            // Arrange
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            // Act
            session.Pause();

            // Assert
            session.Paused.ShouldBeTrue();
            session.PausedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Resume_Should_ClearPausedState()
        {
            // Arrange
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());
            
            session.Pause();

            // Act
            session.Resume();

            // Assert
            session.Paused.ShouldBeFalse();
            session.ResumedAt.ShouldNotBeNull();
        }

        #region Test Helpers

        private SkillDevelopmentSession CreateSessionAtMasteryStep()
        {
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid());

            // Advance through all steps to Mastery
            for (int i = 0; i < 5; i++)
            {
                session.CompleteCurrentStep();
                session.AdvanceToNextStep();
            }

            return session;
        }

        #endregion
    }
}
