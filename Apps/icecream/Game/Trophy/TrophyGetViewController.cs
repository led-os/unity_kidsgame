using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyGetViewController : PopViewController
{
    public UITrophyGet uiPrefab;
    public UITrophyGet ui;
    static private TrophyGetViewController _main = null;
    public static TrophyGetViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new TrophyGetViewController();
                _main.Init();
            }
            return _main;
        }
    }

    // public OnUICommentDidClickDelegate callBackClick { get; set; }

    void Init()
    {
        string strPrefab = "AppCommon/Prefab/Trophy/UITrophyGet";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UITrophyGet>();
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
        ui = (UITrophyGet)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);

    }


}
