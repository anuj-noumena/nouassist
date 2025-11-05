# UI Breakdown: Feature Slices for Financial Bounded Contexts (Avalonia UI)

This document details the UI/UX design for the primary feature slices supporting the various financial Bounded Contexts (GL, AP, AR, FP&A, Analytics, Debt, Tax, Audit, Currency), optimized for desktop application using Avalonia UI.

## 1. General Ledger (GL) BC Feature Slices

### 1.1. `ChartOfAccountsView` Slice

**User Story:**

- "As an accountant or finance manager, I want to view and manage the Chart of Accounts (CoA), including adding new accounts, editing existing ones, and understanding the account hierarchy, so I can ensure accurate financial classification."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ChartOfAccountsView.axaml` - routed via URL pattern).
- **Organization:**
    - **`ToolBar:`** "Create Account" button (`Button`), Search field (`TextBox` with search icon), View options (Tree view, List view `ToggleButton` group).
    - **Main Content:**
        - **Tree View:** `TreeView` displaying the hierarchical structure of `LedgerAccount`s based on `ChartOfAccounts` structure. Nodes show `AccountNumber` and `AccountName`.
        - **List View:** `DataGrid` displaying `LedgerAccount`s. Columns: `AccountNumber`, `AccountName`, `AccountType`, Current Balance (`GetAccountBalance`), Actions (Button with icon for Edit).
- **Primary Action(s):** Viewing CoA structure, searching for accounts, creating new accounts (`CreateLedgerAccount`), editing account details (`UpdateLedgerAccount`).

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `TextBox`, `ToggleButton`, `TreeView`, `TreeViewItem`, `DataGrid`, buttons with icons. Account creation/editing via modal `Window` (`StackPanel`, `TextBox` for Number/Name, `ComboBox` for Type/Parent Account).
- **Desktop Optimizations:**
  - **Keyboard Navigation:** Full keyboard support with shortcuts (Ctrl+N for new account, F2 for edit)
  - **Drag & Drop:** Allow reordering accounts in tree view via drag and drop
  - **Context Menus:** Right-click menus for common operations
  - **Multi-select:** Allow selecting multiple accounts for batch operations
- **ADHD Considerations:**
    - **Visual Hierarchy:** The tree view is crucial for understanding structure. Ensure it's clear and not overly dense. Allow collapsing/expanding branches easily.
    - **Search:** Robust search is essential for quickly finding accounts in potentially large CoAs.
    - **Clear Naming:** Emphasize clear `AccountName`s alongside `AccountNumber`s.
    - **Simplified Creation:** Guide users through account creation, perhaps suggesting account numbers based on hierarchy.

### 1.2. `JournalEntryListView` Slice

**User Story:**

- "As an accountant, I want to view posted Journal Entry Batches and their individual lines (debits/credits), filter by date or account, and drill down into details, so I can review transaction history and investigate discrepancies."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`JournalEntriesView.axaml`).
- **Organization:**
    - **`ToolBar:`** Date range selector (`DatePicker` pair), Filter by Account (`AutoCompleteBox` searching `LedgerAccount`s), Search by Memo (`TextBox`). "Manual Entry" button (`Button` -> `JournalEntryForm`).
    - **Main Content:** `DataGrid` displaying `JournalEntryBatch` records. Columns: Batch ID, `TransactionDate`, `Memo`, Total Amount. Master-detail view with expandable rows or detail pane showing `JournalEntryLine`s (Account, Debit, Credit). (`GetJournalEntries`)

- **Primary Action(s):** Viewing journal entries, filtering/searching, drilling into batch details, initiating manual journal entry.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `DatePicker`, `AutoCompleteBox`, `TextBox`, `Button`, `DataGrid`, `Expander` for detail rows.
- **Desktop Optimizations:**
  - **Row Details:** Expandable detail section within DataGrid for journal lines
  - **Advanced Sorting/Filtering:** Header-based filtering capabilities
  - **Export Functionality:** Direct export to Excel/CSV
  - **Split View:** Optional master-detail split panel layout
- **User Interaction:** User views list of batches. Uses filters to narrow down results. Expands rows or clicks links to see individual debit/credit lines. Clicks "Manual Entry" to open `JournalEntryForm`.
- **ADHD Considerations:**
    - **Clear Batch View:** Keep the main table focused on batches. Use expandable rows or clear links for details to avoid clutter.
    - **Balanced Entries:** Visually emphasize that debits equal credits within a batch's detail view.
    - **Filtering:** Powerful filtering (date, account) is key to managing potentially large volumes of entries.
    - **Memo Search:** Searching by memo text is important for finding specific transactions.

### 1.3. `JournalEntryForm` Slice (Manual Entry)

**User Story:**

- "As an accountant, I need to manually create a balanced Journal Entry Batch with multiple debit and credit lines for adjustments or specific transactions."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view launched from `JournalEntryListView`.
- **Organization:** Form layout containing:
    - Batch details: `TransactionDate` (`DatePicker`), `Memo` (`TextBox`).
    - Line Item Entry: A dynamic list/grid (`DataGrid` or Eremex advanced grid) where users can add rows for `JournalEntryLine`s. Each row needs:
        - `LedgerAccount` (`AutoCompleteBox`).
        - Debit Amount (`NumericUpDown`).
        - Credit Amount (`NumericUpDown`).
        - (Optional) Line Memo (`TextBox`).
    - Validation: Display running totals for Debits and Credits, highlighting imbalance. Disable Post button if unbalanced.
    - "Post Batch" button (`Button`).

- **Primary Action(s):** Creating and posting a manual journal entry batch (`PostJournalEntryBatch`).

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, `StackPanel`, `DatePicker`, `TextBox`, `DataGrid`, `AutoCompleteBox`, `NumericUpDown`, `Button`, `TextBlock` (for totals/validation).
- **Desktop Optimizations:**
  - **Keyboard Shortcuts:** Tab navigation between fields, arrow keys for line navigation
  - **Clipboard Support:** Copy/paste from Excel into grid
  - **Autosave:** Background draft saving
  - **Templates:** Save and reuse common journal entry patterns
- **User Interaction:** User enters batch details. Adds multiple lines, selecting accounts and entering debit/credit amounts. System validates balance in real-time. User clicks "Post Batch" when balanced.
- **ADHD Considerations:**
    - **Guided Entry:** Clearly structure the form. Make adding/removing lines easy.
    - **Real-time Balance:** Immediate feedback on whether debits equal credits is crucial to prevent errors and frustration. Visually highlight the totals and any imbalance.
    - **Account Selection:** Fast and accurate account selection via autocomplete is important.
    - **Clear Errors:** If posting fails, provide clear error messages.

### 1.4. `AccountingPeriodControl` Slice

**User Story:**

- "As a finance manager, I need to view the status of accounting periods (Open/Closed) and initiate/finalize the period closing process."

**Overall Layout & Structure:**

- **Container:** Settings section or dedicated Period Management view (`AccountingPeriodsView.axaml`).
- **Organization:**
    - **Main Content:** `DataGrid` listing `AccountingPeriod`s. Columns: Period Name/Date Range, `PeriodStatus` (with visual status indicator), Actions. (`GetAccountingPeriodStatus`)
    - **Actions:** Buttons on each row (or selected row): "Initiate Close" (`Button`), "Finalize Close" (`Button`). These buttons might be disabled based on status/permissions. View `ClosingProcedureLog` link.

- **Primary Action(s):** Viewing period statuses, initiating period close (`InitiatePeriodClose`), finalizing period close (`FinalizePeriodClose`).

**Component Breakdown & Interactions:**

- **UI Elements:** `DataGrid`, Status indicators, `Button`, `Hyperlink`. Confirmation dialogs for closing actions.
- **Desktop Optimizations:**
  - **Timeline View:** Optional visualization of accounting periods
  - **Calendar Integration:** Show accounting periods in relation to calendar
  - **Batch Operations:** Allow initiating multiple period closings
- **User Interaction:** User views period statuses. Clicks "Initiate Close" (triggers background process/checks). Clicks "Finalize Close" after review/checks are complete. Clicks link to view logs.
- **ADHD Considerations:**
    - **Clear Status:** Use distinct colors/icons for Open vs. Closed periods.
    - **Guided Process:** Clearly indicate the steps involved in closing (Initiate -> Review -> Finalize). Provide feedback during the process.
    - **Prevent Accidental Actions:** Use confirmation dialogs for irreversible actions like closing a period. Clearly show prerequisites for closing.

### 1.5. `BankReconciliationView` Slice

**User Story:**

- "As an accountant, I need to view imported bank statement lines alongside potentially matching GL transactions, manually match items, trigger auto-reconciliation, and see the overall reconciliation status for a bank account."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`BankReconciliationView.axaml`).
- **Organization:**
    - **`ToolBar:`** Bank Account selector (`ComboBox`), Statement Period selector (`ComboBox` or `DatePicker` pair), "Auto-Reconcile" button (`Button`), Filter (`ComboBox`: Unreconciled, Matched, All). Import button (`Button` - triggers `ImportBankStatement` if manual).
    - **Main Content:** Split panel or side-by-side grids:
        - **Left Grid:** `DataGrid` for `BankStatementLine`s for the selected statement (`GetBankStatementDetails`, `GetUnreconciledStatementLines`). Columns: Date, Description, Amount, Status (`ReconciliationStatus`).
        - **Right Grid:** `DataGrid` for potential matching `JournalEntryLine`s (filtered based on selected statement line). Columns: Date, Account, Memo, Amount.
    - **Matching Interface:** When a statement line is selected, the right table updates. Buttons appear: "Match Selected" (`MatchStatementLine`), potentially showing `MatchConfidenceScore` if suggested by `BankReconciliationService`.
    - **Summary:** Panel showing overall `ReconciliationStatus` (Balance per Bank, Balance per GL, Outstanding Items, Difference). "Mark as Reconciled" button (`MarkAsReconciled`).

- **Primary Action(s):** Viewing statement lines and GL entries, performing manual matches (`MatchStatementLine`), triggering auto-reconciliation (`AutoReconcileStatement`), viewing status (`GetReconciliationStatus`), marking as reconciled (`MarkAsReconciled`).

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `ComboBox`, `DatePicker`, `Button`, `DataGrid` (x2), `Panel`, `TextBlock`. Matching logic involves backend service (`BankReconciliationService`).
- **Desktop Optimizations:**
  - **Drag & Drop Matching:** Allow dragging items between panels to match
  - **Visual Matching Lines:** Draw connection lines between matched items
  - **Split/Merge Support:** Tools to split bank entries or combine multiple entries
  - **Document Preview:** Show uploaded bank statements alongside entries
- **User Interaction:** User selects bank account and statement period. Views unreconciled lines. Selects a statement line, reviews suggested GL matches. Clicks "Match Selected" to confirm a manual match. Clicks "Auto-Reconcile" to run automated matching. Reviews summary and clicks "Mark as Reconciled" when complete.
- **ADHD Considerations:**
    - **Clear Layout:** The side-by-side view must clearly distinguish bank lines from GL lines. Highlight the selected line on both sides if possible.
    - **Focus on Unreconciled:** Default view should prioritize unreconciled items.
    - **Visual Matching Cues:** Clearly indicate matched items (e.g., greyed out, checkmark icon). Show confidence scores for suggestions.
    - **Simplified Summary:** The reconciliation summary needs to be very clear about the key numbers and any difference.
    - **Guided Workflow:** Break down the process: Import -> Auto-Match -> Manual Review -> Reconcile.

## 2. Accounts Payable (AP) BC Feature Slices

### 2.1. `BillListView` Slice

**User Story:**

- "As an AP clerk or manager, I want to see a list of incoming bills, filterable by status (e.g., Pending Approval, Approved, Scheduled, Paid), due date, or payee, so I can manage the bill payment workflow."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`BillsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Record Bill" button (`Button`), Filter by `BillStatus` (`ComboBox`), Filter by Payee (`AutoCompleteBox`), Date range filter for Due Date (`DatePicker` pair).
    - **Main Content:** `DataGrid` displaying `Bill` records. Columns: Payee (`PayeeRef`), `BillReference`, Issue Date, Due Date, Amount (`MonetaryValue`), Status (with visual indicator), Actions (Button with icon for View/Edit/Pay). (`ListBillsByStatus`)

- **Primary Action(s):** Viewing bills, filtering/searching, navigating to bill details, initiating bill recording.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `AutoCompleteBox`, `DatePicker`, `DataGrid`, Status indicators, Buttons with icons.
- **Desktop Optimizations:**
  - **Grouping:** Group bills by status, due date, or vendor
  - **Custom Views:** Save and switch between different view configurations
  - **Payment Scheduling:** Visual calendar for scheduling payments
  - **Batch Processing:** Select multiple bills for batch operations
- **User Interaction:** User views the list, applies filters. Clicks view/edit icon to see `BillDetailView`. Clicks "Record Bill" to open `BillEntryForm`. Clicks "Pay" icon (might trigger `InitiateBillPaymentRun` or navigate to payment screen).
- **ADHD Considerations:**
    - **Status Clarity:** Use color-coded indicators for bill status (e.g., Red for Overdue, Orange for Due Soon, Green for Paid).
    - **Prioritization:** Allow sorting by Due Date to easily see upcoming payments. Default view might show unpaid bills first.
    - **Filtering:** Essential filters for status and due date. Payee filter helps manage vendor-specific bills.
    - **Clear Actions:** Action icons should be clear (View, Edit, Pay).

### 2.2. `BillEntryForm` Slice

**User Story:**

