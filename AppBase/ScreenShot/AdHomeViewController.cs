using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdHomeViewController : UIViewController
{ 
    public UIShotBase uiPrefab;
    public UIShotBase ui;
    static private AdHomeViewController _main = null;
    public static AdHomeViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new AdHomeViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "App/Prefab/ScreenShot/UIAdHomeController";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        uiPrefab = obj.GetComponent<UIShotBase>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();  
    }  
      public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("AdHomeViewController LayOutView ");
        if (ui != null)
        {
            ui.LayOut();
        }
    }
    public void CreateUI()
    {
        ui = (UIShotBase)GameObject.Instantiate(uiPrefab);
        ui.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiPrefab.gameObject, ui.gameObject);
    }
}
