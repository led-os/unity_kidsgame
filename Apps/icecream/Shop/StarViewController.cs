using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarViewController : PopViewController
{
    public const int TYPE_STAR_BUY = 0;
    public const int TYPE_STAR_GET = 1;
    public const int TYPE_STAR_NOTENOUGHT = 2;
    public const int TYPE_STAR_RESTORE = 3;//恢复购买
    public int starType;
    UIView uiPrefab;
    UIView ui;


    static private StarViewController _main = null;
    public static StarViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new StarViewController();
                //_main.Init();
            }
            return _main;
        }
    }

    public void SetType(int ty)
    {
        starType = ty;
        string strPrefab = "";//  
        if (uiPrefab != null)
        {
            // GameObject.DestroyImmediate(uiPrefab.gameObject, true);
        }
        switch (starType)
        {
            case TYPE_STAR_BUY:
                strPrefab = "AppCommon/Prefab/Shop/UIStarBuy";
                break;
            case TYPE_STAR_GET:
                strPrefab = "AppCommon/Prefab/Shop/UIStarGet";
                break;
            case TYPE_STAR_RESTORE:
                strPrefab = "AppCommon/Prefab/Shop/UIStarBuy";
                break;
            case TYPE_STAR_NOTENOUGHT:
                strPrefab = "AppCommon/Prefab/Shop/UIStarNotEnough";
                break;
        }
        // PrefabCache.main.DeleteItem
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIView>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public void CreateUI()
    {
        ui = (UIView)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
