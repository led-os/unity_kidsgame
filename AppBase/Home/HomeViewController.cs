using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeViewController : UIViewController
{

    UIHomeBase uiHomePrefab;
    UIHomeBase uiHome;


    static private HomeViewController _main = null;
    public static HomeViewController main
    {
        get
        {
            if (_main == null)
            {
                _main = new HomeViewController();
                _main.Init();
            }
            return _main;
        }
    }

    void Init()
    {
        string strPrefab = "App/Prefab/Home/" + GetPrefabName();
        string strPrefabDefault = "Common/Prefab/Home/UIHomeDefault";
        GameObject obj = PrefabCache.main.Load(strPrefab);
        if (obj == null)
        {
            obj = PrefabCache.main.Load(strPrefabDefault);
        }

        uiHomePrefab = obj.GetComponent<UIHomeBase>();
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        CreateUI();
    }
    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
    }
    public override void LayOutView()
    {
        base.LayOutView();
        Debug.Log("HomeViewController LayOutView ");
        if (uiHome != null)
        {
            uiHome.LayOut();
        }
    }

    public void CreateUI()
    {
        if (this.naviController != null)
        {
            this.naviController.HideNavibar(true);
        }
        uiHome = (UIHomeBase)GameObject.Instantiate(uiHomePrefab);
        uiHome.SetController(this);
        ViewControllerManager.ClonePrefabRectTransform(uiHomePrefab.gameObject, uiHome.gameObject);
        uiHome.Init();
    }


    string GetPrefabName()
    {
        //Resources.Load 文件可以不区分大小写字母
        name = "UIHome" + Common.appType;
        return name;
    }

}
