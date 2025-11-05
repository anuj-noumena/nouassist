# Software Requirements Specification
## NouAssist Financial Management System

**Version:** 1.0  
**Date:** June 14, 2025  
**Document ID:** SRS-NouAssist-FM-001

---

## Table of Contents

1. [Introduction](#1-introduction)
2. [Overall Description](#2-overall-description)
3. [System Features](#3-system-features)
4. [Non-Functional Requirements](#4-non-functional-requirements)
5. [Integration Requirements](#5-integration-requirements)
6. [Constraints](#6-constraints)
7. [Assumptions and Dependencies](#7-assumptions-and-dependencies)

---

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) defines the functional and non-functional requirements for the NouAssist Financial Management System. The document serves as the primary reference for system development, testing, and validation.

### 1.2 Scope
The NouAssist Financial Management System is a comprehensive financial management solution designed for individuals, small businesses, and organizations. The system provides end-to-end financial management capabilities including:

- Bills and invoice management
- Financial planning and budgeting
- Expense tracking and reporting
- Double-entry accounting
- Debt and loan management
- Tax management
- Financial analytics and reporting
- Multi-currency support
- Document management
- Audit trail and compliance support

### 1.3 Definitions and Acronyms

- **API**: Application Programming Interface
- **OCR**: Optical Character Recognition
- **CSV**: Comma-Separated Values
- **PDF**: Portable Document Format
- **SRS**: Software Requirements Specification
- **Payee**: An individual or organization to whom payment is made

### 1.4 References
- IEEE Std 830-1998 - IEEE Recommended Practice for Software Requirements Specifications
- Generally Accepted Accounting Principles (GAAP)
- International Financial Reporting Standards (IFRS)

---

## 2. Overall Description

### 2.1 Product Perspective
NouAssist Financial Management System is a standalone software solution that integrates with external banking APIs, email services, and tax calculation services. The system is designed with a modular architecture supporting scalability and customization.

### 2.2 Product Functions
The system provides the following major functional areas:

- **Bills Management**: Create, categorize, and track bill payments
- **Invoice Management**: Generate, send, and track invoices
- **Financial Planning and Tracking**: Budget creation, expense tracking, and financial goal management
- **Financial Reporting**: Generate standard financial statements and reports
- **Accounting Core**: Double-entry bookkeeping and chart of accounts management
- **Financial Analytics**: Advanced reporting and data visualization
- **Audit Support**: Comprehensive audit trails and reporting
- **Debt Management**: Track and manage various types of debt
- **Tax Management**: Tax calculation, reporting, and document storage
- **Multi-currency Support**: Handle multiple currencies with exchange rate management

### 2.3 User Classes and Characteristics

- **Individual Users**: Personal financial management
- **Small Business Owners**: Business financial management and compliance
- **Accountants**: Professional accounting and reporting
- **Financial Analysts**: Advanced analytics and reporting
- **Auditors**: Audit trail access and reporting

### 2.4 Operating Environment
- Web-based application accessible via modern browsers
- Mobile application support (iOS/Android)
- Desktop application support (Windows/macOS/Linux)
- Cloud-based deployment with on-premises options

---

## 3. System Features

### 3.1 Bills Management (NouAssist.FinancialManagement.BillsManagement)

#### 3.1.1 Create Bill Entry (FM-BM-001)
**Description**: Enable users to manually enter bills with OCR support for scanning physical bills.

**Functional Requirements**:
- The system shall allow manual bill entry with fields for amount, due date, payee, category, and description
- The system shall support OCR functionality to scan and extract data from physical bills
- The system shall validate bill data for completeness and accuracy
- The system shall save bill entries to the database

#### 3.1.2 Categorize Bills (FM-BM-002)
**Description**: Allow organization of bills by category, vendor (Payee), and project.

**Functional Requirements**:
- The system shall provide predefined bill categories
- The system shall allow users to create custom categories
- The system shall associate bills with specific payees
- The system shall support project-based bill organization

#### 3.1.3 Set Up Recurring Bills (FM-BM-003)
**Description**: Support configuration of recurring bill payments with custom schedules.

**Functional Requirements**:
- The system shall allow creation of recurring bill schedules (weekly, monthly, quarterly, annually)
- The system shall support custom recurring patterns
- The system shall automatically generate recurring bills based on configured schedules
- The system shall provide notifications for upcoming recurring bills

#### 3.1.4 Mark Bill as Paid (FM-BM-004)
**Description**: Allow marking of bills as paid.

**Functional Requirements**:
- The system shall allow users to mark bills as paid
- The system shall record payment date and amount
- The system shall update bill status and accounting records
- The system shall prevent duplicate payment marking

### 3.2 Invoice Management (NouAssist.FinancialManagement.InvoiceManagement)

#### 3.2.1 Generate Invoice (FM-IC-001)
**Description**: Create professional invoices using customizable templates.

**Functional Requirements**:
- The system shall provide multiple invoice templates
- The system shall allow template customization with company branding
- The system shall auto-generate invoice numbers
- The system shall calculate totals, taxes, and discounts automatically

#### 3.2.2 Manage Invoice Items (FM-IC-002)
**Description**: Support line item management with tax and discount calculations.

**Functional Requirements**:
- The system shall support multiple line items per invoice
- The system shall calculate taxes based on configurable rates
- The system shall apply discounts at line item or invoice level
- The system shall maintain product/service catalogs for quick item selection

#### 3.2.3 Send Invoices (FM-IC-003)
**Description**: Enable automated invoice delivery via email with tracking.

**Functional Requirements**:
- The system shall send invoices via email automatically
- The system shall track email delivery and read status
- The system shall support multiple delivery channels
- The system shall maintain delivery history and logs

#### 3.2.4 Record Invoice Payment (FM-IC-004)
**Description**: Allow recording of payments (full or partial) against invoices.

**Functional Requirements**:
- The system shall record full and partial payments
- The system shall update invoice status based on payment amount
- The system shall track payment methods and dates
- The system shall generate receipts for payments

#### 3.2.5 Manage Clients (FM-IC-005)
**Description**: Create and manage client information.

**Functional Requirements**:
- The system shall maintain client contact information
- The system shall track client payment history
- The system shall support client categorization and notes
- The system shall integrate client data with invoice generation

### 3.3 Financial Planning and Tracking (NouAssist.FinancialManagement.FinancialPlanningAndTracking)

#### 3.3.1 Log Expenses (FM-ET-001)
**Description**: Record expenses with receipt capture and categorization.

**Functional Requirements**:
- The system shall allow manual expense entry
- The system shall support receipt image capture and OCR
- The system shall categorize expenses automatically based on rules
- The system shall validate expense data and prevent duplicates

#### 3.3.2 Track Mileage (FM-ET-002)
**Description**: Monitor travel expenses and mileage for reimbursement.

**Functional Requirements**:
- The system shall record mileage with start/end locations
- The system shall calculate mileage rates based on current IRS standards
- The system shall categorize mileage by business purpose
- The system shall generate mileage reports for reimbursement

#### 3.3.3 Generate Expense Reports (FM-ET-003)
**Description**: Create detailed expense reports for accounting and tax purposes.

**Functional Requirements**:
- The system shall generate expense reports by date range, category, or project
- The system shall include receipt images in reports
- The system shall export reports in multiple formats (PDF, Excel, CSV)
- The system shall support custom report templates

#### 3.3.4 Budget Management (FM-BU-001 through FM-BU-005)
**Description**: Comprehensive budget creation, tracking, and forecasting capabilities.

**Functional Requirements**:
- The system shall support zero-based budgeting methodology
- The system shall create category-specific budget allocations
- The system shall track actual spending against budget allocations
- The system shall allow budget modifications with version control
- The system shall generate budget forecasts based on historical data

#### 3.3.5 Financial Goals Management (FM-FP-001 through FM-FP-003)
**Description**: Create and track financial goals with progress monitoring.

**Functional Requirements**:
- The system shall allow creation of financial goals with target amounts and dates
- The system shall track progress toward financial goals
- The system shall provide goal achievement notifications
- The system shall support goal modification and updates

### 3.4 Financial Reports (NouAssist.FinancialManagement.FinancialReports)

#### 3.4.1 Standard Financial Statements (FM-FR-001 through FM-FR-006)
**Description**: Generate standard accounting reports including Balance Sheet, Income Statement, Trial Balance, Cash Flow Statement, Budget vs. Actual, and Net Worth reports.

**Functional Requirements**:
- The system shall generate Balance Sheet reports showing assets, liabilities, and equity
- The system shall create Income Statement (P&L) reports for specified periods
- The system shall produce Trial Balance reports for accounting verification
- The system shall generate Cash Flow Statements showing cash movements
- The system shall create Budget vs. Actual comparison reports
- The system shall calculate and report Net Worth statements

### 3.5 Financial Intelligence Core (NouAssist.FinancialManagement.FinancialIntelligenceCore)

#### 3.5.1 Double-Entry Accounting (FM-DE-001 through FM-DE-004)
**Description**: Full double-entry bookkeeping system with automated balancing and period management.

**Functional Requirements**:
- The system shall enforce double-entry accounting principles
- The system shall automatically balance journal entries
- The system shall manage accounting period closures
- The system shall maintain accurate ledger balances
- The system shall prevent modifications to closed periods

#### 3.5.2 Chart of Accounts Management (FM-CA-001)
**Description**: Complete chart of accounts management with CRUD operations.

**Functional Requirements**:
- The system shall maintain a comprehensive chart of accounts
- The system shall support account creation, modification, and deletion
- The system shall enforce account numbering schemes
- The system shall automatically create payee accounts based on policies

### 3.6 Financial Analytics (NouAssist.FinancialManagement.FinancialAnalytics)

#### 3.6.1 Advanced Analytics (FM-FA-001 through FM-FA-004)
**Description**: Comprehensive financial analysis, visualization, and ratio calculations.

**Functional Requirements**:
- The system shall generate comprehensive financial analysis reports
- The system shall create visual representations of financial data
- The system shall export data in multiple formats
- The system shall calculate financial ratios for liquidity, solvency, and profitability

### 3.7 Audit Support (NouAssist.FinancialManagement.AuditSupport)

#### 3.7.1 Audit Trail Management (FM-AS-001 through FM-AS-003)
**Description**: Complete audit trail functionality with secure document storage.

**Functional Requirements**:
- The system shall maintain comprehensive audit trails for all transactions
- The system shall generate detailed audit reports
- The system shall provide secure storage for audit documents
- The system shall track user actions with timestamps and change details

### 3.8 Debt and Loan Management (NouAssist.FinancialManagement.DebtAndLoanManagement)

#### 3.8.1 Debt Account Management (FM-Debt-001 through FM-Debt-008)
**Description**: Complete debt management including various debt types, payments, interest accrual, and amortization schedules.

**Functional Requirements**:
- The system shall support multiple debt types (credit cards, loans, mortgages, lines of credit)
- The system shall record and track debt payments
- The system shall calculate and apply interest automatically
- The system shall manage payment schedules and late fees
- The system shall generate amortization schedules
- The system shall handle debt payoff and account closure

### 3.9 Supporting Modules

#### 3.9.1 Shared Kernel (NouAssist.FinancialManagement.SharedKernel)
- Payee management across all modules

#### 3.9.2 Tax Management (NouAssist.FinancialManagement.TaxManagement)
- Tax liability tracking and calculation
- Tax report generation and document storage

#### 3.9.3 Currency Exchange (NouAssist.FinancialManagement.CurrencyExchange)
- Multi-currency support with exchange rate management

#### 3.9.4 Document Management (NouAssist.FinancialManagement.DocumentManagement)
- Secure document storage and retrieval

---

## 4. Non-Functional Requirements

### 4.1 Performance Requirements
- The system shall respond to user requests within 2 seconds for 95% of operations
- The system shall support concurrent access by up to 1000 users
- The system shall process batch operations within acceptable time limits

### 4.2 Security Requirements
- The system shall implement role-based access control
- The system shall encrypt sensitive financial data at rest and in transit
- The system shall maintain audit logs for all user actions
- The system shall comply with financial data protection regulations

### 4.3 Reliability Requirements
- The system shall maintain 99.9% uptime availability
- The system shall implement automated backup and recovery procedures
- The system shall provide data redundancy and failover capabilities

### 4.4 Usability Requirements
- The system shall provide an intuitive user interface
- The system shall support accessibility standards (WCAG 2.1)
- The system shall provide comprehensive help documentation
- The system shall support multiple languages

### 4.5 Scalability Requirements
- The system shall scale horizontally to accommodate growth
- The system shall support database partitioning and sharding
- The system shall handle increasing transaction volumes efficiently

---

## 5. Integration Requirements

### 5.1 Banking Integration
- Integration with banking APIs for account balance retrieval
- Support for transaction import and reconciliation
- Real-time balance monitoring and alerts

### 5.2 Email Integration
- SMTP/IMAP integration for invoice delivery
- Email tracking and delivery confirmation
- Automated payment reminders

### 5.3 Tax System Integration
- Integration with jurisdiction-specific tax calculation services
- Automated tax rate updates
- Compliance with local tax regulations

### 5.4 Exchange Rate Integration
- Real-time exchange rate feeds
- Historical exchange rate data
- Multi-currency transaction support

---

## 6. Constraints

### 6.1 Regulatory Constraints
- Compliance with accounting standards (GAAP/IFRS)
- Adherence to financial data protection regulations
- Tax reporting compliance requirements

### 6.2 Technical Constraints
- Platform compatibility requirements
- Database performance limitations
- Third-party API rate limits

### 6.3 Business Constraints
- Budget limitations for third-party services
- Timeline constraints for implementation
- Resource availability for development and testing

---

## 7. Assumptions and Dependencies

### 7.1 Assumptions
- Users have basic computer literacy and financial knowledge
- Internet connectivity is available for cloud-based features
- Third-party services maintain acceptable uptime and performance

### 7.2 Dependencies
- Banking API availability and reliability
- Email service provider capabilities
- Tax calculation service accuracy and updates
- Exchange rate data provider reliability

---

**Document Approval**

| Role | Name | Signature | Date |
|------|------|-----------|------|
| Product Owner | | | |
| System Architect | | | |
| Development Lead | | | |
| Quality Assurance Lead | | | |

---

# Use Case and Data Flow Diagrams
## NouAssist Financial Management System

**Document Version:** 1.0  
**Date:** June 14, 2025  
**Purpose:** System Analysis and Design Documentation

---

## 1. Use Case Diagrams

### 1.1 High-Level System Use Case Diagram

```
                           NouAssist Financial Management System
                                  Overall Use Case Diagram

                    ┌─────────────────────────────────────────────────────────┐
                    │                                                         │
                    │                 System Boundary                        │
                    │                                                         │
    ┌─────────────┐ │  ┌──────────────────────────────────────────────────┐ │ ┌─────────────┐
    │Individual   │─┼──┤                                                  ├─┼─│Banking      │
    │User         │ │  │  ○ Manage Personal Finances                      │ │ │System       │
    └─────────────┘ │  │  ○ Track Expenses and Income                     │ │ └─────────────┘
                    │  │  ○ Create and Monitor Budgets                    │ │
    ┌─────────────┐ │  │  ○ Generate Personal Reports                     │ │ ┌─────────────┐
    │Business     │─┼──┤                                                  ├─┼─│Email        │
    │Owner        │ │  │  ○ Manage Business Finances                      │ │ │Service      │
    └─────────────┘ │  │  ○ Create and Send Invoices                      │ │ └─────────────┘
                    │  │  ○ Track Bills and Payments                      │ │
    ┌─────────────┐ │  │  ○ Manage Clients and Vendors                    │ │ ┌─────────────┐
    │Accountant   │─┼──┤                                                  ├─┼─│Tax          │
    └─────────────┘ │  │  ○ Perform Double-Entry Accounting               │ │ │Service      │
                    │  │  ○ Generate Financial Statements                 │ │ └─────────────┘
    ┌─────────────┐ │  │  ○ Manage Chart of Accounts                      │ │
    │Financial    │─┼──┤  ○ Close Accounting Periods                      │ │ ┌─────────────┐
    │Analyst      │ │  │                                                  ├─┼─│Currency     │
    └─────────────┘ │  │  ○ Generate Advanced Analytics                   │ │ │Exchange     │
                    │  │  ○ Create Financial Visualizations               │ │ │Service      │
    ┌─────────────┐ │  │  ○ Calculate Financial Ratios                    │ │ └─────────────┘
    │Auditor      │─┼──┤  ○ Export Financial Data                         │ │
    └─────────────┘ │  │                                                  │ │ ┌─────────────┐
                    │  │  ○ Access Audit Trails                           ├─┼─│Document     │
    ┌─────────────┐ │  │  ○ Generate Audit Reports                        │ │ │Storage      │
    │System       │─┼──┤  ○ Review Financial Controls                     │ │ │Service      │
    │Administrator│ │  │                                                  │ │ └─────────────┘
    └─────────────┘ │  │  ○ Manage System Configuration                   │ │
                    │  │  ○ Maintain User Access                          │ │
                    │  │  ○ Monitor System Performance                    │ │
                    │  └──────────────────────────────────────────────────┘ │
                    │                                                         │
                    └─────────────────────────────────────────────────────────┘
```

### 1.2 Bills Management Use Case Diagram

```
                              Bills Management Module
                                 Use Case Diagram

    ┌─────────────┐                                                    ┌─────────────┐
    │Individual   │                                                    │Business     │
    │User         │                                                    │Owner        │
    └──────┬──────┘                                                    └──────┬──────┘
           │                                                                  │
           │ ┌────────────────────────────────────────────────────────────────────────┐
           │ │                    Bills Management                               │
           ├─┤                                                                   │
           │ │  ○ Create Bill Entry                                              │
           │ │    ├─ Manual Data Entry                                           │
           │ │    ├─ Scan Bill with OCR                                          │
           │ │    └─ Import from Email                                           │
           │ │                                                                   │
           │ │  ○ Categorize Bills                                               │
           │ │    ├─ Assign Expense Category                                     │
           │ │    ├─ Associate with Payee                                        │
           │ │    └─ Link to Project                                             │
           │ │                                                                   │
           │ │  ○ Set Up Recurring Bills                                         │
           │ │    ├─ Configure Frequency (Weekly/Monthly/Quarterly/Annual)       │
           │ │    ├─ Set Amount and Variation Rules                              │
           │ │    └─ Configure Notifications                                     │
           │ │                                                                   │
           │ │  ○ Mark Bill as Paid                                              │
           │ │    ├─ Record Payment Date                                         │
           │ │    ├─ Record Payment Method                                       │
           │ │    ├─ Record Payment Amount                                       │
           │ │    └─ Update Account Balances                                     │
           │ │                                                                   │
           │ │  ○ Search and Filter Bills                                        │
           │ │    ├─ By Date Range                                               │
           │ │    ├─ By Status (Paid/Unpaid/Overdue)                           │
           │ │    ├─ By Category                                                 │
           │ │    └─ By Payee                                                    │
           │ │                                                                   │
           │ │  ○ Generate Bill Reports                                          │
           │ │    ├─ Bills Due Report                                            │
           │ │    ├─ Payment History Report                                      │
           │ │    └─ Bills by Category Report                                    │
           │ └────────────────────────────────────────────────────────────────────────┘
           │
           │                    «extends»
           │ ┌────────────────────────────────────────────────────────────────────────┐
           └─┤  ○ Receive Payment Reminders                                        │
             │  ○ View Payment Calendar                                             │
             │  ○ Export Bill Data                                                 │
             └────────────────────────────────────────────────────────────────────────┘

    ┌─────────────┐
    │System       │
    │Scheduler    │──────── ○ Generate Recurring Bills
    └─────────────┘         ○ Send Payment Notifications
```

### 1.3 Invoice Management Use Case Diagram

```
                             Invoice Management Module
                                 Use Case Diagram

    ┌─────────────┐                                                    ┌─────────────┐
    │Business     │                                                    │Accountant   │
    │Owner        │                                                    │             │
    └──────┬──────┘                                                    └──────┬──────┘
           │                                                                  │
           │ ┌────────────────────────────────────────────────────────────────────────┐
           │ │                   Invoice Management                              │
           ├─┤                                                                   │
           │ │  ○ Generate Invoice                                               │
           │ │    ├─ Select Invoice Template                                     │
           │ │    ├─ Auto-generate Invoice Number                                │
           │ │    ├─ Add Company Information                                     │
           │ │    └─ Set Invoice Terms and Conditions                           │
           │ │                                                                   │
           │ │  ○ Manage Invoice Items                                           │
           │ │    ├─ Add Line Items                                              │
           │ │    ├─ Calculate Item Totals                                       │
           │ │    ├─ Apply Taxes                                                 │
           │ │    ├─ Apply Discounts                                             │
           │ │    └─ Calculate Invoice Total                                     │
           │ │                                                                   │
           │ │  ○ Send Invoices                                                  │
           │ │    ├─ Send via Email                                              │
           │ │    ├─ Generate PDF                                                │
           │ │    ├─ Track Delivery Status                                       │
           │ │    └─ Track Read Receipts                                         │
           │ │                                                                   │
           │ │  ○ Record Invoice Payment                                         │
           │ │    ├─ Record Full Payment                                         │
           │ │    ├─ Record Partial Payment                                      │
           │ │    ├─ Apply Payment to Multiple Invoices                         │
           │ │    └─ Generate Payment Receipt                                    │
           │ │                                                                   │
           │ │  ○ Manage Clients                                                 │
           │ │    ├─ Create Client Profile                                       │
           │ │    ├─ Update Client Information                                   │
           │ │    ├─ Track Client Payment History                                │
           │ │    └─ Set Client-specific Terms                                   │
           │ │                                                                   │
           │ │  ○ Track Invoice Status                                           │
           │ │    ├─ Draft Invoices                                              │
           │ │    ├─ Sent Invoices                                               │
           │ │    ├─ Paid Invoices                                               │
           │ │    └─ Overdue Invoices                                            │
           │ └────────────────────────────────────────────────────────────────────────┘
           │
           │                    «extends»
           │ ┌────────────────────────────────────────────────────────────────────────┐
           └─┤  ○ Send Payment Reminders                                           │
             │  ○ Generate Invoice Reports                                          │
             │  ○ Create Recurring Invoices                                         │
             │  ○ Export Invoice Data                                               │
             └────────────────────────────────────────────────────────────────────────┘

    ┌─────────────┐                          ┌─────────────┐
    │Email        │                          │Client       │
    │Service      │──── ○ Deliver Invoices   │             │──── ○ View Invoice Online
    └─────────────┘                          └─────────────┘     ○ Make Online Payment
```

### 1.4 Financial Reporting Use Case Diagram

```
                            Financial Reporting Module
                                Use Case Diagram

    ┌─────────────┐              ┌─────────────┐              ┌─────────────┐
    │Business     │              │Accountant   │              │Financial    │
    │Owner        │              │             │              │Analyst      │
    └──────┬──────┘              └──────┬──────┘              └──────┬──────┘
           │                            │                            │
           │ ┌──────────────────────────────────────────────────────────────────────┐
           │ │                    Financial Reporting                             │
           ├─┤                                                                     │
           │ │  ○ Generate Balance Sheet                                           │
           │ │    ├─ Current Assets and Liabilities                               │
           │ │    ├─ Fixed Assets                                                  │
           │ │    ├─ Owner's Equity                                                │
           │ │    └─ Comparative Analysis                                          │
           │ │                                                                     │
           │ │  ○ Generate Income Statement                                        │
           │ │    ├─ Revenue Recognition                                           │
           │ │    ├─ Cost of Goods Sold                                            │
           │ │    ├─ Operating Expenses                                            │
           │ │    ├─ Net Income Calculation                                        │
           │ │    └─ Period Comparison                                             │
           │ │                                                                     │
           │ │  ○ Generate Cash Flow Statement                                     │
           │ │    ├─ Operating Activities                                          │
           │ │    ├─ Investing Activities                                          │
           │ │    ├─ Financing Activities                                          │
           │ │    └─ Net Cash Flow Analysis                                        │
           │ │                                                                     │
           │ │  ○ Generate Trial Balance                                           │
           │ │    ├─ Account Balances                                              │
           │ │    ├─ Debit/Credit Verification                                     │
           │ │    └─ Balance Validation                                            │
           │ │                                                                     │
           │ │  ○ Generate Budget vs Actual Report                                 │
           │ │    ├─ Budget Variance Analysis                                      │
           │ │    ├─ Performance Metrics                                           │
           │ │    └─ Trend Analysis                                                │
           │ │                                                                     │
           │ │  ○ Generate Net Worth Report                                        │
           │ │    ├─ Asset Valuation                                               │
           │ │    ├─ Liability Assessment                                          │
           │ │    └─ Net Worth Tracking                                            │
           │ │                                                                     │
           │ │  ○ Customize Report Parameters                                      │
           │ │    ├─ Date Range Selection                                          │
           │ │    ├─ Account Filtering                                             │
           │ │    ├─ Report Format Options                                         │
           │ │    └─ Export Formats (PDF, Excel, CSV)                            │
           │ └──────────────────────────────────────────────────────────────────────┘
           │
           │                    «extends»
           │ ┌──────────────────────────────────────────────────────────────────────┐
           └─┤  ○ Schedule Automatic Reports                                        │
             │  ○ Email Report Distribution                                         │
             │  ○ Create Report Templates                                           │
             │  ○ Archive Historical Reports                                        │
             └──────────────────────────────────────────────────────────────────────┘

    ┌─────────────┐
    │Auditor      │──── ○ Access Audit Reports
    └─────────────┘     ○ Review Financial Controls
                        ○ Export Audit Trail Data
```

---

## 2. Data Flow Diagrams

### 2.1 Context Diagram (Level 0)

```
                               Context Diagram (Level 0)
                        NouAssist Financial Management System

                                        External Entities

    ┌─────────────┐                                                    ┌─────────────┐
    │Individual   │                                                    │Business     │
    │User         │                                                    │Owner        │
    └──────┬──────┘                                                    └──────┬──────┘
           │                                                                  │
           │ • Personal financial data                                        │
           │ • Expense information                                            │
           │ • Budget requests                                  • Invoice data│
           │ • Reports requests                                 • Client info │
           ▼                                                    • Bill data   ▼
    ┌──────────────────────────────────────────────────────────────────────────────────┐
    │                                                                                  │
    │                          NouAssist Financial                                    │
    │                         Management System                                       │
    │                                                                                  │
    │                            Process 0                                            │
    │                                                                                  │
    └──────────────────────────────────────────────────────────────────────────────────┘
           ▲                                                                  ▲
           │ • Financial reports                              • Audit reports │
           │ • Budget analysis                                • Financial     │
           │ • Expense reports                                  statements    │
           │                                                                  │
    ┌──────┴──────┐                                                    ┌──────┴──────┐
    │Accountant   │                                                    │Auditor      │
    │             │                                                    │             │
    └─────────────┘                                                    └─────────────┘

                                   External Systems

    ┌─────────────┐    • Account balances           • Email notifications    ┌─────────────┐
    │Banking      │    • Transaction data          • Invoice delivery       │Email        │
    │System       │◄───────────────────────────────────────────────────────►│Service      │
    └─────────────┘                    ▲                    ▲               └─────────────┘
                                       │                    │
                              ┌────────┴────────────────────┴────────┐
                              │                                      │
                              │      Central System                  │
                              │                                      │
                              └────────┬────────────────────┬────────┘
                                       │                    │
                                       ▼                    ▼
    ┌─────────────┐    • Tax calculations          • Exchange rates          ┌─────────────┐
    │Tax          │    • Tax regulations           • Currency conversion     │Currency     │
    │Service      │◄───────────────────────────────────────────────────────►│Exchange     │
    └─────────────┘                                                          │Service      │
                                                                             └─────────────┘

Data Flows:
• User Inputs: Financial transactions, requests, configurations
• System Outputs: Reports, statements, notifications, alerts
• External Data: Bank feeds, tax updates, exchange rates, email confirmations
```

### 2.2 Level 1 Data Flow Diagram

```
                              Level 1 Data Flow Diagram
                        NouAssist Financial Management System

┌─────────────┐                                                          ┌─────────────┐
│   Users     │                                                          │External     │
│             │                                                          │Systems      │
└──────┬──────┘                                                          └──────┬──────┘
       │                                                                        │
       │ User Requests                                            External Data │
       ▼                                                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                        NouAssist Financial Management System                            │
│                                                                                         │
│  ┌─────────────────┐    Bill Data      ┌─────────────────┐    Invoice Data            │
│  │1.0 Bills        │◄─────────────────►│     D1 Bills    │◄──────────────────┐        │
│  │Management       │                   │     Database    │                   │        │
│  │Process          │                   └─────────────────┘                   │        │
│  └─────────────────┘                            ▲                            │        │
│            │                                    │                            │        │
│            │ Payment Info                       │ Bill Info                  │        │
│            ▼                                    │                            │        │
│  ┌─────────────────┐                           │                ┌─────────────────┐ │
│  │2.0 Invoice      │                           │                │3.0 Financial    │ │
│  │Management       │───── Invoice Data ────────┘                │Planning &       │ │
│  │Process          │                                            │Tracking Process │ │
│  └─────────────────┘                                            └─────────────────┘ │
│            │                                                             │           │
│            │ Client Data                                                 │           │
│            ▼                                                             │           │
│  ┌─────────────────┐         Transaction Data           ┌─────────────────┐│         │
│  │     D2 Client   │◄────────────────────────────────────│5.0 Accounting   ││         │
│  │     Database    │                                     │Core Process     ││         │
│  └─────────────────┘                                     └─────────────────┘│         │
│                                                                   │         │         │
│  ┌─────────────────┐         Ledger Data                         │         │         │
│  │     D3 General  │◄───────────────────────────────────────────┘         │         │
│  │     Ledger      │                                                       │         │
│  └─────────────────┘                                                       │         │
│            ▲                                                               │         │
│            │ Account Data                                                  │         │
│            │                                                               │         │
│  ┌─────────────────┐                                            ┌─────────────────┐ │
│  │6.0 Financial    │                                            │4.0 Financial    │ │
│  │Analytics        │                                            │Reporting        │ │
│  │Process          │                                            │Process          │ │
│  └─────────────────┘                                            └─────────────────┘ │
│            │                                                             │           │
│            │ Analytics Data                                              │           │
│            ▼                                                             │           │
│  ┌─────────────────┐                                                     │           │
│  │     D4 Analytics│                                                     │           │
│  │     Database    │                                                     │           │
│  └─────────────────┘                                                     │           │
│                                                                          │           │
│  ┌─────────────────┐                                                     │           │
│  │7.0 Audit        │                                                     │           │
│  │Support Process  │                                                     │           │
│  └─────────────────┘                                                     │           │
│            │                                                             │           │
│            │ Audit Data                                                  │           │
│            ▼                                                             │           │
│  ┌─────────────────┐                                            ┌─────────────────┐ │
│  │     D5 Audit    │                                            │     D6 Reports  │ │
│  │     Trail       │                                            │     Database    │◄┘
│  └─────────────────┘                                            └─────────────────┘
│                                                                                       │
│  ┌─────────────────┐         ┌─────────────────┐         ┌─────────────────┐        │
│  │8.0 Debt         │         │9.0 Tax          │         │10.0 Document    │        │
│  │Management       │         │Management       │         │Management       │        │
│  │Process          │         │Process          │         │Process          │        │
│  └─────────────────┘         └─────────────────┘         └─────────────────┘        │
│            │                           │                           │                │
│            ▼                           ▼                           ▼                │
│  ┌─────────────────┐         ┌─────────────────┐         ┌─────────────────┐        │
│  │     D7 Debt     │         │     D8 Tax      │         │     D9 Document │        │
│  │     Database    │         │     Database    │         │     Storage     │        │
│  └─────────────────┘         └─────────────────┘         └─────────────────┘        │
│                                                                                       │
└─────────────────────────────────────────────────────────────────────────────────────────┘
```

### 2.3 Level 2 Data Flow Diagram - Bills Management Process

```
                         Level 2 DFD - Bills Management Process (1.0)

┌─────────────┐
│   Users     │
└──────┬──────┘
       │
       │ Bill Input Data
       ▼
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                              Bills Management Process                                  │
│                                                                                         │
│  ┌─────────────────┐                           ┌─────────────────┐                    │
│  │1.1 Create Bill  │    Scanned Bill Image    │1.2 OCR          │                    │
│  │Entry            │─────────────────────────►│Processing       │                    │
│  └─────────────────┘                          └─────────────────┘                    │
│           │                                             │                              │
│           │ Manual Bill Data                            │ Extracted Data               │
│           ▼                                             ▼                              │
│  ┌─────────────────┐    Validated Bill Data   ┌─────────────────┐                    │
│  │1.3 Validate     │◄────────────────────────┤1.4 Data         │                    │
│  │and Store Bill   │                          │Validation       │                    │
│  └─────────────────┘                          └─────────────────┘                    │
│           │                                                                            │
│           │ Bill Record                                                                │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │     D1.1        │                                                                  │
│  │   Bill Master   │                                                                  │
│  │     Records     │                                                                  │
│  └─────────────────┘                                                                  │
│           │                                                                            │
│           │ Bill Data                                                                  │
│           ▼                                                                            │
│  ┌─────────────────┐    Category Assignment   ┌─────────────────┐                    │
│  │1.5 Categorize   │─────────────────────────►│     D1.2        │                    │
│  │Bills            │                          │   Categories    │                    │
│  └─────────────────┘                          │   Database      │                    │
│           │                                   └─────────────────┘                    │
│           │ Categorized Bill                                                          │
│           ▼                                                                            │
│  ┌─────────────────┐                          ┌─────────────────┐                    │
│  │1.6 Setup        │    Schedule Data        │     D1.3        │                    │
│  │Recurring Bills  │─────────────────────────►│   Recurring     │                    │
│  └─────────────────┘                          │   Schedules     │                    │
│           ▲                                   └─────────────────┘                    │
│           │                                             │                              │
│           │ Recurring Bill Request                      │ Schedule Trigger             │
│           │                                             ▼                              │
│  ┌─────────────────┐                          ┌─────────────────┐                    │
│  │1.7 Mark Bill    │    Payment Data         │     D1.4        │                    │
│  │as Paid          │─────────────────────────►│   Payment       │                    │
│  └─────────────────┘                          │   Records       │                    │
│           ▲                                   └─────────────────┘                    │
│           │                                                                            │
│           │ Payment Information                                                        │
│           │                                                                            │
│  ┌─────────────────┐                                                                  │
│  │1.8 Generate     │◄─── Bill Report Request                                          │
│  │Bill Reports     │                                                                  │
│  └─────────────────┘                                                                  │
│           │                                                                            │
│           │ Bill Reports                                                               │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │External Systems │                                                                  │
│  │(Accounting Core)│                                                                  │
│  └─────────────────┘                                                                  │
│                                                                                       │
└─────────────────────────────────────────────────────────────────────────────────────────┘
```

### 2.4 Level 2 Data Flow Diagram - Invoice Management Process

```
                       Level 2 DFD - Invoice Management Process (2.0)

┌─────────────┐
│Business     │
│Owner        │
└──────┬──────┘
       │
       │ Invoice Request
       ▼
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                            Invoice Management Process                                  │
│                                                                                         │
│  ┌─────────────────┐    Client Data      ┌─────────────────┐                          │
│  │2.1 Generate     │◄───────────────────┤     D2.1        │                          │
│  │Invoice          │                     │   Client        │                          │
│  └─────────────────┘                     │   Database      │                          │
│           │                              └─────────────────┘                          │
│           │ Draft Invoice                                                              │
│           ▼                                                                            │
│  ┌─────────────────┐    Product Data     ┌─────────────────┐                          │
│  │2.2 Manage       │◄───────────────────┤     D2.2        │                          │
│  │Invoice Items    │                     │   Product       │                          │
│  └─────────────────┘                     │   Catalog       │                          │
│           │                              └─────────────────┘                          │
│           │ Invoice with Items                                                         │
│           ▼                                                                            │
│  ┌─────────────────┐    Tax Rates       ┌─────────────────┐                          │
│  │2.3 Calculate    │◄───────────────────┤     D2.3        │                          │
│  │Taxes and Totals │                     │   Tax Rules     │                          │
│  └─────────────────┘                     │   Database      │                          │
│           │                              └─────────────────┘                          │
│           │ Finalized Invoice                                                          │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │2.4 Store       │                                                                  │
│  │Invoice          │                                                                  │
│  └─────────────────┘                                                                  │
│           │                                                                            │
│           │ Invoice Record                                                             │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │     D2.4        │                                                                  │
│  │   Invoice       │                                                                  │
│  │   Database      │                                                                  │
│  └─────────────────┘                                                                  │
│           │                                                                            │
│           │ Invoice Data                                                               │
│           ▼                                                                            │
│  ┌─────────────────┐    Email Data       ┌─────────────────┐                          │
│  │2.5 Send         │────────────────────►│   Email Service │                          │
│  │Invoice          │                      │   (External)    │                          │
│  └─────────────────┘                      └─────────────────┘                          │
│           │                                       │                                    │
│           │ Delivery Status                       │ Delivery Confirmation              │
│           ▼                                       ▼                                    │
│  ┌─────────────────┐                     ┌─────────────────┐                          │
│  │2.6 Track        │                     │     D2.5        │                          │
│  │Delivery Status  │────────────────────►│   Delivery      │                          │
│  └─────────────────┘                     │   Log           │                          │
│           ▲                              └─────────────────┘                          │
│           │                                                                            │
│           │ Payment Information                                                        │
│  ┌─────────────────┐    Payment Data     ┌─────────────────┐                          │
│  │2.7 Record       │────────────────────►│     D2.6        │                          │
│  │Invoice Payment  │                      │   Payment       │                          │
│  └─────────────────┘                      │   Records       │                          │
│           ▲                              └─────────────────┘                          │
│           │                                                                            │
│           │ Payment Entry                                                              │
│                                                                                       │
└─────────────────────────────────────────────────────────────────────────────────────────┘
```

### 2.5 Level 2 Data Flow Diagram - Financial Reporting Process

```
                      Level 2 DFD - Financial Reporting Process (4.0)

┌─────────────┐
│   Users     │
└──────┬──────┘
       │
       │ Report Request
       ▼
┌─────────────────────────────────────────────────────────────────────────────────────────┐
│                           Financial Reporting Process                                  │
│                                                                                         │
│  ┌─────────────────┐    Account Data     ┌─────────────────┐                          │
│  │4.1 Generate     │◄───────────────────┤     D3 General  │                          │
│  │Balance Sheet    │                     │     Ledger      │                          │
│  └─────────────────┘                     └─────────────────┘                          │
│           │                                       ▲                                    │
│           │ Balance Sheet Data                    │                                    │
│           ▼                                       │                                    │
│  ┌─────────────────┐                             │                                    │
│  │4.2 Generate     │◄───────────────────────────┤                                    │
│  │Income Statement │                             │                                    │
│  └─────────────────┘                             │                                    │
│           │                                       │                                    │
│           │ Income Statement Data                 │                                    │
│           ▼                                       │                                    │
│  ┌─────────────────┐                             │                                    │
│  │4.3 Generate     │◄───────────────────────────┤                                    │
│  │Cash Flow        │                             │                                    │
│  │Statement        │                             │                                    │
│  └─────────────────┘                             │                                    │
│           │                                       │                                    │
│           │ Cash Flow Data                        │                                    │
│           ▼                                       │                                    │
│  ┌─────────────────┐                             │                                    │
│  │4.4 Generate     │◄───────────────────────────┤                                    │
│  │Trial Balance    │                             │                                    │
│  └─────────────────┘                             │                                    │
│           │                                       │                                    │
│           │ Trial Balance Data                    │                                    │
│           ▼                                       │                                    │
│  ┌─────────────────┐    Budget Data     ┌─────────────────┐                          │
│  │4.5 Generate     │◄───────────────────┤     D4 Budget   │                          │
│  │Budget vs Actual │                     │     Database    │                          │
│  └─────────────────┘                     └─────────────────┘                          │
│           │                                                                            │
│           │ Budget Comparison Data                                                     │
│           ▼                                                                            │
│  ┌─────────────────┐    Asset Data      ┌─────────────────┐                          │
│  │4.6 Generate     │◄───────────────────┤     D5 Asset    │                          │
│  │Net Worth Report │                     │     Database    │                          │
│  └─────────────────┘                     └─────────────────┘                          │
│           │                                                                            │
│           │ All Report Data                                                            │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │4.7 Format and   │                                                                  │
│  │Consolidate      │                                                                  │
│  │Reports          │                                                                  │
│  └─────────────────┘                                                                  │
│           │                                                                            │
│           │ Formatted Reports                                                          │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │4.8 Export and   │                                                                  │
│  │Distribute       │                                                                  │
│  │Reports          │                                                                  │
│  └─────────────────┘                                                                  │
│           │                                                                            │
│           │ Final Reports                                                              │
│           ▼                                                                            │
│  ┌─────────────────┐                                                                  │
│  │     D6 Reports  │                                                                  │
│  │     Archive     │                                                                  │
│  └─────────────────┘                                                                  │
│                                                                                       │
└─────────────────────────────────────────────────────────────────────────────────────────┘
```

---

## 3. Data Dictionary

### 3.1 Data Flows

| Data Flow Name | Description | Data Elements |
|----------------|-------------|---------------|
| **Bill Data** | Information about bills to be paid | Bill_ID + Payee_ID + Amount + Due_Date + Category + Description + Status + Created_Date |
| **Invoice Data** | Customer invoice information | Invoice_ID + Client_ID + Invoice_Number + Issue_Date + Due_Date + Line_Items + Tax_Amount + Total_Amount + Status |
| **Payment Data** | Payment transaction details | Payment_ID + Invoice_ID/Bill_ID + Amount + Payment_Date + Payment_Method + Reference_Number + Notes |
| **Expense Data** | Expense transaction information | Expense_ID + Category_ID + Amount + Date + Description + Receipt_Image + Payee + Project_ID |
| **Journal Entries** | Double-entry accounting records | Entry_ID + Transaction_Date + Description + Debit_Account + Credit_Account + Amount + Reference |
| **Account Data** | Chart of accounts information | Account_ID + Account_Number + Account_Name + Account_Type + Parent_Account + Balance + Status |
| **Client Data** | Customer/client information | Client_ID + Company_Name + Contact_Name + Address + Phone + Email + Tax_ID + Payment_Terms |
| **Report Data** | Generated financial reports | Report_ID + Report_Type + Period_Start + Period_End + Parameters + Generated_Date + Data_Content |
| **Budget Data** | Budget planning information | Budget_ID + Period + Category + Budgeted_Amount + Actual_Amount + Variance + Notes |
| **Audit Data** | System audit trail information | Audit_ID + User_ID + Action + Table_Name + Record_ID + Old_Values + New_Values + Timestamp |

### 3.2 Data Stores

| Data Store | Description | Primary Key | Key Attributes |
|------------|-------------|-------------|----------------|
| **D1 Bills Database** | Central storage for all bill information | Bill_ID | Amount, Due_Date, Payee_ID, Status, Category |
| **D2 Client Database** | Customer and client master data | Client_ID | Company_Name, Contact_Info, Payment_Terms |
| **D3 General Ledger** | Double-entry accounting transactions | Transaction_ID | Date, Debit_Account, Credit_Account, Amount |
| **D4 Budget Database** | Budget plans and tracking data | Budget_ID | Period, Category, Budgeted_Amount, Variance |
| **D5 Asset Database** | Asset management information | Asset_ID | Asset_Name, Value, Depreciation, Category |
| **D6 Reports Archive** | Historical report storage | Report_ID | Report_Type, Generation_Date, Parameters |
| **D7 Debt Database** | Debt and loan management | Debt_ID | Principal, Interest_Rate, Payment_Schedule |
| **D8 Tax Database** | Tax-related information | Tax_Record_ID | Tax_Year, Liability, Payments, Documents |
| **D9 Document Storage** | Document management system | Document_ID | File_Name, File_Type, Upload_Date, Path |
| **D10 Audit Trail** | Complete system audit logs | Audit_ID | User_ID, Action, Timestamp, Changes |

### 3.3 External Entities

| External Entity | Description | Interface Type |
|----------------|-------------|----------------|
| **Users** | System users including individuals, business owners, accountants | Web Interface, Mobile App, Desktop App |
| **Banking System** | External banking APIs for account data | REST API, File Import |
| **Email Service** | Email delivery service for notifications | SMTP, API Integration |
| **Tax Service** | Tax calculation and regulation service | Web Service API |
| **Currency Exchange** | Exchange rate and currency data | Real-time API Feed |
| **Document Storage** | External document storage service | Cloud Storage API |

### 3.4 Process Descriptions

| Process ID | Process Name | Description | Inputs | Outputs |
|------------|--------------|-------------|---------|---------|
| **1.0** | Bills Management | Manage bill creation, categorization, and payment | Bill Input, Payment Info | Bill Records, Payment Status |
| **2.0** | Invoice Management | Create, send, and track customer invoices | Client Data, Service Info | Invoices, Payment Records |
| **3.0** | Financial Planning & Tracking | Budget creation and expense tracking | Budget Requests, Expenses | Budget Reports, Variance Analysis |
| **4.0** | Financial Reporting | Generate financial statements and reports | Account Data, Parameters | Financial Reports |
| **5.0** | Accounting Core | Double-entry bookkeeping and ledger management | Transactions | Journal Entries, Account Balances |
| **6.0** | Financial Analytics | Advanced analysis and ratio calculations | Financial Data | Analytics Reports, Ratios |
| **7.0** | Audit Support | Maintain audit trails and compliance | System Activities | Audit Reports, Compliance Data |
| **8.0** | Debt Management | Track and manage debt obligations | Debt Info, Payments | Debt Status, Schedules |
| **9.0** | Tax Management | Tax calculation and reporting | Income Data, Deductions | Tax Calculations, Forms |
| **10.0** | Document Management | Store and retrieve financial documents | Documents, Metadata | Document Access, Archive |

---

**Document Information**
- **Version:** 1.0
- **Date:** June 14, 2025
- **Created by:** System Analyst
- **Reviewed by:** Architecture Team
- **Status:** Draft for Review