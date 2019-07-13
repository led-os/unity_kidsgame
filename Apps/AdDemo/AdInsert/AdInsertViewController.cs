using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdInsertViewController : UIViewController
{

    UIAdInsertController uiPrefab;
    UIAdInsertController ui;

    static private AdInsertViewController _main = null;
    public static AdInsertViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new AdInsertViewController();
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
            GameObject obj = PrefabCache.main.Load("AppCommon/Prefab/AdInsert/UIAdInsertController");
            uiPrefab = obj.GetComponent<UIAdInsertController>();
        }
    }

    public void CreateUI()
    {
        ui = (UIAdInsertController)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        UIViewController.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }

}
