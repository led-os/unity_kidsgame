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

    [MenuItem(KEY_MENU_GameObject_UI + "/UITypeButton", false, 4)]
    static void CreateTypeButton()
    {
        // GameObject obj = new GameObject("UITypeButton");
        // UITypeButton ui = obj.AddComponent<UITypeButton>();

        var selectedObj = Selection.activeObject as GameObject;
        if (selectedObj != null)
        {
            GameObject obj = PrefabCache.main.Load("Common/Prefab/UIKit/UIButton/UITypeButton");
            if (obj != null)
            {
                UITypeButton uiPrefab = obj.GetComponent<UITypeButton>();
                UITypeButton ui = (UITypeButton)GameObject.Instantiate(uiPrefab);
                ui.transform.SetParent(selectedObj.transform);
                Selection.activeGameObject = ui.gameObject;
                ui.transform.localScale = new Vector3(1f,1f,1f);
                RectTransform rctran = ui.GetComponent<RectTransform>();
                rctran.anchoredPosition = Vector2.zero;
            }
        }

    }
}
