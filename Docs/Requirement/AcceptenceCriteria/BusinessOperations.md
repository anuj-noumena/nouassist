# Acceptance Criteria for Business Operations Platform Aggregates

## 1. Freelance Operations Bounded Context

### 1.1 FreelanceProject Aggregate Root

#### AC-FO-01: Project Creation
**Given** a valid freelance client exists and required project details are provided  
**When** creating a new freelance project  
**Then** the system should:
- Generate a unique ProjectId
- Validate billing type and hourly rate consistency
- Ensure start date is not in the past
- Ensure end date (if provided) is after start date
- Link project to existing client
- Publish FreelanceProjectCreated event
- Set initial status to "Planning"

#### AC-FO-02: Time Entry Validation
**Given** an active freelance project exists  
**When** logging time entries  
**Then** the system should:
- Only accept time entries for projects with "Active" status
- Validate time entry date is within project timeline
- Ensure duration is positive and reasonable (≤ 24 hours)
- Calculate billable amount using project's hourly rate or entry-specific override
- Update project's total time tracking
- Publish ProjectTimeLogged event

#### AC-FO-03: Project Status Transitions
**Given** a freelance project in any valid status  
**When** updating project status  
**Then** the system should:
- Validate status transition is allowed (Planning → Active → Invoiced → Completed)
- Prevent time logging for non-active projects
- Calculate final billing totals when status moves to "Invoiced"
- Maintain status change audit trail
- Publish appropriate status change events

#### AC-FO-04: Milestone Management
**Given** an active freelance project  
**When** adding or updating project milestones  
**Then** the system should:
- Ensure milestone target dates are within project timeline
- Validate milestone dates are chronologically ordered
- Allow milestone completion only in sequential order
- Update project progress percentage based on completed milestones
- Trigger client notifications for milestone completions

### 1.2 FreelanceClient Aggregate Root

#### AC-FO-05: Client Creation and Validation
**Given** new client information is provided  
**When** creating a freelance client  
**Then** the system should:
- Generate unique FreelanceClientId
- Validate contact information completeness and format
- Ensure client name is unique within tenant
- Initialize empty communication history
- Set creation timestamp
- Enable client for project assignment

#### AC-FO-06: Communication Tracking
**Given** an existing freelance client  
**When** logging client communications  
**Then** the system should:
- Create immutable communication records
- Timestamp all communications accurately
- Support multiple communication types (email, call, meeting, etc.)
- Allow document attachments via Document Management BC
- Maintain chronological communication history
- Publish ClientCommunicationLogged event

#### AC-FO-07: Client Information Updates
**Given** an existing freelance client  
**When** updating client contact information  
**Then** the system should:
- Validate new contact information format
- Maintain history of contact information changes
- Update all related project notifications
- Preserve communication history integrity
- Notify relevant project stakeholders of changes

### 1.3 Proposal Aggregate Root

#### AC-FO-08: Proposal Creation and Content Management
**Given** a freelance client exists and proposal content is provided  
**When** creating a new proposal  
**Then** the system should:
- Generate unique ProposalId
- Link proposal to existing client
- Validate pricing structure completeness
- Support document attachment via Document Management BC
- Set initial status to "Draft"
- Enable proposal editing until sent

#### AC-FO-09: Proposal Versioning
**Given** an existing proposal in "Draft" or "Under Review" status  
**When** creating a new version  
**Then** the system should:
- Generate sequential version numbers
- Create new document version in Document Management BC
- Maintain complete version history
- Record change notes and version metadata
- Preserve previous versions immutably
- Update proposal's current version reference

#### AC-FO-10: Proposal Workflow Management
**Given** a proposal ready for sending  
**When** sending proposal to client  
**Then** the system should:
- Validate proposal content completeness
- Verify client contact information is current
- Lock proposal from further editing
- Track delivery method and timestamp
- Set status to "Sent"
- Generate tracking capabilities for client engagement
- Publish ProposalSent event

#### AC-FO-11: Proposal Status Tracking
**Given** a sent proposal  
**When** tracking proposal status changes  
**Then** the system should:
- Accept status updates: Viewed, Under Review, Accepted, Rejected
- Prevent modification of accepted proposals
- Support automatic project creation for accepted proposals
- Maintain status change audit trail
- Enable automated follow-up based on status

### 1.4 UpworkEngagement Aggregate Root

