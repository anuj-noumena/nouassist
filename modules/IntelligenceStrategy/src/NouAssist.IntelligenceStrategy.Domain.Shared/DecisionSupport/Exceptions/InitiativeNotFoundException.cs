using System;

namespace NouAssist.IntelligenceStrategy.DecisionSupport.Exceptions
{
    /// <summary>
    /// Exception thrown when a strategic initiative cannot be found
    /// </summary>
    public class InitiativeNotFoundException : DecisionSupportDomainException
    {
        /// <summary>
        /// ID of the initiative that was not found
        /// </summary>
        public Guid InitiativeId { get; }

        /// <summary>
        /// Creates a new instance of <see cref="InitiativeNotFoundException"/>
        /// </summary>
        /// <param name="initiativeId">ID of the initiative that was not found</param>
        public InitiativeNotFoundException(Guid initiativeId)
            : base("DecisionSupport:ErrorMessage:InitiativeNotFound")
        {
            InitiativeId = initiativeId;
            WithData("initiativeId", initiativeId);
        }
    }
}
