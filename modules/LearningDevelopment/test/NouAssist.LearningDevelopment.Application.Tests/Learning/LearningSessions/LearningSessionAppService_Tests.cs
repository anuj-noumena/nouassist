using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.Learning.Enums;
using NouAssist.Learning.LearningSessions;
using NouAssist.Learning.LearningSessions.Dtos;
using NouAssist.Learning.Topics;
using NouAssist.Learning.Topics.Dtos;
using Shouldly;
using Xunit;

namespace NouAssist.Learning.Tests.LearningSessions
{
    public class LearningSessionAppService_Tests : LearningApplicationTestBase
    {
        private readonly ILearningSessionAppService _sessionAppService;
        private readonly ITopicAppService _topicAppService;

        public LearningSessionAppService_Tests()
        {
            _sessionAppService = GetRequiredService<ILearningSessionAppService>();
            _topicAppService = GetRequiredService<ITopicAppService>();
        }

        [Fact]
        public async Task StartSessionAsync_Should_Create_New_Session()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();

            // Act
            var session = await _sessionAppService.StartSessionAsync(topic.Id);

            // Assert
            session.ShouldNotBeNull();
            session.TopicId.ShouldBe(topic.Id);
            session.CurrentPhase.ShouldBe(LearningPhase.PreLearning);
            session.Completed.ShouldBeFalse();
            session.Paused.ShouldBeFalse();
        }

        [Fact]
        public async Task StartSessionAsync_Should_Throw_If_Session_Exists()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();
            await _sessionAppService.StartSessionAsync(topic.Id);

