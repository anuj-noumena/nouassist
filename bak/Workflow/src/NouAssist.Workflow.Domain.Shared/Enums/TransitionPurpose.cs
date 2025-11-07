namespace NouAssist.Workflow.Enums;

public enum TransitionPurpose : byte
{
    Standard = 0,      // Default/generic transitions (e.g., "Start Work")
    Delegation = 1,    // Specifically for task delegation (with accept/decline)
    Escalation = 2,    // For priority bumps/escalations
    Resolution = 3,    // Terminal resolutions (e.g., "Resolve", "Close")
    Review = 4,        // Feedback/inspection gates
    Custom = 99        // Extensible for user-defined
}