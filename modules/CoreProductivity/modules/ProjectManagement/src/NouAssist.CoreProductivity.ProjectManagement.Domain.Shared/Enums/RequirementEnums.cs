namespace NouAssist.CoreProductivity.ProjectManagement.Enums
{
    public enum ProjectRequirementType : byte
    {
        Functional = 1,
        NonFunctional = 2,
        Business = 3,
        Technical = 4,
        Regulatory = 5
    }

    public enum ProjectRequirementStatus : byte
    {
        Proposed = 1,
        Approved = 2,
        InProgress = 3,
        Implemented = 4,
        Verified = 5,
        Rejected = 6
    }

    public enum RefinementStatus : byte
    {
        /// <summary>
        /// Raw - newly created, not yet reviewed
        /// </summary>
        Raw = 1,

        /// <summary>
        /// In Discussion - team is analyzing
        /// </summary>
        InDiscussion = 2,

        /// <summary>
        /// In Progress - currently being refined
        /// </summary>
        InProgress = 3,

        /// <summary>
        /// Refined - ready for sprint planning
        /// </summary>
        Refined = 4,

        /// <summary>
        /// Ready - approved and prioritized for development
        /// </summary>
        Ready = 5,

        /// <summary>
        /// Blocked - cannot be refined due to dependencies
        /// </summary>
        Blocked = 6,

        /// <summary>
        /// Rejected - not valuable or feasible
        /// </summary>
        Rejected = 7,

        /// <summary>
        /// Deferred - postponed to future release
        /// </summary>
        Deferred = 8
    }

    public enum PriorityLevel : byte
    {
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum ProjectMethodology : byte
    {
        Scrum = 1,
        Hybrid = 2,
        Waterfall = 3
    }

    public enum ProjectEventType : byte
    {
        Milestone = 1,
        Deadline = 2,
        Meeting = 3,
        Review = 4,
        Sprint = 5,
        Release = 6
    }
}
