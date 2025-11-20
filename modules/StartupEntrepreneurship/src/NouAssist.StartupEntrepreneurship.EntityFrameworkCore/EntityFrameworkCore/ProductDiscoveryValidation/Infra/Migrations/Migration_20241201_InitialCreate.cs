namespace NouAssist.StartupEntrepreneurship.ProductDiscoveryValidation.Migrations
{
    /// <summary>
    /// Initial migration for Product Discovery module
    /// </summary>
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Customer Segments table
            migrationBuilder.CreateTable(
                name: "ProductDiscoveryCustomerSegments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TargetSampleSize = table.Column<int>(type: "integer", nullable: false),
                    ActualSampleSize = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CriteriaMinimumSampleSize = table.Column<int>(type: "integer", nullable: false),
                    CriteriaSettings = table.Column<string>(type: "jsonb", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscoveryCustomerSegments", x => x.Id);
                });

            // Customer Interviews table
            migrationBuilder.CreateTable(
                name: "ProductDiscoveryCustomerInterviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InterviewerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerEmail = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CustomerSegmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    GuideObjective = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    GuideEstimatedDuration = table.Column<int>(type: "integer", nullable: false),
                    GuideQuestions = table.Column<string>(type: "jsonb", nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscoveryCustomerInterviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscoveryCustomerInterviews_ProductDiscoveryCustom~",
                        column: x => x.CustomerSegmentId,
                        principalTable: "ProductDiscoveryCustomerSegments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Problem Hypotheses table
            migrationBuilder.CreateTable(
                name: "ProductDiscoveryProblemHypotheses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Context = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    RelatedCustomerSegmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ValidatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidationSummary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    StatementProblem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StatementTarget = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StatementSolution = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    StatementExpectedOutcome = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FitProblemClarity = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    FitSolutionEffectiveness = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    FitCustomerValidation = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    FitMarketDemand = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    FitNotes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscoveryProblemHypotheses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscoveryProblemHypotheses_ProductDiscoveryCustomer~",
                        column: x => x.RelatedCustomerSegmentId,
                        principalTable: "ProductDiscoveryCustomerSegments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Solution Prototypes table
            migrationBuilder.CreateTable(
                name: "ProductDiscoverySolutionPrototypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RelatedHypothesisId = table.Column<Guid>(type: "uuid", nullable: true),
                    Purpose = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TargetAudience = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscoverySolutionPrototypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscoverySolutionPrototypes_ProductDiscoveryProblem~",
                        column: x => x.RelatedHypothesisId,
                        principalTable: "ProductDiscoveryProblemHypotheses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Feature Definitions table
            migrationBuilder.CreateTable(
                name: "ProductDiscoveryFeatureDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    UserStory = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    AcceptanceCriteria = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Priority = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MvpStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    RelatedHypothesisId = table.Column<Guid>(type: "uuid", nullable: true),
                    RelatedPrototypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    BusinessValue = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TechnicalNotes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscoveryFeatureDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscoveryFeatureDefinitions_ProductDiscoveryProblem~",
                        column: x => x.RelatedHypothesisId,
                        principalTable: "ProductDiscoveryProblemHypotheses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ProductDiscoveryFeatureDefinitions_ProductDiscoverySolution~",
                        column: x => x.RelatedPrototypeId,
                        principalTable: "ProductDiscoverySolutionPrototypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            // Continue with child tables...
            // Interview Transcripts
            migrationBuilder.CreateTable(
                name: "ProductDiscoveryInterviewTranscripts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InterviewId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsProcessed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDiscoveryInterviewTranscripts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDiscoveryInterviewTranscripts_ProductDiscoveryCustom~",
                        column: x => x.InterviewId,
                        principalTable: "ProductDiscoveryCustomerInterviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Continue with all indexes...
            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscoveryCustomerSegments_Name",
                table: "ProductDiscoveryCustomerSegments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscoveryCustomerInterviews_ScheduledDate",
                table: "ProductDiscoveryCustomerInterviews",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscoveryCustomerInterviews_Status",
                table: "ProductDiscoveryCustomerInterviews",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDiscoveryCustomerInterviews_CustomerSegmentId",
                table: "ProductDiscoveryCustomerInterviews",
                column: "CustomerSegmentId");

            // Add more indexes for other tables as needed...
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop tables in reverse order
            migrationBuilder.DropTable("ProductDiscoveryInterviewTranscripts");
            migrationBuilder.DropTable("ProductDiscoveryInsights");
            migrationBuilder.DropTable("ProductDiscoveryValidationAttempts");
            migrationBuilder.DropTable("ProductDiscoveryPrototypeVersions");
            migrationBuilder.DropTable("ProductDiscoveryFeedbackData");
            migrationBuilder.DropTable("ProductDiscoveryPriorityScores");
            migrationBuilder.DropTable("ProductDiscoveryUserFeedbackSessions");
            migrationBuilder.DropTable("ProductDiscoveryFeatureDefinitions");
            migrationBuilder.DropTable("ProductDiscoverySolutionPrototypes");
            migrationBuilder.DropTable("ProductDiscoveryProblemHypotheses");
            migrationBuilder.DropTable("ProductDiscoveryCustomerInterviews");
            migrationBuilder.DropTable("ProductDiscoveryCustomerSegments");
        }
    }
}
