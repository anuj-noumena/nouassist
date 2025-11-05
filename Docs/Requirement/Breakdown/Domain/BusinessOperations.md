**1. Freelance Operations BC** (Focus: Freelancer workflow)

* **Subdomains:** Core (for freelancer persona).
* **Aggregates:**
    * `FreelanceProject` (AR)
    * `FreelanceClient` (AR)
    * `Proposal` (AR)
    * `UpworkEngagement` (AR)
* **Entities:** `FreelanceProject` (AR), `TimeEntry`, `ProjectMilestone`, `FreelanceClient` (AR), `ClientCommunicationRecord`, `Proposal` (AR), `ProposalVersion`. _(Entities referencing documents like `Proposal` would now hold a `DocumentID` VO instead of embedding document data/entities)_.
* **Value Objects:** `ProjectScopeDescription`, `BillingType`, `HourlyRate`, `MilestoneDetails`, `ProjectStatus` (Freelance specific), `ClientContactInfo`, `CommunicationLog`, `ProposalTemplateReference`, `ProposalContentSummary`, `PricingStructure`, `ProposalStatus`, `DocumentID` (for proposal document), `UpworkCredentials`, etc.
* **Services:** `UpworkConnector`, `ClientPortalService`, `AutomatedReminderScheduler`. _(Services needing document upload/retrieval interact with Document Management BC)_.
* **Commands:** `CreateFreelanceProject`, `LogProjectTime`, ..., `CreateProposal` _(may involve interaction with Document Management BC to upload proposal file)_, `SendProposal`, ...
* **Queries:** `GetFreelanceProjectDetails`, ..., `GetProposalDetails` _(may involve fetching document URL from Document Management BC)_, ...
* **Events:** `FreelanceProjectCreated`, ..., `ProposalSent` _(may include DocumentID)_, ...
* **User Stories:**
    * **Project Management:**
        * As a freelancer, I want to create freelance-specific projects so that I can manage my work effectively.
        * As a freelancer, I want to track hours or milestones for a project so that I can bill my clients accurately.
    * **Client Management:**
        * As a freelancer, I want to add new clients with their contact information and project history so that I can maintain a record of my clientele (FP-2.1).
        * As a freelancer, I want to update existing client details so that my client information is always current (FP-2.2).
        * As a freelancer, I want to delete client records, including their associations with projects and billing, so that I can manage my client list and comply with data removal requests (FP-2.3).
        * As a freelancer, I want to store client details so that I have all relevant information in one place.
        * As a freelancer, I want to maintain communication logs with clients so that I can track our interactions and agreements.
    * **Proposal Management:**
        * As a freelancer, I want to create detailed project proposals using templates, including scope, timeline, deliverables, and pricing, so that I can present professional proposals to potential clients (FP-4.1).
        * As a freelancer, I want to send proposals directly to clients via email or in-app notifications and track their status (sent, viewed, accepted, rejected) so that I can manage the proposal lifecycle efficiently (FP-4.2).
        * As a freelancer, I want to track proposal versions and acceptance so that I have a clear record of agreements.
    * **Upwork Integration:**
        * As a freelancer, I want to retrieve job listings from Upwork via API so that I can find new opportunities.
        * As a freelancer, I want to submit proposals or track applications on Upwork so that I can manage my Upwork activities within the platform.
        * As a freelancer, I want to handle contract data from Upwork so that I can integrate my Upwork projects with my other freelance work.
    * **Client Portal:**
        * As a client of a freelancer, I want to view project progress and communicate with the freelancer through a client portal so that I can stay informed and collaborate effectively (TK-02-FR028).
    * **Automated Reminders:**
        * As a freelancer, I want to receive automated reminders for project-related events so that I don't miss important deadlines or tasks (TK-02-FR029).
* **Missing/Considerations:**
    * Consider `Invoice` aggregate and related entities/VOs for billing features.
    * `PaymentTracking` features might be relevant.

