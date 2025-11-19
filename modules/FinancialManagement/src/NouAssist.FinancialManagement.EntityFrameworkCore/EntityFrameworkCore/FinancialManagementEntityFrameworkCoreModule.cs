using Microsoft.Extensions.DependencyInjection;
using NouAssist.FinancialManagement.AccountsPayable.Aggregates;
using NouAssist.FinancialManagement.AccountsReceivable.Aggregates;
using NouAssist.FinancialManagement.EntityFrameworkCore.AccountsPayable.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.AccountsReceivable.Repositories;
using NouAssist.FinancialManagement.EntityFrameworkCore.FinancialPlanning.Repositories;
using NouAssist.FinancialManagement.FinancialPlanning.Aggregates;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace NouAssist.FinancialManagement.EntityFrameworkCore;

[DependsOn(
    typeof(FinancialManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class FinancialManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FinancialManagementDbContext>(options =>
        {
            options.AddDefaultRepositories<IFinancialManagementDbContext>(includeAllEntities: true);

            // Accounts Payable repositories
            options.AddRepository<Bill, EfCoreBillRepository>();
            options.AddRepository<Vendor, EfCoreVendorRepository>();

            // Accounts Receivable repositories
            options.AddRepository<Invoice, EfCoreInvoiceRepository>();
            options.AddRepository<Customer, EfCoreCustomerRepository>();
            options.AddRepository<CustomerPayment, EfCoreCustomerPaymentRepository>();

            // Financial Planning repositories
            options.AddRepository<Budget, EfCoreBudgetRepository>();
            options.AddRepository<Expense, EfCoreExpenseRepository>();
            options.AddRepository<ExpenseReport, EfCoreExpenseReportRepository>();
            options.AddRepository<FinancialGoal, EfCoreFinancialGoalRepository>();
            options.AddRepository<LinkedBankAccount, EfCoreLinkedBankAccountRepository>();
        });
    }
}