// ***********************************************************************
// Assembly         : RecordingBot.Services
// Author           : JasonTheDeveloper
// Created          : 09-07-2020
//
// Last Modified By : dannygar
// Last Modified On : 08-17-2020
// ***********************************************************************
// <copyright file="LRUCache.cs" company="Microsoft Corporation">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// <summary>Initialize the HttpConfiguration for OWIN</summary>
// ***********************************************************************-

using System;
using System.Collections.Generic;

namespace RecordingBot.Services.Bot
{
    /// <summary>
    /// Helper LRUCache used for the multiview subscription process.
    /// </summary>
    public class LRUCache
    {
        private readonly int capacity;
        /// <summary>
        /// LRU Cache.
        /// </summary>
        private readonly Dictionary<uint, LinkedListNode<uint>> cache;
        
        private readonly LinkedList<uint> lruList;
        
        /// <summary>
        /// Maximum size of the LRU cache.
        /// </summary>
        public const uint Max = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="LRUCache" /> class.
        /// Constructor for the LRU cache.
        /// </summary>
        /// <param name="capacity">Size ofthe cache.</param>
        /// <exception cref="ArgumentException">capacity value too large; max value is {Max}</exception>
        public LRUCache(int capacity)
        {
            if (capacity > Max)
            {
                throw new ArgumentException($"size value too large; max value is {Max}");
            }

            this.capacity = capacity;
            cache = new Dictionary<uint, LinkedListNode<uint>>(capacity);
            lruList = new LinkedList<uint>();
        }

        /// <summary>
        /// Gets the count of items in the cache.
        /// </summary>
        /// <value>The count.</value>
        public int Count => cache.Count;

        /// <inheritdoc/>
        public override string ToString()
        {
            return "{" + string.Join(", ", lruList) + "}";
        }

        /// <summary>
        /// Insert item k at the front of the set, possibly evicting item evictedKey;
        /// if key is already in the set, move it to the front, shifting the items
        /// before key over to the right one position.
        /// </summary>
        /// <param name="key">Item to insert.</param>
        /// <param name="evictedKey">Item to evict (optional).</param>
        public void TryInsert(uint key, out uint? evictedKey)
        {
            evictedKey = null;

            if (cache.TryGetValue(key, out var node))
            {
                lruList.Remove(node);
                lruList.AddFirst(node);
            }
            else
            {
                if (cache.Count >= capacity)
                {
                    var lastNode = lruList.Last;
                    evictedKey = lastNode.Value;
                    cache.Remove(lastNode.Value);
                    lruList.RemoveLast();
                }

                node = new LinkedListNode<uint>(key);
                cache.Add(key, node);
                lruList.AddFirst(node);
            }
        }

        /// <summary>
        /// Remove item key from the set (if found); shifting any items after key
        /// to the left one position to fill the gap.
        /// </summary>
        /// <param name="key">Item to remove.</param>
        /// <returns>True if item was removed.</returns>
        public bool TryRemove(uint key)
        {
            if (cache.TryGetValue(key, out var node))
            {
                cache.Remove(key);
                lruList.Remove(node);
                return true;
            }

            return false;
        }
    }
}
