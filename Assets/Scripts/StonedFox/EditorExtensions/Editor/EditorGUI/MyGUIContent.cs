using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class MyGUIContent
    {
        public static GUIContent playButtonIcon;
        public static GUIContent pauseButtonIcon;
        public static GUIContent smallErrorIcon;

        static MyGUIContent()
        {
            playButtonIcon = GetContent("PlayButton");
            pauseButtonIcon = GetContent("PauseButton");
            smallErrorIcon = GetContent("console.erroricon.sml");
        }

        public static GUIContent ErrorMessageWithIcon(string message)
        {
            return new GUIContent(smallErrorIcon) { text = message };
        }

        public static GUIContent GetContent(string contentName)
        {
            return EditorGUIUtility.IconContent(contentName);
        }
    }
}
