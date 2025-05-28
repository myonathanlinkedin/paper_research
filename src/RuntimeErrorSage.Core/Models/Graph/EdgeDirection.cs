using System;

namespace RuntimeErrorSage.Core.Models.Graph
{
    /// <summary>
    /// Defines the direction of edges to consider when getting neighbors.
    /// </summary>
    public enum EdgeDirection
    {
        /// <summary>
        /// Incoming edges only.
        /// </summary>
        Incoming,

        /// <summary>
        /// Outgoing edges only.
        /// </summary>
        Outgoing,

        /// <summary>
        /// Both incoming and outgoing edges.
        /// </summary>
        Both
    }
} 