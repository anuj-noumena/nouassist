namespace NouAssist.RequirementsManagement.Elicitation.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to modify a survey that has already been deployed
    /// </summary>
    public class SurveyAlreadyDeployedException : ElicitationDomainException
    {
        public SurveyAlreadyDeployedException(Guid surveyId)
            : base("Elicitation:ErrorMessage:SurveyAlreadyDeployed")
        {
            WithData("surveyId", surveyId);
        }
    }
}
