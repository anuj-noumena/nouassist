namespace NouAssist.RequirementsManagement.Elicitation.Entities
{
    /// <summary>
    /// Entity for ideas captured during brainstorming sessions
    /// </summary>
    public class BrainstormingIdea : Entity<Guid>
    {
        public Guid SessionId { get; private set; }
        public Guid ContributorId { get; private set; }
        public string Description { get; private set; }
        public int Priority { get; private set; }
        public bool IsSelected { get; private set; }

        private readonly List<IdeaTag> _tags;
        public IReadOnlyCollection<IdeaTag> Tags => _tags.AsReadOnly();

        protected BrainstormingIdea()
        {
            _tags = new List<IdeaTag>();
        }

        internal BrainstormingIdea(Guid sessionId, Guid contributorId, string description, int priority = 1)
        {
            Id = Guid.NewGuid();
            SessionId = sessionId;
            ContributorId = contributorId;
            SetDescription(description);
            Priority = priority;
            IsSelected = false;
            _tags = new List<IdeaTag>();
        }

        public void UpdateDescription(string description)
        {
            SetDescription(description);
        }

        public void SetPriority(int priority)
        {
            Priority = Math.Max(1, Math.Min(priority, 10)); // Clamp between 1-10
        }

        public void Select()
        {
            IsSelected = true;
        }

        public void Deselect()
        {
            IsSelected = false;
        }

        public void AddTag(IdeaTag tag)
        {
            if (!_tags.Any(t => t.Name.Equals(tag.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _tags.Add(tag);
            }
        }

        public void RemoveTag(string tagName)
        {
            var tag = _tags.FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));
            if (tag != null)
            {
                _tags.Remove(tag);
            }
        }

        private void SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description));
        }
    }
}
