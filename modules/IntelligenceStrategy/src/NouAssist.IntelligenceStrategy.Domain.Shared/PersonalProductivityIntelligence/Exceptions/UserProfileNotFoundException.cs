using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown when a user productivity profile is not found
    /// </summary>
    public class UserProfileNotFoundException : PersonalProductivityIntelligenceDomainException
    {
        /// <summary>
        /// Creates a new UserProfileNotFoundException
        /// </summary>
        /// <param name="userId">ID of the user</param>
        public UserProfileNotFoundException(Guid userId)
            : base("PersonalProductivityIntelligence:ErrorMessage:UserProfileNotFound")
        {
            WithData("userId", userId);
        }
    }
}
