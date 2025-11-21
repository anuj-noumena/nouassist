#!/bin/bash

# Script to analyze test coverage for all modules

MODULES=(
    "AssetManagement"
    "Collaboration"
    "Customization"
    "FinancialManagement"
    "IdentityAccess"
    "Infrastructure"
    "Integrations"
    "LearningDevelopment"
    "Notification"
    "PersonalDevelopment"
    "SharedKernel"
    "TimeManagement"
    "WorkManagement"
)

echo "========================================="
echo "TEST COVERAGE ANALYSIS REPORT"
echo "========================================="
echo ""

for MODULE in "${MODULES[@]}"; do
    echo "--- MODULE: $MODULE ---"

    # Count domain entities
    DOMAIN_ENTITIES=$(find modules/$MODULE/src -path "*/Domain/*" -name "*.cs" 2>/dev/null | grep -E "(Aggregates|Entities)/" | wc -l)
    echo "Domain Entities/Aggregates: $DOMAIN_ENTITIES"

    # Count domain tests
    DOMAIN_TESTS=$(find modules/$MODULE/test -path "*Domain.Tests*" -name "*Tests.cs" 2>/dev/null | grep -v "TestBase\|TestModule\|Sample" | wc -l)
    echo "Domain Tests: $DOMAIN_TESTS"

    # Count repositories
    REPOSITORIES=$(find modules/$MODULE/src -path "*/EntityFrameworkCore/*" -name "*Repository.cs" 2>/dev/null | wc -l)
    echo "Repositories: $REPOSITORIES"

    # Count EF tests
    EF_TESTS=$(find modules/$MODULE/test -path "*EntityFrameworkCore.Tests*" -name "*Tests.cs" 2>/dev/null | grep -v "TestBase\|TestModule\|Sample" | wc -l)
    echo "EF Tests: $EF_TESTS"

    # Count app services
    APP_SERVICES=$(find modules/$MODULE/src -path "*/Application/*" -name "*AppService.cs" 2>/dev/null | wc -l)
    echo "App Services: $APP_SERVICES"

    # Count app tests
    APP_TESTS=$(find modules/$MODULE/test -path "*Application.Tests*" -name "*Tests.cs" 2>/dev/null | grep -v "TestBase\|TestModule\|Sample" | wc -l)
    echo "App Tests: $APP_TESTS"

    echo ""
done
