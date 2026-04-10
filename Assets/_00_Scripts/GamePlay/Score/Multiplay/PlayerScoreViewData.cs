namespace TenMaker.Gameplay.Score.Multiplay
{
    public struct PlayerScoreViewData
    {
        public ulong ClientId;
        public int Score;
        
        public PlayerScoreViewData(ulong clientId, int score)
        {
            ClientId = clientId;
            Score = score;
        }
    }
}