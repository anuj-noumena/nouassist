using System;
using System.Collections.Generic;
using NouAssist.Learning.Aggregates;
using NouAssist.Learning.Enums;
using NouAssist.Learning.Events;
using NouAssist.Learning.Services;
using NouAssist.Learning.ValueObjects;
using Shouldly;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Xunit;

namespace NouAssist.Learning.Tests.Services
{
    public class LearningWorkflowServiceTests
    {
        private readonly LearningWorkflowService _workflowService;
        private readonly LearningProgressService _progressService;

        public LearningWorkflowServiceTests()
        {
            _progressService = new LearningProgressService();
            _workflowService = new LearningWorkflowService(_progressService);
        }

        [Fact]
        public void StartNewSession_Should_CreateSession_ForValidTopic()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var topic = CreateTopic(userId, "Machine Learning Basics");

            // Act
            var session = _workflowService.StartNewSession(topic, userId);

            // Assert
            session.ShouldNotBeNull();
            session.TopicId.ShouldBe(topic.Id);
            session.UserId.ShouldBe(userId);
            session.CurrentPhase.ShouldBe(LearningPhase.PreLearning);
            session.Completed.ShouldBeFalse();
            topic.Status.ShouldBe(TopicStatus.Active);
            topic.LearningSessionId.ShouldBe(session.Id);
        }

        [Fact]
        public void StartNewSession_Should_ThrowException_ForFolderTopic()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var folder = CreateTopic(userId, "Programming", isFolder: true);

            // Act & Assert
            Should.Throw<Volo.Abp.BusinessException>(() =>
                _workflowService.StartNewSession(folder, userId));
        }

        [Fact]
        public void RecordPreLearningQuestions_Should_StoreQuestions()
        {
            // Arrange
            var session = CreateSession();
            var questions = new List<string> { "What is ML?", "How does it work?" };

            // Act
            _workflowService.RecordPreLearningQuestions(session, questions);

            // Assert
            var milestone = session.GetCurrentPhaseMilestone();
            milestone.Questions.ShouldContain(questions[0]);
            milestone.Questions.ShouldContain(questions[1]);
        }

        [Fact]
        public void AdvancePhase_Should_MoveToNextPhase_WhenCompleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var topic = CreateTopic(userId, "Test Topic");
            var session = CreateSession(userId, topic.Id);
            
            // Complete Phase 1
            var questions = new List<string> { "Question 1" };
            _workflowService.RecordPreLearningQuestions(session, questions);

            // Act
            _workflowService.AdvancePhase(session, topic, 1000);

            // Assert
            session.CurrentPhase.ShouldBe(LearningPhase.ActiveLearning);
            topic.Progress.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void CompleteSpacedRepetitionCycle_Should_RecordCycleData()
        {
            // Arrange
            var session = CreateSession();
            session.GetCurrentPhaseMilestone().MarkCompleted(1000);
            for (int i = 0; i < 4; i++)
            {
                session.AdvanceToNextPhase(1000);
                if (i < 3) session.GetCurrentPhaseMilestone().MarkCompleted(1000);
            }

            // Now in Phase 5
            session.CurrentPhase.ShouldBe(LearningPhase.SpacedRepetition);

            var improvements = new Dictionary<string, bool> { { "recall", true } };
            var reflection = new Dictionary<string, string> { { "notes", "Good progress" } };

            // Act
            _workflowService.CompleteSpacedRepetitionCycle(
                session, 1, 0.85, 0.9, 95, improvements, reflection);

            // Assert
            var milestone = session.GetCurrentPhaseMilestone();
            milestone.SpacedCycles.ShouldContainKey(1);
            milestone.SpacedCycles[1].RecallAccuracy.ShouldBe(0.85);
        }

        [Fact]
        public void RecordBloomAssessment_Should_StoreAssessmentResults()
        {
            // Arrange
            var session = CreateSession();
            AdvanceToPhase(session, LearningPhase.BloomAssessment);
            
            var assessment = new BloomAssessmentResult(0.8, 0.85, 0.9, 0.75, 0.7, 0.65);

            // Act
            _workflowService.RecordBloomAssessment(session, assessment);

            // Assert
            var milestone = session.GetCurrentPhaseMilestone();
            milestone.BloomAssessment.ShouldNotBeNull();
            milestone.BloomAssessment.OverallScore.ShouldBe(0.775, 0.01);
        }

        [Fact]
        public void AdvancePhase_Should_CompleteTopicAndSession_WhenMasteryReached()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var topic = CreateTopic(userId, "Test Topic");
            var session = CreateSession(userId, topic.Id);
            
            // Complete all phases
            AdvanceToPhase(session, LearningPhase.Mastery);
            var validation = new Dictionary<string, object> { { "final_score", 95 } };
            _workflowService.RecordMasteryValidation(session, validation);

            // Act
            _workflowService.AdvancePhase(session, topic, 5000);

            // Assert
            session.Completed.ShouldBeTrue();
            topic.Status.ShouldBe(TopicStatus.Completed);
            topic.Progress.ShouldBe(100);
            
            var eto = session.GetDistributedEvents()
                .ShouldContain(e => e.EventData is TopicMasteredEto);
        }

        #region Helper Methods

        private Topic CreateTopic(Guid userId, string title, bool isFolder = false)
        {
            return new Topic(
                Guid.NewGuid(),
                userId,
                title,
                isFolder,
                DifficultyLevel.Intermediate,
                10
            );
        }

        private LearningSession CreateSession(Guid? userId = null, Guid? topicId = null)
        {
            return new LearningSession(
                Guid.NewGuid(),
                userId ?? Guid.NewGuid(),
                topicId ?? Guid.NewGuid()
            );
        }

        private void AdvanceToPhase(LearningSession session, LearningPhase targetPhase)
        {
            while (session.CurrentPhase < targetPhase)
            {
                var milestone = session.GetCurrentPhaseMilestone();
                
                // Add minimal data to satisfy validation
                switch (session.CurrentPhase)
                {
                    case LearningPhase.PreLearning:
                        milestone.AddPreLearningQuestions(new List<string> { "Test Q" });
                        break;
                    case LearningPhase.ActiveLearning:
                        milestone.AddSectionReflection("section1", "reflection");
                        break;
                    case LearningPhase.FeynmanSummary:
                        milestone.SetCornellSummary("Summary");
                        break;
                    case LearningPhase.BloomAssessment:
                        milestone.SetBloomAssessment(new BloomAssessmentResult(0.8, 0.8, 0.8, 0.8, 0.8, 0.8));
                        break;
                    case LearningPhase.SpacedRepetition:
                        for (int i = 1; i <= 4; i++)
                        {
                            var cycle = new CycleProgress(i).Complete(0.8, 0.8, 90, null, null);
                            milestone.AddSpacedCycle(cycle);
                        }
                        break;
                }
                
                milestone.MarkCompleted(1000);
                session.AdvanceToNextPhase(1000);
            }
        }

        #endregion
    }
}