- "As an AP clerk, I need to record a new bill, either manually entering details or uploading a document for OCR processing, assigning categories, and scheduling it if it's recurring."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`BillEntryView.axaml`).
- **Organization:** Form layout containing:
    - Payee (`AutoCompleteBox` searching `Payee` SK, potentially allowing creation).
    - `BillReference` (`TextBox`).
    - Issue Date (`DatePicker`), Due Date (`DatePicker`).
    - Amount (`NumericUpDown` with `Currency` selection via SK).
    - Category (`AutoCompleteBox` searching `FinancialCategory` SK).
    - Project (`AutoCompleteBox` searching `ProjectLink` SK - optional).
    - Document Upload (Button triggering `OpenFileDialog` - triggers `BillOcrProcessor` service). Display `OcrExtractedData` for confirmation/correction. Link to `DocumentID`.
    - Recurring Options (`CheckBox`, `ComboBox` for frequency, `DatePicker` for end date - `ScheduleRecurringBill`).
    - "Save Bill" button (`Button`).

- **Primary Action(s):** Recording bill details (`RecordBill`), categorizing (`CategorizeBill`), scheduling (`ScheduleRecurringBill`), uploading documents.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Form layout, `AutoCompleteBox`, `TextBox`, `DatePicker`, `NumericUpDown`, Button with `OpenFileDialog`, `CheckBox`, `ComboBox`, `Button`, `Hyperlink` (to document).
- **Desktop Optimizations:**
  - **Document Preview:** Side-by-side document viewer with form
  - **OCR Highlighting:** Visualize OCR-extracted data on the document
  - **Drag & Drop:** Support dragging files from file explorer
  - **Templates:** Save and apply vendor-specific templates
- **User Interaction:** User fills in fields manually. Optionally uploads a document; OCR data populates fields for review. Sets recurring schedule if needed. Saves the bill.
- **ADHD Considerations:**
    - **OCR Assistance:** OCR significantly reduces manual entry friction. Clearly show extracted data and allow easy correction.
    - **Clear Fields:** Use appropriate input types and clear labels.
    - **Required Fields:** Indicate mandatory fields.
    - **Payee/Category Selection:** Fast autocomplete is important. Allow creating new payees/categories on the fly if needed (with confirmation).
    - **Document Link:** Make it easy to see the linked document (`ReceiptDocumentID`/`DocumentID`).

### 2.3. `BillDetailView` Slice

**User Story:**

- "As an AP clerk or approver, I want to view all details of a specific bill, including its status, associated document, payment history, and categorization, so I can review it for accuracy or payment."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`BillDetailView.axaml`).
- **Organization:**
    - **Header:** Payee Name, Amount, Due Date, Status (visual indicator).
    - **Main Content:** Grid or Panel sections:
        - **Bill Details:** Issue Date, Bill Reference, Category, Project.
        - **Document Viewer:** Embedded viewer or link (`Hyperlink`) to view the document associated with `DocumentID`.
        - **Payment History:** DataGrid showing `BillPayment` records (Date Paid, Amount, Method). (`GetBillDetails` provides this).
        - **Actions:** "Approve Bill" button (if approval workflow exists), "Initiate Payment" button (`InitiateBillPaymentRun`), "Mark as Paid" button (`MarkBillAsPaid` - for manual recording), "Edit Bill" button.

- **Primary Action(s):** Viewing bill details, viewing associated document, initiating payment, marking as paid manually.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Status indicator, `Grid`, `Panel`, `TextBlock`, `Hyperlink`, `DataGrid`, `Button`.
- **Desktop Optimizations:**
  - **Tabbed Interface:** Organize content in tabs for easier navigation
  - **Document Annotation:** Allow adding notes to the document
  - **Payment Workflow Visualization:** Show progress in payment workflow
  - **Related Bills:** Show other bills from same vendor
- **User Interaction:** User reviews details. Clicks link to view document. Clicks action buttons (Approve, Initiate Payment, Mark Paid, Edit).
- **ADHD Considerations:**
    - **Key Info Prominent:** Display Payee, Amount, Due Date, and Status clearly at the top.
    - **Easy Document Access:** One-click access to the scanned bill/document is crucial.
    - **Clear Payment Status:** Clearly show if the bill is unpaid, partially paid, or fully paid, along with payment details.
    - **Action Buttons:** Clearly label action buttons and disable irrelevant ones based on status.

### 2.4. `PaymentRunView` Slice

**User Story:**

- "As an AP manager, I want to initiate a payment run for approved bills due within a certain period, review the proposed payments, generate a payment file or submit directly, and monitor the status."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`PaymentRunView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Initiate New Run" button (`Button`).
    - **Main Content:**
        - **Run Configuration (Dialog/Form):** Select payment date range, payment method (`ComboBox`). Triggers `InitiateBillPaymentRun`.
        - **Run Review Table:** `DataGrid` displaying bills included in the *current* run. Columns: Payee, Bill Ref, Due Date, Amount, Status (Pending, Processing, Paid/Failed). Checkboxes to include/exclude. (`GetUpcomingPayments` filtered by run criteria, `GetPaymentRunStatus`).
        - **Run Actions:** "Generate Payment File" button (`GeneratePaymentFile`), "Submit Payments Directly" button (`SubmitPaymentDirectly` - if API integration exists), View Run History link.

- **Primary Action(s):** Initiating payment runs, reviewing included bills, generating payment files or submitting directly, monitoring status.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `Window`/Form, `ComboBox`, `DatePicker`, `DataGrid`, `CheckBox`, `Hyperlink`. File generation/submission involves backend services.
- **Desktop Optimizations:**
  - **Payment Calendar:** Visual timeline of scheduled payments
  - **Approval Workflow:** Visual progress tracking through approval stages
  - **Cash Flow Impact:** Visualize impact on cash position
  - **Batch File Preview:** Preview and edit payment files before submission
- **User Interaction:** User clicks "Initiate New Run", configures criteria. Reviews the list of bills proposed for payment, potentially deselecting some. Clicks "Generate File" or "Submit Directly". Monitors the status of the run (`GetPaymentRunStatus`) and individual payments.
- **ADHD Considerations:**
    - **Guided Process:** Clearly separate run configuration, review, and execution steps.
    - **Clear Selection:** Make it obvious which bills are included in the run. Provide totals.
    - **Status Feedback:** Provide clear, real-time status updates for the run and individual payments (Pending, Submitted, Confirmed, Failed).
    - **Error Handling:** Clearly indicate any failed payments with reasons.

## 3. Accounts Receivable (AR) BC Feature Slices

### 3.1. `ClientListView` Slice

**User Story:**

- "As an AR specialist or salesperson, I want to view a list of clients, see key contact information and outstanding balances, and quickly navigate to their details."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ClientsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Create Client" button (`Button`), Search field (`TextBox`).
    - **Main Content:** `DataGrid` displaying `Client` records. Columns: Client Name (`Hyperlink` to `ClientDetailView`), Primary Contact (`ClientContact`), Outstanding Balance (`GetAccountsReceivableAging` summary per client), Actions (Button with icon for Edit). (`GetClientDetails` for list).

- **Primary Action(s):** Viewing clients, searching, navigating to client details, initiating client creation (`CreateClient`).

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `TextBox`, `DataGrid`, `Hyperlink`, Button with icon.
- **Desktop Optimizations:**
  - **Client Cards:** Optional card view with key client metrics
  - **Contact Integration:** Integration with system contacts
  - **Client Grouping:** Group by industry, size, or status
  - **Quick Actions:** Context menu with common client operations
- **User Interaction:** User views list, searches for clients. Clicks client name to view details. Clicks "Create Client" to open `ClientEntryForm`.
- **ADHD Considerations:**
    - **Key Info:** Highlight Client Name and Outstanding Balance.
    - **Search:** Fast search is important.
    - **Clear Navigation:** Easy links to client details.

### 3.2. `ClientEntryForm` Slice

**User Story:**

- "As an AR specialist, I need to create a new client record or edit an existing one, capturing their name, contact information, and any specific billing preferences."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`ClientEntryView.axaml`).
- **Organization:** Form layout containing:
    - Client Name (`TextBox`).
    - Contact Information (`TextBox` for Email, Phone, Address - potentially structured `ContactInfo` VO fields). Add multiple `ClientContact`s.
    - Billing Preferences (Payment Terms `ComboBox`, Preferred Delivery Method `ComboBox`).
    - "Save Client" button (`Button`).

- **Primary Action(s):** Creating (`CreateClient`) or updating (`UpdateClient`) client details.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Form layout, `TextBox`, `ComboBox`, `Button`. Component for adding multiple contacts.
- **Desktop Optimizations:**
  - **Address Validation:** Integration with address validation services
  - **Contact Import:** Import from system contacts or CSV
  - **Form Validation:** Real-time validation with visual feedback
  - **History Tracking:** View previous versions of client data
- **User Interaction:** User fills in client details and contact information. Selects preferences. Saves the client record.
- **ADHD Considerations:**
    - **Clear Sections:** Separate client details from contact information clearly.
    - **Structured Input:** Use specific fields for email, phone, etc., rather than large text boxes.
    - **Required Fields:** Indicate mandatory fields.

### 3.3. `InvoiceListView` Slice

**User Story:**

- "As an AR specialist, I want to see a list of invoices, filterable by status (Draft, Sent, Paid, Overdue), client, or date range, so I can manage the invoicing and collections process."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`InvoicesView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Create Invoice" button (`Button`), Filter by `InvoiceStatus` (`ComboBox`), Filter by Client (`AutoCompleteBox`), Date range filter (`DatePicker` pair).
    - **Main Content:** `DataGrid` displaying `Invoice` records. Columns: `InvoiceNumber`, Client Name, Issue Date, Due Date, Amount (`MonetaryValue`), Status (with visual indicator), Actions (Button with icon for View/Edit/Send/Record Payment). (`ListInvoicesByStatus`)

- **Primary Action(s):** Viewing invoices, filtering/searching, navigating to invoice details, initiating invoice creation.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `AutoCompleteBox`, `DatePicker`, `DataGrid`, Status indicators, Buttons with icons.
- **Desktop Optimizations:**
  - **Aging Analysis:** Visual aging buckets for receivables
  - **Collection Workflow:** Track invoices through collection stages
  - **Batch Operations:** Select multiple invoices for batch actions
  - **Customer Statement:** Generate statements for selected periods
- **User Interaction:** User views list, applies filters. Clicks view/edit icon for `InvoiceDetailView`. Clicks "Create Invoice" for `InvoiceCreateEditForm`. Clicks action icons (Send, Record Payment).
- **ADHD Considerations:**
    - **Status Clarity:** Use color-coded indicators for invoice status (e.g., Red for Overdue, Orange for Due Soon, Blue for Sent, Green for Paid).
    - **Prioritization:** Allow sorting by Due Date. Default view might show unpaid/overdue invoices first.
    - **Filtering:** Essential filters for status, client, and date range.
    - **Clear Actions:** Action icons should be clear (View, Edit, Send, Record Payment).

### 3.4. `InvoiceCreateEditForm` Slice

**User Story:**

- "As an AR specialist, I need to create a new invoice or edit a draft, adding line items, selecting the client, setting dates, and finalizing it for sending."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`InvoiceCreateEditView.axaml`).
- **Organization:** Form layout containing:
    - Client (`AutoCompleteBox` searching `Client` records).
    - `InvoiceNumber` (auto-generated or manual `TextBox`).
    - Issue Date (`DatePicker`), Due Date (`DatePicker`).
    - Line Item Entry: Dynamic `DataGrid` for `InvoiceLineItem`s. Each row: `LineItemDescription` (`TextBox`), Quantity (`NumericUpDown`), Unit Price (`NumericUpDown`), Total (calculated). Link to `ProjectTaskLink` (optional `AutoCompleteBox`).
    - Summary: Subtotal, Tax (calculated or manual), Total Amount (`MonetaryValue`).
    - Notes/Terms (`TextBox`).
    - Actions: "Save Draft" button (`CreateInvoice`/`UpdateInvoice`), "Finalize & Preview" button (`FinalizeInvoice`).

- **Primary Action(s):** Creating/editing invoice details and line items (`CreateInvoice`, `UpdateInvoice`), finalizing the invoice (`FinalizeInvoice`).

**Component Breakdown & Interactions:**

- **UI Elements:** View, Form layout, `AutoCompleteBox`, `TextBox`, `DatePicker`, `DataGrid`, `NumericUpDown`, `Button`.
- **Desktop Optimizations:**
  - **Split View:** Edit form and preview side-by-side
  - **Templates:** Save and apply invoice templates
  - **Product Catalog:** Drag items from product catalog to invoice
  - **Excel Import:** Import line items from spreadsheet
- **User Interaction:** User selects client, sets dates. Adds/edits line items. Reviews totals. Saves draft or finalizes.
- **ADHD Considerations:**
    - **Clear Layout:** Separate header (client, dates), line items, and summary sections.
    - **Line Item Ease:** Make adding/editing line items simple. Auto-calculate row/invoice totals.
    - **Client Selection:** Fast autocomplete for client selection.
    - **Preview:** Provide a clear preview before finalizing/sending.

### 3.5. `InvoiceDetailView` Slice

**User Story:**

- "As an AR specialist, I want to view the details of a specific invoice, see its status, send it to the client, record payments received (manually or via gateway), and send reminders."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`InvoiceDetailView.axaml`).
- **Organization:**
    - **Header:** Invoice Number, Client Name, Amount, Due Date, Status (visual indicator).
    - **Main Content:**
        - **Invoice Preview:** Visual representation of the invoice as it would be sent.
        - **Payment History:** `DataGrid` showing `InvoicePayment` records (Date, Amount, Method). (`GetInvoiceDetails`).
        - **Delivery Status:** Email delivery status (Sent, Opened, Bounced).
    - **Actions (`DockPanel` or action bar):**
        - "Send Invoice" button (`Button` - disabled if already sent).
        - "Record Manual Payment" button (`Button` -> opens dialog).
        - "Charge Card / Process Online Payment" button (`ChargeClientCard` / `ProcessOnlinePayment` - requires `PaymentGatewayIntegrationService`).
        - "Send Reminder" button (`SendPaymentReminder` - enabled if overdue/due soon).
        - "Edit" button (if Draft status).

