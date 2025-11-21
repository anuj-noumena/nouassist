using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Aggregates;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Entities;
using NouAssist.StartupEntrepreneurship.BusinessModelManagement.Enums;

namespace NouAssist.StartupEntrepreneurship.BusinessModelManagement.Services.Interfaces
{
    /// <summary>
    /// Domain service for hypothesis tracking and validation
    /// </summary>
    public interface IHypothesisTrackingService
    {
        Task<AssociatedHypothesis> CreateHypothesisAsync(
            BusinessModelCanvas canvas,
            CanvasComponentType componentType,
            string assumption,
            Guid createdBy,
            DateTime creationTime);

        Task<ValidationResult> ValidateHypothesisTestAsync(
            AssociatedHypothesis hypothesis,
            string testDescription,
            string testResult,
            ValidationStatus finalStatus);

        Task<List<AssociatedHypothesis>> GetPendingHypothesesAsync(BusinessModelCanvas canvas);

        Task<double> CalculateValidationProgressAsync(BusinessModelCanvas canvas);

        Task<List<string>> GenerateTestSuggestionsAsync(AssociatedHypothesis hypothesis);
    }
}
