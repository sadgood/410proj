using System;
using System.Collections.Generic;

/// <summary>
/// 俺写的一个实现了类似stl中的multimap，自动排序多键值，c#竟然没找到现成的！
/// </summary>
/// <typeparam name="K">key的类型</typeparam>
/// <typeparam name="V">value的类型</typeparam>
public class MultiMap<K, V>
{
    SortedDictionary<K, List<V>> _dictionary = new SortedDictionary<K, List<V>>();
    public void Add(K key, V value)
    {
        List<V> list;
        if (this._dictionary.TryGetValue(key, out list))
        {
            list.Add(value);
        }
        else
        {
            list = new List<V>();
            list.Add(value);
            this._dictionary[key] = list;
        }
    }
    public IEnumerable<K> Keys
    {
        get
        {
            return this._dictionary.Keys;
        }
    }
    public List<V> this[K key]
    {
        get
        {
            List<V> list;
            if (this._dictionary.TryGetValue(key, out list))
            {
                return list;
            }
            else
            {
                return new List<V>();
            }
        }
    }
    public void Clear()
    {
        _dictionary.Clear();
    }
    public int Count
    {
        get
        {
            int count = 0;
            foreach (K key in Keys)
            {
                count += this[key].Count;
            }
            return count;
        }
    }
}