using System;
using System.Collections.Generic;
using NouAssist.DecisionSupport.Dtos.Score;
using Volo.Abp.Application.Dtos;

namespace NouAssist.DecisionSupport.Dtos.Alternative
{
    /// <summary>
    /// DTO for alternative data
    /// </summary>
    public class AlternativeDto : EntityDto<Guid>
    {
        /// <summary>
        /// The decision model this alternative belongs to
        /// </summary>
        public Guid DecisionModelId { get; set; }
        
        /// <summary>
        /// The name of the alternative
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The description of the alternative
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Scores for this alternative against various criteria
        /// </summary>
        public List<AlternativeScoreDto> Scores { get; set; }
        
        public AlternativeDto()
        {
            Scores = new List<AlternativeScoreDto>();
        }
    }
}
