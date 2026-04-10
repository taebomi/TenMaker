using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Gameplay.RegionClear
{
    public class ClearEffector : MonoBehaviour
    {
        [SerializeField] private ClearEffect clearEffect;

        private IObjectPool<ClearEffect> _pool;

        public void Initialize()
        {
            _pool = new ObjectPool<ClearEffect>(CreateEffect, null, null, null, false);
        }

        public void PlayEffect(IEnumerable<CellObject> clearedCells)
        {
            foreach (var cell in clearedCells)
            {
                var effect = _pool.Get();
                effect.Setup(cell.WorldPosition);
            }
        }

        private ClearEffect CreateEffect()
        {
            var effect = Object.Instantiate(clearEffect);
            effect.Initialize(_pool);
            return effect;
        }
    }
}