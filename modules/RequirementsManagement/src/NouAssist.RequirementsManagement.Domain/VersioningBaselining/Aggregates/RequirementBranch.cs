namespace NouAssist.RequirementsManagement.VersioningBaselining.Aggregates
{
    /// <summary>
    /// Aggregate root representing a branch of requirements
    /// </summary>
    public class RequirementBranch : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// ID of the tenant this branch belongs to (if multi-tenant)
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// Name of the branch
        /// </summary>
        public BranchName Name { get; private set; }
        
        /// <summary>
        /// Description of the branch
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Status of the branch
        /// </summary>
        public BranchStatus Status { get; private set; }
        
        /// <summary>
        /// ID of the source baseline or branch from which this branch was created
        /// </summary>
        public Guid SourceId { get; private set; }
        
        /// <summary>
        /// Type of the source ("Baseline" or "Branch")
        /// </summary>
        public string SourceType { get; private set; }
        
        /// <summary>
        /// Reference to the user who created this branch
        /// </summary>
        public AuthorRef CreatedBy { get; private set; }
        
        private readonly List<MergeOperation> _mergeOperations;
        
        /// <summary>
        /// Collection of merge operations involving this branch
        /// </summary>
        public IReadOnlyCollection<MergeOperation> MergeOperations => _mergeOperations.AsReadOnly();

        protected RequirementBranch()
        {
            _mergeOperations = new List<MergeOperation>();
        }

        /// <summary>
        /// Creates a new RequirementBranch from a baseline
        /// </summary>
        public static RequirementBranch CreateFromBaseline(
            Guid id,
            BranchName name,
            string description,
            Guid baselineId,
            AuthorRef createdBy,
            DateTime creationTime,
            Guid? tenantId)
        {
            return new RequirementBranch(
                id,
                name,
                description,
                baselineId,
                "Baseline",
                createdBy,
                creationTime,
                tenantId);
        }

        /// <summary>
        /// Creates a new RequirementBranch from another branch
        /// </summary>
        public static RequirementBranch CreateFromBranch(
            Guid id,
            BranchName name,
            string description,
            Guid sourceBranchId,
            AuthorRef createdBy,
            DateTime creationTime,
            Guid? tenantId)
        {
            return new RequirementBranch(
                id,
                name,
                description,
                sourceBranchId,
                "Branch",
                createdBy,
                creationTime,
                tenantId);
        }

        private RequirementBranch(
            Guid id,
            BranchName name,
            string description,
            Guid sourceId,
            string sourceType,
            AuthorRef createdBy,
            DateTime creationTime,
            Guid? tenantId) : base(id)
        {
            Name = name;
            Description = description;
            SourceId = sourceId;
            SourceType = sourceType;
            CreatedBy = createdBy;
            Status = BranchStatus.Active;
            TenantId = tenantId;
            _mergeOperations = new List<MergeOperation>();
            
            AddDistributedEvent(new BranchCreatedEto
            {
                TenantId = TenantId,
                BranchId = Id,
                Name = name.Value,
                SourceId = sourceId,
                SourceType = sourceType,
                CreatorId = createdBy.Id,
                CreationTime = creationTime
            });
        }

        /// <summary>
        /// Updates the branch properties
        /// </summary>
        public void Update(string description)
        {
            EnsureActive();
            
            Description = description;
        }

        /// <summary>
        /// Initiates a merge operation from this branch to a target branch
        /// </summary>
        public MergeOperation InitiateMerge(
            Guid mergeId,
            Guid targetBranchId,
            AuthorRef initiatedBy,
            DateTime initiatedAt)
        {
            EnsureActive();
            
            if (Id == targetBranchId)
            {
                throw new BusinessException("VersionControl:ErrorMessage:CannotMergeToBranchItself")
                    .WithData("branchId", Id);
            }
            
            var mergeOperation = new MergeOperation(
                mergeId, 
                Id, 
                targetBranchId, 
                initiatedBy, 
                initiatedAt);
                
            _mergeOperations.Add(mergeOperation);
            
            return mergeOperation;
        }

        /// <summary>
        /// Completes a merge operation
        /// </summary>
        public MergeOperation CompleteMerge(
            Guid mergeOperationId,
            MergeResult result,
            AuthorRef completedBy,
            DateTime completedAt)
        {
            var mergeOperation = FindMergeOperation(mergeOperationId);
            
            mergeOperation.Complete(result, completedBy, completedAt);
            
            // If this was the source branch and the merge was completed successfully, mark it as merged
            if (mergeOperation.SourceBranchId == Id && result.Status == MergeStatus.Completed)
            {
                Status = BranchStatus.Merged;
            }
            
            AddDistributedEvent(new MergeOperationCompletedEto
            {
                TenantId = TenantId,
                MergeOperationId = mergeOperationId,
                SourceBranchId = mergeOperation.SourceBranchId,
                SourceBranchName = Name.Value,
                TargetBranchId = mergeOperation.TargetBranchId,
                TargetBranchName = "Target Branch", // This should be populated by the service
                Status = result.Status,
                MergedById = completedBy.Id,
                CompletionTime = completedAt,
                MergedRequirementCount = result.MergedRequirementCount,
                ResolvedConflictsCount = result.ConflictCount
            });
            
            return mergeOperation;
        }

        /// <summary>
        /// Aborts a merge operation
        /// </summary>
        public MergeOperation AbortMerge(
            Guid mergeOperationId,
            string reason,
            AuthorRef abortedBy,
            DateTime abortedAt)
        {
            var mergeOperation = FindMergeOperation(mergeOperationId);
            
            mergeOperation.Abort(reason, abortedBy, abortedAt);
            
            return mergeOperation;
        }

        /// <summary>
        /// Marks the branch as abandoned
        /// </summary>
        public void Abandon()
        {
            EnsureActive();
            
            Status = BranchStatus.Abandoned;
        }

        private MergeOperation FindMergeOperation(Guid mergeOperationId)
        {
            var mergeOperation = _mergeOperations.FirstOrDefault(m => m.Id == mergeOperationId);
            
            if (mergeOperation == null)
            {
                throw new BusinessException("VersionControl:ErrorMessage:MergeOperationNotFound")
                    .WithData("mergeOperationId", mergeOperationId)
                    .WithData("branchId", Id);
            }
            
            if (mergeOperation.Status != MergeStatus.InProgress)
            {
                throw new BusinessException("VersionControl:ErrorMessage:MergeOperationAlreadyCompleted")
                    .WithData("mergeOperationId", mergeOperationId)
                    .WithData("status", mergeOperation.Status);
            }
            
            return mergeOperation;
        }

        private void EnsureActive()
        {
            if (Status != BranchStatus.Active)
            {
                throw new BusinessException("VersionControl:ErrorMessage:InvalidBranchStatus")
                    .WithData("branchId", Id)
                    .WithData("status", Status);
            }
        }
    }
}