#### AC-FO-12: Upwork Integration Management
**Given** valid Upwork credentials and engagement details  
**When** creating an Upwork engagement  
**Then** the system should:
- Validate Upwork API credentials
- Store engagement details securely
- Enable optional linking to internal FreelanceProject
- Support synchronization status tracking
- Maintain engagement status consistency with Upwork
- Handle API connectivity issues gracefully

#### AC-FO-13: Project Synchronization
**Given** an Upwork engagement and internal freelance project  
**When** linking engagement to project  
**Then** the system should:
- Validate project exists and is accessible
- Ensure one-to-one relationship (one engagement per project)
- Synchronize time tracking between systems
- Maintain billing consistency
- Support unlinking with data preservation

---

## 2. Customer Relationship Management (CRM) Bounded Context

### 2.1 Lead Aggregate Root

#### AC-CRM-01: Lead Capture and Qualification
**Given** lead information from various sources  
**When** capturing a new lead  
**Then** the system should:
- Generate unique LeadId
- Validate contact information requirements
- Record lead source for attribution
- Initialize qualification score to zero
- Set status to "New"
- Publish LeadCaptured event
- Trigger automation workflows if configured

#### AC-CRM-02: Lead Qualification Process
**Given** a captured lead with qualification criteria  
**When** qualifying the lead  
**Then** the system should:
- Calculate qualification score based on business rules
- Validate score is within acceptable range (0-100)
- Update lead status based on score thresholds
- Record qualification notes and history
- Trigger automated follow-up if score meets thresholds
- Publish LeadQualified event

#### AC-CRM-03: Lead Conversion
**Given** a qualified lead ready for conversion  
**When** converting lead to contact  
**Then** the system should:
- Create new Contact in Contact BC
- Transfer all lead information to contact
- Link to organization if specified
- Set lead status to "Converted"
- Maintain conversion audit trail
- Publish LeadConvertedToContact event
- Archive lead with reference to new contact

### 2.2 Contact Aggregate Root

#### AC-CRM-04: Contact Management
**Given** contact information and optional organization  
**When** creating a new contact  
**Then** the system should:
- Generate unique ContactId
- Validate contact information completeness
- Link to organization if provided and valid
- Initialize engagement level as "Active"
- Support flexible tagging system
- Enable grouping and segmentation
- Set up interaction tracking

#### AC-CRM-05: Engagement Level Management
**Given** a contact with interaction history  
**When** calculating engagement level  
**Then** the system should:
- Automatically update based on interaction frequency
- Consider interaction types and recency
- Support manual override with justification
- Trigger automation workflows for engagement changes
- Maintain engagement history for analysis
- Alert on significant engagement drops

### 2.3 Organization Aggregate Root

#### AC-CRM-06: Organization Management
**Given** organization details and structure  
**When** creating or managing organizations  
**Then** the system should:
- Generate unique OrganizationId
- Validate organization information
- Support hierarchical organization structures
- Manage contact relationships within organization
- Track organization status lifecycle
- Enable organization-level interaction tracking

### 2.4 InteractionLog Aggregate Root

#### AC-CRM-07: Interaction Recording
**Given** contact interaction details  
**When** logging customer interactions  
**Then** the system should:
- Create immutable interaction records
- Link to valid contacts and/or organizations
- Support multiple interaction types
- Enable document attachments via Document Management BC
- Generate follow-up tasks automatically
- Maintain chronological interaction timeline
- Publish InteractionLogged event

#### AC-CRM-08: Follow-up Task Management
**Given** recorded interactions requiring follow-up  
**When** creating follow-up tasks  
**Then** the system should:
- Generate tasks with appropriate due dates
- Assign to interaction owner or specified user
- Set priority based on interaction context
- Link tasks to original interactions
- Support task completion tracking
- Enable recurring follow-up patterns

### 2.5 AutomationWorkflow Aggregate Root

#### AC-CRM-09: Workflow Configuration
**Given** automation requirements and trigger conditions  
**When** configuring automation workflows  
**Then** the system should:
- Validate trigger conditions are testable
- Ensure workflow steps are logically sequenced
- Support multiple action types (email, task, status update)
- Enable workflow testing before activation
- Maintain workflow execution history
- Support workflow versioning and updates

#### AC-CRM-10: Workflow Execution
**Given** an active automation workflow and trigger event  
**When** workflow conditions are met  
**Then** the system should:
- Evaluate trigger conditions accurately
- Execute workflow steps in defined sequence
- Handle step failures gracefully
- Log all execution details for audit
- Support manual intervention if needed
- Publish AutomationWorkflowTriggered event

---