**2. Customer Relationship Management (CRM) BC** (Focus: Lead-to-Contact lifecycle)

* **Subdomains:** Core.
* **Aggregates:**
    * `Lead` (AR)
    * `Contact` (AR)
    * `Organization` (AR)
    * `InteractionLog` (AR)
    * `AutomationWorkflow` (AR) _(Decision: Kept as AR)_
* **Entities:** `Lead` (AR), `LeadSourceRecord`, ..., `Contact` (AR), `Organization` (AR), ..., `InteractionLog` (AR), `CommunicationRecord` _(If attaching files, holds `DocumentID` VO)_, `FollowUpTask`, `CommunicationTemplate`, `AutomationWorkflow` (AR), `WorkflowStep`, `WorkflowExecutionRecord`, `DataQualityRecord`, ...
* **Value Objects:** `LeadDetails`, `ContactInfo`, `OrganizationInfo`, ..., `DocumentID` (for attachments), `AutomationTriggerCondition`, `AutomationActionType`, ...
* **Services:** `LeadCaptureService`, ..., `CommunicationManager` _(interacts with Document Management BC for attachments)_, `AutomationEngine`, `AnalyticsService`, ...
* **Commands:** `CaptureLead`, ..., `LogInteraction` _(may include DocumentID for attachment)_, ..., `ConfigureAutomation`, `TriggerAutomation`, ...
* **Queries:** `GetLeadDetails`, ..., `GetInteractionHistory` _(may include retrieving doc info)_, ...
* **Events:** `LeadCaptured`, ..., `InteractionLogged` _(may include DocumentID)_, ..., `AutomationWorkflowTriggered`, ...
* **User Stories:**
    * **Lead Management:**
        * As a sales user, I want to capture new leads from various sources with custom fields so that I can build my sales pipeline (LC-LC-001).
        * As a sales user, I want to bulk import leads from external sources so that I can efficiently add multiple leads at once (LC-LC-002).
        * As a sales user, I want the system to validate and deduplicate lead information so that I maintain high-quality lead data (LC-LC-003).
        * As a sales user, I want to capture leads from web forms so that I can automate lead generation from my website.
    * **Track Lead Interactions:**
        * As a sales user, I want to record all interactions with leads (calls, emails, meetings) so that I have a complete history of communication (LC-LT-001).
        * As a sales user, I want to monitor lead engagement and response patterns so that I can understand lead interest and tailor my approach (LC-LT-002).
        * As a sales user, I want to set follow-up reminders for lead interactions so that I can nurture leads effectively (LC-LT-003).
    * **Contact Management:**
        * As a user, I want to store contact information and organization details so that I have a centralized contact database.
        * As a user, I want to manage contact groups and view analytics so that I can segment and understand my contacts.
    * **Lead Qualification:**
        * As a sales manager, I want to implement lead scoring based on defined criteria so that sales reps can prioritize high-potential leads (LC-LQ-001).
        * As a sales user, I want to use a qualification process with custom stages so that I can systematically move leads through the sales funnel (LC-LQ-002).
        * As a sales user, I want to track changes in lead qualification status so that I always know where a lead stands (LC-LQ-003).
    * **Contact Organization:**
        * As a user, I want to organize contacts into groups so that I can manage them more effectively (LC-CO-001).
        * As a user, I want to use a tagging system for contact categorization so that I can easily filter and find contacts (LC-CO-002).
        * As a user, I want to manage relationship hierarchies between contacts so that I understand the connections within an organization (LC-CO-003).
    * **Communication:**
        * As a user, I want to send messages directly to contacts so that I can communicate efficiently (LC-CM-001).
        * As a user, I want all communication history to be recorded so that I have a complete log of interactions (LC-CM-002).
        * As a user, I want to manage communication templates so that I can send consistent and professional messages quickly (LC-CM-003).
    * **Integration:**
        * As a user, I want to connect with external CRM systems so that I can synchronize data (LC-IN-001).
        * As a user, I want to sync with email platforms so that my communications are automatically logged (LC-IN-002).
        * As a marketing user, I want to integrate with marketing automation platforms so that lead data flows seamlessly between systems (TK-02-FR031).
    * **Analytics:**
        * As a sales manager, I want to generate lead performance analytics so that I can understand the effectiveness of our lead generation efforts (LC-AN-001).
        * As a sales manager, I want to analyze contact engagement patterns so that I can identify trends and opportunities (LC-AN-002).
        * As a sales manager, I want to measure the return on investment (ROI) for lead generation efforts so that I can optimize our spending (LC-AN-003).
    * **Automation:**
        * As a sales user, I want to set up automated follow-up sequences so that I can nurture leads consistently without manual effort (LC-AU-001).
        * As a sales manager, I want the system to create automated tasks based on lead activity so that follow-up actions are not missed (LC-AU-002).
        * As a user, I want to manage an automated notification system so that I am alerted to important lead or contact events (LC-AU-003).
    * **Data Management:**
        * As an admin, I want tools for data cleanup so that I can maintain data quality and accuracy (LC-DM-001).
        * As an admin, I want the system to identify and merge duplicate records so that our CRM data is clean (LC-DM-002).
        * As a user, I want to enrich contact data from external sources so that I have more complete profiles (LC-DM-003).
    * **Lead Source Tracking:**
        * As a marketing manager, I want to implement lead source tracking so that I can understand which channels are most effective for generating leads (TK-02-FR030).
