# Financial Management ↔ Task Management Integration Design

## 📋 Overview

This document outlines the integration strategy between the **FinancialManagement** module and the **TaskManagement** module (part of WorkManagement). The integration automates financial workflows by creating, tracking, and completing tasks for various financial activities.

## 🎯 Integration Objectives

1. **Automate Financial Workflows** - Automatically create tasks for bills, invoices, payments, and approvals
2. **Improve Payment Timeliness** - Task reminders for bill payments and invoice collections
3. **Enforce Approval Processes** - Multi-level approval workflows via tasks
4. **Budget Compliance** - Alert tasks when budgets are exceeded
5. **Goal Achievement** - Break down financial goals into actionable tasks
6. **Tax Preparation** - Structured tasks for tax filing and document collection
7. **Reconciliation Tracking** - Monthly bank reconciliation tasks

---

## 🔗 Integration Points

### 1. **Accounts Payable (Bills) → Task Management**

#### Use Cases:
- **Bill Approval Workflow**: When a bill requires approval, create approval tasks for designated approvers
- **Payment Reminders**: Create tasks X days before bill due date to schedule payment
- **Overdue Escalation**: Create escalation tasks when bills become overdue
- **Recurring Bills**: Auto-generate payment tasks for recurring bills

#### Events to Publish:
```csharp
// From FinancialManagement
BillCreatedEto              → Create approval task (if RequiresApproval)
BillApprovedEto             → Mark approval task complete, create payment task
BillRejectedEto             → Mark approval task failed, create revision task
BillDueSoonEto              → Create payment reminder task (3 days before due)
BillOverdueEto              → Create escalation task
RecurringBillScheduledEto   → Create recurring payment tasks

// From TaskManagement
BillApprovalTaskCompletedEto → Trigger bill approval in FinancialManagement
BillPaymentTaskCompletedEto  → Mark bill as paid/scheduled
```

#### Task Types to Create:
- **Bill Approval Task**: Review and approve bill
  - Priority: Based on amount (High for > $1000, Medium for $500-$1000, Low for < $500)
  - Due Date: 2 days before bill due date
  - Custom Fields: `BillId`, `BillNumber`, `VendorName`, `Amount`, `ApprovalLevel`

- **Bill Payment Task**: Schedule/process payment
  - Priority: ImportantUrgent (2 days before due), ImportantNotUrgent (> 2 days)
  - Due Date: Bill due date - 1 day
  - Custom Fields: `BillId`, `VendorId`, `PaymentMethod`, `AccountId`

---

### 2. **Accounts Receivable (Invoices) → Task Management**

#### Use Cases:
- **Invoice Follow-up**: Create follow-up tasks after invoice is sent
- **Collection Reminders**: Escalating reminders as invoice approaches due date
- **Overdue Collections**: Aggressive collection tasks for overdue invoices
- **Payment Reconciliation**: Tasks to reconcile received payments

#### Events to Publish:
```csharp
// From FinancialManagement
InvoiceCreatedEto           → Create follow-up task (7 days after send)
InvoiceDueSoonEto           → Create reminder task (3 days before due)
InvoiceOverdueEto           → Create collection task (escalating priority)
InvoicePartiallyPaidEto     → Create reconciliation task
InvoiceDisputedEto          → Create dispute resolution task

// From TaskManagement
FollowUpTaskCompletedEto    → Log contact attempt in invoice
CollectionTaskCompletedEto  → Update invoice collection notes
```

#### Task Types to Create:
- **Invoice Follow-Up Task**: Contact customer about sent invoice
  - Priority: Medium
  - Due Date: 7 days after invoice sent
  - Custom Fields: `InvoiceId`, `CustomerId`, `ContactMethod`

- **Invoice Collection Task**: Collect payment for overdue invoice
  - Priority: Increases with age (Medium at 1-7 days overdue, High at 8-14 days, Urgent > 14 days)
  - Due Date: Today + 2 days
  - Custom Fields: `InvoiceId`, `DaysOverdue`, `AmountDue`, `LastContactDate`

---

### 3. **Financial Planning (Budgets) → Task Management**

#### Use Cases:
- **Budget Approval**: Multi-level budget approval workflow
- **Budget Review**: Periodic budget variance review tasks
- **Overspend Alerts**: Investigation tasks when budget is exceeded
- **Period Close**: Month/quarter-end budget reconciliation tasks

