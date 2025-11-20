namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates
{
    /// <summary>
    /// Represents a workflow definition
    /// </summary>
    public class WorkflowDefinition : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID this workflow definition belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The name of the workflow definition
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of the workflow definition
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The category of the workflow definition
        /// </summary>
        public string Category { get; private set; }
        
        /// <summary>
        /// Indicates if this workflow definition is published and available for use
        /// </summary>
        public bool IsPublished { get; private set; }
        
        /// <summary>
        /// The ID of the published version, if any
        /// </summary>
        public int? PublishedVersionNumber { get; private set; }
        
        /// <summary>
        /// The latest version number
        /// </summary>
        public int LatestVersionNumber { get; private set; }
        
        /// <summary>
        /// The performance metrics for this workflow definition
        /// </summary>
        public PerformanceMetrics PerformanceMetrics { get; private set; }
        
        /// <summary>
        /// The documentation content for this workflow definition
        /// </summary>
        public string Documentation { get; private set; }
        
        private readonly List<WorkflowVersion> _versions;
        
        /// <summary>
        /// The versions of this workflow definition
        /// </summary>
        public IReadOnlyCollection<WorkflowVersion> Versions => _versions.AsReadOnly();

        protected WorkflowDefinition()
        {
            _versions = new List<WorkflowVersion>();
        }

        /// <summary>
        /// Creates a new workflow definition
        /// </summary>
        /// <param name="id">The ID of the workflow definition</param>
        /// <param name="name">The name of the workflow definition</param>
        /// <param name="description">The description of the workflow definition</param>
        /// <param name="category">The category of the workflow definition</param>
        /// <param name="tenantId">The tenant ID</param>
        /// <returns>A new workflow definition</returns>
        public static WorkflowDefinition Create(
            Guid id,
            string name,
            string description,
            string category,
            Guid? tenantId = null)
        {
            return new WorkflowDefinition(id, name, description, category, tenantId);
        }

        private WorkflowDefinition(
            Guid id,
            string name,
            string description,
            string category,
            Guid? tenantId = null)
            : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            Description = description;
            Category = category;
            IsPublished = false;
            LatestVersionNumber = 0;
            _versions = new List<WorkflowVersion>();
            
            // Create initial version
            CreateNewVersion("Initial version");
        }
        
        /// <summary>
        /// Updates the basic information of the workflow definition
        /// </summary>
        /// <param name="name">The new name</param>
        /// <param name="description">The new description</param>
        /// <param name="category">The new category</param>
        public void UpdateBasicInfo(string name, string description, string category)
        {
            SetName(name);
            Description = description;
            Category = category;
        }
        
        /// <summary>
        /// Creates a new version of the workflow definition
        /// </summary>
        /// <param name="comments">Optional comments for the new version</param>
        /// <returns>The new version</returns>
        public WorkflowVersion CreateNewVersion(string comments = null)
        {
            LatestVersionNumber++;
            
            var newVersion = new WorkflowVersion(
                Id,
                LatestVersionNumber,
                comments
            );
            
            _versions.Add(newVersion);
            
            AddDistributedEvent(new WorkflowVersionedEto
            {
                Id = Id,
                Name = Name,
                VersionNumber = LatestVersionNumber,
                TenantId = TenantId
            });
            
            return newVersion;
        }
        
        /// <summary>
        /// Publishes a specific version of the workflow definition
        /// </summary>
        /// <param name="versionNumber">The version number to publish</param>
        public void PublishVersion(int versionNumber)
        {
            var version = _versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
            if (version == null)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:VersionNotFound",
                    $"Version {versionNumber} not found."
                );
            }
            
            if (!version.Validate())
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:InvalidWorkflowVersion",
                    "The workflow version is not valid and cannot be published."
                );
            }
            
            // If there's already a published version, unpublish it
            if (PublishedVersionNumber.HasValue)
            {
                var currentPublishedVersion = _versions.FirstOrDefault(v => v.VersionNumber == PublishedVersionNumber.Value);
                if (currentPublishedVersion != null)
                {
                    // We don't actually change the IsPublished flag on the version to keep a history
                    // of which versions were published, but we update the aggregate's reference
                }
            }
            
            version.Publish();
            IsPublished = true;
            PublishedVersionNumber = versionNumber;
            
            AddDistributedEvent(new WorkflowPublishedEto
            {
                Id = Id,
                Name = Name,
                VersionNumber = versionNumber,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Gets a specific version of the workflow definition
        /// </summary>
        /// <param name="versionNumber">The version number</param>
        /// <returns>The workflow version</returns>
        public WorkflowVersion GetVersion(int versionNumber)
        {
            var version = _versions.FirstOrDefault(v => v.VersionNumber == versionNumber);
            if (version == null)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:VersionNotFound",
                    $"Version {versionNumber} not found."
                );
            }
            
            return version;
        }
        
        /// <summary>
        /// Gets the published version of the workflow definition
        /// </summary>
        /// <returns>The published workflow version</returns>
        public WorkflowVersion GetPublishedVersion()
        {
            if (!PublishedVersionNumber.HasValue)
            {
                throw new WorkflowEngineDomainException(
                    "WorkflowEngine:ErrorMessage:NoPublishedVersion",
                    "This workflow has no published version."
                );
            }
            
            return GetVersion(PublishedVersionNumber.Value);
        }
        
        /// <summary>
        /// Gets the latest version of the workflow definition
        /// </summary>
        /// <returns>The latest workflow version</returns>
        public WorkflowVersion GetLatestVersion()
        {
            return GetVersion(LatestVersionNumber);
        }
        
        /// <summary>
        /// Updates the performance metrics for this workflow definition
        /// </summary>
        /// <param name="metrics">The new performance metrics</param>
        public void UpdatePerformanceMetrics(PerformanceMetrics metrics)
        {
            PerformanceMetrics = metrics;
            
            AddDistributedEvent(new WorkflowPerformanceAnalyzedEto
            {
                Id = Id,
                Name = Name,
                AverageCycleTimeMinutes = metrics.AverageCycleTimeMinutes,
                BottleneckScore = metrics.BottleneckScore,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Updates the documentation for this workflow definition
        /// </summary>
        /// <param name="documentation">The new documentation content</param>
        public void UpdateDocumentation(string documentation)
        {
            Documentation = Check.NotNull(
                documentation,
                nameof(documentation),
                maxLength: WorkflowEngineConsts.MaxDocumentationLength
            );
            
            AddDistributedEvent(new WorkflowDocumentationGeneratedEto
            {
                Id = Id,
                Name = Name,
                TenantId = TenantId
            });
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: WorkflowEngineConsts.MaxNameLength
            );
        }
    }
}
