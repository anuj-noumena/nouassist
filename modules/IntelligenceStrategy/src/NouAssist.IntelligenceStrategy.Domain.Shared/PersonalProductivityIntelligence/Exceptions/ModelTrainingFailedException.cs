using System;

namespace NouAssist.IntelligenceStrategy.PersonalProductivityIntelligence.Exceptions
{
    /// <summary>
    /// Exception thrown when model training fails
    /// </summary>
    public class ModelTrainingFailedException : PersonalProductivityIntelligenceDomainException
    {
        /// <summary>
        /// Creates a new ModelTrainingFailedException
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="modelType">Type of model that failed training</param>
        /// <param name="reason">Reason for the failure</param>
        public ModelTrainingFailedException(Guid userId, string modelType, string reason)
            : base("PersonalProductivityIntelligence:ErrorMessage:ModelTrainingFailed")
        {
            WithData("userId", userId);
            WithData("modelType", modelType);
            WithData("reason", reason);
        }
    }
}