* **Missing/Considerations:**
    * `Campaign` aggregate for managing marketing campaigns.
    * More explicit entities/VOs for `Segment` or `Audience`.

**3. Contract & Portfolio Management (C&PM) BC** (Focus: Contract lifecycle & high-level portfolio oversight)

* **Subdomains:** Core.
* **Aggregates:**
    * `Contract` (AR)
    * `Portfolio` (AR)
    * `PortfolioRoadmap` (AR)
* **Entities:**
    * Within `Contract` AR: `ContractVersion`, `ContractClause`, `ContractParty`, `ContractAmendment`, `ContractAlert`. _(Note: No `ContractDocument` entity)_
    * Within `Portfolio` AR: `PortfolioItem` _(Now an Entity)_, `PortfolioCategory`, `PortfolioRisk`, `PortfolioComplianceRecord`.
    * Within `PortfolioRoadmap` AR: `RoadmapMilestone`, `CrossItemDependency`.
* **Value Objects:** `ContractTemplateReference`, `ContractTermsData`, `ContractStatus`, `DocumentID` _(held by Contract/Version/Amendment entities to reference documents)_, `AmendmentDetails`, `AlertCondition`, `VersionMetadata`, `PortfolioGoal`, `ItemReference` (within `PortfolioItem`, holding e.g., `ProjectID` or `ContractID`), `PerformanceMetric`, `SharingLink`, `AccessPermissions`, `RiskAssessmentData`, `ComplianceRule`, `AuditTrailEntry`, `MilestoneDate`, `DependencyType`, `ResourceSkillRequirement`.
* **Services:** `ContractCreationService` _(interacts with Document Management BC)_, `ContractStatusMonitor`, `ContractVersioningService` _(interacts with Document Management BC)_, `PortfolioAggregationService`, `PortfolioReportingService`, ..., `ResourceForecastingService` (interacts with RIM).
* **Commands:** `CreateContract` _(includes DocumentID)_, `SetContractTerms`, ..., `UploadContractDocument` _(replaced by interaction with Document Management BC, likely part of Create/Version commands)_, `VersionContract` _(includes DocumentID)_, ..., `CreatePortfolio`, `AddPortfolioItem` _(takes ItemReference)_, ...
* **Queries:** `GetContractDetails` _(may include retrieving doc info)_, ..., `GetPortfolioOverview`, `GetPortfolioItems`, ...
* **Events:** `ContractCreated` _(includes DocumentID)_, ..., `ContractVersioned` _(includes DocumentID)_, `PortfolioCreated`, `PortfolioItemAdded`, ...
* **User Stories:**
    * **Contract Management:**
        * As a legal user/contract manager, I want to create new contracts using customizable templates so that I can standardize contract creation (CP-CC-001).
        * As a legal user/contract manager, I want to define contract terms, conditions, and clauses so that all aspects of the agreement are captured (CP-CC-002).
        * As a legal user/contract manager, I want to manage the parties involved in the contract so that it's clear who the signatories are (CP-CC-003).
        * As a user, I want to upload and version contracts so that I have a history of all contract documents.
        * As a user, I want to track contract status (e.g., draft, active, closed) so that I know the current state of any contract (CP-CM-001).
        * As a legal user/contract manager, I want to manage contract modifications and amendments so that changes are properly documented and approved (CP-CM-002).
        * As a user, I want to create alerts for key contract dates and milestones so that I don't miss important obligations or renewal dates (CP-CM-003).
    * **Portfolio Management:**
        * As a portfolio manager, I want to create project and work portfolios so that I can group related initiatives (CP-PC-001).
        * As a portfolio manager, I want to add items (projects, contracts) to portfolios so that I can build out my portfolio view (CP-PC-002).
        * As a portfolio manager, I want to organize portfolios by categories so that I can better structure and analyze them (CP-PC-003).
        * As a portfolio manager, I want to monitor portfolio performance metrics so that I can assess the health and progress of my portfolios (CP-PM-001).
        * As a portfolio manager, I want to manage updates to portfolio content so that the information remains current (CP-PM-002).
        * As a portfolio manager, I want to generate portfolio performance reports so that I can share insights with stakeholders (CP-PM-003).
        * As an executive/stakeholder, I want a high-level overview of active projects (progress, budgets, resource usage) so that I can make informed strategic decisions (PM-CP-001).
    * **Portfolio Sharing:**
        * As a portfolio manager, I want to share portfolios with stakeholders so that they can view relevant information (CP-PS-001).
        * As a portfolio manager, I want to manage access permissions for shared portfolios so that I can control who sees what (CP-PS-002).
        * As a portfolio manager, I want to track portfolio access and views so that I know who is looking at the information (CP-PS-003).
    * **Document Management (within C&PM context):**
        * As a user, I want to store contract and portfolio-related documents so that all relevant files are centralized (CP-DM-001).
        * As a user, I want to maintain an organized file structure for these documents so that they are easy to find (CP-DM-002).
        * As a user, I want to search and retrieve documents quickly so that I can access information when needed (CP-DM-003).
    * **Risk Management:**
        * As a portfolio/contract manager, I want to evaluate contract and portfolio risks so that I can identify potential issues (CP-RM-001).
        * As a portfolio/contract manager, I want to track identified risk factors so that I can monitor them proactively (CP-RM-002).
        * As a portfolio/contract manager, I want to create risk assessment reports so that I can communicate risks to stakeholders (CP-RM-003).
    * **Compliance:**
        * As a compliance officer/contract manager, I want to monitor compliance with contract terms so that we meet our obligations (CP-CO-001).
        * As a compliance officer/contract manager, I want to create compliance-related alerts so that I am notified of potential issues (CP-CO-002).
        * As a compliance officer/contract manager, I want to maintain a compliance audit trail so that we have a record of compliance activities (CP-CO-003).
    * **Analytics:**
        * As a portfolio manager, I want to generate portfolio performance analytics so that I can gain deeper insights into portfolio health (CP-AN-001).
        * As a contract manager, I want to analyze contract performance and trends so that I can identify areas for improvement (CP-AN-002).
        * As a user, I want to export analytics reports so that I can share them or use them in other tools (CP-AN-003).
    * **Contract Version Control:**
        * As a legal user/contract manager, I want to maintain a version history of contract changes so that I can see how a contract has evolved (CP-VC-001).
        * As a legal user/contract manager, I want to compare different contract versions so that I can easily identify changes (CP-VC-002).
        * As a legal user/contract manager, I want to manage the contract archival process so that old contracts are stored appropriately (CP-VC-003).
    * **Roadmapping:**
        * As a project/portfolio manager, I want to create timeline-based roadmaps with milestone tracking and dependency visualization across projects so that I can plan and communicate the strategic direction (PM-CP-002).
    * **Resource Pooling (as it relates to portfolio):**
        * As a resource manager/portfolio manager, I want to allocate resources from shared pools across multiple projects, track conflicts, and forecast resource needs so that I can optimize resource utilization at a portfolio level (PM-CP-003).
