namespace TenMaker.Services.UGS
{
    public static class ErrorCode
    {
        public const int UNKNOWN = 0;
        public const int INVALID = -100;
        public const int CANCELLED = -500;
        public const int NOT_INITIALIZED = -1000;


        public static class Leaderboards
        {
            public const int NO_ENTRY = -20000;
        }
    }
}