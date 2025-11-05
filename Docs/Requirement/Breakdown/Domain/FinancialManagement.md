### 1. General Ledger (GL) BC

* **Focus:** Maintaining the authoritative record of financial transactions using double-entry principles, managing the CoA, enabling period closing, standard financial statement generation, **and performing bank reconciliation.**
* **Aggregates:**
    * `LedgerAccount` (AR): Represents an account in the CoA, tracking its balance.
    * `JournalEntryBatch` (AR): A collection of balanced debit/credit lines representing a single transaction event.
    * `AccountingPeriod` (AR): Manages the status (Open/Closed) of financial periods.
    * **`BankStatement` (AR): Represents an imported bank statement for reconciliation.** *(Alternative: Could be entities if statement lifecycle is simple)*
* **Entities:**
    * `LedgerAccount` (AR),
    * `ChartOfAccounts` (*Singleton entity managing CoA structure, rules, hierarchy; distinct from individual LedgerAccount ARs*),
    * `JournalEntryBatch` (AR), `JournalEntryLine`,
    * `AccountingPeriod` (AR), `ClosingProcedureLog`,
    * **`BankStatement` (AR), `BankStatementLine`, `ReconciliationMatch`**
* **Value Objects:** `AccountNumber`, `AccountName`, `AccountType`, `MonetaryValue` (SK), `DebitCreditIndicator`, `TransactionDate`, `Memo`, `PeriodStatus`, `AutoCreationRule` (SK), **`StatementLineDetails`, `MatchConfidenceScore`, `ReconciliationStatus`**
* **Services:** `JournalPostingService`, `PeriodClosingService`, `FinancialStatementGenerator`, **`BankReconciliationService`**
* **User Stories:**
    * As an Accountant, I want to create and manage ledger accounts in the Chart of Accounts so that I can accurately categorize financial transactions.
    * As an Accountant, I want to post journal entry batches so that all financial transactions are recorded using double-entry principles.
    * As an Accountant, I want to manage accounting periods (open/close) so that I can ensure financial data integrity for specific periods.
    * As an Accountant, I want to import bank statements so that I can reconcile them with my internal records.
    * As an Accountant, I want to automatically and manually match bank statement lines to journal entries so that I can efficiently reconcile accounts.
    * As an Accountant, I want to generate standard financial statements (Balance Sheet, Income Statement, Trial Balance, Cash Flow Statement, Net Worth Report) so that I can understand the financial health of the organization.
    * As a System Administrator, I want to define rules for auto-creating ledger accounts for new payees so that the Chart of Accounts can be expanded consistently.
* **Commands:** `CreateLedgerAccount`, `UpdateLedgerAccount`, `PostJournalEntryBatch`, `InitiatePeriodClose`, `FinalizePeriodClose`, **`ImportBankStatement` (triggered by event from FP&A), `MatchStatementLine` (Manual/Suggested), `AutoReconcileStatement`, `MarkAsReconciled`**
* **Queries:** `GetAccountBalance`, `GetJournalEntries`, `GetAccountingPeriodStatus`, `GenerateBalanceSheet`, ..., `GenerateNetWorthReport`, **`GetUnreconciledStatementLines`, `GetReconciliationStatus`, `GetBankStatementDetails`**
* **Events:** `JournalBatchPosted`, `AccountBalanceUpdated`, `PeriodClosingInitiated`, `PeriodClosed`, `LedgerAccountCreated`, **`BankStatementImported`, `StatementLineMatched`, `StatementReconciled`**

### 2. Accounts Payable (AP) BC

