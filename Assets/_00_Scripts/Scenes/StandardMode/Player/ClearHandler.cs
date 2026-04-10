using System;
using System.Collections.Generic;
using FMODUnity;
using TenMaker.Core;
using TenMaker.Core.Audio;
using TenMaker.Core.Player;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class ClearHandler : MonoBehaviour
    {
        [Header("Effect")]
        [SerializeField] private ClearEffectController clearEffectController;

        [Header("Audio")]
        [SerializeField] private ComboPitchDataSO comboPitchDataSO;
        [SerializeField] private EventReference cellClearSfx;

        public event Action<Cell, Cell, List<Cell>> AreaClearRequested;

        private List<Cell> _cachedCells;

        private void Awake()
        {
            _cachedCells = new List<Cell>();
        }

        public void Setup()
        {
            clearEffectController.Setup();
        }

        public void RequestAreaClear(Cell startCell, Cell endCell)
        {
            AreaClearRequested?.Invoke(startCell, endCell, _cachedCells);
            
        }

        public void HandleClear(int combo)
        {
            clearEffectController.PlayEffect(_cachedCells);
            var pitch = comboPitchDataSO.GetPitch(combo);
            TMAudioManager.Instance.PlaySfx(cellClearSfx, pitch);
        }
    }
}