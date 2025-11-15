using System;
using System.Threading.Tasks;
using NouAssist.SkillDevelopment.Aggregates;
using NouAssist.SkillDevelopment.Enums;
using NouAssist.SkillDevelopment.Skills;
using NouAssist.SkillDevelopment.Skills.Dtos;
using Shouldly;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NouAssist.SkillDevelopment.Application.Tests.Skills
{
    public class SkillAppService_Tests : SkillDevelopmentApplicationTestBase
    {
        private readonly ISkillAppService _skillAppService;
        private readonly IRepository<Skill, Guid> _skillRepository;

        public SkillAppService_Tests()
        {
            _skillAppService = GetRequiredService<ISkillAppService>();
            _skillRepository = GetRequiredService<IRepository<Skill, Guid>>();
        }

        [Fact]
        public async Task Should_Create_Skill()
        {
            // Arrange
            var input = new CreateSkillDto
            {
                Title = "Python Programming",
                Description = "Learn Python programming language",
                TargetProficiency = ProficiencyLevel.Advanced,
                EstimatedPracticeHours = 100,
                Category = "Programming"
            };

            // Act
            var result = await _skillAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.Title.ShouldBe(input.Title);
            result.Status.ShouldBe(SkillStatus.NotStarted);
            result.CurrentProficiencyScore.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Get_Skill()
        {
            // Arrange
            var skill = await CreateTestSkillAsync();

            // Act
            var result = await _skillAppService.GetAsync(skill.Id);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(skill.Id);
            result.Title.ShouldBe(skill.Title);
        }

        [Fact]
        public async Task Should_Update_Proficiency()
        {
            // Arrange
            var skill = await CreateTestSkillAsync();
            var input = new UpdateSkillProficiencyDto
            {
                Score = 75,
                Notes = "Making good progress"
            };

            // Act
            var result = await _skillAppService.UpdateProficiencyAsync(skill.Id, input);

            // Assert
            result.CurrentProficiencyScore.ShouldBe(75);
        }

        [Fact]
        public async Task Should_Mark_As_Mastered()
        {
            // Arrange
            var skill = await CreateTestSkillAsync();
            
            // First update proficiency to 95
            await _skillAppService.UpdateProficiencyAsync(skill.Id, new UpdateSkillProficiencyDto
            {
                Score = 95,
                Notes = "Ready for mastery"
            });

            // Act
            var result = await _skillAppService.MarkAsMasteredAsync(skill.Id);

            // Assert
            result.Status.ShouldBe(SkillStatus.Mastered);
        }

        [Fact]
        public async Task Should_Get_My_Skills()
        {
            // Arrange
            await CreateTestSkillAsync();
            await CreateTestSkillAsync("JavaScript Fundamentals");

            // Act
            var result = await _skillAppService.GetMySkillsAsync();

            // Assert
            result.ShouldNotBeEmpty();
            result.Count.ShouldBeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Should_Add_Prerequisite()
        {
            // Arrange
            var prerequisite = await CreateTestSkillAsync("Python Basics");
            var skill = await CreateTestSkillAsync("Advanced Python");

            // Act
            await _skillAppService.AddPrerequisiteAsync(skill.Id, prerequisite.Id);

            // Assert
            var updatedSkill = await _skillAppService.GetAsync(skill.Id);
            updatedSkill.Prerequisites.ShouldContain(prerequisite.Id);
        }

        #region Test Helpers

        private async Task<Skill> CreateTestSkillAsync(string title = "Test Skill")
        {
            var skill = new Skill(
                GuidGenerator.Create(),
                CurrentUser.Id.Value,
                title,
                ProficiencyLevel.Intermediate,
                50);

            return await _skillRepository.InsertAsync(skill);
        }

        #endregion
    }
}
