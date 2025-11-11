using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.TaskManagement.ValueObjects
{
    public class FlagInfo : ValueObject
    {
        public bool IsFlagged { get; private set; }
        public string FlaggedReason { get; private set; }

        private FlagInfo() { }

        // Represents the default state (not flagged)
        public static FlagInfo NotFlagged => new() 
        { 
            IsFlagged = false, 
            FlaggedReason = string.Empty 
        };

        // Factory method to create a flagged state
        public static FlagInfo Create(string reason)
        {
            return new FlagInfo
            {
                IsFlagged = true,
                FlaggedReason = reason ?? string.Empty
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return IsFlagged;
            yield return FlaggedReason;
        }
    }
}