## 3. Contract & Portfolio Management (C&PM) Bounded Context

### 3.1 Contract Aggregate Root

#### AC-CPM-01: Contract Creation and Management
**Given** contract details, parties, and template reference  
**When** creating a new contract  
**Then** the system should:
- Generate unique ContractId
- Validate all parties have complete information
- Ensure minimum two parties are specified
- Link to contract template if provided
- Store contract document via Document Management BC
- Set initial status to "Draft"
- Initialize version control system

#### AC-CPM-02: Contract Versioning
**Given** an existing contract requiring updates  
**When** creating a new contract version  
**Then** the system should:
- Generate sequential version numbers
- Create new document version in Document Management BC
- Maintain complete version history
- Record change notes and metadata
- Preserve all previous versions immutably
- Update contract references to current version
- Publish ContractVersioned event

#### AC-CPM-03: Contract Amendment Process
**Given** an active contract requiring modifications  
**When** adding contract amendments  
**Then** the system should:
- Validate contract is in amendable status
- Require appropriate approval workflow
- Link amendment documents if provided
- Set effective dates for amendments
- Maintain amendment history
- Update contract terms accordingly
- Notify all contract parties

#### AC-CPM-04: Contract Alerts and Notifications
**Given** contract milestones and important dates  
**When** setting up contract alerts  
**Then** the system should:
- Support multiple alert types (renewal, expiration, milestone)
- Calculate trigger dates accurately
- Enable customizable alert timing
- Support multiple notification channels
- Maintain alert history and acknowledgments
- Handle alert escalation if unacknowledged

### 3.2 Portfolio Aggregate Root

#### AC-CPM-05: Portfolio Creation and Structure
**Given** portfolio requirements and initial items  
**When** creating a new portfolio  
**Then** the system should:
- Generate unique PortfolioId
- Validate portfolio goals and objectives
- Initialize performance tracking
- Set up access permissions
- Enable item categorization
- Support sharing and collaboration features
- Publish PortfolioCreated event

#### AC-CPM-06: Portfolio Item Management
**Given** external references to projects, contracts, or other entities  
**When** adding items to portfolio  
**Then** the system should:
- Validate external references exist and are accessible
- Support multiple item types (Project, Contract, Initiative)
- Initialize performance metrics for each item
- Enable categorization and tagging
- Track item dependencies
- Maintain item lifecycle status
- Publish PortfolioItemAdded event

#### AC-CPM-07: Portfolio Performance Calculation
**Given** portfolio items with performance data  
**When** calculating portfolio performance  
**Then** the system should:
- Aggregate metrics across all portfolio items
- Calculate weighted performance based on item importance
- Include budget variance, timeline adherence, quality metrics
- Support custom KPI definitions
- Generate trend analysis over time
- Identify risk factors and alerts
- Update performance dashboards automatically

### 3.3 PortfolioRoadmap Aggregate Root

#### AC-CPM-08: Roadmap Planning
**Given** portfolio strategic objectives and timeline  
**When** creating portfolio roadmap  
**Then** the system should:
- Generate unique RoadmapId
- Link to specific portfolio
- Define strategic milestones with target dates
- Map dependencies between portfolio items
- Validate timeline feasibility
- Support resource requirement planning
- Enable roadmap visualization

#### AC-CPM-09: Cross-Item Dependency Management
**Given** portfolio items with interdependencies  
**When** tracking cross-item dependencies  
**Then** the system should:
- Prevent circular dependency creation
- Support multiple dependency types (finish-to-start, start-to-start, etc.)
- Calculate critical path automatically
- Propagate timeline changes to dependent items
- Alert on dependency conflicts
- Maintain dependency change history

#### AC-CPM-10: Milestone Tracking
**Given** defined roadmap milestones  
**When** tracking milestone progress  
**Then** the system should:
- Monitor milestone status against target dates
- Calculate percentage completion
- Identify at-risk milestones early
- Support milestone completion workflows
- Update dependent milestone timelines
- Publish RoadmapMilestoneCompleted event

---

## 4. Project Execution Bounded Context

### 4.1 Project Aggregate Root

#### AC-PE-01: Project Initialization
**Given** project requirements and team structure  
**When** creating a new project  
**Then** the system should:
- Generate unique ProjectId
- Validate project details and timeline
- Assign initial team members with roles
- Set up project phases if defined
- Initialize resource requirements
- Set project status to "Planning"
- Publish ProjectCreated event
- Create integration references for other BCs

