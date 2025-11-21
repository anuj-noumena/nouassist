using System;
using NouAssist.SharedKernel.Aggregates;
using Shouldly;
using Volo.Abp;
using Xunit;

namespace NouAssist.SharedKernel
{
    public class TagTests : SharedKernelDomainTestBase<SharedKernelDomainTestModule>
    {
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Should_Create_Tag_With_Valid_Data()
        {
            // Arrange & Act
            var tag = new Tag(Guid.NewGuid(), _userId, "Important");

            // Assert
            tag.ShouldNotBeNull();
            tag.Title.ShouldBe("Important");
            tag.OwnerId.ShouldBe(_userId);
            tag.UsageCount.ShouldBe(0);
            tag.IsPopular.ShouldBeFalse();
        }

        [Fact]
        public void Should_Not_Create_Tag_With_Empty_Title()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new Tag(Guid.NewGuid(), _userId, ""));
        }

        [Fact]
        public void Should_Not_Create_Tag_With_Null_Title()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new Tag(Guid.NewGuid(), _userId, null!));
        }

        [Fact]
        public void Should_Rename_Tag_Successfully()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Old Name");

            // Act
            tag.Rename("New Name");

            // Assert
            tag.Title.ShouldBe("New Name");
        }

        [Fact]
        public void Should_Not_Rename_Tag_With_Empty_Title()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Original");

            // Assert
            Should.Throw<ArgumentException>(() => tag.Rename(""));
        }

        [Fact]
        public void Should_Increment_Usage_Count()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Test");

            // Act
            tag.IncrementUsage();
            tag.IncrementUsage();
            tag.IncrementUsage();

            // Assert
            tag.UsageCount.ShouldBe(3);
        }

        [Fact]
        public void Should_Decrement_Usage_Count()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Test");
            tag.IncrementUsage();
            tag.IncrementUsage();
            tag.IncrementUsage();

            // Act
            tag.DecrementUsage();

            // Assert
            tag.UsageCount.ShouldBe(2);
        }

        [Fact]
        public void Should_Not_Decrement_Below_Zero()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Test");

            // Act
            tag.DecrementUsage();
            tag.DecrementUsage();

            // Assert
            tag.UsageCount.ShouldBe(0);
        }

        [Fact]
        public void Should_Mark_Tag_As_Popular_When_Usage_Exceeds_10()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Test");

            // Act
            for (int i = 0; i < 15; i++)
            {
                tag.IncrementUsage();
            }

            // Assert
            tag.UsageCount.ShouldBe(15);
            tag.IsPopular.ShouldBeTrue();
        }

        [Fact]
        public void Should_Not_Mark_Tag_As_Popular_With_Low_Usage()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Test");

            // Act
            for (int i = 0; i < 10; i++)
            {
                tag.IncrementUsage();
            }

            // Assert
            tag.UsageCount.ShouldBe(10);
            tag.IsPopular.ShouldBeFalse();
        }

        [Fact]
        public void Should_Assign_To_Category()
        {
            // Arrange
            var tag = new Tag(Guid.NewGuid(), _userId, "Test");
            var categoryId = Guid.NewGuid();

            // Act
            tag.AssignToCategory(categoryId);

            // Assert
            tag.CategoryId.ShouldBe(categoryId);
        }
    }

    public class TagCategoryTests : SharedKernelDomainTestBase<SharedKernelDomainTestModule>
    {
        private readonly Guid _userId = Guid.NewGuid();

        [Fact]
        public void Should_Create_TagCategory_With_Valid_Data()
        {
            // Arrange & Act
            var category = new TagCategory(Guid.NewGuid(), _userId, "Work");

            // Assert
            category.ShouldNotBeNull();
            category.Name.ShouldBe("Work");
            category.OwnerId.ShouldBe(_userId);
        }

        [Fact]
        public void Should_Not_Create_TagCategory_With_Empty_Name()
        {
            // Assert
            Should.Throw<ArgumentException>(() =>
                new TagCategory(Guid.NewGuid(), _userId, ""));
        }

        [Fact]
        public void Should_Rename_TagCategory_Successfully()
        {
            // Arrange
            var category = new TagCategory(Guid.NewGuid(), _userId, "Old Name");

            // Act
            category.Rename("New Name");

            // Assert
            category.Name.ShouldBe("New Name");
        }
    }
}
