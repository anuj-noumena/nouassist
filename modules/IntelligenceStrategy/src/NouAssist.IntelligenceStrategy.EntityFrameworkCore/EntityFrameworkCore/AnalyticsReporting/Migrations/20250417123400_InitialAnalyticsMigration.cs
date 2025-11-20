using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NouAssist.Analytics.EntityFrameworkCore.Migrations
{
    public partial class InitialAnalyticsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dashboards Table
            migrationBuilder.CreateTable(
                name: "AnalyticsDashboards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    Columns = table.Column<int>(type: "integer", nullable: false),
                    Theme = table.Column<string>(type: "text", nullable: false),
                    AutoRefresh = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshIntervalSeconds = table.Column<int>(type: "integer", nullable: false),
                    CustomCss = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CompactView = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_AnalyticsDashboards", x => x.Id);
                });

            // Dashboard Widgets Table
            migrationBuilder.CreateTable(
                name: "AnalyticsDashboardWidgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DashboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DataSourceId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataQuery = table.Column<string>(type: "text", nullable: false),
                    ChartType = table.Column<int>(type: "integer", nullable: true),
                    ShowLegend = table.Column<bool>(type: "boolean", nullable: false),
                    ShowLabels = table.Column<bool>(type: "boolean", nullable: false),
                    ColorScheme = table.Column<string>(type: "text", nullable: true),
                    DateFormat = table.Column<string>(type: "text", nullable: true),
                    NumberFormat = table.Column<string>(type: "text", nullable: true),
                    AutoRefresh = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshIntervalSeconds = table.Column<int>(type: "integer", nullable: false),
                    AdditionalSettings = table.Column<string>(type: "jsonb", nullable: true),
                    PositionX = table.Column<int>(type: "integer", nullable: false),
                    PositionY = table.Column<int>(type: "integer", nullable: false),
                    Width = table.Column<int>(type: "integer", nullable: false),
                    Height = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsDashboardWidgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsDashboardWidgets_AnalyticsDashboards_DashboardId",
                        column: x => x.DashboardId,
                        principalTable: "AnalyticsDashboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Dashboard Templates Table
            migrationBuilder.CreateTable(
                name: "AnalyticsDashboardTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DashboardId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsDashboardTemplates", x => x.Id);
                });

            // Report Configurations Table
            migrationBuilder.CreateTable(
                name: "AnalyticsReportConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    DefaultFormat = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CronExpression = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NextRunTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastRunTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmailRecipients = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    StoreInRepository = table.Column<bool>(type: "boolean", nullable: false),
                    DataSources = table.Column<string>(type: "jsonb", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_AnalyticsReportConfigurations", x => x.Id);
                });

            // Report Parameters Table
            migrationBuilder.CreateTable(
                name: "AnalyticsReportParameters",
                columns: table => new
                {
                    ReportConfigurationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DataType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DefaultValue = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    ValidationRegex = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsReportParameters", x => new { x.ReportConfigurationId, x.Name });
                    table.ForeignKey(
                        name: "FK_AnalyticsReportParameters_AnalyticsReportConfigurations_ReportConfigurationId",
                        column: x => x.ReportConfigurationId,
                        principalTable: "AnalyticsReportConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Report Generation Logs Table
            migrationBuilder.CreateTable(
                name: "AnalyticsReportGenerationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportConfigurationId = table.Column<Guid>(type: "uuid", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GeneratedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    Format = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    StoragePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsScheduled = table.Column<bool>(type: "boolean", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsReportGenerationLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsReportGenerationLogs_AnalyticsReportConfigurations_ReportConfigurationId",
                        column: x => x.ReportConfigurationId,
                        principalTable: "AnalyticsReportConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // KPI Definitions Table
            migrationBuilder.CreateTable(
                name: "AnalyticsKpiDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    CalculationExpression = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CurrentValue = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataSources = table.Column<string>(type: "jsonb", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_AnalyticsKpiDefinitions", x => x.Id);
                });

            // KPI Thresholds Table
            migrationBuilder.CreateTable(
                name: "AnalyticsKpiThresholds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KpiDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ThresholdValue = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    IsUpperBound = table.Column<bool>(type: "boolean", nullable: false),
                    StatusWhenBreached = table.Column<int>(type: "integer", nullable: false),
                    Severity = table.Column<int>(type: "integer", nullable: false),
                    ConditionExpression = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ConsecutiveBreaches = table.Column<int>(type: "integer", nullable: false),
                    SuppressMinutes = table.Column<int>(type: "integer", nullable: false),
                    NotifyOnBackToNormal = table.Column<bool>(type: "boolean", nullable: false),
                    NotificationMessage = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsKpiThresholds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsKpiThresholds_AnalyticsKpiDefinitions_KpiDefinitionId",
                        column: x => x.KpiDefinitionId,
                        principalTable: "AnalyticsKpiDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // KPI Alert Logs Table
            migrationBuilder.CreateTable(
                name: "AnalyticsKpiAlertLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KpiDefinitionId = table.Column<Guid>(type: "uuid", nullable: false),
                    KpiValue = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    ThresholdId = table.Column<Guid>(type: "uuid", nullable: false),
                    ThresholdValue = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsKpiAlertLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsKpiAlertLogs_AnalyticsKpiDefinitions_KpiDefinitionId",
                        column: x => x.KpiDefinitionId,
                        principalTable: "AnalyticsKpiDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Data Sources Table
            migrationBuilder.CreateTable(
                name: "AnalyticsDataSources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ConnectionString = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CredentialsUsername = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CredentialsPassword = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CredentialsApiKey = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CredentialsTokenType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CredentialsSecrets = table.Column<string>(type: "jsonb", nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
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
                    table.PrimaryKey("PK_AnalyticsDataSources", x => x.Id);
                });

            // Data Transformation Steps Table
            migrationBuilder.CreateTable(
                name: "AnalyticsDataTransformationSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TransformationType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Configuration = table.Column<string>(type: "jsonb", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsDataTransformationSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsDataTransformationSteps_AnalyticsDataSources_DataSourceId",
                        column: x => x.DataSourceId,
                        principalTable: "AnalyticsDataSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Aggregated Data Views Table
            migrationBuilder.CreateTable(
                name: "AnalyticsAggregatedDataViews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DataSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ViewDefinition = table.Column<string>(type: "jsonb", nullable: false),
                    RefreshStrategy = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastRefreshed = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsAggregatedDataViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnalyticsAggregatedDataViews_AnalyticsDataSources_DataSourceId",
                        column: x => x.DataSourceId,
                        principalTable: "AnalyticsDataSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Create Indexes
            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsAggregatedDataViews_DataSourceId",
                table: "AnalyticsAggregatedDataViews",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsAggregatedDataViews_Name",
                table: "AnalyticsAggregatedDataViews",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDashboardTemplates_TenantId_Name",
                table: "AnalyticsDashboardTemplates",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDashboards_TenantId_IsDefault",
                table: "AnalyticsDashboards",
                columns: new[] { "TenantId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDashboards_TenantId_Name",
                table: "AnalyticsDashboards",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDashboardWidgets_DashboardId",
                table: "AnalyticsDashboardWidgets",
                column: "DashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDataSources_TenantId_Name",
                table: "AnalyticsDataSources",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDataSources_TenantId_Type_IsActive",
                table: "AnalyticsDataSources",
                columns: new[] { "TenantId", "Type", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDataTransformationSteps_DataSourceId",
                table: "AnalyticsDataTransformationSteps",
                column: "DataSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsDataTransformationSteps_Order",
                table: "AnalyticsDataTransformationSteps",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsKpiAlertLogs_KpiDefinitionId",
                table: "AnalyticsKpiAlertLogs",
                column: "KpiDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsKpiAlertLogs_OccurredAt",
                table: "AnalyticsKpiAlertLogs",
                column: "OccurredAt");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsKpiDefinitions_TenantId_Name",
                table: "AnalyticsKpiDefinitions",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsKpiDefinitions_TenantId_Status",
                table: "AnalyticsKpiDefinitions",
                columns: new[] { "TenantId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsKpiThresholds_KpiDefinitionId",
                table: "AnalyticsKpiThresholds",
                column: "KpiDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsReportConfigurations_TenantId_Name",
                table: "AnalyticsReportConfigurations",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsReportGenerationLogs_ReportConfigurationId",
                table: "AnalyticsReportGenerationLogs",
                column: "ReportConfigurationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyticsAggregatedDataViews");

            migrationBuilder.DropTable(
                name: "AnalyticsDashboardTemplates");

            migrationBuilder.DropTable(
                name: "AnalyticsDashboardWidgets");

            migrationBuilder.DropTable(
                name: "AnalyticsDataTransformationSteps");

            migrationBuilder.DropTable(
                name: "AnalyticsKpiAlertLogs");

            migrationBuilder.DropTable(
                name: "AnalyticsKpiThresholds");

            migrationBuilder.DropTable(
                name: "AnalyticsReportGenerationLogs");

            migrationBuilder.DropTable(
                name: "AnalyticsReportParameters");

            migrationBuilder.DropTable(
                name: "AnalyticsDashboards");

            migrationBuilder.DropTable(
                name: "AnalyticsDataSources");

            migrationBuilder.DropTable(
                name: "AnalyticsKpiDefinitions");

            migrationBuilder.DropTable(
                name: "AnalyticsReportConfigurations");
        }
    }
}
