using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NouAssist.CurrencyManagement.Aggregates;
using NouAssist.CurrencyManagement.ValueObjects;
using Volo.Abp.Domain.Repositories;

namespace NouAssist.CurrencyManagement.Data
{
    public interface ICurrencyRepository : IRepository<Currency, Guid>
    {
        Task<Currency> GetByCodeAsync(CurrencyCode code);
        
        Task<List<Currency>> GetActiveCurrenciesAsync();
        
        Task<Currency> GetBaseCurrencyAsync();
        
        Task<bool> CurrencyExistsAsync(CurrencyCode code);
    }
}
