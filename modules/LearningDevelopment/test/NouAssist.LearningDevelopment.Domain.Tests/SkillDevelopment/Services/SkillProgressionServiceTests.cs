using System;
using System.Collections.Generic;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Services;
using Shouldly;
using Xunit;

namespace NouAssist.SkillDevelopment.Tests.Services
{
    public class SkillProgressionServiceTests
    {
        private readonly SkillProgressionService _service;

        public SkillProgressionServiceTests()
        {
            _service = new SkillProgressionService();
        }

        [Fact]
        public void ProgressSkill_Should_UpdateProficiency_WhenSessionCompleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var skill = CreateSkill(userId);
            var session = CreateCompletedSession(skill.Id, userId);
            var prerequisites = new List<Skill>();

            // Act
            var result = _service.ProgressSkill(skill, session, prerequisites, userId);

            // Assert
            result.IsSuccessful.ShouldBeTrue();
            result.ProficiencyGain.ShouldBeGreaterThan(0);
            result.UpdatedSkill.CurrentProficiency.Score.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void ValidatePrerequisites_Should_FailWhen_PrerequisiteNotMastered()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var prerequisiteSkill = CreateSkill(userId);
            prerequisiteSkill.UpdateProficiency(50, "Low proficiency", userId); // Below threshold
            
            var skill = CreateSkill(userId);
            skill.AddPrerequisite(prerequisiteSkill.Id);

            // Act
            var result = _service.ValidatePrerequisites(skill, new List<Skill> { prerequisiteSkill });

            // Assert
            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(e => e.Contains("insufficient proficiency"));
        }

        [Fact]
        public void GeneratePracticePlan_Should_CreateAppropriateItems_ForLevelGap()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var skill = CreateSkill(userId);
            var currentLevel = ProficiencyLevel.Beginner;
            var targetLevel = ProficiencyLevel.Advanced;
            var hoursPerWeek = 10;

            // Act
            var recommendation = _service.GeneratePracticePlan(
                skill, currentLevel, targetLevel, hoursPerWeek);

            // Assert
            recommendation.ShouldNotBeNull();
            recommendation.EstimatedWeeks.ShouldBeGreaterThan(0);
            recommendation.PracticeItems.ShouldNotBeEmpty();
        }

        #region Test Helpers

        private Skill CreateSkill(Guid userId, string title = "Python Programming")
        {
            return new Skill(
                Guid.NewGuid(),
                userId,
                title,
                ProficiencyLevel.Intermediate,
                40);
        }

        private SkillDevelopmentSession CreateCompletedSession(Guid skillId, Guid userId)
        {
            var session = new SkillDevelopmentSession(
                Guid.NewGuid(),
                userId,
                skillId);

            // Simulate completing all steps
            for (int step = 1; step <= 6; step++)
            {
                session.CompleteCurrentStep();
                if (step < 6)
                    session.AdvanceToNextStep();
            }

            return session;
        }

        #endregion
    }
}