* **Focus:** Managing incoming bills, scheduling/recording payments owed, **and initiating payments.**
* **Aggregates:** `Bill` (AR)
* **Entities:** `Bill` (AR), `BillPayment` (*records payment execution*)
* **Value Objects:** `BillReference`, `IssueDate`, `DueDate`, `BillStatus`, `MonetaryValue` (SK), `PayeeRef` (SK), `CategoryRef` (SK), `ProjectRef` (SK), `RecurringScheduleInfo`, `OcrExtractedData`, **`PaymentInitiationDetails`** (Method, RunID, GatewayRef), `PaymentConfirmation`, **`DocumentID`** (*Held by Bill for associated document; obtained from/links to Document Management BC*)
* **Services:** `BillOcrProcessor`, **`PaymentInitiationService`** (*interacts with external payment systems/banks*)
* **User Stories:**
    * As an Accounts Payable Clerk, I want to record new bills, including scanning physical bills using OCR, so that all liabilities are captured accurately.
    * As an Accounts Payable Clerk, I want to categorize bills by vendor (Payee), category, and project so that expenses can be tracked and analyzed effectively.
    * As an Accounts Payable Clerk, I want to schedule recurring bills so that regular payments are not missed.
    * As an Accounts Payable Clerk, I want to initiate payment runs for due bills so that vendors are paid on time.
    * As an Accounts Payable Clerk, I want to generate payment files in the required format for bank processing so that payments can be made efficiently.
    * As an Accounts Payable Clerk, I want to submit payments directly through integrated gateways so that the payment process is streamlined.
    * As an Accounts Payable Clerk, I want to record bill payments and mark bills as paid so that the status of liabilities is always up-to-date.
    * As an Accounts Payable Clerk, I want to view a list of upcoming payments so that I can manage cash flow effectively.
    * As an Accounts Payable Clerk, I want to see the status of payment runs so that I can track the progress of payments.
* **Commands:** `RecordBill`, `CategorizeBill`, `ScheduleRecurringBill`, **`InitiateBillPaymentRun`**, **`GeneratePaymentFile`**, **`SubmitPaymentDirectly`**, `RecordBillPayment` (*May be triggered by PaymentInitiationService upon confirmation*), `MarkBillAsPaid` (*May follow successful payment*)
* **Queries:** `GetBillDetails`, `ListBillsByStatus`, `GetUpcomingPayments`, **`GetPaymentRunStatus`**
* **Events:** `BillRecorded`, `BillCategorized`, `BillScheduled`, **`BillPaymentInitiated`**, **`PaymentFileGenerated`**, **`PaymentSubmittedToGateway`**, **`PaymentProcessingFailed`**, `BillPaymentRecorded`, `BillPaid`, `BillOverdue`

### 3. Accounts Receivable (AR) BC

* **Focus:** Managing clients, creating/sending invoices, tracking payments received, **and processing online/gateway payments.**
* **Aggregates:** `Invoice` (AR), `Client` (AR)
* **Entities:** `Invoice` (AR), `InvoiceLineItem`, `InvoicePayment`, `Client` (AR), `ClientContact`
* **Value Objects:** `InvoiceNumber`, `IssueDate`, `DueDate`, `InvoiceStatus`, `MonetaryValue` (SK), `ClientRef`, `LineItemDescription`, ..., `ProjectTaskLink` (SK), `EmailDeliveryStatus`, `ReminderConfiguration`, **`PaymentGatewayReference`**
* **Services:** `InvoiceGenerator`, `InvoiceEmailSender`, `PaymentReminderService`, **`PaymentGatewayIntegrationService`**
* **User Stories:**
    * As a Salesperson/Account Manager, I want to create and manage client information so that I have a central record for all customer details.
    * As a Billing Specialist, I want to create professional invoices using customizable templates so that I can bill clients accurately and consistently.
    * As a Billing Specialist, I want to manage invoice line items, including tax and discount calculations, so that invoices are detailed and correct.
    * As a Billing Specialist, I want to send invoices to clients via email and track their delivery status so that I can ensure clients receive their bills promptly.
    * As a Billing Specialist, I want to charge a client's credit card for an invoice amount so that payments can be collected quickly.
    * As a Billing Specialist, I want to process online payments received through a payment gateway so that revenue is recognized efficiently.
    * As a Billing Specialist, I want to set up direct debit for clients so that recurring payments are automated.
    * As a Billing Specialist, I want to record invoice payments (full or partial) so that the accounts receivable are accurate.
    * As a Billing Specialist, I want to send payment reminders for overdue invoices so that I can encourage timely payment.
    * As an Accountant, I want to get an accounts receivable aging report so that I can monitor outstanding invoices and manage collections.
