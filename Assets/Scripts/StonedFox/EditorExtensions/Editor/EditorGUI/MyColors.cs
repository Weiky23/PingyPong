using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class MyColors
    {
        public static Color reorderableListColor;
        public static Color errorAutoCompleteColor;

        static MyColors()
        {
            UnityEngine.ColorUtility.TryParseHtmlString("#E4E5E4FF", out reorderableListColor);
            errorAutoCompleteColor = new Color(1f, 0f, 0f, 0.28f);
        }
    }
}