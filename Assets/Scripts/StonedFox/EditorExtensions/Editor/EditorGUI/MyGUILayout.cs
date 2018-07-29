using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public static class MyGUILayout
    {
        public static void DrawSeparatorLine()
        {
            EditorGUILayout.LabelField(GUIContent.none, GUI.skin.horizontalSlider);
        }
    }
}
