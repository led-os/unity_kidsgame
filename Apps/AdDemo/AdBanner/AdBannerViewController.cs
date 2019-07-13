using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdBannerViewController : UIViewController
{

    UIAdBannerController uiPrefab;
    UIAdBannerController ui;

    static private AdBannerViewController _main = null;
    public static AdBannerViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new AdBannerViewController();
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/AdBanner/UIAdBannerController");
            uiPrefab = obj.GetComponent<UIAdBannerController>();
        }
    }

    public void CreateUI()
    {
        ui = (UIAdBannerController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
