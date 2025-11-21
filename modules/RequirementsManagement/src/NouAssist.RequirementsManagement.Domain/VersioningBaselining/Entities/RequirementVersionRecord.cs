using System;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp.Domain.Entities;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Entities
{
    /// <summary>
    /// Entity representing a specific version of a requirement
    /// </summary>
    public class RequirementVersionRecord : Entity
    {
        /// <summary>
        /// ID of the requirement this version belongs to
        /// </summary>
        public Guid RequirementId { get; private set; }
        
        /// <summary>
        /// Version identifier
        /// </summary>
        public VersionIdentifier Version { get; private set; }
        
        /// <summary>
        /// Reference to the author who created this version
        /// </summary>
        public AuthorRef Author { get; private set; }
        
        /// <summary>
        /// Comment describing the changes in this version
        /// </summary>
        public ChangeComment ChangeComment { get; private set; }
        
        /// <summary>
        /// Timestamp when this version was created
        /// </summary>
        public DateTime CreationTime { get; private set; }
        
        /// <summary>
        /// Content snapshot of the requirement at this version
        /// (could be a serialized JSON representation of the requirement)
        /// </summary>
        public string ContentSnapshot { get; private set; }

        protected RequirementVersionRecord() { }

        internal RequirementVersionRecord(
            Guid requirementId,
            VersionIdentifier version,
            AuthorRef author,
            ChangeComment changeComment,
            DateTime creationTime,
            string contentSnapshot)
        {
            RequirementId = requirementId;
            Version = version;
            Author = author;
            ChangeComment = changeComment;
            CreationTime = creationTime;
            ContentSnapshot = contentSnapshot;
        }

        public override object[] GetKeys()
        {
            return new object[] { RequirementId, Version.Value };
        }
    }
}
