using System.Collections.Generic;
using NouAssist.WorkManagement.Shared.ValueObjects;
using Shouldly;
using Xunit;

namespace NouAssist.WorkManagement.Shared
{
    public class MetadataTests : WorkManagementDomainTestBase<WorkManagementDomainTestModule>
    {
        [Fact]
        public void Create_WithNoEntries_Should_ReturnEmptyMetadata()
        {
            // Act
            var metadata = Metadata.Create();

            // Assert
            metadata.ShouldNotBeNull();
            metadata.Entries.ShouldNotBeNull();
            metadata.Entries.Count.ShouldBe(0);
        }

        [Fact]
        public void Create_WithEntries_Should_ReturnMetadataWithEntries()
        {
            // Arrange
            var entries = new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", 42 }
            };

            // Act
            var metadata = Metadata.Create(entries);

            // Assert
            metadata.ShouldNotBeNull();
            metadata.Entries.Count.ShouldBe(2);
            metadata.Entries["key1"].ShouldBe("value1");
            metadata.Entries["key2"].ShouldBe(42);
        }

        [Fact]
        public void AddOrUpdate_NewKey_Should_AddEntry()
        {
            // Arrange
            var metadata = Metadata.Create();

            // Act
            var updated = metadata.AddOrUpdate("newKey", "newValue");

            // Assert
            updated.Entries.Count.ShouldBe(1);
            updated.Entries["newKey"].ShouldBe("newValue");
            // Original should be unchanged (immutability)
            metadata.Entries.Count.ShouldBe(0);
        }

        [Fact]
        public void AddOrUpdate_ExistingKey_Should_UpdateEntry()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "oldValue" }
            });

            // Act
            var updated = metadata.AddOrUpdate("key1", "newValue");

            // Assert
            updated.Entries["key1"].ShouldBe("newValue");
            // Original should be unchanged (immutability)
            metadata.Entries["key1"].ShouldBe("oldValue");
        }

        [Fact]
        public void Remove_ExistingKey_Should_RemoveEntry()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" },
                { "key2", "value2" }
            });

            // Act
            var updated = metadata.Remove("key1");

            // Assert
            updated.Entries.Count.ShouldBe(1);
            updated.Entries.ContainsKey("key1").ShouldBeFalse();
            updated.Entries["key2"].ShouldBe("value2");
            // Original should be unchanged (immutability)
            metadata.Entries.Count.ShouldBe(2);
        }

        [Fact]
        public void Remove_NonExistingKey_Should_ReturnSameInstance()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });

            // Act
            var result = metadata.Remove("nonExistingKey");

            // Assert
            result.ShouldBe(metadata);
        }

        [Fact]
        public void Get_ExistingKey_Should_ReturnValue()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });

            // Act
            var value = metadata.Get("key1");

            // Assert
            value.ShouldBe("value1");
        }

        [Fact]
        public void Get_NonExistingKey_Should_ReturnNull()
        {
            // Arrange
            var metadata = Metadata.Create();

            // Act
            var value = metadata.Get("nonExistingKey");

            // Assert
            value.ShouldBeNull();
        }

        [Fact]
        public void Get_Generic_WithCorrectType_Should_ReturnTypedValue()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "intKey", 42 },
                { "stringKey", "test" }
            });

            // Act
            var intValue = metadata.Get<int>("intKey");
            var stringValue = metadata.Get<string>("stringKey");

            // Assert
            intValue.ShouldBe(42);
            stringValue.ShouldBe("test");
        }

        [Fact]
        public void Contains_ExistingKey_Should_ReturnTrue()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });

            // Act & Assert
            metadata.Contains("key1").ShouldBeTrue();
        }

        [Fact]
        public void Contains_NonExistingKey_Should_ReturnFalse()
        {
            // Arrange
            var metadata = Metadata.Create();

            // Act & Assert
            metadata.Contains("nonExistingKey").ShouldBeFalse();
        }

        [Fact]
        public void GetValues_Should_ReturnCopyOfDictionary()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });

            // Act
            var values = metadata.GetValues();
            values["key1"] = "modified";

            // Assert
            metadata.Get("key1").ShouldBe("value1"); // Original unchanged
        }

        [Fact]
        public void ToString_Should_ReturnJsonString()
        {
            // Arrange
            var metadata = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });

            // Act
            var json = metadata.ToString();

            // Assert
            json.ShouldNotBeNullOrEmpty();
            json.ShouldContain("key1");
            json.ShouldContain("value1");
        }

        [Fact]
        public void Equality_WithSameValues_Should_BeEqual()
        {
            // Arrange
            var metadata1 = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });
            var metadata2 = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });

            // Act & Assert
            metadata1.Equals(metadata2).ShouldBeTrue();
        }

        [Fact]
        public void Equality_WithDifferentValues_Should_NotBeEqual()
        {
            // Arrange
            var metadata1 = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value1" }
            });
            var metadata2 = Metadata.Create(new Dictionary<string, object>
            {
                { "key1", "value2" }
            });

            // Act & Assert
            metadata1.Equals(metadata2).ShouldBeFalse();
        }
    }
}
