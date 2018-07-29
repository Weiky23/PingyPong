using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
    public class MyGUIStyles
    {
        static GUIStyle centerHeader;
        static GUIStyle centerBoldHeader;
        static GUIStyle noMargin;
        static GUIStyle assetLabel;
        public static GUIStyle searchTextField;
        public static GUIStyle searchTextCancelButtonEmpty;
        public static GUIStyle searchTextCancelButton;
        public static GUIStyle leftBoldHeader;
        public static GUIStyle right;

        public static GUIStyle CenterBoldHeader { get { return centerBoldHeader; } }
        public static GUIStyle CenterHeader { get { return centerHeader; } }
        public static GUIStyle NoMargin { get { return noMargin; } }
        public static GUIStyle AssetLabel { get { return assetLabel; } }
        public static GUIStyle SearchTextField { get { return searchTextField; } }

        static MyGUIStyles()
        {
            centerHeader = new GUIStyle(EditorStyles.largeLabel) { alignment = TextAnchor.MiddleCenter };
            centerBoldHeader = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
            noMargin = new GUIStyle(EditorStyles.miniLabel) { margin = new RectOffset(0, 0, 0, 0) };
            assetLabel = GetStyle("AssetLabel");
            searchTextField = GetStyle("SearchTextField");
            searchTextCancelButtonEmpty = GetStyle("SearchCancelButtonEmpty");
            searchTextCancelButton = GetStyle("SearchCancelButton");
            leftBoldHeader = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.LowerLeft };
            right = new GUIStyle() { alignment = TextAnchor.LowerRight };
        }

        public static GUIStyle GetStyle(string styleName)
        {
            GUIStyle s = GUI.skin.FindStyle(styleName);
            if (s == null)
                s = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle(styleName);
            if (s == null)
            {
                Debug.LogError("Missing built-in guistyle " + styleName);
            }
            return s;
        }
    }
}
