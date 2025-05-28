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
        [InlineData(RemediationActionSeverity.Critical, RemediationActionImpactScope.Global, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.Critical, RemediationActionImpactScope.System, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.Critical, RemediationActionImpactScope.Service, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.Critical, RemediationActionImpactScope.Module, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.Critical, RemediationActionImpactScope.Local, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.Critical, RemediationActionImpactScope.None, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.High, RemediationActionImpactScope.Global, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.High, RemediationActionImpactScope.System, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.High, RemediationActionImpactScope.Service, RemediationRiskLevel.Critical)]
        [InlineData(RemediationActionSeverity.High, RemediationActionImpactScope.Module, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.High, RemediationActionImpactScope.Local, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.High, RemediationActionImpactScope.None, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.Medium, RemediationActionImpactScope.Global, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.Medium, RemediationActionImpactScope.System, RemediationRiskLevel.High)]
        [InlineData(RemediationActionSeverity.Medium, RemediationActionImpactScope.Service, RemediationRiskLevel.Medium)]
        [InlineData(RemediationActionSeverity.Medium, RemediationActionImpactScope.Module, RemediationRiskLevel.Medium)]
        [InlineData(RemediationActionSeverity.Medium, RemediationActionImpactScope.Local, RemediationRiskLevel.Medium)]
        [InlineData(RemediationActionSeverity.Medium, RemediationActionImpactScope.None, RemediationRiskLevel.Medium)]
        [InlineData(RemediationActionSeverity.Low, RemediationActionImpactScope.Global, RemediationRiskLevel.Medium)]
        [InlineData(RemediationActionSeverity.Low, RemediationActionImpactScope.System, RemediationRiskLevel.Medium)]
        [InlineData(RemediationActionSeverity.Low, RemediationActionImpactScope.Service, RemediationRiskLevel.Low)]
        [InlineData(RemediationActionSeverity.Low, RemediationActionImpactScope.Module, RemediationRiskLevel.Low)]
        [InlineData(RemediationActionSeverity.Low, RemediationActionImpactScope.Local, RemediationRiskLevel.Low)]
        [InlineData(RemediationActionSeverity.Low, RemediationActionImpactScope.None, RemediationRiskLevel.Low)]
        [InlineData(RemediationActionSeverity.None, RemediationActionImpactScope.Global, RemediationRiskLevel.None)]
        [InlineData(RemediationActionSeverity.None, RemediationActionImpactScope.System, RemediationRiskLevel.None)]
        [InlineData(RemediationActionSeverity.None, RemediationActionImpactScope.Service, RemediationRiskLevel.None)]
        [InlineData(RemediationActionSeverity.None, RemediationActionImpactScope.Module, RemediationRiskLevel.None)]
        [InlineData(RemediationActionSeverity.None, RemediationActionImpactScope.Local, RemediationRiskLevel.None)]
        [InlineData(RemediationActionSeverity.None, RemediationActionImpactScope.None, RemediationRiskLevel.None)]
        public void CalculateRiskLevel_ReturnsExpectedRiskLevel(
            RemediationActionSeverity severity,
            RemediationActionImpactScope impactScope,
            RemediationRiskLevel expectedRiskLevel)
        {
            // Arrange
            var helper = new RiskAssessmentHelper();

            // Act
            var result = helper.CalculateRiskLevel(severity, impactScope);

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