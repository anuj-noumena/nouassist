namespace NouAssist.RequirementsManagement.Elicitation.Aggregates
{
    /// <summary>
    /// Aggregate for elicitation artifacts (User Story Draft, Use Case Draft, Idea, Event Definition)
    /// </summary>
    public class ElicitationArtifact : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; protected set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public ArtifactType Type { get; private set; }
        public ArtifactStatus Status { get; private set; }
        public Guid SessionId { get; private set; }
        public Guid CreatedByStakeholderId { get; private set; }

        // Type-specific content
        public StoryText StoryText { get; private set; } // For UserStory
        public UseCaseModel UseCaseModel { get; private set; } // For UseCase
        public EventDefinition EventDefinition { get; private set; } // For EventDefinition

        private readonly List<IdeaTag> _ideaTags;
        public IReadOnlyCollection<IdeaTag> IdeaTags => _ideaTags.AsReadOnly();

        protected ElicitationArtifact()
        {
            _ideaTags = new List<IdeaTag>();
        }

        internal ElicitationArtifact(
            Guid id,
            string title,
            string description,
            ArtifactType type,
            Guid sessionId,
            Guid createdByStakeholderId,
            Guid? tenantId) : base(id)
        {
            SetTitle(title);
            SetDescription(description);
            Type = type;
            SessionId = sessionId;
            CreatedByStakeholderId = createdByStakeholderId;
            TenantId = tenantId;
            Status = ArtifactStatus.Draft;
            _ideaTags = new List<IdeaTag>();

            AddDistributedEvent(new ElicitationArtifactCreatedEto
            {
                Id = Id,
                Title = Title,
                Type = Type,
                SessionId = SessionId,
                CreatedByStakeholderId = CreatedByStakeholderId,
                TenantId = TenantId
            });
        }

        public static ElicitationArtifact Create(
            Guid id,
            string title,
            string description,
            ArtifactType type,
            Guid sessionId,
            Guid createdByStakeholderId,
            Guid? tenantId)
        {
            return new ElicitationArtifact(id, title, description, type, sessionId, createdByStakeholderId, tenantId);
        }

        public void SetUserStoryContent(StoryText storyText)
        {
            if (Type != ArtifactType.UserStory)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            StoryText = Check.NotNull(storyText, nameof(storyText));
        }

        public void SetUseCaseContent(UseCaseModel useCaseModel)
        {
            if (Type != ArtifactType.UseCase)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            UseCaseModel = Check.NotNull(useCaseModel, nameof(useCaseModel));
        }

        public void SetEventDefinitionContent(EventDefinition eventDefinition)
        {
            if (Type != ArtifactType.EventDefinition)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            EventDefinition = Check.NotNull(eventDefinition, nameof(eventDefinition));
        }

        public void AddTag(IdeaTag tag)
        {
            if (Type != ArtifactType.Idea)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidArtifactType");
            }

            if (!_ideaTags.Any(t => t.Name.Equals(tag.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _ideaTags.Add(tag);
            }
        }

        public void RemoveTag(string tagName)
        {
            var tag = _ideaTags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
            if (tag != null)
            {
                _ideaTags.Remove(tag);
            }
        }

        public void SubmitForReview()
        {
            if (Status != ArtifactStatus.Draft)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidStatusTransition");
            }

            Status = ArtifactStatus.UnderReview;
        }

        public void Approve()
        {
            if (Status != ArtifactStatus.UnderReview)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidStatusTransition");
            }

            Status = ArtifactStatus.Approved;
        }

        public void Reject()
        {
            if (Status != ArtifactStatus.UnderReview)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidStatusTransition");
            }

            Status = ArtifactStatus.Rejected;
        }

        public void MarkAsImplemented()
        {
            if (Status != ArtifactStatus.Approved)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidStatusTransition");
            }

            Status = ArtifactStatus.Implemented;
        }

        public void MarkAsObsolete()
        {
            Status = ArtifactStatus.Obsolete;
        }

        public void ReturnToDraft()
        {
            if (Status != ArtifactStatus.Rejected)
            {
                throw new BusinessException("Elicitation:ErrorMessage:InvalidStatusTransition");
            }

            Status = ArtifactStatus.Draft;
        }

        private void SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(
                title,
                nameof(title),
                maxLength: ElicitationConsts.MaxArtifactTitleLength
            );
        }

        private void SetDescription(string description)
        {
            Description = description?.Length <= ElicitationConsts.MaxArtifactDescriptionLength
                ? description
                : throw new BusinessException("Elicitation:Validation:StringLength")
                    .WithData("propertyName", "Description")
                    .WithData("min", 0)
                    .WithData("max", ElicitationConsts.MaxArtifactDescriptionLength);
        }
    }
}
