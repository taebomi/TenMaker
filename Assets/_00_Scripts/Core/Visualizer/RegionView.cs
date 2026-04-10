using System;
using System.Collections.Generic;
using TenMaker.RankMode;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Core.Visualizer
{
    public class RegionView : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private RegionDisplayPool pool;

        private List<RegionDisplay> usingRegionDisplays;

        public bool IsEnabled { get; private set; }

        private void Awake()
        {
            usingRegionDisplays = new List<RegionDisplay>();
            IsEnabled = false;
        }

        private void OnDisable()
        {
            Disable();
        }
        
        
        public void Enable()
        {
            if (IsEnabled) return;
            IsEnabled = true;

            OnGridChanged();
        }

        public void Disable()
        {
            if (IsEnabled is false) return;
            IsEnabled = false;

            Clear();
        }

        private void OnGridChanged()
        {
            var validRegions = gridManager.ValidRegionProcessor.ValidRegions;
            Clear();
            Visualize(validRegions);
        }

        public void Visualize(List<Region> regions)
        {
            foreach (var region in regions)
            {
                var regionDisplay = pool.Get();
                regionDisplay.SetRegion(region);
                usingRegionDisplays.Add(regionDisplay);
            }
        }

        public void Clear()
        {
            foreach (var regionDisplay in usingRegionDisplays)
            {
                pool.Return(regionDisplay);
            }

            usingRegionDisplays.Clear();
        }
    }
}