#### Events to Publish:
```csharp
// From FinancialManagement
BudgetCreatedEto            → Create approval task
BudgetApprovedEto           → Mark approval task complete
BudgetVarianceDetectedEto   → Create investigation task (when variance > 10%)
BudgetExceededEto           → Create escalation task (when spent > allocated)
BudgetPeriodEndingEto       → Create review task (5 days before period end)

// From TaskManagement
BudgetApprovalTaskCompletedEto   → Approve budget
BudgetReviewTaskCompletedEto     → Mark budget reviewed
```

#### Task Types to Create:
- **Budget Approval Task**: Review and approve budget
  - Priority: High
  - Due Date: 3 days from creation
  - Custom Fields: `BudgetId`, `BudgetName`, `TotalAmount`, `PeriodStart`, `PeriodEnd`

- **Budget Variance Investigation Task**: Investigate budget overspend
  - Priority: High (if overspend), Medium (if variance)
  - Due Date: Today + 1 day
  - Custom Fields: `BudgetId`, `CategoryId`, `PlannedAmount`, `ActualAmount`, `VariancePercent`

---

### 4. **Financial Goals → Task Management**

#### Use Cases:
- **Goal Action Plan**: Break down financial goal into actionable steps
- **Goal Milestones**: Create tasks for goal milestones
- **Goal Check-ins**: Periodic review tasks to track progress
- **Goal Completion**: Celebration/reflection task when goal achieved

#### Events to Publish:
```csharp
// From FinancialManagement
FinancialGoalCreatedEto         → Create action planning task
GoalMilestoneReachedEto         → Create celebration task
GoalCheckInDueEto               → Create check-in task (monthly/quarterly)
GoalAtRiskEto                   → Create intervention task
GoalAchievedEto                 → Create completion/reflection task

// From TaskManagement
GoalActionTaskCompletedEto      → Update goal progress
GoalCheckInTaskCompletedEto     → Log goal review
```

#### Task Types to Create:
- **Goal Action Planning Task**: Create action plan for goal
  - Priority: Medium
  - Due Date: 7 days from goal creation
  - Custom Fields: `GoalId`, `GoalType`, `TargetAmount`, `TargetDate`

- **Goal Progress Check-In Task**: Review goal progress
  - Priority: Medium
  - Due Date: Based on goal timeline (monthly/quarterly)
  - Custom Fields: `GoalId`, `CurrentAmount`, `TargetAmount`, `ProgressPercent`

---

### 5. **Debt Management → Task Management**

#### Use Cases:
- **Payment Reminders**: Tasks for upcoming debt payments
- **Payoff Plan Tracking**: Tasks for each payment in payoff plan
- **Refinancing Opportunities**: Alert tasks when better rates available
- **Debt Consolidation**: Multi-step tasks for debt consolidation process

#### Events to Publish:
```csharp
// From FinancialManagement
DebtPaymentDueEto               → Create payment reminder task
PayoffPlanCreatedEto            → Create series of payment tasks
DebtPaidOffEto                  → Create celebration task
RefinancingOpportunityEto       → Create evaluation task

// From TaskManagement
DebtPaymentTaskCompletedEto     → Record payment in debt account
```

#### Task Types to Create:
- **Debt Payment Task**: Make debt payment
  - Priority: ImportantUrgent (if due within 3 days)
  - Due Date: Payment due date - 1 day
  - Custom Fields: `DebtAccountId`, `PaymentAmount`, `AccountNumber`, `CreditorName`

---

### 6. **Tax Management → Task Management**

#### Use Cases:
- **Tax Filing Preparation**: Multi-step task checklist for tax filing
- **Document Collection**: Tasks to gather W-2s, 1099s, receipts
- **Deadline Reminders**: Escalating reminders as tax deadline approaches
- **Quarterly Estimates**: Tasks for quarterly estimated tax payments

#### Events to Publish:
```csharp
// From FinancialManagement
TaxSeasonStartedEto             → Create document collection tasks
TaxDeadlineApproachingEto       → Create filing reminder tasks
QuarterlyEstimateDueEto         → Create payment task
TaxReturnCreatedEto             → Create review task

// From TaskManagement
TaxDocumentCollectedEto         → Mark document as collected
TaxFilingTaskCompletedEto       → Mark tax return as filed
```

