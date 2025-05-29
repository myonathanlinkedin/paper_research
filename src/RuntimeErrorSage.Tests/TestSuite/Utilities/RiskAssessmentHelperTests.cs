using System.Collections.ObjectModel;
using System;
using System.Linq;
using Xunit;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Core.Storage.Utilities;

namespace RuntimeErrorSage.Tests.TestSuite.Utilities
{
    public class RiskAssessmentHelperTests
    {
        [Theory]
        [InlineData(SeverityLevel.Critical, RemediationActionImpactScope.Global, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.Critical, RemediationActionImpactScope.System, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.Critical, RemediationActionImpactScope.Service, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.Critical, RemediationActionImpactScope.Module, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.Critical, RemediationActionImpactScope.Local, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.Critical, RemediationActionImpactScope.None, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.High, RemediationActionImpactScope.Global, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.High, RemediationActionImpactScope.System, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.High, RemediationActionImpactScope.Service, RemediationRiskLevel.Critical)]
        [InlineData(SeverityLevel.High, RemediationActionImpactScope.Module, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.High, RemediationActionImpactScope.Local, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.High, RemediationActionImpactScope.None, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.Medium, RemediationActionImpactScope.Global, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.Medium, RemediationActionImpactScope.System, RemediationRiskLevel.High)]
        [InlineData(SeverityLevel.Medium, RemediationActionImpactScope.Service, RemediationRiskLevel.Medium)]
        [InlineData(SeverityLevel.Medium, RemediationActionImpactScope.Module, RemediationRiskLevel.Medium)]
        [InlineData(SeverityLevel.Medium, RemediationActionImpactScope.Local, RemediationRiskLevel.Medium)]
        [InlineData(SeverityLevel.Medium, RemediationActionImpactScope.None, RemediationRiskLevel.Medium)]
        [InlineData(SeverityLevel.Low, RemediationActionImpactScope.Global, RemediationRiskLevel.Medium)]
        [InlineData(SeverityLevel.Low, RemediationActionImpactScope.System, RemediationRiskLevel.Medium)]
        [InlineData(SeverityLevel.Low, RemediationActionImpactScope.Service, RemediationRiskLevel.Low)]
        [InlineData(SeverityLevel.Low, RemediationActionImpactScope.Module, RemediationRiskLevel.Low)]
        [InlineData(SeverityLevel.Low, RemediationActionImpactScope.Local, RemediationRiskLevel.Low)]
        [InlineData(SeverityLevel.Low, RemediationActionImpactScope.None, RemediationRiskLevel.Low)]
        [InlineData(SeverityLevel.Info, RemediationActionImpactScope.Global, RemediationRiskLevel.None)]
        [InlineData(SeverityLevel.Info, RemediationActionImpactScope.System, RemediationRiskLevel.None)]
        [InlineData(SeverityLevel.Info, RemediationActionImpactScope.Service, RemediationRiskLevel.None)]
        [InlineData(SeverityLevel.Info, RemediationActionImpactScope.Module, RemediationRiskLevel.None)]
        [InlineData(SeverityLevel.Info, RemediationActionImpactScope.Local, RemediationRiskLevel.None)]
        [InlineData(SeverityLevel.Info, RemediationActionImpactScope.None, RemediationRiskLevel.None)]
        public 
            SeverityLevel severity,
            RemediationActionImpactScope impactScope,
            RemediationRiskLevel expectedRiskLevel { ArgumentNullException.ThrowIfNull(
            SeverityLevel severity,
            RemediationActionImpactScope impactScope,
            RemediationRiskLevel expectedRiskLevel); }
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
        public RemediationRiskLevel riskLevel { ArgumentNullException.ThrowIfNull(RemediationRiskLevel riskLevel); }
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
        public RemediationRiskLevel riskLevel { ArgumentNullException.ThrowIfNull(RemediationRiskLevel riskLevel); }
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





