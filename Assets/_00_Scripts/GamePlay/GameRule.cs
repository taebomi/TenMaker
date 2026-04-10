namespace TenMaker.Core
{
    public static class GameRule
    {
        public const int PORTRAIT_ROW_COUNT = 17;
        public const int PORTRAIT_COLUMN_COUNT = 10;
        public const int LANDSCAPE_ROW_COUNT = PORTRAIT_COLUMN_COUNT;
        public const int LANDSCAPE_COLUMN_COUNT = PORTRAIT_ROW_COUNT;
        
        public const int MIN_VALUE = 1;
        public const int MAX_VALUE = 9;
        public const int TARGET_SUM = 10;

        // 테스트 결과 매번 50개 이상 나옴
        public const int MINIMUM_VALID_REGION_COUNT = 30; 
        public const int DEFAULT_VALUE_WHEN_FAILED = 2;
        
        
        // 콤보
        public const int MINIMUM_COMBO = 3;
        public const float COMBO_RESET_TIME = 33f;
    }
}