            // Act & Assert
            await Should.ThrowAsync<Volo.Abp.BusinessException>(
                async () => await _sessionAppService.StartSessionAsync(topic.Id));
        }

        [Fact]
        public async Task RecordPreLearningQuestionsAsync_Should_Store_Questions()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            var input = new RecordPreLearningQuestionsDto
            {
                Questions = new List<string>
                {
                    "What is machine learning?",
                    "How does supervised learning work?"
                }
            };

            // Act
            await _sessionAppService.RecordPreLearningQuestionsAsync(session.Id, input);

            // Assert
            var updated = await _sessionAppService.GetAsync(session.Id);
            var milestone = updated.PhaseMilestones[1]; // Phase 1
            milestone.Questions.Count.ShouldBe(2);
        }

        [Fact]
        public async Task AdvancePhaseAsync_Should_Move_To_Next_Phase()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            
            // Record required data for Phase 1
            await _sessionAppService.RecordPreLearningQuestionsAsync(session.Id,
                new RecordPreLearningQuestionsDto
                {
                    Questions = new List<string> { "Question 1" }
                });

            var input = new AdvancePhaseDto { TimeSpentMs = 3600000 }; // 1 hour

            // Act
            var result = await _sessionAppService.AdvancePhaseAsync(session.Id, input);

            // Assert
            result.CurrentPhase.ShouldBe(LearningPhase.ActiveLearning);
            result.PhaseMilestones[1].Completed.ShouldBeTrue();
            result.PhaseMilestones[1].TimeSpentMs.ShouldBe(3600000);
        }

        [Fact]
        public async Task RecordBloomAssessmentAsync_Should_Store_Assessment()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            await AdvanceToPhaseAsync(session.Id, LearningPhase.BloomAssessment);

            var input = new RecordBloomAssessmentDto
            {
                RememberScore = 0.85,
                UnderstandScore = 0.80,
                ApplyScore = 0.75,
                AnalyzeScore = 0.70,
                EvaluateScore = 0.65,
                CreateScore = 0.60
            };

            // Act
            await _sessionAppService.RecordBloomAssessmentAsync(session.Id, input);

            // Assert
            var updated = await _sessionAppService.GetAsync(session.Id);
            var milestone = updated.PhaseMilestones[4]; // Phase 4
            milestone.BloomAssessment.ShouldNotBeNull();
            milestone.BloomAssessment.RememberScore.ShouldBe(0.85);
            milestone.BloomAssessment.OverallScore.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CompleteSpacedCycleAsync_Should_Record_Cycle_Progress()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            await AdvanceToPhaseAsync(session.Id, LearningPhase.SpacedRepetition);

            var input = new CompleteSpacedCycleDto
            {
                CycleNumber = 1,
                RecallAccuracy = 0.90,
                AverageConfidence = 0.85,
                CompletionRate = 95,
                Improvements = new Dictionary<string, bool> { { "recall", true } },
                Reflection = new Dictionary<string, string> { { "notes", "Good progress" } }
            };

            // Act
            await _sessionAppService.CompleteSpacedCycleAsync(session.Id, input);

            // Assert
            var updated = await _sessionAppService.GetAsync(session.Id);
            var milestone = updated.PhaseMilestones[5]; // Phase 5
            milestone.SpacedCycles.ShouldContainKey(1);
            milestone.SpacedCycles[1].RecallAccuracy.ShouldBe(0.90);
        }

        [Fact]
        public async Task PauseSessionAsync_Should_Pause_Active_Session()
        {
            // Arrange
            var session = await CreateTestSessionAsync();

            // Act
            var result = await _sessionAppService.PauseSessionAsync(session.Id);

            // Assert
            result.Paused.ShouldBeTrue();
            result.PausedAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task ResumeSessionAsync_Should_Resume_Paused_Session()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            await _sessionAppService.PauseSessionAsync(session.Id);

            // Act
            var result = await _sessionAppService.ResumeSessionAsync(session.Id);

            // Assert
            result.Paused.ShouldBeFalse();
            result.ResumedAt.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetActiveSessionsAsync_Should_Return_Only_Active_Sessions()
        {
            // Arrange
            var topic1 = await CreateTestTopicAsync();
            var topic2 = await CreateTestTopicAsync();
            
            await _sessionAppService.StartSessionAsync(topic1.Id);
            var session2 = await _sessionAppService.StartSessionAsync(topic2.Id);
            await _sessionAppService.PauseSessionAsync(session2.Id);

            // Act
            var result = await _sessionAppService.GetActiveSessionsAsync();

            // Assert
            result.Count.ShouldBe(1);
            result.ShouldAllBe(s => !s.Paused && !s.Completed);
        }

        [Fact]
        public async Task GetPausedSessionsAsync_Should_Return_Only_Paused_Sessions()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();
            var session = await _sessionAppService.StartSessionAsync(topic.Id);
            await _sessionAppService.PauseSessionAsync(session.Id);

            // Act
            var result = await _sessionAppService.GetPausedSessionsAsync();

            // Assert
            result.Count.ShouldBeGreaterThan(0);
            result.ShouldAllBe(s => s.Paused && !s.Completed);
        }

        #region Helper Methods

        private async Task<TopicDto> CreateTestTopicAsync()
        {
            return await _topicAppService.CreateAsync(new CreateTopicDto
            {
                Title = $"Test Topic {Guid.NewGuid()}",
                IsFolder = false,
                Difficulty = DifficultyLevel.Intermediate,
                EstimatedHours = 10
            });
        }

        private async Task<LearningSessionDto> CreateTestSessionAsync()
        {
            var topic = await CreateTestTopicAsync();
            return await _sessionAppService.StartSessionAsync(topic.Id);
        }

        private async Task AdvanceToPhaseAsync(Guid sessionId, LearningPhase targetPhase)
        {
            var session = await _sessionAppService.GetAsync(sessionId);

            while (session.CurrentPhase < targetPhase)
            {
                // Record minimal data for current phase
                switch (session.CurrentPhase)
                {
                    case LearningPhase.PreLearning:
                        await _sessionAppService.RecordPreLearningQuestionsAsync(sessionId,
                            new RecordPreLearningQuestionsDto { Questions = new List<string> { "Q1" } });
                        break;
                    case LearningPhase.ActiveLearning:
                        await _sessionAppService.RecordSectionReflectionAsync(sessionId,
                            new RecordSectionReflectionDto { SectionKey = "s1", Reflection = "R1" });
                        break;
                    case LearningPhase.FeynmanSummary:
                        await _sessionAppService.RecordFeynmanSummaryAsync(sessionId,
                            new RecordFeynmanSummaryDto { Summary = "Test summary" });
                        break;
                    case LearningPhase.BloomAssessment:
                        await _sessionAppService.RecordBloomAssessmentAsync(sessionId,
                            new RecordBloomAssessmentDto
                            {
                                RememberScore = 0.8,
                                UnderstandScore = 0.8,
                                ApplyScore = 0.8,
                                AnalyzeScore = 0.8,
                                EvaluateScore = 0.8,
                                CreateScore = 0.8
                            });
                        break;
                    case LearningPhase.SpacedRepetition:
                        for (int i = 1; i <= 4; i++)
                        {
                            await _sessionAppService.CompleteSpacedCycleAsync(sessionId,
                                new CompleteSpacedCycleDto
                                {
                                    CycleNumber = i,
                                    RecallAccuracy = 0.8,
                                    AverageConfidence = 0.8,
                                    CompletionRate = 80
                                });
                        }
                        break;
                }

                session = await _sessionAppService.AdvancePhaseAsync(sessionId,
                    new AdvancePhaseDto { TimeSpentMs = 1000 });
            }
        }

        #endregion
    }
}