* **Commands:** `CreateClient`, `UpdateClient`, `CreateInvoice`, ..., `FinalizeInvoice`, `SendInvoice`, **`ChargeClientCard`**, **`ProcessOnlinePayment`**, **`SetupDirectDebit`**, `RecordInvoicePayment` (*May be triggered by events like OnlinePaymentReceived or manual entry*), `SendPaymentReminder`, ...
* **Queries:** `GetInvoiceDetails`, `ListInvoicesByStatus`, `GetClientDetails`, `GetAccountsReceivableAging`
* **Events:** `ClientCreated`, `InvoiceCreated`, ..., `InvoiceSent`, `InvoiceViewed`, **`PaymentGatewayInteractionStarted`**, **`OnlinePaymentReceived`**, **`CardChargeSucceeded`**, **`CardChargeFailed`**, `InvoicePaymentRecorded`, `InvoicePaid`, `InvoiceOverdue`, `PaymentReminderSent`

### 4. Financial Planning & Analysis (FP&A) BC

* **Focus:** Managing expenses, budgets, financial goals, and **fetching bank data for reconciliation.**
* **Aggregates:**
    * `Expense` (AR): A single logged expense.
    * **`ExpenseReport` (AR): Collection of expenses for submission/approval.**
    * `Budget` (AR): Defines budget allocations and tracks usage, includes versions.
    * `FinancialGoal` (AR): Defines a financial target and tracks progress.
* **Entities:**
    * `Expense` (AR), `MileageRecord`
    * **`ExpenseReport` (AR), `ExpenseReportLine`** (*links to Expense AR*)
    * `Budget` (AR), `BudgetVersion`, `CategoryAllocation`, `BudgetForecastRecord`
    * `FinancialGoal` (AR), `GoalContributionLog`
    * `LinkedBankAccount`
* **Value Objects:** `ExpenseDate`, `MonetaryValue` (SK), `CategoryRef` (SK), `PayeeRef` (SK), **`ReceiptDocumentID`** (*Held by Expense; obtained from/links to Document Management BC*), `MileageData`, **`ExpenseReportStatus`** (Draft, Submitted, Approved, Rejected, Reimbursed), `ReportPeriod`, `BudgetName`, ..., `GoalDescription`, ..., `BankBalanceInfo`, `LowBalanceAlertConfig`, **`RawBankStatementData`**
* **Services:** `ExpenseReportGenerator`, `BudgetForecaster`, **`BankDataFetcher`** (*retrieves data, potentially emits event*)
* **User Stories:**
    * As an Employee, I want to log my expenses, including capturing receipts, so that I can get reimbursed.
    * As an Employee, I want to track my mileage for business travel so that I can claim reimbursement accurately.
    * As an Employee, I want to create and submit expense reports by adding logged expenses so that I can formally request reimbursement.
    * As a Manager, I want to review, approve, or reject submitted expense reports so that I can ensure compliance with company policy.
    * As an Accountant/Finance Manager, I want to mark expense reports as reimbursed so that the payment status is tracked.
    * As a Financial Planner, I want to create budgets with category-specific allocations (supporting zero-based budgeting) so that I can plan and control spending.
    * As a Financial Planner, I want to track actual spending against budget allocations so that I can monitor financial performance.
    * As a Financial Planner, I want to adjust budgets and maintain versions so that financial plans can adapt to changing circumstances.
    * As a Financial Planner, I want to generate budget forecasts so that I can project future income and expenses.
    * As a User, I want to set financial goals and track my progress towards them so that I can achieve my financial objectives.
    * As a User, I want to link my bank accounts so that my financial data can be automatically fetched for reconciliation and planning.
    * As a User, I want to receive alerts for low bank balances so that I can avoid overdraft fees.
    * As a System, I want to fetch raw bank statement data so that it can be used for reconciliation in the General Ledger.
