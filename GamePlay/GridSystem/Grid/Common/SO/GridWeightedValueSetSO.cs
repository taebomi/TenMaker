using System.Collections.Generic;
using TenMaker.Utility;
using UnityEngine;

namespace TenMaker.Gameplay.GridSystem
{
    [CreateAssetMenu(menuName = "Ten Maker/Grid/Weighted Value Set")]
    public class GridWeightedValueSetSO : ScriptableObject
    {
        public List<WeightedItem<int>> items;
    }
}