#### AC-PE-02: Project Member Management
**Given** an active project and available team members  
**When** managing project membership  
**Then** the system should:
- Validate users exist and are available
- Assign appropriate project roles
- Set effort allocation percentages
- Ensure at least one member has "Owner" role
- Track member addition/removal history
- Update resource allocation in RIM BC
- Notify team members of project assignment

#### AC-PE-03: Project Status Lifecycle
**Given** a project in any valid status  
**When** updating project status  
**Then** the system should:
- Validate status transitions (Planning → Active → On Hold → Completed/Cancelled)
- Update all related tasks accordingly
- Recalculate resource allocations
- Generate status change notifications
- Update portfolio performance metrics
- Maintain status change audit trail
- Publish appropriate status events

### 4.2 Task Aggregate Root

#### AC-PE-04: Task Creation and Management
**Given** project context and task requirements  
**When** creating new tasks  
**Then** the system should:
- Generate unique TaskId
- Validate task belongs to existing project
- Set appropriate priority and estimates
- Support task hierarchy and dependencies
- Initialize task status to "ToDo"
- Enable assignment to project members
- Track task creation in project timeline

#### AC-PE-05: Task Assignment Process
**Given** available project members and task requirements  
**When** assigning tasks  
**Then** the system should:
- Validate assignee is project member
- Check assignee availability and capacity
- Set effort allocation within limits
- Support skill-based assignment validation
- Track assignment history
- Notify assignee of new tasks
- Publish TaskAssigned event

#### AC-PE-06: Task Dependency Management
**Given** related tasks within project  
**When** setting task dependencies  
**Then** the system should:
- Prevent circular dependency creation
- Support multiple dependency types
- Calculate critical path automatically
- Update dependent task timelines
- Alert on dependency conflicts
- Enable dependency visualization
- Maintain dependency change history

#### AC-PE-07: Task Status Progression
**Given** tasks in various completion states  
**When** updating task status  
**Then** the system should:
- Validate status transitions (ToDo → InProgress → Done/Blocked)
- Update project progress calculations
- Release resources when tasks complete
- Trigger dependent task availability
- Maintain task completion audit trail
- Support task reopening with justification

### 4.3 TimeLog Aggregate Root

#### AC-PE-08: Time Logging Validation
**Given** project tasks and team member assignments  
**When** logging work time  
**Then** the system should:
- Validate user is assigned to project/task
- Ensure log date is not in future
- Limit daily time entries to reasonable amounts (≤24 hours)
- Support both project-level and task-specific logging
- Calculate billable vs. non-billable time
- Publish TimeLogged event

#### AC-PE-09: Time Tracking Analytics
**Given** accumulated time logs across projects  
**When** generating time tracking reports  
**Then** the system should:
- Calculate project time utilization
- Identify over/under-estimated tasks
- Track individual team member productivity
- Generate billing-ready time reports
- Support time-based project forecasting
- Enable resource capacity planning

---

## 5. Resource & Inventory Management (RIM) Bounded Context

### 5.1 ResourceType Aggregate Root

#### AC-RIM-01: Resource Type Definition
**Given** organizational resource categorization needs  
**When** defining new resource types  
**Then** the system should:
- Generate unique ResourceTypeId
- Validate type names are unique within category
- Support hierarchical categorization
- Define custom attribute schemas
- Enable type-specific validation rules
- Support resource type evolution over time

### 5.2 ResourceInstance Aggregate Root

#### AC-RIM-02: Resource Instance Management
**Given** defined resource types and organizational needs  
**When** creating resource instances  
**Then** the system should:
- Generate unique ResourceInstanceId
- Link to valid resource type
- Support human resource linkage via UserId
- Validate custom attributes against type schema
- Initialize availability status
- Enable skill assignment for human resources
- Publish ResourceInstanceAvailable event

#### AC-RIM-03: Resource Allocation Process
**Given** project resource requirements and available resources  
**When** allocating resources to projects  
**Then** the system should:
- Validate resource availability and capacity
- Check allocation percentages don't exceed 100%
- Support partial allocations and shared resources
- Track allocation history and performance
- Update resource availability status
- Publish ResourceAllocated event
- Integrate with Project Execution BC

#### AC-RIM-04: Resource Utilization Tracking
**Given** allocated resources and usage patterns  
**When** tracking resource utilization  
**Then** the system should:
- Monitor actual vs. planned utilization
- Calculate efficiency metrics
- Identify under/over-utilized resources
- Support optimization recommendations
- Generate utilization reports and dashboards
- Enable capacity planning and forecasting

