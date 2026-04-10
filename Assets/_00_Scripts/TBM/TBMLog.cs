using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

namespace TenMaker.Utility
{
    public static class TBMLog
    {
        private const string FRONT_TEXT = "<color=yellow>#</color>";
        private const string SIMPLE_FRONT_TEXT = "<color=yellow>##</color>";

        [Conditional("TBM_DEBUG")]
        public static void HeaderLog(object message = null, [CallerFilePath] string filePath = "",
            [CallerMemberName] string memberName = "")
        {
            Debug.Log($"{GetHeader(filePath, memberName)} {message}");
        }

        [Conditional("TBM_DEBUG")]
        public static void HeaderWarning(object message = null,
            [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            Debug.LogWarning($"{GetHeader(filePath, memberName)} {message}");
        }

        public static void HeaderError(object message = null,
            [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            Debug.LogError($"{GetHeader(filePath, memberName)} {message}");
        }

        [Conditional("TBM_DEBUG")]
        public static void HeaderAssert(bool condition, object message,
            [CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
        {
            Debug.Assert(condition, $"{GetHeader(filePath, memberName)} {message}");
        }

        [Conditional("TBM_DEBUG")]
        public static void SimpleLog(object message)
        {
            Debug.Log($"{SIMPLE_FRONT_TEXT} {message}");
        }

        [Conditional("TBM_DEBUG")]
        public static void SimpleWarning(object message)
        {
            Debug.LogWarning($"{SIMPLE_FRONT_TEXT} {message}");
        }

        public static void SimpleError(object message)
        {
            Debug.LogError($"{SIMPLE_FRONT_TEXT} {message}");
        }

        private static string GetHeader(string filePath, string memberName)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var header = $"{FRONT_TEXT} <color=#00ffff>[{fileName}/{memberName}]</color> ";
            return header;
        }
    }
}