#### Task Types to Create:
- **Tax Document Collection Task**: Gather tax documents
  - Priority: Increases as deadline approaches
  - Due Date: 30 days before tax deadline
  - Custom Fields: `TaxYear`, `DocumentType`, `RequiredDocuments[]`

- **Tax Filing Task**: File tax return
  - Priority: ImportantUrgent (within 7 days of deadline)
  - Due Date: Tax deadline - 3 days
  - Custom Fields: `TaxReturnId`, `TaxYear`, `FilingStatus`, `EstimatedRefund`

---

### 7. **Bank Reconciliation → Task Management**

#### Use Cases:
- **Monthly Reconciliation**: Create recurring reconciliation tasks
- **Discrepancy Investigation**: Tasks when reconciliation doesn't match
- **Transaction Review**: Review flagged/unusual transactions

#### Events to Publish:
```csharp
// From FinancialManagement
ReconciliationPeriodStartedEto  → Create reconciliation task
ReconciliationDiscrepancyEto    → Create investigation task
UnusualTransactionDetectedEto   → Create review task

// From TaskManagement
ReconciliationTaskCompletedEto  → Mark period as reconciled
```

#### Task Types to Create:
- **Bank Reconciliation Task**: Reconcile bank account
  - Priority: Medium
  - Due Date: 5th of each month
  - Recurrence: Monthly
  - Custom Fields: `BankAccountId`, `PeriodStart`, `PeriodEnd`, `StatementBalance`

---

## 🏗️ Architecture Design

### Event-Driven Integration

```
┌─────────────────────────────────────────────────────────────────┐
│                    FinancialManagement Module                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  ┌──────────┐  ┌─────────┐  ┌────────┐  ┌─────────┐           │
│  │  Bills   │  │Invoices │  │Budgets │  │  Goals  │  ...      │
│  └────┬─────┘  └────┬────┘  └───┬────┘  └────┬────┘           │
│       │             │            │            │                 │
│       └─────────────┴────────────┴────────────┘                 │
│                         │                                        │
│                    Publish Events                                │
│                         │                                        │
└─────────────────────────┼────────────────────────────────────────┘
                          │
                  Distributed Event Bus
                          │
┌─────────────────────────┼────────────────────────────────────────┐
│                         │                                         │
│                 Event Handlers                                    │
│                         │                                         │
│      ┌──────────────────┴───────────────────┐                   │
│      │                                       │                   │
│ ┌────▼────────────────┐         ┌───────────▼────────┐          │
│ │ FinancialTaskAuto-  │         │ TaskCompletionSync │          │
│ │ mationService       │         │ Handler            │          │
│ └────┬────────────────┘         └───────────┬────────┘          │
│      │                                       │                   │
│      │ Create Tasks                          │ Update Financial  │
│      │                                       │ Entities          │
│      ▼                                       ▼                   │
│  ┌─────────────────────────────────────────────────┐            │
│  │          TaskManagement (WorkManagement)        │            │
│  ├─────────────────────────────────────────────────┤            │
│  │  • Task Templates                               │            │
│  │  • TaskInstances                                │            │
│  │  • Custom Fields (Financial Entity References)  │            │
│  │  • Priority Management                          │            │
│  │  • Recurrence Rules                             │            │
│  └─────────────────────────────────────────────────┘            │
│                                                                   │
│                    WorkManagement Module                          │
└───────────────────────────────────────────────────────────────────┘
```

---

## 📊 Data Model

### Custom Fields on TaskInstance

Financial tasks will use custom fields to link back to financial entities:

```csharp
// Bill-related tasks
"FinancialEntityType": "Bill"
"BillId": Guid
"BillNumber": string
"VendorId": Guid
"VendorName": string
"Amount": decimal
"Currency": string
"DueDate": DateTime
"ApprovalLevel": string  // For approval tasks

// Invoice-related tasks
"FinancialEntityType": "Invoice"
"InvoiceId": Guid
"InvoiceNumber": string
"CustomerId": Guid
"CustomerName": string
"AmountDue": decimal
"DaysOverdue": int

// Budget-related tasks
"FinancialEntityType": "Budget"
"BudgetId": Guid
"CategoryId": Guid
"PlannedAmount": decimal
"ActualAmount": decimal
"VariancePercent": decimal

// Goal-related tasks
"FinancialEntityType": "Goal"
"GoalId": Guid
"GoalType": string
"TargetAmount": decimal
"CurrentAmount": decimal
"ProgressPercent": decimal
```

