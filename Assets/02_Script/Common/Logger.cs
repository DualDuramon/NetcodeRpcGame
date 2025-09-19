using System.Diagnostics;
using Debug = UnityEngine.Debug;


namespace LittleSword.Common
{
    public static class Logger
    {
        [Conditional("DEVELOP_MODE")] //뒤에 파라미터에 써진 특정 조건이 만족됐을때 실행이 되는 어트리뷰트
        [Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        [Conditional("DEVELOP_MODE")]
        [Conditional("UNITY_EDITOR")]
        public static void LoggError(object message)
        {
            Debug.LogError(message);
        }

        [Conditional("DEVELOP_MODE")]
        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
    }
}