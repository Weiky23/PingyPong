using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorExtensions;

[CustomEditor(typeof(BallsOrder))]
public class BallsOrderEditor : SingleReorderableListEditor
{
    void OnEnable()
    {
        header = "Balls Order";
        InitList(BallsOrder.ballsField);
    }

    protected override void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        rect.y += 2;
        rect.height -= 4;
        SerializedProperty property = elements.GetArrayElementAtIndex(index);
        EditorGUI.ObjectField(rect, property, GUIContent.none);
    }
}
