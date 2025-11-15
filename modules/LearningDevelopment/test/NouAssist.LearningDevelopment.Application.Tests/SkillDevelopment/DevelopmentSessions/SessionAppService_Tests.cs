using System;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.DevelopmentSessions;
using NouAssist.SkillDevelopment.DevelopmentSessions.Dtos;
using NouAssist.SkillDevelopment.Enums;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NouAssist.SkillDevelopment.Application.Tests.DevelopmentSessions
{
    public class SessionAppService_Tests : SkillDevelopmentApplicationTestBase
    {
        private readonly ISkillDevelopmentSessionAppService _sessionAppService;
        private readonly IRepository<Skill, Guid> _skillRepository;
        private readonly IRepository<SkillDevelopmentSession, Guid> _sessionRepository;

        public SessionAppService_Tests()
        {
            _sessionAppService = GetRequiredService<ISkillDevelopmentSessionAppService>();
            _skillRepository = GetRequiredService<IRepository<Skill, Guid>>();
            _sessionRepository = GetRequiredService<IRepository<SkillDevelopmentSession, Guid>>();
        }

        [Fact]
        public async Task Should_Start_Development_Session()
        {
            // Arrange
            var skill = await CreateTestSkillAsync();
            var input = new StartDevelopmentSessionDto
            {
                SkillId = skill.Id
            };

            // Act
            var result = await _sessionAppService.StartAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.SkillId.ShouldBe(skill.Id);
            result.CurrentStep.ShouldBe(DevelopmentStep.Assessment);
            result.Completed.ShouldBeFalse();
        }

        [Fact]
        public async Task Should_Record_Assessment()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            var input = new RecordAssessmentDto
            {
                Scores = new System.Collections.Generic.Dictionary<string, int>
                {
                    { "Fundamentals", 60 },
                    { "Practice", 50 }
                },
                StrengthAreas = new System.Collections.Generic.List<string> { "Quick learner" },
                ImprovementAreas = new System.Collections.Generic.List<string> { "More practice needed" },
                OverallNotes = "Good foundation"
            };

            // Act
            var result = await _sessionAppService.RecordAssessmentAsync(session.Id, input);

            // Assert
            result.ShouldNotBeNull();
            result.Milestones[DevelopmentStep.Assessment].InitialAssessment.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Add_Practice_Log()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            
            // Move to practice step
            await _sessionAppService.RecordAssessmentAsync(session.Id, new RecordAssessmentDto
            {
                Scores = new System.Collections.Generic.Dictionary<string, int> { { "Test", 70 } }
            });
            await _sessionAppService.CompleteCurrentStepAsync(session.Id);
            await _sessionAppService.AdvanceToNextStepAsync(session.Id); // Move to Planning
            await _sessionAppService.CompleteCurrentStepAsync(session.Id);
            await _sessionAppService.AdvanceToNextStepAsync(session.Id); // Move to Practice

            var input = new AddPracticeLogDto
            {
                Date = DateTime.UtcNow,
                DurationMinutes = 60,
                FocusArea = "Core concepts",
                SelfRating = 7,
                Notes = "Good progress"
            };

            // Act
            var result = await _sessionAppService.AddPracticeLogAsync(session.Id, input);

            // Assert
            result.ShouldNotBeNull();
            result.Milestones[DevelopmentStep.Practice].PracticeLogs.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task Should_Complete_Step_And_Advance()
        {
            // Arrange
            var session = await CreateTestSessionAsync();
            await _sessionAppService.RecordAssessmentAsync(session.Id, new RecordAssessmentDto
            {
                Scores = new System.Collections.Generic.Dictionary<string, int> { { "Test", 70 } }
            });

            // Act
            await _sessionAppService.CompleteCurrentStepAsync(session.Id);
            var result = await _sessionAppService.AdvanceToNextStepAsync(session.Id);

            // Assert
            result.CurrentStep.ShouldBe(DevelopmentStep.Planning);
        }

        [Fact]
        public async Task Should_Pause_And_Resume_Session()
        {
            // Arrange
            var session = await CreateTestSessionAsync();

            // Act - Pause
            var pausedResult = await _sessionAppService.PauseAsync(session.Id);
            
            // Assert - Paused
            pausedResult.Paused.ShouldBeTrue();
            pausedResult.PausedAt.ShouldNotBeNull();

            // Act - Resume
            var resumedResult = await _sessionAppService.ResumeAsync(session.Id);

            // Assert - Resumed
            resumedResult.Paused.ShouldBeFalse();
            resumedResult.ResumedAt.ShouldNotBeNull();
        }

        #region Test Helpers

        private async Task<Skill> CreateTestSkillAsync()
        {
            var skill = new Skill(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                "Test Skill",
                ProficiencyLevel.Intermediate,
                50);

            return await _skillRepository.InsertAsync(skill);
        }

        private async Task<SkillDevelopmentSession> CreateTestSessionAsync()
        {
            var skill = await CreateTestSkillAsync();
            var session = new SkillDevelopmentSession(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                skill.Id);

            skill.StartDevelopment(session.Id);

            await _sessionRepository.InsertAsync(session);
            await _skillRepository.UpdateAsync(skill);

            return session;
        }

        #endregion
    }
}
