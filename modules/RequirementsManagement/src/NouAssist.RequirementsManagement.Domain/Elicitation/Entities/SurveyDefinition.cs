namespace NouAssist.RequirementsManagement.Elicitation.Entities
{
    /// <summary>
    /// Entity for survey definition and structure
    /// </summary>
    public class SurveyDefinition : Entity<Guid>
    {
        public Guid SessionId { get; private set; }
        public QuestionnaireStructure Questionnaire { get; private set; }
        public bool IsDeployed { get; private set; }
        public DateTime? DeploymentDate { get; private set; }

        private readonly List<SurveyResponse> _responses;
        public IReadOnlyCollection<SurveyResponse> Responses => _responses.AsReadOnly();

        protected SurveyDefinition()
        {
            _responses = new List<SurveyResponse>();
        }

        internal SurveyDefinition(Guid sessionId, QuestionnaireStructure questionnaire, DateTime? deploymentDate)
        {
            Id = Guid.NewGuid();
            SessionId = sessionId;
            Questionnaire = Check.NotNull(questionnaire, nameof(questionnaire));
            DeploymentDate = deploymentDate;
            IsDeployed = deploymentDate.HasValue;
            _responses = new List<SurveyResponse>();
        }

        public void Deploy(DateTime deploymentDate)
        {
            if (IsDeployed)
            {
                throw new SurveyAlreadyDeployedException(Id);
            }

            IsDeployed = true;
            DeploymentDate = deploymentDate;
        }

        public void AddResponse(Guid respondentId, ResponseData responseData)
        {
            if (!IsDeployed)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidOperation");
            }

            var existingResponse = _responses.FirstOrDefault(r => r.RespondentId == respondentId);
            if (existingResponse != null)
            {
                // Update existing response
                existingResponse.UpdateResponse(responseData);
            }
            else
            {
                // Add new response
                _responses.Add(new SurveyResponse(Id, respondentId, responseData));
            }
        }

        public int GetResponseCount()
        {
            return _responses.Count;
        }

        public double GetCompletionRate()
        {
            if (_responses.Count == 0)
                return 0.0;

            var totalQuestions = Questionnaire.TotalQuestions;
            var completedResponses = _responses.Count(r => r.IsComplete(totalQuestions));
            
            return (double)completedResponses / _responses.Count * 100;
        }
    }
}
