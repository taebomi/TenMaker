using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace TenMaker.Core.Visualizer
{
    public class RegionDisplayPool : MonoBehaviour
    {
        private IObjectPool<RegionDisplay> _pool;

        [SerializeField] private RegionDisplay prefab;

        private void Awake()
        {
            _pool = new ObjectPool<RegionDisplay>(CreateInstance);
        }

        public RegionDisplay Get()
        {
            var regionDisplay = _pool.Get();
            regionDisplay.SetActive(true);
            return regionDisplay;
        }

        public void Return(RegionDisplay regionDisplay)
        {
            regionDisplay.SetActive(false);
            _pool.Release(regionDisplay);
        }

        private RegionDisplay CreateInstance()
        {
            return Instantiate(prefab, transform);
        }
    }
}