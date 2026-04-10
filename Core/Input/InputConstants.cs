namespace TenMaker.Core.Input
{
    public static class InputConstants
    {
        public static class Maps
        {
            public const string GAME_PLAY = "GamePlay";
            public const string UI = "UI";
        }

        public static class GamePlay
        {
            private const string PREFIX = Maps.GAME_PLAY + "/";
            public const string DRAG = PREFIX + "Drag";
        }
    }
}