using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NouAssist.Migrations
{
    /// <inheritdoc />
    public partial class tasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks_TaskInstances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkflowStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssigneeId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReporterId = table.Column<Guid>(type: "uuid", nullable: true),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ApprovalRequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequiresApproval = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    TaskStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProgressPercent = table.Column<double>(type: "double precision", nullable: false),
                    Importance = table.Column<double>(type: "double precision", nullable: false),
                    Urgency = table.Column<double>(type: "double precision", nullable: false),
                    EisenhowerQuadrant = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DelegatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    DelegatedTo = table.Column<Guid>(type: "uuid", nullable: false),
                    DelegationStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DelegatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DelegationRespondedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DelegationNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DelegationResponseNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    PlannedStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PlannedDueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualDueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ActualMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    LastTrackedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OrderInParent = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ParentInstanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubtasksCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    SubtasksDoneCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CommentsCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    AttachmentsCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Watchers = table.Column<string>(type: "jsonb", nullable: false),
                    IsDiscussionLocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsNextAction = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BlockedReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    BlockedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsFlagged = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FlaggedReason = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Resolution = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ResolutionNote = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CustomFieldValues = table.Column<Dictionary<string, object>>(type: "jsonb", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_TaskInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: true),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Notes = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    ExpectedResult = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    DefaultImportance = table.Column<double>(type: "double precision", nullable: false),
                    DefaultUrgency = table.Column<double>(type: "double precision", nullable: false),
                    DefaultContextId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultAreaId = table.Column<Guid>(type: "uuid", nullable: true),
                    DefaultSprintId = table.Column<Guid>(type: "uuid", nullable: true),
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    EstimateMinutes = table.Column<int>(type: "integer", nullable: true),
                    DefaultDueDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    RecurrenceFrequency = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RecurrenceInterval = table.Column<int>(type: "integer", nullable: false),
                    RecurrenceEndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    RecurrenceCount = table.Column<int>(type: "integer", nullable: true),
                    RecurrenceDaysOfWeek = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_TaskTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_TaskTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_AcceptanceCriterionCompletions",
                columns: table => new
                {
                    TaskInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CriterionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsMet = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MetAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    MetBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_AcceptanceCriterionCompletions", x => new { x.TaskInstanceId, x.CriterionId });
                    table.ForeignKey(
                        name: "FK_Tasks_AcceptanceCriterionCompletions_Tasks_TaskInstances_Ta~",
                        column: x => x.TaskInstanceId,
                        principalTable: "Tasks_TaskInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_TaskActionStepCompletions",
                columns: table => new
                {
                    TaskInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskActionStepId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CompletedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_TaskActionStepCompletions", x => new { x.TaskInstanceId, x.TaskActionStepId });
                    table.ForeignKey(
                        name: "FK_Tasks_TaskActionStepCompletions_Tasks_TaskInstances_TaskIns~",
                        column: x => x.TaskInstanceId,
                        principalTable: "Tasks_TaskInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_TaskDependencies",
                columns: table => new
                {
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependsOnTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependsOnTaskInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependencyType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LagMinutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsBlocking = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_TaskDependencies", x => new { x.TaskId, x.TaskInstanceId, x.DependsOnTaskId, x.DependsOnTaskInstanceId });
                    table.ForeignKey(
                        name: "FK_Tasks_TaskDependencies_Tasks_TaskInstances_TaskInstanceId",
                        column: x => x.TaskInstanceId,
                        principalTable: "Tasks_TaskInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_AcceptanceCriteria",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    GivenContext = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    WhenAction = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ThenOutcome = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TestScenario = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_AcceptanceCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_AcceptanceCriteria_Tasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks_Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_TaskActionSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Sequence = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_TaskActionSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskActionSteps_Tasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks_Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks_TaskTypeCustomFields",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TaskTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomFieldDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DefaultValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    TabName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SectionName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShowIfFieldKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShowIfValue = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks_TaskTypeCustomFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_TaskTypeCustomFields_Tasks_TaskTypes_TaskTypeId",
                        column: x => x.TaskTypeId,
                        principalTable: "Tasks_TaskTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AcceptanceCriteria_TaskId",
                table: "Tasks_AcceptanceCriteria",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AcceptanceCriteria_TaskId_Priority",
                table: "Tasks_AcceptanceCriteria",
                columns: new[] { "TaskId", "Priority" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AcceptanceCriterionCompletions_CriterionId",
                table: "Tasks_AcceptanceCriterionCompletions",
                column: "CriterionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AcceptanceCriterionCompletions_TaskInstanceId",
                table: "Tasks_AcceptanceCriterionCompletions",
                column: "TaskInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AcceptanceCriterionCompletions_TaskInstanceId_IsMet",
                table: "Tasks_AcceptanceCriterionCompletions",
                columns: new[] { "TaskInstanceId", "IsMet" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskActionStepCompletions_TaskActionStepId",
                table: "Tasks_TaskActionStepCompletions",
                column: "TaskActionStepId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskActionStepCompletions_TaskInstanceId",
                table: "Tasks_TaskActionStepCompletions",
                column: "TaskInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskActionStepCompletions_TaskInstanceId_IsDone",
                table: "Tasks_TaskActionStepCompletions",
                columns: new[] { "TaskInstanceId", "IsDone" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskActionSteps_TaskId",
                table: "Tasks_TaskActionSteps",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskActionSteps_TaskId_Sequence",
                table: "Tasks_TaskActionSteps",
                columns: new[] { "TaskId", "Sequence" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskDependencies_DependsOnTaskId",
                table: "Tasks_TaskDependencies",
                column: "DependsOnTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskDependencies_DependsOnTaskInstanceId",
                table: "Tasks_TaskDependencies",
                column: "DependsOnTaskInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskDependencies_TaskId",
                table: "Tasks_TaskDependencies",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskDependencies_TaskInstanceId",
                table: "Tasks_TaskDependencies",
                column: "TaskInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskDependencies_TaskInstanceId_IsBlocking",
                table: "Tasks_TaskDependencies",
                columns: new[] { "TaskInstanceId", "IsBlocking" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_ActualDueDate",
                table: "Tasks_TaskInstances",
                column: "ActualDueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_AssigneeId",
                table: "Tasks_TaskInstances",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_CompletedAt",
                table: "Tasks_TaskInstances",
                column: "CompletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_IsBlocked",
                table: "Tasks_TaskInstances",
                column: "IsBlocked");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_IsNextAction",
                table: "Tasks_TaskInstances",
                column: "IsNextAction");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_OwnerId",
                table: "Tasks_TaskInstances",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_ParentInstanceId",
                table: "Tasks_TaskInstances",
                column: "ParentInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_PlannedDueDate",
                table: "Tasks_TaskInstances",
                column: "PlannedDueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_ProjectId",
                table: "Tasks_TaskInstances",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_TaskId",
                table: "Tasks_TaskInstances",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_TaskStatus",
                table: "Tasks_TaskInstances",
                column: "TaskStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_TenantId",
                table: "Tasks_TaskInstances",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_TenantId_IsDeleted",
                table: "Tasks_TaskInstances",
                columns: new[] { "TenantId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskInstances_WorkflowStatusId",
                table: "Tasks_TaskInstances",
                column: "WorkflowStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_IsActive",
                table: "Tasks_Tasks",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_IsRecurring",
                table: "Tasks_Tasks",
                column: "IsRecurring");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_OwnerId",
                table: "Tasks_Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_ProjectId",
                table: "Tasks_Tasks",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_TenantId",
                table: "Tasks_Tasks",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_TenantId_IsDeleted",
                table: "Tasks_Tasks",
                columns: new[] { "TenantId", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_Tasks_Title",
                table: "Tasks_Tasks",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeCustomFields_CustomFieldDefinitionId",
                table: "Tasks_TaskTypeCustomFields",
                column: "CustomFieldDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeCustomFields_TaskTypeId",
                table: "Tasks_TaskTypeCustomFields",
                column: "TaskTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeCustomFields_TaskTypeId_CustomFieldDefinition~",
                table: "Tasks_TaskTypeCustomFields",
                columns: new[] { "TaskTypeId", "CustomFieldDefinitionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeCustomFields_TaskTypeId_DisplayOrder",
                table: "Tasks_TaskTypeCustomFields",
                columns: new[] { "TaskTypeId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypes_IsActive",
                table: "Tasks_TaskTypes",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypes_Name",
                table: "Tasks_TaskTypes",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypes_TenantId",
                table: "Tasks_TaskTypes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypes_TenantId_Name",
                table: "Tasks_TaskTypes",
                columns: new[] { "TenantId", "Name" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks_AcceptanceCriteria");

            migrationBuilder.DropTable(
                name: "Tasks_AcceptanceCriterionCompletions");

            migrationBuilder.DropTable(
                name: "Tasks_TaskActionStepCompletions");

            migrationBuilder.DropTable(
                name: "Tasks_TaskActionSteps");

            migrationBuilder.DropTable(
                name: "Tasks_TaskDependencies");

            migrationBuilder.DropTable(
                name: "Tasks_TaskTypeCustomFields");

            migrationBuilder.DropTable(
                name: "Tasks_Tasks");

            migrationBuilder.DropTable(
                name: "Tasks_TaskInstances");

            migrationBuilder.DropTable(
                name: "Tasks_TaskTypes");
        }
    }
}
