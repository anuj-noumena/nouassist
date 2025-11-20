using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown when an automation definition is invalid
    /// </summary>
    public class InvalidAutomationDefinitionException : PersonalProductivityIntelligenceDomainException
    {
        /// <summary>
        /// Creates a new InvalidAutomationDefinitionException
        /// </summary>
        /// <param name="automationId">ID of the automation</param>
        /// <param name="reason">Reason for the invalid definition</param>
        public InvalidAutomationDefinitionException(Guid automationId, string reason)
            : base("PersonalProductivityIntelligence:ErrorMessage:InvalidAutomationDefinition")
        {
            WithData("automationId", automationId);
            WithData("reason", reason);
        }
    }
}