- **Primary Action(s):** Viewing invoice, sending (`SendInvoice`), recording payments (`RecordInvoicePayment`, `ChargeClientCard`, `ProcessOnlinePayment`), sending reminders (`SendPaymentReminder`).

**Component Breakdown & Interactions:**

- **UI Elements:** View, Status indicator, Invoice Preview Component, `DataGrid`, `TextBlock`, `DockPanel`/Action bar, `Button`. Payment recording dialog (Form layout, `NumericUpDown`, `DatePicker`, `ComboBox` for method).
- **Desktop Optimizations:**
  - **Document Generation:** Generate PDF directly from application
  - **Email Integration:** Direct email sending with templates
  - **Payment Processing:** Integrated payment terminal support
  - **Communication Log:** Track all client communications
- **User Interaction:** User views invoice details and preview. Clicks "Send". Clicks "Record Payment" to log manual payments or "Charge Card" to initiate gateway payment. Clicks "Send Reminder".
- **ADHD Considerations:**
    - **Clear Status & Actions:** Prominently display the status and make relevant actions obvious (disable irrelevant buttons).
    - **Invoice Preview:** A clear visual preview is essential.
    - **Payment Recording Simplicity:** Make logging payments (manual or gateway) straightforward.
    - **Reminder Logic:** Automate reminder suggestions based on due dates.

## 4. Financial Planning & Analysis (FP&A) BC Feature Slices

### 4.1. `ExpenseLoggingForm` Slice

**User Story:**

- "As an employee or finance user, I need a quick way to log expenses as they occur, categorize them, attach receipts, and note the payee."

**Overall Layout & Structure:**

- **Container:** Modal `Window` (for quick logging) or dedicated view (`ExpenseLogView.axaml`).
- **Organization:** Form layout containing:
    - `ExpenseDate` (`DatePicker`).
    - Amount (`NumericUpDown` with `Currency` selection).
    - Category (`AutoCompleteBox` searching `FinancialCategory` SK).
    - Payee (`AutoCompleteBox` searching `Payee` SK).
    - Description/Notes (`TextBox`).
    - Receipt Upload (Button with `OpenFileDialog` - links to `Document Management BC`, stores `ReceiptDocumentID`).
    - (Optional) Mileage Details (`LogMileage` - `NumericUpDown` for distance, `TextBox` for route).
    - "Log Expense" button (`Button`).

- **Primary Action(s):** Logging expenses (`LogExpense`), logging mileage (`LogMileage`), uploading receipts.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Form layout, `DatePicker`, `NumericUpDown`, `AutoCompleteBox`, `TextBox`, Button with `OpenFileDialog`, `Button`.
- **Desktop Optimizations:**
  - **Receipt Scanning:** Direct scanner integration
  - **OCR Processing:** Extract data from receipts automatically
  - **Recurring Expenses:** Create expense templates for regular items
  - **Drag & Drop:** Support dragging files from file explorer
- **User Interaction:** User fills in expense details, selects category/payee, uploads receipt image/PDF. Saves the expense.
- **ADHD Considerations:**
    - **Quick Entry:** Prioritize speed for logging common expenses. Use dialogs for quick logs.
    - **Receipt Capture:** Make receipt upload seamless (e.g., using scanner integration). OCR could pre-fill fields.
    - **Category/Payee Assistance:** Fast autocomplete and remembering frequent entries.
    - **Clear Feedback:** Confirm successful logging.

### 4.2. `ExpenseReportListView` Slice

**User Story:**

- "As an employee or manager, I want to view my expense reports, see their status (Draft, Submitted, Approved, Reimbursed), and create new reports."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ExpenseReportsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Create Report" button (`Button`), Filter by `ExpenseReportStatus` (`ComboBox`), Date range filter (`DatePicker` pair).
    - **Main Content:** `DataGrid` displaying `ExpenseReport` records. Columns: Report Name/Period, Submission Date, Total Amount, Status (with visual indicator), Actions (Button with icon for View/Edit/Submit). (`GetExpenseReportDetails` for list).

- **Primary Action(s):** Viewing expense reports, filtering by status/date, initiating report creation (`CreateExpenseReport`).

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `DatePicker`, `DataGrid`, Status indicators, Buttons with icons.
- **Desktop Optimizations:**
  - **Approval Workflow:** Visual progress tracking
  - **Batch Submission:** Submit multiple reports at once
  - **Report Templates:** Save and reuse report templates
  - **Export Options:** Generate PDF or Excel exports
- **User Interaction:** User views reports, applies filters. Clicks "Create Report". Clicks action icons to view, edit (if draft), or submit.
- **ADHD Considerations:**
    - **Status Clarity:** Use clear, color-coded status indicators.
    - **Actionable List:** Make it easy to see reports needing action (Drafts to submit, Submitted awaiting approval).
    - **Filtering:** Filter by status is essential.

### 4.3. `ExpenseReportEditView` Slice

**User Story:**

- "As an employee, I need to create an expense report for a specific period, add logged expenses to it, review the total, and submit it for approval."
- "As a manager, I need to review a submitted expense report, see the included expenses and receipts, and approve or reject it."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ExpenseReportEditView.axaml`).
- **Organization:**
    - **Header:** Report Name/Period (`TextBox`), Total Amount, Status (visual indicator).
    - **Expense Selection:**
        - `DataGrid` displaying `ExpenseReportLine`s already added. Columns: Date, Category, Payee, Amount, Receipt (`Hyperlink` to `ReceiptDocumentID`). Remove button (Button with icon).
        - "Add Expense" button (`Button` -> opens dialog/panel).
    - **Add Expense Dialog/Panel:** `DataGrid` displaying unassigned `Expense` records (filtered by date range). Checkboxes to select expenses. "Add Selected Expenses" button (`AddExpenseToReport`).
    - **Actions (depending on Status/Role):**
        - Employee (Draft): "Submit Report" button (`SubmitExpenseReport`), "Save Draft" button.
        - Manager (Submitted): "Approve Report" button (`ApproveExpenseReport`), "Reject Report" button (`RejectExpenseReport` - requires reason `TextBox`).
        - Finance (Approved): "Mark Reimbursed" button (`MarkReportReimbursed`).

- **Primary Action(s):** Creating report details, adding expenses (`AddExpenseToReport`), submitting (`SubmitExpenseReport`), approving (`ApproveExpenseReport`), rejecting (`RejectExpenseReport`), marking reimbursed (`MarkReportReimbursed`).

**Component Breakdown & Interactions:**

- **UI Elements:** View, `TextBox`, Status indicator, `DataGrid` (x2), `Hyperlink`, Button with icon, `Button`, `CheckBox`, `Window`/Panel.
- **Desktop Optimizations:**
  - **Receipt Viewer:** Integrated document viewer
  - **Dual-Pane Interface:** Expenses and receipt side-by-side
  - **Comments and Annotations:** Add notes to specific expenses
  - **Approval History:** Track all approval actions
- **User Interaction:** Employee creates report, adds expenses from list, reviews, submits. Manager reviews submitted report, checks expenses/receipts, approves or rejects. Finance marks as reimbursed.
- **ADHD Considerations:**
    - **Clear Workflow:** Visibly separate adding expenses from submitting/approving actions. Clearly indicate the current status and available actions.
    - **Easy Expense Addition:** Make selecting and adding logged expenses simple (checkboxes, clear list).
    - **Receipt Access:** One-click access to view receipts for each line item is crucial for approvers.
    - **Approval Simplicity:** Make Approve/Reject actions clear. Require a reason for rejection.

### 4.4. `BudgetView` Slice

**User Story:**

- "As a finance manager or individual, I want to define budgets for different categories over specific periods, track actual spending against the budget, and see forecasts."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`BudgetsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Create Budget" button (`Button`), Budget selector (`ComboBox` to choose active budget/version).
    - **Main Content:**
        - **Budget Overview:** Panel showing Budget Name, Period, Total Budgeted Amount.
        - **Category Allocations:** `DataGrid` displaying `CategoryAllocation`s. Columns: Category Name, Budgeted Amount, Actual Spent (from GL/AP data via events), Remaining/Over (with visual indicator), % Used (`ProgressBar`). (`GetBudgetDetails`)
        - **Forecast:** (Optional) Chart component (Eremex chart control or other visualization) showing `BudgetForecastRecord` data (`BudgetForecaster` service results).

- **Primary Action(s):** Viewing budget allocations and usage (`GetBudgetDetails`), creating new budgets (`CreateBudget`), potentially editing allocations (via Dialog).

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `Panel`, `TextBlock`, `DataGrid`, Status indicators, `ProgressBar`, Chart component. Budget creation/editing via `Window` (Form layout, `TextBox` for name, `DatePicker` pair, grid/list to define `CategoryAllocation` amounts).
- **Desktop Optimizations:**
  - **Budget Scenarios:** Create multiple budget scenarios
  - **What-If Analysis:** Interactive forecasting tools
  - **Budget Versioning:** Track budget changes over time
  - **Export and Reporting:** Generate budget reports
- **User Interaction:** User selects a budget to view. Reviews spending against allocations per category. Clicks "Create Budget" to define a new one.
- **ADHD Considerations:**
    - **Visual Feedback:** Progress bars and color-coding (e.g., red for over budget) provide quick visual understanding of budget status per category.
    - **Clear Comparison:** Clearly show Budgeted vs. Actual amounts.
    - **Simplicity:** Keep the main view focused on tracking. Make budget creation straightforward, perhaps suggesting categories.
    - **Forecast Clarity:** If forecasts are shown, ensure the chart is easy to interpret.

### 4.5. `FinancialGoalView` Slice

**User Story:**

- "As an individual, I want to set financial goals (e.g., 'Save $5000 for Vacation'), track my contributions towards them, and see my progress."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`FinancialGoalsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Set New Goal" button (`Button`).
    - **Main Content:** `ItemsControl` or `Grid` displaying `FinancialGoal`s as cards/panels.
        - Each card shows: `GoalDescription`, Target Amount, Current Saved Amount (`GetGoalProgress`), Target Date, Progress Bar (`ProgressBar`).
        - Button to "Log Contribution" (`Button` -> opens dialog).

- **Primary Action(s):** Defining goals (`SetFinancialGoal`), viewing progress (`GetGoalProgress`), logging contributions (`GoalContributionLog` - *Note: Command missing, assuming internal logic or link to savings account*)

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ItemsControl`/`Grid`, `Border`/Panel, `TextBlock`, `ProgressBar`. Goal definition via `Window` (Form layout, `TextBox` for description, `NumericUpDown` for target, `DatePicker`). Contribution logging via `Window` (`NumericUpDown`, `DatePicker`).
- **Desktop Optimizations:**
  - **Goal Timeline:** Visual timeline of goals and progress
  - **Milestone Tracking:** Define and track interim milestones
  - **Forecast Tools:** Predict completion dates based on contribution rates
  - **Goal Categories:** Organize goals into categories
- **User Interaction:** User views goals and progress. Clicks "Set New Goal" to define one. Clicks "Log Contribution" to record savings towards a goal.
- **ADHD Considerations:**
    - **Visual Progress:** Progress bars are highly motivating.
    - **Clear Goals:** Ensure goal descriptions and targets are clear.
    - **Simple Contribution:** Make logging contributions easy. Consider linking to bank accounts for automation (advanced).
    - **Celebrate Milestones:** (Gamification) Acknowledge when goals are reached.

## 5. Financial Analytics BC Feature Slices

### 5.1. `FinancialAnalysisRunner` Slice

**User Story:**

- "As a financial analyst, I want to initiate different types of financial analyses (like Ratio or Trend analysis) for specific periods and data sets."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`RunAnalysisView.axaml`).
- **Organization:** Form layout containing:
    - `AnalysisType` selector (`ComboBox`: Ratio, Trend).
    - `AnalysisPeriod` selector (`DatePicker` pair).
    - Data Source selector (if applicable, e.g., specific company division - depends on data model granularity).
    - Specific options based on type:
        - Ratio: Select specific `RatioDefinition`s (`ListBox` or `ComboBox` with multiple selection).
        - Trend: Select `TrendMetric`s (`ListBox` or `ComboBox` with multiple selection).
    - "Run Analysis" button (`Button`).

- **Primary Action(s):** Configuring and initiating financial analyses (`PerformRatioAnalysis`, `PerformTrendAnalysis`).

**Component Breakdown & Interactions:**

- **UI Elements:** View, Form layout, `ComboBox`, `DatePicker`, `ListBox`, `Button`.
- **Desktop Optimizations:**
  - **Analysis Templates:** Save and reuse analysis configurations
  - **Scheduled Analysis:** Configure recurring analysis runs
  - **Batch Processing:** Run multiple analyses sequentially
  - **Algorithm Selection:** Advanced options for analysis methods
- **User Interaction:** User selects analysis type, period, and specific metrics/ratios. Clicks "Run Analysis". System performs calculations in the background.
- **ADHD Considerations:**
    - **Guided Selection:** Clearly explain each analysis type and the available metrics/ratios.
    - **Clear Parameters:** Make period and data source selection obvious.
    - **Feedback:** Provide clear feedback that the analysis has started and indicate when results are ready (e.g., notification, redirect to results view).

### 5.2. `FinancialAnalysisResultsView` Slice

**User Story:**

- "As a financial analyst or manager, I want to view the results of completed financial analyses, including calculated ratios, trend data points, and visualizations."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`AnalysisResultsView.axaml`). Could also be a list view of past analyses.
- **Organization:**
    - **Header:** Analysis Type, Period, Run Date. "Export" button (`ExportAnalyticsData`).
    - **Main Content:** Dynamically displays based on `AnalysisType`:
        - **Ratio Results:** `DataGrid` showing `CalculatedRatio`s. Columns: Ratio Name, `RatioValue`, Comparison (e.g., vs. previous period, industry benchmark - if available). (`GetRatioAnalysisResults`)
        - **Trend Results:** Chart component (Eremex chart control) displaying `TrendDataPointSeries` for selected metrics. Table below chart with data points. (`GetTrendAnalysisResults`)
        - **Visualizations:** Display generated charts/graphs (`GetFinancialVisualizationData`, `GenerateFinancialVisualization`).

