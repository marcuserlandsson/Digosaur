// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.ReadOnlyDictionary`2
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal class ReadOnlyDictionary<K, V> : 
    IDictionary<K, V>,
    ICollection<KeyValuePair<K, V>>,
    IEnumerable<KeyValuePair<K, V>>,
    IDictionary,
    ICollection,
    IEnumerable
  {
    internal static readonly ReadOnlyDictionary<K, V> Empty = new ReadOnlyDictionary<K, V>(new Dictionary<K, V>());
    private Dictionary<K, V> _dict;

    internal ReadOnlyDictionary(Dictionary<K, V> dict) => this._dict = dict;

    IEnumerator<KeyValuePair<K, V>> IEnumerable<KeyValuePair<K, V>>.GetEnumerator()
    {
      return ((IEnumerable<KeyValuePair<K, V>>) this._dict).GetEnumerator();
    }

    public void Add(KeyValuePair<K, V> pair)
    {
      throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public void Clear() => throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();

    public bool Contains(KeyValuePair<K, V> pair)
    {
      return ((ICollection<KeyValuePair<K, V>>) this._dict).Contains(pair);
    }

    public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
    {
      ((ICollection<KeyValuePair<K, V>>) this._dict).CopyTo(array, arrayIndex);
    }

    public bool Remove(KeyValuePair<K, V> pair)
    {
      throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public void Add(K key, V value)
    {
      throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public bool ContainsKey(K key) => this._dict.ContainsKey(key);

    public bool Remove(K key)
    {
      throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public bool TryGetValue(K key, out V value) => this._dict.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this._dict).GetEnumerator();

    public void CopyTo(Array array, int index) => ((ICollection) this._dict).CopyTo(array, index);

    public void Add(object key, object value)
    {
      throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public bool Contains(object key) => ((IDictionary) this._dict).Contains(key);

    IDictionaryEnumerator IDictionary.GetEnumerator() => ((IDictionary) this._dict).GetEnumerator();

    public void Remove(object key)
    {
      throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public int Count => this._dict.Count;

    public bool IsReadOnly => true;

    public V this[K key]
    {
      get => this._dict[key];
      set => throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }

    public ICollection<K> Keys => ((IDictionary<K, V>) this._dict).Keys;

    public ICollection<V> Values => ((IDictionary<K, V>) this._dict).Values;

    public bool IsSynchronized => ((ICollection) this._dict).IsSynchronized;

    public object SyncRoot => ((ICollection) this._dict).SyncRoot;

    public bool IsFixedSize => true;

    ICollection IDictionary.Keys => ((IDictionary) this._dict).Keys;

    ICollection IDictionary.Values => ((IDictionary) this._dict).Values;

    public object this[object key]
    {
      get => ((IDictionary) this._dict)[key];
      set => throw SurfaceCoreExceptions.ReadOnlyDictionaryOperationIsNotSupported();
    }
  }
}
