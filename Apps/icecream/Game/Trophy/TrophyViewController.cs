using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyViewController : PopViewController
{
    public UITrophyList uiPrefab;
    public UITrophyList ui;

    UITrophyGet uiTrophyGetPrefab;
    public UITrophyGet uiTrophyGet;

    static private TrophyViewController _main = null;
    public static TrophyViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new TrophyViewController();
                _main.Init();
            }
            return _main;
        }
    }

    // public OnUICommentDidClickDelegate callBackClick { get; set; }

    void Init()
    {
        string strPrefab = "AppCommon/Prefab/Trophy/UITrophyList";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UITrophyList>();

        {
            obj = PrefabCache.main.Load("AppCommon/Prefab/Trophy/UITrophyGet");
            uiTrophyGetPrefab = obj.GetComponent<UITrophyGet>();
        }
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public override void LayOutView()
    {
        base.LayOutView();

    }
    public void CreateUI()
    {
        ui = (UITrophyList)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);

        {
            uiTrophyGet = (UITrophyGet)GameObject.Instantiate(uiTrophyGetPrefab);
            ui.uiTrophyGet = uiTrophyGet;
            uiTrophyGet.uiTrophyList = ui;
            uiTrophyGet.SetController(this);

           UIViewController.ClonePrefabRectTransform(uiTrophyGetPrefab.gameObject, uiTrophyGet.gameObject);
        }

    }


}
