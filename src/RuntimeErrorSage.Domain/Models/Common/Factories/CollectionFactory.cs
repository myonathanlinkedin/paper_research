using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using RuntimeErrorSage.Application.Models.Common.Interfaces;

namespace RuntimeErrorSage.Application.Models.Common.Factories
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
        public Collection<T> CreateCollection<T>()
        {
            return new Collection<T>();
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

        /// <summary>
        /// Creates a new Queue instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the queue.</typeparam>
        /// <returns>A new Queue instance.</returns>
        public Queue<T> CreateQueue<T>()
        {
            return new Queue<T>();
        }

        /// <summary>
        /// Creates a new Stack instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the stack.</typeparam>
        /// <returns>A new Stack instance.</returns>
        public Stack<T> CreateStack<T>()
        {
            return new Stack<T>();
        }

        /// <summary>
        /// Creates a new SortedDictionary instance.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the sorted dictionary.</typeparam>
        /// <returns>A new SortedDictionary instance.</returns>
        public SortedDictionary<TKey, TValue> CreateSortedDictionary<TKey, TValue>()
        {
            return new SortedDictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates a new SortedList instance.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the sorted list.</typeparam>
        /// <typeparam name="TValue">The type of values in the sorted list.</typeparam>
        /// <returns>A new SortedList instance.</returns>
        public SortedCollection<TKey, TValue> CreateSortedCollection<TKey, TValue>()
        {
            return new SortedCollection<TKey, TValue>();
        }

        /// <summary>
        /// Creates a new SortedSet instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sorted set.</typeparam>
        /// <returns>A new SortedSet instance.</returns>
        public SortedSet<T> CreateSortedSet<T>()
        {
            return new SortedSet<T>();
        }

        /// <summary>
        /// Creates a new LinkedList instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the linked list.</typeparam>
        /// <returns>A new LinkedList instance.</returns>
        public LinkedCollection<T> CreateLinkedCollection<T>()
        {
            return new LinkedCollection<T>();
        }

        /// <summary>
        /// Creates a new ConcurrentDictionary instance.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the concurrent dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the concurrent dictionary.</typeparam>
        /// <returns>A new ConcurrentDictionary instance.</returns>
        public ConcurrentDictionary<TKey, TValue> CreateConcurrentDictionary<TKey, TValue>()
        {
            return new ConcurrentDictionary<TKey, TValue>();
        }

        /// <summary>
        /// Creates a new ConcurrentQueue instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the concurrent queue.</typeparam>
        /// <returns>A new ConcurrentQueue instance.</returns>
        public ConcurrentQueue<T> CreateConcurrentQueue<T>()
        {
            return new ConcurrentQueue<T>();
        }

        /// <summary>
        /// Creates a new ConcurrentStack instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the concurrent stack.</typeparam>
        /// <returns>A new ConcurrentStack instance.</returns>
        public ConcurrentStack<T> CreateConcurrentStack<T>()
        {
            return new ConcurrentStack<T>();
        }

        /// <summary>
        /// Creates a new ConcurrentBag instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the concurrent bag.</typeparam>
        /// <returns>A new ConcurrentBag instance.</returns>
        public ConcurrentBag<T> CreateConcurrentBag<T>()
        {
            return new ConcurrentBag<T>();
        }

        /// <summary>
        /// Creates a new BlockingCollection instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the blocking collection.</typeparam>
        /// <returns>A new BlockingCollection instance.</returns>
        public BlockingCollection<T> CreateBlockingCollection<T>()
        {
            return new BlockingCollection<T>();
        }
    }
} 





