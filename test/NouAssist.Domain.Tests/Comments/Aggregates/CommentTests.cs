using System;
using System.Collections.Generic;
using NouAssist.Comments.Enums;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.Comments.Aggregates
{
    public class CommentTests
    {
        [Fact]
        public void Constructor_Should_CreateValidComment()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var entityType = CommentableEntityType.Task;
            var entityId = Guid.NewGuid();
            var content = "This is a test comment";

            // Act
            var comment = new Comment(
                Guid.NewGuid(),
                authorId,
                entityType,
                entityId,
                content);

            // Assert
            comment.ShouldNotBeNull();
            comment.AuthorId.ShouldBe(authorId);
            comment.EntityType.ShouldBe(entityType);
            comment.EntityId.ShouldBe(entityId);
            comment.Content.ShouldBe(content);
            comment.IsDeleted.ShouldBeFalse();
            comment.IsPinned.ShouldBeFalse();
            comment.IsEdited.ShouldBeFalse();
        }

        [Fact]
        public void Edit_Should_UpdateContentAndSetEditedFlag()
        {
            // Arrange
            var comment = CreateValidComment();
            var newContent = "Updated comment content";

            // Act
            comment.Edit(newContent);

            // Assert
            comment.Content.ShouldBe(newContent);
            comment.IsEdited.ShouldBeTrue();
            comment.EditedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Edit_Should_ThrowException_WhenDeleted()
        {
            // Arrange
            var comment = CreateValidComment();
            comment.Delete();

            // Act & Assert
            Should.Throw<BusinessException>(() => comment.Edit("New content"));
        }

        [Fact]
        public void Delete_Should_SetDeletedFlag()
        {
            // Arrange
            var comment = CreateValidComment();

            // Act
            comment.Delete();

            // Assert
            comment.IsDeleted.ShouldBeTrue();
            comment.DeletedAt.ShouldNotBeNull();
        }

        [Fact]
        public void Pin_Should_SetPinnedFlag()
        {
            // Arrange
            var comment = CreateValidComment();

            // Act
            comment.Pin();

            // Assert
            comment.IsPinned.ShouldBeTrue();
        }

        [Fact]
        public void AddReaction_Should_IncreaseReactionCount()
        {
            // Arrange
            var comment = CreateValidComment();
            var userId = Guid.NewGuid();
            var emoji = "👍";

            // Act
            comment.AddReaction(userId, emoji);

            // Assert
            comment.ReactionsCount.ShouldBe(1);
            comment.HasReactions.ShouldBeTrue();
        }

        [Fact]
        public void RemoveReaction_Should_DecreaseReactionCount()
        {
            // Arrange
            var comment = CreateValidComment();
            var userId = Guid.NewGuid();
            var emoji = "👍";
            comment.AddReaction(userId, emoji);

            // Act
            comment.RemoveReaction(userId, emoji);

            // Assert
            comment.ReactionsCount.ShouldBe(0);
        }

        [Fact]
        public void Constructor_Should_ThrowException_WhenTooManyMentions()
        {
            // Arrange
            var mentionedUsers = new List<Guid>();
            for (int i = 0; i < 15; i++)
            {
                mentionedUsers.Add(Guid.NewGuid());
            }

            // Act & Assert
            Should.Throw<BusinessException>(() => new Comment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CommentableEntityType.Task,
                Guid.NewGuid(),
                "Comment with too many mentions",
                null,
                null,
                mentionedUsers));
        }

        private Comment CreateValidComment()
        {
            return new Comment(
                Guid.NewGuid(),
                Guid.NewGuid(),
                CommentableEntityType.Task,
                Guid.NewGuid(),
                "Test comment content");
        }
    }
}