* **Commands:** `LogExpense`, `LogMileage`, **`CreateExpenseReport`**, **`AddExpenseToReport`**, **`SubmitExpenseReport`**, **`ApproveExpenseReport`**, **`RejectExpenseReport`**, **`MarkReportReimbursed`**, `CreateBudget`, ..., `SetFinancialGoal`, ..., `LinkBankAccount`, **`WorkspaceBankStatementData`**
* **Queries:** `GetExpenseDetails`, ..., **`GetExpenseReportDetails`**, `GetBudgetDetails`, ..., `GetGoalProgress`, **`GetLinkedBankAccounts`**
* **Events:** `ExpenseLogged`, **`ExpenseReportCreated`**, **`ExpenseReportSubmitted`**, **`ExpenseReportApproved`**, **`ExpenseReportRejected`**, **`ExpenseReportReimbursed`**, `BudgetCreated`, ..., `FinancialGoalSet`, ..., `BankAccountLinked`, `BankBalanceUpdated`, **`BankStatementDataRetrieved`** (*Payload: RawBankStatementData, consumed by GL*)

### 5. Financial Analytics BC

* **Focus:** Providing deeper financial insights through ratios, trends, and visualizations.
* **Subdomains:** Supporting.
* **Aggregates:** `FinancialAnalysis` (AR): Represents a specific run/result of an analysis type (e.g., Q1 Ratio Analysis).
* **Entities:** `FinancialAnalysis` (AR), `CalculatedRatio`, `TrendDataPointSeries`.
* **Value Objects:** `AnalysisType` (Ratio, Trend), `AnalysisPeriod`, `RatioDefinition` (e.g., Current Ratio = Current Assets / Current Liabilities), `RatioValue`, `TrendMetric`, `TimePoint`, `VisualizationType` (Chart, Graph), `ExportFormat`.
* **Services:** `FinancialRatioEngine`, `TrendAnalysisEngine`, `DataVisualizer`.
* **User Stories:**
    * As a Financial Analyst, I want to perform ratio analysis (e.g., liquidity, solvency, profitability) on financial data so that I can assess the company's financial health.
    * As a Financial Analyst, I want to perform trend analysis on financial data so that I can identify patterns and forecast future performance.
    * As a Financial Analyst, I want to generate visualizations (charts, graphs) of financial data so that insights can be easily understood and communicated.
    * As a Financial Analyst, I want to export financial analysis results and data in various formats (CSV, Excel, PDF) so that I can use them in other reports or tools.
* **Commands:** `PerformRatioAnalysis`, `PerformTrendAnalysis`, `GenerateFinancialVisualization`, `ExportAnalyticsData`.
* **Queries:** `GetRatioAnalysisResults`, `GetTrendAnalysisResults`, `GetFinancialVisualizationData`.
* **Events:** `FinancialAnalysisCompleted`, `VisualizationGenerated`.

### 6. Debt Management BC

