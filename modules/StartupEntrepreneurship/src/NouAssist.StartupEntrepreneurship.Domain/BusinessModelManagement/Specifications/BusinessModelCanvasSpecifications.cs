using System;
using System.Linq;
using System.Linq.Expressions;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications.Base;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Specifications
{
    /// <summary>
    /// Specification for published business model canvases
    /// </summary>
    public class PublishedCanvasSpecification : BusinessModelManagementSpecification<BusinessModelCanvas>
    {
        public override Expression<Func<BusinessModelCanvas, bool>> ToExpression()
        {
            return canvas => canvas.GetOverallValidationProgress() >= 80.0;
        }
    }

    /// <summary>
    /// Specification for canvases owned by a specific user
    /// </summary>
    public class CanvasOwnedByUserSpecification : BusinessModelManagementSpecification<BusinessModelCanvas>
    {
        private readonly Guid _userId;

        public CanvasOwnedByUserSpecification(Guid userId)
        {
            _userId = userId;
        }

        public override Expression<Func<BusinessModelCanvas, bool>> ToExpression()
        {
            return canvas => canvas.OwnerId == _userId;
        }
    }

    /// <summary>
    /// Specification for canvases with specific validation status
    /// </summary>
    public class CanvasWithValidationStatusSpecification : BusinessModelManagementSpecification<BusinessModelCanvas>
    {
        private readonly ValidationStatus _status;

        public CanvasWithValidationStatusSpecification(ValidationStatus status)
        {
            _status = status;
        }

        public override Expression<Func<BusinessModelCanvas, bool>> ToExpression()
        {
            return canvas => canvas.Components
                .Where(c => c.VersionId == canvas.CurrentVersionId)
                .Any(c => c.ValidationState.Status == _status);
        }
    }

    /// <summary>
    /// Specification for canvases modified within a date range
    /// </summary>
    public class CanvasModifiedInDateRangeSpecification : BusinessModelManagementSpecification<BusinessModelCanvas>
    {
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;

        public CanvasModifiedInDateRangeSpecification(DateTime startDate, DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }

        public override Expression<Func<BusinessModelCanvas, bool>> ToExpression()
        {
            return canvas => canvas.LastModificationTime >= _startDate && 
                           canvas.LastModificationTime <= _endDate;
        }
    }
}
