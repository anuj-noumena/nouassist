namespace NouAssist.RequirementsManagement.QualityAssurance.Services
{
    /// <summary>
    /// Implementation of defect management domain service
    /// </summary>
    public class DefectManager : DomainService, IDefectManager
    {
        private readonly IDefectReportRepository _defectRepository;
        private readonly ILogger<DefectManager> _logger;

        public DefectManager(
            IDefectReportRepository defectRepository,
            ILogger<DefectManager> logger)
        {
            _defectRepository = defectRepository;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<DefectReport> CreateAsync(
            string title,
            string description,
            string stepsToReproduce,
            string expectedOutcome,
            string actualOutcome,
            DefectSeverity severity,
            DefectPriority priority,
            string environment,
            string version,
            Guid? testRunId = null,
            Guid? testCycleId = null,
            Guid? testPlanId = null)
        {
            try
            {
                var defect = DefectReport.Create(
                    GuidGenerator.Create(),
                    title,
                    description,
                    stepsToReproduce,
                    expectedOutcome,
                    actualOutcome,
                    severity,
                    priority,
                    environment,
                    version,
                    testRunId,
                    testCycleId,
                    testPlanId,
                    CurrentTenant.Id);
                
                await _defectRepository.InsertAsync(defect);
                
                _logger.LogInformation("Created defect {DefectId}: {Title}", defect.Id, title);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create defect: {Title}", title);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> AssignAsync(Guid defectId, Guid assigneeId)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.Assign(assigneeId, Clock.Now);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Assigned defect {DefectId} to user {AssigneeId}", defectId, assigneeId);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to assign defect {DefectId} to user {AssigneeId}", defectId, assigneeId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> MarkAsInProgressAsync(Guid defectId)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.MarkAsInProgress();
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Marked defect {DefectId} as in progress", defectId);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark defect {DefectId} as in progress", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> MarkAsFixedAsync(Guid defectId, string fixCommitId)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.MarkAsFixed(fixCommitId, Clock.Now);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Marked defect {DefectId} as fixed with commit {CommitId}", 
                    defectId, fixCommitId);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark defect {DefectId} as fixed", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> VerifyFixAsync(Guid defectId, string fixedInVersion)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.VerifyFix(CurrentUser.Id.Value, fixedInVersion, Clock.Now);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Verified fix for defect {DefectId} in version {Version}", 
                    defectId, fixedInVersion);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to verify fix for defect {DefectId}", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> RejectFixAsync(Guid defectId, string reason)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.RejectFix(reason);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Rejected fix for defect {DefectId}. Reason: {Reason}", 
                    defectId, reason);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reject fix for defect {DefectId}", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> CloseAsync(Guid defectId)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.Close(Clock.Now);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Closed defect {DefectId}", defectId);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close defect {DefectId}", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectReport> ReopenAsync(Guid defectId, string reason)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.Reopen(reason);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Reopened defect {DefectId}. Reason: {Reason}", 
                    defectId, reason);
                
                return defect;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reopen defect {DefectId}", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<DefectComment> AddCommentAsync(Guid defectId, string text)
        {
            try
            {
                var defect = await _defectRepository.GetWithDetailsAsync(defectId);
                
                var comment = defect.AddComment(text);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Added comment to defect {DefectId}", defectId);
                
                return comment;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add comment to defect {DefectId}", defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task LinkRequirementAsync(Guid defectId, Guid requirementId)
        {
            try
            {
                var defect = await _defectRepository.GetAsync(defectId);
                
                defect.AddAffectedRequirement(requirementId);
                
                await _defectRepository.UpdateAsync(defect);
                
                _logger.LogInformation("Linked requirement {RequirementId} to defect {DefectId}", 
                    requirementId, defectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to link requirement {RequirementId} to defect {DefectId}", 
                    requirementId, defectId);
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<List<DefectReport>> GetHighPriorityDefectsAsync()
        {
            try
            {
                var spec = new HighPriorityDefectSpecification();
                
                var defects = await _defectRepository.GetListAsync(spec);
                
                return defects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get high priority defects");
                throw;
            }
        }
    }
}