* **Focus:** Managing specific debt instruments like loans, mortgages, credit cards.
* **Subdomains:** Core/Supporting.
* **Aggregates:** `DebtAccount` (AR): Represents a single debt instrument.
* **Entities:** `DebtAccount` (AR), `DebtPaymentRecord`, `InterestAccrual`, `LateFeeApplication`.
* **Value Objects:** `DebtAccountType` (Enum: Loan, Mortgage, CreditCard, LineOfCredit), `AccountNumber`, `LenderName`, `OriginalPrincipal` (SK), `InterestRateInfo` (Rate, Type), `Term`, `PaymentSchedule`, `CurrentBalance` (SK), `AccruedInterest` (SK), `DueDate`, `PaymentAmount` (SK), `LateFeePolicy`, `AmortizationSchedule`.
* **Services:** `InterestCalculator`, `AmortizationScheduleGenerator`.
* **User Stories:**
    * As a User, I want to add new debt accounts (e.g., credit card, loan, mortgage) so that I can track all my debts in one place.
    * As a User, I want to record payments made against my debt accounts so that my balances are up-to-date.
    * As a System, I want to automatically accrue interest on debt accounts based on their terms so that balances reflect interest charges.
    * As a System/User, I want to apply late fees to overdue debt payments according to the defined policy so that penalties are correctly assessed.
    * As a User, I want to update the payment schedule for a debt account so that changes in my repayment plan are reflected.
    * As a User, I want to update the details of a debt account (e.g., interest rate, lender information) so that the information remains accurate.
    * As a User, I want to mark a debt account as paid off so that I can see it's closed.
    * As a User, I want to generate an amortization schedule for a debt account so that I can understand the breakdown of principal and interest payments over time.
* **Commands:** `AddDebtAccount`, `RecordDebtPayment`, `AccrueInterest`, `ApplyLateFee`, `UpdateDebtPaymentSchedule`, `UpdateDebtAccountDetails`, `MarkDebtPaidOff`, `GenerateAmortizationSchedule`.
* **Queries:** `GetDebtAccountDetails`, `ListDebtAccounts`, `GetTotalDebt`, `GetUpcomingDebtPayments`, `GetAmortizationSchedule`.
* **Events:** `DebtAccountAdded`, `DebtPaymentRecorded`, `InterestAccrued`, `LateFeeApplied`, `DebtAccountUpdated`, `DebtPaidOff`.

### 7. Tax Management BC

* **Focus:** Tracking tax liabilities, calculating taxes based on rules, generating reports, and storing related documents.
* **Subdomains:** Supporting.
* **Aggregates:** `TaxLiability` (AR)
* **Entities:** `TaxLiability` (AR), `TaxPayment`, `TaxReportLog`
* **Value Objects:** `TaxType`, `JurisdictionInfo`, `TaxPeriod`, `DueDate`, `CalculatedAmount` (SK), `PaidAmount` (SK), `TaxRuleConfiguration`, `ReportFormat`, **`TaxDocumentID`** (*Held by TaxLiability/ReportLog; obtained from/links to Document Management BC*)
* **Services:** `TaxCalculator`
* **User Stories:**
    * As an Accountant, I want to record tax liabilities for different tax types and jurisdictions so that all tax obligations are tracked.
    * As an Accountant, I want the system to calculate tax liabilities based on configured rules and financial data so that tax amounts are accurate.
    * As an Accountant, I want to record tax payments made so that payment history is maintained.
    * As an Accountant, I want to generate tax reports and supporting documentation so that I can prepare for tax filing.
    * As an Accountant, I want to store tax-related documents securely and link them to relevant tax liabilities or reports so that documentation is easily accessible.
    * As a System Administrator, I want to configure tax rules for different jurisdictions so that tax calculations are accurate and compliant.
* **Commands:** `RecordTaxLiability`, `CalculateTaxLiability`, `RecordTaxPayment`, `GenerateTaxReport`, `StoreTaxDocumentLink` (*associates DocumentID with tax entity*)
* **Queries:** `GetTaxLiabilityStatus`, `ListTaxPayments`, `GetTaxReport`
* **Events:** `TaxLiabilityRecorded`, `TaxLiabilityCalculated`, `TaxPaymentMade`, `TaxReportGenerated`, `TaxDocumentAssociated`.

### 8. Audit Trail BC

