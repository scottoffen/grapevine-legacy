using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Grapevine.Util
{
    public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection
    {
        private readonly IDictionary<TKey, TValue> _dictionary;
        private object syncRoot;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionaryToWrap)
        {
            if (dictionaryToWrap ==  null) throw new ArgumentNullException(nameof(dictionaryToWrap));
            _dictionary = dictionaryToWrap;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            IEnumerable<KeyValuePair<TKey, TValue>> enumerator = _dictionary;
            return enumerator.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// This class is read only; this method will throw a NotSupportedException
        /// </summary>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ThrowNotSupportedException();
        }

        /// <summary>
        /// This class is read only; this method will throw a NotSupportedException
        /// </summary>
        public void Clear()
        {
            ThrowNotSupportedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _dictionary.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _dictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// This class is read only; this method will throw a NotSupportedException
        /// </summary>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            ThrowNotSupportedException();
            return false;
        }

        public void CopyTo(Array array, int index)
        {
            ICollection collection = new List<KeyValuePair<TKey, TValue>>(_dictionary);
            collection.CopyTo(array, index);
        }

        int ICollection.Count => _dictionary.Count;

        public object SyncRoot
        {
            get
            {
                if (syncRoot != null) return syncRoot;

                var collection = _dictionary as ICollection;

                if (collection != null)
                {
                    syncRoot = collection.SyncRoot;
                }
                else
                {
                    Interlocked.CompareExchange(ref syncRoot, new object(), null);
                }

                return syncRoot;
            }
        }

        public bool IsSynchronized => false;

        int ICollection<KeyValuePair<TKey, TValue>>.Count => _dictionary.Count;

        public bool IsReadOnly => true;

        public bool ContainsKey(TKey key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// This class is read only; this method will throw a NotSupportedException
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            ThrowNotSupportedException();
        }

        /// <summary>
        /// This class is read only; this method will throw a NotSupportedException
        /// </summary>
        public bool Remove(TKey key)
        {
            ThrowNotSupportedException();
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }


        /// <summary>
        /// Gets the element with the specified key
        /// </summary>
        public TValue this[TKey key]
        {
            get { return _dictionary[key]; }
            set { ThrowNotSupportedException(); }
        }

        public ICollection<TKey> Keys => _dictionary.Keys;

        public ICollection<TValue> Values => _dictionary.Values;

        private static void ThrowNotSupportedException()
        {
            throw new NotSupportedException("This Dictionary is read-only");
        }
    }
}
