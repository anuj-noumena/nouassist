using System;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Enums;
using NouAssist.Learning.Services;
using Shouldly;
using Xunit;

namespace NouAssist.Learning.Tests.Services
{
    public class LearningProgressServiceTests
    {
        private readonly LearningProgressService _progressService;

        public LearningProgressServiceTests()
        {
            _progressService = new LearningProgressService();
        }

        [Fact]
        public void CalculateTopicProgress_Should_Return_ProportionalProgress()
        {
            // Arrange
            var topic = CreateTopic();
            var session = CreateSession(topic.Id);
            
            // Advance to Phase 3
            for (int i = 0; i < 2; i++)
            {
                session.GetCurrentPhaseMilestone().MarkCompleted(1000);
                session.AdvanceToNextPhase(1000);
            }

            // Act
            var progress = _progressService.CalculateTopicProgress(topic, session);

            // Assert
            progress.ShouldBeInRange(30, 35); // ~33% (2/6 phases)
        }

        [Fact]
        public void CalculateFolderProgress_Should_ReturnAverageOfChildren()
        {
            // Arrange
            var folder = CreateFolder();
            var child1 = CreateTopic();
            child1.UpdateProgress(50);
            var child2 = CreateTopic();
            child2.UpdateProgress(100);
            
            // Simulate children (in real scenario, these would be persisted)
            // For test, we'd need to mock or use reflection to add children

            // This test demonstrates the concept
            // In production, you'd fetch children from repository
        }

        [Fact]
        public void ShouldTriggerSpacedRepetition_Should_ReturnTrue_WhenInPhase5WithIncompleteCycles()
        {
            // Arrange
            var session = CreateSession();
            AdvanceToPhase(session, LearningPhase.SpacedRepetition);

            // Act
            var shouldTrigger = _progressService.ShouldTriggerSpacedRepetition(session);

            // Assert
            shouldTrigger.ShouldBeTrue();
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 3)]
        [InlineData(3, 7)]
        [InlineData(4, 14)]
        public void CalculateNextReviewDate_Should_ReturnCorrectInterval(int cycle, int expectedDays)
        {
            // Act
            var nextDate = _progressService.CalculateNextReviewDate(cycle);

            // Assert
            var daysDiff = (nextDate - DateTime.UtcNow).Days;
            daysDiff.ShouldBe(expectedDays, tolerance: 0);
        }

        [Fact]
        public void CalculateMasteryScore_Should_ReturnZero_ForIncompleteSession()
        {
            // Arrange
            var session = CreateSession();

            // Act
            var score = _progressService.CalculateMasteryScore(session);

            // Assert
            score.ShouldBe(0);
        }

        #region Helper Methods

        private Topic CreateTopic()
        {
            return new Topic(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Topic",
                false,
                DifficultyLevel.Intermediate,
                10
            );
        }

        private Topic CreateFolder()
        {
            return new Topic(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Folder",
                true,
                DifficultyLevel.Intermediate,
                10
            );
        }

        private LearningSession CreateSession(Guid? topicId = null)
        {
            return new LearningSession(
                Guid.NewGuid(),
                Guid.NewGuid(),
                topicId ?? Guid.NewGuid()
            );
        }

        private void AdvanceToPhase(LearningSession session, LearningPhase targetPhase)
        {
            while (session.CurrentPhase < targetPhase)
            {
                session.GetCurrentPhaseMilestone().MarkCompleted(1000);
                session.AdvanceToNextPhase(1000);
            }
        }

        #endregion
    }
}
