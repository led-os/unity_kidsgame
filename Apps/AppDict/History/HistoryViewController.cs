using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoryViewController : PopViewController
{

    UIHistory uiPrefab;
    UIHistory ui;

    static private HistoryViewController _main = null;
    public static HistoryViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HistoryViewController();
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
            GameObject obj = PrefabCache.main.Load("App/Prefab/History/UIHistory");
            uiPrefab = obj.GetComponent<UIHistory>();
        }
    }

    public void CreateUI()
    {
         if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIHistory)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