* **Missing/Considerations:**
    * `ObligationTracking` as a specific entity/service.
    * More detailed `ApprovalWorkflow` for contracts and amendments.

**4. Resource & Inventory Management (RIM) BC** (Focus: Resource/Inventory definition, allocation, tracking, optimization)

* **Subdomains:** Core/Supporting.
* **Aggregates:**
    * `ResourceType` (AR): Defines classes (e.g., "HumanResource", "Equipment"). Includes `ResourceAttributeDefinition` entities.
    * `ResourceInstance` (AR): Specific trackable resource (e.g., "Jane Doe", "Laptop XYZ"). Holds attribute values based on its `ResourceType`. _Crucially, for Human Resources, includes `UserID` VO linking to identity._
    * `InventoryItemDefinition` (AR): Definition of a stockable item (SKU, description). _Includes `StockLevel` Entities._
    * `ResourceBooking` (AR): Reservation for a `ResourceInstance`.
    * `StorageLocation` (AR): Physical or logical place for inventory.
* **Entities:**
    * Within `ResourceType`: `ResourceAttributeDefinition`.
    * Within `ResourceInstance`: `AllocationRecord`, `UsageRecord`.
    * Within `InventoryItemDefinition`: `StockLevel` _(Entity, containing LocationID, Quantity)_, `InventoryTransaction`.
    * `InventoryAuditSession`, `AuditDiscrepancy`, `SharingPolicy`, `ResourceBooking` (AR), `StorageLocation` (AR), `LocationHierarchyNode`.
