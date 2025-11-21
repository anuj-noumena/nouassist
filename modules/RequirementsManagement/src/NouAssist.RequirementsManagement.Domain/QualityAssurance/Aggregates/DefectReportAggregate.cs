using System;
using System.Collections.Generic;
using NouAssist.RequirementsManagement.QualityAssurance.Constants;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Events;
using NouAssist.RequirementsManagement.QualityAssurance.Exceptions;
using NouAssist.RequirementsManagement.QualityAssurance.Specifications;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace NouAssist.RequirementsManagement.QualityAssurance.Aggregates
{
    /// <summary>
    /// Represents a defect report that manages the lifecycle of a reported issue
    /// </summary>
    public class DefectReport : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// The tenant that this defect belongs to
        /// </summary>
        public Guid? TenantId { get; protected set; }
        
        /// <summary>
        /// The title of the defect
        /// </summary>
        public string Title { get; private set; }
        
        /// <summary>
        /// The detailed description of the defect
        /// </summary>
        public string Description { get; private set; }
        
        /// <summary>
        /// Steps to reproduce the defect
        /// </summary>
        public string StepsToReproduce { get; private set; }
        
        /// <summary>
        /// The expected behavior or outcome
        /// </summary>
        public string ExpectedOutcome { get; private set; }
        
        /// <summary>
        /// The actual behavior or outcome
        /// </summary>
        public string ActualOutcome { get; private set; }
        
        /// <summary>
        /// The severity of the defect
        /// </summary>
        public DefectSeverity Severity { get; private set; }
        
        /// <summary>
        /// The priority of the defect
        /// </summary>
        public DefectPriority Priority { get; private set; }
        
        /// <summary>
        /// The current status of the defect
        /// </summary>
        public DefectStatus Status { get; private set; }
        
        /// <summary>
        /// The user assigned to fix the defect
        /// </summary>
        public Guid? AssigneeId { get; private set; }
        
        /// <summary>
        /// The test run that discovered this defect
        /// </summary>
        public Guid? TestRunId { get; private set; }
        
        /// <summary>
        /// The test cycle associated with this defect
        /// </summary>
        public Guid? TestCycleId { get; private set; }
        
        /// <summary>
        /// The test plan associated with this defect
        /// </summary>
        public Guid? TestPlanId { get; private set; }
        
        /// <summary>
        /// Environment where the defect was found
        /// </summary>
        public string Environment { get; private set; }
        
        /// <summary>
        /// Version or build where the defect was found
        /// </summary>
        public string Version { get; private set; }
        
        /// <summary>
        /// The commit ID that fixed the defect
        /// </summary>
        public string FixCommitId { get; private set; }
        
        /// <summary>
        /// User who verified the fix
        /// </summary>
        public Guid? VerifierId { get; private set; }
        
        /// <summary>
        /// Version or build where the fix was verified
        /// </summary>
        public string FixedInVersion { get; private set; }
        
        /// <summary>
        /// Timestamp when the defect was assigned
        /// </summary>
        public DateTime? AssignedTime { get; private set; }
        
        /// <summary>
        /// Timestamp when the defect was fixed
        /// </summary>
        public DateTime? FixedTime { get; private set; }
        
        /// <summary>
        /// Timestamp when the fix was verified
        /// </summary>
        public DateTime? VerifiedTime { get; private set; }
        
        /// <summary>
        /// Timestamp when the defect was closed
        /// </summary>
        public DateTime? ClosedTime { get; private set; }
        
        /// <summary>
        /// List of additional comments on this defect
        /// </summary>
        private readonly List<DefectComment> _comments;
        
        /// <summary>
        /// Read-only access to comments
        /// </summary>
        public IReadOnlyCollection<DefectComment> Comments => _comments.AsReadOnly();
        
        /// <summary>
        /// List of requirements affected by this defect
        /// </summary>
        private readonly List<Guid> _affectedRequirementIds;
        
        /// <summary>
        /// Read-only access to affected requirement IDs
        /// </summary>
        public IReadOnlyCollection<Guid> AffectedRequirementIds => _affectedRequirementIds.AsReadOnly();

        protected DefectReport()
        {
            _comments = new List<DefectComment>();
            _affectedRequirementIds = new List<Guid>();
        }

        /// <summary>
        /// Factory method to create a new defect report
        /// </summary>
        public static DefectReport Create(
            Guid id,
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
            Guid? testPlanId = null,
            Guid? tenantId = null)
        {
            return new DefectReport(
                id,
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
                tenantId);
        }

        private DefectReport(
            Guid id,
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
            Guid? testPlanId = null,
            Guid? tenantId = null) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            SetStepsToReproduce(stepsToReproduce);
            SetExpectedOutcome(expectedOutcome);
            SetActualOutcome(actualOutcome);
            Severity = severity;
            Priority = priority;
            Environment = environment;
            Version = version;
            TestRunId = testRunId;
            TestCycleId = testCycleId;
            TestPlanId = testPlanId;
            TenantId = tenantId;
            Status = DefectStatus.New;
            _comments = new List<DefectComment>();
            _affectedRequirementIds = new List<Guid>();
            
            AddDistributedEvent(new DefectCreatedEto
            {
                Id = Id,
                Title = Title,
                Severity = Severity,
                Priority = Priority,
                TestRunId = TestRunId,
                CreatorId = CreatorId.Value,
                CreationTime = CreationTime,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Updates the basic information of the defect
        /// </summary>
        public void UpdateBasicInfo(
            string title,
            string description,
            string stepsToReproduce,
            string expectedOutcome,
            string actualOutcome,
            DefectSeverity severity,
            DefectPriority priority,
            string environment,
            string version)
        {
            if (Status == DefectStatus.Closed || Status == DefectStatus.Verified)
            {
                throw new BusinessRuleViolationException("Cannot update a defect that is verified or closed");
            }

            SetTitle(title);
            SetDescription(description);
            SetStepsToReproduce(stepsToReproduce);
            SetExpectedOutcome(expectedOutcome);
            SetActualOutcome(actualOutcome);
            Severity = severity;
            Priority = priority;
            Environment = environment;
            Version = version;
        }

        /// <summary>
        /// Assigns the defect to a developer
        /// </summary>
        public void Assign(Guid assigneeId, DateTime assignedTime)
        {
            var spec = new AssignableDefectSpecification();
            var validationResult = spec.ValidateAssignment(this, assigneeId);
            
            if (!validationResult.IsValid)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot assign defect: {string.Join(", ", validationResult.Errors)}");
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Assigned;
            AssigneeId = assigneeId;
            AssignedTime = assignedTime;
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = assignedTime,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the defect as in progress
        /// </summary>
        public void MarkAsInProgress()
        {
            if (Status != DefectStatus.Assigned)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.InProgress);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.InProgress;
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the defect as fixed
        /// </summary>
        public void MarkAsFixed(string fixCommitId, DateTime fixedTime)
        {
            var spec = new FixableDefectSpecification();
            var validationResult = spec.ValidateFixing(this);
            
            if (!validationResult.IsValid)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot mark defect as fixed: {string.Join(", ", validationResult.Errors)}");
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Fixed;
            FixCommitId = fixCommitId;
            FixedTime = fixedTime;
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = fixedTime,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Verifies the fix for the defect
        /// </summary>
        public void VerifyFix(Guid verifierId, string fixedInVersion, DateTime verifiedTime)
        {
            var spec = new VerifiableDefectSpecification();
            var validationResult = spec.ValidateVerification(this, verifierId);
            
            if (!validationResult.IsValid)
            {
                throw new BusinessRuleViolationException(
                    $"Cannot verify defect fix: {string.Join(", ", validationResult.Errors)}");
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Verified;
            VerifierId = verifierId;
            FixedInVersion = fixedInVersion;
            VerifiedTime = verifiedTime;
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = verifierId,
                ChangeTime = verifiedTime,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Rejects the fix for the defect
        /// </summary>
        public void RejectFix(string reason)
        {
            if (Status != DefectStatus.Fixed)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.Rejected);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Rejected;
            
            // Add a comment with the rejection reason
            AddComment(reason);
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Closes the defect
        /// </summary>
        public void Close(DateTime closedTime)
        {
            if (Status != DefectStatus.Verified && 
                Status != DefectStatus.CannotReproduce && 
                Status != DefectStatus.NotADefect && 
                Status != DefectStatus.Duplicate && 
                Status != DefectStatus.Deferred)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.Closed);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Closed;
            ClosedTime = closedTime;
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = closedTime,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the defect as cannot reproduce
        /// </summary>
        public void MarkAsCannotReproduce(string reason)
        {
            if (Status != DefectStatus.New && 
                Status != DefectStatus.Assigned && 
                Status != DefectStatus.InProgress)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.CannotReproduce);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.CannotReproduce;
            
            // Add a comment with the reason
            AddComment(reason);
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the defect as not a defect
        /// </summary>
        public void MarkAsNotADefect(string reason)
        {
            if (Status != DefectStatus.New && 
                Status != DefectStatus.Assigned && 
                Status != DefectStatus.InProgress)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.NotADefect);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.NotADefect;
            
            // Add a comment with the reason
            AddComment(reason);
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Marks the defect as a duplicate of another defect
        /// </summary>
        public void MarkAsDuplicate(Guid duplicateOfDefectId, string reason)
        {
            if (Status != DefectStatus.New && 
                Status != DefectStatus.Assigned && 
                Status != DefectStatus.InProgress)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.Duplicate);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Duplicate;
            
            // Add a comment with the duplicate information
            AddComment($"Duplicate of defect {duplicateOfDefectId}. {reason}");
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        /// <summary>
        /// Defers the defect to a future release
        /// </summary>
        public void Defer(string targetRelease, string reason)
        {
            if (Status != DefectStatus.New && 
                Status != DefectStatus.Assigned && 
                Status != DefectStatus.InProgress)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.Deferred);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.Deferred;
            
            // Add a comment with the deferral information
            AddComment($"Deferred to {targetRelease}. Reason: {reason}");
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,

                AddDistributedEvent(new DefectStatusChangedEto
                {
                    Id = Id,
                    Title = Title,
                    PreviousStatus = previousStatus,
                    NewStatus = Status,
                    ChangedById = LastModifierId.Value,
                    ChangeTime = Clock.Now,
                    TenantId = TenantId
                });
            }
        }

        /// <summary>
        /// Adds a comment to the defect
        /// </summary>
        public DefectComment AddComment(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new BusinessRuleViolationException("Comment text cannot be empty");
            }

            var comment = new DefectComment
            {
                Text = text.Length > QADomainConstants.MaxCommentLength 
                    ? text.Substring(0, QADomainConstants.MaxCommentLength) 
                    : text,
                CreationTime = Clock.Now,
                CreatorId = CurrentUser.Id
            };

            _comments.Add(comment);
            return comment;
        }

        /// <summary>
        /// Adds a requirement to the list of affected requirements
        /// </summary>
        public void AddAffectedRequirement(Guid requirementId)
        {
            if (!_affectedRequirementIds.Contains(requirementId))
            {
                _affectedRequirementIds.Add(requirementId);
            }
        }

        /// <summary>
        /// Removes a requirement from the list of affected requirements
        /// </summary>
        public void RemoveAffectedRequirement(Guid requirementId)
        {
            _affectedRequirementIds.Remove(requirementId);
        }

        /// <summary>
        /// Updates the severity of the defect
        /// </summary>
        public void UpdateSeverity(DefectSeverity severity)
        {
            if (Status == DefectStatus.Closed || Status == DefectStatus.Verified)
            {
                throw new BusinessRuleViolationException("Cannot update severity of a defect that is verified or closed");
            }

            Severity = severity;
        }

        /// <summary>
        /// Updates the priority of the defect
        /// </summary>
        public void UpdatePriority(DefectPriority priority)
        {
            if (Status == DefectStatus.Closed || Status == DefectStatus.Verified)
            {
                throw new BusinessRuleViolationException("Cannot update priority of a defect that is verified or closed");
            }

            Priority = priority;
        }

        /// <summary>
        /// Reopens a defect that was previously closed or marked as cannot reproduce, not a defect, etc.
        /// </summary>
        public void Reopen(string reason)
        {
            if (Status != DefectStatus.Closed && 
                Status != DefectStatus.CannotReproduce && 
                Status != DefectStatus.NotADefect && 
                Status != DefectStatus.Duplicate && 
                Status != DefectStatus.Deferred)
            {
                throw new InvalidDefectStatusTransitionException(Status, DefectStatus.New);
            }

            DefectStatus previousStatus = Status;
            Status = DefectStatus.New;
            
            // Reset relevant fields
            AssigneeId = null;
            FixCommitId = null;
            VerifierId = null;
            FixedInVersion = null;
            ClosedTime = null;
            
            // Add a comment with the reopening reason
            AddComment($"Defect reopened. Reason: {reason}");
            
            AddDistributedEvent(new DefectStatusChangedEto
            {
                Id = Id,
                Title = Title,
                PreviousStatus = previousStatus,
                NewStatus = Status,
                ChangedById = LastModifierId.Value,
                ChangeTime = Clock.Now,
                TenantId = TenantId
            });
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: QADomainConstants.MaxDefectTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: QADomainConstants.MaxDefectDescriptionLength
            );
        }

        private void SetStepsToReproduce(string stepsToReproduce)
        {
            StepsToReproduce = Check.NotNullOrWhiteSpace(
                stepsToReproduce,
                nameof(stepsToReproduce),
                maxLength: QADomainConstants.MaxStepsToReproduceLength
            );
        }

        private void SetExpectedOutcome(string expectedOutcome)
        {
            ExpectedOutcome = Check.NotNullOrWhiteSpace(
                expectedOutcome,
                nameof(expectedOutcome),
                maxLength: QADomainConstants.MaxExpectedOutcomeLength
            );
        }

        private void SetActualOutcome(string actualOutcome)
        {
            ActualOutcome = Check.NotNullOrWhiteSpace(
                actualOutcome,
                nameof(actualOutcome),
                maxLength: QADomainConstants.MaxActualOutcomeLength
            );
        }
    }

    /// <summary>
    /// Represents a comment on a defect report
    /// </summary>
    public class DefectComment : Entity, IHasCreationTime, IMayHaveCreator
    {
        /// <summary>
        /// The text content of the comment
        /// </summary>
        public string Text { get; internal set; }
        
        /// <summary>
        /// When the comment was created
        /// </summary>
        public DateTime CreationTime { get; set; }
        
        /// <summary>
        /// Who created the comment
        /// </summary>
        public Guid? CreatorId { get; set; }
    }
}
