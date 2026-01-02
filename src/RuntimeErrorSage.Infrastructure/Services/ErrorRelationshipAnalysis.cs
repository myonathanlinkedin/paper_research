using System;
using System.Collections.Generic;
using System.Linq;
using RuntimeErrorSage.Domain.Enums;
using RuntimeErrorSage.Domain.Models.Error;

namespace RuntimeErrorSage.Infrastructure.Services;

/// <summary>
/// Provides core functionality for analyzing relationships between errors.
/// </summary>
public class ErrorRelationshipAnalysis
{
    private readonly ErrorPatternMatcher _patternMatcher;

    public ErrorRelationshipAnalysis(ErrorPatternMatcher patternMatcher)
    {
        _patternMatcher = patternMatcher ?? throw new ArgumentNullException(nameof(patternMatcher));
    }

    /// <summary>
    /// Determines if two components are dependent on each other.
    /// </summary>
    public bool AreComponentsDependent(string component1, string component2)
    {
        // Implementation would check dependency graph or configuration
        return false;
    }

    /// <summary>
    /// Checks if two errors have similar patterns.
    /// </summary>
    public bool HaveSimilarPatterns(ErrorContext error1, ErrorContext error2)
    {
        return _patternMatcher.HaveSimilarPatterns(error1, error2);
    }

    /// <summary>
    /// Checks if two runtime errors have similar patterns.
    /// </summary>
    public bool HaveSimilarPatterns(RuntimeError error1, RuntimeError error2)
    {
        return _patternMatcher.HaveSimilarPatterns(error1, error2);
    }

    /// <summary>
    /// Checks if two errors have a temporal relationship.
    /// </summary>
    public bool HasTemporalRelationship(RuntimeError source, RuntimeError target)
    {
        if (source == null || target == null) return false;
        return Math.Abs((source.Timestamp - target.Timestamp).TotalMinutes) < 5;
    }

    /// <summary>
    /// Checks if two errors have a spatial relationship.
    /// </summary>
    public bool HasSpatialRelationship(RuntimeError source, RuntimeError target)
    {
        if (source == null || target == null) return false;
        return source.ComponentId == target.ComponentId;
    }

    /// <summary>
    /// Checks if two errors have a logical relationship.
    /// </summary>
    public bool HasLogicalRelationship(RuntimeError source, RuntimeError target)
    {
        if (source == null || target == null) return false;
        return source.ErrorType == target.ErrorType;
    }

    /// <summary>
    /// Calculates string similarity between two strings.
    /// </summary>
    private double CalculateStringSimilarity(string s1, string s2)
    {
        if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;
        if (s1 == s2) return 1.0;

        int distance = LevenshteinDistance(s1, s2);
        int maxLength = Math.Max(s1.Length, s2.Length);
        return 1.0 - (double)distance / maxLength;
    }

    /// <summary>
    /// Calculates the Levenshtein distance between two strings.
    /// </summary>
    private int LevenshteinDistance(string s1, string s2)
    {
        int[,] d = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; i++)
            d[i, 0] = i;

        for (int j = 0; j <= s2.Length; j++)
            d[0, j] = j;

        for (int j = 1; j <= s2.Length; j++)
        {
            for (int i = 1; i <= s1.Length; i++)
            {
                if (s1[i - 1] == s2[j - 1])
                    d[i, j] = d[i - 1, j - 1];
                else
                    d[i, j] = Math.Min(Math.Min(
                        d[i - 1, j] + 1,    // deletion
                        d[i, j - 1] + 1),   // insertion
                        d[i - 1, j - 1] + 1 // substitution
                    );
            }
        }

        return d[s1.Length, s2.Length];
    }
} 