* **Value Objects:** `AttributeSchema`, `AttributeValue`, `UserID` (in Human `ResourceInstance`), `Skill` (if defined as VO), `Role` (if defined as VO), `CategoryPath`, `AllocationDetails`, `UsageMetric`, `StockKeepingUnit` (SKU), `Quantity`, `UnitOfMeasure`, `LocationID`, `TransactionType`, `AlertThreshold`, `AuditResult`, `SharingRule`, `BookingTimeSlot`, `ReservationStatus`, `Barcode`, `LocationCoordinates`.
* **Services:** `ResourceAllocator`, `UsageTracker`, `ResourceOptimizer`, `InventoryManager`, `InventoryAuditor`, `ResourceSharer`, `InventoryForecaster`, `BarcodeGenerator/ScannerInterface`, `LocationManager`, `BookingService`.
* **Commands:** `DefineResourceType`, `DefineInventoryItem`, `CreateResourceInstance` _(associates UserID for humans)_, `AdjustStockLevel` _(replaces UpdateStockLevel, creates Transaction, updates StockLevel entity within Item)_, `AllocateResource`, `LogResourceUsage`, ..., `ConductInventoryAudit`, ... `BookResource`, ...
* **Queries:** `GetResourceTypeDetails`, `ListResourceInstances` _(can filter by attributes like Skill)_, `GetResourceAllocation`, ..., `GetInventoryItemDefinition` _(includes stock levels)_, `GetStockLevelByItemAndLocation`, ..., `GetResourceAvailability`, ...
* **Events:** `ResourceTypeDefined`, `InventoryItemDefined`, `ResourceInstanceAvailable`, `StockLevelAdjusted` _(replaces StockLevelUpdated)_, `ResourceAllocated`, `ResourceUsageLogged`, ..., `InventoryAuditCompleted`, ..., `ResourceBooked`, ...
* **User Stories:**
    * **Resource Definition:**
        * As an admin/resource manager, I want to create resource types with attributes, categories, and properties so that I can define the kinds of resources we manage (RI-RD-001).
        * As an admin/resource manager, I want to update existing resource type definitions so that I can modify them as our needs change (RI-RD-002).
        * As an admin/resource manager, I want to archive unused resource types while maintaining historical data so that our list of active resource types is current (RI-RD-003).
        * As an admin/resource manager, I want to manage category hierarchies for resources so that they are well-organized (RI-CM-001, RI-CM-002).
        * As an admin/resource manager, I want to merge similar or duplicate resource categories so that our categorization is clean (RI-CM-003).
    * **Resource Allocation & Tracking:**
        * As a project manager/resource manager, I want to assign resources to tasks or projects so that work can be completed (RI-RA-001).
        * As a project manager/resource manager, I want to monitor the utilization of allocated resources so that I can see how resources are being used (RI-RA-002).
        * As a project manager/resource manager, I want to manage the release of resources back to the available pool so that they can be reallocated (RI-RA-003).
    * **Resource Optimization:**
        * As a resource manager, I want to analyze resource utilization patterns so that I can identify inefficiencies (RI-RO-001).
        * As a resource manager, I want to detect resource constraints and bottlenecks so that I can address them (RI-RO-002).
        * As a resource manager, I want the system to recommend alternative resource allocation strategies so that I can optimize resource use (RI-RO-003).
    * **Inventory Tracking:**
        * As an inventory manager, I want to monitor current inventory levels across locations so that I know what we have in stock (RI-IT-001).
        * As an inventory manager, I want all inventory movements and changes to be logged so that I have an accurate transaction history (RI-IT-002).
        * As an inventory manager, I want to create alerts for low stock or other inventory conditions so that I can take timely action (RI-IT-003, TK-02-FR033).
    * **Inventory Audit:**
        * As an inventory manager, I want to conduct physical inventory counts and reconcile them with system records so that I can ensure accuracy (RI-IA-001).
        * As an inventory manager, I want to track differences between expected and actual inventory so that discrepancies can be investigated (RI-IA-002).
        * As an inventory manager, I want to create detailed reports of audit findings so that I can document the audit process (RI-IA-003).
    * **Resource Sharing:**
        * As a team lead/manager, I want to enable sharing of resources between teams or projects so that we can maximize resource utilization (RI-RS-001).
        * As an admin, I want to control access permissions for shared resources so that only authorized users can access them (RI-RS-002).
        * As a manager, I want to monitor the usage of shared resources across teams so that I can understand sharing patterns (RI-RS-003).
    * **Inventory Forecasting:**
        * As an inventory manager, I want to generate demand forecasts based on historical data so that I can plan procurement (RI-IF-001).
        * As an inventory manager, I want to allow manual adjustment of automated forecasts so that I can incorporate my expertise (RI-IF-002).
        * As an inventory manager, I want to monitor the accuracy of inventory predictions so that I can improve forecasting models (RI-IF-003).
    * **Resource Analytics:**
        * As a manager, I want to create comprehensive resource usage analytics so that I can understand resource performance (RI-AN-001).
        * As a manager, I want to evaluate resource allocation efficiency so that I can identify areas for improvement (RI-AN-002).
        * As a manager, I want to export analytics in various formats so that I can use the data in other reports or systems (RI-AN-003).
    * **Barcode Integration:**
        * As an inventory manager, I want to create barcodes for inventory items so that I can easily track them (RI-BI-001).
        * As an inventory staff, I want to use barcode scanning for inventory operations so that I can speed up processes and reduce errors (RI-BI-002).
        * As an admin, I want to manage barcode system configuration and settings so that the system works correctly (RI-BI-003).
    * **Location Management:**
        * As an admin/inventory manager, I want to define storage locations and zones so that I can organize our inventory physically (RI-LM-001).
        * As an admin/inventory manager, I want to organize locations in a hierarchical structure so that complex storage setups can be modeled (RI-LM-002).
        * As an admin/inventory manager, I want to monitor the capacity and utilization of locations so that I can optimize space (RI-LM-003).
    * **Resource Booking & Reservation:**
        * As a user, I want to book or reserve resources so that I can ensure their availability when needed (TK-02-FR032).
