using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NouAssist.Learning.Enums;
using NouAssist.Learning.Topics;
using NouAssist.Learning.Topics.Dtos;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace NouAssist.Learning.Tests.Topics
{
    public class TopicAppService_Tests : LearningApplicationTestBase
    {
        private readonly ITopicAppService _topicAppService;

        public TopicAppService_Tests()
        {
            _topicAppService = GetRequiredService<ITopicAppService>();
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Valid_Topic()
        {
            // Arrange
            var input = new CreateTopicDto
            {
                Title = "Introduction to Machine Learning",
                Description = "Learn the basics of ML",
                IsFolder = false,
                Difficulty = DifficultyLevel.Intermediate,
                EstimatedHours = 20,
                Category = "Technology",
                LearningGoals = "Understand ML fundamentals",
                Tags = new List<string> { "ML", "AI", "Tech" }
            };

            // Act
            var result = await _topicAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldNotBe(Guid.Empty);
            result.Title.ShouldBe(input.Title);
            result.Difficulty.ShouldBe(input.Difficulty);
            result.Status.ShouldBe(TopicStatus.Planned);
            result.Progress.ShouldBe(0);
            result.Tags.Count.ShouldBe(3);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Folder_Topic()
        {
            // Arrange
            var input = new CreateTopicDto
            {
                Title = "Computer Science Fundamentals",
                IsFolder = true,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 100
            };

            // Act
            var result = await _topicAppService.CreateAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.IsFolder.ShouldBeTrue();
            result.LearningSessionId.ShouldBeNull(); // Folders don't have sessions
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Topic_Properties()
        {
            // Arrange
            var createInput = new CreateTopicDto
            {
                Title = "Original Title",
                IsFolder = false,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 10
            };
            var created = await _topicAppService.CreateAsync(createInput);

            var updateInput = new UpdateTopicDto
            {
                Title = "Updated Title",
                Description = "New description",
                Difficulty = DifficultyLevel.Advanced,
                EstimatedHours = 20,
                Tags = new List<string> { "Updated" }
            };

            // Act
            var result = await _topicAppService.UpdateAsync(created.Id, updateInput);

            // Assert
            result.Title.ShouldBe(updateInput.Title);
            result.Description.ShouldBe(updateInput.Description);
            result.Difficulty.ShouldBe(updateInput.Difficulty);
            result.EstimatedHours.ShouldBe(updateInput.EstimatedHours);
        }

        [Fact]
        public async Task GetListAsync_Should_Return_Filtered_Topics()
        {
            // Arrange
            await CreateTestTopicsAsync();

            var input = new GetTopicListDto
            {
                Status = TopicStatus.Planned,
                Difficulty = DifficultyLevel.Intermediate,
                MaxResultCount = 10
            };

            // Act
            var result = await _topicAppService.GetListAsync(input);

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(t => t.Status == TopicStatus.Planned);
            result.Items.ShouldAllBe(t => t.Difficulty == DifficultyLevel.Intermediate);
        }

        [Fact]
        public async Task AddChildAsync_Should_Add_Child_To_Folder()
        {
            // Arrange
            var parentInput = new CreateTopicDto
            {
                Title = "Parent Folder",
                IsFolder = true,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 50
            };
            var parent = await _topicAppService.CreateAsync(parentInput);

            var childInput = new CreateTopicDto
            {
                Title = "Child Topic",
                IsFolder = false,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 10
            };

            // Act
            var child = await _topicAppService.AddChildAsync(parent.Id, childInput);

            // Assert
            child.ShouldNotBeNull();
            child.ParentId.ShouldBe(parent.Id);
        }

        [Fact]
        public async Task AddChildAsync_Should_Throw_For_NonFolder_Parent()
        {
            // Arrange
            var parentInput = new CreateTopicDto
            {
                Title = "Non-Folder Topic",
                IsFolder = false,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 10
            };
            var parent = await _topicAppService.CreateAsync(parentInput);

            var childInput = new CreateTopicDto
            {
                Title = "Child Topic",
                IsFolder = false,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 5
            };

            // Act & Assert
            await Should.ThrowAsync<Volo.Abp.BusinessException>(async () =>
                await _topicAppService.AddChildAsync(parent.Id, childInput));
        }

        [Fact]
        public async Task GetRootTopicsAsync_Should_Return_Only_Root_Topics()
        {
            // Arrange
            await CreateTestTopicHierarchyAsync();

            // Act
            var result = await _topicAppService.GetRootTopicsAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Items.ShouldAllBe(t => t.ParentId == null);
        }

        [Fact]
        public async Task UpdateStatusAsync_Should_Update_Topic_Status()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();
            var input = new UpdateTopicStatusDto { Status = TopicStatus.Active };

            // Act
            var result = await _topicAppService.UpdateStatusAsync(topic.Id, input);

            // Assert
            result.Status.ShouldBe(TopicStatus.Active);
        }

        [Fact]
        public async Task UpdateProgressAsync_Should_Update_Topic_Progress()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();
            var input = new UpdateTopicProgressDto { Progress = 50 };

            // Act
            var result = await _topicAppService.UpdateProgressAsync(topic.Id, input);

            // Assert
            result.Progress.ShouldBe(50);
        }

        [Fact]
        public async Task UpdateProgressAsync_Should_Auto_Complete_At_100_Percent()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();
            var input = new UpdateTopicProgressDto { Progress = 100 };

            // Act
            var result = await _topicAppService.UpdateProgressAsync(topic.Id, input);

            // Assert
            result.Progress.ShouldBe(100);
            result.Status.ShouldBe(TopicStatus.Completed);
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Topic()
        {
            // Arrange
            var topic = await CreateTestTopicAsync();

            // Act
            await _topicAppService.DeleteAsync(topic.Id);

            // Assert
            await Should.ThrowAsync<Volo.Abp.Domain.Entities.EntityNotFoundException>(
                async () => await _topicAppService.GetAsync(topic.Id));
        }

        [Fact]
        public async Task DeleteAsync_Should_Throw_For_Topic_With_Children()
        {
            // Arrange
            var parent = await CreateTestFolderAsync();
            await _topicAppService.AddChildAsync(parent.Id, new CreateTopicDto
            {
                Title = "Child",
                IsFolder = false,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 5
            });

            // Act & Assert
            await Should.ThrowAsync<Volo.Abp.BusinessException>(
                async () => await _topicAppService.DeleteAsync(parent.Id));
        }

        #region Helper Methods

        private async Task<TopicDto> CreateTestTopicAsync()
        {
            return await _topicAppService.CreateAsync(new CreateTopicDto
            {
                Title = "Test Topic",
                IsFolder = false,
                Difficulty = DifficultyLevel.Intermediate,
                EstimatedHours = 15
            });
        }

        private async Task<TopicDto> CreateTestFolderAsync()
        {
            return await _topicAppService.CreateAsync(new CreateTopicDto
            {
                Title = "Test Folder",
                IsFolder = true,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 50
            });
        }

        private async Task CreateTestTopicsAsync()
        {
            var topics = new[]
            {
                new CreateTopicDto { Title = "Topic 1", IsFolder = false, Difficulty = DifficultyLevel.Beginner, EstimatedHours = 10 },
                new CreateTopicDto { Title = "Topic 2", IsFolder = false, Difficulty = DifficultyLevel.Intermediate, EstimatedHours = 15 },
                new CreateTopicDto { Title = "Topic 3", IsFolder = false, Difficulty = DifficultyLevel.Advanced, EstimatedHours = 20 },
            };

            foreach (var topic in topics)
            {
                await _topicAppService.CreateAsync(topic);
            }
        }

        private async Task CreateTestTopicHierarchyAsync()
        {
            var root1 = await CreateTestFolderAsync();
            var root2 = await CreateTestFolderAsync();

            await _topicAppService.AddChildAsync(root1.Id, new CreateTopicDto
            {
                Title = "Child of Root 1",
                IsFolder = false,
                Difficulty = DifficultyLevel.Beginner,
                EstimatedHours = 5
            });
        }

        #endregion
    }
}
