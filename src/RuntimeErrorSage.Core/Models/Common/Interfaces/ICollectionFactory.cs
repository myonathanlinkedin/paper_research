using System.Collections.Generic;

namespace RuntimeErrorSage.Core.Models.Common.Interfaces
{
    /// <summary>
    /// Interface for creating collections of various types.
    /// </summary>
    public interface ICollectionFactory
    {
        /// <summary>
        /// Creates a new dictionary with the specified key and value types.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <returns>A new dictionary instance.</returns>
        Dictionary<TKey, TValue> CreateDictionary<TKey, TValue>();

        /// <summary>
        /// Creates a new list with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <returns>A new list instance.</returns>
        List<T> CreateList<T>();

        /// <summary>
        /// Creates a new hash set with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the hash set.</typeparam>
        /// <returns>A new hash set instance.</returns>
        HashSet<T> CreateHashSet<T>();

        /// <summary>
        /// Creates a new queue with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the queue.</typeparam>
        /// <returns>A new queue instance.</returns>
        Queue<T> CreateQueue<T>();

        /// <summary>
        /// Creates a new stack with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the stack.</typeparam>
        /// <returns>A new stack instance.</returns>
        Stack<T> CreateStack<T>();

        /// <summary>
        /// Creates a new sorted dictionary with the specified key and value types.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the sorted dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the sorted dictionary.</typeparam>
        /// <returns>A new sorted dictionary instance.</returns>
        SortedDictionary<TKey, TValue> CreateSortedDictionary<TKey, TValue>();

        /// <summary>
        /// Creates a new sorted list with the specified key and value types.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the sorted list.</typeparam>
        /// <typeparam name="TValue">The type of the values in the sorted list.</typeparam>
        /// <returns>A new sorted list instance.</returns>
        SortedList<TKey, TValue> CreateSortedList<TKey, TValue>();

        /// <summary>
        /// Creates a new sorted set with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sorted set.</typeparam>
        /// <returns>A new sorted set instance.</returns>
        SortedSet<T> CreateSortedSet<T>();

        /// <summary>
        /// Creates a new linked list with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the linked list.</typeparam>
        /// <returns>A new linked list instance.</returns>
        LinkedList<T> CreateLinkedList<T>();

        /// <summary>
        /// Creates a new concurrent dictionary with the specified key and value types.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the concurrent dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the concurrent dictionary.</typeparam>
        /// <returns>A new concurrent dictionary instance.</returns>
        System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue> CreateConcurrentDictionary<TKey, TValue>();

        /// <summary>
        /// Creates a new concurrent queue with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the concurrent queue.</typeparam>
        /// <returns>A new concurrent queue instance.</returns>
        System.Collections.Concurrent.ConcurrentQueue<T> CreateConcurrentQueue<T>();

        /// <summary>
        /// Creates a new concurrent stack with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the concurrent stack.</typeparam>
        /// <returns>A new concurrent stack instance.</returns>
        System.Collections.Concurrent.ConcurrentStack<T> CreateConcurrentStack<T>();

        /// <summary>
        /// Creates a new concurrent bag with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the concurrent bag.</typeparam>
        /// <returns>A new concurrent bag instance.</returns>
        System.Collections.Concurrent.ConcurrentBag<T> CreateConcurrentBag<T>();

        /// <summary>
        /// Creates a new blocking collection with the specified element type.
        /// </summary>
        /// <typeparam name="T">The type of elements in the blocking collection.</typeparam>
        /// <returns>A new blocking collection instance.</returns>
        System.Collections.Concurrent.BlockingCollection<T> CreateBlockingCollection<T>();
    }
} 