---

## 🔄 Integration Workflows

### Workflow 1: Bill Approval & Payment

```
1. User creates Bill (RequiresApproval = true)
   ↓
2. Bill aggregate publishes BillCreatedEto
   ↓
3. BillCreatedEventHandler receives event
   ↓
4. Creates "Approve Bill #1234" TaskInstance
   - Assigned to: Approver based on amount thresholds
   - Priority: High if amount > $1000, Medium otherwise
   - Due: Bill.DueDate - 2 days
   - Custom Fields: BillId, VendorName, Amount, etc.
   ↓
5. Approver completes approval task
   ↓
6. TaskCompletedEto published
   ↓
7. BillApprovalCompletedHandler calls Bill.Approve()
   ↓
8. Bill publishes BillApprovedEto
   ↓
9. BillApprovedEventHandler creates "Pay Bill #1234" TaskInstance
   - Assigned to: Accounts Payable clerk
   - Priority: ImportantUrgent if due within 3 days
   - Due: Bill.DueDate - 1 day
   ↓
10. AP clerk completes payment task
    ↓
11. BillPaymentCompletedHandler marks bill as Paid
```

### Workflow 2: Invoice Collection

```
1. User sends Invoice
   ↓
2. Invoice.Send() → Status = Sent
   ↓
3. Invoice publishes InvoiceCreatedEto
   ↓
4. InvoiceCreatedEventHandler creates "Follow up on Invoice #5678"
   - Due: 7 days after invoice sent
   - Priority: Medium
   ↓
5. System monitors invoice due date
   ↓
6. 3 days before due: Publishes InvoiceDueSoonEto
   ↓
7. Creates "Payment reminder for Invoice #5678"
   - Due: Invoice.DueDate
   - Priority: High
   ↓
8. If invoice becomes overdue: Publishes InvoiceOverdueEto
   ↓
9. Creates "Collect overdue payment for Invoice #5678"
   - Priority: Escalates based on days overdue
   - Creates new escalation task every 7 days
```

### Workflow 3: Financial Goal → Action Tasks

```
1. User creates FinancialGoal (e.g., "Save $10,000 for vacation")
   ↓
2. Goal publishes FinancialGoalCreatedEto
   ↓
3. GoalCreatedEventHandler creates initial tasks:
   a) "Create action plan for vacation savings goal"
      - Due: 7 days from now
   b) "Set up automatic monthly transfer"
      - Due: 3 days from now
   c) "Review budget to find savings opportunities"
      - Due: 5 days from now
   ↓
4. System creates recurring check-in tasks based on goal timeline:
   - Monthly review if goal > 12 months
   - Bi-weekly review if goal 3-12 months
   - Weekly review if goal < 3 months
   ↓
5. As goal progresses, publishes GoalMilestoneReachedEto (25%, 50%, 75%)
   ↓
6. Creates celebration/motivation tasks at milestones
```

---

## 🛠️ Implementation Components

### 1. Integration Events (FinancialManagement.Domain.Shared)

**Location**: `modules/FinancialManagement/src/NouAssist.FinancialManagement.Domain.Shared/Integration/Events/`

```csharp
// AccountsPayable
BillCreatedEto
BillApprovedEto
BillRejectedEto
BillPaidEto
BillDueSoonEto
BillOverdueEto

// AccountsReceivable
InvoiceCreatedEto (exists - extend)
InvoiceDueSoonEto
InvoiceOverdueEto
InvoicePartiallyPaidEto

// FinancialPlanning
BudgetCreatedEto
BudgetApprovedEto
BudgetVarianceDetectedEto
BudgetExceededEto
FinancialGoalCreatedEto
GoalMilestoneReachedEto
GoalCheckInDueEto

// DebtManagement
DebtPaymentDueEto
PayoffPlanCreatedEto

// TaxManagement
TaxDeadlineApproachingEto
QuarterlyEstimateDueEto
```

### 2. Event Handlers (WorkManagement.Domain)

**Location**: `modules/WorkManagement/src/NouAssist.WorkManagement.Domain/TaskManagement/EventHandlers/`

