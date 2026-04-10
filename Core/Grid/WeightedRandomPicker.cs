using System.Collections.Generic;
using UnityEngine;

namespace TenMaker.Core
{
    public static class WeightedRandomPicker
    {
        private struct WeightedValue
        {
            public int value;
            public int weight;
        }

        private static List<WeightedValue> _weightedValues;
        private static int _totalWeight;

        static WeightedRandomPicker()
        {
            _weightedValues = new List<WeightedValue>
            {
                new() { value = 1, weight = 10 },
                new() { value = 2, weight = 15 },
                new() { value = 3, weight = 15 },
                new() { value = 4, weight = 15 },
                new() { value = 5, weight = 15 },
                new() { value = 6, weight = 10 },
                new() { value = 7, weight = 8 },
                new() { value = 8, weight = 7 },
                new() { value = 9, weight = 5 },
            };

            foreach (var weightedValue in _weightedValues)
            {
                _totalWeight += weightedValue.weight;
            }
        }

        public static int Pick()
        {
            var random = Random.Range(0, _totalWeight);
            var cumulativeWeight = 0;
            foreach (var weightedValue in _weightedValues)
            {
                cumulativeWeight += weightedValue.weight;
                if (random < cumulativeWeight)
                {
                    return weightedValue.value;
                }
            }

            return _weightedValues[^1].value;
        }
    }
}