* **Missing/Considerations:**
    * `MaintenanceSchedule` for equipment resources.
    * `SkillManagement` features (e.g., skill expiry, certification tracking) if skills are complex.
    * More detailed `ProcurementRequest` or `PurchaseOrder` link for inventory items.

**5. Project Execution BC** (New - Focus: Detailed project management)

* **Subdomains:** Supporting/Core (depending on business focus).
* **Aggregates:**
    * `Project` (AR): Represents a distinct piece of work with objectives, timelines, resources, tasks. Generates the canonical `ProjectID`.
    * `Task` (AR): A unit of work within a project, can be assigned, estimated, tracked.
    * `TimeLog` (AR): Records time spent by resources on tasks/projects.
* **Entities:** `Project` (AR), `ProjectMember`, `ProjectPhase`/`ProjectMilestone` (detailed level), `Task` (AR), `TaskAssignment`, `TaskDependency`, `TimeLog` (AR).
* **Value Objects:** `ProjectID`, `ProjectDetails` (Name, Description, Dates), `ProjectStatus` (Execution specific: Planning, Active, On Hold, Completed), `TaskDetails` (Description, Estimate, Priority), `TaskStatus` (ToDo, InProgress, Done, Blocked), `DependencyType` (FS, SS, etc.), `TimeLogDetails` (UserID, ResourceInstanceID, Duration, Date, Notes), `ResourceRequirement` (Skill, Role needed for Task/Project).
* **Services:** `ProjectScheduler`, `TaskAssignmentService`, `ProgressTrackingService`, `TimeLoggingService`, `ProjectTemplateService`.
* **Commands:** `CreateProject`, `DefineProjectPhase`, `AddProjectMember`, `UpdateProjectStatus`, `CreateTask`, `AssignTask`, `SetTaskDependency`, `UpdateTaskStatus`, `LogTime`, `CompleteProject`.
* **Queries:** `GetProjectDetails`, `GetProjectPlan` (Tasks, Timeline), `GetProjectStatusReport`, `GetTaskDetails`, `GetUserTasks`, `GetProjectTimeLogs`, `GetResourceWorkload`.
* **Events:** `ProjectCreated` (includes `ProjectID`), `ProjectPhaseDefined`, `ProjectMemberAdded`, `ProjectStatusUpdated`, `TaskCreated`, `TaskAssigned`, `TaskStatusUpdated`, `TimeLogged`, `ProjectCompleted`.
* **User Stories:**
    * As a project manager, I want to create a new project with a name, description, and timeline so that I can initiate and define the scope of work.
    * As a project manager, I want to define project phases and detailed milestones so that I can break down the project into manageable parts and track progress.
    * As a project manager, I want to add members to a project and assign roles so that the project team is clearly defined.
    * As a project manager, I want to update the project status (e.g., Planning, Active, On Hold, Completed) so that stakeholders are aware of the project's current state.
    * As a project member/manager, I want to create tasks within a project, providing details like description, estimate, and priority, so that work items are clearly defined.
    * As a project manager, I want to assign tasks to project members so that responsibility for work is clear.
    * As a project manager, I want to set dependencies between tasks (e.g., finish-to-start) so that the project schedule accurately reflects task relationships.
    * As a project member, I want to update the status of my tasks (e.g., ToDo, InProgress, Done, Blocked) so that progress is tracked.
    * As a project member, I want to log time spent on tasks or projects, including duration, date, and notes, so that effort is recorded accurately.
    * As a project manager, I want to mark a project as complete so that the project lifecycle is concluded.
    * As a project manager, I want to view the overall project plan, including tasks and timelines, so that I can manage the project schedule.
    * As a stakeholder, I want to get a project status report so that I am informed about progress, risks, and issues.
    * As a project member, I want to see a list of tasks assigned to me so that I know what I need to work on.
    * As a resource manager, I want to see the workload of resources based on project time logs so that I can manage resource allocation.
    * As a project manager, I want to use project templates so that I can quickly create new projects based on standard structures.
