namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Aggregates
{
    /// <summary>
    /// Aggregate root representing a user feedback session with structured feedback data
    /// </summary>
    public class UserFeedbackSession : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public Guid? RelatedPrototypeId { get; private set; }
        public Guid? PrototypeVersionId { get; private set; }
        public string TestScenario { get; private set; }
        public DateTime ScheduledDate { get; private set; }
        public DateTime? CompletedDate { get; private set; }
        public string ParticipantEmail { get; private set; }
        public string ParticipantProfile { get; private set; }
        public InterviewStatus Status { get; private set; }

        // Entities within the aggregate
        private readonly List<FeedbackData> _feedbackData;

        // Public read-only access
        public IReadOnlyCollection<FeedbackData> FeedbackData => _feedbackData.AsReadOnly();

        protected UserFeedbackSession()
        {
            _feedbackData = new List<FeedbackData>();
        }

        public static UserFeedbackSession Create(
            Guid id,
            string title,
            string description,
            DateTime scheduledDate,
            string participantEmail,
            string participantProfile,
            string testScenario,
            Guid? relatedPrototypeId = null,
            Guid? prototypeVersionId = null,
            Guid? tenantId = null)
        {
            return new UserFeedbackSession(
                id, title, description, scheduledDate,
                participantEmail, participantProfile, testScenario,
                relatedPrototypeId, prototypeVersionId, tenantId);
        }

        private UserFeedbackSession(
            Guid id,
            string title,
            string description,
            DateTime scheduledDate,
            string participantEmail,
            string participantProfile,
            string testScenario,
            Guid? relatedPrototypeId,
            Guid? prototypeVersionId,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            SetScheduledDate(scheduledDate);
            SetParticipantEmail(participantEmail);
            SetParticipantProfile(participantProfile);
            SetTestScenario(testScenario);
            RelatedPrototypeId = relatedPrototypeId;
            PrototypeVersionId = prototypeVersionId;
            TenantId = tenantId;
            Status = InterviewStatus.Scheduled;
            _feedbackData = new List<FeedbackData>();
        }

        public void UpdateDetails(string title, string description, string testScenario)
        {
            if (Status != InterviewStatus.Scheduled)
            {
                throw new InvalidInterviewStatusTransitionException(Status, Status);
            }

            SetTitle(title);
            SetDescription(description);
            SetTestScenario(testScenario);
        }

        public void StartSession(DateTime startTime)
        {
            if (Status != InterviewStatus.Scheduled && Status != InterviewStatus.Rescheduled)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.InProgress);
            }

            Status = InterviewStatus.InProgress;
        }

        public void CompleteSession(DateTime completedTime)
        {
            if (Status != InterviewStatus.InProgress)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.Completed);
            }

            Status = InterviewStatus.Completed;
            CompletedDate = completedTime;
        }

        public void CancelSession()
        {
            if (Status == InterviewStatus.Completed)
            {
                throw new InvalidInterviewStatusTransitionException(Status, InterviewStatus.Cancelled);
            }

            Status = InterviewStatus.Cancelled;
        }

        public void AddFeedback(
            string category,
            string question,
            string response,
            FeedbackRating rating,
            DateTime recordedAt)
        {
            if (Status != InterviewStatus.InProgress && Status != InterviewStatus.Completed)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:InvalidOperation",
                    "Feedback can only be added during or after the session");
            }

            var feedback = new FeedbackData(
                GuidGenerator.Create(),
                Id,
                category,
                question,
                response,
                rating,
                recordedAt);

            _feedbackData.Add(feedback);
        }

        public void UpdateFeedback(Guid feedbackId, string response, FeedbackRating rating)
        {
            var feedback = GetFeedback(feedbackId);
            feedback.UpdateResponse(response);
            feedback.UpdateRating(rating);
        }

        public void AddTagToFeedback(Guid feedbackId, FeedbackTag tag)
        {
            var feedback = GetFeedback(feedbackId);
            feedback.AddTag(tag);
        }

        public void RemoveTagFromFeedback(Guid feedbackId, string tagName)
        {
            var feedback = GetFeedback(feedbackId);
            feedback.RemoveTag(tagName);
        }

        public decimal GetAverageRating(string category = null)
        {
            var relevantFeedback = string.IsNullOrEmpty(category)
                ? _feedbackData
                : _feedbackData.Where(f => f.Category == category);

            if (!relevantFeedback.Any())
                return 0;

            return relevantFeedback.Average(f => f.Rating.PercentageScore());
        }

        public Dictionary<string, int> GetFeedbackCountByCategory()
        {
            return _feedbackData
                .GroupBy(f => f.Category)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public List<FeedbackData> GetFeedbackByCategory(string category)
        {
            return _feedbackData.Where(f => f.Category == category).ToList();
        }

        private FeedbackData GetFeedback(Guid feedbackId)
        {
            var feedback = _feedbackData.FirstOrDefault(f => f.Id == feedbackId);
            if (feedback == null)
            {
                throw new BusinessException("ProductDiscovery:ErrorMessage:EntityNotFound",
                    "Feedback data not found");
            }
            return feedback;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ProductDiscoveryConsts.MaxInterviewTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(
                description,
                nameof(description),
                maxLength: ProductDiscoveryConsts.MaxInterviewDescriptionLength
            );
        }

        private void SetScheduledDate(DateTime scheduledDate)
        {
            if (scheduledDate <= DateTime.UtcNow.AddMinutes(-30))
            {
                throw new BusinessException("ProductDiscovery:Validation:FutureDate",
                    "Scheduled date must be in the future");
            }

            ScheduledDate = scheduledDate;
        }

        private void SetParticipantEmail(string participantEmail)
        {
            ParticipantEmail = Check.NotNullOrWhiteSpace(
                participantEmail,
                nameof(participantEmail),
                maxLength: 255
            );
        }

        private void SetParticipantProfile(string participantProfile)
        {
            ParticipantProfile = Check.NotNullOrWhiteSpace(
                participantProfile,
                nameof(participantProfile),
                maxLength: 1000
            );
        }

        private void SetTestScenario(string testScenario)
        {
            TestScenario = Check.NotNullOrWhiteSpace(
                testScenario,
                nameof(testScenario),
                maxLength: ProductDiscoveryConsts.MaxValidationNotesLength
            );
        }
    }
}