* **Focus:** Recording all changes to financial data for audit purposes.
* **Subdomains:** Supporting.
* **Aggregates:** `AuditEntry` (AR)
* **Entities:** `AuditEntry` (AR).
* **Value Objects:** `Timestamp`, `UserId`, `SourceContext` (e.g., AP, GL), `AggregateId`, `Action` (Enum: Create, Update, Delete), `ChangeDetails` (Field, OldValue, NewValue).
* **Services:** `AuditReportGenerator`.
* **User Stories:**
    * As a System, I want to automatically log every creation, update, and deletion of financial data, including who made the change and when, so that a complete audit trail is maintained.
    * As an Auditor/Administrator, I want to search the audit trail based on various criteria (e.g., user, date range, data entity) so that I can investigate specific activities.
    * As an Auditor/Administrator, I want to generate audit reports so that I can review changes made to the financial system for compliance and security purposes.
* **Commands:** `LogAuditEvent` (Internal command triggered by events from other BCs).
* **Queries:** `SearchAuditTrail`, `GenerateAuditReport`.
* **Events:** `AuditEventLogged`.

### 9. Currency Management BC

* **Focus:** Managing currencies and their exchange rates.
* **Subdomains:** Supporting/Generic.
* **Aggregates:** `ExchangeRateSet` (AR): Holds rates between currency pairs for a given time.
* **Entities:** `ExchangeRateSet` (AR), `CurrencyRate`.
* **Value Objects:** `Currency` (SK), `RateValue`, `EffectiveDateTime`.
* **Services:** `ExchangeRateProvider` (fetches external rates).
* **User Stories:**
    * As an Administrator, I want to update and manage exchange rates between different currencies so that financial transactions in foreign currencies are accurately converted.
    * As a System, I want to fetch the latest exchange rates from an external provider so that conversions are based on current market values.
    * As a User/System, I want to get the exchange rate for a specific currency pair and date so that I can perform accurate currency conversions for historical transactions.
* **Commands:** `UpdateExchangeRate`, `WorkspaceLatestRates`.
* **Queries:** `GetExchangeRate` (for a currency pair and date).
* **Events:** `ExchangeRateUpdated`.

### 10. Credit Score Monitoring BC

* **Focus:** Fetching, storing, analyzing credit scores and reports from various bureaus, and alerting users to significant changes or events.
* **Subdomains:** Supporting.
* **Aggregates:**
    * `CreditProfile` (AR): Represents the user's consolidated credit information and monitoring status across bureaus.
    * `CreditReport` (AR): Represents a specific credit report snapshot obtained from a bureau at a point in time.
* **Entities:**
    * `CreditProfile` (AR)
    * `CreditReport` (AR), `CreditScoreHistory` (tracks score over time), `CreditInquiry` (record of hard/soft pulls), `CreditAccountRecord` (details of accounts reported), `PublicRecord`
    * `AlertRule` (user-defined conditions for notifications)
    * `BureauConnection` (stores credentials/links to fetch data)
* **Value Objects:**
    * `BureauType` (Enum: Experian, Equifax, TransUnion)
    * `ScoreValue` (integer)
    * `ScoreModel` (e.g., FICO 8, VantageScore 3.0)
    * `ReportDate`
    * `AlertType` (e.g., New Account, High Utilization, Score Drop)
    * `AlertCondition` (e.g., Score drops > 10 points)
    * `MonitoringStatus` (Enum: Active, Paused, Error)
    * `InquiryType` (Hard, Soft)
    * `AccountStatus` (Open, Closed, Delinquent)
    * `CreditLimit` (SK: MonetaryValue)
    * `AccountBalance` (SK: MonetaryValue)
* **Services:**
    * `CreditDataFetcherService` (integrates with credit bureau APIs/services)
    * `CreditAlertingService` (evaluates rules and triggers notifications)
    * `ScoreAnalysisService` (provides insights on score factors)