- **Primary Action(s):** Viewing analysis results (ratios, trends, visualizations), exporting data.

**Component Breakdown & Interactions:**

- **UI Elements:** View, `TextBlock`, `Button`, `DataGrid`, Chart component. Export options via dropdown menu attached to Export button.
- **Desktop Optimizations:**
  - **Interactive Visualizations:** Drill-down and interactive charts
  - **Comparison Tools:** Compare multiple analysis results
  - **Advanced Export:** Export to Excel with formulas preserved
  - **Annotation Tools:** Add notes and comments to analysis results
- **User Interaction:** User navigates to a specific analysis result. Views tables and charts. Interacts with charts (hover, zoom). Clicks "Export" and selects format (`ExportFormat`).
- **ADHD Considerations:**
    - **Clear Summaries:** Highlight key findings or significant changes in ratios/trends.
    - **Intuitive Visualizations:** Charts should be well-labeled and easy to interpret. Use appropriate chart types.
    - **Data Context:** Provide context for ratios (e.g., what does a "good" Current Ratio look like?).
    - **Export Options:** Offer common export formats (CSV, PDF).

## 6. Debt Management BC Feature Slices

### 6.1. `DebtAccountListView` Slice

**User Story:**

- "As an individual or finance manager, I want to see a list of all my debt accounts (loans, credit cards, mortgages), their current balances, and upcoming payment due dates."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`DebtAccountsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Add Debt Account" button (`Button`), Summary Panel (Panel showing `GetTotalDebt`).
    - **Main Content:** `DataGrid` displaying `DebtAccount` records. Columns: Account Name/Number, `DebtAccountType`, Lender Name, `CurrentBalance`, Next Payment Due Date (`DueDate`), Next Payment Amount. Actions (Button with icon for View Details). (`ListDebtAccounts`, `GetUpcomingDebtPayments`)

- **Primary Action(s):** Viewing debt accounts and summaries, navigating to details, initiating adding a new debt account.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `Panel`, `TextBlock`, `DataGrid`, Button with icon.
- **Desktop Optimizations:**
  - **Debt Dashboard:** Visual overview of all debt accounts
  - **Payment Calendar:** Calendar view of upcoming payments
  - **What-If Calculator:** Tools for payoff strategies
  - **Interest Visualization:** Visual breakdown of interest vs. principal
- **User Interaction:** User views the list of debts and total debt. Clicks view icon for `DebtAccountDetailView`. Clicks "Add Debt Account" for `DebtAccountEntryForm`.
- **ADHD Considerations:**
    - **Prioritize Key Info:** Highlight Current Balance and Next Due Date/Amount.
    - **Clear Totals:** The total debt summary is important context.
    - **Visual Grouping:** Consider grouping by Debt Account Type (Loans, Credit Cards, etc.).

### 6.2. `DebtAccountEntryForm` Slice

**User Story:**

- "As an individual, I need to add a new debt account, specifying its type, lender, principal, interest rate, term, and payment schedule."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`DebtAccountEntryView.axaml`).
- **Organization:** Form layout containing:
    - `DebtAccountType` (`ComboBox` Enum).
    - Account Name/Number (`TextBox`).
    - `LenderName` (`TextBox`).
    - `OriginalPrincipal` (`NumericUpDown` with `Currency`).
    - `InterestRateInfo` (Rate `NumericUpDown`, Type `ComboBox`).
    - `Term` (`NumericUpDown` + `ComboBox` for units years/months).
    - `PaymentSchedule` (`ComboBox` frequency, `NumericUpDown` amount, `DatePicker` first due date).
    - Current Balance (if different from principal `NumericUpDown`).
    - "Save Account" button (`Button`).

- **Primary Action(s):** Adding a new debt account (`AddDebtAccount`).

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Form layout, `ComboBox`, `TextBox`, `NumericUpDown`, `DatePicker`, `Button`.
- **Desktop Optimizations:**
  - **Loan Calculator:** Built-in calculator for payment estimation
  - **Amortization Preview:** Show preliminary payment schedule
  - **Interest Visualization:** Show interest breakdown over loan life
  - **Import Options:** Import from financial institution statements
- **User Interaction:** User fills in the details of the debt instrument. Saves the account.
- **ADHD Considerations:**
    - **Guided Input:** Break down complex loan terms into clear fields.
    - **Clear Labels:** Use standard financial terms but provide tooltips if necessary.
    - **Required Fields:** Indicate mandatory information.

### 6.3. `DebtAccountDetailView` Slice

**User Story:**

- "As an individual, I want to view the details of a specific debt account, including its payment history, accrued interest, amortization schedule (if applicable), and record payments made."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`DebtAccountDetailView.axaml`).
- **Organization:** `TabControl` for organization:
    - **Overview Tab:** Key details (Type, Lender, Rate, Balance, Next Due). "Record Payment" button (`Button` -> opens dialog).
    - **Payment History Tab:** `DataGrid` showing `DebtPaymentRecord`s (Date, Amount Paid, Principal, Interest). (`GetDebtAccountDetails`)
    - **Amortization Tab:** `DataGrid` displaying the `AmortizationSchedule` (Payment #, Date, Payment Amount, Principal, Interest, Remaining Balance). "Generate/Refresh Schedule" button (`GenerateAmortizationSchedule`). (`GetAmortizationSchedule`)
    - **Details Tab:** All other fields (`OriginalPrincipal`, `Term`, `InterestAccrual` logs, `LateFeeApplication` logs). Edit button (`UpdateDebtAccountDetails`).

- **Primary Action(s):** Viewing debt details, recording payments (`RecordDebtPayment`), viewing payment history, viewing/generating amortization schedule, editing account details.

**Component Breakdown & Interactions:**

- **UI Elements:** View, `TabControl`, `TabItem`, `TextBlock`, `Button`, `DataGrid`, Button with icon. Payment recording dialog (Form layout, `NumericUpDown`, `DatePicker`).
- **Desktop Optimizations:**
  - **Payment Calculator:** Tools for calculating extra payments
  - **What-If Scenarios:** Model different payment strategies
  - **Chart Visualization:** Visual representation of payoff timeline
  - **Export Options:** Export amortization schedule to Excel
- **User Interaction:** User navigates tabs to view different aspects. Clicks "Record Payment" to log a payment. Views amortization schedule. Edits details if necessary.
- **ADHD Considerations:**
    - **Tab Organization:** Tabs help break down the information.
    - **Key Info First:** Show the most important info (balance, next payment) on the Overview tab.
    - **Clear Tables:** Ensure payment history and amortization tables are easy to read.
    - **Simple Payment Logging:** Make recording a payment quick and easy.

## 7. Tax Management BC Feature Slices

### 7.1. `TaxLiabilityView` Slice

**User Story:**

- "As a finance manager or individual, I want to view recorded tax liabilities, their status, due dates, calculated amounts, and associated documents."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`TaxLiabilitiesView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Record Liability" button (`Button`), Filter by `TaxType` (`ComboBox`), Filter by `JurisdictionInfo` (`ComboBox`), Filter by `TaxPeriod` (`DatePicker` pair).
    - **Main Content:** `DataGrid` displaying `TaxLiability` records. Columns: `TaxType`, `JurisdictionInfo`, `TaxPeriod`, Due Date, `CalculatedAmount`, `PaidAmount`, Status (e.g., Due, Paid, Overdue - derived), Document (`Hyperlink` to `TaxDocumentID`), Actions (Button with icon for Record Payment, View Report). (`GetTaxLiabilityStatus`)

- **Primary Action(s):** Viewing tax liabilities, filtering, initiating liability recording, recording payments, viewing reports/documents.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `DatePicker`, `DataGrid`, `Hyperlink`, Button with icon. Liability recording via `Window` (Form layout, `ComboBox` Type/Jurisdiction, `DatePicker` pair Period, `NumericUpDown` Amount, `DatePicker` Due). Payment recording via `Window`.
- **Desktop Optimizations:**
  - **Tax Calendar:** Visual calendar of tax deadlines
  - **Document Management:** Integrated document storage and viewing
  - **Jurisdiction Rules:** Built-in tax rules and calculation assistance
  - **Filing Status Tracking:** Track preparation and filing status
- **User Interaction:** User views liabilities, applies filters. Clicks "Record Liability" to add a new one. Clicks "Record Payment" (`RecordTaxPayment`). Clicks document link (`GetTaxReport`, or link to Document Management).
- **ADHD Considerations:**
    - **Due Date Prominence:** Clearly highlight upcoming due dates. Use status indicators (color) for overdue items.
    - **Clear Identification:** Make Tax Type, Jurisdiction, and Period easy to identify.
    - **Document Access:** Easy access to associated tax forms/reports is important.

## 8. Audit Trail BC Feature Slices

### 8.1. `AuditTrailView` Slice

**User Story:**

- "As an auditor or administrator, I need to search and view the audit trail of financial data changes, filtering by user, date range, context (e.g., AP, GL), or specific record, so I can track activity and investigate issues."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`AuditTrailView.axaml`).
- **Organization:**
    - **`ToolBar:`** Extensive filtering options:
        - Date range (`DatePicker` pair).
        - User (`AutoCompleteBox` searching users).
        - `SourceContext` (`ComboBox`: GL, AP, AR, etc.).
        - `Action` (`ComboBox`: Create, Update, Delete).
        - `AggregateId` (`TextBox`).
        - Search Term (for `ChangeDetails` `TextBox`).
        - "Apply Filters" button (`Button`).
    - **Main Content:** `DataGrid` displaying `AuditEntry` records matching filters. Columns: `Timestamp`, `UserId`, `SourceContext`, `AggregateId`, `Action`, `ChangeDetails` (Field, Old Value, New Value - potentially truncated with expand option). (`SearchAuditTrail`)

- **Primary Action(s):** Searching and viewing audit log entries.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `DatePicker`, `AutoCompleteBox`, `ComboBox`, `TextBox`, `Button`, `DataGrid`, Button with icon (Expand details).
- **Desktop Optimizations:**
  - **Advanced Search:** Complex search condition builder
  - **Filtering Profiles:** Save and apply search configurations
  - **Change Highlighting:** Visually emphasize changed values
  - **Export Capabilities:** Export audit data for external review
- **User Interaction:** User sets various filter criteria. Clicks "Apply Filters". Views the resulting log entries. May expand `ChangeDetails` for more information.
- **ADHD Considerations:**
    - **Powerful Filtering:** The key here is effective filtering to narrow down potentially vast amounts of data. Make filter controls clear and easy to use.
    - **Clear Log Format:** The table needs to be very clear, especially the `ChangeDetails`. Show old/new values distinctly.
    - **Performance:** Ensure searching and filtering are reasonably fast, even with large datasets. Use virtualized loading.
    - **Readability:** Use monospace fonts for IDs or changed values if helpful.

## 9. Currency Management BC Feature Slices

### 9.1. `CurrencyRateView` Slice

**User Story:**

- "As a finance manager or administrator, I want to view the currently configured currencies and their exchange rates, and potentially update rates manually or trigger an update from an external provider."

**Overall Layout & Structure:**

