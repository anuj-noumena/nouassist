using System;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Events;
using Shouldly;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Xunit;

namespace NouAssist.SkillDevelopment.Tests.Aggregates
{
    public class SkillTests
    {
        [Fact]
        public void Should_CreateSkill_WithValidParameters()
        {
            // Arrange & Act
            var skill = new Skill(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Skill",
                ProficiencyLevel.Intermediate,
                40);

            // Assert
            skill.ShouldNotBeNull();
            skill.Title.ShouldBe("Test Skill");
            skill.Status.ShouldBe(SkillStatus.NotStarted);
            skill.CurrentProficiency.Score.ShouldBe(0);
        }

        [Fact]
        public void Should_ThrowException_When_EstimatedHoursIsNegative()
        {
            // Arrange & Act & Assert
            Should.Throw<ArgumentException>(() =>
                new Skill(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    "Test Skill",
                    ProficiencyLevel.Beginner,
                    -10));
        }

        [Fact]
        public void UpdateProficiency_Should_PublishProgressEvent()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var skill = new Skill(
                Guid.NewGuid(),
                userId,
                "Test Skill",
                ProficiencyLevel.Intermediate,
                40);

            // Act
            skill.UpdateProficiency(50, "Mid-way progress", userId);

            // Assert
            skill.CurrentProficiency.Score.ShouldBe(50);
            var events = skill.GetDistributedEvents();
            events.ShouldNotBeEmpty();
            events.ShouldContain(e => e.EventData is SkillProgressUpdatedEto);
        }

        [Fact]
        public void MarkAsMastered_Should_PublishMasteryEvent_WhenProficiencySufficient()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var skill = new Skill(
                Guid.NewGuid(),
                userId,
                "Test Skill",
                ProficiencyLevel.Expert,
                40);
            
            skill.UpdateProficiency(95, "Ready for mastery", userId);

            // Act
            skill.MarkAsMastered(userId);

            // Assert
            skill.Status.ShouldBe(SkillStatus.Mastered);
            var events = skill.GetDistributedEvents();
            events.ShouldContain(e => e.EventData is SkillMasteredEto);
        }

        [Fact]
        public void MarkAsMastered_Should_ThrowException_WhenProficiencyInsufficient()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var skill = new Skill(
                Guid.NewGuid(),
                userId,
                "Test Skill",
                ProficiencyLevel.Intermediate,
                40);
            
            skill.UpdateProficiency(75, "Not ready yet", userId);

            // Act & Assert
            Should.Throw<BusinessException>(() => skill.MarkAsMastered(userId))
                .Code.ShouldBe("SkillDevelopment:InsufficientProficiencyForMastery");
        }

        [Fact]
        public void AddPrerequisite_Should_ThrowException_WhenDuplicate()
        {
            // Arrange
            var skill = new Skill(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Skill",
                ProficiencyLevel.Advanced,
                40);
            
            var prerequisiteId = Guid.NewGuid();
            skill.AddPrerequisite(prerequisiteId);

            // Act & Assert
            Should.Throw<BusinessException>(() => skill.AddPrerequisite(prerequisiteId))
                .Code.ShouldBe("SkillDevelopment:DuplicatePrerequisite");
        }

        [Fact]
        public void AddSubSkill_Should_ThrowException_WhenNotCategory()
        {
            // Arrange
            var skill = new Skill(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Test Skill",
                ProficiencyLevel.Intermediate,
                40,
                isCategory: false);

            // Act & Assert
            Should.Throw<BusinessException>(() => skill.AddSubSkill(Guid.NewGuid()))
                .Code.ShouldBe("SkillDevelopment:OnlyCategoriesCanHaveSubSkills");
        }
    }
}