* **Integration:**
    * -> C&PM: `ProjectCreated`/`ProjectStatusUpdated` events allow C&PM to update its `PortfolioItem` reference.
    * -> RIM: Sends requests (Commands like `AllocateResource` or custom requests) for resources based on `ResourceRequirement` VO. `TimeLog` may reference `ResourceInstanceID`.
    * -> CRM: `Project` may hold `ContactID`/`OrganizationID`.
* **Missing/Considerations:**
    * `RiskLog` or `IssueLog` entities specifically for projects.
    * `ProjectBudget` and `ExpenseTracking` features.
    * Gantt chart/visual timeline generation capability.
    * Collaboration features like comments on tasks/projects.

**6. Document Management BC** (New - Focus: Generic document storage & retrieval)

* **Subdomains:** Generic/Infrastructure.
* **Aggregates:**
    * `Document` (AR): Represents the file, its metadata, versions, and access control. Generates `DocumentID`.
* **Entities:** `DocumentVersion`.
* **Value Objects:** `DocumentID`, `DocumentMetadata` (Filename, Size, MIME Type, UploadedBy UserID, Timestamp), `StorageReference` (Internal pointer to Blob storage etc.), `AccessGrant` (UserID/Role, Permission Level), `FileVersionNumber`.
* **Services:** `DocumentStorageService` (Handles upload/download streams, interacts with blob storage), `DocumentRetrievalService`, `VersioningService`, `AccessControlService`, `MetadataService`.
* **Commands:** `UploadDocument`, `GetDocumentDownloadUrl`, `GetDocumentMetadata`, `CreateDocumentVersion`, `GrantDocumentAccess`, `RevokeDocumentAccess`, `DeleteDocument` (logical/physical).
* **Queries:** `GetDocumentMetadata`, `ListDocumentVersions`, `CheckDocumentAccess`.
* **Events:** `DocumentUploaded` (includes `DocumentID`, `Metadata`), `DocumentVersionCreated`, `DocumentAccessGranted`, `DocumentDeleted`.
* **User Stories:**
    * As a user, I want to upload a document and store its metadata (filename, size, type, uploader, timestamp) so that I can centrally manage files.
    * As a user, I want to get a download URL for a document so that I can access its content.
    * As a user, I want to retrieve the metadata of a document so that I can see its properties.
    * As a user, I want to create a new version of an existing document so that I can track changes over time.
    * As an owner/admin, I want to grant access permissions (e.g., view, edit) for a document to specific users or roles so that I can control who can access it.
    * As an owner/admin, I want to revoke access permissions for a document so that I can manage security.
    * As a user, I want to delete a document (logically or physically) so that I can manage my stored files.
    * As a user, I want to list all versions of a document so that I can see its history.
    * As a user, I want to check if I have access to a specific document so that I know my permissions.
* **Integration:** Used by C&PM, Freelance Ops, CRM, Project Execution via `DocumentID` references and service calls.
* **Missing/Considerations:**
    * Document tagging/categorization.
    * Full-text search capabilities within documents.
    * Document preview generation.
    * Audit trail for document access and modifications.
