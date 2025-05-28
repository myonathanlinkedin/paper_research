using System;
using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common.Factories
{
    /// <summary>
    /// Factory for creating collection instances.
    /// </summary>
    public class CollectionFactory : ICollectionFactory
    {
        /// <summary>
        /// Creates a new List instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A new List instance.</returns>
        public List<T> CreateList<T>()
        {
            return new List<T>();
        }

        /// <summary>
        /// Creates a new Dictionary instance.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <returns>A new Dictionary instance.</returns>
        public Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates a new HashSet instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <returns>A new HashSet instance.</returns>
        public HashSet<T> CreateHashSet<T>()
        {
            return new HashSet<T>();
        }
    }
} 