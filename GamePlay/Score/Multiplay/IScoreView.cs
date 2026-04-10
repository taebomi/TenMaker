using System.Collections.Generic;
using TenMaker.Gameplay.Multiplay;
using TenMaker.Gameplay.Player;
using TenMaker.Gameplay.Player.Multiplay;

namespace TenMaker.Gameplay.Score.Multiplay
{
    public interface IScoreView
    {
        void Initialize(PlayersContext playerContext);
        void UpdateScore(List<PlayerScoreViewData> scoreDataList);
    }
}