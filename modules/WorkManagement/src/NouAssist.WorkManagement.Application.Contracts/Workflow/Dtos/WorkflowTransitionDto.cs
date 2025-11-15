using System;
using System.Collections.Generic;
using NouAssist.WorkManagement.Workflow.Enums;

namespace NouAssist.WorkManagement.Workflow.Dtos
{
    public class WorkflowTransitionDto
    {
        public Guid Id { get; set; }
        public Guid WorkflowId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid FromStatusId { get; set; }
        public Guid ToStatusId { get; set; }
        
        // NEW: Added Purpose
        public TransitionPurpose Purpose { get; set; }
        
        // REMOVED: AllowedRoles and AllowedUserIds (moved to TransitionPermissionPolicy)
        // These are now managed separately through TransitionPermissionPolicy
        
        public bool RequiresComment { get; set; }
        public bool RequiresResolution { get; set; }
        public bool IsGlobal { get; set; }
        public int OrderIndex { get; set; }
        public string ButtonLabel { get; set; }
        public string ButtonColor { get; set; }
        public bool HasConditions { get; set; }
        public bool HasValidators { get; set; }
        
        // REMOVED: HasRestrictions (now handled by TransitionPermissionPolicy)
        
        public List<WorkflowActionDto> Actions { get; set; } = new();
    }
}