```csharp
BillCreatedEventHandler
    → Creates approval task if RequiresApproval
    → Creates payment reminder task

InvoiceSentEventHandler
    → Creates follow-up task
    → Schedules collection reminder tasks

BudgetCreatedEventHandler
    → Creates approval task

FinancialGoalCreatedEventHandler
    → Creates action planning tasks
    → Creates recurring check-in tasks

DebtPaymentDueEventHandler
    → Creates payment reminder task
```

### 3. Reverse Event Handlers (FinancialManagement.Domain)

**Location**: `modules/FinancialManagement/src/NouAssist.FinancialManagement.Domain/Integration/EventHandlers/`

```csharp
BillApprovalTaskCompletedHandler
    → Calls Bill.Approve()

BillPaymentTaskCompletedHandler
    → Marks Bill as Paid or Scheduled

InvoiceCollectionTaskCompletedHandler
    → Updates Invoice collection notes

BudgetApprovalTaskCompletedHandler
    → Calls Budget.Approve()
```

### 4. Domain Services

**FinancialTaskAutomationService** (WorkManagement)
```csharp
Task<List<TaskInstance>> CreateBillApprovalWorkflowAsync(Bill bill)
Task<TaskInstance> CreateBillPaymentTaskAsync(Bill bill)
Task<List<TaskInstance>> CreateInvoiceCollectionWorkflowAsync(Invoice invoice)
Task<List<TaskInstance>> CreateFinancialGoalTasksAsync(FinancialGoal goal)
Task<TaskInstance> CreateBudgetApprovalTaskAsync(Budget budget)
```

### 5. Application Services

**FinancialTaskIntegrationAppService** (FinancialManagement.Application)
```csharp
Task<FinancialTaskDashboardDto> GetFinancialTaskDashboardAsync()
Task<List<TaskDto>> GetPendingApprovalsAsync()
Task<List<TaskDto>> GetUpcomingPaymentsAsync()
Task<List<TaskDto>> GetOverdueCollectionsAsync()
Task CompleteApprovalTaskAsync(Guid taskId, bool approved, string comments)
```

---

## 📈 Benefits

### 1. **Operational Efficiency**
- Eliminate manual task creation for routine financial activities
- Automatic escalation of overdue items
- Structured workflows ensure nothing falls through cracks

### 2. **Improved Cash Flow**
- Timely bill payments avoid late fees
- Systematic invoice follow-up improves collection rates
- Payment reminders reduce missed payments

### 3. **Better Financial Control**
- Budget variance alerts enable quick corrective action
- Approval workflows enforce spending controls
- Audit trail of all financial activities via tasks

### 4. **Goal Achievement**
- Financial goals broken into actionable steps
- Regular check-ins keep goals on track
- Progress visibility motivates continued effort

### 5. **Compliance & Audit**
- Complete task history for all financial decisions
- Approval chains documented via task completion
- Tax preparation tasks ensure timely filing

---

## 🎯 Success Metrics

1. **Bill Payment Timeliness**: % of bills paid before due date
2. **Invoice Collection Rate**: Average days to payment
3. **Task Completion Rate**: % of financial tasks completed on time
4. **Budget Variance Resolution**: Time to investigate and resolve variances
5. **Goal Achievement Rate**: % of financial goals achieved on target
6. **Late Fee Reduction**: Decrease in late payment fees
7. **Approval Cycle Time**: Average time from bill creation to approval

---

## 🔮 Future Enhancements

1. **AI-Powered Task Prioritization**: ML model to prioritize based on urgency, amount, and user patterns
2. **Vendor Payment Optimization**: Suggest optimal payment timing for cash flow
3. **Collection Prediction**: Predict likelihood of invoice payment based on customer history
4. **Smart Scheduling**: Recommend best times for financial tasks based on calendar and cash flow
5. **Integration with Banking**: Automatic task creation when bank transactions detected
6. **Voice/Chatbot Task Creation**: "Hey NouAssist, remind me to pay the electric bill"

---

## 📝 Next Steps

1. Review and validate integration points with stakeholders
2. Prioritize implementation (suggest starting with Bills & Invoices)
3. Define event schemas and contracts
4. Implement core event handlers
5. Create domain services for task automation
6. Build application services and DTOs
7. Implement UI components for financial task dashboard
8. Testing and iteration
9. Documentation and training

