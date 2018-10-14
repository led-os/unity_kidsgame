using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconViewController : UIViewController
{
    public UIShotBase uiPrefab;
    public UIShotBase ui;
    public ShotDeviceInfo deviceInfo;
    static private IconViewController _main = null;
    public static IconViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new IconViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "App/Prefab/ScreenShot/UIIconController";
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
