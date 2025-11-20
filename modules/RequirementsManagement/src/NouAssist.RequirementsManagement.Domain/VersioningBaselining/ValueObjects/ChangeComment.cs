namespace NouAssist.RequirementsManagement.VersioningBaselining.ValueObjects
{
    /// <summary>
    /// Value object representing a change comment
    /// </summary>
    public class ChangeComment : ValueObject
    {
        /// <summary>
        /// The comment text
        /// </summary>
        public string Text { get; private set; }

        private ChangeComment() { }

        /// <summary>
        /// Creates a new ChangeComment with the specified text
        /// </summary>
        public ChangeComment(string text)
        {
            SetText(text);
        }

        private void SetText(string text)
        {
            Text = Check.NotNullOrWhiteSpace(
                text, 
                nameof(text), 
                maxLength: VersionControlConsts.MaxChangeCommentLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Text;
        }
        
        public override string ToString()
        {
            return Text;
        }
    }
}
