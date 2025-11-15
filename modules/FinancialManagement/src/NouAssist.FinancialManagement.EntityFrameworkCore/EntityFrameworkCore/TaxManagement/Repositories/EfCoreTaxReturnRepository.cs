using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NouAssist.TaxManagement.Aggregates;
using NouAssist.TaxManagement.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace NouAssist.TaxManagement.EntityFrameworkCore.Repositories
{
    public class EfCoreTaxReturnRepository 
        : EfCoreRepository<ITaxManagementDbContext, TaxReturn, Guid>, 
          ITaxReturnRepository
    {
        public EfCoreTaxReturnRepository(
            IDbContextProvider<ITaxManagementDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<TaxReturn?> GetWithDetailsAsync(Guid id)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.TaxReturns
                .Include(t => t.Deductions)
                .Include(t => t.Documents)
                .Include(t => t.EstimatedPayments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaxReturn?> GetByUserAndYearAsync(Guid userId, int taxYear)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.TaxReturns
                .Include(t => t.Deductions)
                .Include(t => t.Documents)
                .Include(t => t.EstimatedPayments)
                .FirstOrDefaultAsync(t => t.UserId == userId && t.TaxYear == taxYear);
        }

        public async Task<List<TaxReturn>> GetUserTaxReturnsAsync(Guid userId)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.TaxReturns
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.TaxYear)
                .ToListAsync();
        }

        public async Task<bool> HasReturnForYearAsync(Guid userId, int taxYear)
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.TaxReturns
                .AnyAsync(t => t.UserId == userId && t.TaxYear == taxYear);
        }
    }
}
