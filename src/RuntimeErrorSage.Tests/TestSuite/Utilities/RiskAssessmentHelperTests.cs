using System;
using System.Linq;
using Xunit;
using RuntimeErrorSage.Core.Models.Enums;
using RuntimeErrorSage.Core.Utilities;

namespace RuntimeErrorSage.Tests.TestSuite.Utilities
{
    public class RiskAssessmentHelperTests
    {
        [Theory]
        [InlineData(RemediationSeverity.Critical, RemediationActionImpactScope.Global, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.Critical, RemediationActionImpactScope.System, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.Critical, RemediationActionImpactScope.Service, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.Critical, RemediationActionImpactScope.Module, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.Critical, RemediationActionImpactScope.Local, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.Critical, RemediationActionImpactScope.None, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.High, RemediationActionImpactScope.Global, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.High, RemediationActionImpactScope.System, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.High, RemediationActionImpactScope.Service, RemediationRiskLevel.Critical)]
        [InlineData(RemediationSeverity.High, RemediationActionImpactScope.Module, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.High, RemediationActionImpactScope.Local, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.High, RemediationActionImpactScope.None, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.Medium, RemediationActionImpactScope.Global, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.Medium, RemediationActionImpactScope.System, RemediationRiskLevel.High)]
        [InlineData(RemediationSeverity.Medium, RemediationActionImpactScope.Service, RemediationRiskLevel.Medium)]
        [InlineData(RemediationSeverity.Medium, RemediationActionImpactScope.Module, RemediationRiskLevel.Medium)]
        [InlineData(RemediationSeverity.Medium, RemediationActionImpactScope.Local, RemediationRiskLevel.Medium)]
        [InlineData(RemediationSeverity.Medium, RemediationActionImpactScope.None, RemediationRiskLevel.Medium)]
        [InlineData(RemediationSeverity.Low, RemediationActionImpactScope.Global, RemediationRiskLevel.Medium)]
        [InlineData(RemediationSeverity.Low, RemediationActionImpactScope.System, RemediationRiskLevel.Low)]
        [InlineData(RemediationSeverity.Low, RemediationActionImpactScope.Service, RemediationRiskLevel.Low)]
        [InlineData(RemediationSeverity.Low, RemediationActionImpactScope.Module, RemediationRiskLevel.Low)]
        [InlineData(RemediationSeverity.Low, RemediationActionImpactScope.Local, RemediationRiskLevel.Low)]
        [InlineData(RemediationSeverity.Low, RemediationActionImpactScope.None, RemediationRiskLevel.Low)]
        [InlineData(RemediationSeverity.None, RemediationActionImpactScope.Global, RemediationRiskLevel.None)]
        [InlineData(RemediationSeverity.None, RemediationActionImpactScope.System, RemediationRiskLevel.None)]
        [InlineData(RemediationSeverity.None, RemediationActionImpactScope.Service, RemediationRiskLevel.None)]
        [InlineData(RemediationSeverity.None, RemediationActionImpactScope.Module, RemediationRiskLevel.None)]
        [InlineData(RemediationSeverity.None, RemediationActionImpactScope.Local, RemediationRiskLevel.None)]
        [InlineData(RemediationSeverity.None, RemediationActionImpactScope.None, RemediationRiskLevel.None)]
        public void CalculateRiskLevel_WithSeverityAndScope_ReturnsExpectedRiskLevel(
            RemediationSeverity severity, 
            RemediationActionImpactScope impactScope, 
            RemediationRiskLevel expectedRiskLevel)
        {
            // Act
            var result = RiskAssessmentHelper.CalculateRiskLevel(severity, impactScope);

            // Assert
            Assert.Equal(expectedRiskLevel, result);
        }

