using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeViewController : UIViewController
{
    public const int RUN_COUNT_SHOW_AD = 2;
    UIHomeBase uiHomePrefab;
    UIHomeBase uiHome;

    public static bool isAdVideoHasFinish = false;
    public static int runCount = 0;

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
        Debug.Log("HomeViewCon)troller ViewDidLoad");

        if ((!isAdVideoHasFinish) && (runCount >= RUN_COUNT_SHOW_AD))
        {
            //至少在home界面显示一次视频广告
            AdKitCommon.main.callbacAdVideokFinish = OnAdKitAdVideoFinish;
            if (uiHome != null)
            {
                uiHome.OnClickBtnAdVideo();
            }
        }
        runCount++;
    }
    public override void ViewDidUnLoad()
    {
        base.ViewDidUnLoad();
        Debug.Log("HomeViewController ViewDidUnLoad");
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
        UIViewController.ClonePrefabRectTransform(uiHomePrefab.gameObject, uiHome.gameObject);
        uiHome.Init();
    }


    string GetPrefabName()
    {
        //Resources.Load 文件可以不区分大小写字母
        name = "UIHome" + Common.appType;
        return name;
    }

    public void OnAdKitAdVideoFinish(AdKitCommon.AdType type, AdKitCommon.AdStatus status, string str)
    {
        //if (type == AdKitCommon.AdType.VIDEO)
        {
            if (status == AdKitCommon.AdStatus.SUCCESFULL)
            {
                isAdVideoHasFinish = true;
            }
        }
    }
}
