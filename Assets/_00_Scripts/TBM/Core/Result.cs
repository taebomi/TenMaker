namespace TenMaker.Utility.Core
{
    public struct Result
    {
        public bool IsSuccess { get; }
        public int ErrorCode { get; }
        public string Message { get; }

        private Result(bool success, int errorCode, string message)
        {
            IsSuccess = success;
            Message = message;
            ErrorCode = errorCode;
        }

        public static Result Success()
        {
            return new Result(true, 0, null);
        }

        public static Result Fail(int errorCode, string message = null)
        {
            return new Result(false, errorCode, message);
        }
    }

    public struct Result<T>
    {
        public bool IsSuccess { get; }
        public int ErrorCode { get; }
        public string Message { get; }
        public T Value { get; }

        private Result(bool success, int errorCode, string message, T value)
        {
            IsSuccess = success;
            ErrorCode = errorCode;
            Message = message;
            Value = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, 0, null, value);
        }

        public static Result<T> Fail(int errorCode, T value = default, string message = null)
        {
            return new Result<T>(false, errorCode, message, value);
        }
    }
}