* **User Stories:**
    * As a User, I want to add connections to credit bureaus (Experian, Equifax, TransUnion) so that my credit information can be fetched.
    * As a User, I want to request updates to my credit report and refresh my credit scores so that I have the latest information.
    * As a User, I want to configure alert rules for significant changes in my credit profile (e.g., new account, score drop) so that I am notified promptly.
    * As a User, I want to view my credit score (from specific bureaus or an average) and my detailed credit report so that I can understand my credit standing.
    * As a User, I want to see my credit score history so that I can track changes over time.
    * As a User, I want to list active or historical credit alerts and acknowledge them so that I am aware of important events.
    * As a User, I want to get insights into factors positively or negatively affecting my credit score so that I can take steps to improve it.
    * As a User, I want to pause or resume credit monitoring so that I have control over the service.
* **Commands:**
    * `AddBureauConnection`
    * `RequestCreditReportUpdate`
    * `RefreshCreditScores`
    * `ConfigureAlertRule`
    * `AcknowledgeCreditAlert`
    * `PauseMonitoring`
    * `ResumeMonitoring`
* **Queries:**
    * `GetCreditScore` (for a specific bureau or average)
    * `GetCreditReportDetails`
    * `GetScoreHistory`
    * `ListCreditAlerts` (active or historical)
    * `GetCreditFactors` (positive/negative contributors)
    * `GetBureauConnectionStatus`
* **Events:**
    * `BureauConnectionAdded`
    * `CreditReportRetrieved`
    * `CreditScoreUpdated`
    * `CreditAlertTriggered`
    * `AlertAcknowledged`
    * `MonitoringStatusChanged`

### 11. Investment Management BC

* **Focus:** Tracking investment accounts, holdings, transactions, performance, and potentially syncing data from external brokerages.
* **Subdomains:** Core/Supporting (depending on depth).
* **Aggregates:**
    * `InvestmentAccount` (AR): Represents a single brokerage or investment account held by the user.
    * `PortfolioSnapshot` (AR): Captures the state (holdings, value) of one or more accounts at a specific point in time for performance calculation.
* **Entities:**
    * `InvestmentAccount` (AR)
    * `Holding` (represents a specific asset held within an account)
    * `InvestmentTransaction` (records buys, sells, dividends, etc.)
    * `PortfolioSnapshot` (AR), `SnapshotHolding` (value of a holding at snapshot time)
    * `PerformanceMetricRecord` (stores calculated returns, benchmarks)
    * `BrokerageLink` (configuration for syncing data)
    * `SecurityMaster` (Stores reference data about tradable securities like stocks, bonds, ETFs)
* **Value Objects:**
    * `AccountNumber`
    * `BrokerageName`
    * `AccountType` (Enum: Taxable, Traditional IRA, Roth IRA, 401k, 529, etc.)
    * `TickerSymbol`
    * `AssetName`
    * `SecurityType` (Enum: Stock, ETF, MutualFund, Bond, Crypto, Option)
    * `Quantity` (decimal)
    * `SharePrice` (SK: MonetaryValue)
    * `CostBasis` (SK: MonetaryValue)
    * `MarketValue` (SK: MonetaryValue)
    * `TransactionType` (Enum: Buy, Sell, Dividend, Reinvest, Fee, TransferIn, TransferOut)
    * `TransactionDate`
    * `PerformanceTimeframe` (e.g., YTD, 1yr, 3yr)
    * `ReturnPercentage`
    * `BenchmarkIndex` (e.g., S&P 500)
    * `AssetAllocation` (map of SecurityType to Percentage)
* **Services:**
    * `BrokerageDataSyncService` (connects to brokerage APIs/data feeds like Plaid)
    * `PortfolioPerformanceCalculator` (calculates TWRR, MWRR, etc.)
    * `MarketDataService` (fetches current prices for securities)
