using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Core
{
    public class CellClearEffect : MonoBehaviour
    {
        private IObjectPool<CellClearEffect> _pool;

        public void Setup(IObjectPool<CellClearEffect> pool)
        {
            _pool = pool;
        }

        private void OnParticleSystemStopped()
        {
            if (_pool != null)
            {
                _pool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}