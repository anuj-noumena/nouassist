using System.Collections.Generic;
using NouAssist.RequirementsManagement.Traceability.Constants;
using NouAssist.RequirementsManagement.Traceability.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Traceability.ValueObjects
{
    /// <summary>
    /// Represents a unique identifier for a trace link
    /// </summary>
    public class LinkIdentifier : ValueObject
    {
        /// <summary>
        /// Unique identifier for the link
        /// </summary>
        public string Value { get; private set; }

        protected LinkIdentifier() { }

        public LinkIdentifier(string value)
        {
            SetValue(value);
        }

        private void SetValue(string value)
        {
            Value = Check.NotNullOrWhiteSpace(
                value, 
                nameof(value), 
                maxLength: TraceabilityManagementConsts.MaxLinkIdentifierLength
            );
        }

        /// <summary>
        /// Creates a link identifier from source and target artifacts and link type
        /// </summary>
        public static LinkIdentifier Create(ArtifactIdentifier source, ArtifactIdentifier target, LinkType linkType)
        {
            return new LinkIdentifier($"{source}:{target}:{linkType}");
        }

        /// <summary>
        /// Generates a consistent link identifier based on the source and target artifacts and link type
        /// </summary>
        public static string GenerateLinkId(string sourceId, string targetId, LinkType linkType)
        {
            return $"{sourceId}:{targetId}:{linkType}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
