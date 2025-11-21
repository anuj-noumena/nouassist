using System;
using System.Linq.Expressions;
using NouAssist.RequirementsManagement.QualityAssurance.Aggregates;
using NouAssist.RequirementsManagement.QualityAssurance.Enums;
using NouAssist.RequirementsManagement.QualityAssurance.Specifications.Base;

namespace NouAssist.RequirementsManagement.QualityAssurance.Specifications
{
    /// <summary>
    /// Specifications for defects
    /// </summary>
    public abstract class DefectSpecification : QASpecification<DefectReport>
    {
    }

    /// <summary>
    /// Specification for defects that can be assigned
    /// </summary>
    public class AssignableDefectSpecification : DefectSpecification
    {
        public override Expression<Func<DefectReport, bool>> ToExpression()
        {
            return defect => 
                defect.Status == DefectStatus.New || 
                defect.Status == DefectStatus.Rejected;
        }
        
        /// <summary>
        /// Validates if a defect can be assigned and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateAssignment(DefectReport defect, Guid assigneeId)
        {
            var result = new ValidationResult();
            
            if (defect.Status != DefectStatus.New && defect.Status != DefectStatus.Rejected)
            {
                result.AddError($"Defect must be in New or Rejected status to be assigned, current status: {defect.Status}");
            }
            
            if (assigneeId == Guid.Empty)
            {
                result.AddError("Assignee ID cannot be empty");
            }
            
            return result;
        }
    }

    /// <summary>
    /// Specification for defects that can be marked as fixed
    /// </summary>
    public class FixableDefectSpecification : DefectSpecification
    {
        public override Expression<Func<DefectReport, bool>> ToExpression()
        {
            return defect => 
                defect.Status == DefectStatus.Assigned || 
                defect.Status == DefectStatus.InProgress;
        }
        
        /// <summary>
        /// Validates if a defect can be marked as fixed and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateFixing(DefectReport defect)
        {
            var result = new ValidationResult();
            
            if (defect.Status != DefectStatus.Assigned && defect.Status != DefectStatus.InProgress)
            {
                result.AddError($"Defect must be in Assigned or InProgress status to be fixed, current status: {defect.Status}");
            }
            
            if (defect.AssigneeId == null)
            {
                result.AddError("Defect must be assigned to a user before it can be fixed");
            }
            
            return result;
        }
    }

    /// <summary>
    /// Specification for defects that can be verified
    /// </summary>
    public class VerifiableDefectSpecification : DefectSpecification
    {
        public override Expression<Func<DefectReport, bool>> ToExpression()
        {
            return defect => defect.Status == DefectStatus.Fixed;
        }
        
        /// <summary>
        /// Validates if a defect can be verified and returns detailed reasons if not
        /// </summary>
        public ValidationResult ValidateVerification(DefectReport defect, Guid verifierId)
        {
            var result = new ValidationResult();
            
            if (defect.Status != DefectStatus.Fixed)
            {
                result.AddError($"Defect must be in Fixed status to be verified, current status: {defect.Status}");
            }
            
            if (verifierId == Guid.Empty)
            {
                result.AddError("Verifier ID cannot be empty");
            }
            
            if (defect.FixCommitId == null || string.IsNullOrEmpty(defect.FixCommitId))
            {
                result.AddError("Fix commit ID is required before verification");
            }
            
            return result;
        }
    }

    /// <summary>
    /// Specification for high priority defects
    /// </summary>
    public class HighPriorityDefectSpecification : DefectSpecification
    {
        public override Expression<Func<DefectReport, bool>> ToExpression()
        {
            return defect => 
                (defect.Priority == DefectPriority.Immediate || defect.Priority == DefectPriority.High) &&
                defect.Status != DefectStatus.Verified &&
                defect.Status != DefectStatus.Closed;
        }
    }
}
