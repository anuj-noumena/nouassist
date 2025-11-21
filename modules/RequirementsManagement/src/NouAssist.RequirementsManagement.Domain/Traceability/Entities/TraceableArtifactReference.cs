using System;
using NouAssist.RequirementsManagement.Traceability.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.Traceability.Entities
{
    /// <summary>
    /// Represents a reference to a traceable artifact
    /// </summary>
    public class TraceableArtifactReference : Entity
    {
        /// <summary>
        /// Identifier of the trace link this artifact belongs to
        /// </summary>
        public Guid TraceLinkId { get; private set; }
        
        /// <summary>
        /// Artifact identifier value object
        /// </summary>
        public ArtifactIdentifier ArtifactIdentifier { get; private set; }
        
        /// <summary>
        /// Display name for the artifact
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Last verified timestamp
        /// </summary>
        public DateTime? LastVerified { get; private set; }
        
        /// <summary>
        /// Last modified timestamp from the source system
        /// </summary>
        public DateTime? LastModified { get; private set; }
        
        /// <summary>
        /// Indicates if this artifact reference is considered suspect and needs verification
        /// </summary>
        public bool IsSuspect { get; private set; }
        
        protected TraceableArtifactReference() { }

        internal TraceableArtifactReference(
            Guid traceLinkId,
            ArtifactIdentifier artifactIdentifier,
            string name)
        {
            TraceLinkId = traceLinkId;
            ArtifactIdentifier = artifactIdentifier;
            Name = !string.IsNullOrEmpty(name) ? name : artifactIdentifier.Id;
            IsSuspect = false;
        }

        /// <summary>
        /// Marks this artifact reference as verified at the current time
        /// </summary>
        internal void MarkAsVerified(DateTime verificationTime)
        {
            LastVerified = verificationTime;
            IsSuspect = false;
        }

        /// <summary>
        /// Updates the last modified timestamp from source system and marks as suspect if needed
        /// </summary>
        internal void UpdateModificationInfo(DateTime modificationTime)
        {
            if (LastVerified.HasValue && modificationTime > LastVerified.Value)
            {
                IsSuspect = true;
            }
            
            LastModified = modificationTime;
        }

        public override object[] GetKeys()
        {
            return new object[] { TraceLinkId, ArtifactIdentifier.Id, ArtifactIdentifier.Type };
        }
    }
}
