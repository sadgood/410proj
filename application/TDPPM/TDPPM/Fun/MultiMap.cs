using System;
using System.Collections.Generic;

/// <summary>
/// ��д��һ��ʵ��������stl�е�multimap���Զ�������ֵ��c#��Ȼû�ҵ��ֳɵģ�
/// </summary>
/// <typeparam name="K">key������</typeparam>
/// <typeparam name="V">value������</typeparam>
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