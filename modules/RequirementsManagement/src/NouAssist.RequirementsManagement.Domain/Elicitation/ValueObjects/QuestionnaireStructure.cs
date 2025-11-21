using System.Collections.Generic;
using System.Linq;
using NouAssist.RequirementsManagement.Elicitation.Constants;
using NouAssist.RequirementsManagement.Elicitation.Enums;
using Volo.Abp;
using Volo.Abp.Domain.Values;

namespace NouAssist.RequirementsManagement.Elicitation.ValueObjects
{
    /// <summary>
    /// Value object for survey questionnaire structure
    /// </summary>
    public class QuestionnaireStructure : ValueObject
    {
        public List<SurveyPage> Pages { get; private set; }
        public Dictionary<string, string> Logic { get; private set; }

        private QuestionnaireStructure() { }

        public QuestionnaireStructure(List<SurveyPage> pages, Dictionary<string, string> logic = null)
        {
            Pages = pages ?? new List<SurveyPage>();
            Logic = logic ?? new Dictionary<string, string>();

            if (Pages.Count == 0)
            {
                throw new BusinessException("Elicitation:Validation:Required")
                    .WithData("propertyName", "Pages");
            }
        }

        public int TotalQuestions => Pages.Sum(p => p.Questions.Count);

        protected override IEnumerable<object> GetAtomicValues()
        {
            foreach (var page in Pages)
            {
                yield return page;
            }
            foreach (var logicItem in Logic)
            {
                yield return logicItem.Key;
                yield return logicItem.Value;
            }
        }
    }

    public class SurveyPage : ValueObject
    {
        public string Title { get; private set; }
        public List<SurveyQuestion> Questions { get; private set; }

        private SurveyPage() { }

        public SurveyPage(string title, List<SurveyQuestion> questions)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            Questions = questions ?? new List<SurveyQuestion>();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Title;
            foreach (var question in Questions)
            {
                yield return question;
            }
        }
    }

    public class SurveyQuestion : ValueObject
    {
        public string Id { get; private set; }
        public string Text { get; private set; }
        public QuestionType Type { get; private set; }
        public bool IsRequired { get; private set; }
        public List<string> Options { get; private set; }

        private SurveyQuestion() { }

        public SurveyQuestion(string id, string text, QuestionType type, bool isRequired = false, List<string> options = null)
        {
            Id = Check.NotNullOrWhiteSpace(id, nameof(id));
            SetText(text);
            Type = type;
            IsRequired = isRequired;
            Options = options ?? new List<string>();
        }

        private void SetText(string text)
        {
            Text = Check.NotNullOrWhiteSpace(
                text,
                nameof(text),
                maxLength: ElicitationConsts.MaxQuestionTextLength
            );
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Text;
            yield return Type;
            yield return IsRequired;
            foreach (var option in Options)
            {
                yield return option;
            }
        }
    }
}
