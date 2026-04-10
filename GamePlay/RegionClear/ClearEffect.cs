using TenMaker.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Gameplay.RegionClear
{
    public class ClearEffect : MonoBehaviour
    {
        private IObjectPool<ClearEffect> _pool;

        public void Initialize(IObjectPool<ClearEffect> pool)
        {
            _pool = pool;
        }

        public void Setup(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);
        }

        private void OnParticleSystemStopped()
        {
            if (_pool != null)
            {
                _pool.Release(this);
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}