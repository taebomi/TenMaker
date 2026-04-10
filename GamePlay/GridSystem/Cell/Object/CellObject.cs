using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TenMaker.Utility.SO;
using TenMaker.Gameplay.Customization;
using TenMaker.Gameplay.GridSystem;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.GridSystem.Multiplay;
using TenMaker.Gameplay.Player.Multiplay;
using UnityEngine;

namespace TenMaker.Gameplay
{
    public class CellObject : MonoBehaviour
    {
        public const float SIZE = 1f;
        public const float HALF_SIZE = SIZE / 2;

        // References
        public Cell Cell { get; private set; }

        // Properties
        public Vector2Int Coordinate { get; private set; }

        public Vector3 WorldPosition => transform.position;
        public PlayerColor Color { get; private set; }

        // Components
        private CellBackground _background;
        private CellNumber _number;

        public void Initialize(Cell cell, CellBackground background, CellNumber number, Vector2Int coordinate)
        {
            Cell = cell;
            Coordinate = coordinate;
            _background = background;
            _number = number;
            Color = PlayerColor.None;
            SetValue(cell.Value);
        }

        public void SetPlayerCell(PlayerColor color)
        {
            Color = color;
            _background.SetPlayerArea(color);
        }

        /// <summary>
        /// value 값 설정
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(int newValue)
        {
            _number.SetValue(newValue);
        }

        /// <summary>
        /// value 값 보여줄지 설정
        /// </summary>
        /// <param name="value"></param>
        public void SetValueVisible(bool value)
        {
            _number.SetVisible(value);
        }
    }
}