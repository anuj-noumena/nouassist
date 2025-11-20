namespace NouAssist.BusinessOperations.ProjectExecution.Aggregates
{
    /// <summary>
    /// Project aggregate root representing a distinct piece of work with objectives, timelines, and resources
    /// </summary>
    public class Project : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        #region Properties
        
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Project details value object
        /// </summary>
        public ProjectDetails Details { get; private set; }
        
        /// <summary>
        /// Current status of the project
        /// </summary>
        public ProjectStatus Status { get; private set; }
        
        /// <summary>
        /// Actual end date of the project, if completed
        /// </summary>
        public DateTime? ActualEndDate { get; private set; }
        
        /// <summary>
        /// Optional ID of the organization associated with the project
        /// </summary>
        public Guid? OrganizationId { get; private set; }
        
        /// <summary>
        /// Optional ID of the contact associated with the project
        /// </summary>
        public Guid? ContactId { get; private set; }
        
        /// <summary>
        /// Collection of project phases
        /// </summary>
        private readonly List<ProjectPhase> _phases;
        public IReadOnlyCollection<ProjectPhase> Phases => _phases.AsReadOnly();
        
        /// <summary>
        /// Collection of project milestones
        /// </summary>
        private readonly List<ProjectMilestone> _milestones;
        public IReadOnlyCollection<ProjectMilestone> Milestones => _milestones.AsReadOnly();
        
        /// <summary>
        /// Collection of project members
        /// </summary>
        private readonly List<ProjectMember> _members;
        public IReadOnlyCollection<ProjectMember> Members => _members.AsReadOnly();
        
        #endregion
        
        #region Constructors
        
        protected Project()
        {
            _phases = new List<ProjectPhase>();
            _milestones = new List<ProjectMilestone>();
            _members = new List<ProjectMember>();
        }
        
        /// <summary>
        /// Creates a new instance of Project aggregate
        /// </summary>
        public static Project Create(
            Guid id,
            string name,
            string description,
            DateTime startDate,
            DateTime plannedEndDate,
            Guid? tenantId = null,
            Guid? organizationId = null,
            Guid? contactId = null)
        {
            return new Project(
                id,
                name,
                description,
                startDate,
                plannedEndDate,
                tenantId,
                organizationId,
                contactId);
        }
        
        private Project(
            Guid id,
            string name,
            string description,
            DateTime startDate,
            DateTime plannedEndDate,
            Guid? tenantId = null,
            Guid? organizationId = null,
            Guid? contactId = null) : base(id)
        {
            Details = new ProjectDetails(name, description, startDate, plannedEndDate);
            Status = ProjectStatus.Planning;
            TenantId = tenantId;
            OrganizationId = organizationId;
            ContactId = contactId;
            
            _phases = new List<ProjectPhase>();
            _milestones = new List<ProjectMilestone>();
            _members = new List<ProjectMember>();
            
            AddDistributedEvent(new ProjectCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Details.Name,
                StartDate = Details.StartDate,
                PlannedEndDate = Details.PlannedEndDate,
                Status = Status
            });
        }
        
        #endregion
        
        #region Public Methods
        
        /// <summary>
        /// Updates the project details
        /// </summary>
        public void UpdateDetails(
            string name,
            string description,
            DateTime startDate,
            DateTime plannedEndDate)
        {
            EnsureCanBeModified();
            
            Details = new ProjectDetails(name, description, startDate, plannedEndDate);
        }
        
        /// <summary>
        /// Updates the project status
        /// </summary>
        public void UpdateStatus(ProjectStatus newStatus, DateTime? actualEndDate = null)
        {
            if (!CanTransitionTo(newStatus))
            {
                throw new InvalidProjectStatusTransitionException(Status, newStatus);
            }
            
            var previousStatus = Status;
            Status = newStatus;
            
            if (newStatus == ProjectStatus.Completed)
            {
                ActualEndDate = actualEndDate ?? Clock.Now;
            }
            
            AddDistributedEvent(new ProjectStatusUpdatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Details.Name,
                PreviousStatus = previousStatus,
                NewStatus = newStatus
            });
        }
        
        /// <summary>
        /// Adds a new phase to the project
        /// </summary>
        public ProjectPhase AddPhase(
            string name,
            DateTime startDate,
            DateTime endDate,
            int order)
        {
            EnsureCanBeModified();
            
            if (_phases.Any(p => p.Name == name))
            {
                throw new BusinessException("ProjectExecution:ErrorMessage:DuplicateName")
                    .WithData("entityType", "Project Phase")
                    .WithData("name", name);
            }
            
            var phase = new ProjectPhase(Id, name, startDate, endDate, order);
            _phases.Add(phase);
            
            return phase;
        }
        
        /// <summary>
        /// Adds a milestone to the project
        /// </summary>
        public ProjectMilestone AddMilestone(
            string name,
            DateTime targetDate,
            string description = null)
        {
            EnsureCanBeModified();
            
            if (_milestones.Any(m => m.Name == name))
            {
                throw new BusinessException("ProjectExecution:ErrorMessage:DuplicateName")
                    .WithData("entityType", "Project Milestone")
                    .WithData("name", name);
            }
            
            var milestone = new ProjectMilestone(Id, name, targetDate, description);
            _milestones.Add(milestone);
            
            return milestone;
        }
        
        /// <summary>
        /// Adds a member to the project
        /// </summary>
        public ProjectMember AddMember(
            Guid userId,
            string role,
            DateTime joinDate)
        {
            EnsureCanBeModified();
            
            if (_members.Any(m => m.UserId == userId))
            {
                throw new BusinessException("ProjectExecution:Validation:MemberAlreadyExists");
            }
            
            var member = new ProjectMember(Id, userId, role, joinDate);
            _members.Add(member);
            
            return member;
        }
        
        /// <summary>
        /// Removes a member from the project
        /// </summary>
        public void RemoveMember(Guid userId)
        {
            EnsureCanBeModified();
            
            var member = _members.FirstOrDefault(m => m.UserId == userId);
            if (member != null)
            {
                _members.Remove(member);
            }
        }
        
        /// <summary>
        /// Sets organization associated with the project
        /// </summary>
        public void SetOrganization(Guid organizationId)
        {
            OrganizationId = organizationId;
        }
        
        /// <summary>
        /// Sets contact associated with the project
        /// </summary>
        public void SetContact(Guid contactId)
        {
            ContactId = contactId;
        }
        
        #endregion
        
        #region Private Methods
        
        private bool CanTransitionTo(ProjectStatus targetStatus)
        {
            switch (Status)
            {
                case ProjectStatus.Planning:
                    return targetStatus == ProjectStatus.Active || targetStatus == ProjectStatus.Cancelled;
                
                case ProjectStatus.Active:
                    return targetStatus == ProjectStatus.OnHold || 
                           targetStatus == ProjectStatus.Completed || 
                           targetStatus == ProjectStatus.Cancelled;
                
                case ProjectStatus.OnHold:
                    return targetStatus == ProjectStatus.Active || 
                           targetStatus == ProjectStatus.Cancelled;
                
                case ProjectStatus.Completed:
                case ProjectStatus.Cancelled:
                    return false;
                
                default:
                    return false;
            }
        }
        
        private void EnsureCanBeModified()
        {
            if (Status == ProjectStatus.Completed || Status == ProjectStatus.Cancelled)
            {
                throw new BusinessException("ProjectExecution:ErrorMessage:ProjectCannotBeModified")
                    .WithData("status", Status);
            }
        }
        
        #endregion
    }
}
