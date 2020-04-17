using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeViewControllerDict : PopViewController
{

    UIHome uiPrefab;
    UIHome ui;


    static private HomeViewControllerDict _main = null;
    public static HomeViewControllerDict main
    {
        get
        {
            if (_main == null)
            {
                _main = new HomeViewControllerDict();
                _main.Init();
            }
            return _main;
        }
    }
    void Init()
    {
        GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/Home/UIHome");
        uiPrefab = obj.GetComponent<UIHome>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        ui = (UIHome)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);

        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
