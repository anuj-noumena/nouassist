namespace NouAssist.RequirementsManagement.Elicitation.Aggregates
{
    /// <summary>
    /// Aggregate for managing elicitation sessions (Survey, Interview, Workshop)
    /// </summary>
    public class ElicitationSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public SessionType Type { get; private set; }
        public SessionStatus Status { get; private set; }
        public SessionSchedule Schedule { get; private set; }
        public Guid OrganizerId { get; private set; }

        private readonly List<ElicitationSessionParticipant> _participants;
        public IReadOnlyCollection<ElicitationSessionParticipant> Participants => _participants.AsReadOnly();

        private readonly List<ElicitationArtifact> _generatedArtifacts;
        public IReadOnlyCollection<ElicitationArtifact> GeneratedArtifacts => _generatedArtifacts.AsReadOnly();

        // Type-specific entities
        public SurveyDefinition SurveyDefinition { get; private set; }
        public InterviewRecord InterviewRecord { get; private set; }

        protected ElicitationSession()
        {
            _participants = new List<ElicitationSessionParticipant>();
            _generatedArtifacts = new List<ElicitationArtifact>();
        }

        internal ElicitationSession(
            Guid id,
            string title,
            string description,
            SessionType type,
            SessionSchedule schedule,
            Guid organizerId,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            Type = type;
            Schedule = Check.NotNull(schedule, nameof(schedule));
            OrganizerId = organizerId;
            TenantId = tenantId;
            Status = SessionStatus.Planned;
            
            _participants = new List<ElicitationSessionParticipant>();
            _generatedArtifacts = new List<ElicitationArtifact>();
        }

        public static ElicitationSession Create(
            Guid id,
            string title,
            string description,
            SessionType type,
            SessionSchedule schedule,
            Guid organizerId,
            Guid? tenantId)
        {
            return new ElicitationSession(id, title, description, type, schedule, organizerId, tenantId);
        }

        public void AddParticipant(Guid stakeholderId, bool isRequired = false)
        {
            if (Status != SessionStatus.Planned && Status != SessionStatus.Scheduled)
            {
                throw new InvalidSessionStatusTransitionException(Status, Status);
            }

            if (_participants.Any(p => p.StakeholderId == stakeholderId))
            {
                return; // Already a participant
            }

            _participants.Add(new ElicitationSessionParticipant(Id, stakeholderId, isRequired));
        }

        public void RemoveParticipant(Guid stakeholderId)
        {
            if (Status != SessionStatus.Planned && Status != SessionStatus.Scheduled)
            {
                throw new InvalidSessionStatusTransitionException(Status, Status);
            }

            var participant = _participants.FirstOrDefault(p => p.StakeholderId == stakeholderId);
            if (participant != null)
            {
                _participants.Remove(participant);
            }
        }

        public void Schedule()
        {
            if (Status != SessionStatus.Planned)
            {
                throw new InvalidSessionStatusTransitionException(Status, SessionStatus.Scheduled);
            }

            Status = SessionStatus.Scheduled;
        }

        public void Start()
        {
            if (Status != SessionStatus.Scheduled)
            {
                throw new InvalidSessionStatusTransitionException(Status, SessionStatus.InProgress);
            }

            Status = SessionStatus.InProgress;
        }

        public void Complete()
        {
            if (Status != SessionStatus.InProgress)
            {
                throw new InvalidSessionStatusTransitionException(Status, SessionStatus.Completed);
            }

            Status = SessionStatus.Completed;

            AddDistributedEvent(new ElicitationSessionCompletedEto
            {
                Id = Id,
                Title = Title,
                Type = Type,
                CompletedAt = DateTime.UtcNow,
                ParticipantIds = _participants.Select(p => p.StakeholderId).ToList(),
                ArtifactsGenerated = _generatedArtifacts.Count,
                TenantId = TenantId
            });
        }

        public void Cancel()
        {
            if (Status == SessionStatus.Completed)
            {
                throw new InvalidSessionStatusTransitionException(Status, SessionStatus.Cancelled);
            }

            Status = SessionStatus.Cancelled;
        }

        public void Postpone()
        {
            if (Status != SessionStatus.Scheduled)
            {
                throw new InvalidSessionStatusTransitionException(Status, SessionStatus.Postponed);
            }

            Status = SessionStatus.Postponed;
        }

        public void SetSurveyDefinition(QuestionnaireStructure questionnaire, DateTime? deploymentDate = null)
        {
            if (Type != SessionType.Survey)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            SurveyDefinition = new SurveyDefinition(Id, questionnaire, deploymentDate);
        }

        public void AddSurveyResponse(Guid respondentId, ResponseData responseData)
        {
            if (Type != SessionType.Survey || SurveyDefinition == null)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            SurveyDefinition.AddResponse(respondentId, responseData);

            AddDistributedEvent(new SurveyResponseSubmittedEto
            {
                Id = Guid.NewGuid(),
                SurveyId = Id,
                RespondentId = respondentId,
                SubmittedAt = responseData.SubmittedAt,
                QuestionsAnswered = responseData.Answers.Count,
                TenantId = TenantId
            });
        }

        public void SetInterviewRecord(TranscriptRecordingRef transcriptRecording)
        {
            if (Type != SessionType.Interview)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            InterviewRecord = new InterviewRecord(Id, transcriptRecording);
        }

        public void GenerateArtifact(ElicitationArtifact artifact)
        {
            if (Status != SessionStatus.InProgress && Status != SessionStatus.Completed)
            {
                throw new BusinessException("Elicitation:ErrorMessage:SessionNotCompleted");
            }

            _generatedArtifacts.Add(artifact);
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ElicitationConsts.MaxSessionTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = description?.Length <= ElicitationConsts.MaxSessionDescriptionLength
                ? description
                : throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Description")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxSessionDescriptionLength);
        }
    }
}
