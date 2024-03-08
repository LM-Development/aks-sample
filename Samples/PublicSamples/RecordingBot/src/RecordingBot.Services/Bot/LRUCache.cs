using System;
using System.Collections.Generic;

namespace RecordingBot.Services.Bot
{
    /// <summary>
    /// Helper LRUCache used for the multiview subscription process.
    /// </summary>
    public class LRUCache
    {
        private readonly int _capacity;
        /// <summary>
        /// LRU Cache.
        /// </summary>
        private readonly Dictionary<uint, LinkedListNode<uint>> _cache;
        
        private readonly LinkedList<uint> _lruList;
        
        /// <summary>
        /// Maximum size of the LRU cache.
        /// </summary>
        public const uint Max = 10;

        public LRUCache(int capacity)
        {
            if (capacity > Max)
            {
                throw new ArgumentException($"size value too large; max value is {Max}");
            }

            _capacity = capacity;
            _cache = new Dictionary<uint, LinkedListNode<uint>>(capacity);
            _lruList = new LinkedList<uint>();
        }

        /// <summary>
        /// Gets the count of items in the cache.
        /// </summary>
        /// <value>The count.</value>
        public int Count => _cache.Count;

        /// <inheritdoc/>
        public override string ToString()
        {
            return "{" + string.Join(", ", _lruList) + "}";
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

            if (_cache.TryGetValue(key, out var node))
            {
                _lruList.Remove(node);
                _lruList.AddFirst(node);
            }
            else
            {
                if (_cache.Count >= _capacity)
                {
                    var lastNode = _lruList.Last;
                    evictedKey = lastNode.Value;
                    _cache.Remove(lastNode.Value);
                    _lruList.RemoveLast();
                }

                node = new LinkedListNode<uint>(key);
                _cache.Add(key, node);
                _lruList.AddFirst(node);
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
            if (_cache.TryGetValue(key, out var node))
            {
                _cache.Remove(key);
                _lruList.Remove(node);
                return true;
            }

            return false;
        }
    }
}
