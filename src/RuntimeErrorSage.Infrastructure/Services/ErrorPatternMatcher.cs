using System.Collections.ObjectModel;
using System;
using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Application.Services;

/// <summary>
/// Provides functionality for matching and comparing error patterns.
/// </summary>
public class ErrorPatternMatcher
{
    /// <summary>
    /// Checks if two errors have similar patterns.
    /// </summary>
    public bool HaveSimilarPatterns(ErrorContext error1, ErrorContext error2)
    {
        if (error1 == null || error2 == null) return false;
        return CalculateStringSimilarity(error1.ErrorMessage, error2.ErrorMessage) > 0.8;
    }

    /// <summary>
    /// Checks if two runtime errors have similar patterns.
    /// </summary>
    public bool HaveSimilarPatterns(RuntimeError error1, RuntimeError error2)
    {
        if (error1 == null || error2 == null) return false;
        return CalculateStringSimilarity(error1.Message, error2.Message) > 0.8;
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




