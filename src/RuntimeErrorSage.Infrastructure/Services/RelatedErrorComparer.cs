using System.Collections.ObjectModel;
using System.Collections.Generic;
using RuntimeErrorSage.Application.Models.Error;

namespace RuntimeErrorSage.Application.Services;

/// <summary>
/// Comparer for RelatedError objects to ensure proper equality comparison.
/// </summary>
public class RelatedErrorComparer : IEqualityComparer<RelatedError>
{
    /// <inheritdoc />
    public RelatedError x, RelatedError y { ArgumentNullException.ThrowIfNull(RelatedError x, RelatedError y); }
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return x.ErrorId == y.ErrorId;
    }

    /// <inheritdoc />
    public RelatedError obj { ArgumentNullException.ThrowIfNull(RelatedError obj); }
    {
        return obj?.ErrorId?.GetHashCode() ?? 0;
    }
} 





