namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Aggregates
{
    /// <summary>
    /// Aggregate root representing a fundraising round
    /// </summary>
    public class FundraisingRound : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public RoundType Type { get; private set; }
        public RoundStage Stage { get; private set; }
        public decimal TargetAmount { get; private set; }
        public decimal CommittedAmount { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? TargetCloseDate { get; private set; }
        public DateTime? ActualCloseDate { get; private set; }
        
        private readonly List<PipelineStage> _pipelineStages;
        public IReadOnlyCollection<PipelineStage> PipelineStages => _pipelineStages.AsReadOnly();
        
        protected FundraisingRound()
        {
            _pipelineStages = new List<PipelineStage>();
        }
        
        /// <summary>
        /// Factory method to create a new fundraising round
        /// </summary>
        public static FundraisingRound Create(
            Guid id,
            string name,
            RoundType type,
            decimal targetAmount,
            DateTime startDate,
            DateTime? targetCloseDate = null,
            string description = null,
            Guid? tenantId = null)
        {
            return new FundraisingRound(
                id,
                name,
                type,
                targetAmount,
                startDate,
                targetCloseDate,
                description,
                tenantId);
        }
        
        private FundraisingRound(
            Guid id,
            string name,
            RoundType type,
            decimal targetAmount,
            DateTime startDate,
            DateTime? targetCloseDate = null,
            string description = null,
            Guid? tenantId = null) : base(id)
        {
            SetName(name);
            Description = description;
            Type = type;
            SetTargetAmount(targetAmount);
            StartDate = startDate;
            TargetCloseDate = targetCloseDate;
            TenantId = tenantId;
            
            Stage = RoundStage.Planning;
            CommittedAmount = 0;
            _pipelineStages = new List<PipelineStage>();
            
            // Add default pipeline stages
            InitializeDefaultPipelineStages();
            
            AddDistributedEvent(new FundraisingRoundCreatedEto
            {
                Id = Id,
                TenantId = TenantId,
                Name = Name,
                Type = Type,
                Stage = Stage,
                TargetAmount = TargetAmount
            });
        }
        
        /// <summary>
        /// Updates the basic information of the fundraising round
        /// </summary>
        public void UpdateBasicInfo(
            string name,
            RoundType type,
            decimal targetAmount,
            DateTime startDate,
            DateTime? targetCloseDate = null,
            string description = null)
        {
            SetName(name);
            Description = description;
            Type = type;
            SetTargetAmount(targetAmount);
            StartDate = startDate;
            TargetCloseDate = targetCloseDate;
        }
        
        /// <summary>
        /// Changes the stage of the fundraising round
        /// </summary>
        public void ChangeStage(RoundStage newStage)
        {
            ValidateStageTransition(newStage);
            Stage = newStage;
            
            if (newStage == RoundStage.Completed)
            {
                ActualCloseDate = DateTime.Now;
                
                AddDistributedEvent(new FundraisingRoundClosedEto
                {
                    Id = Id,
                    TenantId = TenantId,
                    Name = Name,
                    RaisedAmount = CommittedAmount,
                    InvestorCount = 0, // This would need to be calculated from prospect data
                    ClosedDate = ActualCloseDate.Value
                });
            }
        }
        
        /// <summary>
        /// Adds a new pipeline stage to the fundraising round
        /// </summary>
        public PipelineStage AddPipelineStage(string name, int order, string description = null)
        {
            if (_pipelineStages.Any(s => s.Name == name))
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:DuplicatePipelineStageName")
                    .WithData("name", name);
            }
            
            var stage = new PipelineStage(
                GuidGenerator.Create(),
                Id,
                name,
                order,
                description);
            
            _pipelineStages.Add(stage);
            return stage;
        }
        
        /// <summary>
        /// Updates an existing pipeline stage
        /// </summary>
        public void UpdatePipelineStage(Guid stageId, string name, int order, string description = null)
        {
            var stage = _pipelineStages.FirstOrDefault(s => s.Id == stageId);
            if (stage == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PipelineStageNotFound")
                    .WithData("id", stageId);
            }
            
            // Check for duplicate name
            if (_pipelineStages.Any(s => s.Name == name && s.Id != stageId))
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:DuplicatePipelineStageName")
                    .WithData("name", name);
            }
            
            stage.Update(name, order, description);
        }
        
        /// <summary>
        /// Removes a pipeline stage
        /// </summary>
        public void RemovePipelineStage(Guid stageId)
        {
            var stage = _pipelineStages.FirstOrDefault(s => s.Id == stageId);
            if (stage == null)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:PipelineStageNotFound")
                    .WithData("id", stageId);
            }
            
            _pipelineStages.Remove(stage);
        }
        
        /// <summary>
        /// Updates the committed amount for the round
        /// </summary>
        public void UpdateCommittedAmount(decimal amount)
        {
            if (amount < 0)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:InvalidAmount");
            }
            
            CommittedAmount = amount;
        }
        
        private void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(
                name,
                nameof(name),
                maxLength: FundraisingPipelineConsts.FundraisingRound.MaxNameLength);
        }
        
        private void SetTargetAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new BusinessException("FundraisingPipeline:ErrorMessage:InvalidAmount");
            }
            
            TargetAmount = amount;
        }
        
        private void ValidateStageTransition(RoundStage newStage)
        {
            // Implement stage transition rules
            // This is simplified; in a real implementation, you might have more complex rules
            if (Stage == RoundStage.Completed || Stage == RoundStage.Cancelled)
            {
                throw new InvalidRoundStageTransitionException(Stage, newStage);
            }
            
            // Prevent backwards movement in the stage progression (simplified)
            if ((int)newStage < (int)Stage && newStage != RoundStage.Cancelled)
            {
                throw new InvalidRoundStageTransitionException(Stage, newStage);
            }
        }
        
        private void InitializeDefaultPipelineStages()
        {
            AddPipelineStage("Research", 1, "Initial research and identification of prospects");
            AddPipelineStage("Initial Contact", 2, "First outreach to prospects");
            AddPipelineStage("Meeting Scheduled", 3, "Meeting or call scheduled");
            AddPipelineStage("Pitched", 4, "Formal pitch presented to prospect");
            AddPipelineStage("Due Diligence", 5, "Prospect conducting due diligence");
            AddPipelineStage("Negotiation", 6, "Term sheet negotiation");
            AddPipelineStage("Committed", 7, "Verbal or written commitment received");
            AddPipelineStage("Closed", 8, "Investment legally closed");
        }
    }
}