- **Container:** Settings section or dedicated view (`CurrencySettingsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Update Latest Rates" button (`Button` - triggers `WorkspaceLatestRates` via `ExchangeRateProvider`), Base Currency display (`TextBlock`).
    - **Main Content:** `DataGrid` displaying `CurrencyRate`s from the latest `ExchangeRateSet`. Columns: `Currency` Code (From), `Currency` Code (To - Base), `RateValue`, `EffectiveDateTime`. (`GetExchangeRate` - implicitly used to show latest). "Edit Rate" button (Button with icon - `UpdateExchangeRate`).

- **Primary Action(s):** Viewing current exchange rates, triggering rate updates, manually editing specific rates.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `TextBlock`, `DataGrid`, Button with icon. Rate editing via `Window` (`NumericUpDown`).
- **Desktop Optimizations:**
  - **Rate History:** Track historical rate changes
  - **Rate Sources:** Configure and manage rate providers
  - **Rate Alerts:** Set up notifications for significant changes
  - **Currency Calculator:** Built-in conversion tools
- **User Interaction:** User views the list of rates against the base currency. Clicks "Update Latest Rates" to fetch new ones. Clicks edit icon to manually override a specific rate (with warnings).
- **ADHD Considerations:**
    - **Clear Base Currency:** Make the base currency obvious.
    - **Effective Date:** Clearly show the timestamp for the displayed rates.
    - **Manual Override Caution:** Clearly indicate manually overridden rates and provide warnings about potential inaccuracies.
    - **Simplicity:** Keep the table focused on the rates themselves.


## 10. Cash Management BC Feature Slices

### 10.1. `CashAccountListView` Slice

**User Story:**

- "As a finance manager, I want to view all cash accounts (bank accounts, cash drawers, petty cash), see their current balances, and quickly access transaction details."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`CashAccountsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Add Account" button (`Button`), "Transfer Funds" button (`Button`), Filter by `AccountType` (`ComboBox`).
    - **Main Content:** `DataGrid` displaying `CashAccount` records. Columns: Account Name, Account Number, `AccountType`, `CurrentBalance`, Last Transaction Date, Actions (View Transactions, Edit). (`ListCashAccounts`)
    - **Summary Panel:** Panel showing total cash position across all accounts. (`GetTotalCashPosition`)

- **Primary Action(s):** Viewing cash accounts, initiating account creation, initiating fund transfers, viewing transactions.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `DataGrid`, `Panel`, `TextBlock`.
- **Desktop Optimizations:**
  - **Balance Trend:** Mini sparkline charts showing recent balance trends
  - **Account Grouping:** Group by institution or account type
  - **Quick Actions:** Contextual menu for common account operations
  - **Drag & Drop:** Allow fund transfers via drag and drop between accounts
- **User Interaction:** User views list of accounts and balances. Clicks view icon to see `CashTransactionListView` for a specific account. Clicks "Add Account" or "Transfer Funds" to open relevant forms.
- **ADHD Considerations:**
    - **Clear Balances:** Make current balances prominent and easy to read.
    - **Visual Account Types:** Use icons to distinguish account types (checking, savings, etc.).
    - **Total Visibility:** Keep total cash position clearly visible.

### 10.2. `CashTransactionListView` Slice

**User Story:**

- "As a finance manager or accountant, I want to view a list of transactions for a specific cash account, filter by date or type, and reconcile them with statements."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`CashTransactionsView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Record Transaction" button (`Button`), Date range selector (`DatePicker` pair), Transaction Type filter (`ComboBox`), Search field (`TextBox`).
    - **Header Panel:** Account details (Name, Number, Current Balance).
    - **Main Content:** `DataGrid` displaying `CashTransaction` records. Columns: Date, Description, Reference, Amount (credit/debit visually distinguished), Balance, Reconciled status (with checkbox). (`GetCashTransactions`)
    - **Running Balance:** Calculated balance visible at bottom, potentially with variance from statement.

- **Primary Action(s):** Viewing transactions, filtering, recording new transactions, reconciling.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `DatePicker`, `ComboBox`, `TextBox`, `Panel`, `DataGrid`, `CheckBox`.
- **Desktop Optimizations:**
  - **Inline Editing:** Edit transaction details directly in grid
  - **Split View:** Side-by-side view with bank statement
  - **Categorization Tools:** Quick assignment of categories
  - **Reconciliation Mode:** Special interface for bank reconciliation
- **User Interaction:** User navigates transactions, applies filters. Checks "Reconciled" boxes to mark transactions. Clicks "Record Transaction" to open entry form.
- **ADHD Considerations:**
    - **Clear Credits/Debits:** Visually distinguish between deposits and withdrawals.
    - **Running Balance:** Show running balance for context.
    - **Reconciliation Status:** Clear visual indication of reconciled vs. unreconciled items.

### 10.3. `CashFlowForecastView` Slice

**User Story:**

- "As a finance manager, I want to see a forecast of cash inflows and outflows based on scheduled transactions, recurring patterns, and historical data to anticipate cash positions."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`CashFlowForecastView.axaml`).
- **Organization:**
    - **`ToolBar:`** Forecast Period selector (`ComboBox`: 30/60/90 days), Account filter (`ComboBox`), "Regenerate Forecast" button (`Button`).
    - **Main Content:** Two-panel layout:
        - **Chart Area:** Line or area chart showing projected cash balance over time. (`GetCashFlowForecast`)
        - **Detail Grid:** `DataGrid` showing projected transactions by date. Columns: Date, Description, Amount, Running Balance, Certainty (%).
    - **Summary Cards:** Panel showing key metrics (Lowest Projected Balance, Date of Lowest Balance, End of Period Balance).

- **Primary Action(s):** Viewing cash flow projections, adjusting forecast parameters, drilling into forecast details.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `ComboBox`, `Button`, Chart component (Eremex chart control), `DataGrid`, `Panel`, `TextBlock`.
- **Desktop Optimizations:**
  - **Interactive Chart:** Click on chart points to see transaction details
  - **What-If Scenarios:** Model different payment/receipt scenarios
  - **Cash Gap Detection:** Alert to potential cash shortfalls
  - **Multiple Scenario Comparison:** Compare different forecast scenarios
- **User Interaction:** User selects forecast period and account filters. Views projection chart and details. Clicks chart points to see details of that date's transactions.
- **ADHD Considerations:**
    - **Visual Clarity:** Use color coding for periods of surplus vs. potential shortfall.
    - **Key Indicators:** Highlight critical points (low balances, large outflows).
    - **Certainty Indication:** Clearly communicate forecast certainty levels.

## 11. Document Management BC Feature Slices

### 11.1. `DocumentLibraryView` Slice

**User Story:**

- "As a finance user, I want to access all financial documents (receipts, invoices, statements, contracts) in a centralized library, with filtering and search capabilities."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`DocumentLibraryView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Upload Document" button (`Button`), Document Type filter (`ComboBox`), Date range filter (`DatePicker` pair), Search field (`TextBox`), View type toggle (List/Grid).
    - **Main Content:** 
        - **List View:** `DataGrid` showing documents. Columns: Name, Type, Date, Size, Related Entity (e.g., Invoice #), Tags, Actions.
        - **Grid View:** `ItemsControl` showing document cards with thumbnails.
    - **Tag Panel:** Side panel with selectable document tags for filtering.

- **Primary Action(s):** Browsing documents, filtering, searching, uploading new documents.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `DatePicker`, `TextBox`, `ToggleButton`, `DataGrid`, `ItemsControl`, `Panel`.
- **Desktop Optimizations:**
  - **Thumbnail Generation:** Preview thumbnails for documents
  - **Drag & Drop Upload:** Direct file drag from file explorer
  - **Virtual Folders:** Create custom document organization
  - **Advanced Search:** Full-text document content search
- **User Interaction:** User browses documents, applies filters. Double-clicks document to open in `DocumentViewerView`. Clicks "Upload Document" to add new files.
- **ADHD Considerations:**
    - **Clear Organization:** Provide multiple ways to find documents (date, type, tags).
    - **Visual Previews:** Thumbnails help with recognition.
    - **Quick Access:** Recently accessed documents section.

### 11.2. `DocumentViewerView` Slice

**User Story:**

- "As a finance user, I need to view document details and content, potentially annotate them, and see any system links (to invoices, expenses, etc.)."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`DocumentViewerView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Download" button (`Button`), "Print" button (`Button`), "Add Annotation" button (`Button`), "Edit Properties" button (`Button`).
    - **Main Content:** Split layout:
        - **Document Viewer:** PDF/image viewer component with zoom controls.
        - **Document Properties Panel:** Side panel showing document metadata, tags, and system links.
    - **Annotations Panel:** (Optional) Shows list of annotations with timestamps and users.

- **Primary Action(s):** Viewing documents, adding annotations, editing properties, downloading.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, Document viewer component, `Panel`, `TextBlock`, `ItemsControl` (for annotations).
- **Desktop Optimizations:**
  - **Document Comparison:** View multiple documents side-by-side
  - **Advanced Annotation:** Drawing, text, and shape annotations
  - **OCR Integration:** Extract and search text from images
  - **Document History:** Track document versions and changes
- **User Interaction:** User views document, zooms/pans as needed. Adds annotations by selecting text/regions and clicking "Add Annotation". Edits properties in the side panel.
- **ADHD Considerations:**
    - **Clear Controls:** Document viewer controls should be intuitive.
    - **Properties Access:** Important metadata should be easily visible.
    - **Navigation Ease:** Easy navigation between multiple pages.

### 11.3. `DocumentUploadForm` Slice

**User Story:**

- "As a finance user, I need to upload new documents, add metadata and tags, and optionally link them to financial records (invoices, bills, etc.)."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`DocumentUploadView.axaml`).
- **Organization:** Form layout containing:
    - File selection (Button with `OpenFileDialog` or drag-drop area).
    - `DocumentType` (`ComboBox`).
    - Document Name (`TextBox` - autofilled from filename but editable).
    - Document Date (`DatePicker`).
    - Description/Notes (`TextBox`).
    - Tags (Tokenized input for adding multiple tags).
    - Related Entity selector (e.g., Invoice # - `AutoCompleteBox`).
    - "Upload" button (`Button`).

- **Primary Action(s):** Selecting files, adding metadata, associating with records, uploading.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Form layout, `Button` with `OpenFileDialog`, `ComboBox`, `TextBox`, `DatePicker`, Tag input component, `AutoCompleteBox`.
- **Desktop Optimizations:**
  - **Batch Upload:** Process multiple files at once
  - **Scanner Integration:** Direct scanner capture
  - **OCR Processing:** Extract metadata from document content
  - **Auto-Categorization:** Suggest document type based on content
- **User Interaction:** User selects file(s), adds metadata. System may suggest some fields based on content analysis. User associates with financial records if relevant.
- **ADHD Considerations:**
    - **Simplified Process:** Make the upload process as streamlined as possible.
    - **Auto-Population:** Use OCR and filename parsing to auto-suggest metadata.
    - **Clear Feedback:** Show upload progress and confirmation.

Okay, here are the UI slice breakdowns for the Credit Score Monitoring and Investment Management Bounded Contexts, following the pattern from the `FinancialManagement.md` file.

---

## 10. Credit Score Monitoring BC Feature Slices

### 10.1. `CreditScoreDashboardView` Slice

**User Story:**

* "As a user concerned about my credit, I want to see my latest credit scores from different bureaus, recent changes, active alerts, and the overall monitoring status at a glance, so I can quickly assess my credit health."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`CreditScoreDashboardView.axaml`).
* **Organization:**
    * **`ToolBar:`** "Refresh Scores" button (`Button` -> `RefreshCreditScores`), Monitoring Status indicator (`TextBlock`/Icon showing `MonitoringStatus`), "Configure Alerts" button (`Button` -> `AlertRuleConfigurationView`).
    * **Main Content:** Grid layout of widgets/cards:
        * **Score Widgets:** Separate cards for each `BureauType` (Experian, Equifax, TransUnion) showing latest `ScoreValue` (`GetCreditScore`), `ScoreModel`, and `ReportDate`. Possibly an average score widget. Trend indicator (up/down arrow) based on `GetScoreHistory`.
        * **Recent Alerts Widget:** `ItemsControl` listing the top 3-5 recent `CreditAlertTriggered` events (`ListCreditAlerts`). Each item shows `AlertType` and date, potentially clickable to `CreditAlertListView`.
        * **Credit Factors Summary:** Panel highlighting key positive/negative factors influencing the score (`GetCreditFactors`).
        * **Monitoring Status Panel:** Displays `MonitoringStatus` and links to manage `BureauConnection`s (`BureauConnectionSetupView`).
* **Primary Action(s):** Viewing current scores and history trends, seeing active alerts, refreshing scores (`RefreshCreditScores`), accessing alert configuration (`AlertRuleConfigurationView`) and bureau connections (`BureauConnectionSetupView`).

**Component Breakdown & Interactions:**

* **UI Elements:** `ToolBar`, `Button`, `TextBlock`, Icons, `Grid`, Card/`Border` components, `ItemsControl`, `Hyperlink`. Uses `GetCreditScore`, `GetScoreHistory`, `ListCreditAlerts`, `GetCreditFactors`, `GetBureauConnectionStatus`.
* **Desktop Optimizations:**
    * **Score Trend Charts:** Small sparkline charts within score widgets showing recent history.
    * **Customizable Layout:** Allow users to reorder or hide widgets.
    * **Notifications Integration:** Link alerts directly to the `NotificationCenter`.
    * **Quick Refresh:** Background score refresh options.
* **User Interaction:** User views the dashboard for a quick overview. Clicks "Refresh Scores" to manually trigger an update. Clicks on alerts or score details for more information. Navigates to configure alerts or manage connections.
* **ADHD Considerations:**
    * **Visual Clarity:** Use clear fonts, color-coding (e.g., red for score drops/alerts), and icons for quick comprehension.
    * **Key Info First:** Scores and critical alerts should be most prominent.
    * **Minimize Clutter:** Keep widgets concise. Provide links for deeper dives rather than overwhelming the dashboard.
    * **Actionable Alerts:** Make it obvious what alerts require attention.

### 10.2. `CreditReportDetailView` Slice

**User Story:**

* "As a user reviewing my credit, I want to view the full details of a specific credit report snapshot from a bureau, including personal information, accounts, inquiries, and public records, so I can check for accuracy and understand my credit history."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`CreditReportDetailView.axaml`), potentially accessed from the dashboard or a report list.
* **Organization:** `TabControl` or scrolling view with clear sections:
    * **Header:** Bureau (`BureauType`), `ReportDate`, Overall `ScoreValue` at that time. "Download/Print Report" button.
    * **Personal Information:** Section displaying recorded name, addresses, employers.
    * **Credit Score History:** Chart/Table showing `ScoreValue` over time for this bureau (`GetScoreHistory`).
    * **Account Summary:** Overview of total accounts, balances, utilization.
    * **Credit Account Records:** `DataGrid` listing `CreditAccountRecord`s. Columns: Creditor Name, Account Number (masked), `AccountType`, `AccountStatus`, Open Date, `CreditLimit`, `AccountBalance`, Payment History (visual). Expandable rows for more detail.
    * **Credit Inquiries:** `DataGrid` listing `CreditInquiry` records. Columns: Inquiry Date, Creditor Name, `InquiryType` (Hard/Soft).
    * **Public Records:** Section listing any `PublicRecord`s (e.g., bankruptcies, liens).
* **Primary Action(s):** Viewing detailed credit report components (`GetCreditReportDetails`), checking for inaccuracies, understanding account history and inquiries.

**Component Breakdown & Interactions:**

* **UI Elements:** View, `TabControl`/`ScrollViewer`, `TextBlock`, `Button`, `DataGrid`, Chart component, `Expander` for details. Uses `GetCreditReportDetails`, `GetScoreHistory`.
* **Desktop Optimizations:**
    * **Search/Filter:** Allow searching within report sections (e.g., find specific account).
    * **Highlighting:** Options to highlight potential issues (e.g., late payments, high utilization).
    * **Comparison View:** Ability to compare two reports side-by-side (e.g., current vs. previous).
    * **Export:** Export report data to CSV/PDF.
* **User Interaction:** User navigates through report sections/tabs. Expands account details. Reviews inquiries and public records. Uses download/print functionality.
* **ADHD Considerations:**
    * **Sectioning:** Clear separation between report sections (Personal Info, Accounts, Inquiries, etc.) is crucial.
    * **Visual Summaries:** Use charts for score history and visual indicators (e.g., color-coded payment history) in tables.
    * **Highlighting:** Draw attention to potentially negative items like delinquencies or high balances.
    * **Clear Language:** Avoid overly technical jargon where possible, or provide tooltips.

### 10.3. `CreditAlertListView` Slice

**User Story:**

* "As a user monitoring my credit, I want to see a list of all triggered credit alerts, filter them by type or date, and acknowledge them, so I can stay informed about important changes."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`CreditAlertListView.axaml`), possibly accessed from the dashboard or notification center.
* **Organization:**
    * **`ToolBar:`** Filter by `AlertType` (`ComboBox`), Date range filter (`DatePicker` pair), Filter by status (Acknowledged/Unacknowledged `ToggleButton` group).
    * **Main Content:** `DataGrid` displaying `CreditAlertTriggered` events (`ListCreditAlerts`). Columns: Date/Time, `AlertType`, Description (e.g., "New account opened", "Score dropped 15 points"), Status (Acknowledged/New), Action (Button "Acknowledge" -> `AcknowledgeCreditAlert`).
* **Primary Action(s):** Viewing triggered alerts (`ListCreditAlerts`), filtering alerts, acknowledging alerts (`AcknowledgeCreditAlert`).

**Component Breakdown & Interactions:**

* **UI Elements:** `ToolBar`, `ComboBox`, `DatePicker`, `ToggleButton`, `DataGrid`, `Button`. Uses `ListCreditAlerts`, `AcknowledgeCreditAlert`.
* **Desktop Optimizations:**
    * **Grouping:** Group alerts by date or type.
    * **Direct Navigation:** Link alerts to the relevant section in the `CreditReportDetailView` where possible.
    * **Bulk Actions:** Allow acknowledging multiple alerts at once.
    * **Severity Indicators:** Use icons/colors to indicate alert severity.
* **User Interaction:** User views the list of alerts. Applies filters to narrow down the list. Clicks "Acknowledge" on alerts they have reviewed. Clicks alert description for more context (potentially navigating to report details).
* **ADHD Considerations:**
    * **Prioritize Unacknowledged:** Default view should likely show unacknowledged alerts first.
    * **Clear Descriptions:** Alert descriptions need to be concise and immediately understandable.
    * **Simple Acknowledgment:** Make acknowledging alerts a simple, one-click action.
    * **Visual Cues:** Use icons or colors effectively to differentiate alert types or severity.

### 10.4. `AlertRuleConfigurationView` Slice

**User Story:**

* "As a user proactively managing my credit, I want to define specific rules for when I should be alerted about changes to my credit profile, so I only get notifications relevant to my concerns."

**Overall Layout & Structure:**

* **Container:** Dedicated view or Modal `Window` (`AlertRuleConfigurationView.axaml`), accessed from dashboard or settings.
* **Organization:**
    * **`ToolBar:`** "Add New Rule" button (`Button`).
    * **Main Content:** `ItemsControl` displaying existing `AlertRule`s. Each rule shows:
        * Description (e.g., "Alert if score drops > 10 points").
        * `AlertType`, `AlertCondition`.
        * Enabled/Disabled toggle (`ToggleSwitch`).
        * Edit/Delete buttons (`Button` with icons).
    * **Rule Editor (Dialog/Panel triggered by Add/Edit):**
        * Select `AlertType` (`ComboBox`).
        * Define `AlertCondition` using relevant controls (e.g., `NumericUpDown` for score drop threshold, `ComboBox` for utilization percentage).
        * Enable/Disable toggle.
        * "Save Rule" button (`Button` -> `ConfigureAlertRule`).
* **Primary Action(s):** Viewing existing alert rules, creating/editing rules (`ConfigureAlertRule`), enabling/disabling rules.

**Component Breakdown & Interactions:**

* **UI Elements:** `ToolBar`, `Button`, `ItemsControl`, `TextBlock`, `ToggleSwitch`, Icons, `Window`/Panel, `ComboBox`, `NumericUpDown`. Uses `ConfigureAlertRule` command. (Implicit query needed to list existing rules).
* **Desktop Optimizations:**
    * **Rule Templates:** Provide common predefined alert rules users can enable.
    * **Clarity:** Use natural language descriptions for rules where possible.
    * **Validation:** Prevent conflicting or illogical rules.
    * **Test Rule:** Option to simulate an alert based on a rule.
* **User Interaction:** User views existing rules. Clicks "Add New Rule" or edits an existing one. Configures the type and condition in the editor form. Saves the rule. Enables/disables rules using toggles.
* **ADHD Considerations:**
    * **Simplicity:** Keep the rule creation process straightforward. Use clear language.
    * **Visual Toggles:** Use clear visual toggles for enabling/disabling rules.
    * **Defaults:** Offer sensible default rules.
    * **Avoid Overwhelm:** Don't present too many complex condition options initially; perhaps use an "Advanced" section.

### 10.5. `BureauConnectionSetupView` Slice

**User Story:**

* "As a user wanting comprehensive credit monitoring, I need to securely connect the application to my accounts at different credit bureaus (or third-party providers), so it can fetch my reports and scores."

**Overall Layout & Structure:**

* **Container:** Dedicated view or section within Settings (`BureauConnectionSetupView.axaml`).
* **Organization:**
    * **Header:** Explanation of the connection process and security measures.
    * **Connection List:** `ItemsControl` displaying configured `BureauConnection`s. Each item shows:
        * `BureauType` (or provider name).
        * `MonitoringStatus` (Active, Needs Attention, Error).
        * Last successful fetch date.
        * "Manage" button (`Button`).
    * **Add Connection Button:** "Add Bureau Connection" (`Button` -> triggers `AddBureauConnection` flow).
* **Primary Action(s):** Viewing connection statuses (`GetBureauConnectionStatus`), initiating adding a new connection (`AddBureauConnection`), managing existing connections (e.g., update credentials, remove).

**Component Breakdown & Interactions:**

* **UI Elements:** `TextBlock`, `ItemsControl`, Icons, `Button`. The `AddBureauConnection` flow might involve a wizard (`Window`) with steps for selecting provider, entering credentials (securely handled), and authorizing access via the `CreditDataFetcherService`. Uses `AddBureauConnection`, `GetBureauConnectionStatus`, `PauseMonitoring`, `ResumeMonitoring`.
* **Desktop Optimizations:**
    * **Connection Wizard:** Step-by-step guidance for adding connections.
    * **Secure Credential Handling:** Integration with OS credential managers or secure input fields.
    * **Status Indicators:** Clear visual cues for connection health.
    * **Troubleshooting:** Links or guides for resolving connection errors.
* **User Interaction:** User views existing connections. Clicks "Add" to launch the wizard for linking a new bureau/provider. Enters credentials and follows authorization steps. Manages existing connections (e.g., re-authenticate, pause/resume monitoring).
* **ADHD Considerations:**
    * **Guided Setup:** The connection wizard needs to be very clear and break down the process into manageable steps.
    * **Security Assurance:** Clearly communicate security measures to build trust.
    * **Clear Status:** Make the status of each connection immediately obvious (e.g., green for active, red for error).
    * **Error Handling:** Provide clear, actionable steps if a connection fails.

---

## 11. Investment Management BC Feature Slices

### 11.1. `InvestmentPortfolioOverviewView` Slice

**User Story:**

* "As an investor, I want a dashboard showing the total value of my investment portfolio, its overall performance over different timeframes, and my asset allocation breakdown, so I can quickly gauge my investment status."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`InvestmentPortfolioOverviewView.axaml`).
* **Organization:**
    * **`ToolBar:`** Date range selector for performance (`ComboBox`: YTD, 1yr, 3yr, Max), "Sync All Accounts" button (`Button` -> `SyncInvestmentData`), "Add Account" button (`Button` -> `BrokerageLinkSetupView` or `ManualAccountEntryForm`).
    * **Main Content:** Grid layout of widgets/cards:
        * **Total Value Widget:** Large display of total portfolio market value (sum across `ListInvestmentAccounts` -> `GetAccountSummary`). Trend indicator based on recent `PortfolioSnapshot`s.
        * **Performance Chart Widget:** Line chart showing portfolio value over selected timeframe, optionally compared against a `BenchmarkIndex`. (`GetPortfolioPerformance`).
        * **Performance Metrics Widget:** Cards showing key `ReturnPercentage`s for selected `PerformanceTimeframe`s (`GetPortfolioPerformance`).
        * **Asset Allocation Widget:** Pie chart or bar chart showing allocation across `SecurityType`s (`GetAssetAllocation`).
        * **Top Movers Widget:** List of holdings with significant gains/losses recently (derived from `MarketDataService` and `GetAccountHoldings`).
* **Primary Action(s):** Viewing overall portfolio value and performance (`GetPortfolioPerformance`), seeing asset allocation (`GetAssetAllocation`), initiating data sync (`SyncInvestmentData`), navigating to add accounts.

**Component Breakdown & Interactions:**

* **UI Elements:** `ToolBar`, `ComboBox`, `Button`, `Grid`, Card/`Border`, `TextBlock`, Chart components (Line, Pie/Bar), `ItemsControl`. Uses `ListInvestmentAccounts`, `GetAccountSummary`, `GetPortfolioPerformance`, `GetAssetAllocation`, `SyncInvestmentData`. Needs `MarketDataService` integration for movers.
* **Desktop Optimizations:**
    * **Interactive Charts:** Hover effects, zoom, click to drill down.
    * **Customizable Timeframes:** Allow custom date ranges for performance.
    * **Multiple Portfolios:** Support grouping accounts into different portfolios.
    * **Benchmark Comparison:** Easy selection and comparison against multiple benchmarks.
* **User Interaction:** User views the dashboard to get a high-level summary. Selects different timeframes to view performance. Initiates sync to update data. Clicks on chart elements or metrics for more details (e.g., navigating to `InvestmentAccountListView`).
* **ADHD Considerations:**
    * **Key Metrics Prominent:** Total value and key performance figures should be easy to see.
    * **Visualizations:** Charts are crucial for understanding performance and allocation quickly. Ensure they are clean and well-labeled.
    * **Color Coding:** Use colors effectively (e.g., green for gains, red for losses) but ensure accessibility.
    * **Keep it Focused:** Avoid overwhelming the user with too many metrics; allow customization.

### 11.2. `InvestmentAccountListView` Slice

**User Story:**

* "As an investor with multiple accounts, I want to see a list of all my investment accounts, their current market value, and basic type information, so I can easily navigate to specific account details."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`InvestmentAccountListView.axaml`), potentially accessed from the Overview.
* **Organization:**
    * **`ToolBar:`** "Add Account" button (`Button`), "Sync All Accounts" button (`Button`), Filter by `AccountType` (`ComboBox`), Filter by `BrokerageName` (`ComboBox`).
    * **Main Content:** `DataGrid` or `ItemsControl` (as cards) displaying `InvestmentAccount` records (`ListInvestmentAccounts`).
        * **Grid Columns:** Account Name (`Hyperlink` to `InvestmentAccountDetailView`), `BrokerageName`, `AccountNumber` (masked), `AccountType`, `MarketValue` (`GetAccountSummary`).
        * **Card View:** Each card shows Account Name, Brokerage, Type, Market Value, and maybe a mini-chart of recent value.
* **Primary Action(s):** Viewing list of accounts (`ListInvestmentAccounts`), seeing summary values (`GetAccountSummary`), filtering accounts, navigating to account details.

**Component Breakdown & Interactions:**

* **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `DataGrid`/`ItemsControl`, `Hyperlink`, Card/`Border`. Uses `ListInvestmentAccounts`, `GetAccountSummary`.
* **Desktop Optimizations:**
    * **Grouping:** Group accounts by brokerage or type.
    * **Custom Views:** Save different filtered/sorted views.
    * **Quick Sync:** Button on each account row/card to sync individually (`SyncInvestmentData` for specific account).
    * **Account Totals:** Show subtotals per group.
* **User Interaction:** User views the list of accounts. Applies filters. Clicks on an account name/card to navigate to `InvestmentAccountDetailView`. Uses sync buttons.
* **ADHD Considerations:**
    * **Clear Identification:** Make Account Name, Brokerage, and Value easy to scan.
    * **Visual Grouping:** Cards or grouping in the grid can help differentiate accounts visually.
    * **Minimize Data:** Show only essential summary data in the list view; details belong in the detail view.

### 11.3. `InvestmentAccountDetailView` Slice

**User Story:**

* "As an investor, I want to view the detailed holdings, transaction history, and performance specific to one of my investment accounts, so I can analyze its individual contribution and activity."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`InvestmentAccountDetailView.axaml`), navigated from `InvestmentAccountListView`.
* **Organization:** `TabControl` for organization:
    * **Header:** Account Name, Account Number (masked), `BrokerageName`, `AccountType`, Current Total `MarketValue` (`GetAccountSummary`). "Sync Account" button (`SyncInvestmentData`).
    * **Holdings Tab:** `DataGrid` displaying `Holding` records (`GetAccountHoldings`). Columns: `AssetName`/`TickerSymbol`, `SecurityType`, `Quantity`, `SharePrice` (current), `MarketValue`, `CostBasis`, Gain/Loss ($, %), Day Change.
    * **Transactions Tab:** `DataGrid` displaying `InvestmentTransaction` records (`GetTransactionHistory`). Columns: `TransactionDate`, `TransactionType` (Buy/Sell/Div, etc.), `AssetName`/`TickerSymbol`, `Quantity`, Price, Total Amount.
    * **Performance Tab:** Chart and metrics showing performance specifically for this account over selected timeframes (`GetPortfolioPerformance` filtered for this account).
    * **Asset Allocation Tab:** Chart showing asset allocation specifically for this account (`GetAssetAllocation` filtered).
* **Primary Action(s):** Viewing holdings (`GetAccountHoldings`), transaction history (`GetTransactionHistory`), account-specific performance and allocation (`GetPortfolioPerformance`, `GetAssetAllocation`), initiating sync for this account (`SyncInvestmentData`).

**Component Breakdown & Interactions:**

* **UI Elements:** View, `TabControl`, `TabItem`, `TextBlock`, `Button`, `DataGrid`, Chart components. Uses `GetAccountSummary`, `GetAccountHoldings`, `GetTransactionHistory`, `GetPortfolioPerformance`, `GetAssetAllocation`, `SyncInvestmentData`. Requires `MarketDataService` for current prices.
* **Desktop Optimizations:**
    * **Inline Editing:** Allow editing `CostBasis` or other manual fields directly in the Holdings grid (triggers `UpdateHoldingDetails`).
    * **Filtering/Sorting:** Advanced filtering within Holdings and Transactions grids.
    * **Export:** Export holdings or transactions to CSV/Excel.
    * **Lot Level Details:** Option to expand holdings to show individual purchase lots.
* **User Interaction:** User navigates through tabs to view holdings, transactions, and performance. Sorts/filters data within grids. May edit cost basis. Initiates sync. Clicks on a holding ticker to potentially see `HoldingDetailView` or `SecurityMaster` info.
* **ADHD Considerations:**
    * **Tabbed Interface:** Breaks down complex information into manageable sections.
    * **Clear Grids:** Ensure tables are easy to read with good spacing and clear column headers. Use alternating row colors.
    * **Visual Performance:** Charts are key for understanding account performance quickly.
    * **Highlighting:** Draw attention to significant gains/losses or recent transactions.

### 11.4. `TransactionListView` Slice (Consolidated)

**User Story:**

* "As an investor tracking activity, I want to see a consolidated list of all investment transactions across all my accounts, filterable by date, type, account, or security, so I can easily find specific trades or income events."

**Overall Layout & Structure:**

* **Container:** Dedicated view (`TransactionListView.axaml`).
* **Organization:**
    * **`ToolBar:`** Date range filter (`DatePicker` pair), Filter by `TransactionType` (`ComboBox`), Filter by Account (`ComboBox` multi-select), Filter by Security (`AutoCompleteBox` searching `SecurityMaster`), "Record Manual Transaction" button (`Button` -> `ManualTransactionEntryForm`).
    * **Main Content:** `DataGrid` displaying `InvestmentTransaction` records (`GetTransactionHistory` potentially aggregated across accounts). Columns: `TransactionDate`, Account Name, `TransactionType`, `AssetName`/`TickerSymbol`, `Quantity`, Price, Total Amount.
* **Primary Action(s):** Viewing consolidated transaction history (`GetTransactionHistory`), filtering transactions extensively, initiating manual transaction entry.

**Component Breakdown & Interactions:**

* **UI Elements:** `ToolBar`, `DatePicker`, `ComboBox`, `AutoCompleteBox`, `Button`, `DataGrid`. Uses `GetTransactionHistory`, `RecordManualTransaction`.
* **Desktop Optimizations:**
    * **Advanced Filtering:** Build complex filter criteria.
    * **Grouping:** Group transactions by date, account, or type.
    * **Export:** Export filtered transaction data.
    * **Tax Lot Matching:** (Advanced) Tools to view or adjust tax lot assignments for sales.
* **User Interaction:** User views the transaction list. Applies multiple filters to find specific transactions. Clicks "Record Manual Transaction" to open the form.
* **ADHD Considerations:**
    * **Powerful Filtering:** Effective filtering is key to managing potentially large lists of transactions. Make filter controls intuitive.
    * **Clear Grid:** Ensure the transaction grid is highly readable. Use visual cues for transaction types (e.g., color for Buy vs. Sell).
    * **Context:** Clearly show which account each transaction belongs to.

### 11.5. `BrokerageLinkSetupView` Slice

**User Story:**

* "As a user wanting automated investment tracking, I need a way to securely link my external brokerage accounts using a service like Plaid, so the application can automatically sync my holdings and transactions."

**Overall Layout & Structure:**

* **Container:** Dedicated view or wizard (`BrokerageLinkSetupView.axaml`), accessed from Overview or Account List.
* **Organization:** Similar to `BureauConnectionSetupView`:
    * **Header:** Explanation of the linking process and security (mentioning the `BrokerageDataSyncService` provider, e.g., Plaid).
    * **Link List:** `ItemsControl` displaying linked `BrokerageLink`s. Each shows:
        * `BrokerageName`.
        * Status (Linked, Syncing, Error, Needs Action).
        * Last Sync Date.
        * "Manage" button (`Button`).
    * **Add Link Button:** "Link Brokerage Account" (`Button` -> `LinkBrokerageAccount` flow).
* **Primary Action(s):** Viewing linked accounts and statuses, initiating the linking process (`LinkBrokerageAccount`), managing links (re-authenticate, remove).

**Component Breakdown & Interactions:**

* **UI Elements:** `TextBlock`, `ItemsControl`, Icons, `Button`. The `LinkBrokerageAccount` flow likely involves launching a web view or component provided by the `BrokerageDataSyncService` (e.g., Plaid Link) within a `Window`. Uses `LinkBrokerageAccount`, `SyncInvestmentData`. (Implicit query needed to list links).
* **Desktop Optimizations:**
    * **Integrated Flow:** Embed the provider's linking module smoothly within the application window.
    * **Clear Status Updates:** Provide real-time feedback during the linking and initial sync process.
    * **Multi-Account Handling:** Support linking multiple accounts from a single brokerage login.
    * **Troubleshooting:** Clear guidance for common linking errors.
* **User Interaction:** User clicks "Link Brokerage Account". Follows the steps within the provider's interface (selecting institution, entering credentials, MFA). Application receives confirmation and initiates first sync. User manages existing links via the list.
* **ADHD Considerations:**
    * **Guided Process:** Break down the linking process into clear, simple steps within the provider's UI.
    * **Trust and Security:** Clearly explain data handling and security.
    * **Status Visibility:** Make the status of each link clear (e.g., "Syncing...", "Linked", "Action Required").
    * **Minimize Friction:** Make the process as seamless as possible.

### 11.6. `ManualTransactionEntryForm` Slice

**User Story:**

* "As an investor with accounts not linked automatically or needing to record specific events, I need a form to manually enter investment transactions like buys, sells, dividends, or transfers."

**Overall Layout & Structure:**

* **Container:** Modal `Window` or dedicated view (`ManualTransactionEntryView.axaml`).
* **Organization:** Form layout:
    * Target `InvestmentAccount` (`ComboBox` listing manual/linked accounts).
    * `TransactionDate` (`DatePicker`).
    * `TransactionType` (`ComboBox`: Buy, Sell, Dividend, etc.).
    * Security (`AutoCompleteBox` searching `SecurityMaster` - `TickerSymbol`/`AssetName`). Allow creating new basic security entry if not found.
    * `Quantity` (`NumericUpDown`).
    * Price per Share (`NumericUpDown` with `Currency`).
    * Commission/Fee (`NumericUpDown` with `Currency` - optional).
    * Total Amount (Calculated `TextBlock`, or input for dividends/transfers).
    * Notes (`TextBox`).
    * "Save Transaction" button (`Button` -> `RecordManualTransaction`).
* **Primary Action(s):** Entering details for and saving a manual investment transaction (`RecordManualTransaction`).

**Component Breakdown & Interactions:**

* **UI Elements:** `Window`/View, Form layout, `ComboBox`, `DatePicker`, `AutoCompleteBox`, `NumericUpDown`, `TextBlock`, `TextBox`, `Button`. Uses `RecordManualTransaction`, requires `ListInvestmentAccounts`, `GetSecurityDetails`.
* **Desktop Optimizations:**
    * **Security Lookup:** Fast search and selection for securities.
    * **Calculation:** Auto-calculate total amounts based on quantity and price.
    * **Validation:** Ensure required fields are filled correctly based on transaction type.
    * **Recurring Transactions:** (Advanced) Option to set up recurring manual entries (e.g., regular investment).
* **User Interaction:** User selects account, date, type. Enters security, quantity, price details. Adds optional notes. Saves the transaction.
* **ADHD Considerations:**
    * **Clear Form:** Structure the form logically. Guide the user based on selected `TransactionType` (e.g., hide price field for simple transfers).
    * **Required Fields:** Clearly indicate mandatory fields.
    * **Security Selection:** Make finding the correct security easy (autocomplete is key).
    * **Feedback:** Confirm successful saving of the transaction.

---

## 12. Reporting BC Feature Slices

### 12.1. `ReportCatalogView` Slice

**User Story:**

- "As a finance manager or accountant, I want to browse available financial reports, run them with specific parameters, and access saved report results."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ReportCatalogView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Create Custom Report" button (`Button`), Category filter (`ComboBox`), Search field (`TextBox`).
    - **Main Content:** `TreeView` or categorized list of available reports.
        - Each report item shows: Report Name, Description, Last Run Date.
        - Action buttons: "Run Report" (`Button`), "View History" (`Button`).
    - **Recent Reports Panel:** `ItemsControl` showing recently run reports for quick access.

- **Primary Action(s):** Browsing available reports, initiating report runs, accessing report history.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `ComboBox`, `TextBox`, `TreeView`/`ItemsControl`, `Panel`.
- **Desktop Optimizations:**
  - **Report Categories:** Hierarchical organization of reports
  - **Report Favorites:** Bookmark commonly used reports
  - **Scheduled Reports:** Configure recurring report generation
  - **Report Templates:** Save parameter sets for reuse
- **User Interaction:** User browses reports by category. Clicks "Run Report" to open parameter selection (`ReportParameterForm`). Clicks "View History" to see previous runs.
- **ADHD Considerations:**
    - **Clear Categorization:** Organize reports logically by type/purpose.
    - **Description Access:** Make report descriptions clear and accessible.
    - **Quick Access:** Provide quick access to frequently used reports.

### 12.2. `ReportParameterForm` Slice

**User Story:**

- "As a report user, I need to configure parameters for a report (date ranges, accounts, entities) and specify the output format."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`ReportParameterView.axaml`).
- **Organization:** Form layout containing:
    - Report Name and Description (read-only).
    - Dynamic parameter fields based on report definition:
        - Date ranges (`DatePicker` pairs).
        - Entity selectors (`ComboBox`, `AutoCompleteBox`, `ListBox` for multi-select).
        - Grouping options (`ComboBox`).
        - Filter criteria (potentially complex filter builders).
    - Output Format selector (`ComboBox`: PDF, Excel, CSV).
    - "Save as Template" option (`CheckBox` + `TextBox` for name).
    - "Run Report" button (`Button`).

- **Primary Action(s):** Configuring report parameters, saving parameter templates, initiating report generation.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, Form layout with dynamic controls based on report type, `Button`, `ComboBox`, `CheckBox`, `TextBox`.
- **Desktop Optimizations:**
  - **Parameter Presets:** Save and reapply parameter configurations
  - **Live Preview:** Show sample output based on current parameters
  - **Advanced Filtering:** Complex filter creation interface
  - **Parameter Validation:** Contextual validation of parameter combinations
- **User Interaction:** User configures parameters specific to the selected report. Optionally saves configuration as a template. Clicks "Run Report" to generate the report.
- **ADHD Considerations:**
    - **Guided Configuration:** Group related parameters and provide clear explanations.
    - **Default Values:** Provide sensible defaults where possible.
    - **Visual Validation:** Clear indication of valid/invalid parameter combinations.

### 12.3. `ReportViewerView` Slice

**User Story:**

- "As a report user, I want to view a generated report, drill down into details, export it in different formats, and potentially schedule recurring generation."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ReportViewerView.axaml`).
- **Organization:**
    - **`ToolBar:`** "Export" button (`Button` with dropdown), "Print" button (`Button`), "Schedule" button (`Button`), "Refresh" button (`Button`).
    - **Parameter Summary:** Collapsible panel showing the parameters used to generate the report.
    - **Main Content:** Report viewer component displaying the rendered report.
        - Interactive elements for drilling down into data.
        - Page navigation controls for multi-page reports.
    - **Action Panel:** Side panel with additional options (save, share, annotate).

- **Primary Action(s):** Viewing reports, exporting, printing, drilling down into details, scheduling recurring generation.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `Button`, `Panel`, Report viewer component, `Expander` for parameter summary.
- **Desktop Optimizations:**
  - **Interactive Data:** Click values to drill down into details
  - **Advanced Export:** Control export formatting options
  - **Annotation Tools:** Add notes and highlights to reports
  - **Multi-Window Support:** Compare reports side-by-side
- **User Interaction:** User views the report, navigates through pages. Interacts with data points to drill down. Uses toolbar options to export, print, or schedule the report.
- **ADHD Considerations:**
    - **Clear Navigation:** Make page navigation intuitive.
    - **Data Context:** Maintain context when drilling down into data.
    - **Export Options:** Make exporting straightforward with appropriate format options.

## 13. Global Desktop Application Components

### 13.1. `ApplicationShell` Component

**User Story:**

- "As a finance application user, I need a cohesive application shell with consistent navigation, quick access to features, and workspace management."

**Overall Layout & Structure:**

- **Container:** Main window (`MainWindow.axaml`).
- **Organization:**
    - **`MainMenu:`** Application menu with File, Edit, View, Tools, Help sections.
    - **`ToolBar:`** Configurable toolbar with common actions.
    - **Navigation Panel:** Left sidebar with module navigation tree.
    - **Workspace Area:** Main content area (potentially tabbed for multiple open views).
    - **Status Bar:** Bottom bar with system information, user details, connectivity status.

- **Primary Action(s):** Navigating between modules, managing workspace, accessing application features.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`, `Menu`, `MenuItem`, `ToolBar`, `SplitView` (or similar for resizable panels), `TabControl` for workspace, `StatusBar`.
- **Desktop Optimizations:**
  - **Window State Management:** Remember size, position, layout
  - **Multiple Monitor Support:** Allow window detachment
  - **Keyboard Navigation:** Comprehensive keyboard shortcuts
  - **Workspace Configurations:** Save/restore workspace layouts
- **User Interaction:** User navigates application through menus and sidebar. Opens multiple views in tabs within workspace. Accesses tool and status information as needed.
- **ADHD Considerations:**
    - **Clean Organization:** Clear visual hierarchy prevents overwhelm.
    - **Consistent Navigation:** Predictable navigation patterns reduce cognitive load.
    - **Visual Focus:** Current module/area should be clearly highlighted.

### 13.2. `GlobalSearch` Component

**User Story:**

- "As a finance application user, I need a powerful search capability to quickly find financial records, transactions, documents, or application features."

**Overall Layout & Structure:**

- **Container:** Search box in toolbar, expandable to advanced search dialog.
- **Organization:**
    - **`SearchBox:`** `TextBox` with search icon, keyboard shortcut (Ctrl+F or similar).
    - **Results Dropdown:** Immediate results in categorized dropdown.
    - **Advanced Search Dialog:** Expanded view for complex search with filters and categories.
    - **Search Results View:** Full results page for comprehensive search results.

- **Primary Action(s):** Quick searching across application, viewing results, navigating to items.

**Component Breakdown & Interactions:**

- **UI Elements:** `TextBox`, `Popup` for quick results, `Window` for advanced search, categorized result components.
- **Desktop Optimizations:**
  - **Shortcut Activation:** Global keyboard shortcut (Ctrl+F)
  - **Search History:** Access to previous searches
  - **Type-Ahead:** Search as you type with suggestions
  - **Search Scoping:** Control search context and scope
- **User Interaction:** User activates search, enters terms. Quick results appear in dropdown. User can navigate to results or expand to full search view for complex queries.
- **ADHD Considerations:**
    - **Fast Access:** Search should be accessible from anywhere in the application.
    - **Immediate Feedback:** Start showing results as user types.
    - **Clear Categories:** Clearly categorize different types of search results.

### 13.3. `NotificationCenter` Component

**User Story:**

- "As a finance application user, I need to be alerted about important system events, process completions, or required actions."

**Overall Layout & Structure:**

- **Container:** Notification icon in toolbar, expanding to notification panel.
- **Organization:**
    - **`NotificationIcon:`** Icon with badge showing unread count.
    - **Notification Panel:** Slide-out panel listing recent notifications.
    - **Notification Toast:** Temporary pop-up for new notifications.
    - **Notification Settings:** Configuration for notification types and display.

- **Primary Action(s):** Viewing notifications, taking action on notifications, managing notification settings.

**Component Breakdown & Interactions:**

- **UI Elements:** Toolbar icon with badge, `Popup`/panel for notification list, toast notification component.
- **Desktop Optimizations:**
  - **System Integration:** Use native OS notifications when available
  - **Actionable Notifications:** Direct action buttons in notifications
  - **Notification Grouping:** Group related notifications
  - **Priority Levels:** Visual distinction for critical alerts
- **User Interaction:** User receives toast notifications for new alerts. Clicks notification icon to view all notifications. Can click notification to navigate to relevant area or dismiss with swipe/button.
- **ADHD Considerations:**
    - **Non-Intrusive:** Notifications should not disrupt workflow unless critical.
    - **Clear Priority:** Visually distinguish between critical and informational notifications.
    - **Action Context:** Make it clear what action is needed for each notification.

### 13.4. `UserPreferencesView` Slice

**User Story:**

- "As a finance application user, I want to customize the application appearance, behavior, and default settings to match my workflow."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or dedicated view (`UserPreferencesView.axaml`).
- **Organization:** `TabControl` with categories:
    - **Appearance Tab:** Theme selection, color scheme, font size, density.
    - **Behavior Tab:** Confirmation dialogs, default views, startup options.
    - **Workspace Tab:** Layout preferences, tab behavior, window management.
    - **Notifications Tab:** Configure notification types and delivery.
    - **Advanced Tab:** Performance options, logging, system integration.

- **Primary Action(s):** Configuring user preferences, saving configuration, applying changes.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/View, `TabControl`, `TabItem`, various setting controls (`ComboBox`, `CheckBox`, `Slider`, etc.).
- **Desktop Optimizations:**
  - **Profile Support:** Save/load preference profiles
  - **Real-time Preview:** Show changes as they're made
  - **Keyboard Shortcut Editor:** Customize application shortcuts
  - **Display Scaling:** Adapt to different DPI settings
- **User Interaction:** User navigates preference categories, adjusts settings. Changes may apply immediately or on save depending on setting type.
- **ADHD Considerations:**
    - **Organized Settings:** Group related settings logically.
    - **Default Presets:** Provide sensible defaults and presets.
    - **Visual Preview:** Show visual examples of appearance changes.

### 13.5. `ImportExportManager` Component

**User Story:**

- "As a finance application user, I need to import data from external sources (CSV, Excel) or export financial data for use in other systems."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`ImportExportView.axaml`).
- **Organization:** `TabControl` with Import and Export tabs:
    - **Import Tab:**
        - Data Type selector (`ComboBox`: Transactions, Accounts, etc.).
        - File selection (Button with `OpenFileDialog`).
        - Mapping configuration (source fields to system fields).
        - Validation panel showing issues and warnings.
        - "Import Data" button (`Button`).
    - **Export Tab:**
        - Data Type selector (`ComboBox`).
        - Filter/selection criteria for data to export.
        - Format options (`ComboBox`: CSV, Excel, PDF).
        - "Export Data" button (`Button`).

- **Primary Action(s):** Importing external data, mapping fields, validating data, exporting system data.

**Component Breakdown & Interactions:**

- **UI Elements:** `TabControl`, `ComboBox`, `Button` with `OpenFileDialog`, mapping configuration controls, validation display, export configuration controls.
- **Desktop Optimizations:**
  - **Field Mapping Memory:** Remember mappings for repeat imports
  - **Template Support:** Save import/export configurations
  - **Preview Data:** Show sample of data before processing
  - **Batch Processing:** Handle large datasets efficiently
- **User Interaction:** For import: User selects data type, chooses file, configures field mapping, validates data, confirms import. For export: User selects data type, configures filters, chooses format, initiates export.
- **ADHD Considerations:**
    - **Guided Process:** Break complex import/export into clear steps.
    - **Validation Feedback:** Clearly communicate data issues before processing.
    - **Progress Indication:** Show progress for long-running operations.

## 14. Dashboard BC Feature Slices

### 14.1. `FinancialDashboardView` Slice

**User Story:**

- "As a finance manager, I want a customizable dashboard showing key financial metrics, alerts, and visualizations to get a quick overview of financial health."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`FinancialDashboardView.axaml`).
- **Organization:**
    - **`ToolBar:`** Date range selector (`DatePicker` pair), "Customize Dashboard" button (`Button`), Dashboard preset selector (`ComboBox`).
    - **Main Content:** Grid layout of widgets/cards:
        - **KPI Widgets:** Cards showing key metrics with trends.
        - **Chart Widgets:** Various visualizations of financial data.
        - **Alert Widgets:** Notifications about exceptions or action items.
        - **Recent Activity Widgets:** Lists of recent transactions/events.
    - **Customization Panel:** (When in edit mode) Side panel with available widgets and configuration options.

- **Primary Action(s):** Viewing dashboard metrics, customizing layout, drilling into data.

**Component Breakdown & Interactions:**

- **UI Elements:** `ToolBar`, `DatePicker`, `Button`, `ComboBox`, `Grid` or custom dashboard container, various widget components.
- **Desktop Optimizations:**
  - **Drag & Drop Customization:** Rearrange widgets via drag and drop
  - **Widget Resize:** Adjust widget sizes to emphasize important metrics
  - **Multiple Dashboards:** Switch between role-specific dashboards
  - **Drill-Down Navigation:** Click metrics to see underlying data
- **User Interaction:** User views dashboard data, customizes layout by dragging/resizing widgets. Clicks metrics to drill down into source data.
- **ADHD Considerations:**
    - **Visual Clarity:** Use color and size to emphasize important metrics.
    - **Information Hierarchy:** Organize metrics by importance.
    - **Customization:** Allow users to focus on metrics most relevant to them.

### 14.2. `DashboardWidgetLibrary` Component

**User Story:**

- "As a dashboard user, I want to browse available dashboard widgets, preview them, and add them to my dashboard."

**Overall Layout & Structure:**

- **Container:** Modal `Window` or panel within dashboard customization mode.
- **Organization:**
    - **Category Navigation:** List or tabs of widget categories (Financial, Operational, Charts, Lists, etc.).
    - **Widget Grid:** Grid/list of available widgets with preview thumbnails.
    - **Widget Preview:** Hover or selection shows larger preview with description.
    - **Configuration Panel:** Options specific to the selected widget type.

- **Primary Action(s):** Browsing widgets, previewing, configuring, adding to dashboard.

**Component Breakdown & Interactions:**

- **UI Elements:** `Window`/Panel, category navigation control, `ItemsControl` for widget grid, preview component, configuration form.
- **Desktop Optimizations:**
  - **Widget Previews:** Interactive previews with sample data
  - **Custom Widgets:** Tools to create custom metric widgets
  - **Widget Templates:** Save configurations for reuse
  - **Widget Sharing:** Share configurations with other users
- **User Interaction:** User browses widget categories, selects widget type, configures options, adds to dashboard.
- **ADHD Considerations:**
    - **Visual Previews:** Clear visual examples help with selection.
    - **Categorization:** Organize widgets logically by type/purpose.
    - **Simple Configuration:** Make widget configuration straightforward.

## 15. Desktop Integration Features

### 15.1. `BackupRestoreTools` Component

**User Story:**

- "As a finance application administrator, I need to backup financial data, schedule automatic backups, and restore from backups when needed."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`BackupRestoreView.axaml`).
- **Organization:** `TabControl` with Backup and Restore tabs:
    - **Backup Tab:**
        - Manual backup section with location selector and "Backup Now" button.
        - Scheduled backup configuration (frequency, retention, location).
        - Backup history list showing previous backups.
    - **Restore Tab:**
        - Backup selection (from history or file selection).
        - Restore options (complete vs. selective).
        - Verification and warning panel.
        - "Restore Data" button.

- **Primary Action(s):** Creating backups, configuring automatic backups, restoring from backups.

**Component Breakdown & Interactions:**

- **UI Elements:** `TabControl`, location selector controls, `Button`, schedule configuration controls, `DataGrid` for backup history, restore options controls, verification panel.
- **Desktop Optimizations:**
  - **Direct File Access:** Integration with local file system
  - **Scheduled Tasks:** Use system scheduler for automated backups
  - **Differential Backups:** Optimize backup size and speed
  - **Validation Tools:** Verify backup integrity
- **User Interaction:** For backup: User configures backup location, triggers manual backup or sets up schedule. For restore: User selects backup to restore from, configures options, confirms restore.
- **ADHD Considerations:**
    - **Clear Process:** Make backup/restore steps explicit.
    - **Verification:** Confirm successful operations clearly.
    - **Warning Emphasis:** Clearly communicate risks of restore operations.

### 15.2. `SystemIntegrationManager` Component

**User Story:**

- "As a finance application administrator, I want to configure integrations with external systems (accounting software, banking systems, ERP) for data exchange."

**Overall Layout & Structure:**

- **Container:** Dedicated view (`SystemIntegrationView.axaml`).
- **Organization:**
    - **Integration List:** `DataGrid` or list showing configured integrations and status.
    - **Configuration Panel:** Form for setting up new integrations or editing existing ones.
    - **Connection Test:** Tools to test and verify integration connectivity.
    - **Sync History:** Log of synchronization events and errors.

- **Primary Action(s):** Configuring system integrations, testing connections, monitoring sync status.

**Component Breakdown & Interactions:**

- **UI Elements:** `DataGrid`/list for integrations, configuration form with connection settings, test tools, log viewer for sync history.
- **Desktop Optimizations:**
  - **Connection Wizards:** Step-by-step setup guides
  - **Credentials Management:** Secure storage of connection details
  - **Scheduled Syncs:** Configure automatic data exchange
  - **Error Recovery:** Tools to troubleshoot and fix sync issues
- **User Interaction:** User views existing integrations, configures new connections with system-specific settings, tests connectivity, monitors sync status and history.
- **ADHD Considerations:**
    - **Guided Setup:** Complex integrations should have clear setup steps.
    - **Status Clarity:** Connection status should be immediately visible.
    - **Error Handling:** Clear explanation of sync issues and resolution steps.

### 15.3. `FileAttachmentHandler` Component

**User Story:**

- "As a finance application user, I want to easily attach files from my local system to financial records and manage those attachments efficiently."

**Overall Layout & Structure:**

- **Component Integration:** Embedded within various forms that support attachments.
- **Organization:**
    - **Attachment List:** List of currently attached files with icons and actions.
    - **Add Button:** Button triggering file selection (`OpenFileDialog`).
    - **Drag-Drop Zone:** Area supporting direct file drag from file explorer.
    - **Attachment Preview:** Thumbnail or preview of selected attachment.

- **Primary Action(s):** Adding file attachments, viewing attachments, removing attachments.

**Component Breakdown & Interactions:**

- **UI Elements:** Attachment list component, `Button` with `OpenFileDialog`, drag-drop handler, preview component.
- **Desktop Optimizations:**
  - **Direct File System Access:** Seamless local file integration
  - **Thumbnail Generation:** Create previews of common file types
  - **Multi-File Selection:** Add multiple files at once
  - **Compression Options:** Optimize storage for large attachments
- **User Interaction:** User clicks "Add" or drags files directly to attach them. Can view thumbnails/previews or remove attachments as needed.
- **ADHD Considerations:**
    - **Simple Process:** Make attaching files straightforward and intuitive.
    - **Visual Confirmation:** Clearly show when files are successfully attached.
    - **Thumbnail Support:** Visual previews help with recognition.
