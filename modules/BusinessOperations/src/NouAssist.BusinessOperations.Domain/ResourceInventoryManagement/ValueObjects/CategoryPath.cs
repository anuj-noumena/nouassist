using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.BusinessOperations.ResourceInventoryManagement.ValueObjects
{
    /// <summary>
    /// Represents a hierarchical category path
    /// </summary>
    public class CategoryPath : ValueObject
    {
        private const char Separator = '/';

        /// <summary>
        /// The path string representation
        /// </summary>
        public string Path { get; private set; }

        private CategoryPath()
        {
        }

        /// <summary>
        /// Creates a new category path from a string representation
        /// </summary>
        /// <param name="path">Path string in format "parent/child/grandchild"</param>
        public CategoryPath(string path)
        {
            Path = NormalizePath(Check.NotNull(path, nameof(path)));
        }

        /// <summary>
        /// Creates a new category path from a list of category names
        /// </summary>
        /// <param name="categories">List of category names from root to leaf</param>
        public CategoryPath(IEnumerable<string> categories)
        {
            Check.NotNull(categories, nameof(categories));
            Path = NormalizePath(string.Join(Separator, categories));
        }

        /// <summary>
        /// Gets the parent category path
        /// </summary>
        /// <returns>Parent category path, or null if this is a root category</returns>
        public CategoryPath GetParent()
        {
            var lastSeparatorIndex = Path.LastIndexOf(Separator);
            if (lastSeparatorIndex <= 0)
            {
                return null; // Root category has no parent
            }

            return new CategoryPath(Path.Substring(0, lastSeparatorIndex));
        }

        /// <summary>
        /// Gets the leaf category name
        /// </summary>
        /// <returns>Leaf category name</returns>
        public string GetLeafName()
        {
            var lastSeparatorIndex = Path.LastIndexOf(Separator);
            if (lastSeparatorIndex < 0)
            {
                return Path; // Single category
            }

            return Path.Substring(lastSeparatorIndex + 1);
        }

        /// <summary>
        /// Gets all category names in the path
        /// </summary>
        /// <returns>List of category names from root to leaf</returns>
        public IReadOnlyList<string> GetCategoryNames()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return new List<string>();
            }

            return Path.Split(Separator, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Checks if this path is a child of the specified parent path
        /// </summary>
        /// <param name="parentPath">Potential parent path</param>
        /// <returns>True if this path is a child of the parent path</returns>
        public bool IsChildOf(CategoryPath parentPath)
        {
            if (parentPath == null || string.IsNullOrEmpty(parentPath.Path))
            {
                return !string.IsNullOrEmpty(Path);
            }

            return Path.StartsWith(parentPath.Path + Separator);
        }

        /// <summary>
        /// Creates a child category path by appending a category name
        /// </summary>
        /// <param name="childName">Child category name</param>
        /// <returns>New category path</returns>
        public CategoryPath CreateChild(string childName)
        {
            Check.NotNullOrWhiteSpace(childName, nameof(childName));
            
            if (string.IsNullOrEmpty(Path))
            {
                return new CategoryPath(childName);
            }

            return new CategoryPath($"{Path}{Separator}{childName}");
        }

        private string NormalizePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            // Remove leading/trailing separators and normalize multiple separators
            var parts = path.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(Separator, parts);
        }

        /// <summary>
        /// Gets the atomic values for value comparison
        /// </summary>
        /// <returns>Collection of atomic values</returns>
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Path;
        }

        /// <summary>
        /// Returns the string representation of the category path
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return Path;
        }
    }
}
