using System.Diagnostics;
using Debug = UnityEngine.Debug;


namespace LittleSword.Common
{
    public static class Logger
    {
        [Conditional("DEVELOP_MODE")] //�ڿ� �Ķ���Ϳ� ���� Ư�� ������ ���������� ������ �Ǵ� ��Ʈ����Ʈ
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