        [Theory]
        [InlineData(RemediationRiskLevel.Critical)]
        [InlineData(RemediationRiskLevel.High)]
        [InlineData(RemediationRiskLevel.Medium)]
        [InlineData(RemediationRiskLevel.Low)]
        [InlineData(RemediationRiskLevel.None)]
        [InlineData(RemediationRiskLevel.Unknown)]
        public void GeneratePotentialIssues_WithRiskLevel_ReturnsNonEmptyList(RemediationRiskLevel riskLevel)
        {
            // Act
            var result = RiskAssessmentHelper.GeneratePotentialIssues(riskLevel);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GeneratePotentialIssues_WithCriticalRiskLevel_ReturnsSpecificIssues()
        {
            // Act
            var result = RiskAssessmentHelper.GeneratePotentialIssues(RemediationRiskLevel.Critical);

            // Assert
            Assert.Contains(result, issue => issue.Contains("system downtime"));
            Assert.Contains(result, issue => issue.Contains("dependent services"));
            Assert.Contains(result, issue => issue.Contains("manual intervention"));
            Assert.Contains(result, issue => issue.Contains("Data loss"));
        }

        [Fact]
        public void GeneratePotentialIssues_WithHighRiskLevel_ReturnsSpecificIssues()
        {
            // Act
            var result = RiskAssessmentHelper.GeneratePotentialIssues(RemediationRiskLevel.High);

            // Assert
            Assert.Contains(result, issue => issue.Contains("service disruption"));
            Assert.Contains(result, issue => issue.Contains("related components"));
            Assert.Contains(result, issue => issue.Contains("monitoring"));
        }

        [Fact]
        public void GeneratePotentialIssues_WithMediumRiskLevel_ReturnsSpecificIssues()
        {
            // Act
            var result = RiskAssessmentHelper.GeneratePotentialIssues(RemediationRiskLevel.Medium);

            // Assert
            Assert.Contains(result, issue => issue.Contains("minor service disruption"));
            Assert.Contains(result, issue => issue.Contains("specific functionality"));
            Assert.Contains(result, issue => issue.Contains("validation"));
        }

        [Fact]
        public void GeneratePotentialIssues_WithLowRiskLevel_ReturnsSpecificIssues()
        {
            // Act
            var result = RiskAssessmentHelper.GeneratePotentialIssues(RemediationRiskLevel.Low);

            // Assert
            Assert.Contains(result, issue => issue.Contains("Minimal impact"));
            Assert.Contains(result, issue => issue.Contains("specific component"));
        }

        [Fact]
        public void GeneratePotentialIssues_WithNoneRiskLevel_ReturnsSpecificIssues()
        {
            // Act
            var result = RiskAssessmentHelper.GeneratePotentialIssues(RemediationRiskLevel.None);

            // Assert
            Assert.Contains(result, issue => issue.Contains("No significant impact"));
        }

        [Theory]
        [InlineData(RemediationRiskLevel.Critical)]
        [InlineData(RemediationRiskLevel.High)]
        [InlineData(RemediationRiskLevel.Medium)]
        [InlineData(RemediationRiskLevel.Low)]
        [InlineData(RemediationRiskLevel.None)]
        [InlineData(RemediationRiskLevel.Unknown)]
        public void GenerateMitigationSteps_WithRiskLevel_ReturnsNonEmptyList(RemediationRiskLevel riskLevel)
        {
            // Act
            var result = RiskAssessmentHelper.GenerateMitigationSteps(riskLevel);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void GenerateMitigationSteps_AlwaysIncludesCommonSteps()
        {
            // Act
            var criticalResult = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.Critical);
            var lowResult = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.Low);
            var noneResult = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.None);

            // Assert
            Assert.Contains("Verify system state before execution", criticalResult);
            Assert.Contains("Verify system state before execution", lowResult);
            Assert.Contains("Verify system state before execution", noneResult);
            
            Assert.Contains("Validate system state after execution", criticalResult);
            Assert.Contains("Validate system state after execution", lowResult);
            Assert.Contains("Validate system state after execution", noneResult);
        }

        [Fact]
        public void GenerateMitigationSteps_WithCriticalRiskLevel_ReturnsSpecificSteps()
        {
            // Act
            var result = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.Critical);

            // Assert
            Assert.Contains(result, step => step.Contains("full system backup"));
            Assert.Contains(result, step => step.Contains("maintenance window"));
            Assert.Contains(result, step => step.Contains("Notify all stakeholders"));
            Assert.Contains(result, step => step.Contains("rollback plan"));
            Assert.Contains(result, step => step.Contains("emergency response team"));
        }

        [Fact]
        public void GenerateMitigationSteps_WithHighRiskLevel_ReturnsSpecificSteps()
        {
            // Act
            var result = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.High);

            // Assert
            Assert.Contains(result, step => step.Contains("targeted backup"));
            Assert.Contains(result, step => step.Contains("key stakeholders"));
            Assert.Contains(result, step => step.Contains("rollback procedure"));
            Assert.Contains(result, step => step.Contains("Monitor system"));
            Assert.Contains(result, step => step.Contains("staging environment"));
        }

        [Fact]
        public void GenerateMitigationSteps_WithMediumRiskLevel_ReturnsSpecificSteps()
        {
            // Act
            var result = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.Medium);

            // Assert
            Assert.Contains(result, step => step.Contains("Backup affected configuration"));
            Assert.Contains(result, step => step.Contains("service owners"));
            Assert.Contains(result, step => step.Contains("Document current state"));
            Assert.Contains(result, step => step.Contains("Monitor affected components"));
        }

        [Fact]
        public void GenerateMitigationSteps_WithLowRiskLevel_ReturnsSpecificSteps()
        {
            // Act
            var result = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.Low);

            // Assert
            Assert.Contains(result, step => step.Contains("Document changes"));
            Assert.Contains(result, step => step.Contains("Verify functionality"));
        }

        [Fact]
        public void GenerateMitigationSteps_WithNoneRiskLevel_ReturnsSpecificSteps()
        {
            // Act
            var result = RiskAssessmentHelper.GenerateMitigationSteps(RemediationRiskLevel.None);

            // Assert
            Assert.Contains(result, step => step.Contains("Standard execution"));
        }
    }
} 