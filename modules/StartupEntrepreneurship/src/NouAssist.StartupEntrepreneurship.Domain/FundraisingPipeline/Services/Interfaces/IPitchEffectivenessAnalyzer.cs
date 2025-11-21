using System;
using System.Threading.Tasks;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces
{
    /// <summary>
    /// Interface for service that analyzes pitch effectiveness
    /// </summary>
    public interface IPitchEffectivenessAnalyzer
    {
        Task<PitchEffectivenessResult> AnalyzePitchEffectivenessAsync(Guid pitchDeckId);
    }
}
