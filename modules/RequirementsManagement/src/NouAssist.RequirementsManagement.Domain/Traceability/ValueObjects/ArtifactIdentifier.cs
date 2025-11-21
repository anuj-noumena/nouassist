using System.Collections.Generic;
using NouAssist.RequirementsManagement.Traceability.Constants;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Traceability.ValueObjects
{
    /// <summary>
    /// Represents a unique identifier for a traceable artifact
    /// </summary>
    public class ArtifactIdentifier : ValueObject
    {
        /// <summary>
        /// Unique ID of the artifact within its system
        /// </summary>
        public string Id { get; private set; }
        
        /// <summary>
        /// Type of the artifact
        /// </summary>
        public ArtifactType Type { get; private set; }
        
        /// <summary>
        /// Version of the artifact, if applicable
        /// </summary>
        public string Version { get; private set; }
        
        /// <summary>
        /// URI to access the artifact, if applicable
        /// </summary>
        public string Uri { get; private set; }

        protected ArtifactIdentifier() { }

        public ArtifactIdentifier(string id, ArtifactType type, string version = null, string uri = null)
        {
            SetId(id);
            Type = type;
            SetVersion(version);
            SetUri(uri);
        }

        private void SetId(string id)
        {
            Id = Check.NotNullOrWhiteSpace(
                id, 
                nameof(id), 
                maxLength: TraceabilityManagementConsts.MaxArtifactIdLength
            );
        }
        
        private void SetVersion(string version)
        {
            if (version != null && version.Length > TraceabilityManagementConsts.MaxArtifactVersionLength)
            {
                throw new BusinessException("TraceabilityManagement:Validation:StringLength")
                    .WithData("propertyName", "Version")
                    .WithData("min", 1)
                    .WithData("max", TraceabilityManagementConsts.MaxArtifactVersionLength);
            }
            
            Version = version;
        }
        
        private void SetUri(string uri)
        {
            if (uri != null && uri.Length > TraceabilityManagementConsts.MaxArtifactUriLength)
            {
                throw new BusinessException("TraceabilityManagement:Validation:StringLength")
                    .WithData("propertyName", "Uri")
                    .WithData("min", 1)
                    .WithData("max", TraceabilityManagementConsts.MaxArtifactUriLength);
            }
            
            Uri = uri;
        }

        /// <summary>
        /// Gets a string representation of this artifact identifier
        /// </summary>
        public override string ToString()
        {
            var result = $"{Type}:{Id}";
            if (!string.IsNullOrEmpty(Version))
            {
                result += $":{Version}";
            }
            return result;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Type;
            if (!string.IsNullOrEmpty(Version))
            {
                yield return Version;
            }
            if (!string.IsNullOrEmpty(Uri))
            {
                yield return Uri;
            }
        }
    }
}
