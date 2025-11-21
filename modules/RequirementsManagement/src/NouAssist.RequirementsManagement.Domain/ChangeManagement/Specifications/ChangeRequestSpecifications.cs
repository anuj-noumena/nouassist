using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.ChangeManagement.Aggregates;
using NouAssist.RequirementsManagement.ChangeManagement.Enums;
using Volo.Abp.Specifications;

namespace NouAssist.RequirementsManagement.ChangeManagement.Specifications
{
    /// <summary>
    /// Base specification class for RequirementChangeRequest aggregate
    /// </summary>
    public abstract class ChangeRequestSpecification : Specification<RequirementChangeRequest>
    {
    }

    /// <summary>
    /// Specification for change requests awaiting approval
    /// </summary>
    public class PendingApprovalSpecification : ChangeRequestSpecification
    {
        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => request.Status == RequestStatus.PendingApproval;
        }
    }

    /// <summary>
    /// Specification for change requests submitted by a specific user
    /// </summary>
    public class SubmittedByUserSpecification : ChangeRequestSpecification
    {
        private readonly Guid _userId;

        public SubmittedByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => request.SubmitterId == _userId;
        }
    }

    /// <summary>
    /// Specification for change requests with high impact
    /// </summary>
    public class HighImpactSpecification : ChangeRequestSpecification
    {
        private readonly decimal _costThreshold;
        private readonly int _timeThreshold;

        public HighImpactSpecification(decimal costThreshold = 10000m, int timeThreshold = 30)
        {
            _costThreshold = costThreshold;
            _timeThreshold = timeThreshold;
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => 
                request.ImpactStatements.Any(i => 
                    (i.ImpactDetails.CostImpact.HasValue && i.ImpactDetails.CostImpact.Value > _costThreshold) ||
                    (i.ImpactDetails.TimeImpactDays.HasValue && i.ImpactDetails.TimeImpactDays.Value > _timeThreshold));
        }
    }

    /// <summary>
    /// Specification for change requests affecting a specific requirement
    /// </summary>
    public class AffectingRequirementSpecification : ChangeRequestSpecification
    {
        private readonly Guid _requirementId;

        public AffectingRequirementSpecification(Guid requirementId)
        {
            _requirementId = requirementId;
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => request.AffectedRequirement.RequirementId == _requirementId;
        }
    }

    /// <summary>
    /// Specification for change requests with specific status
    /// </summary>
    public class WithStatusSpecification : ChangeRequestSpecification
    {
        private readonly RequestStatus[] _statuses;

        public WithStatusSpecification(params RequestStatus[] statuses)
        {
            _statuses = statuses ?? throw new ArgumentNullException(nameof(statuses));
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => _statuses.Contains(request.Status);
        }
    }

    /// <summary>
    /// Specification for change requests requiring approval from a specific user
    /// </summary>
    public class RequiringApprovalFromUserSpecification : ChangeRequestSpecification
    {
        private readonly Guid _approverId;

        public RequiringApprovalFromUserSpecification(Guid approverId)
        {
            _approverId = approverId;
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => 
                request.Status == RequestStatus.PendingApproval &&
                request.ApprovalSteps.Any(step => 
                    step.ApproverId == _approverId && 
                    step.Decision == ApprovalDecision.Pending);
        }
    }

    /// <summary>
    /// Specification for change requests submitted within a date range
    /// </summary>
    public class SubmittedInDateRangeSpecification : ChangeRequestSpecification
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public SubmittedInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            return request => 
                request.SubmissionDate >= _startDate && 
                request.SubmissionDate <= _endDate;
        }
    }

    /// <summary>
    /// Specification for overdue change requests (submitted more than specified days ago without completion)
    /// </summary>
    public class OverdueSpecification : ChangeRequestSpecification
    {
        private readonly int _overdueDays;

        public OverdueSpecification(int overdueDays = 30)
        {
            _overdueDays = overdueDays;
        }

        public override Expression<Func<RequirementChangeRequest, bool>> ToExpression()
        {
            var overdueDate = DateTime.UtcNow.AddDays(-_overdueDays);
            
            return request => 
                request.SubmissionDate < overdueDate &&
                request.Status != RequestStatus.Completed &&
                request.Status != RequestStatus.Cancelled;
        }
    }
}
