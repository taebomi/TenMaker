using System.Collections.Generic;
using TenMaker.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.RankMode
{
    public class ClearEffectController : MonoBehaviour
    {
        [SerializeField] private CellClearEffect cellClearEffectPrefab;

        private IObjectPool<CellClearEffect> _pool;

        // 커스터마이즈 기능
        // 설정한 Effect로 세팅

        public void Setup()
        {
            _pool = new ObjectPool<CellClearEffect>(CreateEffect, OnGetEffect, OnReleaseEffect);
        }

        private CellClearEffect CreateEffect()
        {
            var effect = Instantiate(cellClearEffectPrefab, transform);
            effect.Setup(_pool);
            return effect;
        }

        private void OnGetEffect(CellClearEffect effect)
        {
            effect.gameObject.SetActive(true);
        }

        private void OnReleaseEffect(CellClearEffect effect)
        {
            effect.gameObject.SetActive(false);
        }

        public void PlayEffect(List<Cell> clearedCells)
        {
            foreach (var cell in clearedCells)
            {
                var effect = _pool.Get();
                effect.transform.position = cell.transform.position;
            }
        }
    }
}