#if UNITY_EDITOR
using UnityEditor;

namespace EditorUtils
{
    public static class EditorUtils
    {
        public static void ExitEditor()
        {
            EditorApplication.isPlaying = false;
        }
    }
}
#endif