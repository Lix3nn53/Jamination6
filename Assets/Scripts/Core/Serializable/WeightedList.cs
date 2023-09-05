using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Lix.Core
{
  [Serializable]
  public class WeightedList<TKey> : SerializableDictionaryBase<TKey, int, int>
  {
    public WeightedList() { }
    public WeightedList(IDictionary<TKey, int> dict) : base(dict) { }
    protected WeightedList(SerializationInfo info, StreamingContext context) : base(info, context) { }

    protected override int GetValue(int[] storage, int i)
    {
      return storage[i];
    }

    protected override void SetValue(int[] storage, int i, int value)
    {
      storage[i] = value;
    }

    public TKey GetRandom()
    {
      int totalWeight = 0;
      foreach (int weight in Values)
      {
        totalWeight += weight;
      }

      int random = UnityEngine.Random.Range(0, totalWeight);
      foreach (TKey key in Keys)
      {
        random -= this[key];
        if (random < 0)
        {
          return key;
        }
      }

      return default(TKey);
    }

    public List<TKey> GetUniqueRandom(int count)
    {
      List<TKey> selectedKeys = new List<TKey>();
      int totalWeight = 0;

      foreach (int weight in Values)
      {
        totalWeight += weight;
      }

      for (int i = 0; i < count; i++)
      {
        int random = UnityEngine.Random.Range(0, totalWeight);
        TKey selectedKey = default(TKey);

        foreach (TKey key in Keys)
        {
          random -= this[key];

          if (random < 0 && !selectedKeys.Contains(key))
          {
            selectedKey = key;
            selectedKeys.Add(key);
            break;
          }
        }

        if (selectedKey.Equals(default(TKey)))
        {
          break;
        }
      }

      return selectedKeys;
    }
  }
}