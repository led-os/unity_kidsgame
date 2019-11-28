using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotEditor : Editor
{
    public const string KEY_MENU_GameObject_UI = "ScreenShot";
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/ConverIcon", false, 4)]
    static void OnConverIcon()
    {
        IconConvert.main.OnConvertAll();
    }

    [MenuItem(KEY_MENU_GameObject_UI + "/CopyRight", false, 4)]
    static void OnCopyRight()
    {

    }
}
