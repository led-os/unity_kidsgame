using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdVideoViewController : UIViewController
{

    UIAdVideoController uiPrefab;
    UIAdVideoController ui;

    static private AdVideoViewController _main = null;
    public static AdVideoViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new AdVideoViewController();
                _main.Init();
            }
            return _main;
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    void Init()
    {
        {
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/AdVideo/UIAdVideoController");
            uiPrefab = obj.GetComponent<UIAdVideoController>();
        }
    }

    public void CreateUI()
    {
        ui = (UIAdVideoController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
