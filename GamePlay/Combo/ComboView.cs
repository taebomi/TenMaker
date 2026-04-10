using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TenMaker.Gameplay.Combo
{
    public class ComboView : MonoBehaviour
    {
        // 콤보 수치가 일정 수치 이상일 경우 이펙트 출력
        // 이펙트는 해당 영역의 중앙에 생성, 일정 시간 이후에 빠르게 상승하며 사라짐
        // 콤보 수치에 따라 이펙트 크기, 효과, 지속시간 다를 것

        [SerializeField] private ComboObject comboObjectPrefab;

        private IObjectPool<ComboObject> _pool;

        private void Awake()
        {
            _pool = new ObjectPool<ComboObject>(CreateComboObject);
        }

        public void ShowCombo(int combo, Vector2 position)
        {
            var comboObject = _pool.Get();
            comboObject.Setup(combo, position);
        }

        private ComboObject CreateComboObject()
        {
            var comboObject = Instantiate(comboObjectPrefab, transform);
            comboObject.Initialize(_pool);
            return comboObject;
        }
    }
}