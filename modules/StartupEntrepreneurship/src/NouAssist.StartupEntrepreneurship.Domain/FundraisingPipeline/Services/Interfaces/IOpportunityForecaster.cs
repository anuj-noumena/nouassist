using System;
using System.Threading.Tasks;
using NouAssist.StartupEntrepreneurship.FundraisingPipeline.ValueObjects;

namespace NouAssist.StartupEntrepreneurship.FundraisingPipeline.Services.Interfaces
{
    /// <summary>
    /// Interface for service that forecasts investment opportunities
    /// </summary>
    public interface IOpportunityForecaster
    {
        Task<OpportunityForecastValue> GenerateForecastAsync(Guid prospectId, decimal baseAmount);
        Task UpdateForecastAsync(Guid prospectId, decimal baseAmount);
    }
}
