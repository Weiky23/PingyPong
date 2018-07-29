using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AppearanceHelper : SingletonMonobehaviour<AppearanceHelper>
{
    Camera mainCam;
    static Vector2 screen;
    static float widthInUnits;
    public static float WidthInUnits { get { return widthInUnits; } }
    static float heightInUnits;
    public static float HeightInUnits { get { return heightInUnits; } }
    public static float AspectRatio { get { return Screen.width / Screen.height; } }

    protected override void Awake()
    {
        base.Awake();
        mainCam = GetComponent<Camera>();
        heightInUnits = mainCam.orthographicSize * 2f;
        widthInUnits = heightInUnits * AspectRatio;
        screen = new Vector2(Screen.width, Screen.height);
    }

    public static float ToWorldUnits(float deltaScreen)
    {
        return deltaScreen / Screen.height * Instance.mainCam.orthographicSize * 2f;
    }

    public static Vector2 ToWorldUnits(Vector2 deltaScreen)
    {
        return deltaScreen / Screen.height * Instance.mainCam.orthographicSize * 2f;
    }
}
