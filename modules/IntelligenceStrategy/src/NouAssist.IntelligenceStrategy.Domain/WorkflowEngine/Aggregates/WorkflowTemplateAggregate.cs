namespace NouAssist.IntelligenceStrategy.WorkflowEngine.Aggregates
{
    /// <summary>
    /// Represents a reusable workflow template
    /// </summary>
    public class WorkflowTemplate : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant ID this template belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The name of the template
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of the template
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// The category of the template
        /// </summary>
        public string Category { get; private set; }
        
        /// <summary>
        /// Tags for the template
        /// </summary>
        public List<string> Tags { get; private set; }
        
        /// <summary>
        /// The BPMN diagram for this template
        /// </summary>
        public BpmnDiagram BpmnDiagram { get; private set; }
        
        /// <summary>
        /// The documentation for this template
        /// </summary>
        public string Documentation { get; private set; }
        
        /// <summary>
        /// Indicates if this template is public and available to all tenants
        /// </summary>
        public bool IsPublic { get; private set; }

        protected WorkflowTemplate() { }

        /// <summary>
        /// Creates a new workflow template
        /// </summary>
        /// <param name="id">The ID of the template</param>
        /// <param name="name">The name of the template</param>
        /// <param name="description">The description of the template</param>
        /// <param name="category">The category of the template</param>
        /// <param name="bpmnXml">The BPMN XML content</param>
        /// <param name="bpmnVersion">The BPMN version</param>
        /// <param name="tenantId">The tenant ID</param>
        /// <returns>A new workflow template</returns>
        public static WorkflowTemplate Create(
            Guid id,
            string name,
            string description,
            string category,
            string bpmnXml,
            string bpmnVersion,
            Guid? tenantId = null)
        {
            return new WorkflowTemplate(id, name, description, category, bpmnXml, bpmnVersion, tenantId);
        }

        private WorkflowTemplate(
            Guid id,
            string name,
            string description,
            string category,
            string bpmnXml,
            string bpmnVersion,
            Guid? tenantId = null)
            : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            Description = description;
            Category = category;
            Tags = new List<string>();
            BpmnDiagram = new BpmnDiagram(bpmnXml, bpmnVersion);
            IsPublic = false;
            
            AddDistributedEvent(new WorkflowTemplateCreatedEto
            {
                Id = Id,
                Name = Name,
                TenantId = TenantId
            });
        }
        
        /// <summary>
        /// Updates the basic information of the template
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
        /// Updates the BPMN diagram for this template
        /// </summary>
        /// <param name="bpmnXml">The new BPMN XML content</param>
        /// <param name="bpmnVersion">The BPMN version</param>
        public void UpdateBpmnDiagram(string bpmnXml, string bpmnVersion)
        {
            BpmnDiagram = new BpmnDiagram(bpmnXml, bpmnVersion);
        }
        
        /// <summary>
        /// Updates the documentation for this template
        /// </summary>
        /// <param name="documentation">The new documentation content</param>
        public void UpdateDocumentation(string documentation)
        {
            Documentation = Check.NotNull(
                documentation,
                nameof(documentation),
                maxLength: WorkflowEngineConsts.MaxDocumentationLength
            );
        }
        
        /// <summary>
        /// Sets the tags for this template
        /// </summary>
        /// <param name="tags">The new tags</param>
        public void SetTags(List<string> tags)
        {
            Tags = tags ?? new List<string>();
        }
        
        /// <summary>
        /// Makes this template public and available to all tenants
        /// </summary>
        public void MakePublic()
        {
            IsPublic = true;
        }
        
        /// <summary>
        /// Makes this template private and available only to its tenant
        /// </summary>
        public void MakePrivate()
        {
            IsPublic = false;
        }
        
        /// <summary>
        /// Creates a workflow definition from this template
        /// </summary>
        /// <param name="id">The ID for the new workflow definition</param>
        /// <param name="name">The name for the new workflow definition</param>
        /// <param name="tenantId">The tenant ID for the new workflow definition</param>
        /// <returns>A new workflow definition</returns>
        public WorkflowDefinition CreateWorkflowDefinition(
            Guid id,
            string name,
            Guid? tenantId = null)
        {
            var workflowDefinition = WorkflowDefinition.Create(
                id,
                name,
                Description,
                Category,
                tenantId ?? TenantId
            );
            
            // Get the latest version to update its BPMN diagram
            var version = workflowDefinition.GetLatestVersion();
            version.UpdateBpmnDiagram(BpmnDiagram.XmlContent, BpmnDiagram.BpmnVersion);
            
            return workflowDefinition;
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
