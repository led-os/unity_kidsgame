using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdOfferWallViewController : UIViewController
{

    UIAdOfferWallController uiPrefab;
    UIAdOfferWallController ui;

    static private AdOfferWallViewController _main = null;
    public static AdOfferWallViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new AdOfferWallViewController();
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/AdOfferWall/UIAdOfferWallController");
            uiPrefab = obj.GetComponent<UIAdOfferWallController>();
        }
    }

    public void CreateUI()
    {
        ui = (UIAdOfferWallController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
