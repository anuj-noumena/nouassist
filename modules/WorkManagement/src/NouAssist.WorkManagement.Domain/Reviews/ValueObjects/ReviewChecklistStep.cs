using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.WorkManagement.Reviews.ValueObjects
{
    public class ReviewChecklistStep : ValueObject
    {
        public int Step { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        
        private readonly List<string> _items;
        public IReadOnlyList<string> Items => _items.AsReadOnly();

        public ReviewChecklistStep(int step, string title, List<string> items, string description = null)
        {
            Step = step;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), maxLength: 200);
            Description = description;
            _items = items ?? new List<string>();

            if (_items.Count > 50)
            {
                throw new BusinessException("Reviews:TooManyChecklistItems", 
                    "Maximum 50 items per checklist step");
            }
        }

        public int ItemCount => _items.Count;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Step;
            yield return Title;
            yield return Description ?? string.Empty;
            foreach (var item in _items)
            {
                yield return item;
            }
        }
    }
}
