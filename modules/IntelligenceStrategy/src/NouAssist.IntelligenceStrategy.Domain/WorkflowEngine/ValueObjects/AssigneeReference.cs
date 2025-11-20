namespace NouAssist.IntelligenceStrategy.WorkflowEngine.ValueObjects
{
    /// <summary>
    /// Value object representing a reference to an assignee (user, team, or role)
    /// </summary>
    public class AssigneeReference : ValueObject
    {
        /// <summary>
        /// The type of assignee (User, Team, or Role)
        /// </summary>
        public AssigneeType Type { get; private set; }
        
        /// <summary>
        /// The ID of the assignee
        /// </summary>
        public Guid Id { get; private set; }
        
        /// <summary>
        /// Optional display name of the assignee
        /// </summary>
        public string DisplayName { get; private set; }

        protected AssigneeReference() { }

        public AssigneeReference(AssigneeType type, Guid id, string displayName = null)
        {
            Type = type;
            Id = id;
            DisplayName = displayName;
        }
        
        /// <summary>
        /// Creates a user assignee reference
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <param name="displayName">Optional display name</param>
        /// <returns>A new AssigneeReference</returns>
        public static AssigneeReference ForUser(Guid userId, string displayName = null)
        {
            return new AssigneeReference(AssigneeType.User, userId, displayName);
        }
        
        /// <summary>
        /// Creates a team assignee reference
        /// </summary>
        /// <param name="teamId">The ID of the team</param>
        /// <param name="displayName">Optional display name</param>
        /// <returns>A new AssigneeReference</returns>
        public static AssigneeReference ForTeam(Guid teamId, string displayName = null)
        {
            return new AssigneeReference(AssigneeType.Team, teamId, displayName);
        }
        
        /// <summary>
        /// Creates a role assignee reference
        /// </summary>
        /// <param name="roleId">The ID of the role</param>
        /// <param name="displayName">Optional display name</param>
        /// <returns>A new AssigneeReference</returns>
        public static AssigneeReference ForRole(Guid roleId, string displayName = null)
        {
            return new AssigneeReference(AssigneeType.Role, roleId, displayName);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Type;
            yield return Id;
            yield return DisplayName;
        }
    }
}
