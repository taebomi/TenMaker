using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TenMaker.Utility;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay.Multiplay
{
    public class FlipAnimator
    {
        private const float FLIP_INTERVAL = 0.025f;
        private const float FLIP_DURATION = 0.075f;

        private GridVisualizer _gridVisualizer;

        public FlipAnimator(GridVisualizer gridVisualizer)
        {
            _gridVisualizer = gridVisualizer;
        }

        public async UniTask FlipAsync(PlayerColor color, Region region, CancellationToken ct)
        {
            for (var row = region.maxRow; row >= region.minRow; row--)
            {
                for (var col = region.minCol; col <= region.maxCol; col++)
                {
                    var cellObject = _gridVisualizer.CellObjects[row, col];
                    if (cellObject.Color == color) continue;
                    FlipCellAsync(cellObject, color);
                    await UniTask.Delay(TimeSpan.FromSeconds(FLIP_INTERVAL), cancellationToken: ct);
                }
            }
        }

        public void FlipCellAsync(CellObject cellObject, PlayerColor color)
        {
            DOTween.Sequence()
                .Append(cellObject.transform.DOScaleX(0f, FLIP_DURATION).SetEase(Ease.InSine))
                .AppendCallback(OnFlip)
                .Append(cellObject.transform.DOScaleX(1f, FLIP_DURATION).SetEase(Ease.OutSine)).Play();
            return;

            void OnFlip()
            {
                cellObject.SetPlayerCell(color);
            }
        }
    }
}