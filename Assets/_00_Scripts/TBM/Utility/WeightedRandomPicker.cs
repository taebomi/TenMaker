using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TenMaker.Utility
{
    [Serializable]
    public struct WeightedItem<T>
    {
        public T value;
        public int weight;

        public WeightedItem(T value, int weight)
        {
            this.value = value;
            this.weight = weight;
        }
    }

    public class WeightedRandomPicker<T>
    {
        private class ProcessedItem
        {
            public T Value { get; }
            public int CumulativeWeight { get; }

            public ProcessedItem(T value, int cumulativeWeight)
            {
                Value = value;
                CumulativeWeight = cumulativeWeight;
            }
        }

        private readonly List<ProcessedItem> _processedItems;
        private readonly int _totalWeight;


        public WeightedRandomPicker(IEnumerable<WeightedItem<T>> weightedItems)
        {
            _processedItems = new List<ProcessedItem>();
            _totalWeight = 0;

            if (weightedItems == null) throw new ArgumentNullException($"{nameof(weightedItems)} cannot be null");

            foreach (var item in weightedItems)
            {
                if (item.weight <= 0) throw new ArgumentException($"{nameof(item.weight)} cannot be negative");

                _totalWeight += item.weight;
                _processedItems.Add(new ProcessedItem(item.value, _totalWeight));
            }

            if (_totalWeight == 0) throw new ArgumentException($"{nameof(weightedItems)} cannot be empty");
        }

        public T Pick()
        {
            var roll = Random.Range(0, _totalWeight);
            foreach (var processedItem in _processedItems)
            {
                if (roll < processedItem.CumulativeWeight)
                {
                    return processedItem.Value;
                }
            }

            return _processedItems[^1].Value;
        }
    }
}