* **User Stories:**
    * As an Investor, I want to manually add my investment accounts so that I can track all my investments.
    * As an Investor, I want to link my external brokerage accounts (e.g., via Plaid) so that my investment data can be automatically synced.
    * As an Investor, I want to sync investment data (holdings, transactions) for one or all linked accounts so that my portfolio is up-to-date.
    * As an Investor, I want to manually record investment transactions (buy, sell, dividend) so that my records are complete.
    * As an Investor, I want to update holding details (e.g., cost basis) so that performance calculations are accurate.
    * As an Investor, I want to generate a portfolio snapshot at a specific point in time so that I can track its historical value.
    * As an Investor, I want to calculate my portfolio performance (e.g., TWRR, MWRR) for various timeframes and compare it against benchmarks so that I can evaluate my investment strategy.
    * As a System, I want to fetch current market prices for securities so that portfolio valuations are accurate.
    * As an Investor, I want to view my account holdings, transaction history, and asset allocation so that I have a clear overview of my investments.
    * As an Investor, I want to get a summary of my investment accounts (total value, cash balance) so that I can quickly assess my overall investment status.
    * As an Investor, I want to view details about a specific security (from SecurityMaster) so that I can get more information about my investments.
* **Commands:**
    * `AddInvestmentAccountManually`
    * `LinkBrokerageAccount`
    * `SyncInvestmentData` (for one or all linked accounts)
    * `RecordManualTransaction`
    * `UpdateHoldingDetails` (e.g., cost basis)
    * `GeneratePortfolioSnapshot`
    * `CalculatePortfolioPerformance`
    * `WorkspaceCurrentMarketPrices`
* **Queries:**
    * `GetAccountHoldings`
    * `GetTransactionHistory`
    * `GetPortfolioPerformance` (for various timeframes)
    * `GetAssetAllocation`
    * `GetAccountSummary` (total value, cash balance)
    * `GetSecurityDetails` (from SecurityMaster)
    * `ListInvestmentAccounts`
* **Events:**
    * `InvestmentAccountAdded`
    * `BrokerageAccountLinked`
    * `InvestmentDataSyncStarted`
    * `InvestmentDataSyncCompleted`
    * `HoldingsUpdated`
    * `TransactionRecorded`
    * `PortfolioSnapshotGenerated`
    * `PortfolioPerformanceCalculated`
    * `MarketPricesUpdated`

### Shared Kernel (Design/Code Artifact)

* **Value Objects:** `MonetaryValue { Amount: decimal, Currency: Currency }`, `Currency { Code: string, Symbol: string }`, `FinancialCategory { Name: string, Type: enum }`, `Payee { Id: Guid, Name: string, ContactInfo: Contact, DefaultCategoryRef?: CategoryId, AutoCreateAccountPolicy?: bool }`, `ProjectLink { Id: string, Name: string }`.

### Integration & Policies (Conceptual Examples):

* **Policy:** *Paying a Bill*
    1.  User triggers `MarkBillAsPaid` in AP BC.
    2.  AP BC emits `BillPaid` event including Payee, Amount, Category, Date.
    3.  GL BC subscribes, creates `JournalEntryBatch` (Debit Expense/Asset using Category, Credit Cash/AP using Payee linkage), and triggers `PostJournalEntryBatch`.
    4.  FP&A BC subscribes to `BillPaid` (or `JournalBatchPosted`), updates `Budget` actuals for the Category.
    5.  Audit Trail BC subscribes to `BillPaid` and `JournalBatchPosted`, creates `AuditEntry` records.
* **Policy:** *Receiving Invoice Payment (Multi-Currency)*
    1.  User triggers `RecordInvoicePayment` in AR BC for a USD invoice, payment received in EUR.
    2.  AR BC queries Currency Management BC for EUR-USD rate at payment date. Calculates received amount in USD.
    3.  GL BC subscribes, creates `JournalEntryBatch` (Debit Cash EUR, potentially Debit/Credit Exchange Gain/Loss, Credit AR USD).
    4.  Audit Trail BC logs relevant entries.
* **Policy:** *Auto-Creating Ledger Account for New Payee*
    1.  User creates a `Bill` in AP BC for a new `Payee` (defined in SK).
    2.  AP BC emits `BillRecorded` including Payee details.
    3.  GL BC subscribes. Checks if a `LedgerAccount` (e.g., an Expense account) exists for this Payee.
