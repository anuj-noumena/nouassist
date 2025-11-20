namespace NouAssist.RequirementsManagement.Traceability.Aggregates
{
    /// <summary>
    /// Represents a trace relationship between two artifacts
    /// </summary>
    public class TraceLink : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        
        /// <summary>
        /// Source artifact in the trace relationship
        /// </summary>
        public TraceableArtifactReference SourceArtifact { get; private set; }
        
        /// <summary>
        /// Target artifact in the trace relationship
        /// </summary>
        public TraceableArtifactReference TargetArtifact { get; private set; }
        
        /// <summary>
        /// Type of relationship between the artifacts
        /// </summary>
        public LinkType LinkType { get; private set; }
        
        /// <summary>
        /// Direction of the relationship between artifacts
        /// </summary>
        public LinkDirection LinkDirection { get; private set; }
        
        /// <summary>
        /// Custom link type description when LinkType is Custom
        /// </summary>
        public string CustomLinkType { get; private set; }
        
        /// <summary>
        /// Optional description of the trace link
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Unique textual identifier for this link
        /// </summary>
        public LinkIdentifier Identifier { get; private set; }
        
        /// <summary>
        /// Last time this link was verified by a user
        /// </summary>
        public DateTime? LastVerifiedTime { get; private set; }
        
        /// <summary>
        /// User who last verified this link
        /// </summary>
        public Guid? LastVerifiedBy { get; private set; }
        
        /// <summary>
        /// Indicates if this link is considered suspect and needs verification
        /// </summary>
        public bool IsSuspect { get; private set; }

        protected TraceLink() { }

        /// <summary>
        /// Creates a new trace link between two artifacts
        /// </summary>
        internal TraceLink(
            Guid id,
            ArtifactIdentifier sourceArtifactId,
            string sourceArtifactName,
            ArtifactIdentifier targetArtifactId,
            string targetArtifactName,
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            TenantId = tenantId;
            LinkType = linkType;
            LinkDirection = linkDirection;
            SetDescription(description);
            SetCustomLinkType(customLinkType);
            
            // Create the artifact references
            SourceArtifact = new TraceableArtifactReference(id, sourceArtifactId, sourceArtifactName);
            TargetArtifact = new TraceableArtifactReference(id, targetArtifactId, targetArtifactName);
            
            // Create the link identifier
            Identifier = LinkIdentifier.Create(sourceArtifactId, targetArtifactId, linkType);
            
            IsSuspect = false;
        }

        /// <summary>
        /// Factory method to create a new trace link
        /// </summary>
        public static TraceLink Create(
            Guid id,
            ArtifactIdentifier sourceArtifactId,
            string sourceArtifactName,
            ArtifactIdentifier targetArtifactId,
            string targetArtifactName,
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null,
            Guid? tenantId = null)
        {
            return new TraceLink(
                id,
                sourceArtifactId,
                sourceArtifactName,
                targetArtifactId,
                targetArtifactName,
                linkType,
                linkDirection,
                customLinkType,
                description,
                tenantId);
        }

        /// <summary>
        /// Updates the properties of the trace link
        /// </summary>
        public void Update(
            LinkType linkType,
            LinkDirection linkDirection,
            string customLinkType = null,
            string description = null)
        {
            LinkType = linkType;
            LinkDirection = linkDirection;
            SetCustomLinkType(customLinkType);
            SetDescription(description);
            
            // Mark as needing verification if link type or direction changes
            IsSuspect = true;
            
            AddDistributedEvent(new TraceLinkUpdatedEto
            {
                Id = Id,
                SourceArtifactId = SourceArtifact.ArtifactIdentifier.Id,
                TargetArtifactId = TargetArtifact.ArtifactIdentifier.Id,
                LinkType = LinkType,
                LinkDirection = LinkDirection,
                TenantId = TenantId,
                LastModificationTime = Clock.Now
            });
        }

        /// <summary>
        /// Marks the trace link as verified
        /// </summary>
        public void MarkAsVerified(DateTime verificationTime, Guid userId)
        {
            LastVerifiedTime = verificationTime;
            LastVerifiedBy = userId;
            IsSuspect = false;
            
            // Mark both artifacts as verified
            SourceArtifact.MarkAsVerified(verificationTime);
            TargetArtifact.MarkAsVerified(verificationTime);
        }

        /// <summary>
        /// Updates the source artifact's last modified time and marks link as suspect if needed
        /// </summary>
        public void UpdateSourceArtifactModificationInfo(DateTime modificationTime)
        {
            SourceArtifact.UpdateModificationInfo(modificationTime);
            
            // If source artifact is suspect, the link is suspect
            if (SourceArtifact.IsSuspect)
            {
                IsSuspect = true;
            }
        }

        /// <summary>
        /// Updates the target artifact's last modified time and marks link as suspect if needed
        /// </summary>
        public void UpdateTargetArtifactModificationInfo(DateTime modificationTime)
        {
            TargetArtifact.UpdateModificationInfo(modificationTime);
            
            // If target artifact is suspect, the link is suspect
            if (TargetArtifact.IsSuspect)
            {
                IsSuspect = true;
            }
        }

        /// <summary>
        /// Set the description of the trace link
        /// </summary>
        private void SetDescription(string description)
        {
            if (description != null && description.Length > TraceabilityManagementConsts.MaxLinkDescriptionLength)
            {
                throw new BusinessException("TraceabilityManagement:Validation:StringLength")
                    .WithData("propertyName", "Description")
                    .WithData("min", 1)
                    .WithData("max", TraceabilityManagementConsts.MaxLinkDescriptionLength);
            }
            
            Description = description;
        }

        /// <summary>
        /// Set the custom link type
        /// </summary>
        private void SetCustomLinkType(string customLinkType)
        {
            if (LinkType != LinkType.Custom && !string.IsNullOrEmpty(customLinkType))
            {
                throw new BusinessException("TraceabilityManagement:ErrorMessage:InvalidOperation")
                    .WithData("message", "Custom link type should only be set when LinkType is Custom");
            }
            
            if (LinkType == LinkType.Custom && string.IsNullOrEmpty(customLinkType))
            {
                throw new BusinessException("TraceabilityManagement:Validation:Required")
                    .WithData("propertyName", "CustomLinkType");
            }
            
            if (customLinkType != null && customLinkType.Length > TraceabilityManagementConsts.MaxCustomLinkTypeLength)
            {
                throw new BusinessException("TraceabilityManagement:Validation:StringLength")
                    .WithData("propertyName", "CustomLinkType")
                    .WithData("min", 1)
                    .WithData("max", TraceabilityManagementConsts.MaxCustomLinkTypeLength);
            }
            
            CustomLinkType = customLinkType;
        }
    }
}
