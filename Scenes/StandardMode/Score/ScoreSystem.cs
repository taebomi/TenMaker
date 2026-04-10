using System;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine;

namespace TenMaker.RankMode
{
    public class ScoreSystem : MonoBehaviour
    {
        public ObscuredInt Score { get; private set; }
        private int _score;

        [SerializeField] private ScoreUI scoreUI;

        private void Awake()
        {
            Score = 0;
            _score = 0;
            scoreUI.Setup();
        }

        public void ResetScore()
        {
            Score = 0;
            _score = 0;
            scoreUI.Setup();
        }

        public void AddScore(int score)
        {
            _score += score;
            Score += score;
            scoreUI.SetCurScore(_score);
        }
    }
}