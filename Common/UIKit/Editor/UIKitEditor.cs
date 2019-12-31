using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIKitEditor : Editor
{
    public const string KEY_MENU_GameObject_UI = "GameObject/Moonma";
    void Awake()
    {

    }
    // Use this for initialization
    void Start()
    {

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIView", false, 4)]
    static void CreateUIView()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = new GameObject("UIView");
            if (obj != null)
            {
                UIView ui = obj.AddComponent<UIView>();
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = obj.AddComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIButton", false, 4)]
    static void CreateUIButton()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIButton/UIButton");
            if (obj != null)
            {
                UIButton uiPrefab = obj.GetComponent<UIButton>();
                UIButton ui = (UIButton)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIImage", false, 4)]
    static void CreateUIImage()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIImage/UIImage");
            if (obj != null)
            {
                UIImage uiPrefab = obj.GetComponent<UIImage>();
                UIImage ui = (UIImage)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }

    [MenuItem(KEY_MENU_GameObject_UI + "/UIText", false, 4)]
    static void CreateUIText()
    {
        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIText/UIText");
            if (obj != null)
            {
                UIText uiPrefab = obj.GetComponent<UIText>();
                UIText ui = (UIText)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f, 1f, 1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }
}
