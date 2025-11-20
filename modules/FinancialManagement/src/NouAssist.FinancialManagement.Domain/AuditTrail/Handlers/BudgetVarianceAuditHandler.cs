using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NouAssist.FinancialManagement.AuditTrail.Services;
using NouAssist.FinancialManagement.Integration.Events;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Uow;

namespace NouAssist.FinancialManagement.AuditTrail.Handlers
{
    /// <summary>
    /// Handles BudgetVarianceDetectedEto event for audit trail
    /// Creates audit entry for budget control compliance
    /// </summary>
    public class BudgetVarianceAuditHandler :
        IDistributedEventHandler<BudgetVarianceDetectedEto>,
        ITransientDependency
    {
        private readonly FinancialAuditService _auditService;
        private readonly ILogger<BudgetVarianceAuditHandler> _logger;

        public BudgetVarianceAuditHandler(
            FinancialAuditService auditService,
            ILogger<BudgetVarianceAuditHandler> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        [UnitOfWork]
        public virtual async Task HandleEventAsync(BudgetVarianceDetectedEto eventData)
        {
            try
            {
                await _auditService.AuditBudgetVarianceAsync(
                    budgetId: eventData.BudgetId,
                    budgetName: eventData.BudgetName,
                    categoryId: eventData.CategoryId,
                    categoryName: eventData.CategoryName,
                    variance: eventData.VariancePercent,
                    isOverBudget: eventData.IsOverBudget,
                    tenantId: eventData.TenantId
                );

                _logger.LogDebug(
                    "Audit entry created for budget variance: {BudgetName}/{CategoryName} - {VariancePercent:F2}%",
                    eventData.BudgetName,
                    eventData.CategoryName,
                    eventData.VariancePercent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to create audit entry for budget variance: {BudgetId}",
                    eventData.BudgetId);
                // Don't throw - audit failure shouldn't break budget tracking
            }
        }
    }
}
