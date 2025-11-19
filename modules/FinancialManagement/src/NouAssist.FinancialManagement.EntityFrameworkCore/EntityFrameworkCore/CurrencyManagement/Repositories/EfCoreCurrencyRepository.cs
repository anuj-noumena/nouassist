using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.FinancialManagement.CurrencyManagement.Aggregates;
using NouAssist.FinancialManagement.CurrencyManagement.Data;
using NouAssist.FinancialManagement.CurrencyManagement.Enums;
using NouAssist.FinancialManagement.CurrencyManagement.ValueObjects;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.CurrencyManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreCurrencyRepository
        : EfCoreRepository<ICurrencyManagementDbContext, Currency, Guid>, ICurrencyRepository
    {
        public EfCoreCurrencyRepository(
            IDbContextProvider<ICurrencyManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<Currency> GetByCodeAsync(
            CurrencyCode code,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();
            
            return await query.FirstOrDefaultAsync(
                c => c.Code.Code == code.Code,
                cancellationToken);
        }

        public async Task<List<Currency>> GetActiveCurrenciesAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query
                .Where(c => c.Status == CurrencyStatus.Active)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Currency> GetBaseCurrencyAsync(
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.FirstOrDefaultAsync(
                c => c.IsBaseCurrency,
                cancellationToken);
        }

        public async Task<bool> CurrencyExistsAsync(
            CurrencyCode code,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryableAsync();

            return await query.AnyAsync(
                c => c.Code.Code == code.Code,
                cancellationToken);
        }
    }
}
