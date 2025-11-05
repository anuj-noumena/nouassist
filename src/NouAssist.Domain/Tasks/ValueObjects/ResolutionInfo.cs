using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace NouAssist.Tasks.ValueObjects
{
    public class ResolutionInfo : ValueObject
    {
        public string Resolution { get; private set; }
        public string ResolutionNote { get; private set; }

        private ResolutionInfo() { }

        // Represents the default state (unresolved)
        public static ResolutionInfo Unresolved => new() 
        { 
            Resolution = string.Empty, 
            ResolutionNote = string.Empty 
        };

        // Factory method to create a resolved state
        public static ResolutionInfo Create(string resolution, string resolutionNote = null)
        {
            return new ResolutionInfo
            {
                Resolution = resolution ?? string.Empty,
                ResolutionNote = resolutionNote ?? string.Empty
            };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Resolution;
            yield return ResolutionNote;
        }
    }
}