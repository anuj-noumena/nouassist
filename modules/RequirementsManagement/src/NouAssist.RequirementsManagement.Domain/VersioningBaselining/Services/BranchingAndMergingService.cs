using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.RequirementsManagement.VersioningBaselining.Aggregates;
using NouAssist.RequirementsManagement.VersioningBaselining.Entities;
using NouAssist.RequirementsManagement.VersioningBaselining.Enums;
using NouAssist.RequirementsManagement.VersioningBaselining.Exceptions;
using NouAssist.RequirementsManagement.VersioningBaselining.Repositories;
using NouAssist.RequirementsManagement.VersioningBaselining.Services.Interfaces;
using NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.VersioningBaselining.Services
{
    /// <summary>
    /// Implementation of the branching and merging domain service
    /// </summary>
    public class BranchingAndMergingService : DomainService, IBranchingAndMergingService
    {
        private readonly IRequirementBranchRepository _branchRepository;
        private readonly IBaselineDefinitionRepository _baselineRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentUser _currentUser;
        private readonly ILogger<BranchingAndMergingService> _logger;

        public BranchingAndMergingService(
            IRequirementBranchRepository branchRepository,
            IBaselineDefinitionRepository baselineRepository,
            IGuidGenerator guidGenerator,
            ICurrentUser currentUser,
            ILogger<BranchingAndMergingService> logger)
        {
            _branchRepository = branchRepository;
            _baselineRepository = baselineRepository;
            _guidGenerator = guidGenerator;
            _currentUser = currentUser;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<RequirementBranch> CreateBranchFromBaselineAsync(
            string branchName,
            string description,
            Guid baselineId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (await _branchRepository.ExistsByNameAsync(branchName, null, cancellationToken))
            {
                throw new BranchNameAlreadyExistsException(branchName);
            }
            
            // Verify the baseline exists
            await _baselineRepository.GetAsync(baselineId, cancellationToken);
            
            var currentUserId = _currentUser.GetId();
            var currentUserName = _currentUser.Name ?? "Unknown User";
            var author = new AuthorRef(currentUserId, currentUserName);
            
            var branch = RequirementBranch.CreateFromBaseline(
                _guidGenerator.Create(),
                new BranchName(branchName),
                description,
                baselineId,
                author,
                Clock.Now,
                tenantId ?? CurrentTenant.Id);
            
            await _branchRepository.InsertAsync(branch, true, cancellationToken);
            
            _logger.LogInformation(
                "Created branch '{Name}' from baseline {BaselineId}",
                branchName,
                baselineId);
            
            return branch;
        }

        /// <inheritdoc/>
        public async Task<RequirementBranch> CreateBranchFromBranchAsync(
            string branchName,
            string description,
            Guid sourceBranchId,
            Guid? tenantId = null,
            CancellationToken cancellationToken = default)
        {
            if (await _branchRepository.ExistsByNameAsync(branchName, null, cancellationToken))
            {
                throw new BranchNameAlreadyExistsException(branchName);
            }
            
            // Verify the source branch exists
            var sourceBranch = await _branchRepository.GetAsync(sourceBranchId, cancellationToken);
            
            if (sourceBranch.Status != BranchStatus.Active)
            {
                throw new BusinessException("VersionControl:ErrorMessage:InvalidBranchStatus")
                    .WithData("branchId", sourceBranchId)
                    .WithData("status", sourceBranch.Status);
            }
            
            var currentUserId = _currentUser.GetId();
            var currentUserName = _currentUser.Name ?? "Unknown User";
            var author = new AuthorRef(currentUserId, currentUserName);
            
            var branch = RequirementBranch.CreateFromBranch(
                _guidGenerator.Create(),
                new BranchName(branchName),
                description,
                sourceBranchId,
                author,
                Clock.Now,
                tenantId ?? CurrentTenant.Id);
            
            await _branchRepository.InsertAsync(branch, true, cancellationToken);
            
            _logger.LogInformation(
                "Created branch '{Name}' from branch {SourceBranchId}",
                branchName,
                sourceBranchId);
            
            return branch;
        }

        /// <inheritdoc/>
        public async Task<MergeOperation> InitiateMergeAsync(
            Guid sourceBranchId,
            Guid targetBranchId,
            CancellationToken cancellationToken = default)
        {
            var sourceBranch = await _branchRepository.GetAsync(sourceBranchId, cancellationToken);
            
            // Verify the target branch exists
            await _branchRepository.GetAsync(targetBranchId, cancellationToken);
            
            var currentUserId = _currentUser.GetId();
            var currentUserName = _currentUser.Name ?? "Unknown User";
            var author = new AuthorRef(currentUserId, currentUserName);
            
            var mergeId = _guidGenerator.Create();
            var mergeOperation = sourceBranch.InitiateMerge(
                mergeId,
                targetBranchId,
                author,
                Clock.Now);
            
            await _branchRepository.UpdateAsync(sourceBranch, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Initiated merge operation {MergeOperationId} from branch {SourceBranchId} to branch {TargetBranchId}",
                mergeId,
                sourceBranchId,
                targetBranchId);
            
            return mergeOperation;
        }

        /// <inheritdoc/>
        public async Task<MergeOperation> CompleteMergeAsync(
            Guid mergeOperationId,
            MergeResult mergeResult,
            CancellationToken cancellationToken = default)
        {
            // Find the merge operation and its associated branch
            var branches = await _branchRepository.GetListAsync(cancellationToken: cancellationToken);
            RequirementBranch sourceBranch = null;
            
            foreach (var branch in branches)
            {
                var mergeOp = branch.MergeOperations.FirstOrDefault(m => m.Id == mergeOperationId);
                if (mergeOp != null)
                {
                    sourceBranch = branch;
                    break;
                }
            }
            
            if (sourceBranch == null)
            {
                throw new BusinessException("VersionControl:ErrorMessage:MergeOperationNotFound")
                    .WithData("mergeOperationId", mergeOperationId);
            }
            
            var currentUserId = _currentUser.GetId();
            var currentUserName = _currentUser.Name ?? "Unknown User";
            var author = new AuthorRef(currentUserId, currentUserName);
            
            var updatedMergeOperation = sourceBranch.CompleteMerge(
                mergeOperationId,
                mergeResult,
                author,
                Clock.Now);
            
            await _branchRepository.UpdateAsync(sourceBranch, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Completed merge operation {MergeOperationId} with status {Status}",
                mergeOperationId,
                mergeResult.Status);
            
            return updatedMergeOperation;
        }

        /// <inheritdoc/>
        public async Task<MergeOperation> AbortMergeAsync(
            Guid mergeOperationId,
            string reason,
            CancellationToken cancellationToken = default)
        {
            // Find the merge operation and its associated branch
            var branches = await _branchRepository.GetListAsync(cancellationToken: cancellationToken);
            RequirementBranch sourceBranch = null;
            
            foreach (var branch in branches)
            {
                var mergeOp = branch.MergeOperations.FirstOrDefault(m => m.Id == mergeOperationId);
                if (mergeOp != null)
                {
                    sourceBranch = branch;
                    break;
                }
            }
            
            if (sourceBranch == null)
            {
                throw new BusinessException("VersionControl:ErrorMessage:MergeOperationNotFound")
                    .WithData("mergeOperationId", mergeOperationId);
            }
            
            var currentUserId = _currentUser.GetId();
            var currentUserName = _currentUser.Name ?? "Unknown User";
            var author = new AuthorRef(currentUserId, currentUserName);
            
            var updatedMergeOperation = sourceBranch.AbortMerge(
                mergeOperationId,
                reason,
                author,
                Clock.Now);
            
            await _branchRepository.UpdateAsync(sourceBranch, cancellationToken: cancellationToken);
            
            _logger.LogInformation(
                "Aborted merge operation {MergeOperationId}: {Reason}",
                mergeOperationId,
                reason);
            
            return updatedMergeOperation;
        }

        /// <inheritdoc/>
        public async Task AbandonBranchAsync(
            Guid branchId,
            CancellationToken cancellationToken = default)
        {
            var branch = await _branchRepository.GetAsync(branchId, cancellationToken);
            
            branch.Abandon();
            
            await _branchRepository.UpdateAsync(branch, cancellationToken: cancellationToken);
            
            _logger.LogInformation("Abandoned branch {BranchId}", branchId);
        }
    }
}
