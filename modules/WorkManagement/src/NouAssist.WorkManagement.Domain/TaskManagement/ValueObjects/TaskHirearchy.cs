using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class TaskHierarchy : ValueObject
    {
        public Guid? ParentId { get; private set; }
        public int Level { get; private set; }
        public string Path { get; private set; } // e.g., "root/parent/child"

        private TaskHierarchy() { }

        public static TaskHierarchy Root()
        {
            return new TaskHierarchy
            {
                ParentId = null,
                Level = 0,
                Path = "/"
            };
        }

        public static TaskHierarchy Child(Guid parentId, int parentLevel, string parentPath)
        {
            return new TaskHierarchy
            {
                ParentId = parentId,
                Level = parentLevel + 1,
                Path = $"{parentPath}/{parentId}"
            };
        }

        public bool IsRoot => !ParentId.HasValue;
        public bool IsChild => ParentId.HasValue;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ParentId;
            yield return Level;
            yield return Path;
        }
    }
}