### 5.3 InventoryItemDefinition Aggregate Root

#### AC-RIM-05: Inventory Item Management
**Given** organizational inventory requirements  
**When** defining inventory items  
**Then** the system should:
- Generate unique ItemDefinitionId with SKU
- Ensure SKU uniqueness across system
- Define appropriate units of measure
- Set up stock level tracking by location
- Configure automatic alert thresholds
- Support item categorization and search

#### AC-RIM-06: Stock Level Management
**Given** defined inventory items and storage locations  
**When** managing stock levels  
**Then** the system should:
- Prevent negative stock levels
- Validate unit of measure consistency
- Track all stock movements and adjustments
- Generate automatic alerts for low/high stock
- Support multi-location inventory tracking
- Publish StockLevelAdjusted event

#### AC-RIM-07: Inventory Audit Process
**Given** physical inventory and system records  
**When** conducting inventory audits  
**Then** the system should:
- Compare physical counts with system records
- Identify and report discrepancies
- Support adjustment workflows for variances
- Maintain audit trail and history
- Generate audit reports and analytics
- Publish InventoryAuditCompleted event

### 5.4 ResourceBooking Aggregate Root

#### AC-RIM-08: Resource Booking Management
**Given** bookable resources and scheduling requirements  
**When** creating resource bookings  
**Then** the system should:
- Validate resource availability for requested time slot
- Prevent double-booking of resources
- Support booking confirmation workflows
- Enable booking modifications and cancellations
- Track booking utilization and patterns
- Publish ResourceBooked event

### 5.5 StorageLocation Aggregate Root

#### AC-RIM-09: Location Hierarchy Management
**Given** organizational storage structure  
**When** defining storage locations  
**Then** the system should:
- Generate unique LocationId
- Support hierarchical location structures
- Prevent circular location references
- Track capacity and utilization per location
- Enable location-based inventory tracking
- Support location search and navigation

---

## 6. Document Management Bounded Context

### 6.1 Document Aggregate Root

#### AC-DM-01: Document Upload and Storage
**Given** file content and metadata  
**When** uploading new documents  
**Then** the system should:
- Generate unique DocumentId
- Validate file type and size constraints
- Store file content securely in blob storage
- Extract and store document metadata
- Set up initial access permissions
- Create initial document version
- Publish DocumentUploaded event

#### AC-DM-02: Document Version Control
**Given** existing documents requiring updates  
**When** creating new document versions  
**Then** the system should:
- Generate sequential version numbers
- Maintain complete version history
- Preserve all previous versions immutably
- Record version change notes
- Update document references to latest version
- Publish DocumentVersionCreated event

#### AC-DM-03: Access Control Management
**Given** documents and organizational security requirements  
**When** managing document access  
**Then** the system should:
- Support user-specific and role-based permissions
- Validate permission levels (View, Edit, Admin)
- Enforce permission inheritance and expiration
- Track access grants and revocations
- Audit all document access attempts
- Publish DocumentAccessGranted/Revoked events

#### AC-DM-04: Document Retrieval and Sharing
**Given** authorized users and document requests  
**When** retrieving documents  
**Then** the system should:
- Validate user permissions before access
- Generate secure, time-limited download URLs
- Support version-specific retrieval
- Track document access and downloads
- Enable document sharing with external parties
- Maintain download audit trail

#### AC-DM-05: Document Search and Discovery
**Given** document repositories and search requirements  
**When** searching for documents  
**Then** the system should:
- Index document metadata and content
- Support full-text search capabilities
- Filter results by file type, date, and custom metadata
- Respect user access permissions in search results
- Provide relevant search result ranking
- Enable advanced search with multiple criteria

---

## Cross-Aggregate Validation Rules

### Integration Consistency
- **Document References**: All DocumentId references must point to valid documents in Document Management BC
- **User References**: All UserId references must exist in identity system
- **Project References**: ProjectId references must be consistent across Freelance Operations, Project Execution, and C&PM BCs
- **Resource References**: Resource allocations must be validated against actual resource availability

### Event Publishing Requirements
- All aggregate modifications must publish appropriate domain events
- Events must contain sufficient data for integration scenarios
- Event ordering must be maintained for consistency
- Failed event publishing must not prevent aggregate persistence

### Multi-Tenant Validation
- All aggregates must enforce tenant isolation
- Cross-tenant references are prohibited
- Tenant context must be validated for all operations
- Data export/import must respect tenant boundaries
