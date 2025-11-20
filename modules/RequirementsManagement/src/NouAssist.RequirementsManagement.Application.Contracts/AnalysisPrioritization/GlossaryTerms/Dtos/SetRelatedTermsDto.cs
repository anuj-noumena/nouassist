using System;
using System.Collections.Generic;

namespace NouAssist.RequirementsManagement.AnalysisPrioritization.GlossaryTerms.Dtos
{
    /// <summary>
    /// DTO for setting related terms
    /// </summary>
    public class SetRelatedTermsDto
    {
        public List<Guid> RelatedTermIds { get; set; } = new();
    }
}
