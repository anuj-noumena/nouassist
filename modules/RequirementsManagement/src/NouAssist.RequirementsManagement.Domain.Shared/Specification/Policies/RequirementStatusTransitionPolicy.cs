namespace NouAssist.RequirementsManagement.Specification.Policies
{
    /// <summary>
    /// Defines policies for requirement status transitions
    /// </summary>
    public static class RequirementStatusTransitionPolicy
    {
        private static readonly Dictionary<RequirementStatus, List<RequirementStatus>> AllowedTransitions =
            new Dictionary<RequirementStatus, List<RequirementStatus>>
            {
                {
                    RequirementStatus.Draft,
                    new List<RequirementStatus>
                    {
                        RequirementStatus.UnderReview,
                        RequirementStatus.Archived
                    }
                },
                {
                    RequirementStatus.UnderReview,
                    new List<RequirementStatus>
                    {
                        RequirementStatus.Draft,
                        RequirementStatus.Approved,
                        RequirementStatus.Rejected,
                        RequirementStatus.Archived
                    }
                },
                {
                    RequirementStatus.Approved,
                    new List<RequirementStatus>
                    {
                        RequirementStatus.Implemented,
                        RequirementStatus.Archived
                    }
                },
                {
                    RequirementStatus.Rejected,
                    new List<RequirementStatus>
                    {
                        RequirementStatus.Draft,
                        RequirementStatus.Archived
                    }
                },
                {
                    RequirementStatus.Implemented,
                    new List<RequirementStatus>
                    {
                        RequirementStatus.Tested,
                        RequirementStatus.Archived
                    }
                },
                {
                    RequirementStatus.Tested,
                    new List<RequirementStatus>
                    {
                        RequirementStatus.Archived
                    }
                },
                {
                    RequirementStatus.Archived,
                    new List<RequirementStatus>()
                }
            };

        /// <summary>
        /// Checks if a status transition is allowed
        /// </summary>
        public static bool IsTransitionAllowed(RequirementStatus fromStatus, RequirementStatus toStatus)
        {
            return AllowedTransitions.ContainsKey(fromStatus) &&
                   AllowedTransitions[fromStatus].Contains(toStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from a given status
        /// </summary>
        public static IReadOnlyList<RequirementStatus> GetAllowedTransitions(RequirementStatus fromStatus)
        {
            return AllowedTransitions.ContainsKey(fromStatus)
                ? AllowedTransitions[fromStatus].AsReadOnly()
                : new List<RequirementStatus>().AsReadOnly();
        }
    }
}
